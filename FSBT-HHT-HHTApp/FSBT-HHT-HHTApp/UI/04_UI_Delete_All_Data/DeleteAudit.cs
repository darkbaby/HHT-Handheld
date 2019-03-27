using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Data.SqlServerCe;
using Denso_HHT.Module;

namespace Denso_HHT
{
    public partial class DeleteAudit : Form
    {
        public DeleteAudit()
        {
            InitializeComponent();

            this.KeyPreview = true;
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

        private void DeleteAllData_Load(object sender, EventArgs e)
        {
            List<string[]> resultSet = DatabaseModule.Instance.QuerySelectAuditFromDeletion();
            if (resultSet.Count > 0)
            {
                //cmd = new SqlCeCommand("SELECT Location, COUNT(*) RecordNumber FROM HHTSync GROUP By Location", cn);
                //cmd.CommandType = CommandType.Text;
                //resultSet = cmd.ExecuteResultSet(ResultSetOptions.Scrollable);
                //resultSet.ReadFirst();
                //lbLocationData.Text += resultSet.GetString(0) + " || " + resultSet.GetInt32(1);
                //lbLocationData.Text += Environment.NewLine;
                Label newLabelH1 = new Label();
                Label newLabelH2 = new Label();
                Label newLabelH3 = new Label();
                panel1.Controls.Add(newLabelH1);
                panel1.Controls.Add(newLabelH2);
                panel1.Controls.Add(newLabelH3);
                newLabelH1.TextAlign = ContentAlignment.TopCenter;
                newLabelH2.TextAlign = ContentAlignment.TopCenter;
                newLabelH3.TextAlign = ContentAlignment.TopCenter;
                newLabelH1.Size = new Size(90, 30);
                newLabelH2.Size = new Size(20, 30);
                newLabelH3.Size = new Size(60, 30);
                newLabelH1.Location = new Point(0, 0);
                newLabelH2.Location = new Point(90, 0);
                newLabelH3.Location = new Point(110, 0);
                newLabelH1.Text = "Location Code";
                newLabelH2.Text = "||";
                newLabelH3.Text = "Record";

                //bool first = true;
                int i = 1;
                foreach (string[] item in resultSet)
                {
                    Label newLabel1 = new Label();
                    Label newLabel2 = new Label();
                    Label newLabel3 = new Label();
                    panel1.Controls.Add(newLabel1);
                    panel1.Controls.Add(newLabel2);
                    panel1.Controls.Add(newLabel3);

                    newLabel1.TextAlign = ContentAlignment.TopCenter;
                    newLabel2.TextAlign = ContentAlignment.TopCenter;
                    newLabel3.TextAlign = ContentAlignment.TopCenter;

                    newLabel1.Size = new Size(90, 30);
                    newLabel2.Size = new Size(20, 30);
                    newLabel3.Size = new Size(60, 30);


                    newLabel1.Location = new Point(0, 30 * i);
                    newLabel2.Location = new Point(90, 30 * i);
                    newLabel3.Location = new Point(110, 30 * i);

                    i++;

                    newLabel1.Text = item[0];
                    //newLabel1.Text += Environment.NewLine;
                    newLabel2.Text = "||";
                    //newLabel2.Text += Environment.NewLine;
                    newLabel3.Text = item[1];
                    //newLabel3.Text += Environment.NewLine;
                    //lbLocationData.Text += item[0].PadLeft(13,' ') + " || " + item[1].PadLeft(6,' ');
                    //lbLocationData.Text += Environment.NewLine;
                }
            }
            else
            {
                btnDelete.Enabled = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete all audit data?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                DatabaseModule.Instance.QueryDeleteAuditFromDeletion();
                MessageBox.Show("Deleted [Audit data] Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                //lbLocation.Text = "";
                //lbDelimeter.Text = "";
                //lbRecord.Text = "";
                SetEnableComponent(true);
                btnDelete.Enabled = false;
                panel1.Controls.Clear(); 
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void DeleteAudit_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Escape:
                    btnCancel_Click(null, null);
                    break;
            }
        }
    }
}