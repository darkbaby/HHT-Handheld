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
    public partial class SendDataPassword : Form
    {
        private SendDataMode mode;
        
        public SendDataPassword(SendDataMode mode)
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

            this.mode = mode;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            //SetEnableComponent(false);

            string Password = DatabaseModule.Instance.Password;

            string newPassword = tbPassword.Text;

            if (Password != newPassword)
            {
                MessageBox.Show("Wrong Password, Please try again ", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                tbPassword.Text = "";
                tbPassword.Focus();
            }
            else
            {
                if (mode == SendDataMode.Offline)
                {
                    SetEnableComponent(false);
                    SendAndReceiveDataProcess sendData = new SendAndReceiveDataProcess(mode, SendFTPMode.All);
                    sendData.ShowDialog();
                    SetEnableComponent(false);
                    this.Dispose();
                }
                else
                {
                    SetEnableComponent(false);
                    SendDataFTP d = new SendDataFTP(SendFTPMode.All);
                    d.ShowDialog();
                    SetEnableComponent(false);
                    this.Dispose();
                }
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void SendDataPassword_Load(object sender, EventArgs e)
        {
            //string Password = DatabaseModule.Instance.Password;
            tbPassword.Text = "";
        }

        private void SendDataPassword_KeyDown(object sender, KeyEventArgs e)
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

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:

                    string Password = DatabaseModule.Instance.Password;
                    string newPassword = tbPassword.Text;

                    if (Password != newPassword)
                    {
                        MessageBox.Show("Wrong Password, Please try again ", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                        tbPassword.Text = "";
                        tbPassword.Focus();
                    }
                    else
                    {
                        if (mode == SendDataMode.Offline)
                        {
                            SetEnableComponent(false);
                            SendAndReceiveDataProcess sendData = new SendAndReceiveDataProcess(mode, SendFTPMode.All);
                            sendData.ShowDialog();
                            SetEnableComponent(false);
                            this.Dispose();
                        }
                        else
                        {
                            SetEnableComponent(false);
                            SendDataFTP d = new SendDataFTP(SendFTPMode.All);
                            d.ShowDialog();
                            SetEnableComponent(false);
                            this.Dispose();
                        }

                    }
                    break;
            }

        }
    }
}