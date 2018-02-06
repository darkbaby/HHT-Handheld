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
    public partial class ScanFront : Form
    {
        private List<StockTakingModel> scannedItemData = new List<StockTakingModel>();
        private LocationModel tempLocationModel = new LocationModel();
        private SKUModel tempSKUModel = new SKUModel();
        private int currentCursor = 1;

        private bool isHaveLocationData = false;
        private bool isHaveSKUData = false;

        private int tempSumPCS = 0;

        private string currentLocation = "";
        private string currentBrand = null;

        private bool blockScannerPortState = false;

        private ScanFrontMode scanFrontMode;

        private Scanner scanner = new Scanner();

        public ScanFront(ScanFrontMode scanFrontMode)
        {
            InitializeComponent();

            hhtToolBar1.HideDateTime();

            this.scanFrontMode = scanFrontMode;

            this.scanner.OnDone += new EventHandler(this.Scanner_OnDone1);

            SetupUI();
        }

        private void SetupUI()
        {
            scannedItemData = DatabaseModule.Instance.QuerySelectPreviousAuditDataFromScan(1);
            currentCursor = scannedItemData.Count + 1;
            isHaveLocationData = DatabaseModule.Instance.QuerySelectHaveLocationFromScan();
            isHaveSKUData = DatabaseModule.Instance.QuerySelectHaveSKUMasterFromScan();
            List<UnitModel> tempUnitModel = DatabaseModule.Instance.QuerySelectUnitFromScan("N");
            tempUnitModel.RemoveAt(1);
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

            if (!Program.LastScannedLocationFront.Equals(""))
            {
                ScanLocation(Program.LastScannedLocationFront);
                tbBarcode.Text = Program.LastScannedBarcodeFront;
            }

            btnNext.Text = "Save";
            ddlUnit.SelectedIndex = 0;
            btnDelete.Enabled = false;
            switch (this.scanFrontMode)
            {
                case ScanFrontMode.ScanOnly:
                    const8.Text += "Scan Only";
                    //const7.Visible = false;
                    //tbQuantity.Visible = false;
                    //ddlUnit.Visible = false;
                    tbQuantity.Text = "1";
                    tbQuantity.Enabled = false;
                    tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    hhtToolBar1.BackColor = Color.FromArgb(223, 223, 223);
                    break;
                case ScanFrontMode.ScanQty:
                    const8.Text += "Scan Qty";
                    tbQuantity.Text = "";
                    tbQuantity.Enabled = true;
                    tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
                    hhtToolBar1.BackColor = Color.FromArgb(223, 223, 223);
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
                }
                else
                {
                    ScanLocation(barcode);
                }
            }
            else if (tbBarcode.Focused)
            {
                if (scanFrontMode == ScanFrontMode.ScanOnly)
                {
                    if (barcode == tbBarcode.Text)
                    {
                        int firstQty = Int32.Parse(tbQuantity.Text);
                        firstQty++;
                        tbQuantity.Text = firstQty.ToString();
                    }
                    else if (!tbBarcode.Text.Equals(""))
                    {
                        btnNext_ClickAction();
                    }
                }

                if (barcode.Length > 14)
                {
                    MessageBox.Show("Invalid Barcode", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    tbBarcode.Text = "";
                }
                else
                {
                    tbBarcode.Text = barcode;
                    Program.LastScannedBarcodeFront = barcode;
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
                tempLocationModel.ScanMode = 1;
                tempLocationModel.LocationCode = barcode;

                Program.LastScannedLocationFront = barcode;
            }
            else
            {
                Loading2.OpenLoading();

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
                        tempLocationModel.ScanMode = result.ScanMode;
                        tempLocationModel.LocationCode = result.LocationCode;
                    }
                    else
                    {
                        currentBrand = null;
                        currentLocation = result.LocationCode;

                        tbLocation.Text = result.LocationCode;

                        tempLocationModel.BrandCode = null;
                        tempLocationModel.ScanMode = result.ScanMode;
                        tempLocationModel.LocationCode = result.LocationCode;
                    }

                    Program.LastScannedLocationFront = barcode;
                }
                else
                {
                    result = DatabaseModule.Instance.QueryLocationFromScan(barcode, 4);
                    if (result != null)
                    {
                        Loading2.CloseLoading();
                        MessageBox.Show("Location code not found", "Warning", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        Loading2.CloseLoading();
                        MessageBox.Show("Your HHT can not audit this location code", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
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
                Loading2.CloseLoading();
            }
        }

        private void UpdateSummaryQty(decimal addition, int unitCode, int mode)
        {
            if (string.IsNullOrEmpty(tbLocation.Text))
            {
                lbSumQty.Text = "PCS : ";
            }
            else
            {
                if (mode != 0)
                {
                    DatabaseModule.Instance.QueryUpdateTotalByLocationFromScan(tbLocation.Text, unitCode, addition, mode);
                }

                string sumQtyPCS = DatabaseModule.Instance.QuerySelectTotalByLocationFromScan(tbLocation.Text, "PCS")[0];
                tempSumPCS = Int32.Parse(sumQtyPCS);

                try
                {
                    if (!tbBarcode.Text.Equals("") && currentCursor == scannedItemData.Count + 1)
                    {
                        int tempCal = tempSumPCS + Int32.Parse(tbQuantity.Text);
                        if (tempCal > 99999)
                        {
                            lbSumQty.Text = "PCS : " + ((tempCal / 1000).ToString() + "k").PadLeft(5, ' ');
                            //lbSumQty.Text = "PCS : 99,999+";
                        }
                        else
                        {
                            lbSumQty.Text = "PCS : " + (tempCal).ToString("N0").PadLeft(5, ' ');
                        }
                    }
                    else
                    {
                        if (tempSumPCS > 99999)
                        {
                            lbSumQty.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                            //lbSumQty.Text = "PCS : 99,999+";
                        }
                        else
                        {
                            lbSumQty.Text = "PCS : " + (tempSumPCS).ToString("N0").PadLeft(5, ' ');
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumPCS > 99999)
                    {
                        lbSumQty.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5,' ');
                        //lbSumQty.Text = "PCS : 99,999+";
                    }
                    else
                    {
                        lbSumQty.Text = "PCS : " + (tempSumPCS).ToString("N0").PadLeft(5, ' ');
                    }
                }
            }
        }

        private void UpdateSummaryQty(decimal oldQuantity, decimal newQuantity, int oldUnitcode, int newUnitCode)
        {
            if (string.IsNullOrEmpty(tbLocation.Text))
            {
                lbSumQty.Text = "PCS : ";
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
                tempSumPCS = Int32.Parse(sumQtyPCS);

                try
                {
                    if (!tbBarcode.Text.Equals("") && currentCursor == scannedItemData.Count + 1)
                    {
                        int tempCal = tempSumPCS + Int32.Parse(tbQuantity.Text);
                        if (tempCal > 99999)
                        {
                            lbSumQty.Text = "PCS : " + ((tempCal / 1000).ToString() + "k").PadLeft(5, ' ');
                            //lbSumQty.Text = "PCS : 99,999+";
                        }
                        else
                        {
                            lbSumQty.Text = "PCS : " + (tempCal).ToString("N0").PadLeft(5, ' ');
                        }
                    }
                    else
                    {
                        if (tempSumPCS > 99999)
                        {
                            lbSumQty.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                            //lbSumQty.Text = "PCS : 99,999+";
                        }
                        else
                        {
                            lbSumQty.Text = "PCS : " + (tempSumPCS).ToString("N0").PadLeft(5, ' ');
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumPCS > 99999)
                    {
                        lbSumQty.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                        //lbSumQty.Text = "PCS : 99,999+";
                    }
                    else
                    {
                        lbSumQty.Text = "PCS : " + (tempSumPCS).ToString("N0").PadLeft(5, ' ');
                    }
                }
            }
        }

        private void PrepareToNextScan()
        {
            switch (scanFrontMode)
            {
                case ScanFrontMode.ScanOnly:
                    tbLocation.Text = currentLocation;
                    tbLocation.Enabled = true;
                    tbLocation.BackColor = Color.FromArgb(255, 255, 255);
                    tbBarcode.Text = "";
                    tbBarcode.Enabled = true;
                    tbBarcode.BackColor = Color.FromArgb(255, 255, 255);
                    //const7.Visible = false;
                    //ddlUnit.Visible = false;
                    //tbQuantity.Visible = false;
                    tbQuantity.Text = "1";
                    tbQuantity.Enabled = false;
                    tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
                    ddlUnit.SelectedIndex = 0;
                    btnNext.Text = "Save";
                    btnDelete.Enabled = false;
                    btnClear.Enabled = true;
                    FocusOnSuitableTextBox();
                    break;
                case ScanFrontMode.ScanQty:
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
            if (scanFrontMode == ScanFrontMode.ScanQty)
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
                MessageBox.Show("Quantity can not be other except for number", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
        }

        private void UpdateAuditData(int passCursor)
        {
            try
            {
                decimal oldQuantity = scannedItemData[passCursor - 1].Quantity;
                int oldUnitCode = scannedItemData[passCursor - 1].UnitCode;
                decimal newQuantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                int newUnitCode = Convert.ToInt32(ddlUnit.SelectedValue);

                if (!Program.isNonRealtime)
                {
                    APIModule.Instance.SendRequestThread(scannedItemData[passCursor - 1],
                        Math.Round(Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"))),
                        Convert.ToInt32(ddlUnit.SelectedValue), 1);
                    scannedItemData[passCursor - 1].Quantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                    scannedItemData[passCursor - 1].UnitCode = Convert.ToInt32(ddlUnit.SelectedValue);
                    UpdateSummaryQty(oldQuantity, scannedItemData[passCursor - 1].Quantity, oldUnitCode, Convert.ToInt32(ddlUnit.SelectedValue));
                    this.Show();
                }
                else
                {
                    DatabaseModule.Instance.QueryUpdateFromScan(scannedItemData[passCursor - 1].StocktakingID,
                        Math.Round(Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"))),
                        Convert.ToInt32(ddlUnit.SelectedValue), false, 1);
                    scannedItemData[passCursor - 1].Quantity = Convert.ToDecimal(Convert.ToDecimal(tbQuantity.Text).ToString("F3"));
                    scannedItemData[passCursor - 1].UnitCode = Convert.ToInt32(ddlUnit.SelectedValue);
                    UpdateSummaryQty(oldQuantity, scannedItemData[passCursor - 1].Quantity, oldUnitCode, Convert.ToInt32(ddlUnit.SelectedValue));
                }
            }
            catch (Exception ex)
            {
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
            else if (tbLocation.Text.Length != 5)
            {
                MessageBox.Show("Invalid Location Code", "Warning", MessageBoxButtons.OK,
                     MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
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
                itemData.ScanMode = tempLocationModel.ScanMode;
                itemData.SKUCode = tempSKUModel.SKUCode;
                itemData.UnitCode = Convert.ToInt32(ddlUnit.SelectedValue);
                itemData.SKUMode = isHaveSKUData;
                itemData.DepartmentCode = tempSKUModel.DepartmentCode;

                if (!Program.isNonRealtime)
                {
                    APIModule.Instance.SendRequestThread(itemData, 1);
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
                    if (lastInsertedStocktaking != null && DatabaseModule.Instance.QueryUpdateLastStocktaking(lastInsertedStocktaking, itemData, 1))
                    {
                        scannedItemData[scannedItemData.Count - 1].Quantity = lastInsertedStocktaking.Quantity + itemData.Quantity;
                        currentCursor--;
                    }
                    else
                    {
                        DatabaseModule.Instance.QueryInsertFromScan(itemData, 1);
                        scannedItemData.Add(itemData);

                        if (scannedItemData.Count == 101)
                        {
                            scannedItemData.RemoveAt(0);
                            currentCursor--;
                        }
                    }

                    UpdateSummaryQty(itemData.Quantity, Convert.ToInt32(ddlUnit.SelectedValue), 1);
                }
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show("The application encountered some problem about network, please try again.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                throw ex;
            }
            catch (Exception ex)
            {
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

            DatabaseModule.Instance.QueryDeleteFromScan(scannedItemData[currentCursor - 1].StocktakingID, 1);
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
                Program.LastScannedBarcodeFront = "";
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
                            DialogResult result = MessageBox.Show("Barcode not found in [SKUMaster], do you want to save it?",
                                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                            if (result != DialogResult.Yes)
                            {
                                currentCursor--;
                                isGoingAction = false;
                            }
                            else
                            {
                                tempSKUModel.Flag = "F";
                                AddAuditData();
                            }
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
                                    DialogResult result = MessageBox.Show("This Barcode is not the same BrandCode on your current Location, do you want to save it?",
                                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                                    if (result != DialogResult.Yes)
                                    {
                                        currentCursor--;
                                        isGoingAction = false;
                                    }
                                    else
                                    {
                                        tempSKUModel.Flag = "F";
                                        AddAuditData();
                                    }
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
            if (scanFrontMode != ScanFrontMode.ScanOnly)
            {
                tbQuantity.Text = "";
                tbQuantity.Enabled = true;
                tbQuantity.BackColor = Color.FromArgb(255, 255, 255);
            }
            else
            {
                tbQuantity.Text = "1";
                tbQuantity.Enabled = false;
                tbQuantity.BackColor = Color.FromArgb(223, 223, 223);
            }
            UpdateSummaryQty(0, 0, 0);
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

        private void tbLocation_TextChanged(object sender, EventArgs e)
        {
            if (tbLocation.Text.Length == 5)
            {
                UpdateSummaryQty(0, 0, 0);
            }
            else
            {
                lbSumQty.Text = "PCS : ";
            }
        }

        private void tbQuantity_TextChanged(object sender, EventArgs e)
        {
            if (!tbLocation.Text.Equals("") && !tbBarcode.Text.Equals("") 
                    && currentCursor == scannedItemData.Count + 1 
                    && ddlUnit.Text.Equals("PCS"))
            {
                try
                {
                    int tempCal = tempSumPCS + Int32.Parse(tbQuantity.Text);
                    if (tempCal > 99999)
                    {
                        lbSumQty.Text = "PCS : " + ((tempCal / 1000).ToString() + "k").PadLeft(5, ' ');
                        //lbSumQty.Text = "PCS : 99,999+";
                    }
                    else
                    {
                        lbSumQty.Text = "PCS : " + (tempCal).ToString("N0").PadLeft(5, ' ');
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumPCS > 99999)
                    {
                        lbSumQty.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                        //lbSumQty.Text = "PCS : 99,999+";
                    }
                    else
                    {
                        lbSumQty.Text = "PCS : " + (tempSumPCS).ToString("N0").PadLeft(5, ' ');
                    }
                }
            }
        }

        private void ScanFront_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Enter:
                    if (tbLocation.Text.Equals("") || tbBarcode.Text.Equals("") || tbQuantity.Text.Equals(""))
                    {
                        FocusOnSuitableTextBox();
                    }
                    else
                    {
                        if (this.btnNext.Text.Equals("Save"))
                        {
                            blockScannerPortState = true;
                            Program.LastScannedBarcodeFront = "";
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
            if (!tbLocation.Text.Equals("") && !tbBarcode.Text.Equals("") 
                    && currentCursor == scannedItemData.Count + 1)
            {
                try
                {
                    if (ddlUnit.Text.Equals("PCK"))
                    {
                        if (tempSumPCS > 99999)
                        {
                            lbSumQty.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');   
                            //lbSumQty.Text = "PCS : 99,999+";
                        }
                        else
                        {
                            lbSumQty.Text = "PCS : " + (tempSumPCS).ToString("N0").PadLeft(5, ' ');
                        }
                    }
                    else
                    {
                        int tempCal = tempSumPCS + Int32.Parse(tbQuantity.Text);
                        if (tempCal > 99999)
                        {
                            lbSumQty.Text = "PCS : " + ((tempCal / 1000).ToString() + "k").PadLeft(5, ' ');
                            //lbSumQty.Text = "PCS : 99,999+";
                        }
                        else
                        {
                            lbSumQty.Text = "PCS : " + (tempCal).ToString("N0").PadLeft(5, ' ');
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tempSumPCS > 99999)
                    {
                        lbSumQty.Text = "PCS : " + ((tempSumPCS / 1000).ToString() + "k").PadLeft(5, ' ');
                        //lbSumQty.Text = "PCS : 99,999+";
                    }
                    else
                    {
                        lbSumQty.Text = "PCS : " + (tempSumPCS).ToString("N0").PadLeft(5, ' ');
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
    }

    public enum ScanFrontMode
    {
        ScanOnly, ScanQty
    }

    public class LocationModel
    {
        public string LocationCode { get; set; }
        public string SectionCode { get; set; }
        public int ScanMode { get; set; }
        public string Section { get; set; }
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
    }

    public class UnitModel
    {
        public int UnitCode { get; set; }
        public string UnitName { get; set; }
        public string CodeType { get; set; }
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
    }
}

