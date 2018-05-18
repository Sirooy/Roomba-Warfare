
public enum ScreenType : byte
{
    None, Main, SetAddress,
    SelectPlayer, Connect, 
    Game, End , Exit
}

/* All screens must implement this interface. Whenever a screen
 * ends doing its job, returns the next screen that will follow it. */

public interface IScreen
{
    ScreenType NextScreen { get; set; }

    ScreenType Run();
}
