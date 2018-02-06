using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Denso_HHT
{
    public partial class SendAndReceiveDataMenu : Form
    {
        private SendDataMode mode;

        public SendAndReceiveDataMenu(SendDataMode mode)
        {
            InitializeComponent();

            this.KeyPreview = true;

            this.mode = mode;

            if (this.mode == SendDataMode.Offline)
            {
                constMenu.Text += " (Cable)";
            }
            else
            {
                constMenu.Text += " (WIFI)";
            }
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

        private void btnReceiveData_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SendAndReceiveDataResult sendData = new SendAndReceiveDataResult(mode);
            sendData.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SendAndReceiveDataProcess sendData = new SendAndReceiveDataProcess(mode, SendFTPMode.All);
            sendData.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnOnlyNew_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SendAndReceiveDataProcess sendData = new SendAndReceiveDataProcess(mode, SendFTPMode.OnlyNew);
            sendData.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void SendAndReceiveDataMenu_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Escape:
                    btnExit_Click(null, null);
                    break;
            }
        }
    }
}