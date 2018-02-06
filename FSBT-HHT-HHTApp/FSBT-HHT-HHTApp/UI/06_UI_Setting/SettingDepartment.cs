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
    public partial class SettingDepartment : Form
    {
        public SettingDepartment()
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

        private void SettingDepartment_Load(object sender, EventArgs e)
        {
            tbDepartmentCode.Text = DatabaseModule.Instance.currentDepartmentCode;
            tbDepartmentCode.SelectionStart = 0;
            tbDepartmentCode.SelectionLength = tbDepartmentCode.Text.Length;
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

        private void FocusOnTextBox(TextBox tb)
        {
            tb.Focus();
            tb.Text = DatabaseModule.Instance.currentDepartmentCode;
            tb.SelectionStart = 0;
            tb.SelectionLength = tb.Text.Length;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbDepartmentCode.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please fill department code in the box", "Warning");
                FocusOnTextBox(tbDepartmentCode);
            }
            else
            {
                DatabaseModule.Instance.QueryUpdateSetting("DepartmentCode", tbDepartmentCode.Text.Trim());
                MessageBox.Show("Setting Department code [" + tbDepartmentCode.Text + "] successfully",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                this.Dispose();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void SettingDepartment_KeyDown(object sender, KeyEventArgs e)
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