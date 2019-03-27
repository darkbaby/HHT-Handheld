namespace Denso_HHT
{
    partial class ScanProduct
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (scanner != null)
            {
                scanner.OnDone -= new System.EventHandler(this.Scanner_OnDone1);
                scanner.PortOpen = false;
                scanner.Dispose();
                scanner = null;
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ddlUnit = new System.Windows.Forms.ComboBox();
            this.tbQuantity = new System.Windows.Forms.TextBox();
            this.tbLocation = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.constMenu = new System.Windows.Forms.Label();
            this.constLoc = new System.Windows.Forms.Label();
            this.btnMainMenu = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.constQuan = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbCounter = new System.Windows.Forms.TextBox();
            this.constCounter = new System.Windows.Forms.Label();
            this.tbSerial = new System.Windows.Forms.TextBox();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.snCheck = new System.Windows.Forms.CheckBox();
            this.constSN = new System.Windows.Forms.Label();
            this.tbBarcode_ = new System.Windows.Forms.TextBox();
            this.constItem = new System.Windows.Forms.Label();
            this.lbSumQtyPCS = new System.Windows.Forms.Label();
            this.lbSumQtyPCK = new System.Windows.Forms.Label();
            this.lbFunction = new System.Windows.Forms.Label();
            this.lbAlpha = new System.Windows.Forms.Label();
            this.constSubMenu = new System.Windows.Forms.Label();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ddlUnit
            // 
            this.ddlUnit.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.ddlUnit.Location = new System.Drawing.Point(169, 100);
            this.ddlUnit.Name = "ddlUnit";
            this.ddlUnit.Size = new System.Drawing.Size(65, 31);
            this.ddlUnit.TabIndex = 111;
            this.ddlUnit.TabStop = false;
            this.ddlUnit.SelectedIndexChanged += new System.EventHandler(this.ddlUnit_SelectedIndexChanged);
            // 
            // tbQuantity
            // 
            this.tbQuantity.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.tbQuantity.Location = new System.Drawing.Point(79, 100);
            this.tbQuantity.MaxLength = 4;
            this.tbQuantity.Name = "tbQuantity";
            this.tbQuantity.Size = new System.Drawing.Size(84, 31);
            this.tbQuantity.TabIndex = 105;
            this.tbQuantity.TextChanged += new System.EventHandler(this.tbQuantity_TextChanged);
            this.tbQuantity.GotFocus += new System.EventHandler(this.tbQuantity_GotFocus);
            this.tbQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbQuantity_KeyPress);
            // 
            // tbLocation
            // 
            this.tbLocation.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.tbLocation.Location = new System.Drawing.Point(44, 66);
            this.tbLocation.Name = "tbLocation";
            this.tbLocation.Size = new System.Drawing.Size(127, 39);
            this.tbLocation.TabIndex = 113;
            this.tbLocation.TextChanged += new System.EventHandler(this.tbLocation_TextChanged);
            this.tbLocation.GotFocus += new System.EventHandler(this.tbLocation_GotFocus);
            this.tbLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLocation_KeyPress);
            this.tbLocation.LostFocus += new System.EventHandler(this.tbLocation_LostFocus);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.Location = new System.Drawing.Point(174, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 20);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(104, 20);
            this.constMenu.Text = "Scan Product";
            // 
            // constLoc
            // 
            this.constLoc.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.constLoc.Location = new System.Drawing.Point(4, 75);
            this.constLoc.Name = "constLoc";
            this.constLoc.Size = new System.Drawing.Size(40, 20);
            this.constLoc.Text = "Loc.";
            // 
            // btnMainMenu
            // 
            this.btnMainMenu.Location = new System.Drawing.Point(122, 172);
            this.btnMainMenu.Name = "btnMainMenu";
            this.btnMainMenu.Size = new System.Drawing.Size(112, 30);
            this.btnMainMenu.TabIndex = 9;
            this.btnMainMenu.TabStop = false;
            this.btnMainMenu.Text = "Main Menu";
            this.btnMainMenu.Click += new System.EventHandler(this.btnMainMenu_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(2, 172);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 30);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(122, 136);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(112, 30);
            this.btnNext.TabIndex = 7;
            this.btnNext.TabStop = false;
            this.btnNext.Text = "Next >>";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(2, 136);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(112, 30);
            this.btnBack.TabIndex = 6;
            this.btnBack.TabStop = false;
            this.btnBack.Text = "<< Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(176, 75);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(60, 20);
            this.btnClear.TabIndex = 114;
            this.btnClear.TabStop = false;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // constQuan
            // 
            this.constQuan.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.constQuan.Location = new System.Drawing.Point(3, 105);
            this.constQuan.Name = "constQuan";
            this.constQuan.Size = new System.Drawing.Size(100, 20);
            this.constQuan.Text = "Quantity";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.panel1.Controls.Add(this.tbCounter);
            this.panel1.Controls.Add(this.constCounter);
            this.panel1.Controls.Add(this.tbSerial);
            this.panel1.Controls.Add(this.tbBarcode);
            this.panel1.Controls.Add(this.snCheck);
            this.panel1.Controls.Add(this.constSN);
            this.panel1.Controls.Add(this.ddlUnit);
            this.panel1.Controls.Add(this.tbQuantity);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnMainMenu);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Controls.Add(this.constQuan);
            this.panel1.Controls.Add(this.tbBarcode_);
            this.panel1.Controls.Add(this.constItem);
            this.panel1.Location = new System.Drawing.Point(2, 111);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(236, 207);
            // 
            // tbCounter
            // 
            this.tbCounter.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.tbCounter.Location = new System.Drawing.Point(153, 100);
            this.tbCounter.MaxLength = 14;
            this.tbCounter.Name = "tbCounter";
            this.tbCounter.Size = new System.Drawing.Size(80, 31);
            this.tbCounter.TabIndex = 147;
            this.tbCounter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCounter_KeyPress);
            // 
            // constCounter
            // 
            this.constCounter.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.constCounter.Location = new System.Drawing.Point(3, 107);
            this.constCounter.Name = "constCounter";
            this.constCounter.Size = new System.Drawing.Size(144, 20);
            this.constCounter.Text = "Conversion Counter";
            // 
            // tbSerial
            // 
            this.tbSerial.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.tbSerial.Location = new System.Drawing.Point(42, 64);
            this.tbSerial.Name = "tbSerial";
            this.tbSerial.Size = new System.Drawing.Size(162, 31);
            this.tbSerial.TabIndex = 142;
            this.tbSerial.GotFocus += new System.EventHandler(this.tbSerial_GotFocus);
            this.tbSerial.LostFocus += new System.EventHandler(this.tbSerial_LostFocus);
            // 
            // tbBarcode
            // 
            this.tbBarcode.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.tbBarcode.Location = new System.Drawing.Point(3, 28);
            this.tbBarcode.MaxLength = 14;
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.Size = new System.Drawing.Size(230, 31);
            this.tbBarcode.TabIndex = 137;
            this.tbBarcode.TextChanged += new System.EventHandler(this.tbBarcode_TextChanged);
            this.tbBarcode.GotFocus += new System.EventHandler(this.tbBarcode_GotFocus);
            this.tbBarcode.LostFocus += new System.EventHandler(this.tbBarcode_LostFocus);
            // 
            // snCheck
            // 
            this.snCheck.Location = new System.Drawing.Point(210, 68);
            this.snCheck.Name = "snCheck";
            this.snCheck.Size = new System.Drawing.Size(23, 20);
            this.snCheck.TabIndex = 127;
            this.snCheck.CheckStateChanged += new System.EventHandler(this.snCheck_CheckStateChanged);
            // 
            // constSN
            // 
            this.constSN.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.constSN.Location = new System.Drawing.Point(2, 69);
            this.constSN.Name = "constSN";
            this.constSN.Size = new System.Drawing.Size(42, 20);
            this.constSN.Text = "S/N";
            // 
            // tbBarcode_
            // 
            this.tbBarcode_.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.tbBarcode_.Location = new System.Drawing.Point(210, 28);
            this.tbBarcode_.MaxLength = 14;
            this.tbBarcode_.Name = "tbBarcode_";
            this.tbBarcode_.Size = new System.Drawing.Size(24, 31);
            this.tbBarcode_.TabIndex = 4;
            this.tbBarcode_.GotFocus += new System.EventHandler(this.tbLocation_GotFocus);
            this.tbBarcode_.LostFocus += new System.EventHandler(this.tbLocation_LostFocus);
            // 
            // constItem
            // 
            this.constItem.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.constItem.Location = new System.Drawing.Point(2, 2);
            this.constItem.Name = "constItem";
            this.constItem.Size = new System.Drawing.Size(65, 20);
            this.constItem.Text = "Items";
            // 
            // lbSumQtyPCS
            // 
            this.lbSumQtyPCS.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lbSumQtyPCS.Location = new System.Drawing.Point(101, 0);
            this.lbSumQtyPCS.Name = "lbSumQtyPCS";
            this.lbSumQtyPCS.Size = new System.Drawing.Size(104, 25);
            this.lbSumQtyPCS.Text = "PCS : ";
            // 
            // lbSumQtyPCK
            // 
            this.lbSumQtyPCK.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lbSumQtyPCK.Location = new System.Drawing.Point(101, 30);
            this.lbSumQtyPCK.Name = "lbSumQtyPCK";
            this.lbSumQtyPCK.Size = new System.Drawing.Size(104, 25);
            this.lbSumQtyPCK.Text = "PCK : ";
            // 
            // lbFunction
            // 
            this.lbFunction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(192)))));
            this.lbFunction.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbFunction.ForeColor = System.Drawing.Color.White;
            this.lbFunction.Location = new System.Drawing.Point(4, 38);
            this.lbFunction.Name = "lbFunction";
            this.lbFunction.Size = new System.Drawing.Size(20, 20);
            this.lbFunction.Text = "Fn";
            this.lbFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbAlpha
            // 
            this.lbAlpha.BackColor = System.Drawing.Color.DarkOrange;
            this.lbAlpha.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbAlpha.ForeColor = System.Drawing.Color.White;
            this.lbAlpha.Location = new System.Drawing.Point(30, 38);
            this.lbAlpha.Name = "lbAlpha";
            this.lbAlpha.Size = new System.Drawing.Size(20, 20);
            this.lbAlpha.Text = "Al";
            this.lbAlpha.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // constSubMenu
            // 
            this.constSubMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constSubMenu.Location = new System.Drawing.Point(0, 17);
            this.constSubMenu.Name = "constSubMenu";
            this.constSubMenu.Size = new System.Drawing.Size(104, 20);
            this.constSubMenu.Text = "Scan Only";
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 121;
            this.hhtToolBar1.TabStop = false;
            // 
            // ScanProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.constSubMenu);
            this.Controls.Add(this.lbAlpha);
            this.Controls.Add(this.lbFunction);
            this.Controls.Add(this.lbSumQtyPCK);
            this.Controls.Add(this.lbSumQtyPCS);
            this.Controls.Add(this.tbLocation);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.constLoc);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.hhtToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScanProduct";
            this.Text = "Scan Product";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScanProduct_KeyDown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlUnit;
        private System.Windows.Forms.TextBox tbQuantity;
        private System.Windows.Forms.TextBox tbLocation;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label constMenu;
        private System.Windows.Forms.Label constLoc;
        private System.Windows.Forms.Button btnMainMenu;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label constQuan;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbBarcode_;
        private System.Windows.Forms.Label constItem;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label lbSumQtyPCS;
        private System.Windows.Forms.Label lbSumQtyPCK;
        private System.Windows.Forms.Label lbFunction;
        private System.Windows.Forms.Label lbAlpha;
        private System.Windows.Forms.Label constSubMenu;
        private System.Windows.Forms.Label constSN;
        private System.Windows.Forms.CheckBox snCheck;
        private System.Windows.Forms.TextBox tbBarcode;
        private System.Windows.Forms.TextBox tbSerial;
        private System.Windows.Forms.TextBox tbCounter;
        private System.Windows.Forms.Label constCounter;
    }
}