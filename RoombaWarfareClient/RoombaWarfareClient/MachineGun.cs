using SDL2;

public class MachineGun : Weapon
{
    public MachineGun(ushort maxAmmo, ushort shootDelay, ushort reloadDelay) :
        base(maxAmmo, shootDelay, reloadDelay) { }


    public override void HandleEvents(SDL.SDL_Event e)
    {
        base.HandleEvents(e);

        if(e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
        {
            isShooting = true;
        }
        else if(e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
        {
            isShooting = false;
        }
    }

    public override string Shoot(int id, float playerPosX, float playerPosY)
    {
        if (IsAbleToShoot())
        {
            lastShootTime = SDL.SDL_GetTicks();
            currentAmmo--;
            ammoBar.Resize(currentAmmo, maxAmmo);
            return CreateBullet(id, playerPosX, playerPosY);
        }

        return "";
    }
}

