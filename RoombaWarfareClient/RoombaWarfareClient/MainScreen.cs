using SDL2;

public class MainScreen : IScreen
{
    public ScreenType NextScreen { get; set; }

    private static Image background =
        new Image(@"resources\images\backgrounds\bck_test.png", 640, 480);
    private Button[] buttons;

    public MainScreen()
    {
        buttons = new Button[4];
        //Creates the buttons in the four corners
        buttons[0] = new Button(ButtonType.StartButton);
        buttons[0].SetPos(0, 0);
        buttons[1] = new Button(ButtonType.ExitButton);
        buttons[1].SetPos
            (0, Hardware.ScreenHeight - Button.SPRITE_HEIGHT);
        buttons[2] = new Button(ButtonType.SelectRusherPlayer);
        buttons[2].SetPos
            (Hardware.ScreenWidth - Button.SPRITE_WIDTH, 0);
        buttons[3] = new Button(ButtonType.SelectTankPlayer);
        buttons[3].SetPos
            (Hardware.ScreenWidth - Button.SPRITE_WIDTH,
            Hardware.ScreenHeight - Button.SPRITE_HEIGHT);
    }

    public ScreenType Run()
    {
        //TO DO
        do
        {
            Hardware.ClearScreen();
            Hardware.RenderBackground(background);

            foreach (Button b in buttons)
                b.Render();

            HandleEvents();

            Hardware.UpdateScreen();
            System.Threading.Thread.Sleep(16);
        } while (NextScreen == ScreenType.None);

        return ScreenType.Exit;
        return ScreenType.None;
    }

    public void HandleEvents()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            foreach (Button b in buttons)
                b.HandleEvents(e);
        }

        //Start
        if (buttons[0].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Assault;
            NextScreen = ScreenType.End;
        }
        //Exit
        else if (buttons[1].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Commander;
            NextScreen = ScreenType.Exit;
        }

    }
}
