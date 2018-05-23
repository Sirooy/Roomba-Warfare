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

    public void Render(Camera camera)
    {
        foreach(Bullet bullet in bullets)
        {
            //TO DO Render only the ones that are inside of the camera bounds
            bullet.Render(camera);
        }
    }
}
