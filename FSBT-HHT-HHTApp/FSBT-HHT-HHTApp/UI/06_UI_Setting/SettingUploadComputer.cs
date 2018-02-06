using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;
using System.Reflection;
using Denso_HHT.Module;

namespace Denso_HHT
{
    public partial class SettingUploadComputer : Form
    {
        List<RadioButton> radioButtonList = new List<RadioButton>();

        public SettingUploadComputer()
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

        private void RefreshComputerList()
        {
            this.panel1.Controls.Clear();
            radioButtonList.Clear();

            List<string> resultSet = DatabaseModule.Instance.QuerySelectComputerNameFromSetting();

            if (resultSet.Count > 0)
            {
                //query = "SELECT COUNT(*) FROM Computer";
                //cmd = new SqlCeCommand(query, cn);
                //SqlCeResultSet resultSet2 = cmd.ExecuteResultSet(ResultSetOptions.None);
                //resultSet2.Read();
                //int count = resultSet2.GetInt32(0);
                int i = 0;
                foreach (string item in resultSet)
                {
                    string computerName = item;
                    RadioButton rdo = new RadioButton();
                    rdo.Name = "RadioButton" + i;
                    rdo.Text = computerName;
                    rdo.Location = new Point(3, 3 + (20 * i) + (3 * i));
                    rdo.Size = new Size(160, 20);
                    this.panel1.Controls.Add(rdo);
                    radioButtonList.Add(rdo);
                    i++;
                }
            }

        }

        private string FindSelectedRadioButton()
        {
            foreach (RadioButton item in radioButtonList)
            {
                if (item.Checked)
                {
                    return item.Text;
                }
            }

            return "";
        }

        private void SettingUploadComputer_Load(object sender, EventArgs e)
        {
            RefreshComputerList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);

            AddComputer addComputer = new AddComputer();
            addComputer.ShowDialog();

            SetEnableComponent(true);

            if (addComputer.DialogResult == DialogResult.OK)
            {
                this.Show();
                RefreshComputerList();
            }
            else if (addComputer.DialogResult == DialogResult.Cancel)
            {
                this.Show();
            }
            else if (addComputer.DialogResult == DialogResult.Abort)
            {
                this.DialogResult = DialogResult.Abort;
                this.Dispose();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (radioButtonList.Count == 0)
            {
                MessageBox.Show("None computer name to delete", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                SetEnableComponent(true);
                return;
            }

            string targetComputerName = FindSelectedRadioButton();
            if (targetComputerName.Equals(""))
            {
                MessageBox.Show("Please select target", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                SetEnableComponent(true);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure to delete selected computer name?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                DatabaseModule.Instance.QueryDeleteComputerNameFromSetting(FindSelectedRadioButton());
                MessageBox.Show("Deleted Successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                RefreshComputerList();
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
    }
}