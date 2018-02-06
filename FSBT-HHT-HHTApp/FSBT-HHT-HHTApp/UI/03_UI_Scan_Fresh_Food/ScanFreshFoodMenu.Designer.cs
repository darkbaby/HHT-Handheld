namespace Denso_HHT
{
    partial class ScanFreshFoodMenu
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
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.constMenu = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnScanWeight = new System.Windows.Forms.Button();
            this.btnScanQty = new System.Windows.Forms.Button();
            this.btnScanOnly = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 1;
            this.hhtToolBar1.TabStop = false;
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(123, 32);
            this.constMenu.Text = "Scan Fresh Food Menu";
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnExit.Location = new System.Drawing.Point(0, 280);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(240, 40);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnScanWeight
            // 
            this.btnScanWeight.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnScanWeight.Location = new System.Drawing.Point(40, 210);
            this.btnScanWeight.Name = "btnScanWeight";
            this.btnScanWeight.Size = new System.Drawing.Size(160, 40);
            this.btnScanWeight.TabIndex = 12;
            this.btnScanWeight.Text = "Scan Weight";
            this.btnScanWeight.Click += new System.EventHandler(this.btnScanWeight_Click);
            // 
            // btnScanQty
            // 
            this.btnScanQty.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnScanQty.Location = new System.Drawing.Point(40, 130);
            this.btnScanQty.Name = "btnScanQty";
            this.btnScanQty.Size = new System.Drawing.Size(160, 40);
            this.btnScanQty.TabIndex = 11;
            this.btnScanQty.Text = "Scan Qty";
            this.btnScanQty.Click += new System.EventHandler(this.btnScanQty_Click);
            // 
            // btnScanOnly
            // 
            this.btnScanOnly.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnScanOnly.Location = new System.Drawing.Point(40, 50);
            this.btnScanOnly.Name = "btnScanOnly";
            this.btnScanOnly.Size = new System.Drawing.Size(160, 40);
            this.btnScanOnly.TabIndex = 10;
            this.btnScanOnly.Text = "Scan Only";
            this.btnScanOnly.Click += new System.EventHandler(this.btnScanOnly_Click);
            // 
            // ScanFreshFoodMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnScanWeight);
            this.Controls.Add(this.btnScanQty);
            this.Controls.Add(this.btnScanOnly);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScanFreshFoodMenu";
            this.Text = "Scan FreshFood Menu";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScanFreshFoodMenu_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label constMenu;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnScanWeight;
        private System.Windows.Forms.Button btnScanQty;
        private System.Windows.Forms.Button btnScanOnly;
    }
}