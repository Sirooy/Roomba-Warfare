using System;
using System.Collections;
using System.Collections.Generic;

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

    //TO DO

    public PlayerCollection()
    {
        players = new Dictionary<int, Player>();
    }

    //Adds a player to the list
    public string Add(int id,Player player)
    {
        players.Add(id, player);

        return (int)ServerMessage.NewPlayer + " " +
            player.ToString() + ":";
    }

    public IEnumerator<Player> GetEnumerator()
    {
        foreach(KeyValuePair<int,Player> player in players)
        {
            yield return player.Value; 
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    //TO DO
}

