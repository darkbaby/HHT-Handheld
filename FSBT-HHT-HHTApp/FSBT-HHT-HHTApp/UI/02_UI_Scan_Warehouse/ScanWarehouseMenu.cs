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
    public partial class ScanWarehouseMenu : Form
    {
        public ScanWarehouseMenu()
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

        private void btnScanQty_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanWarehouse scanWarehouse = new ScanWarehouse(ScanWarehouseMode.ScanQty);
            scanWarehouse.ShowDialog();
            if (scanWarehouse.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            }
        }

        private void btnScanOnly_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanWarehouse scanWarehouse = new ScanWarehouse(ScanWarehouseMode.ScanOnly);
            scanWarehouse.ShowDialog();
            if (scanWarehouse.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            };
        }

        private void btnScanPackOnly_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanWarehouse scanWarehouse = new ScanWarehouse(ScanWarehouseMode.ScanPackOnly);
            scanWarehouse.ShowDialog();
            if (scanWarehouse.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            }
        }

        private void btnScanPackQty_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanWarehouse scanWarehouse = new ScanWarehouse(ScanWarehouseMode.ScanPackQty);
            scanWarehouse.ShowDialog();

            if (scanWarehouse.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void ScanWarehouseMenu_KeyDown(object sender, KeyEventArgs e)
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