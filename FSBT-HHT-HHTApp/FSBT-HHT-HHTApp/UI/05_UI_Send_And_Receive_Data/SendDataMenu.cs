using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Denso_HHT.Module;

namespace Denso_HHT
{
    public partial class SendDataMenu : Form
    {
        public SendDataMenu()
        {
            InitializeComponent();

            this.KeyPreview = true;
        }

        private void SetEnableComponent(bool value)
        {
            foreach (Control item in this.Controls)
            {
                if (item.GetType() == typeof(Button))
                {
                    item.Enabled = value;
                }
            }
        }

        private void btnSendWifi_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SendDataFTPMenu sendDataFTPMenu = new SendDataFTPMenu();
            sendDataFTPMenu.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnSyncWifi_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SendAndReceiveDataMenu sendData = new SendAndReceiveDataMenu(SendDataMode.Online);
            sendData.ShowDialog();

            if (sendData.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            }
        }

        private void btnSyncCable_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SendAndReceiveDataMenu sendData = new SendAndReceiveDataMenu(SendDataMode.Offline);
            sendData.ShowDialog();

            if (sendData.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DatabaseModule.Instance.Refresh();
            SetEnableComponent(false);
            this.Dispose();
        }

        private void SendDataMenu_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode){
                case System.Windows.Forms.Keys.Escape:
                    btnExit_Click(null, null);
                    break;
            }
        }
    }
}