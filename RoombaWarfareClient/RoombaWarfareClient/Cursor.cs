using System;
using SDL2;

//Creates a custom cursor
public class Cursor
{
    private IntPtr cursorTexture;

    public Cursor(string path)
    {
        IntPtr surface = SDL_image.IMG_Load(path);
        cursorTexture = SDL.SDL_CreateColorCursor(surface, 10, 10);
        //Sets the texture of the cursor
        SDL.SDL_SetCursor(cursorTexture);
    }
}
