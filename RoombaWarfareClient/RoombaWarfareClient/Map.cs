using System.Collections.Generic;

public struct Hitbox
{
    public static readonly byte WIDTH = 64;
    public static readonly byte HEIGHT = 64;

    public uint x;
    public uint y;

    public Hitbox(uint x, uint y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Map
{
    public static uint Width;
    public static uint Height;

    public Hitbox[] Hitboxes { get; set; }
    public Tile[] Tiles { get; set; }

    public Map()
    {
        Width = 0;
        Height = 0;
    }

    //Creates the map based on a seed.
    public void Create(string seed)
    {
        List<Hitbox> mapHitboxes = new List<Hitbox>();
        List<Tile> mapTiles = new List<Tile>();

        //Get the size
        string[] mapLines = seed.Split('-');
        string[] mapSize = mapLines[0].Split();
        Width = uint.Parse(mapSize[0]);
        Height = uint.Parse(mapSize[1]);

        for(int posY = 1; posY < mapLines.Length; posY++)
        {
            string[] mapParts = mapLines[posY].Split();

            for(int posX= 0; posX < mapParts.Length; posX++)
            {
                bool isHitbox = false;
                TileType type;

                switch (mapParts[posX])
                {
                    case "G1": type = TileType.Ground1; break;
                    case "G2": type = TileType.Ground2; break;
                    case "G3": type = TileType.Ground3; break;
                    case "G4": type = TileType.Ground4; break;

                    case "H1": type = TileType.Hitbox1;
                        isHitbox = true;
                        break;
                    case "H2": type = TileType.Hitbox2;
                        isHitbox = true;
                        break;
                    case "H3": type = TileType.Hitbox3;
                        isHitbox = true;
                        break;
                    case "H4": type = TileType.Hitbox4;
                        isHitbox = true;
                        break;

                    case "BS": type = TileType.BlueSpawnPoint; break;
                    case "RS": type = TileType.RedSpawnPoint; break;

                    default: type = TileType.MissingTexture; break;
                }

                mapTiles.Add(new Tile
                    (posX * Tile.SPRITE_WIDTH,
                    (posY - 1) * Tile.SPRITE_HEIGHT,
                    type));

                if (isHitbox)
                {
                    mapHitboxes.Add(new Hitbox(
                        (uint)posX * Tile.SPRITE_WIDTH,
                        (uint)(posY - 1) * Tile.SPRITE_HEIGHT));
                }
            }
        }

        Tiles = mapTiles.ToArray();
        Hitboxes = mapHitboxes.ToArray();
    }

    //Renders all the tiles.
    public void Render(Camera camera)
    {
        foreach (Tile tile in Tiles)
        {
            //Only render the ones that the player can see
            if (tile.PosX + Tile.SPRITE_WIDTH > camera.X
                && tile.PosX < camera.X + camera.Width
                && tile.PosY + Tile.SPRITE_HEIGHT > camera.Y
                && tile.PosY < camera.Y + camera.Height)
                tile.Render(camera.X, camera.Y);
        }
    }
}
