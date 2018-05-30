using SDL2;

public class MainScreen : IScreen
{
    private const byte START_BUTTON = 0;
    private const byte ADDRESS_BUTTON = 1;
    private const byte EXIT_BUTTON = 2;

    public ScreenType NextScreen { get; set; }

    private static Image background = new Image
        (@"resources\images\backgrounds\main_menu_background.png", 800, 600);
    private Button[] buttons;

    public MainScreen()
    {
        NextScreen = ScreenType.None;

        buttons = new Button[3];

        //Creates all the buttons 
        buttons[START_BUTTON] = new Button(ButtonType.StartButton);
        buttons[START_BUTTON].SetPos(0, 100);
        buttons[ADDRESS_BUTTON] = new Button(ButtonType.SetAddressButton);
        buttons[ADDRESS_BUTTON].SetPos
            (0, Hardware.ScreenHeight / 2 - Button.SPRITE_HEIGHT / 2);
        buttons[EXIT_BUTTON] = new Button(ButtonType.ExitButton);
        buttons[EXIT_BUTTON].SetPos
            (0,Hardware.ScreenHeight - 100 - Button.SPRITE_HEIGHT);
    }

    public ScreenType Run()
    {
        do
        {
            Hardware.ClearScreen();
            Hardware.RenderBackground(background);

            HandleEvents();

            foreach (Button button in buttons)
                button.Render();

            Hardware.UpdateScreen();
            System.Threading.Thread.Sleep(16);
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

        if (buttons[START_BUTTON].IsClicked)
        {
            NextScreen = ScreenType.SelectPlayer;
        }
        else if (buttons[ADDRESS_BUTTON].IsClicked)
        {
            NextScreen = ScreenType.SetAddress;
        }
        else if (buttons[EXIT_BUTTON].IsClicked)
        {
            NextScreen = ScreenType.Exit;
        }
    }
}
