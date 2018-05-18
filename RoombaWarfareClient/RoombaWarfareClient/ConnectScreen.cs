
//This screen tries to connect to the server

public class ConnectScreen : IScreen
{
    public ScreenType NextScreen { get; set; }

    public ScreenType Run()
    {
        if (Game.GameSocket.Connect())
        {
            NextScreen = ScreenType.Game;
        }
        else
        {
            NextScreen = ScreenType.End;
            Game.EndMessage = "Could not connect to the server.";
        }

        return NextScreen;
    }
}

