namespace Denso_HHT
{
    partial class SettingDepartment
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
            this.constMenu = new System.Windows.Forms.Label();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.const2 = new System.Windows.Forms.Label();
            this.tbDepartmentCode = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbAlpha = new System.Windows.Forms.Label();
            this.lbFunction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(118, 20);
            this.constMenu.Text = "Set Department";
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 50;
            this.hhtToolBar1.TabStop = false;
            // 
            // const2
            // 
            this.const2.Location = new System.Drawing.Point(0, 70);
            this.const2.Name = "const2";
            this.const2.Size = new System.Drawing.Size(240, 20);
            this.const2.Text = "Department Code";
            this.const2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbDepartmentCode
            // 
            this.tbDepartmentCode.Location = new System.Drawing.Point(40, 90);
            this.tbDepartmentCode.MaxLength = 3;
            this.tbDepartmentCode.Name = "tbDepartmentCode";
            this.tbDepartmentCode.Size = new System.Drawing.Size(160, 23);
            this.tbDepartmentCode.TabIndex = 1;
            this.tbDepartmentCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbDepartmentCode_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(130, 140);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(40, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            // SettingDepartment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbAlpha);
            this.Controls.Add(this.lbFunction);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.const2);
            this.Controls.Add(this.tbDepartmentCode);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingDepartment";
            this.Text = "Setting Department";
            this.Load += new System.EventHandler(this.SettingDepartment_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingDepartment_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label constMenu;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label const2;
        private System.Windows.Forms.TextBox tbDepartmentCode;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lbAlpha;
        private System.Windows.Forms.Label lbFunction;
    }
}