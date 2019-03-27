using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DNWA.BHTCL;
using Denso_HHT.Module;
using System.Text.RegularExpressions;
using System.Threading;

namespace Denso_HHT
{
    public partial class ScanFreshFood : Form
    {
        private List<StockTakingModel> scannedItemData = new List<StockTakingModel>();
        private LocationModel tempLocationModel = new LocationModel();
        private SKUModel tempSKUModel = new SKUModel();

        //it's a position, not a index
        private int currentCursor = 1;

        private bool isHaveLocationData = false;
        private bool isHaveSKUData = false;

        private int tempSumRec = 0;
        private int tempSumPCS = 0;
        private double tempSumKG = 0;

        private string currentLocation = "";
        private string currentBrand = null;

        private bool blockScannerPortState = false;

        private ScanFreshFoodMode scanFreshFoodMode;

        private Scanner scanner = new Scanner();

        public ScanFreshFood(ScanFreshFoodMode scanFreshFoodMode)
        {
            InitializeComponent();

            hhtToolBar1.HideDateTime();

            this.scanFreshFoodMode = scanFreshFoodMode;

            this.scanner.OnDone += new EventHandler(this.Scanner_OnDone1);

            SetupUI();
        }

        private void SetupUI()
        {
            scannedItemData = DatabaseModule.Instance.QuerySelectPreviousAuditDataFromScan((int)(this.scanFreshFoodMode));
            currentCursor = scannedItemData.Count + 1;
            isHaveLocationData = DatabaseModule.Instance.QuerySelectHaveLocationFromScan();
            isHaveSKUData = DatabaseModule.Instance.QuerySelectHaveSKUMasterFromScan();
            ddlUnit.DataSource = DatabaseModule.Instance.QuerySelectUnitFromScan("F");
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

            //if (!Program.LastScannedLocationFreshFood.Equals(""))
            //{
            //    ScanLocation(Program.LastScannedLocationFreshFood);
            //    if (scanFreshFoodMode != ScanFreshFoodMode.ScanWeight)
            //    {
            //        tbBarcode.Text = Program.LastScannedBarcodeFreshFood;
            //    }
            //}

            btnNext.Text = "Save";
            btnDelete.Enabled = false;
            switch (this.scanFreshFoodMode)
            {
                case ScanFreshFoodMode.ScanOnly:
                    const8.Text += "Scan Only";
                    tbBarcode.MaxLength = 14;
                    tbQuantity.Text = "1";
                    tbQuantity.Enabled = false;
                    tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    ddlUnit.SelectedIndex = 0;
                    hhtToolBar1.BackColor = Color.FromArgb(223, 223, 223);
                    break;
                case ScanFreshFoodMode.ScanQty:
                    const8.Text += "Scan Qty";
                    tbBarcode.MaxLength = 14;
                    tbQuantity.Text = "";
                    tbQuantity.Enabled = true;
                    tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                    ddlUnit.SelectedIndex = 0;
                    hhtToolBar1.BackColor = Color.FromArgb(223, 223, 223);
                    break;
                case ScanFreshFoodMode.ScanWeight:
                    const8.Text += "Scan Weight";
                    tbQuantity.Text = "";
                    tbQuantity.Enabled = true;
                    tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                    tbQuantity.MaxLength = 9;
                    tbQuantity.Font = new Font("Tahoma", 20, FontStyle.Bold);
                    int originalHeight = tbQuantity.Size.Height;
                    tbQuantity.Size = new Size(154, originalHeight);
                    ddlUnit.SelectedIndex = 1;
                    ddlUnit.Visible = false;
                    hhtToolBar1.BackColor = Color.FromArgb(223, 223, 223);
                    lbSumPCS.Visible = true;
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
                if (scanFreshFoodMode == ScanFreshFoodMode.ScanOnly)
                {
                    if (barcode == tbBarcode.Text)
                    {
                        int firstQty = Int32.Parse(tbQuantity.Text);
                        firstQty++;
                        tbQuantity.Text = firstQty.ToString();
                    }
                    //else if (!tbBarcode.Text.Equals(""))
                    //{
                    //    btnNext_Click(null, null);
                    //}

                    if (barcode.Length > 14)
                    {
                        MessageBox.Show("Invalid Barcode", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        tbBarcode.Text = "";
                    }
                    else
                    {
                        tbBarcode.Text = barcode;
                        Program.LastScannedBarcodeFreshFood = barcode;
                        btnNext_ClickAction();
                    }
                }
                else if (scanFreshFoodMode == ScanFreshFoodMode.ScanWeight)
                {
                    if (barcode.Length != 18)
                    {
                        tbQuantity.Text = "";
                        tbQuantity.Enabled = true;
                        tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                        tbBarcode.Text = barcode;
                        ddlUnit.SelectedIndex = 0;

                        //MessageBox.Show("Invalid Barcode", "Error", MessageBoxButtons.OK,
                        //MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        //tbBarcode.Text = "";
                    }
                    else
                    {
                        tbBarcode.Text = barcode.Substring(1, 5);
                        string barcodeWeight = barcode.Substring(12, 5);
                        string beforePoint = barcodeWeight.Substring(0, 2);
                        string afterPoint = barcodeWeight.Substring(2, 3);
                        tbQuantity.Text = Int32.Parse(beforePoint).ToString() + "." + afterPoint;
                        tbQuantity.Enabled = false;
                        tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    }

                    if (tbBarcode.Text.Length == 5)
                    {
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
                        Program.LastScannedBarcodeFreshFood = barcode;
                    }
                }
            }

            if (!scanner.PortOpen)
            {
                try { this.scanner.PortOpen = true; }
                catch { }
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
                //tempLocationModel.ScanMode = (int)(this.scanFreshFoodMode);
                tempLocationModel.LocationCode = barcode;

                Program.LastScannedLocationFreshFood = barcode;
            }

            else
            {
                Loading2.OpenLoading();

                LocationModel result = DatabaseModule.Instance.QueryLocationFromScan(barcode, 3);
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
                    Program.LastScannedLocationFreshFood = barcode;
                    Loading2.CloseLoading();
                }
                else
                {
                    tbLocation.Text = barcode;
                    result = DatabaseModule.Instance.QueryLocationFromScan(barcode, 4);
                    if (result != null)
                    {
                        Loading2.CloseLoading();
                        MessageBox.Show("Location code not found", "Warning", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        tbLocation.Text = "";
                    }
                    else
                    {
                        Loading2.CloseLoading();
                        MessageBox.Show("Your HHT can not audit this location code", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        tbLocation.Text = "";
                    }
                }
            }
        }

        private void ScanProduct(string barcode)
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
                Loading2.OpenLoading();
                SKUModel result = null;
                //if (barcode.Length == 18)
                //{
                //    result = DatabaseModule.Instance.QueryProductFromScan(barcode.Substring(1, 5));
                //}
                //else
                //{
                    result = DatabaseModule.Instance.QueryProductFromScan(barcode);
                //}

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
                Loading2.CloseLoading();
            }
        }

        private void UpdateSummaryQty(decimal addition, int unitCode, int mode)
        {
            if (string.IsNullOrEmpty(tbLocation.Text))
            {
                lbSumRec.Text = "REC : ";
                lbSumPCS.Text = "PCS : ";
                lbSumKG.Text = "KG : ";
            }
            else
            {
                if (mode != 0)
                {
                    DatabaseModule.Instance.QueryUpdateTotalByLocationFromScan(tbLocation.Text, unitCode, addition, mode);
                }

                string[] sumPCS = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "PCS");
                string[] sumPCK = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "PCK");
                string[] sumKG = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "KG");

                tempSumRec = Int32.Parse(sumPCS[1]) + Int32.Parse(sumPCK[1]) + Int32.Parse(sumKG[1]);
                tempSumPCS = Int32.Parse(sumPCS[0]);
                tempSumKG = Double.Parse(sumKG[0]);

                try
                {
                    if (!tbBarcode.Text.Equals("") && currentCursor == scannedItemData.Count + 1)
                    {
                        int tempCal;
                        double tempKGCal;

                        switch (ddlUnit.Text)
                        {
                            case "PCS":
                                tempCal = tempSumPCS;// +Int32.Parse(tbQuantity.Text);

                                if (tempCal > 99999)
                                {
                                    lbSumPCS.Text = "PCS : " + ((tempCal / 1000).ToString() + "k").PadLeft(5, ' ');
                                }
                                else
                                {
                                    lbSumPCS.Text = string.Format("PCS : {0}", (tempCal).ToString("N0")).PadLeft(5, ' ');
                                }
                                break;

                            case "KG":
                                tempKGCal = tempSumKG;// +Double.Parse(tbQuantity.Text);
                                if (tempSumKG > 99999)
                                {
                                    lbSumKG.Text = "KG : " + ((tempKGCal / 1000).ToString() + "k").PadLeft(5, ' ');
                                }
                                else
                                {
                                    lbSumKG.Text = string.Format("KG : {0}", tempKGCal.ToString("N3")).PadLeft(5, ' ');
                                }
                                break;
                        }
                        
                        if (tempSumRec > 99999)
                        {
                            lbSumRec.Text = "REC : " + (((tempSumRec) / 1000).ToString() + "k").PadLeft(5, ' ');                            
                        }
                        else
                        {
                            lbSumRec.Text = string.Format("REC : {0}", (tempSumRec).ToString("N0")).PadLeft(5, ' ');
                        }
                    }
                    else
                    {
                        if (tempSumRec > 99999)
                        {
                            lbSumRec.Text = "REC : " + ((tempSumRec / 1000).ToString() + "k").PadLeft(5, ' ');                            
                        }
                        else
                        {
                            lbSumRec.Text = string.Format("REC : {0}", tempSumRec.ToString("N0")).PadLeft(5, ' ');
                        }

                        if (tempSumPCS > 99999)
                        {
                            lbSumPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                        }
                        else
                        {
                            lbSumPCS.Text = string.Format("PCS : {0}", tempSumPCS.ToString("N0")).PadLeft(5, ' ');
                        }

                        if (tempSumKG > 99999)
                        {
                            lbSumKG.Text = "KG : " + ((tempSumKG / 1000).ToString() + "k").PadLeft(5, ' ');
                        }
                        else
                        {
                            lbSumKG.Text = string.Format("KG : {0}", tempSumKG.ToString("N3")).PadLeft(5, ' ');
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumRec > 99999)
                    {
                        lbSumRec.Text = "REC : " + ((tempSumRec / 1000).ToString() + "k").PadLeft(5, ' ');                            
                    }
                    else
                    {
                        lbSumRec.Text = string.Format("REC : {0}", tempSumRec.ToString("N0")).PadLeft(5, ' ');
                    }

                    if (tempSumPCS > 99999)
                    {
                        lbSumPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                    }
                    else
                    {
                        lbSumPCS.Text = string.Format("PCS : {0}", tempSumPCS.ToString("N0")).PadLeft(5, ' ');
                    }

                    if (tempSumKG > 99999)
                    {
                        lbSumKG.Text = "KG : " + ((tempSumKG / 1000).ToString() + "k").PadLeft(5, ' ');
                    }
                    else
                    {
                        lbSumKG.Text = string.Format("KG : {0}", tempSumKG.ToString("N3")).PadLeft(5, ' ');
                    }
                }
            }
        }

        private void UpdateSummaryQty(decimal oldQuantity, decimal newQuantity, int oldUnitcode, int newUnitCode)
        {

            if (string.IsNullOrEmpty(tbLocation.Text))
            {
                lbSumRec.Text = "REC : ";
                lbSumPCS.Text = "PCS : ";
                lbSumKG.Text = "KG : ";
            }
            else
            {
                string oldUnitName = DatabaseModule.Instance.QuerySelectUnitNameFromScan(oldUnitcode);
                if (oldUnitName.Equals(ddlUnit.Text))
                {
                    newQuantity = -(oldQuantity - newQuantity);
                }
                DatabaseModule.Instance.QueryUpdateTotalByLocationFromScan(tbLocation.Text, oldUnitcode, newUnitCode, oldQuantity, newQuantity);

                string[] sumPCS = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "PCS");
                string[] sumPCK = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "PCK");
                string[] sumKG = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "KG");

                tempSumRec = Int32.Parse(sumPCS[1]) + Int32.Parse(sumPCK[1]) + Int32.Parse(sumKG[1]);
                tempSumPCS = Int32.Parse(sumPCS[0]);
                tempSumKG = Double.Parse(sumKG[0]);

                try
                {
                    if (!tbBarcode.Text.Equals("") && currentCursor == scannedItemData.Count + 1)
                    {
                        int tempCal;
                        double tempKGCal;

                        switch (ddlUnit.Text)
                        {
                            case "PCS":
                                tempCal = tempSumPCS;// +Int32.Parse(tbQuantity.Text);
                                if (tempCal > 99999)
                                {
                                    lbSumPCS.Text = "PCS : " + ((tempCal / 1000).ToString() + "k").PadLeft(5, ' ');
                                }
                                else
                                {
                                    lbSumPCS.Text = string.Format("PCS : {0}", (tempCal).ToString("N0")).PadLeft(5, ' ');
                                }
                                break;

                            case "KG":

                                tempKGCal = tempSumKG;// +Double.Parse(tbQuantity.Text);
                                if (tempSumKG > 99999)
                                {
                                    lbSumKG.Text = "KG : " + ((tempKGCal / 1000).ToString() + "k").PadLeft(5, ' ');
                                }
                                else
                                {
                                    lbSumKG.Text = string.Format("KG : {0}", tempKGCal.ToString("N3")).PadLeft(5, ' ');
                                }
                                break;
                        }

                        if (tempSumRec > 99999)
                        {
                            lbSumRec.Text = "REC : " + ((tempSumRec / 1000).ToString() + "k").PadLeft(5, ' ');
                        }
                        else
                        {
                            lbSumRec.Text = string.Format("REC : {0}", (tempSumRec).ToString("N0")).PadLeft(5, ' ');
                        }
                    }
                    else
                    {
                        if (tempSumRec > 99999)
                        {
                            lbSumRec.Text = "REC : " + ((tempSumRec / 1000).ToString() + "k").PadLeft(5, ' ');                            
                        }
                        else
                        {
                            lbSumRec.Text = string.Format("REC : {0}", tempSumRec.ToString("N0")).PadLeft(5, ' ');
                        }

                        if (tempSumPCS > 99999)
                        {
                            lbSumPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                        }
                        else
                        {
                            lbSumPCS.Text = string.Format("PCS : {0}", tempSumPCS.ToString("N0")).PadLeft(5, ' ');
                        }

                        if (tempSumKG > 99999)
                        {
                            lbSumKG.Text = "KG : " + ((tempSumKG / 1000).ToString() + "k").PadLeft(5, ' ');
                        }
                        else
                        {
                            lbSumKG.Text = string.Format("KG : {0}", tempSumKG.ToString("N3")).PadLeft(5, ' ');
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumRec > 99999)
                    {
                        lbSumRec.Text = "REC : " + ((tempSumRec / 1000).ToString() + "k").PadLeft(5, ' ');                            
                    }
                    else
                    {
                        lbSumRec.Text = string.Format("REC : {0}", tempSumRec.ToString("N0")).PadLeft(5, ' ');
                    }

                    if (tempSumPCS > 99999)
                    {
                        lbSumPCS.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                    }
                    else
                    {
                        lbSumPCS.Text = string.Format("PCS : {0}", tempSumPCS.ToString("N0")).PadLeft(5, ' ');
                    }

                    if (tempSumKG > 99999)
                    {
                        lbSumKG.Text = "KG : " + ((tempSumKG / 1000).ToString() + "k").PadLeft(5, ' ');
                    }
                    else
                    {
                        lbSumKG.Text = string.Format("KG : {0}", tempSumKG.ToString("N3")).PadLeft(5, ' ');
                    }
                }
            }
        }

        private void PrepareToNextScan()
        {
            switch (this.scanFreshFoodMode)
            {
                case ScanFreshFoodMode.ScanOnly:
                    tbLocation.Text = currentLocation;
                    tbLocation.Enabled = true;
                    tbLocation.BackColor = Color.FromArgb(255, 255, 255);
                    tbBarcode.Text = "";
                    tbBarcode.Enabled = true;
                    tbBarcode.BackColor = Color.FromArgb(255, 255, 255);
                    tbQuantity.Text = "1";
                    tbQuantity.Enabled = false;
                    tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    ddlUnit.SelectedIndex = 0;
                    btnNext.Text = "Save";
                    btnDelete.Enabled = false;
                    btnClear.Enabled = true;
                    FocusOnSuitableTextBox();
                    break;
                case ScanFreshFoodMode.ScanQty:
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
                case ScanFreshFoodMode.ScanWeight:
                    tbLocation.Text = currentLocation;
                    tbLocation.Enabled = true;
                    tbLocation.BackColor = Color.FromArgb(255, 255, 255);
                    tbBarcode.Text = "";
                    tbBarcode.Enabled = true;
                    tbBarcode.BackColor = Color.FromArgb(255, 255, 255);
                    tbQuantity.Text = "";
                    tbQuantity.Enabled = true;
                    tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                    ddlUnit.SelectedIndex = 1;
                    btnNext.Text = "Save";
                    btnDelete.Enabled = false;
                    btnClear.Enabled = true;
                    FocusOnSuitableTextBox();
                    break;
            }
        }

        private void PrepareToPreview()
        {
            StockTakingModel tempItemData = scannedItemData[currentCursor - 1];
            tbLocation.Text = tempItemData.LocationCode;
            tbBarcode.Text = tempItemData.Barcode;

            if (tempItemData.UnitCode != 5)
            {
                tbQuantity.Text = ((int)tempItemData.Quantity).ToString();
            }
            else
            {
                tbQuantity.Text = tempItemData.Quantity.ToString("F3");
            }

            tbLocation.Enabled = false;
            tbLocation.BackColor = Color.FromArgb(223, 223, 223);
            tbBarcode.Enabled = false;
            tbBarcode.BackColor = Color.FromArgb(223, 223, 223);
            tbQuantity.Enabled = true;
            tbQuantity.BackColor = Color.FromArgb(255, 255, 255);

            ddlUnit.SelectedIndex = FindDDLIndex(tempItemData.UnitCode);

            btnNext.Text = "Next >>";
            btnDelete.Enabled = true;
            btnClear.Enabled = false;

            FocusOnSuitableTextBox();
        }

        private void FocusOnSuitableTextBox()
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
            else if (tbQuantity.Text.Equals(""))
            {
                if (!tbQuantity.Focused)
                {
                    tbQuantity.Focus();
                }
                tbQuantity.SelectionStart = tbQuantity.Text.Length;
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

        private void ClearScreen()
        {
            tbLocation.Text = "";
            tbBarcode.Text = "";
            if (scanFreshFoodMode == ScanFreshFoodMode.ScanQty || scanFreshFoodMode == ScanFreshFoodMode.ScanWeight)
            {
                tbQuantity.Text = "";
            }
            else
            {
                tbQuantity.Text = "1";
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
                try
                {
                    if (CheckForUpdate(currentCursor))
                    {
                        UpdateAuditData(currentCursor);
                    }
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Quantity can not be other except for number", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
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
                try
                {
                    if (CheckForUpdate(currentCursor))
                    {
                        UpdateAuditData(currentCursor);
                    }
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Quantity can not be other except for number", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                }

                currentCursor = scannedItemData.Count + 1;
                PrepareToNextScan();
            }
        }

        private bool CheckForUpdate(int passCursor)
        {
            try
            {
                if (scannedItemData[passCursor - 1].Quantity
                    == Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"))
                    && scannedItemData[passCursor - 1].UnitCode == Convert.ToInt32(ddlUnit.SelectedValue))
                {
                    return false;
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
                    else
                    {
                        return true;
                    }
                }
            }
            catch (FormatException ex)
            {
                throw ex;
            }
        }

        private void UpdateAuditData(int passCursor)
        {
            try
            {
                decimal oldQuantity = scannedItemData[passCursor - 1].Quantity;
                int oldUnitCode = scannedItemData[passCursor - 1].UnitCode;
                decimal newQuantity;
                int newUnitCode = Convert.ToInt32(ddlUnit.SelectedValue);

                if (Convert.ToInt32(ddlUnit.SelectedValue) == 5)
                {
                    newQuantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                }
                else
                {
                    newQuantity = Math.Round(Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3")));
                }

                FreshfoodSaving.OpenSaving();               
                Thread.Sleep(100);
                
                if (!Program.isNonRealtime)
                {
                    APIModule.Instance.SendRequestThread(scannedItemData[passCursor - 1],
                        newQuantity, Convert.ToInt32(ddlUnit.SelectedValue), (int)(this.scanFreshFoodMode));
                    scannedItemData[passCursor - 1].Quantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                    scannedItemData[passCursor - 1].UnitCode = Convert.ToInt32(ddlUnit.SelectedValue);
                    UpdateSummaryQty(oldQuantity, scannedItemData[passCursor - 1].Quantity, oldUnitCode, Convert.ToInt32(ddlUnit.SelectedValue));
                    this.Show();
                }
                else
                {
                    DatabaseModule.Instance.QueryUpdateFromScan(scannedItemData[passCursor - 1].StocktakingID,
                        newQuantity, Convert.ToInt32(ddlUnit.SelectedValue), false, (int)(this.scanFreshFoodMode));
                    scannedItemData[passCursor - 1].Quantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                    scannedItemData[passCursor - 1].UnitCode = Convert.ToInt32(ddlUnit.SelectedValue);
                    UpdateSummaryQty(oldQuantity, scannedItemData[passCursor - 1].Quantity, oldUnitCode, Convert.ToInt32(ddlUnit.SelectedValue));
                }
                FreshfoodSaving.CloseSaving();
            }
            catch (Exception ex)
            {
                FreshfoodSaving.CloseSaving();
                MessageBox.Show("Update Error : " + ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private bool CheckForAdd()
        {
            if (tbLocation.Text.Equals("") || tbBarcode.Text.Equals("") || tbQuantity.Text.Equals(""))
            {
                MessageBox.Show("Location, Barcode and Quantity must not empty", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
            else if (tbLocation.Text.Length != 5)
            {
                MessageBox.Show("Invalid Location Code", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                tbLocation.Text = "";
                return false;
            }
            //else if (tbBarcode.Text.Length != 5 && tbBarcode.Text.Length != 13
            //    && tbBarcode.Text.Length != 14 && tbBarcode.Text.Length != 18)
            //{
            //    MessageBox.Show("Invalid Barcode for saving", "Warning", MessageBoxButtons.OK,
            //        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            //    return false;
            //}
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

                    return true;
                }
                catch (FormatException ex)
                {
                    throw ex;
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
                itemData.UnitCode = Convert.ToInt32(ddlUnit.SelectedValue);

                if (itemData.UnitCode == 5)
                {
                    itemData.Quantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                }
                else
                {
                    itemData.Quantity = Math.Round(Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3")));
                }

                itemData.ScanMode = (int)(this.scanFreshFoodMode);
                itemData.SKUCode = tempSKUModel.SKUCode;
                itemData.SKUMode = isHaveSKUData;
                itemData.DepartmentCode = tempSKUModel.DepartmentCode;
                itemData.SerialNumber = null;
                itemData.ConversionCounter = null;

                FreshfoodSaving.OpenSaving();
                Thread.Sleep(100);

                if (!Program.isNonRealtime)
                {
                    APIModule.Instance.SendRequestThread(itemData, (int)(this.scanFreshFoodMode));
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
                    if (lastInsertedStocktaking != null && DatabaseModule.Instance.QueryUpdateLastStocktaking(lastInsertedStocktaking, itemData, (int)(this.scanFreshFoodMode)))
                    {
                        scannedItemData[scannedItemData.Count - 1].Quantity = lastInsertedStocktaking.Quantity + itemData.Quantity;
                        currentCursor--;
                        UpdateSummaryQty(lastInsertedStocktaking.Quantity,scannedItemData[scannedItemData.Count-1].Quantity,lastInsertedStocktaking.UnitCode,lastInsertedStocktaking.UnitCode);
                    }
                    else
                    {
                        DatabaseModule.Instance.QueryInsertFromScan(itemData, (int)(this.scanFreshFoodMode));
                        scannedItemData.Add(itemData);

                        if (scannedItemData.Count == 101)
                        {
                            scannedItemData.RemoveAt(0);
                            currentCursor--;
                        }

                        UpdateSummaryQty(itemData.Quantity, Convert.ToInt32(ddlUnit.SelectedValue), 1);
                    }

                    //DatabaseModule.Instance.QueryInsertFromScan(itemData, 3);
                    //scannedItemData.Add(itemData);

                    //if (scannedItemData.Count == 101)
                    //{
                    //    scannedItemData.RemoveAt(0);
                    //    currentCursor--;
                    //}

                }

                FreshfoodSaving.CloseSaving();
            }
            catch (InvalidCastException ex)
            {

                FreshfoodSaving.CloseSaving();
                MessageBox.Show("The application encountered some problem about network, please try again.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                throw ex;
            }
            catch (Exception ex)
            {
                FreshfoodSaving.CloseSaving();
                
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

            DatabaseModule.Instance.QueryDeleteFromScan(scannedItemData[currentCursor - 1].StocktakingID, (int)(this.scanFreshFoodMode));
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
            if (tbLocation.Text.Equals("") || tbBarcode.Text.Equals("") || tbQuantity.Text.Equals(""))
            {
                MessageBox.Show("Location, Barcode and Quantity must not empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                FocusOnSuitableTextBox();
            }
            else
            {
                if (this.btnNext.Text.Equals("Save"))
                {
                    if (tbBarcode.Text.Length != 5)
                    { ddlUnit.SelectedIndex = 0; }
                    
                    blockScannerPortState = true;
                    Program.LastScannedBarcodeFreshFood = "";
                    this.scanner.PortOpen = false;
                    btnNext_ClickAction();
                    if (!this.scanner.PortOpen)
                    {
                        try { this.scanner.PortOpen = true; }
                        catch { }
                    }
                    blockScannerPortState = false;
                }
                else
                {
                    btnNext_ClickAction();
                }
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
                        ScanProduct(tbBarcode.Text);

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
            if (!string.IsNullOrEmpty(tbLocation.Text) && !string.IsNullOrEmpty(tbBarcode.Text) && !string.IsNullOrEmpty(tbQuantity.Text))
            {
                btnNext_ClickAction();
            }
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
            if(!string.IsNullOrEmpty(tbLocation.Text) && !string.IsNullOrEmpty(tbBarcode.Text) && !string.IsNullOrEmpty(tbQuantity.Text))
            {
                if (tbBarcode.Text.Length != 5)
                { ddlUnit.SelectedIndex = 0; }
                
                btnNext_ClickAction();
                Program.LastScannedBarcodeFreshFood = "";
            }
            this.DialogResult = DialogResult.Abort;
            this.Dispose();
        }

        private void tbQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ("KG".Equals(ddlUnit.Text))
            {
                if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == '.')
                {
                    if (e.KeyChar == '.' && tbQuantity.Text.IndexOf('.') > -1)
                    {
                        e.Handled = true;
                        return;
                    }

                    if (scanFreshFoodMode == ScanFreshFoodMode.ScanWeight)
                    {
                        if (Regex.IsMatch(tbQuantity.Text, @"\d\d\d\d\d") && char.IsDigit(e.KeyChar) && !tbQuantity.Text.Contains("."))
                        {
                            e.Handled = true;
                            return;
                        }

                        if (tbQuantity.Text.IndexOf('.') > -1)
                        {
                            if (Regex.IsMatch(tbQuantity.Text, @"\.\d\d\d") && !char.IsControl(e.KeyChar))
                            {
                                e.Handled = true;
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(tbQuantity.Text, @"\d\d\d\d") && char.IsDigit(e.KeyChar) && !tbQuantity.Text.Contains("."))
                        {
                            e.Handled = true;
                            return;
                        }

                        if (tbQuantity.Text.IndexOf('.') > -1)
                        {
                            if (Regex.IsMatch(tbQuantity.Text, @"\.\d\d\d") && !char.IsControl(e.KeyChar))
                            {
                                e.Handled = true;
                                return;
                            }
                        }
                    }       
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
                {
                    //if (e.KeyChar == '0' && tbQuantity.Text.Length == 0)
                    //{
                    //    e.Handled = true;
                    //}
                }
                else
                {
                    e.Handled = true;
                }
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
            switch (tbBarcode.Text.Length)
            {
                case 18:
                    ScanProduct(tbBarcode.Text);
                    string barcodeWeight = tbBarcode.Text.Substring(12, 5);
                    string beforePoint = barcodeWeight.Substring(0, 2);
                    string afterPoint = barcodeWeight.Substring(2, 3);
                    tbQuantity.Text = Int32.Parse(beforePoint).ToString() + "." + afterPoint;
                    tbQuantity.Enabled = false;
                    tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    tbBarcode.Text = tbBarcode.Text.Substring(1, 5);
                    break;
                default:
                    if (scanFreshFoodMode == ScanFreshFoodMode.ScanOnly)
                    {
                        tbQuantity.Text = "1";
                        tbQuantity.Enabled = false;
                        tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    }
                    else if (scanFreshFoodMode == ScanFreshFoodMode.ScanQty)
                    {
                        tbQuantity.Text = "";
                        tbQuantity.Enabled = true;
                        tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    break;
            }
            //UpdateSummaryQty(0, 0, 0);
        }

        private void tbLocation_GotFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                if (!scanner.PortOpen)
                {
                    try { this.scanner.PortOpen = true; }
                    catch { }
                }
            }
        }

        private void tbLocation_LostFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                try { this.scanner.PortOpen = false; }
                catch { }
            }
        }

        private void tbBarcode_GotFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                if (!scanner.PortOpen)
                {
                    try { this.scanner.PortOpen = true; }
                    catch { }
                }
            }
        }

        private void tbBarcode_LostFocus(object sender, EventArgs e)
        {
            if (this.scanner != null && !blockScannerPortState)
            {
                try { this.scanner.PortOpen = false; }
                catch { }
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
                lbSumRec.Text = "REC : ";
                lbSumPCS.Text = "PCS : ";
                lbSumKG.Text = "KG : ";
            }
        }

        private void ScanFreshFood_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Enter:

                    if (tbLocation.Text.Length != 5)
                    {
                        MessageBox.Show("Location Code must be 5 digits", "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        tbLocation.Text = "";
                    }
                    else if (tbLocation.Text.Equals("") || tbBarcode.Text.Equals("") || tbQuantity.Text.Equals(""))
                    {
                        FocusOnSuitableTextBox();
                    }
                    else
                    {
                        if (this.btnNext.Text.Equals("Save"))
                        {
                            if (tbBarcode.Text.Length != 5)
                            { ddlUnit.SelectedIndex = 0; }

                            blockScannerPortState = true;
                            Program.LastScannedBarcodeFreshFood = "";
                            this.scanner.PortOpen = false;
                            btnNext_ClickAction();
                            if (!this.scanner.PortOpen)
                            {
                                try { this.scanner.PortOpen = true; }
                                catch { }
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

                            //MessageBox.Show("You are not on the state that can be save", "Error", MessageBoxButtons.OK,
                            //    MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
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
            FocusOnSuitableTextBox();
        }

        private void OpenScanner()
        {
            if (this.scanner != null && !this.scanner.PortOpen)
            {
                try { this.scanner.PortOpen = true; }
                catch { }
            }
        }

        private void CloseScanner()
        {
            if (this.scanner != null)
            {
                try { this.scanner.PortOpen = false; }
                catch { }
            }
        }

        private void tbQuantity_GotFocus(object sender, EventArgs e)
        {
            CloseScanner();
        }
    }

    public enum ScanFreshFoodMode : int
    {
        ScanOnly = 5, ScanQty = 6, ScanWeight = 7
    }

}