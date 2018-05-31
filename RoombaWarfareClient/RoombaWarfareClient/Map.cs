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
                TileType type;

                switch (mapParts[posX])
                {
                    case "G01": type = TileType.Ground1; break;
                    case "G02": type = TileType.Ground2; break;
                    case "G03": type = TileType.Ground3; break;
                    case "G04": type = TileType.Ground4; break;
                    case "G05": type = TileType.Ground5; break;

                    case "H01": type = TileType.Hitbox1; break;
                    case "H02": type = TileType.Hitbox2; break;
                    case "H03": type = TileType.Hitbox3; break;
                    case "H04": type = TileType.Hitbox4; break;
                    case "H05": type = TileType.Hitbox5; break;
                    case "H06": type = TileType.Hitbox6; break;
                    case "H07": type = TileType.Hitbox7; break;
                    case "H08": type = TileType.Hitbox8; break;
                    case "H09": type = TileType.Hitbox9; break;
                    case "H10": type = TileType.Hitbox10; break;
                    case "H11": type = TileType.Hitbox11; break;
                    case "H12": type = TileType.Hitbox12; break;
                    case "H13": type = TileType.Hitbox13; break;
                    case "H14": type = TileType.Hitbox14; break;
                    case "H15": type = TileType.Hitbox15; break;
                    case "H16": type = TileType.Hitbox16; break;
                    case "H17": type = TileType.Hitbox17; break;

                    case "BSP": type = TileType.BlueSpawnPoint; break;
                    case "RSP": type = TileType.RedSpawnPoint; break;

                    default: type = TileType.MissingTexture; break;
                }

                mapTiles.Add(new Tile
                    (posX * Tile.SPRITE_WIDTH,
                    (posY - 1) * Tile.SPRITE_HEIGHT,
                    type));

                //Anything that contains a H is a hitbox
                if (mapParts[posX].Contains("H"))
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

    //Renders the tiles.
    public void Render(Camera camera)
    {
        foreach (Tile tile in Tiles)
        {
            //Only render the ones that are in the camera bounds
            if(CollisionHandler.CollidesWith
                (camera,tile,Tile.SPRITE_WIDTH,Tile.SPRITE_HEIGHT))
                tile.Render(camera.X, camera.Y);
        }
    }
}
