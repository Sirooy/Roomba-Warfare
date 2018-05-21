//This screen controls the game loop

public class GameScreen : IScreen
{
    public ScreenType NextScreen { get; set; }

    private Map map;
    private LocalPlayer localPlayer;
    private PlayerCollection players;


    public GameScreen()
    {
        map = new Map();
    }

    public ScreenType Run()
    {
        string mapSeed = Game.GameSocket.Receive();
        map.Create(mapSeed);
        string initialData = Game.GameSocket.Receive();
        //TO DO
        return NextScreen;
    }
}

