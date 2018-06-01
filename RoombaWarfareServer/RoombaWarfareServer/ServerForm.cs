using System;
using System.Windows.Forms;
using System.IO;

namespace RoombaWarfareServer
{
    public partial class ServerForm : Form
    {
        private delegate void SetStopButtonCallback(bool isVisible);
        private delegate void SetStartButtonCallback(bool isEnabled);

        private ServerSettingsForm settings;
        private Server server;
        private string mapPath;

        public ServerForm()
        {
            InitializeComponent();
            settings = new ServerSettingsForm();
            server = new Server();
            server.OnCloseEvent += CloseServer;
            mapPath = "";

            //Tries to load the default map
            try
            {
                if(!File.Exists(@"Maps\default.map"))
                    MessageBox.Show("Default map not found", "Warning"
                        , MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    if (Map.ValidateMap(@"Maps\default.map"))
                    {
                        mapPath = @"Maps\default.map";
                    }
                    else
                    {
                        MessageBox.Show("Default map is not valid", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                SetStartButton(false);
                SetStopButton(true);

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
            server.Close(CloseType.ForceClose);
        }

        private void SetStopButton(bool isVisible)
        {
            if (btnStop.InvokeRequired)
            {
                SetStopButtonCallback callback =
                    new SetStopButtonCallback(SetStopButton);
                this.Invoke(callback, new object[] { isVisible });
            }
            else
            {
                btnStop.Visible = isVisible;
            }
        }

        private void SetStartButton(bool isEnabled)
        {
            if (btnStart.InvokeRequired)
            {
                SetStartButtonCallback callback = 
                    new SetStartButtonCallback(SetStartButton);
                this.Invoke(callback, new object[] { isEnabled });
            }
            else
            {
                btnStart.Enabled = isEnabled;
            }
        }

        private void CloseServer()
        {
            SetStopButton(false);
            SetStartButton(true);
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
        private void toolStripOptions_Click(object sender, EventArgs e)
        {
            settings.ShowDialog();
        }
    }
}
