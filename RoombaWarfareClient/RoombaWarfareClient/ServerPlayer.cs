
public class ServerPlayer : Player
{
    public sbyte MultiplierX { get; set; }
    public sbyte MultiplierY { get; set; }
    public bool Interpolation { get; set; }
    public float InterpolationPosX { get; set; }
    public float InterpolationPosY { get; set; }

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
        if (Interpolation)
        {
            if ((int)PosX != (int)InterpolationPosX
                || (int)PosY != (int)InterpolationPosY)
            {
                if ((int)PosX != (int)InterpolationPosX)
                    PosX += (MultiplierX * speed * deltaTime);
                if ((int)PosY != (int)InterpolationPosY)
                    PosY += (MultiplierY * speed * deltaTime);
            }
            else
            {
                Interpolation = false;
            }
        }
    }
}
