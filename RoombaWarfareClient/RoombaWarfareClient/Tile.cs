
public enum TileType
{
    Ground1,
    Ground2,
    Ground3,
    Ground4,
    Hitbox1,
    Hitbox2,
    Hitbox3,
    Hitbox4,
    BlueSpawnPoint,
    RedSpawnPoint,
    MissingTexture
}

public class Tile : StaticEntity
{
    public static readonly byte SPRITE_WIDTH = 64;
    public static readonly byte SPRITE_HEIGHT = 64;
    //TO DO

    public Tile(int posX,int posY,TileType type)
    {
        PosX = posX;
        PosY = posY;

        switch (type)
        {
            case TileType.Ground1: spriteX = 0; spriteY = 0; break;
            case TileType.Ground2: spriteX = 64; spriteY = 0; break;
            case TileType.Ground3: spriteX = 128; spriteY = 0; break;
            case TileType.Ground4: spriteX = 192; spriteY = 0; break;
            case TileType.Hitbox1: spriteX = 0; spriteY = 64; break;
            case TileType.Hitbox2: spriteX = 64; spriteY = 64; break;
            case TileType.Hitbox3: spriteX = 128; spriteY = 64; break;
            case TileType.Hitbox4: spriteX = 192; spriteY = 64; break;
            case TileType.BlueSpawnPoint: spriteX = 192; spriteY = 64; break;
            case TileType.RedSpawnPoint: spriteX = 192; spriteY = 64; break;
            case TileType.MissingTexture: spriteX = 256; spriteY = 64; break;
        }
    }

    public void Render(float camX, float camY)
    {
        RenderDynamic(SpriteSheet.Texture, camX, camY, 
            SPRITE_WIDTH, SPRITE_HEIGHT);
    }
}
