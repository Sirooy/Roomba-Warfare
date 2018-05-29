
public class Bullet : DynamicEntity
{
    public static readonly byte SPRITE_WIDTH = 32;
    public static readonly byte SPRITE_HEIGHT = 32;

    private float dirX;
    private float dirY;

    public Bullet(float posX,float posY,float dirX,float dirY,
        float speed,PlayerTeam team)
    {
        PosX = posX;
        PosY = posY;
        this.dirX = dirX;
        this.dirY = dirY;
        this.speed = speed;

        spriteY = 128;
        switch (team)
        {
            case PlayerTeam.Red: spriteX = 0; break;
            case PlayerTeam.Blue: spriteX = 32; break;
        }
    }

    //Updates the bullet position
    public override void Update(float deltaTime)
    {
        PosX += (deltaTime * speed * dirX);
        PosY += (deltaTime * speed * dirY);
    }

    public void Render(Camera camera)
    {
        RenderDynamic(SpriteSheet.Texture, camera.X, camera.Y, 
            SPRITE_WIDTH, SPRITE_HEIGHT);
    }
}
