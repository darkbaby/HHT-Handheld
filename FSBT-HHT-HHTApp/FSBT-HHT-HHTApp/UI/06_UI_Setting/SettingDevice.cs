using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml;
using System.IO;
using System.Reflection;
using Denso_HHT.Module;

namespace Denso_HHT
{
    public partial class SettingDevice : Form
    {
        //private XmlDocument xmlDocument = new XmlDocument();
        //string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

        public SettingDevice()
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

            //xmlDocument.Load(appPath + "\\config.xml");
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

        private void SettingDeviceName_Load(object sender, EventArgs e)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Ident");
            String deviceName = key.GetValue("Name").ToString();
            tbDeviceName.Text = deviceName;
            tbDeviceName.SelectionStart = tbDeviceName.Text.Length;
            tbDeviceID.Text = DatabaseModule.Instance.HHTID;
        }

        private void FocusOnTextBox(TextBox tb)
        {
            tb.Focus();
            tb.Text = DatabaseModule.Instance.currentDepartmentCode;
            tb.SelectionStart = 0;
            tb.SelectionLength = tb.Text.Length;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbDeviceID.Text))
            {
                MessageBox.Show("Please fill your device id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                FocusOnTextBox(tbDeviceID);
                return;
            }

            if (string.IsNullOrEmpty(tbDeviceName.Text))
            {
                MessageBox.Show("Please fill your device name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                FocusOnTextBox(tbDeviceName);
                return;
            }

            RegistryKey key = Registry.LocalMachine.OpenSubKey("Ident", true);
            String newDeviceName = this.tbDeviceName.Text;
            key.SetValue("Name", newDeviceName);
            DatabaseModule.Instance.QueryUpdateSetting("HHTName", newDeviceName);
            String newHHTID = tbDeviceID.Text.PadLeft(3, '0');
            DatabaseModule.Instance.QueryUpdateSetting("HHTID", newHHTID);
            //xmlDocument.SelectSingleNode("//SharedPreferences/DeviceProperties/DeviceName").InnerText = newDeviceName;
            //xmlDocument.Save(appPath + "\\config.xml");
            MessageBox.Show("Setting device id [" + newHHTID + "], device name [" + newDeviceName + "] successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void tbDeviceID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void SettingDevice_KeyDown(object sender, KeyEventArgs e)
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