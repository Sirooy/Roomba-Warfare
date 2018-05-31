using SDL2;

//In this screen the player is able to select the type of player.
public class SelectPlayerScreen : IScreen
{
    private const byte ASSAULT_PLAYER = 0;
    private const byte COMMANDER_PLAYER = 1;
    private const byte RUSHER_PLAYER = 2;
    private const byte TANK_PLAYER = 3;

    private static Image background;

    public ScreenType NextScreen { get; set; }

    private Button[] buttons;

    static SelectPlayerScreen()
    {
        background = new Image
        (@"resources\images\backgrounds\select_character_background.png", 
        800, 600);
    }

    public SelectPlayerScreen()
    {
        buttons = new Button[4];
        //Creates the buttons in the four corners
        buttons[ASSAULT_PLAYER] = new Button(ButtonType.SelectAssaultPlayer);
        buttons[ASSAULT_PLAYER].SetPos(0,0);
        buttons[COMMANDER_PLAYER] = 
            new Button(ButtonType.SelectCommanderPlayer);
        buttons[COMMANDER_PLAYER].SetPos
            (0, Hardware.ScreenHeight - Button.SPRITE_HEIGHT);
        buttons[RUSHER_PLAYER] = new Button(ButtonType.SelectRusherPlayer);
        buttons[RUSHER_PLAYER].SetPos
            (Hardware.ScreenWidth - Button.SPRITE_WIDTH, 0);
        buttons[TANK_PLAYER] = new Button(ButtonType.SelectTankPlayer);
        buttons[TANK_PLAYER].SetPos
            (Hardware.ScreenWidth - Button.SPRITE_WIDTH,
            Hardware.ScreenHeight - Button.SPRITE_HEIGHT);
    }

    public ScreenType Run()
    {
        do
        {
            Hardware.ClearScreen();
            Hardware.RenderBackground(background);

            foreach (Button button in buttons)
                button.Render();

            HandleEvents();

            Hardware.UpdateScreen();
            System.Threading.Thread.Sleep(16);
        } while (NextScreen == ScreenType.None);

        return NextScreen;
    }

    //Handles all the buttons events
    public void HandleEvents()
    {
        while(SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            foreach (Button b in buttons)
                b.HandleEvents(e);
        }

        if (buttons[ASSAULT_PLAYER].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Assault;
            NextScreen = ScreenType.Connect;
        }
        else if (buttons[COMMANDER_PLAYER].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Commander;
            NextScreen = ScreenType.Connect;
        }
        else if (buttons[RUSHER_PLAYER].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Rusher;
            NextScreen = ScreenType.Connect;
        }
        else if (buttons[3].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Tank;
            NextScreen = ScreenType.Connect;
        }
    }
}
