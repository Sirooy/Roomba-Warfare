using System.Collections.Generic;

//
public class PlayerCollection
{
    private Dictionary<int, ServerPlayer> players;

    public PlayerCollection()
    {
        players = new Dictionary<int, ServerPlayer>();
    }

    public void Add(int id, string[] parts)
    {

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

    public void ChangeTeam(int id, string[] parts)
    {
        //TO DO
    }

    //Renders all the players
    public void Render(float camX, float camY)
    {
        foreach (KeyValuePair<int, ServerPlayer> player in players)
        {
            player.Value.Render(camX, camY);
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
