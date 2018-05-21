using SDL2;

//In this screen the player is able to select the type of player.
public class SelectPlayerScreen : IScreen
{
    private static Image background =
        new Image(@"resources\images\backgrounds\bck_test.png", 640, 480);

    public ScreenType NextScreen { get; set; }

    private Button[] buttons;

    public SelectPlayerScreen()
    {
        buttons = new Button[4];
        //Creates the buttons in the four corners
        buttons[0] = new Button(ButtonType.SelectAssaultPlayer);
        buttons[0].SetPos(0,0);
        buttons[1] = new Button(ButtonType.SelectCommanderPlayer);
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
    }

    
    public void HandleEvents()
    {
        while(SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            foreach (Button b in buttons)
                b.HandleEvents(e);
        }

        //Assault
        if (buttons[0].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Assault;
            NextScreen = ScreenType.Connect;
        }
        //Commander
        else if (buttons[1].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Commander;
            NextScreen = ScreenType.Connect;
        }
        //Rusher
        else if (buttons[2].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Rusher;
            NextScreen = ScreenType.Connect;
        }
        //Tank
        else if (buttons[3].IsClicked)
        {
            Game.PlayerSelectedType = PlayerType.Tank;
            NextScreen = ScreenType.Connect;
        }
    }
}
