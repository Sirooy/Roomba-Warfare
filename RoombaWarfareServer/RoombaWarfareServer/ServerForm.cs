
using System.Windows.Forms;

namespace RoombaWarfareServer
{
    public partial class ServerForm : Form
    {
        private ServerSettingsForm settings;
        private Server server;
        private string mapPath;

        public ServerForm()
        {
            InitializeComponent();
            settings = new ServerSettingsForm();
            server = new Server();
            mapPath = "";
        }
        
        private void toolStripExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        //Starts the server
        private void btnStart_Click(object sender, System.EventArgs e)
        {
            if(mapPath != "")
            {
                btnStop.Visible = true;
                btnStart.Enabled = false;

                server.MapPath = mapPath;
                server.Port = settings.Port;
                if (settings.MaxPlayers != "Limitless")
                    server.MaxPlayers = ushort.Parse(settings.MaxPlayers);
                else
                    server.MaxPlayers = 0;
                server.Rounds = settings.NumRounds;
                server.TickRate = settings.TickRate;

                if (!server.Start())
                {
                    MessageBox.Show("Couldn't start the server", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Map not loaded", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Stops the server
        private void btnStop_Click(object sender, System.EventArgs e)
        {
            btnStop.Visible = false;
            btnStart.Enabled = true;
        }

        //Get the map path and checks if it is valid
        private void toolStripLoadMap_Click(object sender, System.EventArgs e)
        {
            if(openFileMap.ShowDialog() == DialogResult.OK)
            {
                mapPath = openFileMap.FileName;

                if (!Map.ValidateMap(mapPath))
                {
                    mapPath = "";
                    MessageBox.Show("Invalid map format", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Opens the settings 
        private void toolStripOptions_Click(object sender, System.EventArgs e)
        {
            settings.ShowDialog();
        }
    }
}
