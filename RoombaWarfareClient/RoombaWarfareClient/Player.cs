using SDL2;

public enum PlayerTeam : byte { Red, Blue, Spectator}
public enum PlayerType : byte { Assault, Commander, Rusher, Tank }

public class Player : DynamicEntity
{
    public static readonly byte SPRITE_WIDTH = 64;
    public static readonly byte SPRITE_HEIGHT = 64;

    public static Image SpriteSheed =
        new Image("resources/images/sprite_sheet_test.png", 64, 96);

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

        //Assings the sprite coordinates and speed of the player
        switch (type)
        {
            case PlayerType.Assault:
                speed = 2.5f;
                spriteX = 0;
                spriteY = 0;
                break;

            case PlayerType.Commander:
                //TO DO
                break;

            case PlayerType.Rusher:
                //TO DO
                break;

            case PlayerType.Tank:
                //TO DO
                break;
        }
    }

    //Renders the player with the given angle
    public void Render(float camX,float camY)
    {
        SDL.SDL_Point center = new SDL.SDL_Point
        {
            x = SPRITE_WIDTH / 2,
            y = SPRITE_HEIGHT / 2
        };

        Hardware.RenderAdvancedDynamic(SpriteSheet.Texture,
            camX,camY,PosX,PosY,SPRITE_WIDTH,SPRITE_HEIGHT,
            spriteX,spriteY,Angle,center,SDL.SDL_RendererFlip.SDL_FLIP_NONE);
    }
}
