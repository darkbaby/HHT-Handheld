namespace Denso_HHT
{
    partial class SendAndReceiveDataMenu
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
            this.btnExit = new System.Windows.Forms.Button();
            this.constMenu = new System.Windows.Forms.Label();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnOnlyNew = new System.Windows.Forms.Button();
            this.btnReceiveData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnExit.Location = new System.Drawing.Point(0, 280);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(240, 40);
            this.btnExit.TabIndex = 24;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(134, 35);
            this.constMenu.Text = "Syncing Data Menu";
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 100;
            this.hhtToolBar1.TabStop = false;
            // 
            // btnAll
            // 
            this.btnAll.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnAll.Location = new System.Drawing.Point(40, 130);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(160, 40);
            this.btnAll.TabIndex = 22;
            this.btnAll.Text = "All";
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnOnlyNew
            // 
            this.btnOnlyNew.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnOnlyNew.Location = new System.Drawing.Point(40, 210);
            this.btnOnlyNew.Name = "btnOnlyNew";
            this.btnOnlyNew.Size = new System.Drawing.Size(160, 40);
            this.btnOnlyNew.TabIndex = 23;
            this.btnOnlyNew.Text = "Only New";
            this.btnOnlyNew.Click += new System.EventHandler(this.btnOnlyNew_Click);
            // 
            // btnReceiveData
            // 
            this.btnReceiveData.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.btnReceiveData.Location = new System.Drawing.Point(40, 50);
            this.btnReceiveData.Name = "btnReceiveData";
            this.btnReceiveData.Size = new System.Drawing.Size(160, 40);
            this.btnReceiveData.TabIndex = 21;
            this.btnReceiveData.Text = "Receive Data";
            this.btnReceiveData.Click += new System.EventHandler(this.btnReceiveData_Click);
            // 
            // SendAndReceiveDataMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnOnlyNew);
            this.Controls.Add(this.btnReceiveData);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendAndReceiveDataMenu";
            this.Text = "Send And Receive Data Menu";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendAndReceiveDataMenu_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label constMenu;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnOnlyNew;
        private System.Windows.Forms.Button btnReceiveData;
    }
}