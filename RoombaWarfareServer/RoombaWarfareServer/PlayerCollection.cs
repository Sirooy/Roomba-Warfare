﻿using System;
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

