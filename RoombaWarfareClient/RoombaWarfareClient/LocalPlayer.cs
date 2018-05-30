using System;
using SDL2;
using System.Collections.Generic;

public class LocalPlayer : Player
{
    public uint LastCommandProccesed { get; set; }

    private float velX;
    private float velY;

    private readonly short maxHealth;
    private short currentHealth;

    private StatusBar healthBar;
    private Weapon weapon;

    private string messageBuffer;
    private Queue<string> pendingCommands;
    private uint lastCommandSent;

    public LocalPlayer(PlayerType type) : base(type)
    {
        messageBuffer = "";
        pendingCommands = new Queue<string>();
        lastCommandSent = 0;

        healthBar = new StatusBar(StatusBarType.Health);
        healthBar.SetPos
            (0, Hardware.ScreenHeight - StatusBar.SPRITE_HEIGHT * 2);
        //Asings the type of weapon the player will have
        switch (type)
        {
            case PlayerType.Assault:
                weapon = new MachineGun(30, 350, 1800);
                maxHealth = 125;
                break;

            case PlayerType.Commander:
                weapon = new Gun(10, 800, 2000);
                maxHealth = 150;
                break;

            case PlayerType.Rusher:
                weapon = new MachineGun(20, 100, 1000);
                maxHealth = 75;
                break;

            case PlayerType.Tank:
                weapon = new Gun(15, 500, 1800);
                maxHealth = 175;
                break;
        }

        currentHealth = maxHealth;
        healthBar.Resize(currentHealth, maxHealth);
    }

    //Handles the key presses of the player
    public void HandleEvents(SDL.SDL_Event e)
    {
        weapon.HandleEvents(e);

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
        float lastAngle = Angle;

        SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
        Angle = (float)(Math.Atan2(mouseY + camera.Y - (PosY + SPRITE_HEIGHT / 2),
                 mouseX + camera.X - (PosX + SPRITE_WIDTH / 2))
                 * 180 / Math.PI);
        //Send the new angle
        if (Angle != lastAngle)
            messageBuffer += (int)ClientMessage.NewAngle + " " + ID +
                " " + Angle.ToString("0.#") + ":";
    }

    //Updates the player position and sends the new data to the server
    public void Update(float deltaTime, Hitbox[] hitboxes)
    {
        float posXIncrement = (velX * deltaTime);
        float posYIncrement = (velY * deltaTime);
        
        PosX += posXIncrement;
        float centerX = PosX + SPRITE_WIDTH / 2;
        float centerY = PosY + SPRITE_HEIGHT / 2;
        //Checks collision on the X axis and the screen borders
        if (CheckCollisions(hitboxes,centerX,centerY)
            || centerX - RADIUS < 0 
            || centerX + RADIUS > Map.Width)
        {
            PosX -= posXIncrement;
            posXIncrement = 0;
        }

        PosY += posYIncrement;
        centerX = PosX + SPRITE_WIDTH / 2;
        centerY = PosY + SPRITE_HEIGHT / 2;
        //Checks collision on the Y axis and the screen borders
        if (CheckCollisions(hitboxes, centerX, centerY)
            || centerY - RADIUS < 0
            || centerY + RADIUS > Map.Height)
        {
            PosY -= posYIncrement;
            posYIncrement = 0;
        }

        //If the player is moving send the data (Increment of the x and y)
        if(posXIncrement != 0 || posYIncrement != 0)
        {
            lastCommandSent++;
            messageBuffer += (int)ClientMessage.NewPos + " " + ID 
                + " " + posXIncrement.ToString("0.#") 
                + " " + posYIncrement.ToString("0.#") 
                + " " + lastCommandSent + ":";
            pendingCommands.Enqueue
                (posXIncrement.ToString("0.#") + " " +
                posYIncrement.ToString("0.#"));
        }
    }

    //
    public void Shoot(Camera camera)
    {
        messageBuffer += 
            weapon.Shoot(ID, PosX - camera.X, PosY - camera.Y);
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

    //Reconciles the player position with the server
    public void Reconcile()
    {
        //Delete all the commands that have been processed by the server
        while (pendingCommands.Count > (lastCommandSent - LastCommandProccesed))
            pendingCommands.Dequeue();

        foreach (string command in pendingCommands)
        {
            string[] parts = command.Split();
            PosX += float.Parse(parts[0]);
            PosY += float.Parse(parts[1]);
        }
    }

    public bool CheckCollisions(Hitbox[] hitboxes,float centerX,float centerY)
    {
        foreach (Hitbox hitbox in hitboxes)
        {
            if(CollisionHandler.CollidesWith
                ((int)centerX,(int)centerY,RADIUS,hitbox))
            {
                return true;
            }
        }

        return false;
    }

    //Returns the player messages
    public string GetMessage()
    {
        string ret = messageBuffer;
        messageBuffer = "";
        return ret;
    }

    public override void Render(Camera camera)
    {
        base.Render(camera);
        healthBar.Render();
        weapon.Render();
    }

    public void TakeDamage(string[] parts)
    {
        ushort amount = ushort.Parse(parts[1]);
        currentHealth -= (short)amount;
        healthBar.Resize(currentHealth, maxHealth);
    }
}
