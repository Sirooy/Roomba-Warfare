
public class EndScreen : IScreen
{
    public ScreenType NextScreen { get; set; }
    private static Image background =
        new Image(@"resources\images\backgrounds\bck_test.png",
        (ushort)Hardware.ScreenWidth, (ushort)Hardware.ScreenHeigth);
    private Font font;
    private Text endText;

    public EndScreen()
    {
        font = new Font(@"resources\fonts\RWFont.ttf", 24);
        endText = new Text(font, Game.EndMessage, 0, 255, 0);
    }

    public ScreenType Run()
    {
        //TO DO
        do
        {
            Hardware.ClearScreen();
            Hardware.RenderBackground(background);

            endText.Render(Hardware.ScreenWidth - endText.Width,
             Hardware.ScreenHeigth - endText.Height);

            Hardware.UpdateScreen();
            System.Threading.Thread.Sleep(16);
        } while (NextScreen == ScreenType.None);

        return ScreenType.Exit;
    }
}
