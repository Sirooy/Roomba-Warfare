using System;

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
        /*if (Interpolation)
        {
            if (Math.Floor(PosX) != Math.Floor(InterpolationPosX)
                || Math.Floor(PosY) != Math.Floor(InterpolationPosY))
            {
                if (Math.Floor(PosX) != Math.Floor(InterpolationPosX))
                    PosX += (MultiplierX * speed * deltaTime);
                if (Math.Floor(PosY) != Math.Floor(InterpolationPosY))
                    PosY += (MultiplierY * speed * deltaTime);
            }
            else
            {
                Interpolation = false;
            }
        }*/
        if (Interpolation)
        {
            if ((Math.Round(PosX) != Math.Round(InterpolationPosX))
                || (Math.Round(PosY) != Math.Round(InterpolationPosY)))
            {
                if (Math.Round(PosX) < Math.Round(InterpolationPosX))
                {
                    PosX += speed * deltaTime;
                    if (PosX > InterpolationPosX)
                        PosX = InterpolationPosX;
                }
                else if (Math.Round(PosX) > Math.Round(InterpolationPosX))
                {
                    PosX -= speed * deltaTime;
                    if (PosX < InterpolationPosX)
                        PosX = InterpolationPosX;
                }

                if (Math.Round(PosY) < Math.Round(InterpolationPosY))
                {
                    PosY += speed * deltaTime;
                    if (PosY > InterpolationPosY)
                        PosY = InterpolationPosY;
                }
                else if (Math.Round(PosY) > Math.Round(InterpolationPosY))
                {
                    PosY -= speed * deltaTime;
                    if (PosY < InterpolationPosY)
                        PosY = InterpolationPosY;
                }
            }
            else
            {
                Interpolation = false;
            }
        }
    }
}
