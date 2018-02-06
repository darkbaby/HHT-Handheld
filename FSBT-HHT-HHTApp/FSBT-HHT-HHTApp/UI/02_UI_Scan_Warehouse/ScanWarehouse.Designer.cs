namespace Denso_HHT
{
    partial class ScanWarehouse
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
            this.const8 = new System.Windows.Forms.TextBox();
            this.tbQuantity = new System.Windows.Forms.TextBox();
            this.tbLocation = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.constMenu = new System.Windows.Forms.Label();
            this.const2 = new System.Windows.Forms.Label();
            this.btnMainMenu = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.const1 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.const7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.const4 = new System.Windows.Forms.Label();
            this.lbSumQtyPCS = new System.Windows.Forms.Label();
            this.lbSumQtyPCK = new System.Windows.Forms.Label();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.lbFunction = new System.Windows.Forms.Label();
            this.lbAlpha = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ddlUnit
            // 
            this.ddlUnit.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.ddlUnit.Location = new System.Drawing.Point(169, 66);
            this.ddlUnit.Name = "ddlUnit";
            this.ddlUnit.Size = new System.Drawing.Size(65, 31);
            this.ddlUnit.TabIndex = 111;
            this.ddlUnit.TabStop = false;
            this.ddlUnit.SelectedIndexChanged += new System.EventHandler(this.ddlUnit_SelectedIndexChanged);
            // 
            // const8
            // 
            this.const8.BackColor = System.Drawing.Color.Gainsboro;
            this.const8.Enabled = false;
            this.const8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.const8.Location = new System.Drawing.Point(73, 65);
            this.const8.Name = "const8";
            this.const8.ReadOnly = true;
            this.const8.Size = new System.Drawing.Size(163, 21);
            this.const8.TabIndex = 116;
            this.const8.TabStop = false;
            this.const8.Text = "Warehouse : ";
            // 
            // tbQuantity
            // 
            this.tbQuantity.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.tbQuantity.Location = new System.Drawing.Point(79, 66);
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
            this.tbLocation.Location = new System.Drawing.Point(44, 90);
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
            this.btnDelete.Location = new System.Drawing.Point(174, 2);
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
            this.constMenu.Size = new System.Drawing.Size(80, 20);
            this.constMenu.Text = "Scan WH";
            // 
            // const2
            // 
            this.const2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.const2.Location = new System.Drawing.Point(4, 99);
            this.const2.Name = "const2";
            this.const2.Size = new System.Drawing.Size(40, 20);
            this.const2.Text = "Loc.";
            // 
            // btnMainMenu
            // 
            this.btnMainMenu.Location = new System.Drawing.Point(122, 145);
            this.btnMainMenu.Name = "btnMainMenu";
            this.btnMainMenu.Size = new System.Drawing.Size(112, 30);
            this.btnMainMenu.TabIndex = 9;
            this.btnMainMenu.TabStop = false;
            this.btnMainMenu.Text = "Main Menu";
            this.btnMainMenu.Click += new System.EventHandler(this.btnMainMenu_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(2, 145);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 30);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // const1
            // 
            this.const1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.const1.Location = new System.Drawing.Point(4, 66);
            this.const1.Name = "const1";
            this.const1.Size = new System.Drawing.Size(75, 20);
            this.const1.Text = "Scan Mode";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(122, 109);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(112, 30);
            this.btnNext.TabIndex = 7;
            this.btnNext.TabStop = false;
            this.btnNext.Text = "Next >>";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(2, 109);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(112, 30);
            this.btnBack.TabIndex = 6;
            this.btnBack.TabStop = false;
            this.btnBack.Text = "<< Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(176, 99);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(60, 20);
            this.btnClear.TabIndex = 114;
            this.btnClear.TabStop = false;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // const7
            // 
            this.const7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.const7.Location = new System.Drawing.Point(2, 72);
            this.const7.Name = "const7";
            this.const7.Size = new System.Drawing.Size(100, 20);
            this.const7.Text = "Quantity";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.panel1.Controls.Add(this.ddlUnit);
            this.panel1.Controls.Add(this.tbQuantity);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnMainMenu);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Controls.Add(this.const7);
            this.panel1.Controls.Add(this.tbBarcode);
            this.panel1.Controls.Add(this.const4);
            this.panel1.Location = new System.Drawing.Point(2, 138);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(236, 180);
            // 
            // tbBarcode
            // 
            this.tbBarcode.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.tbBarcode.Location = new System.Drawing.Point(2, 27);
            this.tbBarcode.MaxLength = 14;
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.Size = new System.Drawing.Size(232, 31);
            this.tbBarcode.TabIndex = 4;
            this.tbBarcode.TextChanged += new System.EventHandler(this.tbBarcode_TextChanged);
            this.tbBarcode.GotFocus += new System.EventHandler(this.tbBarcode_GotFocus);
            this.tbBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbBarcode_KeyPress);
            this.tbBarcode.LostFocus += new System.EventHandler(this.tbBarcode_LostFocus);
            // 
            // const4
            // 
            this.const4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.const4.Location = new System.Drawing.Point(2, 2);
            this.const4.Name = "const4";
            this.const4.Size = new System.Drawing.Size(100, 20);
            this.const4.Text = "Items";
            // 
            // lbSumQtyPCS
            // 
            this.lbSumQtyPCS.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.lbSumQtyPCS.Location = new System.Drawing.Point(80, 0);
            this.lbSumQtyPCS.Name = "lbSumQtyPCS";
            this.lbSumQtyPCS.Size = new System.Drawing.Size(125, 25);
            this.lbSumQtyPCS.Text = "PCS : ";
            // 
            // lbSumQtyPCK
            // 
            this.lbSumQtyPCK.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.lbSumQtyPCK.Location = new System.Drawing.Point(80, 30);
            this.lbSumQtyPCK.Name = "lbSumQtyPCK";
            this.lbSumQtyPCK.Size = new System.Drawing.Size(125, 25);
            this.lbSumQtyPCK.Text = "PCK : ";
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 121;
            this.hhtToolBar1.TabStop = false;
            // 
            // lbFunction
            // 
            this.lbFunction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(192)))));
            this.lbFunction.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbFunction.ForeColor = System.Drawing.Color.White;
            this.lbFunction.Location = new System.Drawing.Point(4, 20);
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
            this.lbAlpha.Location = new System.Drawing.Point(30, 20);
            this.lbAlpha.Name = "lbAlpha";
            this.lbAlpha.Size = new System.Drawing.Size(20, 20);
            this.lbAlpha.Text = "Al";
            this.lbAlpha.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ScanWarehouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbAlpha);
            this.Controls.Add(this.lbFunction);
            this.Controls.Add(this.lbSumQtyPCK);
            this.Controls.Add(this.lbSumQtyPCS);
            this.Controls.Add(this.const8);
            this.Controls.Add(this.tbLocation);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.const2);
            this.Controls.Add(this.const1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.hhtToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScanWarehouse";
            this.Text = "Scan Warehouse";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScanWarehouse_KeyDown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlUnit;
        private System.Windows.Forms.TextBox const8;
        private System.Windows.Forms.TextBox tbQuantity;
        private System.Windows.Forms.TextBox tbLocation;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label constMenu;
        private System.Windows.Forms.Label const2;
        private System.Windows.Forms.Button btnMainMenu;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label const1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label const7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbBarcode;
        private System.Windows.Forms.Label const4;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label lbSumQtyPCS;
        private System.Windows.Forms.Label lbSumQtyPCK;
        private System.Windows.Forms.Label lbFunction;
        private System.Windows.Forms.Label lbAlpha;







    }
}