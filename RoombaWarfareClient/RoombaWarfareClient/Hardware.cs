using System;
using SDL2;

public static class Hardware
{
    public static int ScreenWidth;
    public static int ScreenHeigth;
    public static IntPtr Renderer;
    private static IntPtr screen;

    //Inits SDL,SDL_image and SDL_ttf or exits the aplication if it fails
    static Hardware()
    {
        if(SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not init SDL", screen);
            Environment.Exit(1);
        }
        if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) < 0)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not init SDL_Image", screen);
            Environment.Exit(1);
        }
        if (SDL_ttf.TTF_Init() < 0)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not init SDL_ttf", screen);
            Environment.Exit(1);
        }
    }

    //Creates the screen and renderer or exits the aplication if it fails
    public static void Init(int w = 640,int h = 480,bool fullScreen = false)
    {
        //Flags for the screen
        SDL.SDL_WindowFlags screenFlags = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN;
        if (fullScreen)
            screenFlags |= SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;

        screen = SDL.SDL_CreateWindow("Roomba Warfare",
            SDL.SDL_WINDOWPOS_UNDEFINED,SDL.SDL_WINDOWPOS_UNDEFINED,
            w,h,screenFlags);

        //Get the window size (In case we active Fullscreen)
        SDL.SDL_GetWindowSize(screen, out ScreenWidth, out ScreenHeigth);

        if(screen == null)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not create the screen", screen);
            Environment.Exit(2);
        }

        //Flags for the renderer, vsync active
        SDL.SDL_RendererFlags rendererFlags =
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
            SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;

        Renderer = SDL.SDL_CreateRenderer(screen, -1, rendererFlags);

        if(Renderer == null)
        {
            SDL.SDL_ShowSimpleMessageBox
                (SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "Error",
                "Could not create the renderer", screen);
            Environment.Exit(2);
        }
    }

    //Renders an image on screen depending on the camera position
    public static void RenderDynamic
        (IntPtr texture, float camX, float camY
        , float posX, float posY
        , ushort spriteWidth, ushort spriteHeight
        , ushort spriteX, ushort spriteY)
    {
        RenderStatic(texture, posX - camX, posY - camY,
            spriteWidth, spriteHeight, spriteX, spriteY);
    }

    //Renders an image on a static position
    public static void RenderStatic
        (IntPtr texture, float posX, float posY
        , ushort spriteWidth, ushort spriteHeight
        , ushort spriteX, ushort spriteY)
    {
        SDL.SDL_Rect source = new SDL.SDL_Rect
        {
            x = spriteX,
            y = spriteY,
            w = spriteWidth,
            h = spriteHeight
        };

        SDL.SDL_Rect target = new SDL.SDL_Rect
        {
            x = (int)(posX),
            y = (int)(posY),
            w = spriteWidth,
            h = spriteHeight
        };

        SDL.SDL_RenderCopy(Renderer, texture, ref source, ref target);
    }

    //Renders an image on a static position with the given angle and flip
    public static void RenderAdvancedStatic
       (IntPtr texture, float posX, float posY
       , ushort spriteWidth, ushort spriteHeight
       , ushort spriteX, ushort spriteY
       , float angle, SDL.SDL_Point center,
       SDL.SDL_RendererFlip flip)
    {
        SDL.SDL_Rect source = new SDL.SDL_Rect
        {
            x = spriteX,
            y = spriteY,
            w = spriteWidth,
            h = spriteHeight
        };

        SDL.SDL_Rect target = new SDL.SDL_Rect
        {
            x = (int)(posX),
            y = (int)(posY),
            w = spriteWidth,
            h = spriteHeight
        };

        SDL.SDL_RenderCopyEx(Renderer, texture, ref source, ref target,
            angle, ref center, flip);
    }

    //Renders an image on screen depending on the camera position with 
    //the given angle and flip.
    public static void RenderAdvancedDynamic
        (IntPtr texture, float camX, float camY
        , float posX, float posY
        , ushort spriteWidth, ushort spriteHeight
        , ushort spriteX, ushort spriteY
        , float angle, SDL.SDL_Point center,
        SDL.SDL_RendererFlip flip)
    {
        RenderAdvancedStatic(texture, posX - camX, posY - camY
            , spriteWidth, spriteHeight, spriteX, spriteY,
            angle, center, flip);
    }

    //Renders an image as if it is a background
    //(Scaling its size to match the screen size)
    public static void RenderBackground(Image background)
    {
        SDL.SDL_Rect source = new SDL.SDL_Rect
        {
            x = 0,
            y = 0,
            w = background.Width,
            h = background.Height
        };

        SDL.SDL_Rect target = new SDL.SDL_Rect
        {
            x = 0,
            y = 0,
            w = ScreenWidth,
            h = ScreenHeigth
        };

        SDL.SDL_RenderCopy(Renderer, background.Texture,ref source,ref target);
    }

    //Returns true if the given key is pressed
    public static bool IsKeyPressed(SDL.SDL_Keycode key,SDL.SDL_Event e)
    {
        return (e.type == SDL.SDL_EventType.SDL_KEYDOWN
                && e.key.repeat == 0
                && e.key.keysym.sym == key);
    }

    //Returns the keycode of the pressed key
    public static SDL.SDL_Keycode KeyPressed()
    {
        int ret = -1;
        
        if(SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            if (e.type == SDL.SDL_EventType.SDL_KEYDOWN
                && e.key.repeat == 0)
                ret = (int)e.key.keysym.sym;
        }

        return (SDL.SDL_Keycode)ret;
    }

    //Updates the screen
    public static void UpdateScreen()
    {
        SDL.SDL_RenderPresent(Renderer);
    }

    //Clears the screen
    public static void ClearScreen()
    {
        SDL.SDL_RenderClear(Renderer);
    }
}

