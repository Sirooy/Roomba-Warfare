
public enum StatusBarType { Health, Ammo }
//This class handles the status of the player (Health, ammo...)
public class StatusBar : StaticEntity
{
    public static readonly byte SPRITE_HEIGHT = 16;

    private ushort spriteWidth;
    private ushort totalWidth;

    public StatusBar(StatusBarType type)
    {
        spriteX = 0;
        switch (type)
        {
            case StatusBarType.Health:
                spriteY = 428;
                totalWidth = 640;
                break;
            case StatusBarType.Ammo:
                spriteY = 444;
                totalWidth = 384;
                break;
        }
    }

    //Recalculates the size of the bar
    public void Resize(float amount,float maxSize)
    {
        double widthPercentage = amount / maxSize;
        spriteWidth = (ushort)(widthPercentage * totalWidth);
    }

    //Renders the bar
    public void Render()
    {
        RenderStatic(SpriteSheet.Texture, spriteWidth, SPRITE_HEIGHT);
    }
}

