using System.Text.RegularExpressions;

//This screens allows the user to change the ip-port of the server
public class SetAddressScreen : IScreen
{
    private static Image background =
        new Image(@"resources\images\backgrounds\bck_test.png", 640, 480);
    private Keyboard keyboard;
    private Font font;

    public ScreenType NextScreen { get; set; }

    public SetAddressScreen()
    {
        NextScreen = ScreenType.None;
        font = new Font(@"resources\fonts\RWFont.ttf", 25);

        keyboard = new Keyboard();
        keyboard.OnSummitEvent += ValidateIPAddress;
        keyboard.OnTextChangedEvent += RenderTextChanged;
    }

    public ScreenType Run()
    {
        //Draw the background
        Hardware.ClearScreen();
        Hardware.RenderBackground(background);
        Hardware.UpdateScreen();

        do
        {
            keyboard.HandleEvents();
            System.Threading.Thread.Sleep(16);
        } while (NextScreen == ScreenType.None);

        return NextScreen;
    }

    //Checks if the ip entered is valid
    public void ValidateIPAddress(string address)
    {
        if(Regex.IsMatch(address,
            @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\-\d{3,5}$"))
        {
            string port = address.Split('-')[1];
            if(ushort.TryParse(port,out ushort result))
            {
                Game.ServerAddress = address;
                NextScreen = ScreenType.SelectPlayer; //Change this when the menu is implemented
            }
        }
    }

    //Whenever the text changes, we render the new text on screen
    public void RenderTextChanged(string address)
    {
        Text ipText = new Text(font, address, 0xff, 0xff, 0xff);

        Hardware.ClearScreen();
        Hardware.RenderBackground(background);
        //Renders the text on the middle of the screen
        ipText.Render(Hardware.ScreenWidth / 2 - ipText.Width / 2,
            Hardware.ScreenHeight / 2 - ipText.Height / 2);
        Hardware.UpdateScreen();
    }
}
