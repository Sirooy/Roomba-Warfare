using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoombaWarfareServer
{
    public partial class ServerSettingsForm : Form
    {
        public ushort Port { get; set; }
        public byte TickRate { get; set; }
        public string MaxPlayers { get; set; }
        public ushort NumRounds { get; set; }

        public ServerSettingsForm()
        {
            InitializeComponent();
            cmbTickRate.Text = "20";
            cmbMaxPlayers.Text = "8";
        }

        private void btnGeneratePort_Click(object sender, EventArgs e)
        {
            Random rndPort = new Random();
            txtbPort.Text = rndPort.Next(2000, ushort.MaxValue).ToString();
        }

        public bool IsValidPort()
        {
            return ushort.TryParse(txtbPort.Text, out ushort port);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (!IsValidPort())
            {
                MessageBox.Show("Invalid port", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Close();
            }
        }

        private void ServerSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsValidPort())
            {
                MessageBox.Show("Port set to 23000","Invalid port"
                    ,MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                txtbPort.Text = "23000";
                Port = 23000;
            }
            else
            {
                Port = ushort.Parse(txtbPort.Text);
            }
        }
    }
}
