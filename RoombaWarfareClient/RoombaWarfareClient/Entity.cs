using System;

public abstract class Entity
{
    public float PosX { get; set; }
    public float PosY { get; set; }

    protected ushort spriteX;
    protected ushort spriteY;

    public void SetPos(float posX, float posY)
    {
        PosX = posX;
        PosY = posY;
    }

    //Renders the entity based on the camera position
    public void RenderDynamic(IntPtr texture,float camX, float camY
        , ushort spriteWidth, ushort spriteHeight)
    {
        Hardware.RenderDynamic(texture, camX, camY, PosX, PosY
            , spriteWidth, spriteHeight, spriteX, spriteY);
    }

    //Renders the entity on a static coords
    public void RenderStatic(IntPtr texture,ushort spriteWidth, ushort spriteHeight)
    {
        Hardware.RenderStatic(texture, PosX, PosY
            , spriteWidth, spriteHeight, spriteX, spriteY);
    }
}

