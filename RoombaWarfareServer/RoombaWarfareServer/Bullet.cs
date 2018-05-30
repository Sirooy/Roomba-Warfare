using System.Collections.Generic;

public class Bullet : Entity
{
    public static readonly byte RADIUS = 16;
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

    //Check if the bullet collides with the walls or get out of the map bound
    public bool CollidesWith(Hitbox[] hitboxes)
    {
        foreach(Hitbox hitbox in hitboxes)
        {
            if (PosX + WIDTH > hitbox.x && PosX < hitbox.x + Hitbox.WIDTH
                && PosY + HEIGHT > hitbox.y && PosY < hitbox.y + Hitbox.HEIGHT)
                return true;
        }

        if (PosX + WIDTH < 0 || PosX > Map.Width
            || PosY + HEIGHT < 0 || PosY > Map.Height)
            return true;

        return false;
    }

    //Checks if the bullet collides with a player
    public int CollidesWith(PlayerCollection players)
    {
        
        float bulletCenterX = PosX + Bullet.WIDTH / 2;
        float bulletCenterY = PosY + Bullet.HEIGHT / 2;

        foreach(Player player in players)
        {
            if(player.IsAlive && team != player.Team)
            {
                float playerCenterX = player.PosX + Player.WIDTH / 2;
                float playerCenterY = player.PosY + Player.HEIGHT / 2;

                float distanceX = (bulletCenterX - playerCenterX)
                    * (bulletCenterX - playerCenterX);
                float distanceY = (bulletCenterY - playerCenterY)
                    * (bulletCenterY - playerCenterY);
                float distance = distanceX + distanceY;

                if (distance <= (RADIUS * RADIUS) + 
                    (Player.RADIUS * Player.RADIUS))
                    return player.ID;
            }
        }

        return -1;
    }

    //Updates the bullet position
    public void Update(float deltaTime)
    {
        PosX += (deltaTime * speed * dirX);
        PosY += (deltaTime * speed * dirY);
    }

    public override string ToString()
    {
        return base.ToString() + " " + dirX.ToString("0.##") + " " +
            dirY.ToString("0.##") + " " + speed + " " + (int)team;
    }
}