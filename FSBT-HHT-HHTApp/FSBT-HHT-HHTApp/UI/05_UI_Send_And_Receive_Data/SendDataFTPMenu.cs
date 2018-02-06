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
    public partial class SendDataFTPMenu : Form
    {
        public SendDataFTPMenu()
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
 
        private void btnAll_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SendDataFTP d = new SendDataFTP(SendFTPMode.All);
            d.ShowDialog();
            //SendData sendData = new SendData(SendDataMode.Online);
            //sendData.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnOnlyNew_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SendDataFTP d = new SendDataFTP(SendFTPMode.OnlyNew);
            d.ShowDialog();
            //SendData sendData = new SendData(SendDataMode.Online);
            //sendData.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void SendDataFTPMenu_KeyDown(object sender, KeyEventArgs e)
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