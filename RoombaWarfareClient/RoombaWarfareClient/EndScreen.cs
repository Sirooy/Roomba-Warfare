﻿using SDL2;

public class EndScreen : IScreen
{
    public ScreenType NextScreen { get; set; }

    private static Image background;

    private Font font;
    private Text text;

    static EndScreen()
    {
        background = new Image
            (Game.LanguageTranslation[Game.GameLanguage + "EndPath"], 
            800, 600);
    }

    public EndScreen()
    {
        NextScreen = ScreenType.None;
        font = new Font(@"resources\fonts\RWFont.ttf", 16);
        text = new Text(font, Game.EndMessage, 0xFF, 0xFF, 0xFF);
    }

    public ScreenType Run()
    {
        Hardware.ClearScreen();
        Hardware.RenderBackground(background);
        text.Render(Hardware.ScreenWidth / 2 - text.Width / 2,
             Hardware.ScreenHeight / 2 - text.Height / 2);
        Hardware.UpdateScreen();

        do
        {
            SDL.SDL_Keycode key = Hardware.KeyPressed();
            if (key == SDL.SDL_Keycode.SDLK_RETURN)
                NextScreen = ScreenType.Main; 

            System.Threading.Thread.Sleep(16);
        } while (NextScreen == ScreenType.None);

        return NextScreen;
    }
}
