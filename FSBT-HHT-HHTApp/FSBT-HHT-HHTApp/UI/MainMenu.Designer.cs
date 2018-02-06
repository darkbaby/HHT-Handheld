using Denso_HHT.Module;
namespace Denso_HHT
{
    partial class MainMenu
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
            DatabaseModule.Instance.End();
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
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnDeleteData = new System.Windows.Forms.Button();
            this.btnSendData = new System.Windows.Forms.Button();
            this.btnFreshFood = new System.Windows.Forms.Button();
            this.btnWarehouse = new System.Windows.Forms.Button();
            this.btnFront = new System.Windows.Forms.Button();
            this.labelVersion = new System.Windows.Forms.Label();
            this.constMenu = new System.Windows.Forms.Label();
            this.labelMode = new System.Windows.Forms.Label();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnExit.Location = new System.Drawing.Point(30, 260);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(180, 30);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnSetting.Location = new System.Drawing.Point(30, 225);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(180, 30);
            this.btnSetting.TabIndex = 12;
            this.btnSetting.Text = "Setting";
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnDeleteData
            // 
            this.btnDeleteData.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnDeleteData.Location = new System.Drawing.Point(30, 190);
            this.btnDeleteData.Name = "btnDeleteData";
            this.btnDeleteData.Size = new System.Drawing.Size(180, 30);
            this.btnDeleteData.TabIndex = 11;
            this.btnDeleteData.Text = "Delete Data";
            this.btnDeleteData.Click += new System.EventHandler(this.btnDeleteData_Click);
            // 
            // btnSendData
            // 
            this.btnSendData.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnSendData.Location = new System.Drawing.Point(30, 135);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(180, 50);
            this.btnSendData.TabIndex = 10;
            this.btnSendData.Text = "TEMP";
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnFreshFood
            // 
            this.btnFreshFood.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnFreshFood.Location = new System.Drawing.Point(30, 100);
            this.btnFreshFood.Name = "btnFreshFood";
            this.btnFreshFood.Size = new System.Drawing.Size(180, 30);
            this.btnFreshFood.TabIndex = 9;
            this.btnFreshFood.Text = "Scan Fresh Food";
            this.btnFreshFood.Click += new System.EventHandler(this.btnFreshFood_Click);
            // 
            // btnWarehouse
            // 
            this.btnWarehouse.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnWarehouse.Location = new System.Drawing.Point(30, 65);
            this.btnWarehouse.Name = "btnWarehouse";
            this.btnWarehouse.Size = new System.Drawing.Size(180, 30);
            this.btnWarehouse.TabIndex = 8;
            this.btnWarehouse.Text = "Scan Warehouse";
            this.btnWarehouse.Click += new System.EventHandler(this.btnWarehouse_Click);
            // 
            // btnFront
            // 
            this.btnFront.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnFront.Location = new System.Drawing.Point(30, 30);
            this.btnFront.Name = "btnFront";
            this.btnFront.Size = new System.Drawing.Size(180, 30);
            this.btnFront.TabIndex = 7;
            this.btnFront.Text = "Scan Front";
            this.btnFront.Click += new System.EventHandler(this.btnFront_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.Location = new System.Drawing.Point(114, 300);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(126, 20);
            this.labelVersion.Text = "Stocktaking v.1.0.0";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(125, 20);
            this.constMenu.Text = "Main Menu";
            // 
            // labelMode
            // 
            this.labelMode.Location = new System.Drawing.Point(0, 300);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(108, 20);
            this.labelMode.Text = "Mode";
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 100;
            this.hhtToolBar1.TabStop = false;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.hhtToolBar1);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnDeleteData);
            this.Controls.Add(this.btnSendData);
            this.Controls.Add(this.btnFreshFood);
            this.Controls.Add(this.btnWarehouse);
            this.Controls.Add(this.btnFront);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainMenu";
            this.Text = "Main Menu";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnDeleteData;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.Button btnFreshFood;
        private System.Windows.Forms.Button btnWarehouse;
        private System.Windows.Forms.Button btnFront;
        private System.Windows.Forms.Label labelVersion;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label constMenu;
        private System.Windows.Forms.Label labelMode;
    }
}

