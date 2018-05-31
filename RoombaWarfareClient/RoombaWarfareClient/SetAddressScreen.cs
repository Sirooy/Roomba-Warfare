using System.Text.RegularExpressions;

//This screens allows the user to change the ip-port of the server
public class SetAddressScreen : IScreen
{
    private static Image background;
    private static Font font;
    private static Text validIPText;
    private static Text invalidIPText;

    private Keyboard keyboard;

    public ScreenType NextScreen { get; set; }

    static SetAddressScreen()
    {
        font = new Font(@"resources\fonts\RWFont.ttf", 25);
        background = new Image
            (Game.LanguageTranslation[Game.GameLanguage + "SetAddressPath"], 
            800, 600);
        validIPText =
            new Text(font,Game.LanguageTranslation[Game.GameLanguage + "VIP"]
            , 0x00, 0xFF, 0x00);
        invalidIPText =
            new Text(font, Game.LanguageTranslation[Game.GameLanguage + "IIP"]
            , 0xFF, 0x00, 0x00);
    }

    public SetAddressScreen()
    {
        NextScreen = ScreenType.None;
        keyboard = new Keyboard(Game.ServerAddress);
        keyboard.OnSummitEvent += OnSummit;
        keyboard.OnTextChangedEvent += RenderTextChanged;
    }

    public ScreenType Run()
    {
        //Draw the current ip
        RenderTextChanged(Game.ServerAddress);

        do
        {
            keyboard.HandleEvents();
            System.Threading.Thread.Sleep(16);
        } while (NextScreen == ScreenType.None);

        return NextScreen;
    }

    //If the ip is valid, saves it and goes to the next screen
    public void OnSummit(string address)
    {
        if (ValidateIPAddress(address))
        {
            Game.ServerAddress = address;
            NextScreen = ScreenType.Main;
        }
    }

    //Checks if the ip entered is valid
    public bool ValidateIPAddress(string address)
    {
        if (!Regex.IsMatch(address,
            @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\-\d{3,5}$"))
        {
            return false;
        }
        else
        {
            string[] ipParts = address.Split('-');
            string port = ipParts[1];

            if (!ushort.TryParse(port, out ushort result))
            {
                return false;
            }
            if (result <= 1024)
                return false;

            string[] ipNums = ipParts[0].Split('.');

            if (!byte.TryParse(ipNums[0], out byte n1) ||
                !byte.TryParse(ipNums[1], out byte n2) ||
                !byte.TryParse(ipNums[2], out byte n3) ||
                !byte.TryParse(ipNums[3], out byte n4))
            {
                return false;
            }
        }

        return true;
    }

    //Whenever the text changes, we render the new text on screen
    //and check if the ip is valid
    public void RenderTextChanged(string address)
    {
        Text ipText = new Text(font, address, 0xff, 0xff, 0xff);

        Hardware.ClearScreen();
        Hardware.RenderBackground(background);
        //Renders the text on the middle of the screen
        ipText.Render(Hardware.ScreenWidth / 2 - ipText.Width / 2,
            Hardware.ScreenHeight / 2 - ipText.Height / 2);
        if (ValidateIPAddress(address))
        {
            validIPText.Render
                (Hardware.ScreenWidth / 2 - validIPText.Width / 2,
                Hardware.ScreenHeight / 2 + validIPText.Height);
        }
        else
        {
            invalidIPText.Render
                (Hardware.ScreenWidth / 2 - invalidIPText.Width / 2,
                Hardware.ScreenHeight / 2 + invalidIPText.Height);
        }
        Hardware.UpdateScreen();
    }
}
