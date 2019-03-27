using System;
using System.Windows.Forms;
using Denso_HHT.Module;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace Denso_HHT
{
    public partial class Login : Form
    {
        public Login()
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

        private void Login_Load(object sender, EventArgs e)
        {
            tbUsername.Text = DatabaseModule.Instance.currentUser;
            tbUsername.SelectionStart = 0;
            tbUsername.SelectionLength = tbUsername.Text.Length;
            tbDepartmentCode.Text = DatabaseModule.Instance.currentDepartmentCode;
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

        private void FocusOnTextBox(TextBox tb)
        {
            tb.Focus();
            tb.SelectionStart = 0;
            tb.SelectionLength = tb.Text.Length;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            if (tbUsername.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please fill username in the box", "Warning");
                SetEnableComponent(true);
                FocusOnTextBox(tbUsername);
            }
            //else if (tbDepartmentCode.Text.Trim().Equals(""))
            //{
            //    MessageBox.Show("Please fill department code in the box", "Warning");
            //    SetEnableComponent(true);
            //    FocusOnTextBox(tbDepartmentCode);
            //}
            else
            {
                DatabaseModule.Instance.QueryUpdateSetting("Username", tbUsername.Text.Trim());
                //DatabaseModule.Instance.QueryUpdateSetting("DepartmentCode", tbDepartmentCode.Text.Trim());
                if (radioBtnRealtime.Checked)
                {
                    Program.isNonRealtime = false;
                    APIModule.Instance.Init();
                }

                this.DialogResult = DialogResult.OK;
                this.Dispose();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (File.Exists(path + @"\Record.txt"))
            {
                File.Delete(path + @"\Record.txt");
            }
            if (File.Exists(path + @"\temp.dat"))
            {
                File.Delete(path + @"\temp.dat");
            }
            if (File.Exists(path + @"\data.zip"))
            {
                File.Delete(path + @"\data.zip");
            }

            this.DialogResult = DialogResult.Abort;
            this.Dispose();
        }

        private void tbDepartmentCode_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Login_KeyDown(object sender, KeyEventArgs e)
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