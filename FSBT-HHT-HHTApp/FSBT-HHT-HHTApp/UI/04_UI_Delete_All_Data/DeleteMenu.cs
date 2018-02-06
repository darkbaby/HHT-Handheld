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
    public partial class DeleteMenu : Form
    {
        public DeleteMenu()
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

        private void btnDeleteMaster_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            DeleteMaster deleteMaster = new DeleteMaster();
            deleteMaster.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnDeleteAudit_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            DeleteAudit deleteAudit = new DeleteAudit();
            deleteAudit.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to restore your database", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                DialogResult result2 = MessageBox.Show("The program will close immediately after your confirmation, do you still insist to restore?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result2 == DialogResult.Yes)
                {
                    DatabaseModule.Instance.QueryDeleteRestoreFromDeletion();
                    this.DialogResult = DialogResult.Abort;
                    this.Dispose();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void DeleteMenu_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Escape:
                    btnExit_Click(null, null);
                    break;
            }
        }
    }
}