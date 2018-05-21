using System.Collections.Generic;

//
public class PlayerCollection
{
    private Dictionary<int, ServerPlayer> players;

    public PlayerCollection()
    {
        players = new Dictionary<int, ServerPlayer>();
    }

    //Adds a new player
    public void Add(int id, string[] parts)
    {
        PlayerType type = (PlayerType)int.Parse(parts[2]);
        PlayerTeam team = (PlayerTeam)int.Parse(parts[3]);
        bool isAlive = bool.Parse(parts[4]);
        float posX = float.Parse(parts[4]);
        float posY = float.Parse(parts[5]);
        float angle = float.Parse(parts[6]);

        ServerPlayer newPlayer = 
            new ServerPlayer(id, type, team, isAlive, posX, posY, angle);
        players.Add(id, newPlayer);
    }

    public void Remove(int id)
    {
        //TO DO
    }

    public void SetPosition(int id, string[] parts)
    {
        //TO DO
    }

    public void SetAngle(int id, string[] parts)
    {
        players[id].Angle = float.Parse(parts[2]);
    }

    public void SetTeam(int id, string[] parts)
    {
        players[id].SetTeam((PlayerTeam)int.Parse(parts[2]));
    }

    public void Respawn(int id, string[] parts)
    {
        float posX = float.Parse(parts[2]);
        float posY = float.Parse(parts[3]);
        players[id].IsAlive = true;
        players[id].SetPos(posX, posY);
    }

    //Renders all the players
    public void Render(Camera camera)
    {
        foreach (KeyValuePair<int, ServerPlayer> player in players)
        {
            player.Value.Render(camera);
        }
    }

    //Update all the players position (Interpolation)
    public void Update(float deltaTime)
    {
        foreach (KeyValuePair<int, ServerPlayer> player in players)
        {
            player.Value.Update(deltaTime);
        }
    }
}
