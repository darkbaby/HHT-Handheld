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
    public partial class ScanProductMenu : Form
    {
        public ScanProductMenu()
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
            ScanProduct ScanProduct = new ScanProduct(ScanProductMode.ScanQty);
            ScanProduct.ShowDialog();
            if (ScanProduct.DialogResult == DialogResult.Abort)
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
            ScanProduct ScanProduct = new ScanProduct(ScanProductMode.ScanOnly);
            ScanProduct.ShowDialog();
            if (ScanProduct.DialogResult == DialogResult.Abort)
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
            ScanProduct ScanProductPack = new ScanProduct(ScanProductMode.ScanPackOnly);
            ScanProductPack.ShowDialog();
            if (ScanProductPack.DialogResult == DialogResult.Abort)
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
            ScanProduct ScanProductPack = new ScanProduct(ScanProductMode.ScanPackQty);
            ScanProductPack.ShowDialog();

            if (ScanProductPack.DialogResult == DialogResult.Abort)
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

        private void ScanProductMenu_KeyDown(object sender, KeyEventArgs e)
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