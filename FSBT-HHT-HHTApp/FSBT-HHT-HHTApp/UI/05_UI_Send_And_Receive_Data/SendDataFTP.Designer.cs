using Denso_HHT.Module;
namespace Denso_HHT
{
    partial class SendDataFTP
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
            //DatabaseModule.Instance.Init();

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
            this.btnSendData = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.constMenu = new System.Windows.Forms.Label();
            this.const1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.lbMode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(25, 275);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(80, 30);
            this.btnSendData.TabIndex = 0;
            this.btnSendData.Text = "Send Data";
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(135, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(134, 20);
            this.constMenu.Text = "Send Data (WIFI)";
            // 
            // const1
            // 
            this.const1.Location = new System.Drawing.Point(0, 35);
            this.const1.Name = "const1";
            this.const1.Size = new System.Drawing.Size(240, 20);
            this.const1.Text = "Remaining Audit Data";
            this.const1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(25, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 150);
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 100;
            this.hhtToolBar1.TabStop = false;
            // 
            // lbMode
            // 
            this.lbMode.Location = new System.Drawing.Point(0, 235);
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(240, 20);
            this.lbMode.Text = "Mode : ";
            this.lbMode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // SendDataFTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbMode);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.const1);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSendData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendDataFTP";
            this.Text = "Send Data FTP";
            this.Load += new System.EventHandler(this.SendDataFTP_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label constMenu;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label const1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbMode;
    }
}