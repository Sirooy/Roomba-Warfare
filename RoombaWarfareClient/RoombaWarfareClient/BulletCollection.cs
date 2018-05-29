using System.Collections.Generic;

public class BulletCollection
{
    private List<Bullet> bullets;

    public BulletCollection()
    {
        bullets = new List<Bullet>();
    }

    //Adds a bullet to the list
    public void Add(string[] parts)
    {
        float posX = float.Parse(parts[1]);
        float posY = float.Parse(parts[2]);
        float dirX = float.Parse(parts[3]);
        float dirY = float.Parse(parts[4]);
        float speed = float.Parse(parts[5]);
        PlayerTeam team = (PlayerTeam)int.Parse(parts[6]);

        bullets.Add(new Bullet(posX, posY, dirX, dirY, speed, team));
    }

    public void Update(float deltaTime)
    {
        foreach(Bullet bullet in bullets)
        {
            bullet.Update(deltaTime);
        }
    }

    //Renders the bullets
    public void Render(Camera camera)
    {
        foreach(Bullet bullet in bullets)
        {
            //Only render the ones that are in the camera bounds
            if(CollisionHandler.CollidesWith
                (camera,bullet,Bullet.SPRITE_WIDTH,Bullet.SPRITE_HEIGHT))
                bullet.Render(camera);
        }
    }
}
