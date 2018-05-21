
//Controls the game 
public class Game
{
    public static string ServerAddress;
    public static PlayerType PlayerSelectedType;
    public static string EndMessage;
    public static SocketClient GameSocket;

    public Game()
    {
        Hardware.Init();
        ServerAddress = "";
        EndMessage = "";
        GameSocket = new SocketClient();
    }

    //Creates the next screen 
    public IScreen GetNewScreen(ScreenType type)
    {
        switch (type)
        {
            case ScreenType.Main:
                return new MainScreen();
            case ScreenType.SetAddress:
                return new SetAddressScreen();
            case ScreenType.SelectPlayer:
                return new SelectPlayerScreen();
            case ScreenType.Connect:
                return new ConnectScreen();
            case ScreenType.Game:
                return new GameScreen();
            case ScreenType.End:
                return new EndScreen();
        }

        return null;
    }

    public void Run()
    {
        Cursor cursor = new Cursor(@"resources\images\crosshair.png");
        ScreenType currentScreen = ScreenType.SetAddress; //Change later to main

        while(currentScreen != ScreenType.Exit)
        {
            currentScreen = GetNewScreen(currentScreen).Run();
        }
    }
}
