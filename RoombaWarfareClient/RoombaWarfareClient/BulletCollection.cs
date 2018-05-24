using System.Collections.Generic;

public class BulletCollection
{
    private List<Bullet> bullets;

    public BulletCollection()
    {
        bullets = new List<Bullet>();
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
