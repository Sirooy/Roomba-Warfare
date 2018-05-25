
public class Bullet : DynamicEntity
{
    public static readonly byte SPRITE_WIDTH = 32;
    public static readonly byte SPRITE_HEIGHT = 32;

    public Bullet(PlayerTeam type)
    {
        spriteY = 128;
        switch (type)
        {
            case PlayerTeam.Red: spriteX = 0; break;
            case PlayerTeam.Blue: spriteX = 32; break;
        }
    }

    public void Render(Camera camera)
    {
        RenderDynamic(SpriteSheet.Texture, camera.X, camera.Y, 
            SPRITE_WIDTH, SPRITE_HEIGHT);
    }
}
