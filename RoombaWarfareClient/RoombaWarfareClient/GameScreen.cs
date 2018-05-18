//This screen controls the game loop

public class GameScreen : IScreen
{
    public ScreenType NextScreen { get; set; }

    public GameScreen()
    {
        //TO DO
    }

    public ScreenType Run()
    {
        //TO DO
        return NextScreen;
    }
}

