using SDL2;

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
        if (isReloading)
        {
            uint timeElapsed = (SDL.SDL_GetTicks() - lastReloadTime);
            if(timeElapsed >= reloadDelay)
            {
                isReloading = false;
                lastReloadTime = SDL.SDL_GetTicks();
                currentAmmo = maxAmmo;
                ammoBar.Resize(currentAmmo, maxAmmo);
            }
        }

        if (Hardware.IsKeyPressed(SDL.SDL_Keycode.SDLK_r, e))
            isReloading = true;
    }

    //Checks if the weapon can shoot
    protected bool IsAbleToShoot()
    {
        uint timeElapsed = (SDL.SDL_GetTicks() - lastShootTime);
        return (timeElapsed >= shootDelay && isShooting
            && !isReloading && currentAmmo != 0);
    }

    //Creates a bullet and returns it like a message for the server
    public string CreateBullet(int id,float playerPosX, float playerPosY)
    {
        //TO DO
        return "";
    }

    public abstract string Shoot();

    //Renders the ammo bar
    public void Render()
    {
        ammoBar.Render();
    }
}
