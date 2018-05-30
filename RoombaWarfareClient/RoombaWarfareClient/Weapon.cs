using SDL2;
using System;

public abstract class Weapon
{
    protected readonly ushort maxAmmo;
    protected ushort currentAmmo;
    protected ushort shootDelay;
    protected ushort reloadDelay;

    protected uint lastShootTime;
    protected uint lastReloadTime;

    protected bool isShooting;
    protected bool isReloading;

    protected StatusBar ammoBar;

    public Weapon(ushort maxAmmo,ushort shootDelay,ushort reloadDelay)
    {
        this.maxAmmo = maxAmmo;
        currentAmmo = maxAmmo;
        this.shootDelay = shootDelay;
        this.reloadDelay = reloadDelay;

        ammoBar = new StatusBar(StatusBarType.Ammo);
        ammoBar.SetPos
            (0, Hardware.ScreenHeight - StatusBar.SPRITE_HEIGHT);
        ammoBar.Resize(currentAmmo, maxAmmo);
    }

    //Handles the events of the weapon
    public virtual void HandleEvents(SDL.SDL_Event e)
    {
        //Checks if the player is reloading and reloads the weapon
        //if enough time has passed (reloadDelay)
        if (isReloading)
        {
            uint timeElapsed = (SDL.SDL_GetTicks() - lastReloadTime);
            if(timeElapsed >= reloadDelay)
            {
                isReloading = false;
                
                currentAmmo = maxAmmo;
                ammoBar.Resize(currentAmmo, maxAmmo);
            }
        }

        if (Hardware.IsKeyPressed(SDL.SDL_Keycode.SDLK_r, e) && !isReloading)
        {
            isReloading = true;
            lastReloadTime = SDL.SDL_GetTicks();
        }
            
    }

    //Checks if the weapon can shoot
    protected bool IsAbleToShoot()
    {
        uint timeElapsed = (SDL.SDL_GetTicks() - lastShootTime);
        return (timeElapsed >= shootDelay && isShooting
            && !isReloading && currentAmmo != 0);
    }

    //Creates a bullet and returns it like a message for the server
    protected string CreateBullet(int id,float playerPosX, float playerPosY)
    {
        SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
        //Get the center of the player
        double playerCenterX = playerPosX + Player.SPRITE_WIDTH / 2;
        double playerCenterY = playerPosY + Player.SPRITE_HEIGHT / 2;
        //Get the distance between the player and the mouse
        double distance = Math.Sqrt(Math.Pow((playerCenterX - mouseX), 2)
            + Math.Pow((playerCenterY - mouseY), 2));
        //Get the direction of the bullet
        double dirX = (mouseX - playerCenterX) / distance;
        double dirY = (mouseY - playerCenterY) / distance;

        return (int)ClientMessage.Shoot + " " + id + " " 
            + dirX.ToString("0.##") + " " + dirY.ToString("0.##") + ":";
    }

    public abstract string Shoot(int id, float playerPosX, float playerPosY);

    //Renders the ammo bar
    public void Render()
    {
        ammoBar.Render();
    }
}
