
using System.Windows.Forms;

namespace RoombaWarfareServer
{
    public partial class ServerForm : Form
    {
        private ServerSettingsForm options;
        private Server server;
        private string mapPath;

        public ServerForm()
        {
            InitializeComponent();
            options = new ServerSettingsForm();
            mapPath = "";
        }
        
        private void toolStripExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        //Starts the server
        private void btnStart_Click(object sender, System.EventArgs e)
        {
            btnStop.Visible = true;
            btnStart.Enabled = false;
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

        private void toolStripOptions_Click(object sender, System.EventArgs e)
        {
            options.ShowDialog();
        }
    }
}
