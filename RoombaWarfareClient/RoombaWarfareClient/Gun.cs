
using SDL2;

public class Gun : Weapon
{
    public Gun(ushort maxAmmo, ushort shootDelay, ushort reloadDelay) :
        base(maxAmmo, shootDelay, reloadDelay) { }

    public override void HandleEvents(SDL.SDL_Event e)
    {
        base.HandleEvents(e);

        if(e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
        {
            isShooting = true;
        }
    }

    public override string Shoot(int id, float playerPosX, float playerPosY)
    {
        if (IsAbleToShoot())
        {
            isShooting = false;
            lastShootTime = SDL.SDL_GetTicks();
            currentAmmo--;
            ammoBar.Resize(currentAmmo, maxAmmo);
            return CreateBullet(id, playerPosX, playerPosY);
        }

        return "";
    }
}

