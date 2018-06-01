using System.Collections.Generic;

//Controls the game 
public class Game
{
    public static string ServerAddress;
    public static PlayerType PlayerSelectedType;
    public static string EndMessage;
    public static SocketClient GameSocket;
    public static Language GameLanguage;
    public static Dictionary<string, string> LanguageTranslation;

    public Game()
    {
        Hardware.Init();
        ServerAddress = "127.0.0.1-23000";
        EndMessage = "";
        GameSocket = new SocketClient();
        GameLanguage = Language.English;

        LanguageTranslation = new Dictionary<string, string>();
        LanguageTranslation.Add(Language.English + "VIP","- Valid IP -");
        LanguageTranslation.Add(Language.English + "IIP", "- Invalid IP -");
        LanguageTranslation.Add(Language.English + "NoConnect", 
            "Could not connect to the server");
        LanguageTranslation.Add(Language.English + "LostConnection", 
            "Connection lost");
        LanguageTranslation.Add(Language.English + "MaxPlayers",
            "Server is full");
        LanguageTranslation.Add(Language.English + "ServerClosed",
            "Server has closed");
        LanguageTranslation.Add(Language.English + "RedWon",
            "Red team won");
        LanguageTranslation.Add(Language.English + "BlueWon",
            "Blue team won");
        LanguageTranslation.Add(Language.English + "Draw",
            "Draw");
        LanguageTranslation.Add(Language.English + "SetAddressPath",
            @"resources\images\backgrounds\set_address_background_english.png");
        LanguageTranslation.Add(Language.English + "EndPath",
            @"resources\images\backgrounds\end_background_english.png");

        LanguageTranslation.Add(Language.Spanish + "VIP", "- IP valida -");
        LanguageTranslation.Add(Language.Spanish + "IIP", "- IP no valida -");
        LanguageTranslation.Add(Language.Spanish + "NoConnect", 
            "No te has podido conectar con el servidor");
        LanguageTranslation.Add(Language.Spanish + "LostConnection", 
            "Se ha perdido la conexion");
        LanguageTranslation.Add(Language.Spanish + "MaxPlayers",
            "El servidor esta lleno");
        LanguageTranslation.Add(Language.Spanish + "ServerClosed",
            "El servidor ha cerrado");
        LanguageTranslation.Add(Language.Spanish + "SetAddressPath",
            @"resources\images\backgrounds\set_address_background_spanish.png");
        LanguageTranslation.Add(Language.Spanish + "EndPath",
            @"resources\images\backgrounds\end_background_spanish.png");
        LanguageTranslation.Add(Language.Spanish + "RedWon",
            "El equipo rojo ha ganado");
        LanguageTranslation.Add(Language.Spanish + "BlueWon",
            "El equipo azul ha ganado");
        LanguageTranslation.Add(Language.Spanish + "Draw",
            "Empate");
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
            case ScreenType.SelectLanguage:
                return new SelectLanguageScreen();
        }

        return null;
    }

    public void Run()
    {
        Cursor cursor = new Cursor(@"resources\images\crosshair.png");
        ScreenType currentScreen = ScreenType.SelectLanguage; 

        while(currentScreen != ScreenType.Exit)
        {
            currentScreen = GetNewScreen(currentScreen).Run();
        }
    }
}
