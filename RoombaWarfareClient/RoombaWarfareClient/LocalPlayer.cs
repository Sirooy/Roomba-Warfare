using System;
using SDL2;

public class LocalPlayer : Player
{
    public uint LastCommandProccesed { get; set; }

    private float velX;
    private float velY;
    private readonly short maxHealth;
    private short currentHealth;

    public LocalPlayer(PlayerType type) : base(type)
    {
        //Asings the type of weapon the player will have
        switch (type)
        {
            case PlayerType.Assault:
                //TO DO
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

    public void HandleEvents(SDL.SDL_Event e)
    {
        if(e.type == SDL.SDL_EventType.SDL_KEYDOWN)
        {
            switch (e.key.keysym.sym)
            {
                case SDL.SDL_Keycode.SDLK_w: velY -= speed; break;
                case SDL.SDL_Keycode.SDLK_s: velY += speed; break;
                case SDL.SDL_Keycode.SDLK_d: velX += speed; break;
                case SDL.SDL_Keycode.SDLK_a: velX -= speed; break;
            }
        }
        if(e.type == SDL.SDL_EventType.SDL_KEYUP)
        {
            switch (e.key.keysym.sym)
            {
                case SDL.SDL_Keycode.SDLK_w: velY += speed; break;
                case SDL.SDL_Keycode.SDLK_s: velY -= speed; break;
                case SDL.SDL_Keycode.SDLK_d: velX -= speed; break;
                case SDL.SDL_Keycode.SDLK_a: velX += speed; break;
            }
        }
    }

    //Sets the angle of the player based on the mouse position
    public void SetAngle(float camX, float camY)
    {
        SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
        Angle = (float)(Math.Atan2(mouseY + camY - (PosY - SPRITE_HEIGHT / 2),
                 mouseX + camX - (PosX - SPRITE_WIDTH / 2))
                 * 180 / Math.PI);
    }

    public override void Update(float deltaTime)
    {
        PosX += (velX * deltaTime);
        PosY += (velY * deltaTime);
    }

    public override void Respawn(float posX, float posY)
    {
        base.Respawn(posX, posY);
        currentHealth = maxHealth;
    }

    public void CheckCollisions()
    {
        //TO DO
    }

    public void TakeDamage(short amount)
    {
        currentHealth -= amount;
    }
}
