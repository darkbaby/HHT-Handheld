using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Runtime.InteropServices;
using Denso_HHT.Module;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using System.Reflection;

namespace Denso_HHT
{
    public partial class MainMenu : Form
    {
        private bool isFirst = true;

        private int countSecret = 0;

        public MainMenu()
        {
            InitializeComponent();

            MakeButtonMultiline(btnSendData);
            btnSendData.Text = "Send And" + Environment.NewLine + "Receive Data";

            DatabaseModule.Instance.Init();
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

            if (!Program.isNonRealtime)
            {
                btnSendData.Enabled = false;
            }
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            if (isFirst)
            {
                Login login = new Login();
                login.ShowDialog();
                if (login.DialogResult == DialogResult.Abort)
                {
                    this.Dispose();
                }
                else
                {
                    if (!Program.isNonRealtime)
                    {
                        btnSendData.Enabled = false;
                        labelMode.Text = "Realtime";
                    }
                    else
                    {
                        labelMode.Text = "Non-Realtime";
                    }
                    isFirst = false;
                }
            }
        }

        private void btnFront_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanFrontMenu scanFrontMenu = new ScanFrontMenu();
            scanFrontMenu.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanWarehouseMenu scanWarehouseMenu = new ScanWarehouseMenu();
            scanWarehouseMenu.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnFreshFood_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanFreshFoodMenu scanFreshFoodMenu = new ScanFreshFoodMenu();
            scanFreshFoodMenu.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            ScanProductMenu scanProductMenu = new ScanProductMenu();
            scanProductMenu.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SendDataMenu sendDataMenu = new SendDataMenu();
            sendDataMenu.ShowDialog();
            SetEnableComponent(true);
            this.Show();
        }

        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            DeleteMenu deleteMenu = new DeleteMenu();
            deleteMenu.ShowDialog();

            if (deleteMenu.DialogResult == DialogResult.Abort)
            {
                this.Dispose();
            }
            else
            {
                SetEnableComponent(true);
                this.Show();
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            SettingMenu settingMenu = new SettingMenu();
            settingMenu.ShowDialog();
            SetEnableComponent(true);
            this.Show();
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

            this.Dispose();
        }

        private void Alert()
        {
            MessageBox.Show("table location : " + DatabaseModule.Instance.QuerySelectCountFromBackdoor("tb_m_Location").ToString());
            MessageBox.Show("table sku : " + DatabaseModule.Instance.QuerySelectCountFromBackdoor("tb_m_SKU").ToString());
        }

        private void panelSecret_Click(object sender, EventArgs e)
        {
            countSecret++;
            if (countSecret == 5)
            {
                Alert();
                countSecret = 0;
            }
        }

        private const int BS_MULTILINE = 0x00002000;
        private const int GWL_STYLE = -16;

        [System.Runtime.InteropServices.DllImport("coredll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("coredll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public static void MakeButtonMultiline(Button b)
        {
            IntPtr hwnd = b.Handle;
            int currentStyle = GetWindowLong(hwnd, GWL_STYLE);
            int newStyle = SetWindowLong(hwnd, GWL_STYLE, currentStyle | BS_MULTILINE);
        }
    }

    public class Stocktaking
    {
        public string StocktakingID { get; set; }
        public int ScanMode { get; set; }
        public string LocationCode { get; set; }
        public string Barcode { get; set; }
        public decimal Quantity { get; set; }
        public int UnitCode { get; set; }
        public string Flag { get; set; }
        public string Description { get; set; }
        public string SKUCode { get; set; }
        public string ExBarcode { get; set; }
        public string InBarcode { get; set; }
        public bool SKUMode { get; set; }
        public string HHTName { get; set; }
        public string HHTID { get; set; }
        public string DepartmentCode { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string SerialNumber { get; set; }
        public string ConversionCounter { get; set; }
    }
}