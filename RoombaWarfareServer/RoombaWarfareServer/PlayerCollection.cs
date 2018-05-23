using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCollection : IEnumerable<Player>
{
    public int Count { get { return players.Count; } }
    public int RedPlayersCount { get { return redPlayers; } }
    public int BluePlayersCount { get { return bluePlayers; } }
    public int RedAlivePlayersCount { get { return redAlivePlayers; } }
    public int BlueAlivePlayersCount { get { return blueAlivePlayers; } }

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
        }

        return (int)ServerMessage.NewPlayer + " " +
            player.ToString() + ":";
    }

    public string ChangeTeam(string[] commandParts)
    {
        int id = int.Parse(commandParts[1]);
        PlayerTeam team = (PlayerTeam)int.Parse(commandParts[2]);

        lock (lockPlayers)
        {
            players[id].Team = team;
        }

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
                           where player.Team == PlayerTeam.Red
                           && player.IsAlive
                           select player).Count();

        return (int)ServerMessage.SetPlayerTeam + " " + id +
            " " + (int)team + ":";
    }

    //Sends a message to all players
    public void Broadcast(string gameState)
    {
        lock (lockPlayers)
        {
            foreach(KeyValuePair<int,Player> player in players)
            {
                string onwStatus = player.Value.GetStatus();
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

