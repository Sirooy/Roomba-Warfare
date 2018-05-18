using System.Collections.Generic;

//
public class PlayerCollection
{
    private Dictionary<int, ServerPlayer> players;

    public PlayerCollection()
    {
        players = new Dictionary<int, ServerPlayer>();
    }
}
