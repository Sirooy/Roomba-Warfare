using System.Collections.Generic;

public class PlayerCollection
{
    public int Count { get { return players.Count; } }

    private Dictionary<int,Player> players;

    public PlayerCollection()
    {
        players = new Dictionary<int, Player>();
    }
}

