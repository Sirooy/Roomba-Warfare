namespace RoombaWarfareServer
{
    partial class ServerSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtbPort = new System.Windows.Forms.TextBox();
            this.btnGeneratePort = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericNumRounds = new System.Windows.Forms.NumericUpDown();
            this.lblRounds = new System.Windows.Forms.Label();
            this.grpbPort = new System.Windows.Forms.GroupBox();
            this.cmbTickRate = new System.Windows.Forms.ComboBox();
            this.lblTickRate = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
            this.lblMaxPlayers = new System.Windows.Forms.Label();
            this.cmbMaxPlayers = new System.Windows.Forms.ComboBox();
            this.grpbOptions = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericNumRounds)).BeginInit();
            this.grpbPort.SuspendLayout();
            this.grpbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtbPort
            // 
            this.txtbPort.Location = new System.Drawing.Point(51, 19);
            this.txtbPort.Name = "txtbPort";
            this.txtbPort.Size = new System.Drawing.Size(100, 20);
            this.txtbPort.TabIndex = 0;
            this.txtbPort.Text = "23000";
            this.txtbPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnGeneratePort
            // 
            this.btnGeneratePort.Location = new System.Drawing.Point(51, 45);
            this.btnGeneratePort.Name = "btnGeneratePort";
            this.btnGeneratePort.Size = new System.Drawing.Size(100, 23);
            this.btnGeneratePort.TabIndex = 1;
            this.btnGeneratePort.Text = "Generate port";
            this.btnGeneratePort.UseVisualStyleBackColor = true;
            this.btnGeneratePort.Click += new System.EventHandler(this.btnGeneratePort_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(14, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port:";
            // 
            // numericNumRounds
            // 
            this.numericNumRounds.Location = new System.Drawing.Point(106, 73);
            this.numericNumRounds.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericNumRounds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericNumRounds.Name = "numericNumRounds";
            this.numericNumRounds.Size = new System.Drawing.Size(45, 20);
            this.numericNumRounds.TabIndex = 3;
            this.numericNumRounds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericNumRounds.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // lblRounds
            // 
            this.lblRounds.AutoSize = true;
            this.lblRounds.Location = new System.Drawing.Point(6, 75);
            this.lblRounds.Name = "lblRounds";
            this.lblRounds.Size = new System.Drawing.Size(94, 13);
            this.lblRounds.TabIndex = 4;
            this.lblRounds.Text = "Number of rounds:";
            // 
            // grpbPort
            // 
            this.grpbPort.Controls.Add(this.txtbPort);
            this.grpbPort.Controls.Add(this.btnGeneratePort);
            this.grpbPort.Controls.Add(this.label1);
            this.grpbPort.Location = new System.Drawing.Point(12, 12);
            this.grpbPort.Name = "grpbPort";
            this.grpbPort.Size = new System.Drawing.Size(157, 82);
            this.grpbPort.TabIndex = 5;
            this.grpbPort.TabStop = false;
            this.grpbPort.Text = "Server port";
            // 
            // cmbTickRate
            // 
            this.cmbTickRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTickRate.FormattingEnabled = true;
            this.cmbTickRate.IntegralHeight = false;
            this.cmbTickRate.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40"});
            this.cmbTickRate.Location = new System.Drawing.Point(106, 46);
            this.cmbTickRate.Name = "cmbTickRate";
            this.cmbTickRate.Size = new System.Drawing.Size(45, 21);
            this.cmbTickRate.TabIndex = 6;
            // 
            // lblTickRate
            // 
            this.lblTickRate.AutoSize = true;
            this.lblTickRate.Location = new System.Drawing.Point(43, 49);
            this.lblTickRate.Name = "lblTickRate";
            this.lblTickRate.Size = new System.Drawing.Size(57, 13);
            this.lblTickRate.TabIndex = 7;
            this.lblTickRate.Text = "Tick-Rate:";
            // 
            // btnDone
            // 
            this.btnDone.BackColor = System.Drawing.Color.PaleGreen;
            this.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDone.Location = new System.Drawing.Point(175, 18);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(45, 76);
            this.btnDone.TabIndex = 8;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = false;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // lblMaxPlayers
            // 
            this.lblMaxPlayers.AutoSize = true;
            this.lblMaxPlayers.Location = new System.Drawing.Point(34, 22);
            this.lblMaxPlayers.Name = "lblMaxPlayers";
            this.lblMaxPlayers.Size = new System.Drawing.Size(66, 13);
            this.lblMaxPlayers.TabIndex = 9;
            this.lblMaxPlayers.Text = "Max players:";
            // 
            // cmbMaxPlayers
            // 
            this.cmbMaxPlayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMaxPlayers.FormattingEnabled = true;
            this.cmbMaxPlayers.Items.AddRange(new object[] {
            "2",
            "4",
            "6",
            "8",
            "10",
            "Limitless"});
            this.cmbMaxPlayers.Location = new System.Drawing.Point(106, 19);
            this.cmbMaxPlayers.Name = "cmbMaxPlayers";
            this.cmbMaxPlayers.Size = new System.Drawing.Size(66, 21);
            this.cmbMaxPlayers.TabIndex = 10;
            // 
            // grpbOptions
            // 
            this.grpbOptions.Controls.Add(this.cmbMaxPlayers);
            this.grpbOptions.Controls.Add(this.lblMaxPlayers);
            this.grpbOptions.Controls.Add(this.lblTickRate);
            this.grpbOptions.Controls.Add(this.lblRounds);
            this.grpbOptions.Controls.Add(this.cmbTickRate);
            this.grpbOptions.Controls.Add(this.numericNumRounds);
            this.grpbOptions.Location = new System.Drawing.Point(12, 100);
            this.grpbOptions.Name = "grpbOptions";
            this.grpbOptions.Size = new System.Drawing.Size(208, 101);
            this.grpbOptions.TabIndex = 11;
            this.grpbOptions.TabStop = false;
            // 
            // ServerSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 208);
            this.Controls.Add(this.grpbOptions);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.grpbPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ServerSettingsForm";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerSettingsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericNumRounds)).EndInit();
            this.grpbPort.ResumeLayout(false);
            this.grpbPort.PerformLayout();
            this.grpbOptions.ResumeLayout(false);
            this.grpbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtbPort;
        private System.Windows.Forms.Button btnGeneratePort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericNumRounds;
        private System.Windows.Forms.Label lblRounds;
        private System.Windows.Forms.GroupBox grpbPort;
        private System.Windows.Forms.ComboBox cmbTickRate;
        private System.Windows.Forms.Label lblTickRate;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Label lblMaxPlayers;
        private System.Windows.Forms.ComboBox cmbMaxPlayers;
        private System.Windows.Forms.GroupBox grpbOptions;
    }
}