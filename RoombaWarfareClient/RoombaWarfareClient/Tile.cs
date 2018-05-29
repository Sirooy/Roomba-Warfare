
public enum TileType
{
    Ground1,
    Ground2,
    Ground3,
    Ground4,
    Ground5,
    Hitbox1,
    Hitbox2,
    Hitbox3,
    Hitbox4,
    Hitbox5,
    Hitbox6,
    Hitbox7,
    Hitbox8,
    Hitbox9,
    Hitbox10,
    Hitbox11,
    Hitbox12,
    Hitbox13,
    Hitbox14,
    Hitbox15,
    Hitbox16,
    Hitbox17,
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
            case TileType.Ground5: spriteX = 384; spriteY = 0; break;
            case TileType.Hitbox1: spriteX = 448; spriteY = 0; break;
            case TileType.Hitbox2: spriteX = 512; spriteY = 0; break;
            case TileType.Hitbox3: spriteX = 576; spriteY = 0; break;
            case TileType.Hitbox4: spriteX = 640; spriteY = 0; break;
            case TileType.Hitbox5: spriteX = 704; spriteY = 0; break;
            case TileType.Hitbox6: spriteX = 768; spriteY = 0; break;
            case TileType.Hitbox7: spriteX = 0; spriteY = 64; break;
            case TileType.Hitbox8: spriteX = 64; spriteY = 64; break;
            case TileType.Hitbox9: spriteX = 128; spriteY = 64; break;
            case TileType.Hitbox10: spriteX = 192; spriteY = 64; break;
            case TileType.Hitbox11: spriteX = 256; spriteY = 64; break;
            case TileType.Hitbox12: spriteX = 320; spriteY = 64; break;
            case TileType.Hitbox13: spriteX = 384; spriteY = 64; break;
            case TileType.Hitbox14: spriteX = 448; spriteY = 64; break;
            case TileType.Hitbox15: spriteX = 512; spriteY = 64; break;
            case TileType.Hitbox16: spriteX = 576; spriteY = 64; break;
            case TileType.Hitbox17: spriteX = 640; spriteY = 64; break;
            case TileType.BlueSpawnPoint: spriteX = 320; spriteY = 0; break;
            case TileType.RedSpawnPoint: spriteX = 256; spriteY = 0; break;
            case TileType.MissingTexture: spriteX = 704; spriteY = 64; break;
        }
    }

    public void Render(float camX, float camY)
    {
        RenderDynamic(SpriteSheet.Texture, camX, camY, 
            SPRITE_WIDTH, SPRITE_HEIGHT);
    }
}
