using System;
using SDL2;

public class Font
{
    public IntPtr FontType { get; set; }
    public ushort Size { get; set; }

    //Creates a font from a path
    public Font(string path,ushort fontSize)
    {
        Size = fontSize;

        FontType = SDL_ttf.TTF_OpenFont(path, fontSize);

        if(FontType == null)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not create the screen", (IntPtr)null);
            Environment.Exit(4);
        }
    }
}

