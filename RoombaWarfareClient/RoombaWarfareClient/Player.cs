using SDL2;

public enum PlayerTeam : byte { Red, Blue, Spectator}
public enum PlayerType : byte { Assault, Commander, Rusher, Tank }

public class Player : DynamicEntity
{
    public static readonly byte RADIUS = 24;
    public static readonly byte SPRITE_WIDTH = 64;
    public static readonly byte SPRITE_HEIGHT = 64;

    public int ID { get; set; }
    public float Angle { get; set; }
    public bool IsAlive { get; set; }
    public PlayerTeam Team { get; set; }
    public PlayerType Type { get; set; }

    public Player(PlayerType type)
    {
        ID = 0;
        Angle = 0;
        IsAlive = false;
        Team = PlayerTeam.Spectator;
        Type = type;
        spriteY = 0;

        //Assings the sprite coordinates and speed of the player
        switch (type)
        {
            case PlayerType.Assault:
                speed = 3.2f;
                spriteX = 0;
                break;

            case PlayerType.Commander:
                speed = 2.0f;
                spriteX = 64;
                break;

            case PlayerType.Rusher:
                speed = 4.0f;
                spriteX = 128;
                break;

            case PlayerType.Tank:
                speed = 1.6f;
                spriteX = 192;
                break;
        }
    }

    //Changes the position of a player
    public void SetPos(string[] parts)
    {
        float posX = float.Parse(parts[2]);
        float posY = float.Parse(parts[3]);
        SetPos(posX, posY);
    }

    //Changes the team of a player and its sprite
    public void SetTeam(string[] parts)
    {
        PlayerTeam team = (PlayerTeam)int.Parse(parts[2]);
        Team = team;

        if (Team == PlayerTeam.Red)
            spriteY = 0;
        else if (Team == PlayerTeam.Blue)
            spriteY = 64;
    }

    public virtual void Kill()
    {
        IsAlive = false;
    }

    //Respawns a player
    public virtual void Respawn(string[] parts)
    {
        float posX = float.Parse(parts[2]);
        float posY = float.Parse(parts[3]);
        SetPos(posX, posY);
        IsAlive = true;
    }

    //Renders the player with the given angle
    public virtual void Render(Camera camera)
    {
        if (IsAlive)
        {
            SDL.SDL_Point center = new SDL.SDL_Point
            {
                x = SPRITE_WIDTH / 2,
                y = SPRITE_HEIGHT / 2
            };

            Hardware.RenderAdvancedDynamic(SpriteSheet.Texture,
                camera.X, camera.Y, PosX, PosY, SPRITE_WIDTH, SPRITE_HEIGHT,
                spriteX, spriteY, Angle, center, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }
}
