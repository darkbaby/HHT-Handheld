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
    public partial class DeleteMaster : Form
    {
        public DeleteMaster()
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

        private void btnDeleteLocation_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete all Location data?",
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                DatabaseModule.Instance.QueryDeleteMasterFromDeletion(0);
                MessageBox.Show("Deleted [Location data] Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnDeleteSKU_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete all SKU data?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                DatabaseModule.Instance.QueryDeleteMasterFromDeletion(1);
                MessageBox.Show("Deleted [SKU data] Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void DeleteMaster_KeyDown(object sender, KeyEventArgs e)
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