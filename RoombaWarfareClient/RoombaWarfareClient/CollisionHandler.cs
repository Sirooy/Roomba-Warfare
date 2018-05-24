using System;

//This class handles all the collisions
public static class CollisionHandler
{
    //Camera with and entity
    public static bool CollidesWith(Camera camera, Entity entity, 
        short entityWidth, short entityHeight)
    {
        if (entity.PosX + entityWidth > camera.X &&
            entity.PosX < camera.X + camera.Width &&
            entity.PosY + entityHeight > camera.Y &&
            entity.PosY < camera.Y + camera.Height)
            return true;

        return false;
    }

    //Circle with a hitbox
    public static bool CollidesWith(int circleX, int circleY, int radius,
        Hitbox hitbox)
    {
        int closestX = Math.Max((int)hitbox.x, 
            Math.Min((int)hitbox.x + Hitbox.WIDTH, circleX));
        int closestY = Math.Max((int)hitbox.y,
            Math.Min((int)hitbox.y + Hitbox.HEIGHT, circleY));

        int distanceX = (circleX - closestX) * (circleX - closestX);
        int distanceY = (circleY - closestY) * (circleY - closestY);

        return (distanceX + distanceY) < (radius * radius);
    }
}

