using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Denso_HHT.Module;

namespace Denso_HHT
{
    public partial class SettingDateTime : Form
    {
        private DateTime dtValue;
        private DateTime original_dtValue;

        public SettingDateTime()
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
            if (original_dtValue.Equals(dtValue))
            {
                this.Dispose();
                return;
            }

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            String formatDate = String.Format("{0:MM-dd-yyyy}", this.dtValue);
            String formatTime = String.Format("{0:HH:mm:ss}", this.dtValue);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = "/c date " + formatDate;
            process.Start();
            process.StartInfo.Arguments = "/c time " + formatTime;
            process.Start();
            process.Close();
            DatabaseModule.Instance.QueryUpdateDateTime(this.dtValue);
            MessageBox.Show("Setting " + formatDate + " " + formatTime + " successfully", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            this.Dispose();
        }

        private void SettingDateTime_Load(object sender, EventArgs e)
        {
            this.original_dtValue = this.dtpDateTime.Value;
            this.dtValue = this.original_dtValue;
        }

        private void dtpDateTime_ValueChanged(object sender, EventArgs e)
        {
            this.dtValue = this.dtpDateTime.Value;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void SettingDateTime_KeyDown(object sender, KeyEventArgs e)
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