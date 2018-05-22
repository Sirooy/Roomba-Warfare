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
        players.Remove(id);
    }

    public void Kill(int id)
    {
        players[id].Kill();
    }

    //Change this function later to interpolate the players positions
    public void SetPosition(int id, string[] parts)
    {
        players[id].SetPos(parts);
    }

    //Sets the angle of a player
    public void SetAngle(int id, string[] parts)
    {
        players[id].Angle = float.Parse(parts[2]);
    }

    //Sets the team of a player
    public void SetTeam(int id, string[] parts)
    {
        players[id].SetTeam(parts);
    }

    public void Respawn(int id, string[] parts)
    {
        players[id].Respawn(parts);
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
