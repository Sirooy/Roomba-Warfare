using System.Collections.Generic;

public class Bullet : Entity
{
    public static readonly byte WIDTH = 32;
    public static readonly byte HEIGHT = 32;

    public ushort Damage { get { return damage; } }

    private float dirX;
    private float dirY;
    private float speed;
    private ushort damage;
    private PlayerTeam team;

    public Bullet(float dirX,float dirY,PlayerTeam team, PlayerType type
        ,float playerPosX, float playerPosY)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.team = team;

        switch (type)
        {
            case PlayerType.Assault: speed = 8; damage = 25; break;
            case PlayerType.Commander: speed = 12; damage = 100; break;
            case PlayerType.Rusher: speed = 8; damage = 15; break;
            case PlayerType.Tank: speed = 9; damage = 45; break;
        }

        PosX = playerPosX + (Player.WIDTH / 2 - WIDTH / 2);
        PosY = playerPosY + (Player.HEIGHT / 2 - HEIGHT / 2);

        //Move the bullet near the cannon of the player
        PosX += (dirX * Player.WIDTH / 2);
        PosY += (dirY * Player.HEIGHT / 2); 
    }

    public void Update(double deltaTime)
    {
        //TO DO
    }

    public override string ToString()
    {
        return base.ToString() + " " + dirX.ToString("0.##") + " " +
            dirY.ToString("0.##") + " " + speed + " " + (int)team;
    }
}