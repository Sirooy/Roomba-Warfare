using SDL2;

public class Weapon
{
    public ushort MaxAmmo { get { return maxAmmo; } }

    private readonly ushort maxAmmo;
    private ushort currentAmmo;

    public virtual string Shoot(SDL.SDL_Event e)
    {
        return "";
    }
}
