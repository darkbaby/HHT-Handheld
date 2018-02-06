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
    public partial class SettingFTP : Form
    {
        public SettingFTP()
        {
            InitializeComponent();

            this.KeyPreview = true;

            if (DNWA.BHTCL.Keys.Settings.FuncMode == DNWA.BHTCL.Keys.Settings.EN_FUNC_MODE.FUNCTION)
            {
                lbFunction.Visible = true;
            }
            else
            {
                lbFunction.Visible = false;
            }

            if (DNWA.BHTCL.Keys.Settings.InputMethod == DNWA.BHTCL.Keys.Settings.EN_INPUT_METHOD.ALPHABET)
            {
                lbAlpha.Visible = true;
            }
            else
            {
                lbAlpha.Visible = false;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);

            string newFTPServer = tbServer.Text;
            string newFTPUsername = tbUsername.Text;
            string newFTPPassword = tbPassword.Text;

            //DatabaseModule.Instance.QueryUpdateSetting("FTPServer", "192.168.10.84");
            //DatabaseModule.Instance.QueryUpdateSetting("FTPUsername", "admin");
            //DatabaseModule.Instance.QueryUpdateSetting("FTPPassword", "admin");

            DatabaseModule.Instance.QueryUpdateSetting("FTPServer", newFTPServer);
            DatabaseModule.Instance.QueryUpdateSetting("FTPUsername", newFTPUsername);
            DatabaseModule.Instance.QueryUpdateSetting("FTPPassword", newFTPPassword);
            MessageBox.Show("Setting FTP Server [" + newFTPServer + "], FTP Username [" + newFTPUsername + "], FTP Password [" + newFTPPassword + "] successfully",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            //xmlDocument.SelectSingleNode("//SharedPreferences/DeviceProperties/DeviceName").InnerText = newDeviceName;
            //xmlDocument.Save(appPath + "\\config.xml");
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void SettingFTP_Load(object sender, EventArgs e)
        {
            tbServer.Text = DatabaseModule.Instance.FtpServer;
            tbUsername.Text = DatabaseModule.Instance.FtpUsername;
            tbPassword.Text = DatabaseModule.Instance.FtpPassword;
        }

        private void SettingFTP_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case DNWA.BHTCL.Keys.FUNC:
                    lbFunction.Visible = !lbFunction.Visible;
                    break;
                case DNWA.BHTCL.Keys.ALP:
                    lbAlpha.Visible = !lbAlpha.Visible;
                    break;
            }
        }
    }
}