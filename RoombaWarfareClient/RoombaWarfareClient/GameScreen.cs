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

    public void TranslateData(string[] allCommands)
    {
        foreach(string command in allCommands)
        {
            string[] commandParts = command.Split();

            switch ((ServerMessage)int.Parse(commandParts[0]))
            {
                case ServerMessage.SetPlayerAngle:
                    break;

                case ServerMessage.SetPlayerPosition:
                    break;

                case ServerMessage.SetPlayerTeam:
                    break;

                case ServerMessage.SetID:
                    localPlayer.ID = int.Parse(commandParts[1]);
                    break;

                case ServerMessage.Disconnect:

                    break;
            }
        }
    }

    public void Receive()
    {
        if (Game.GameSocket.DataAvailable)
        {
            string data = Game.GameSocket.Receive();
            TranslateData(data.Split(':'));
        }
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

