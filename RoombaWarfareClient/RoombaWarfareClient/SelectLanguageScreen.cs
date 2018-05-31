using SDL2;

public enum Language { English, Spanish }

public class SelectLanguageScreen : IScreen
{
    private const byte ENGLISH = 0;
    private const byte SPANISH = 1;

    private static Image background;

    private Button[] buttons;

    public ScreenType NextScreen { get; set; }

    //Remove later
    private Font font;
    private Text englishText;
    private Text spanishText;

    static SelectLanguageScreen()
    {
        background = new Image
            (@"resources\images\backgrounds\set_language_background.png", 
            800, 600);
        
    }

    public SelectLanguageScreen()
    {
        NextScreen = ScreenType.None;
        buttons = new Button[2];
        buttons[ENGLISH] = new Button(ButtonType.SelectLanguageEnglish);
        buttons[ENGLISH].SetPos
            (Hardware.ScreenWidth / 2 - Button.SPRITE_WIDTH / 2,
            Hardware.ScreenHeight / 2 - Button.SPRITE_HEIGHT * 2);
        buttons[SPANISH] = new Button(ButtonType.SelectLanguageSpanish);
        buttons[SPANISH].SetPos
            (Hardware.ScreenWidth / 2 - Button.SPRITE_WIDTH / 2,
            Hardware.ScreenHeight / 2 + Button.SPRITE_HEIGHT);

        //Remove later
        font = new Font(@"resources\fonts\RWFont.ttf", 16);

        englishText =
            new Text(font,"English"
            , 0x00, 0x00, 0xFF);
        spanishText =
            new Text(font,"Spanish"
            , 0x00, 0x00, 0xFF);
    }

    public ScreenType Run()
    {
        do
        {
            Hardware.ClearScreen();
            HandleEvents();

            Hardware.RenderBackground(background);
            foreach (Button button in buttons)
                button.Render();

            //Remove later
            englishText.Render(Hardware.ScreenWidth / 2 - englishText.Width / 2,
            (int)(Hardware.ScreenHeight / 2 - Button.SPRITE_HEIGHT * 1.5));
            spanishText.Render(Hardware.ScreenWidth / 2 - englishText.Width / 2,
            (int)(Hardware.ScreenHeight / 2 + Button.SPRITE_HEIGHT * 1.5));

            Hardware.UpdateScreen();
        } while (NextScreen == ScreenType.None);

        return NextScreen;
    }


    //Handles all the buttons events
    public void HandleEvents()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            foreach (Button button in buttons)
                button.HandleEvents(e);
        }
    
        if (buttons[ENGLISH].IsClicked)
        {
            Game.GameLanguage = Language.English;
            NextScreen = ScreenType.Main;
        }
        else if (buttons[SPANISH].IsClicked)
        {
            Game.GameLanguage = Language.Spanish;
            NextScreen = ScreenType.Main;
        }
    }
}

