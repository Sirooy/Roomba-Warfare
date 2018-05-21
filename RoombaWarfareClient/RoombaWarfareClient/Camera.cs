
public class Camera
{
    public int X { get; set; }
    public int Y { get; set; }

    public void SetPos(Entity entity
        ,ushort entityWidth,ushort entityHeight)
    {
        X = (int)(entity.PosX + entityWidth / 2) - Hardware.ScreenWidth / 2;
        Y = (int)(entity.PosY + entityHeight / 2) - Hardware.ScreenHeight / 2;

        if (X < 0)
            X = 0;
        if (Y < 0)
            Y = 0;
        if (X > Map.Width - Hardware.ScreenWidth)
            X = (int)Map.Width - Hardware.ScreenWidth;
        if( Y > Map.Height - Hardware.ScreenHeight)
            X = (int)Map.Height - Hardware.ScreenHeight;
    }
}

