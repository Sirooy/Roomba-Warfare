
public class ServerPlayer : Player
{
    public ServerPlayer(int id, PlayerType type, PlayerTeam team,
            bool isAlive, float posX, float posY, float angle) : base(type)
    {
        ID = id;
        Type = type;
        Team = team;
        IsAlive = isAlive;
        PosX = posX;
        PosY = posY;
        Angle = angle;

        if (Team == PlayerTeam.Red)
            spriteY = 0;
        else if (Team == PlayerTeam.Blue)
            spriteY = 64;
    }

    public override void Update(float deltaTime)
    {
        //TO DO
    }
}
