using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollection : IEnumerable<Player>
{
    public int Count { get { return players.Count; } }
    public int RedPlayersCount { get; }

    private Dictionary<int,Player> players;

    public PlayerCollection()
    {
        players = new Dictionary<int, Player>();
    }

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
}

