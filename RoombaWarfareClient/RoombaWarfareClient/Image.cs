using System;
using SDL2;

public class Image
{
    public IntPtr Texture { get; set; }
    public ushort Width { get; set; }
    public ushort Height { get; set; }

    //Creates the image or exits the aplication if it fails
    public Image(string path,ushort width, ushort height)
    {
        Width = width;
        Height = height;

        IntPtr surface = SDL_image.IMG_Load(path);

        if(surface == null)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not load the image", (IntPtr)null);
            Environment.Exit(3);
        }

        //Creates the texture from the surface loaded
        Texture = SDL.SDL_CreateTextureFromSurface(Hardware.Renderer, surface);

        if(Texture == null)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not create the texture", (IntPtr)null);
            Environment.Exit(3);
        }

        SDL.SDL_FreeSurface(surface);
    }
}
