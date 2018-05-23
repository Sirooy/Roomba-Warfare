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
        if(e.type == SDL.SDL_EventType.SDL_KEYDOWN && e.key.repeat == 0)
        {
            switch (e.key.keysym.sym)
            {
                case SDL.SDL_Keycode.SDLK_w: velY -= speed; break;
                case SDL.SDL_Keycode.SDLK_s: velY += speed; break;
                case SDL.SDL_Keycode.SDLK_d: velX += speed; break;
                case SDL.SDL_Keycode.SDLK_a: velX -= speed; break;
            }
        }
        if(e.type == SDL.SDL_EventType.SDL_KEYUP && e.key.repeat == 0)
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

    //Sets the angle of the player based on the mouse position and the camera coords
    public void SetAngle(Camera camera)
    {
        SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
        Angle = (float)(Math.Atan2(mouseY + camera.Y - (PosY + SPRITE_HEIGHT / 2),
                 mouseX + camera.X - (PosX + SPRITE_WIDTH / 2))
                 * 180 / Math.PI);
    }

    public override void Update(float deltaTime)
    {
        PosX += (velX * deltaTime);
        PosY += (velY * deltaTime);
    }

    public override void Respawn(string[] parts)
    {
        base.Respawn(parts);
        currentHealth = maxHealth;
    }

    public override void Kill()
    {
        base.Kill();
        currentHealth = 0;
    }

    //TO DO
    public void Reconcile()
    {

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
