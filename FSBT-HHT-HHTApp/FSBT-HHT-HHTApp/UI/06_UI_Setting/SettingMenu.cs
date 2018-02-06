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
    public partial class SettingMenu : Form
    {
        public SettingMenu()
        {
            InitializeComponent();
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

        private void btnDeviceName_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SettingDevice settingDeviceName = new SettingDevice();
            settingDeviceName.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnDateTime_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SettingDateTime settingDateTime = new SettingDateTime();
            settingDateTime.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SettingDepartment settingDepartment = new SettingDepartment();
            settingDepartment.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnComputer_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SettingUploadComputer settingUploadComputer = new SettingUploadComputer();
            DialogResult result = settingUploadComputer.ShowDialog();
            if (result == DialogResult.Abort)
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
            SetEnableComponent(false);
            this.Dispose();
        }

        private void btnFTP_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SettingFTP settingFTP = new SettingFTP();
            settingFTP.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }
    }
}