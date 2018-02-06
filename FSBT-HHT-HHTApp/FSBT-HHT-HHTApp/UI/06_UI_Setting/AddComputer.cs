using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using Denso_HHT.Module;

namespace Denso_HHT
{
    public partial class AddComputer : Form
    {
        public AddComputer()
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
            if (tbComputerName.Text == null || tbComputerName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please fill the computer name", "Warning");
            }
            else
            {
                DatabaseModule.Instance.QueryInsertComputerNameFromSetting(tbComputerName.Text);
                MessageBox.Show("Computer Name Added","Success");
                this.DialogResult = DialogResult.OK;
                this.Dispose();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.DialogResult = DialogResult.Abort;
            this.Dispose();
        }

        private void AddComputer_KeyDown(object sender, KeyEventArgs e)
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