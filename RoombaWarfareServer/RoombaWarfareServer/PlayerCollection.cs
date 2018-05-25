using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCollection : IEnumerable<Player>
{
    public event Action<int> OnPlayerDisconnectEvent;

    public int Count
        { get { lock(lockPlayers) return players.Count; } }
    public int RedPlayersCount
        { get { lock (lockPlayers) return redPlayers; } }
    public int BluePlayersCount
        { get { lock (lockPlayers) return bluePlayers; } }
    public int RedAlivePlayersCount
        { get { lock (lockPlayers) return redAlivePlayers; } }
    public int BlueAlivePlayersCount
        { get { lock (lockPlayers) return blueAlivePlayers; } }

    private int redPlayers;
    private int bluePlayers;
    private int redAlivePlayers;
    private int blueAlivePlayers;

    private Dictionary<int,Player> players;

    private object lockPlayers = new object();

    public PlayerCollection()
    {
        players = new Dictionary<int, Player>();
    }

    //Adds a player to the list
    public string Add(int id,Player player)
    {
        lock (lockPlayers)
        {
            players.Add(id, player);
            players[id].OnPlayerDisconnectEvent += DisconnectPlayer;
        }

        return (int)ServerMessage.NewPlayer + " " +
            player.ToString() + ":";
    }

    //Removes a player
    public string Remove(int id)
    {
        lock (lockPlayers)
        {
            players.Remove(id);
        }

        return (int)ServerMessage.RemovePlayer + " " +
            id + ":";
    }

    //Disconnects a player
    public void DisconnectPlayer(int id)
    {
        lock (lockPlayers)
        {
            players[id].EndConnection();
        }
        
        OnPlayerDisconnectEvent(id);
    }

    //Sets the position of a player
    public void SetPosition(string[] commandParts)
    {
        int id = int.Parse(commandParts[1]);
        float posX = float.Parse(commandParts[2]);
        float posY = float.Parse(commandParts[3]);
        uint commandNum = uint.Parse(commandParts[4]);

        lock (lockPlayers)
        {
            players[id].SetPos(posX, posY);
            players[id].SetMovementStatus((int)ServerMessage.SetPlayerPosition
                + " " + id + " " + posX.ToString("0.#") + " "
                + posY.ToString("0.#") + ":", PlayerState.Position);
            players[id].SetLastProcessedCommand(commandNum); 
        }
    }

    //Sets the angle of a player
    public void SetAngle(string[] commandParts)
    {
        int id = int.Parse(commandParts[1]);
        float angle = float.Parse(commandParts[2]);

        lock (lockPlayers)
        {
            players[id].Angle = angle;
            players[id].SetMovementStatus((int)ServerMessage.SetPlayerAngle 
                + " " + id + " " + angle + ":", PlayerState.Angle);
        }
    }

    public string ChangeTeam(string[] commandParts)
    {
        int id = int.Parse(commandParts[1]);
        PlayerTeam team = (PlayerTeam)int.Parse(commandParts[2]);

        lock (lockPlayers)
        {
            players[id].Team = team;
        }

        //Recalculate the players
        CalculatePlayers();

        return (int)ServerMessage.SetPlayerTeam + " " + id +
            " " + (int)team + ":";
    }

    //Calculates the players that are in every team
    public void CalculatePlayers()
    {
        lock (lockPlayers)
        {
            redPlayers = (from player in players.Values
                          where player.Team == PlayerTeam.Red
                          select player).Count();

            bluePlayers = (from player in players.Values
                           where player.Team == PlayerTeam.Blue
                           select player).Count();

            redAlivePlayers = (from player in players.Values
                               where player.Team == PlayerTeam.Red
                               && player.IsAlive
                               select player).Count();

            blueAlivePlayers = (from player in players.Values
                                where player.Team == PlayerTeam.Blue
                                && player.IsAlive
                                select player).Count();
        }
    }

    //Respawns all the players in the map spawnpoints
    public string GlobalRespawn(Map map)
    {
        string ret = "";
        Random rnd = new Random();

        lock (lockPlayers)
        {
            foreach (KeyValuePair<int, Player> player in players)
            {
                if (player.Value.Team != PlayerTeam.Spectator)
                {
                    int spawnPointNum = 0;
                    SpawnPoint spawnPoint;
                    //Gets a random spawnpoint depending on the ream
                    if (player.Value.Team == PlayerTeam.Red)
                    {
                        spawnPointNum = rnd.Next(0, map.redSpawnPoints.Length);
                        spawnPoint = map.redSpawnPoints[spawnPointNum];
                    }
                    else
                    {
                        spawnPointNum = rnd.Next(0, map.blueSpawnPoints.Length);
                        spawnPoint = map.blueSpawnPoints[spawnPointNum];
                    }

                    //Respawns the player
                    player.Value.Respawn(spawnPoint.X, spawnPoint.Y);
                    ret += (int)ServerMessage.Respawn + " " + player.Value.ID
                        + " " + spawnPoint.X + " " + spawnPoint.Y + ":";
                }
            }
        }

        redAlivePlayers = redPlayers;
        blueAlivePlayers = bluePlayers;

        return ret;
    }

    //Sends a message to all players
    public void Broadcast(string gameState)
    {
        lock (lockPlayers)
        {
            foreach(KeyValuePair<int,Player> player in players)
            {
                string onwStatus = player.Value.GetOwnStatus();
                player.Value.Send(onwStatus + gameState);
            }
        }
    }

    public IEnumerator<Player> GetEnumerator()
    {
        lock (lockPlayers)
        {
            foreach (KeyValuePair<int, Player> player in players)
            {
                yield return player.Value;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    //TO DO
}

