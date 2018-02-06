using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Denso_HHT
{
    public partial class ScanFreshFoodMenu : Form
    {
        public ScanFreshFoodMenu()
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

        private void btnScanOnly_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanFreshFood scanFreshFood = new ScanFreshFood(ScanFreshFoodMode.ScanOnly);
            scanFreshFood.ShowDialog();
            if (scanFreshFood.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            };
        }

        private void btnScanQty_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanFreshFood scanFreshFood = new ScanFreshFood(ScanFreshFoodMode.ScanQty);
            scanFreshFood.ShowDialog();
            if (scanFreshFood.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            };
        }

        private void btnScanWeight_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanFreshFood scanFreshFood = new ScanFreshFood(ScanFreshFoodMode.ScanWeight);
            scanFreshFood.ShowDialog();
            if (scanFreshFood.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            };
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void ScanFreshFoodMenu_KeyDown(object sender, KeyEventArgs e)
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