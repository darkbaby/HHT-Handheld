using Microsoft.Win32;
using Denso_HHT.Module;
using System.IO;
using System.Reflection;
using System;
namespace Denso_HHT
{
    partial class SendAndReceiveData
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
            if (sendDataMode == SendDataMode.Online)
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Comm\FTPD", true);
                key.SetValue("AllowAnonymous", 0);
            }
            timerCheckAS.Enabled = false;
            timerCheckAS.Dispose();

            FileInfo f = new FileInfo(path + @"\Database\STOCKTAKING_HHT.sdf");
            if (firstLastSize != f.Length)
            {
                DatabaseModule.Instance.Init();
            }
            else
            {
                DatabaseModule.Instance.InitWithOutRefresh();
            }

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
            this.const1 = new System.Windows.Forms.Label();
            this.lbIPAddress = new System.Windows.Forms.Label();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.constMenu = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.timerCheckAS = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // const1
            // 
            this.const1.Location = new System.Drawing.Point(0, 80);
            this.const1.Name = "const1";
            this.const1.Size = new System.Drawing.Size(240, 20);
            this.const1.Text = "IP Address";
            this.const1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbIPAddress
            // 
            this.lbIPAddress.Location = new System.Drawing.Point(0, 110);
            this.lbIPAddress.Name = "lbIPAddress";
            this.lbIPAddress.Size = new System.Drawing.Size(240, 20);
            this.lbIPAddress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 0;
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(134, 38);
            this.constMenu.Text = "Syncing Data (WIFI)";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(80, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // timerCheckAS
            // 
            this.timerCheckAS.Interval = 3000;
            this.timerCheckAS.Tick += new System.EventHandler(this.timerCheckAS_Tick);
            // 
            // SendAndReceiveData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 300);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.Controls.Add(this.lbIPAddress);
            this.Controls.Add(this.const1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendAndReceiveData";
            this.Text = "Send Data";
            this.Load += new System.EventHandler(this.SendData_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label const1;
        private System.Windows.Forms.Label lbIPAddress;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label constMenu;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Timer timerCheckAS;
    }
}