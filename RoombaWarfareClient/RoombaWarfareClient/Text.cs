using System;
using SDL2;

//Creates a text
public class Text
{
    public ushort Width { get; }
    public ushort Height { get; }

    private IntPtr textureText;
    
    //Creates a texture from a given font and text with the specified color
    public Text(Font font,string text,byte r,byte g,byte b)
    {
        SDL.SDL_Color textColor = new SDL.SDL_Color
        {
            r = r,
            g = g,
            b = b,
            a = 0xFF
        };

        //Creates a surface with the font,text and color
        IntPtr surface = 
            SDL_ttf.TTF_RenderText_Solid(font.FontType, text, textColor);

        if(surface == null)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not create the text surface", (IntPtr)null);
            Environment.Exit(5);
        }

        textureText =
            SDL.SDL_CreateTextureFromSurface(Hardware.Renderer, surface);

        if (textureText == null)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not create the text texture", (IntPtr)null);
            Environment.Exit(5);
        }

        SDL.SDL_FreeSurface(surface);

        //Assings the width and heigth of the texture based on the font size
        Width = (ushort)(text.Length * font.Size);
        Height = (ushort)(font.Size * 2);
    }

    //Renders the text on screen
    public void Render(float posX,float posY)
    {
        Hardware.RenderStatic(textureText, posX, posY, 
            Width, Height, 0, 0);
    }

    ~Text()
    {
        textureText = (IntPtr)null;
    }
}
