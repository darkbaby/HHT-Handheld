﻿namespace Denso_HHT
{
    partial class SendDataPassword
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
            this.const3 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbAlpha = new System.Windows.Forms.Label();
            this.lbFunction = new System.Windows.Forms.Label();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.SuspendLayout();
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(134, 20);
            this.constMenu.Text = "Confirm Password";
            // 
            // const3
            // 
            this.const3.Location = new System.Drawing.Point(0, 96);
            this.const3.Name = "const3";
            this.const3.Size = new System.Drawing.Size(240, 20);
            this.const3.Text = "Password";
            this.const3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(40, 119);
            this.tbPassword.MaxLength = 24;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(160, 23);
            this.tbPassword.TabIndex = 10;
            this.tbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPassword_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(130, 183);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 30);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(40, 183);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 30);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
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
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 30;
            this.hhtToolBar1.TabStop = false;
            // 
            // SendDataPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbAlpha);
            this.Controls.Add(this.lbFunction);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.const3);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendDataPassword";
            this.Text = "Confirm Password";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendDataPassword_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label constMenu;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label const3;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lbAlpha;
        private System.Windows.Forms.Label lbFunction;
    }
}