using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DNWA.BHTCL;
using Denso_HHT.Module;
using System.Threading;
using System.Text.RegularExpressions;

namespace Denso_HHT
{
    public partial class ScanProduct : Form
    {
        private List<StockTakingModel> scannedItemData = new List<StockTakingModel>();
        private LocationModel tempLocationModel = new LocationModel();
        private SKUModel tempSKUModel = new SKUModel();
        private int currentCursor = 1;

        private bool isHaveLocationData = false;
        private bool isHaveSKUData = false;

        private int tempSumPCS = 0;
        private int tempSumPCK = 0;

        private string currentLocation = "";
        private string currentBrand = null;

        private bool blockScannerPortState = false;

        private ScanProductMode scanProductMode;

        private Scanner scanner = new Scanner();

        public ScanProduct(ScanProductMode scanProductMode)
        {
            InitializeComponent();

            hhtToolBar1.HideDateTime();

            this.scanProductMode = scanProductMode;

            this.scanner.OnDone += new EventHandler(this.Scanner_OnDone1);

            SetupUI();
        }

        private void SetupUI()
        {
            scannedItemData = DatabaseModule.Instance.QuerySelectPreviousAuditDataFromScan((int)(this.scanProductMode));
            currentCursor = scannedItemData.Count + 1;
            isHaveLocationData = DatabaseModule.Instance.QuerySelectHaveLocationFromScan();
            isHaveSKUData = DatabaseModule.Instance.QuerySelectHaveSKUMasterFromScan();
            List<UnitModel> tempUnitModel = DatabaseModule.Instance.QuerySelectUnitFromScan("N");
            //tempUnitModel.RemoveAt(1);
            ddlUnit.DataSource = tempUnitModel;
            ddlUnit.DisplayMember = "UnitName";
            ddlUnit.ValueMember = "UnitCode";

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

            //if (!Program.LastScannedLocationProduct.Equals(""))
            //{
            //    ScanLocation(Program.LastScannedLocationFront);
            //    tbBarcode.Text = Program.LastScannedBarcodeFront;
            //}

            btnNext.Text = "Save";
            ddlUnit.SelectedIndex = 0;
            btnDelete.Enabled = false;

            switch (this.scanProductMode)
            {
                case ScanProductMode.ScanOnly:
                    constSubMenu.Text = "Scan Only";
                    tbQuantity.Text = "1";
                    tbQuantity.Enabled = false;
                    tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    ddlUnit.SelectedIndex = 0;
                    hhtToolBar1.BackColor = Color.FromArgb(223, 223, 223);
                    tbQuantity.Location = new System.Drawing.Point(79, 100);
                    ddlUnit.Location = new System.Drawing.Point(169, 100);
                    constQuan.Location = new System.Drawing.Point(2, 100);
                    tbSerial.Visible = true;
                    constSN.Visible = true;
                    snCheck.Visible = true;
                    tbSerial.Enabled = false;
                    tbSerial.BackColor = Color.FromArgb(223, 223, 223);
                    constCounter.Visible = false;
                    tbCounter.Visible = false;
                    break;

                case ScanProductMode.ScanQty:
                    constSubMenu.Text = "Scan Qty";
                    tbQuantity.Text = "";
                    tbQuantity.Enabled = true;
                    tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                    ddlUnit.SelectedIndex = 0;
                    hhtToolBar1.BackColor = Color.FromArgb(223, 223, 223);
                    constQuan.Location = new System.Drawing.Point(3, 64);
                    tbQuantity.Location = new System.Drawing.Point(79, 64);
                    ddlUnit.Location = new System.Drawing.Point(169, 64);
                    tbSerial.Visible = false;
                    constSN.Visible = false;
                    snCheck.Visible = false;
                    constCounter.Location = new System.Drawing.Point(3, 107);
                    tbCounter.Location = new System.Drawing.Point(153, 100);
                    constCounter.Visible = false;
                    tbCounter.Visible = false;
                    break;

                case ScanProductMode.ScanPackOnly:
                    constSubMenu.Text = "Scan Pack Only";
                    tbQuantity.Text = "1";
                    tbQuantity.Enabled = false;
                    tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    ddlUnit.SelectedIndex = 1;
                    tbSerial.Enabled = false;
                    tbSerial.Visible = false;
                    constSN.Visible = false;
                    snCheck.Visible = false;
                    hhtToolBar1.BackColor = Color.FromArgb(223, 223, 223);
                    tbCounter.Visible = true;
                    constCounter.Visible = true;
                    break;

                case ScanProductMode.ScanPackQty:
                    constSubMenu.Text = "Scan Pack Qty";
                    tbQuantity.Text = "";
                    tbQuantity.Enabled = true;
                    tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                    ddlUnit.SelectedIndex = 1;
                    tbSerial.Enabled = false;
                    tbSerial.Visible = false;
                    constSN.Visible = false;
                    snCheck.Visible = false;
                    hhtToolBar1.BackColor = Color.FromArgb(223, 223, 223);
                    tbCounter.Visible = true;
                    constCounter.Visible = true;
                    break;
                default:
                    break;
            }
            FocusOnSuitableTextBox();
        }

        private int FindDDLIndex(int value)
        {
            string unitNameForSearch = DatabaseModule.Instance.QuerySelectUnitNameFromScan(value);
            for (int i = 0; i < ddlUnit.Items.Count; i++)
            {
                if (ddlUnit.GetItemText(ddlUnit.Items[i]).Equals(unitNameForSearch))
                {
                    return i;
                }
            }
            return -1;
        }

        private void Scanner_OnDone1(object sender, EventArgs e)
        {
            Thread.Sleep(10);
            int digits = scanner.InBufferCount;
            string barcode = "";
            try
            {
                barcode = scanner.Input(18);
                String tempBarcode = "";
                foreach (char c in barcode)
                {
                    if (Char.IsDigit(c))
                    {
                        tempBarcode += c.ToString();
                    }
                }
                barcode = tempBarcode;
                scanner.PortOpen = false;
            }
            catch (Exception ex)
            {
                scanner.PortOpen = false;
                MessageBox.Show("You are scanning too fast", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (!scanner.PortOpen)
                {
                    scanner.PortOpen = true;
                }
                return;
            }

            if (tbLocation.Focused)
            {
                if (digits != 5)
                {
                    MessageBox.Show("Invalid Location Code", "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    tbLocation.Text = "";
                }
                else
                {
                    ScanLocation(barcode);
                }
            }
            else if (tbBarcode.Focused)
            {
                if (scanProductMode == ScanProductMode.ScanOnly && ddlUnit.Text == "PCS")
                {
                    if (barcode == tbBarcode.Text)
                    {
                        int firstQty = Int32.Parse(tbQuantity.Text);
                        firstQty++;
                        tbQuantity.Text = firstQty.ToString();
                    }

                    if (barcode.Length > 14)
                    {
                        MessageBox.Show("Invalid Barcode", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        tbBarcode.Text = "";
                    }
                    else
                    {
                        if (snCheck.Checked)
                        {
                            tbBarcode.Text = barcode;
                            Program.LastScannedBarcodeProduct = barcode;
                            if (IsHaveSerialNumber(barcode))
                            {
                                MessageBox.Show("Please scan serial number", "Warning", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            }
                        }
                        else
                        {
                            tbBarcode.Text = barcode;
                            Program.LastScannedBarcodeProduct = barcode;
                            if (!EnableSerial(barcode))
                            {
                                btnNext_ClickAction();
                            }
                        }                       
                    }
                }
                else if (scanProductMode == ScanProductMode.ScanPackOnly && ddlUnit.Text == "PCS")
                {
                    if (barcode.Length > 14)
                    {
                        MessageBox.Show("Invalid Barcode", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        tbBarcode.Text = "";
                    }
                    else
                    {
                         tbBarcode.Text = barcode;
                         Program.LastScannedBarcodeProduct = barcode;

                         btnNext_ClickAction();                       
                    }
                }
                else 
                {
                    if (barcode.Length > 14)
                    {
                        MessageBox.Show("Invalid Barcode", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        tbBarcode.Text = "";
                    }
                    else
                    {
                        tbBarcode.Text = barcode;
                        Program.LastScannedBarcodeProduct = barcode;
                        if (scanProductMode == ScanProductMode.ScanOnly && ddlUnit.Text == "PCS")
                        {
                            EnableSerial(barcode);
                        }                       
                    }
                }
            }
            else if (tbSerial.Focused)
            {
                if (IsRightSerialNumber(tbBarcode.Text, barcode))
                {
                    tbSerial.Text = barcode;
                    btnNext_ClickAction();
                }
                else
                {
                    MessageBox.Show("Serial Number is wrong", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    tbSerial.Text = barcode;
                }
            }

            if (!scanner.PortOpen)
            {
                scanner.PortOpen = true;
            }

            FocusOnSuitableTextBox();
        }

        private void ScanLocation(string barcode)
        {
            if (!isHaveLocationData)
            {
                ClearScreen();

                currentBrand = null;
                currentLocation = barcode;

                tbLocation.Text = barcode;

                tempLocationModel.BrandCode = null;
                tempLocationModel.LocationCode = barcode;

                Program.LastScannedLocationProduct = barcode;
            }
            else
            {
                LocationModel result = DatabaseModule.Instance.QueryLocationFromScan(barcode, 1);
                if (result != null)
                {
                    ClearScreen();

                    if (result.SectionCode.Equals(result.BrandCode))
                    {
                        currentBrand = result.BrandCode;
                        currentLocation = result.LocationCode;

                        tbLocation.Text = result.LocationCode;

                        tempLocationModel.BrandCode = result.BrandCode;
                        //tempLocationModel.ScanMode = result.ScanMode;
                        tempLocationModel.LocationCode = result.LocationCode;
                    }
                    else
                    {
                        currentBrand = null;
                        currentLocation = result.LocationCode;

                        tbLocation.Text = result.LocationCode;

                        tempLocationModel.BrandCode = null;
                        //tempLocationModel.ScanMode = result.ScanMode;
                        tempLocationModel.LocationCode = result.LocationCode;
                    }

                    Program.LastScannedLocationProduct = barcode;
                }
                else
                {
                    tbLocation.Text = barcode;
                    result = DatabaseModule.Instance.QueryLocationFromScan(barcode, 4);
                    if (result != null)
                    {
                        //Loading2.CloseLoading();
                        MessageBox.Show("Location code not found", "Warning", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        tbLocation.Text = "";
                    }
                    else
                    {
                        //Loading2.CloseLoading();
                        MessageBox.Show("Your HHT can not audit this location code", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        tbLocation.Text = "";
                    }
                }
            }
        }

        private void ScanBarcode(string barcode)
        {
            tempSKUModel = new SKUModel();
            if (!isHaveSKUData)
            {
                tbBarcode.Text = barcode;

                tempSKUModel.Barcode = barcode;
                tempSKUModel.BrandCode = null;
                tempSKUModel.ExBarcode = null;
                tempSKUModel.InBarcode = null;
                tempSKUModel.Description = null;
                tempSKUModel.SKUCode = null;
                tempSKUModel.DepartmentCode = null;
                tempSKUModel.IsNew = true;
            }
            else
            {
                SKUModel result = DatabaseModule.Instance.QueryProductFromScan(barcode);
                if (result != null)
                {
                    tbBarcode.Text = barcode;

                    tempSKUModel.Barcode = barcode;
                    tempSKUModel.BrandCode = result.BrandCode;
                    tempSKUModel.ExBarcode = result.ExBarcode;
                    tempSKUModel.InBarcode = result.InBarcode;
                    tempSKUModel.Description = result.Description;
                    tempSKUModel.SKUCode = result.SKUCode;
                    tempSKUModel.DepartmentCode = result.DepartmentCode;
                    tempSKUModel.IsNew = false;
                }
                else
                {
                    tbBarcode.Text = barcode;

                    tempSKUModel.Barcode = barcode;
                    tempSKUModel.BrandCode = null;
                    tempSKUModel.ExBarcode = null;
                    tempSKUModel.InBarcode = null;
                    tempSKUModel.Description = null;
                    tempSKUModel.SKUCode = null;
                    tempSKUModel.DepartmentCode = null;
                    tempSKUModel.IsNew = true;
                }
                //Loading2.CloseLoading();
            }
            //EnableSerial(barcode);
        }

        private void UpdateData(string stocktakingid, string serialNumber, string conversion , int mode)
        {
            DatabaseModule.Instance.QueryUpdateLastStocktakingData(stocktakingid, serialNumber, conversion , mode);
        }

        private void UpdateSummaryQty(decimal addition, int unitCode, int mode)
        {
            if (string.IsNullOrEmpty(tbLocation.Text))
            {
                lbSumQtyPCS.Text = "PCS : ";
                lbSumQtyPCK.Text = "PCK : ";
            }
            else
            {
                if (mode != 0)
                {
                    DatabaseModule.Instance.QueryUpdateTotalByLocationFromScan(tbLocation.Text, unitCode, addition, mode);
                }

                string sumQtyPCS = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "PCS")[0];
                string sumQtyPCK = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "PCK")[0];
                tempSumPCS = Int32.Parse(sumQtyPCS);
                tempSumPCK = Int32.Parse(sumQtyPCK);

                try
                {
                    if (!tbBarcode.Text.Equals("") && currentCursor == scannedItemData.Count + 1)
                    {
                        int tempCal;
                        switch (ddlUnit.Text)
                        {
                            case "PCS":
                                tempCal = tempSumPCS;// +Int32.Parse(tbQuantity.Text);

                                if (tempCal > 99999)
                                {
                                    lbSumQtyPCS.Text = "PCS : " + ((tempCal / 1000).ToString() + "k");
                                }
                                else
                                {
                                    lbSumQtyPCS.Text = string.Format("PCS : {0}", (tempCal).ToString("N0"));
                                }

                                if (tempSumPCK > 99999)
                                {
                                    lbSumQtyPCK.Text = "PCK : " + ((tempSumPCK / 1000).ToString() + "k");
                                }
                                else
                                {
                                    lbSumQtyPCK.Text = string.Format("PCK : {0}", tempSumPCK.ToString("N0"));
                                }
                                break;
                            case "PCK":

                                if (tempSumPCS > 99999)
                                {
                                    lbSumQtyPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k");
                                }
                                else
                                {
                                    lbSumQtyPCS.Text = string.Format("PCS : {0}", (tempSumPCS).ToString("N0"));
                                }

                                tempCal = tempSumPCK;// +Int32.Parse(tbQuantity.Text);
                                if (tempCal > 99999)
                                {
                                    lbSumQtyPCK.Text = "PCK : " + ((tempCal / 1000).ToString() + "k");
                                }
                                else
                                {
                                    lbSumQtyPCK.Text = string.Format("PCK : {0}", tempCal.ToString("N0"));
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (tempSumPCS > 99999)
                        {
                            lbSumQtyPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k");
                        }
                        else
                        {
                            lbSumQtyPCS.Text = string.Format("PCS : {0}", (tempSumPCS).ToString("N0"));
                        }

                        if (tempSumPCK > 99999)
                        {
                            lbSumQtyPCK.Text = "PCK : " + ((tempSumPCK / 1000).ToString() + "k");
                        }
                        else
                        {
                            lbSumQtyPCK.Text = string.Format("PCK : {0}", (tempSumPCK).ToString("N0"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumPCS > 99999)
                    {
                        lbSumQtyPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k");
                    }
                    else
                    {
                        lbSumQtyPCS.Text = string.Format("PCS : {0}", (tempSumPCS).ToString("N0"));
                    }

                    if (tempSumPCK > 99999)
                    {
                        lbSumQtyPCK.Text = "PCK : " + ((tempSumPCK / 1000).ToString() + "k");
                    }
                    else
                    {
                        lbSumQtyPCK.Text = string.Format("PCK : {0}", (tempSumPCK).ToString("N0"));
                    }
                }
            }
        }

        private void UpdateSummaryQty(decimal oldQuantity, decimal newQuantity, int oldUnitcode, int newUnitCode)
        {

            if (string.IsNullOrEmpty(tbLocation.Text))
            {
                lbSumQtyPCS.Text = "PCS : ";
                lbSumQtyPCK.Text = "PCK : ";
            }
            else
            {
                string oldUnitName = DatabaseModule.Instance.QuerySelectUnitNameFromScan(oldUnitcode);
                if (oldUnitName.Equals(ddlUnit.Text))
                {
                    newQuantity = -(oldQuantity - newQuantity);
                }
                DatabaseModule.Instance.QueryUpdateTotalByLocationFromScan(tbLocation.Text, oldUnitcode, newUnitCode, oldQuantity, newQuantity);

                string sumQtyPCS = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "PCS")[0];
                string sumQtyPCK = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "PCK")[0];
                tempSumPCS = Int32.Parse(sumQtyPCS);
                tempSumPCK = Int32.Parse(sumQtyPCK);

                try
                {
                    if (!tbBarcode.Text.Equals("") && currentCursor == scannedItemData.Count + 1)
                    {
                        int tempCal;
                        switch (ddlUnit.Text)
                        {
                            case "PCS":
                                tempCal = tempSumPCS;// +Int32.Parse(tbQuantity.Text);
                                if (tempCal > 99999)
                                {
                                    lbSumQtyPCS.Text = "PCS : " + ((tempCal / 1000).ToString() + "k");
                                }
                                else
                                {
                                    lbSumQtyPCS.Text = string.Format("PCS : {0}", (tempCal).ToString("N0"));
                                }

                                if (tempSumPCK > 99999)
                                {
                                    lbSumQtyPCK.Text = "PCK : " + ((tempSumPCK / 1000).ToString() + "k");
                                }
                                else
                                {
                                    lbSumQtyPCK.Text = string.Format("PCK : {0}", tempSumPCK.ToString("N0"));
                                }
                                break;
                            case "PCK":
                                if (tempSumPCS > 99999)
                                {
                                    lbSumQtyPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k");
                                }
                                else
                                {
                                    lbSumQtyPCS.Text = string.Format("PCS : {0}", (tempSumPCS).ToString("N0"));
                                }

                                tempCal = tempSumPCK;// +Int32.Parse(tbQuantity.Text);
                                if (tempCal > 99999)
                                {
                                    lbSumQtyPCK.Text = "PCK : " + ((tempCal / 1000).ToString() + "k");
                                }
                                else
                                {
                                    lbSumQtyPCK.Text = string.Format("PCK : {0}", tempCal.ToString("N0"));
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (tempSumPCS > 99999)
                        {
                            lbSumQtyPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k");
                        }
                        else
                        {
                            lbSumQtyPCS.Text = string.Format("PCS : {0}", (tempSumPCS).ToString("N0"));
                        }

                        if (tempSumPCK > 99999)
                        {
                            lbSumQtyPCK.Text = "PCK : " + ((tempSumPCK / 1000).ToString() + "k");
                        }
                        else
                        {
                            lbSumQtyPCK.Text = string.Format("PCK : {0}", (tempSumPCK).ToString("N0"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumPCS > 99999)
                    {
                        lbSumQtyPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k");
                    }
                    else
                    {
                        lbSumQtyPCS.Text = string.Format("PCS : {0}", (tempSumPCS).ToString("N0"));
                    }

                    if (tempSumPCK > 99999)
                    {
                        lbSumQtyPCK.Text = "PCK : " + ((tempSumPCK / 1000).ToString() + "k");
                    }
                    else
                    {
                        lbSumQtyPCK.Text = string.Format("PCK : {0}", (tempSumPCK).ToString("N0"));
                    }
                }
            }
        }

        private void PrepareToNextScan()
        {
            switch (scanProductMode)
            {
                case ScanProductMode.ScanOnly:
                    tbLocation.Text = currentLocation;
                    tbLocation.Enabled = true;
                    tbLocation.BackColor = Color.FromArgb(255, 255, 255);
                    tbBarcode.Text = "";
                    tbBarcode.Enabled = true;
                    tbBarcode.BackColor = Color.FromArgb(255, 255, 255);
                    tbSerial.Text = "";
                    snCheck.Checked = false;
                    tbSerial.Enabled = false;
                    tbSerial.BackColor = Color.FromArgb(223, 223, 223);
                    tbQuantity.Text = "1";
                    tbQuantity.Enabled = false;
                    tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    ddlUnit.SelectedIndex = 0;

                    btnNext.Text = "Save";
                    btnDelete.Enabled = false;
                    btnClear.Enabled = true;

                    FocusOnSuitableTextBox();
                    break;

                case ScanProductMode.ScanQty:
                    tbLocation.Text = currentLocation;
                    tbLocation.Enabled = true;
                    tbLocation.BackColor = Color.FromArgb(255, 255, 255);
                    tbBarcode.Text = "";
                    tbBarcode.Enabled = true;
                    tbBarcode.BackColor = Color.FromArgb(255, 255, 255);
                    tbQuantity.Text = "";
                    tbQuantity.Enabled = true;
                    tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                    ddlUnit.SelectedIndex = 0;

                    btnNext.Text = "Save";
                    btnDelete.Enabled = false;
                    btnClear.Enabled = true;

                    FocusOnSuitableTextBox();
                    break;

                case ScanProductMode.ScanPackOnly:
                    tbLocation.Text = currentLocation;
                    tbLocation.Enabled = true;
                    tbLocation.BackColor = Color.FromArgb(255, 255, 255);
                    tbBarcode.Text = "";
                    tbBarcode.Enabled = true;
                    tbBarcode.BackColor = Color.FromArgb(255, 255, 255);
                    tbQuantity.Text = "1";
                    tbQuantity.Enabled = false;
                    tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    tbCounter.Text = "";
                    tbCounter.Enabled = true;
                    tbCounter.BackColor = Color.FromArgb(255, 255, 255);
                    ddlUnit.SelectedIndex = 1;

                    btnNext.Text = "Save";
                    btnDelete.Enabled = false;
                    btnClear.Enabled = true;
                    FocusOnSuitableTextBox();
                    break;

                case ScanProductMode.ScanPackQty:

                    tbLocation.Text = currentLocation;
                    tbLocation.Enabled = true;
                    tbLocation.BackColor = Color.FromArgb(255, 255, 255);
                    tbBarcode.Text = "";
                    tbBarcode.Enabled = true;
                    tbBarcode.BackColor = Color.FromArgb(255, 255, 255);
                    tbQuantity.Text = "";
                    tbQuantity.Enabled = true;
                    tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                    tbCounter.Text = "";
                    tbCounter.Enabled = true;
                    tbCounter.BackColor = Color.FromArgb(255, 255, 255);
                    ddlUnit.SelectedIndex = 1;

                    btnNext.Text = "Save";
                    btnDelete.Enabled = false;
                    btnClear.Enabled = true;
                    FocusOnSuitableTextBox();

                    break;
                default:
                    break;
            }
        }

        private void PrepareToPreview()
        {
            StockTakingModel tempItemData = scannedItemData[currentCursor - 1];
            tbLocation.Text = tempItemData.LocationCode;
            tbBarcode.Text = tempItemData.Barcode;

            tbQuantity.Text = ((int)tempItemData.Quantity).ToString();
            tbLocation.Enabled = false;
            tbLocation.BackColor = Color.FromArgb(223, 223, 223);
            tbBarcode.Enabled = false;
            tbBarcode.BackColor = Color.FromArgb(223, 223, 223);
            tbQuantity.Enabled = true;
            tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
            ddlUnit.SelectedIndex = FindDDLIndex(tempItemData.UnitCode);

            if(ddlUnit.Text.Equals("PCK"))
            {
                 tbCounter.Text = tempItemData.ConversionCounter;
                 tbCounter.Enabled = true;
                 tbCounter.BackColor = Color.FromArgb(255, 255, 255);
            }
            else
            {
                if(scanProductMode == ScanProductMode.ScanOnly)
                {
                    tbSerial.Text = tempItemData.SerialNumber;

                    if (!string.IsNullOrEmpty(tbSerial.Text))
                    {
                        tbSerial.Enabled = true;
                        tbSerial.BackColor = Color.FromArgb(255, 255, 255);
                        snCheck.Checked = true;
                    }
                    else
                    {
                        tbSerial.Enabled = false;
                        tbSerial.BackColor = Color.FromArgb(223, 223, 223);
                        snCheck.Checked = false;
                    }
                }
            }

            btnNext.Text = "Next >>";
            btnDelete.Enabled = true;
            btnClear.Enabled = false;

            FocusOnSuitableTextBox();
        }     

        private void FocusOnSuitableTextBox()
        {
            try
            {


                if (tbLocation.Text.Equals(""))
                {
                    if (!tbLocation.Focused)
                    {
                        tbLocation.Focus();
                    }
                    tbLocation.SelectionStart = tbLocation.Text.Length;
                }
                else if (tbBarcode.Text.Equals(""))
                {
                    if (!tbBarcode.Focused)
                    {
                        tbBarcode.Focus();
                    }
                    tbBarcode.SelectionStart = tbBarcode.Text.Length;
                }
                else if ( snCheck.Checked && tbSerial.Text.Equals(""))
                {
                    if (!tbSerial.Focused)
                    {
                        tbSerial.Focus();
                    }
                    tbSerial.SelectionStart = tbSerial.Text.Length;
                }
                else if (tbQuantity.Text.Equals(""))
                {
                    if (!tbQuantity.Focused)
                    {
                        tbQuantity.Focus();
                    }
                    tbQuantity.SelectionStart = tbQuantity.Text.Length;
                }
                else if (tbCounter.Text.Equals("") && tbCounter.Visible)
                {
                    if (!tbCounter.Focused)
                    {
                        tbCounter.Focus();
                    }
                    tbCounter.SelectionStart = tbCounter.Text.Length;
                }
                else
                {
                    if (tbQuantity.Visible && tbQuantity.Enabled)
                    {
                        if (!tbQuantity.Focused)
                        {
                            tbQuantity.Focus();
                        }
                        tbQuantity.SelectionStart = tbQuantity.Text.Length;
                    }
                    else if (tbSerial.Visible && !tbSerial.Text.Equals(""))
                    {
                        if (!tbSerial.Focused)
                        {
                            tbSerial.Focus();
                        }
                        tbSerial.SelectionStart = tbSerial.Text.Length;
                    }
                    else
                    {
                        if (!tbBarcode.Focused)
                        {
                            tbBarcode.Focus();
                        }
                        tbBarcode.SelectionStart = tbBarcode.Text.Length;
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void ClearScreen()
        {
            tbLocation.Text = "";
            tbBarcode.Text = "";
            tbSerial.Text = "";
            snCheck.Checked = false;
            if (scanProductMode == ScanProductMode.ScanQty || scanProductMode == ScanProductMode.ScanPackQty)
            {
                tbQuantity.Text = "";
                tbCounter.Text = "";
            }
            else if (scanProductMode == ScanProductMode.ScanOnly || scanProductMode == ScanProductMode.ScanPackOnly)
            {
                tbQuantity.Text = "1";
                tbCounter.Text = "";
            }
        }

        private void BackToFirstRecord()
        {
            if (scannedItemData.Count == 0)
            {
                MessageBox.Show("You have no data", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                FocusOnSuitableTextBox();
            }
            else if (currentCursor == scannedItemData.Count + 1)
            {
                currentCursor = 1;
                PrepareToPreview();
            }
            else if (currentCursor == 1)
            {
                return;
            }
            else
            {
                if (CheckForUpdate(currentCursor))
                {
                    UpdateAuditData(currentCursor);
                }

                currentCursor = 1;
                PrepareToPreview();
            }
        }

        private void NextToScan()
        {
            if (currentCursor == scannedItemData.Count + 1)
            {
                return;
            }
            else
            {
                if (CheckForUpdate(currentCursor))
                {
                    UpdateAuditData(currentCursor);
                }

                currentCursor = scannedItemData.Count + 1;
                PrepareToNextScan();
            }
        }

        private bool CheckForUpdate(int passCursor)
        {
            try
            {
                if ((scannedItemData[passCursor - 1].SerialNumber == null ? "" : scannedItemData[passCursor - 1].SerialNumber) != tbSerial.Text)
                {
                    return true;
                }
                else if ((scannedItemData[passCursor - 1].ConversionCounter == null ? "" : scannedItemData[passCursor - 1].ConversionCounter ) != tbCounter.Text)
                {
                    return true;
                }
                else if (scannedItemData[passCursor - 1].Quantity != Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"))
                    || scannedItemData[passCursor - 1].UnitCode != Convert.ToInt32(ddlUnit.SelectedValue))
                {
                    return true;
                }
                else
                {
                    float quantity = float.Parse(tbQuantity.Text);
                    if (quantity == 0)
                    {
                        MessageBox.Show("Quantity can not be zero", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                    return false;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Quantity can not be other except for number", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
        }

        private void UpdateAuditData(int passCursor)
        {
            try
            {
                string oldStocktakingID = scannedItemData[passCursor - 1].StocktakingID;
                decimal oldQuantity = scannedItemData[passCursor - 1].Quantity;
                int oldUnitCode = scannedItemData[passCursor - 1].UnitCode;
                decimal newQuantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                int newUnitCode = Convert.ToInt32(ddlUnit.SelectedValue);

                string oldSerial = scannedItemData[passCursor - 1].SerialNumber == null ? "" : scannedItemData[passCursor - 1].SerialNumber;
                string newSerial = tbSerial.Text == null ? "" : tbSerial.Text;

                string oldCounter = scannedItemData[passCursor - 1].ConversionCounter == null ? "" : scannedItemData[passCursor - 1].ConversionCounter;
                string newCounter = tbCounter.Text == null ? "" : tbCounter.Text;
                
                Saving.OpenSaving();             
                Thread.Sleep(100);

                if (!Program.isNonRealtime)
                {
                    APIModule.Instance.SendRequestThread(scannedItemData[passCursor - 1],
                        Math.Round(Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"))),
                        Convert.ToInt32(ddlUnit.SelectedValue), (int)(this.scanProductMode));
                    scannedItemData[passCursor - 1].Quantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                    scannedItemData[passCursor - 1].UnitCode = Convert.ToInt32(ddlUnit.SelectedValue);

                    scannedItemData[passCursor - 1].SerialNumber = tbSerial.Text;
                    scannedItemData[passCursor - 1].ConversionCounter = tbCounter.Text;

                    UpdateSummaryQty(oldQuantity, scannedItemData[passCursor - 1].Quantity, oldUnitCode, Convert.ToInt32(ddlUnit.SelectedValue));
                    UpdateData(oldStocktakingID, newSerial, newCounter, (int)(this.scanProductMode));
                    this.Show();
                }
                else
                {
                    DatabaseModule.Instance.QueryUpdateFromScan(scannedItemData[passCursor - 1].StocktakingID,
                        Math.Round(Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"))),
                        Convert.ToInt32(ddlUnit.SelectedValue), false, (int)(this.scanProductMode));
                    scannedItemData[passCursor - 1].Quantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                    scannedItemData[passCursor - 1].UnitCode = Convert.ToInt32(ddlUnit.SelectedValue);

                    scannedItemData[passCursor - 1].SerialNumber = tbSerial.Text;
                    scannedItemData[passCursor - 1].ConversionCounter = tbCounter.Text;

                    UpdateSummaryQty(oldQuantity, scannedItemData[passCursor - 1].Quantity, oldUnitCode, Convert.ToInt32(ddlUnit.SelectedValue));
                    UpdateData(oldStocktakingID, newSerial, newCounter, (int)(this.scanProductMode));
                }
                
                Saving.CloseSaving();
            }
            catch (Exception ex)
            {
                Saving.CloseSaving();
                MessageBox.Show("Update Error : " + ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                throw ex;
            }
        }

        private bool CheckForAdd()
        {
            if (tbLocation.Text.Equals("") || tbBarcode.Text.Equals("") || tbQuantity.Text.Equals("")) 
            {
                MessageBox.Show("Location, Barcode and Quantity must not empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
            else if (tbCounter.Visible && tbCounter.Text.Equals(""))
            {
                MessageBox.Show("Location, Barcode , Quantity and Conversion Counter must not empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
            else if (tbSerial.Visible && tbSerial.Text.Equals("") && snCheck.Checked)
            {
                MessageBox.Show("Location, Barcode, Serial and Quantity must not empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
            else if (tbLocation.Text.Length != 5)
            {
                MessageBox.Show("Invalid Location Code", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                tbLocation.Text = "";
                return false;
            }
            else if (IsHaveSerialNumber(tbBarcode.Text) && scanProductMode == ScanProductMode.ScanOnly)
            {
               if (IsRightSerialNumber(tbBarcode.Text, tbSerial.Text))
               {
                  return true;
               }
               else
               {
                   MessageBox.Show("Serial Number is wrong", "Warning", MessageBoxButtons.OK,
                   MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                   return false;
               }                         
            }
            else
            {
                try
                {
                    float quantity = float.Parse(tbQuantity.Text);
                    if (quantity == 0)
                    {
                        MessageBox.Show("Quantity can not be zero", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Quantity can not be other except for number", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }
        }

        private bool CheckForAddWithoutWarning()
        {
            if (tbLocation.Text.Equals("") || tbBarcode.Text.Equals("") || tbQuantity.Text.Equals(""))
            {
                return false;
            }
            else if (tbCounter.Visible && tbCounter.Text.Equals(""))
            {          
                return false;
            }
            else if (tbSerial.Visible && tbSerial.Text.Equals("") && snCheck.Checked)
            {
                return false;
            }
            else if (tbLocation.Text.Length != 5)
            {
                return false;
            }
            else if (IsHaveSerialNumber(tbBarcode.Text) && scanProductMode == ScanProductMode.ScanOnly)
            {
                if (IsRightSerialNumber(tbBarcode.Text, tbSerial.Text))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    float quantity = float.Parse(tbQuantity.Text);
                    if (quantity == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (FormatException ex)
                {
                    return false;
                }
            }
        }

        private void AddAuditData()
        {
            try
            {
                StockTakingModel itemData = new StockTakingModel();
                itemData.StocktakingID = DatabaseModule.Instance.GetLastStocktakingID();
                itemData.Barcode = tempSKUModel.Barcode;
                itemData.BrandCode = tempLocationModel.BrandCode;
                itemData.Description = tempSKUModel.Description;
                itemData.ExBarcode = tempSKUModel.ExBarcode;
                itemData.Flag = tempSKUModel.Flag;
                itemData.InBarcode = tempSKUModel.InBarcode;
                itemData.LocationCode = tempLocationModel.LocationCode;
                itemData.Quantity = Math.Round(Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3")));
                itemData.ScanMode = (int)(this.scanProductMode);
                itemData.SKUCode = tempSKUModel.SKUCode;
                itemData.UnitCode = Convert.ToInt32(ddlUnit.SelectedValue);
                itemData.SKUMode = isHaveSKUData;
                itemData.DepartmentCode = tempSKUModel.DepartmentCode;
                itemData.SerialNumber = tbSerial.Text == "" ? null : tbSerial.Text;
                itemData.ConversionCounter = tbCounter.Text == "" ? null : tbCounter.Text;
                
                Saving.OpenSaving();            
                Thread.Sleep(100);
                if (!Program.isNonRealtime)
                {
                    APIModule.Instance.SendRequestThread(itemData, (int)(this.scanProductMode));
                    scannedItemData.Add(itemData);

                    if (scannedItemData.Count == 101)
                    {
                        scannedItemData.RemoveAt(0);
                        currentCursor--;
                    }

                    UpdateSummaryQty(itemData.Quantity, Convert.ToInt32(ddlUnit.SelectedValue), 1);
                    this.Show();
                }
                else
                {
                    itemData.SendFlag = false;

                    StockTakingModel lastInsertedStocktaking = DatabaseModule.Instance.GetLastInsertedStocktaking();
                    if (lastInsertedStocktaking != null && DatabaseModule.Instance.QueryUpdateLastStocktaking(lastInsertedStocktaking, itemData, (int)(this.scanProductMode)))
                    {
                        scannedItemData[scannedItemData.Count - 1].Quantity = lastInsertedStocktaking.Quantity + itemData.Quantity;
                        currentCursor--;
                        UpdateSummaryQty(lastInsertedStocktaking.Quantity, scannedItemData[scannedItemData.Count - 1].Quantity, lastInsertedStocktaking.UnitCode, lastInsertedStocktaking.UnitCode);
                    }
                    else
                    {
                        DatabaseModule.Instance.QueryInsertFromScan(itemData, (int)(this.scanProductMode));
                        scannedItemData.Add(itemData);

                        if (scannedItemData.Count == 101)
                        {
                            scannedItemData.RemoveAt(0);
                            currentCursor--;
                        }
                        UpdateSummaryQty(itemData.Quantity, Convert.ToInt32(ddlUnit.SelectedValue), 1);
                    }
                }
                
                Saving.CloseSaving();
            }
            catch (InvalidCastException ex)
            {
                
                Saving.CloseSaving();
                MessageBox.Show("The application encountered some problem about network, please try again.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                throw ex;
            }
            catch (Exception ex)
            {
                
                Saving.CloseSaving();
                MessageBox.Show("Insert Error : " + ex.Message, "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                throw ex;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
            {
                return;
            }

            DatabaseModule.Instance.QueryDeleteFromScan(scannedItemData[currentCursor - 1].StocktakingID, (int)(this.scanProductMode));
            UpdateSummaryQty(scannedItemData[currentCursor - 1].Quantity, scannedItemData[currentCursor - 1].UnitCode, 2);
            scannedItemData.RemoveAt(currentCursor - 1);

            if (currentCursor > scannedItemData.Count)
            {
                PrepareToNextScan();
            }
            else
            {
                PrepareToPreview();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.btnNext.Text.Equals("Save"))
            {
                blockScannerPortState = true;
                Program.LastScannedBarcodeProduct = "";
                this.scanner.PortOpen = false;
                btnNext_ClickAction();
                if (!this.scanner.PortOpen)
                {
                    this.scanner.PortOpen = true;
                }
                blockScannerPortState = false;
            }
            else
            {
                btnNext_ClickAction();
            }
        }

        private void btnNext_ClickAction()
        {
            bool isGoingAction = true;

            try
            {
                currentCursor++;
                if (currentCursor == scannedItemData.Count + 2)
                {
                    if (!CheckForAdd())
                    {
                        currentCursor--;
                        isGoingAction = false;
                    }
                    else
                    {
                        ScanBarcode(tbBarcode.Text);

                        if (!isHaveSKUData)
                        {
                            tempSKUModel.Flag = "I";
                            AddAuditData();
                        }
                        else if (tempSKUModel.IsNew)
                        {
                            tempSKUModel.Flag = "F";
                            AddAuditData();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(currentBrand))
                            {
                                if (!string.IsNullOrEmpty(tempSKUModel.BrandCode) && currentBrand.Equals(tempSKUModel.BrandCode))
                                {
                                    tempSKUModel.Flag = "I";
                                    AddAuditData();
                                }
                                else
                                {
                                    tempSKUModel.Flag = "F";
                                    AddAuditData();
                                }
                            }
                            else
                            {
                                tempSKUModel.Flag = "I";
                                AddAuditData();
                            }
                        }
                    }
                }
                else
                {
                    isGoingAction = true;
                    if (CheckForUpdate(currentCursor - 1))
                    {
                        UpdateAuditData(currentCursor - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                if (currentCursor == scannedItemData.Count + 2)
                {
                    currentCursor--;
                }
            }
            finally
            {
                if (isGoingAction)
                {
                    if (currentCursor == scannedItemData.Count + 1)
                    {
                        PrepareToNextScan();
                    }
                    else
                    {
                        PrepareToPreview();
                    }
                }
                else
                {
                    FocusOnSuitableTextBox();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            currentCursor--;
            if (currentCursor > 0)
            {
                if (currentCursor != scannedItemData.Count)
                {
                    if (CheckForUpdate(currentCursor + 1))
                    {
                        UpdateAuditData(currentCursor + 1);
                    }
                }
                PrepareToPreview();
            }
            else
            {
                currentCursor++;
                MessageBox.Show("No more previous data", "Warning", MessageBoxButtons.OK,
                     MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                FocusOnSuitableTextBox();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearScreen();
            FocusOnSuitableTextBox();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            if (CheckForAddWithoutWarning())
            {
                btnNext_ClickAction();
                Program.LastScannedBarcodeProduct = "";
            }

            this.DialogResult = DialogResult.Abort;
            this.Dispose();
        }

        private void tbQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)System.Windows.Forms.Keys.Back || char.IsDigit(e.KeyChar))
            {
                if (e.KeyChar == '0' && tbQuantity.Text.Length == 0)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbLocation.Text.Length == 5)
            {
                if (e.KeyChar != (char)System.Windows.Forms.Keys.Back)
                {
                    e.Handled = true;
                }
            }
            else if (tbLocation.Text.Length == 4 && Char.IsDigit(e.KeyChar))
            {
                CloseScanner();
                string barcodeInput = tbLocation.Text + e.KeyChar.ToString();
                ScanLocation(barcodeInput);
                e.Handled = true;
                OpenScanner();
                FocusOnSuitableTextBox();
            }
            else
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void tbBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbBarcode.Text.Length == tbBarcode.MaxLength
                && e.KeyChar != (char)System.Windows.Forms.Keys.Back)
            {
                e.Handled = true;
            }
            else if (Char.IsDigit(e.KeyChar) || e.KeyChar == (char)System.Windows.Forms.Keys.Back)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbBarcode_TextChanged(object sender, EventArgs e)
        {
            if (scanProductMode == ScanProductMode.ScanOnly || scanProductMode == ScanProductMode.ScanPackOnly)
             {
                 tbQuantity.Text = "1";
                 tbQuantity.Enabled = false;
                 tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
             }
             else 
             {
                 tbQuantity.Text = "";
                 tbQuantity.Enabled = true;
                 tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
             }
        }

        private void tbLocation_GotFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                if (!scanner.PortOpen)
                {
                    scanner.PortOpen = true;
                }
            }
        }

        private void tbLocation_LostFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                this.scanner.PortOpen = false;
            }
        }

        private void tbBarcode_GotFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                if (!scanner.PortOpen)
                {
                    scanner.PortOpen = true;
                }
            }
        }

        private void tbBarcode_LostFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                this.scanner.PortOpen = false;
            }
        }

        private void tbSerial_GotFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                if (!scanner.PortOpen)
                {
                    scanner.PortOpen = true;
                }
            }
        }

        private void tbSerial_LostFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                this.scanner.PortOpen = false;
            }
        }

        private void tbLocation_TextChanged(object sender, EventArgs e)
        {
            if (tbLocation.Text.Length == 5)
            {
                UpdateSummaryQty(0, 0, 0);
            }
            else
            {
                lbSumQtyPCS.Text = "PCS : ";
                lbSumQtyPCK.Text = "PCK : ";
            }
        }

        private void tbQuantity_TextChanged(object sender, EventArgs e)
        {
            if (!tbLocation.Text.Equals("") && !tbBarcode.Text.Equals("")
                    && currentCursor == scannedItemData.Count + 1)
            {
                try
                {
                    int tempCal;
                    switch (ddlUnit.Text)
                    {
                        case "PCS":
                            tempCal = tempSumPCS;// +Int32.Parse(tbQuantity.Text);
                            if (tempCal > 99999)
                            {
                                lbSumQtyPCS.Text = "PCS : " + ((tempCal / 1000).ToString() + "k");
                            }
                            else
                            {
                                lbSumQtyPCS.Text = "PCS : " + (tempCal).ToString("N0");
                            }
                            break;
                        case "PCK":
                            tempCal = tempSumPCK;// +Int32.Parse(tbQuantity.Text);
                            if (tempCal > 99999)
                            {
                                lbSumQtyPCK.Text = "PCK : " + ((tempCal / 1000).ToString() + "k");
                            }
                            else
                            {
                                lbSumQtyPCK.Text = "PCK : " + (tempCal).ToString("N0");
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumPCS > 99999)
                    {
                        lbSumQtyPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k");
                    }
                    else
                    {
                        lbSumQtyPCS.Text = "PCS : " + (tempSumPCS).ToString("N0");
                    }

                    if (tempSumPCK > 99999)
                    {
                        lbSumQtyPCK.Text = "PCK : " + ((tempSumPCK / 1000).ToString() + "k");
                    }
                    else
                    {
                        lbSumQtyPCK.Text = "PCK : " + (tempSumPCK).ToString("N0");
                    }
                }
            }
        }

        private void ScanProduct_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Enter:

                    if (tbBarcode.Focused && ddlUnit.Text == "PCS" && scanProductMode == ScanProductMode.ScanOnly)
                    {
                        if (!EnableSerial(tbBarcode.Text))
                        {
                            btnNext_ClickAction();
                        }
                    }
                    else if (tbSerial.Focused)
                    {
                        if (IsRightSerialNumber(tbBarcode.Text, tbSerial.Text))
                        {
                            btnNext_ClickAction();
                        }
                        else
                        {
                            MessageBox.Show("Serial Number is wrong", "Warning", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            FocusOnSuitableTextBox();
                        }
                    }
                    else if (tbLocation.Text.Length != 5)
                    {
                        MessageBox.Show("Location Code must be 5 digits", "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        tbLocation.Text = "";
                    }
                    else if (tbLocation.Text.Equals("") || tbBarcode.Text.Equals("") || tbQuantity.Text.Equals(""))
                    {
                        FocusOnSuitableTextBox();
                    }
                    else if (tbSerial.Visible && tbSerial.Text.Equals("") && snCheck.Checked)
                    {
                        FocusOnSuitableTextBox();
                    }
                    else if (tbCounter.Visible && tbCounter.Text.Equals(""))
                    {
                        FocusOnSuitableTextBox();
                    }
                    else
                    {
                        if (this.btnNext.Text.Equals("Save"))
                        {
                            blockScannerPortState = true;
                            Program.LastScannedBarcodeProduct = "";
                            this.scanner.PortOpen = false;
                            btnNext_ClickAction();
                            if (!this.scanner.PortOpen)
                            {
                                this.scanner.PortOpen = true;
                            }
                            blockScannerPortState = false;
                        }
                        else
                        {
                            if (CheckForUpdate(currentCursor))
                            {
                                UpdateAuditData(currentCursor);
                            }
                            PrepareToPreview();
                        }
                    }
                    break;
                case DNWA.BHTCL.Keys.M1:
                    btnMainMenu_Click(null, null);
                    break;
                case System.Windows.Forms.Keys.F1:
                    if (this.btnClear.Enabled)
                    {
                        btnClear_Click(null, null);
                    }
                    break;
                case System.Windows.Forms.Keys.F2:
                    if (this.btnDelete.Enabled)
                    {
                        btnDelete_Click(null, null);
                    }
                    break;
                case System.Windows.Forms.Keys.F5:
                    btnBack_Click(null, null);
                    break;
                case System.Windows.Forms.Keys.F6:
                    if (!this.btnNext.Text.Equals("Save"))
                    {
                        btnNext_Click(null, null);
                    }
                    break;
                case System.Windows.Forms.Keys.F8:
                    BackToFirstRecord();
                    break;
                case System.Windows.Forms.Keys.F9:
                    currentCursor = scannedItemData.Count + 1;
                    PrepareToNextScan();
                    break;
                case System.Windows.Forms.Keys.Escape:
                    btnCancel_Click(null, null);
                    break;
                case DNWA.BHTCL.Keys.FUNC:
                    lbFunction.Visible = !lbFunction.Visible;
                    break;
                case DNWA.BHTCL.Keys.ALP:
                    lbAlpha.Visible = !lbAlpha.Visible;
                    break;
                default:
                    break;
            }
        }

        private void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (scanProductMode == ScanProductMode.ScanOnly )
            {
                if (ddlUnit.Text.Equals("PCK"))
                {
                    HideSerial();
                    VisibleCounter();
                }
                else
                {
                    HideCounter();
                    VisibleSerial();
                }
            }
            else
            {
                if (ddlUnit.Text.Equals("PCK"))
                {
                    VisibleCounter();
                }
                else
                {
                    HideCounter();
                }
            }


            if (!tbLocation.Text.Equals("") && !tbBarcode.Text.Equals("") && currentCursor == scannedItemData.Count + 1)
            {
                try
                {
                    if (ddlUnit.Text.Equals("PCK"))
                    {
                        if (tempSumPCS > 99999)
                        {
                            lbSumQtyPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                        }
                        else
                        {
                            lbSumQtyPCS.Text = "PCS : " + (tempSumPCS).ToString("N0").PadLeft(5, ' ');
                        }
                    }
                    else
                    {
                        int tempCal = tempSumPCS;// +Int32.Parse(tbQuantity.Text);
                        if (tempCal > 99999)
                        {
                            lbSumQtyPCS.Text = "PCS : " + ((tempCal / 1000).ToString() + "k").PadLeft(5, ' ');
                        }
                        else
                        {
                            lbSumQtyPCS.Text = "PCS : " + (tempCal).ToString("N0").PadLeft(5, ' ');
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumPCS > 99999)
                    {
                        lbSumQtyPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                    }
                    else
                    {
                        lbSumQtyPCS.Text = "PCS : " + (tempSumPCS).ToString("N0").PadLeft(5, ' ');
                    }
                }
            }
            FocusOnSuitableTextBox();
        }

        private void OpenScanner()
        {
            if (this.scanner != null && !this.scanner.PortOpen)
            {
                this.scanner.PortOpen = true;
            }
        }

        private void CloseScanner()
        {
            if (this.scanner != null)
            {
                this.scanner.PortOpen = false;
            }
        }

        private void tbQuantity_GotFocus(object sender, EventArgs e)
        {
            CloseScanner();
        }

        private void VisibleCounter()
        {
            constCounter.Location = new System.Drawing.Point(3, 107);
            tbCounter.Location = new System.Drawing.Point(153, 100);

            constQuan.Location = new System.Drawing.Point(3, 64);
            tbQuantity.Location = new System.Drawing.Point(79, 64);
            ddlUnit.Location = new System.Drawing.Point(169, 64);

            constCounter.Visible = true;
            tbCounter.Visible = true;
            tbCounter.Text = "";
        }

        private void HideCounter()
        {
            constCounter.Visible = false;
            tbCounter.Visible = false;
            tbCounter.Text = "";
        }

        private void VisibleSerial()
        {
            constSN.Visible = true;
            tbSerial.Visible = true;
            snCheck.Visible = true;
            tbSerial.Text = "";

            constSN.Location = new System.Drawing.Point(2, 69);
            tbLocation.Location = new System.Drawing.Point(42, 64);
            snCheck.Location = new System.Drawing.Point(210, 68);

            constQuan.Location = new System.Drawing.Point(3, 100);
            tbQuantity.Location = new System.Drawing.Point(79, 100);
            ddlUnit.Location = new System.Drawing.Point(169, 100);
        }

        private void HideSerial()
        {
            constSN.Visible = false;
            tbSerial.Visible = false;
            snCheck.Visible = false;
            tbSerial.Text = "";
        }

        private bool EnableSerial(string barcode)
        {
            if (IsHaveSerialNumber(barcode))
            {
                tbSerial.Text = "";
                tbSerial.Enabled = true;
                tbSerial.BackColor = Color.FromArgb(255, 255, 255);
                snCheck.Checked = true;

                MessageBox.Show("Please scan serial number", "Warning", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return true;
            }
            else
            {
                //tbSerial.Text = "";
                //tbSerial.Enabled = false;
                //tbSerial.BackColor = Color.FromArgb(223, 223, 223);
                //snCheck.Checked = false;
                return false;
            }
        }

        private bool DisableSerial()
        {

            tbSerial.Text = "";
            tbSerial.Enabled = false;
            tbSerial.BackColor = Color.FromArgb(223, 223, 223);
            snCheck.Checked = false;
            return true;

        }

        private bool IsHaveSerialNumber(string Barcode)
        {
            return DatabaseModule.Instance.IsHaveSerialNumber(Barcode);
        }

        private bool IsRightSerialNumber(string Barcode, string Serial)
        {
            return DatabaseModule.Instance.IsRightSerialNumber(Barcode, Serial);
        }

        private void snCheck_CheckStateChanged(object sender, EventArgs e)
        {
            if (snCheck.Checked)
            {
                tbSerial.Enabled = true;
                tbSerial.BackColor = Color.FromArgb(255, 255, 255);
            }
            else
            {
                tbSerial.Enabled = false;
                tbSerial.BackColor = Color.FromArgb(223, 223, 223);
            }
            FocusOnSuitableTextBox();
        }

        private void tbCounter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)System.Windows.Forms.Keys.Back || char.IsDigit(e.KeyChar))
            {
                if (e.KeyChar == '0' && tbCounter.Text.Length == 0)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }
    }

    public enum ScanProductMode : int
    {
        ScanOnly = 1, ScanQty = 2, ScanPackOnly = 3, ScanPackQty = 4
    }

    public class SerialNumberModel
    {
        public string SKUCode { get; set; }
        public string Barcode { get; set; }
        public string SerialNumber { get; set; }
        public string StorageLocation { get; set; }
    }

    public class StockTakingModel
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
        public string BrandCode { get; set; }
        public bool SKUMode { get; set; }
        public string DepartmentCode { get; set; }
        public bool SendFlag { get; set; }
        public string SerialNumber { get; set; }
        public string ConversionCounter { get; set; }
    }

    public class LocationModel
    {
        public string LocationCode { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string BrandCode { get; set; }
    }

    public class SKUModel
    {
        public string Department { get; set; }
        public string Barcode { get; set; }
        public string Flag { get; set; }
        public string SKUCode { get; set; }
        public string BrandCode { get; set; }
        public string ExBarcode { get; set; }
        public string InBarcode { get; set; }
        public string Description { get; set; }
        public bool IsNew { get; set; }
        public string DepartmentCode { get; set; }
        //public string SerialNumber { get; set; }
        //public string ConversionCounter { get; set; }
    }

    public class UnitModel
    {
        public int UnitCode { get; set; }
        public string UnitName { get; set; }
        public string CodeType { get; set; }
    }
}


