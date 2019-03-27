namespace Denso_HHT
{
    partial class Login
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.const1 = new System.Windows.Forms.Label();
            this.constMenu = new System.Windows.Forms.Label();
            this.const2 = new System.Windows.Forms.Label();
            this.tbDepartmentCode = new System.Windows.Forms.TextBox();
            this.radioBtnNonRealtime = new System.Windows.Forms.RadioButton();
            this.radioBtnRealtime = new System.Windows.Forms.RadioButton();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.lbAlpha = new System.Windows.Forms.Label();
            this.lbFunction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(77, 178);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 40);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(150, 280);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(90, 40);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(40, 128);
            this.tbUsername.MaxLength = 20;
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(160, 23);
            this.tbUsername.TabIndex = 0;
            // 
            // const1
            // 
            this.const1.Location = new System.Drawing.Point(0, 108);
            this.const1.Name = "const1";
            this.const1.Size = new System.Drawing.Size(240, 20);
            this.const1.Text = "Username";
            this.const1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(125, 20);
            this.constMenu.Text = "Login";
            // 
            // const2
            // 
            this.const2.Location = new System.Drawing.Point(0, 46);
            this.const2.Name = "const2";
            this.const2.Size = new System.Drawing.Size(240, 20);
            this.const2.Text = "Department Code";
            this.const2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.const2.Visible = false;
            // 
            // tbDepartmentCode
            // 
            this.tbDepartmentCode.Location = new System.Drawing.Point(77, 20);
            this.tbDepartmentCode.MaxLength = 3;
            this.tbDepartmentCode.Name = "tbDepartmentCode";
            this.tbDepartmentCode.Size = new System.Drawing.Size(160, 23);
            this.tbDepartmentCode.TabIndex = 1;
            this.tbDepartmentCode.Visible = false;
            this.tbDepartmentCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbDepartmentCode_KeyPress);
            // 
            // radioBtnNonRealtime
            // 
            this.radioBtnNonRealtime.Checked = true;
            this.radioBtnNonRealtime.Location = new System.Drawing.Point(90, 66);
            this.radioBtnNonRealtime.Name = "radioBtnNonRealtime";
            this.radioBtnNonRealtime.Size = new System.Drawing.Size(110, 20);
            this.radioBtnNonRealtime.TabIndex = 2;
            this.radioBtnNonRealtime.Text = "Non-Realtime";
            this.radioBtnNonRealtime.Visible = false;
            // 
            // radioBtnRealtime
            // 
            this.radioBtnRealtime.Location = new System.Drawing.Point(90, 40);
            this.radioBtnRealtime.Name = "radioBtnRealtime";
            this.radioBtnRealtime.Size = new System.Drawing.Size(110, 20);
            this.radioBtnRealtime.TabIndex = 3;
            this.radioBtnRealtime.Text = "Realtime";
            this.radioBtnRealtime.Visible = false;
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 40;
            this.hhtToolBar1.TabStop = false;
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
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbAlpha);
            this.Controls.Add(this.lbFunction);
            this.Controls.Add(this.radioBtnRealtime);
            this.Controls.Add(this.radioBtnNonRealtime);
            this.Controls.Add(this.const2);
            this.Controls.Add(this.tbDepartmentCode);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.Controls.Add(this.const1);
            this.Controls.Add(this.tbUsername);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Login_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Label const1;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label constMenu;
        private System.Windows.Forms.Label const2;
        private System.Windows.Forms.TextBox tbDepartmentCode;
        private System.Windows.Forms.RadioButton radioBtnNonRealtime;
        private System.Windows.Forms.RadioButton radioBtnRealtime;
        private System.Windows.Forms.Label lbAlpha;
        private System.Windows.Forms.Label lbFunction;
    }
}