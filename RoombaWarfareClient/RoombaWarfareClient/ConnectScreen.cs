
//This screen tries to connect to the server

public class ConnectScreen : IScreen
{
    public ScreenType NextScreen { get; set; }

    public ScreenType Run()
    {
        Game.GameSocket = new SocketClient();

        if (Game.GameSocket.Connect())
        {
            NextScreen = ScreenType.Game;
        }
        else
        {
            NextScreen = ScreenType.End;
            Game.EndMessage = 
                Game.LanguageTranslation[Game.GameLanguage + "NoConnect"];
        }

        return NextScreen;
    }
}

