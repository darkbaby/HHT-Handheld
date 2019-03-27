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
    public partial class SettingPassword : Form
    {
        public SettingPassword()
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

            string newPassword = tbPassword.Text;

            DatabaseModule.Instance.QueryUpdateSetting("Password", newPassword);
            MessageBox.Show("Setting Password Server successfully",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void SettingPassword_Load(object sender, EventArgs e)
        {
            tbPassword.Text = DatabaseModule.Instance.Password;
        }

        private void SettingPassword_KeyDown(object sender, KeyEventArgs e)
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