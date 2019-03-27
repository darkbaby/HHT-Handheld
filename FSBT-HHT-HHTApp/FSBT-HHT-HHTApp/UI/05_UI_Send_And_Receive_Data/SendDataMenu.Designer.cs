using Denso_HHT.Module;
namespace Denso_HHT
{
    partial class SendDataMenu
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
            this.btnSyncCable = new System.Windows.Forms.Button();
            this.btnSendWifi = new System.Windows.Forms.Button();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.constMenu = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSyncWifi = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSyncCable
            // 
            this.btnSyncCable.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnSyncCable.Location = new System.Drawing.Point(40, 169);
            this.btnSyncCable.Name = "btnSyncCable";
            this.btnSyncCable.Size = new System.Drawing.Size(160, 40);
            this.btnSyncCable.TabIndex = 7;
            this.btnSyncCable.Text = "Sync (Cable)";
            this.btnSyncCable.Click += new System.EventHandler(this.btnSyncCable_Click);
            // 
            // btnSendWifi
            // 
            this.btnSendWifi.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnSendWifi.Location = new System.Drawing.Point(40, 50);
            this.btnSendWifi.Name = "btnSendWifi";
            this.btnSendWifi.Size = new System.Drawing.Size(160, 40);
            this.btnSendWifi.TabIndex = 5;
            this.btnSendWifi.Text = "Send (WIFI)";
            this.btnSendWifi.Visible = false;
            this.btnSendWifi.Click += new System.EventHandler(this.btnSendWifi_Click);
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 100;
            this.hhtToolBar1.TabStop = false;
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(134, 35);
            this.constMenu.Text = "Send And Receive Data Menu";
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnExit.Location = new System.Drawing.Point(0, 280);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(240, 40);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSyncWifi
            // 
            this.btnSyncWifi.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnSyncWifi.Location = new System.Drawing.Point(40, 89);
            this.btnSyncWifi.Name = "btnSyncWifi";
            this.btnSyncWifi.Size = new System.Drawing.Size(160, 40);
            this.btnSyncWifi.TabIndex = 6;
            this.btnSyncWifi.Text = "Sync (WIFI)";
            this.btnSyncWifi.Click += new System.EventHandler(this.btnSyncWifi_Click);
            // 
            // SendDataMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.btnSyncWifi);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.Controls.Add(this.btnSyncCable);
            this.Controls.Add(this.btnSendWifi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendDataMenu";
            this.Text = "Send Data Menu";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendDataMenu_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSyncCable;
        private System.Windows.Forms.Button btnSendWifi;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label constMenu;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSyncWifi;
    }
}