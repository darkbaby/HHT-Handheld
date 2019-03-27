namespace Denso_HHT
{
    partial class SettingMenu
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
            this.btnDateTime = new System.Windows.Forms.Button();
            this.btnDeviceName = new System.Windows.Forms.Button();
            this.btnComputer = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.constMenu = new System.Windows.Forms.Label();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.btnDepartment = new System.Windows.Forms.Button();
            this.btnFTP = new System.Windows.Forms.Button();
            this.btnPassword = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDateTime
            // 
            this.btnDateTime.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnDateTime.Location = new System.Drawing.Point(40, 100);
            this.btnDateTime.Name = "btnDateTime";
            this.btnDateTime.Size = new System.Drawing.Size(160, 30);
            this.btnDateTime.TabIndex = 8;
            this.btnDateTime.Text = "Date Time";
            this.btnDateTime.Click += new System.EventHandler(this.btnDateTime_Click);
            // 
            // btnDeviceName
            // 
            this.btnDeviceName.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnDeviceName.Location = new System.Drawing.Point(40, 60);
            this.btnDeviceName.Name = "btnDeviceName";
            this.btnDeviceName.Size = new System.Drawing.Size(160, 30);
            this.btnDeviceName.TabIndex = 7;
            this.btnDeviceName.Text = "Device";
            this.btnDeviceName.Click += new System.EventHandler(this.btnDeviceName_Click);
            // 
            // btnComputer
            // 
            this.btnComputer.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnComputer.Location = new System.Drawing.Point(40, 140);
            this.btnComputer.Name = "btnComputer";
            this.btnComputer.Size = new System.Drawing.Size(160, 30);
            this.btnComputer.TabIndex = 10;
            this.btnComputer.Text = "Computer";
            this.btnComputer.Click += new System.EventHandler(this.btnComputer_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnExit.Location = new System.Drawing.Point(0, 280);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(240, 40);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(100, 20);
            this.constMenu.Text = "Setting Menu";
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 0;
            this.hhtToolBar1.TabStop = false;
            // 
            // btnDepartment
            // 
            this.btnDepartment.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnDepartment.Location = new System.Drawing.Point(77, 26);
            this.btnDepartment.Name = "btnDepartment";
            this.btnDepartment.Size = new System.Drawing.Size(160, 30);
            this.btnDepartment.TabIndex = 9;
            this.btnDepartment.Text = "Department";
            this.btnDepartment.Visible = false;
            this.btnDepartment.Click += new System.EventHandler(this.btnDepartment_Click);
            // 
            // btnFTP
            // 
            this.btnFTP.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnFTP.Location = new System.Drawing.Point(40, 180);
            this.btnFTP.Name = "btnFTP";
            this.btnFTP.Size = new System.Drawing.Size(160, 30);
            this.btnFTP.TabIndex = 11;
            this.btnFTP.Text = "FTP";
            this.btnFTP.Click += new System.EventHandler(this.btnFTP_Click);
            // 
            // btnPassword
            // 
            this.btnPassword.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnPassword.Location = new System.Drawing.Point(40, 220);
            this.btnPassword.Name = "btnPassword";
            this.btnPassword.Size = new System.Drawing.Size(160, 30);
            this.btnPassword.TabIndex = 13;
            this.btnPassword.Text = "PASSWORD";
            this.btnPassword.Click += new System.EventHandler(this.btnPassword_Click);
            // 
            // SettingMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.btnPassword);
            this.Controls.Add(this.btnFTP);
            this.Controls.Add(this.btnDepartment);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnComputer);
            this.Controls.Add(this.btnDateTime);
            this.Controls.Add(this.btnDeviceName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingMenu";
            this.Text = "Setting Menu";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnDateTime;
        private System.Windows.Forms.Button btnDeviceName;
        private System.Windows.Forms.Button btnComputer;
        private System.Windows.Forms.Button btnExit;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label constMenu;
        private System.Windows.Forms.Button btnDepartment;
        private System.Windows.Forms.Button btnFTP;
        private System.Windows.Forms.Button btnPassword;
    }
}