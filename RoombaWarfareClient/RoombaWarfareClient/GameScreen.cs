using System;
using SDL2;

//This screen controls the game loop

public class GameScreen : IScreen
{
    public ScreenType NextScreen { get; set; }

    private Map map;
    private LocalPlayer localPlayer;
    private PlayerCollection players;
    private Camera camera;


    public GameScreen()
    {
        NextScreen = ScreenType.None;
        localPlayer = new LocalPlayer(Game.PlayerSelectedType);
        players = new PlayerCollection();
        camera = new Camera((ushort)Hardware.ScreenWidth, 
            (ushort)Hardware.ScreenHeight);
        map = new Map();
    }

    public ScreenType Run()
    {
        string mapSeed = Game.GameSocket.Receive();
        map.Create(mapSeed);
        string initialData = Game.GameSocket.Receive();
        Game.GameSocket.Send(Convert.ToString((int)Game.PlayerSelectedType));
        GameLoop();
        //TO DO
        return NextScreen;
    }

    public void GameLoop()
    {
        do
        {
            //TO DO
            while(SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
            {

            }
            Hardware.ClearScreen();
            map.Render(camera);
            Hardware.UpdateScreen();
        } while (NextScreen == ScreenType.None);
    }
}

