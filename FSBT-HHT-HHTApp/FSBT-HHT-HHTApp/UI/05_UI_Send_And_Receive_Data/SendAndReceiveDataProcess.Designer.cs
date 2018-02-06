using System.IO;
using Denso_HHT.Module;
namespace Denso_HHT
{
    partial class SendAndReceiveDataProcess
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
            string[] files = Directory.GetFiles(path + @"\temp");
            if (files.Length != 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    string[] filePathSplit = files[i].Split('\\');
                    string fileName = filePathSplit[filePathSplit.Length - 1];
                    if (fileName.Contains("success"))
                    {
                        DatabaseModule.Instance.QueryUpdateShowStatusFromSendData(true, inputSplit);
                    }
                    File.Delete(files[i]);
                }
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
            this.constMenu = new System.Windows.Forms.Label();
            this.lbMode = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.const1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSendData = new System.Windows.Forms.Button();
            this.hhtToolBar1 = new Denso_HHT.HHTToolBar();
            this.SuspendLayout();
            // 
            // constMenu
            // 
            this.constMenu.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.constMenu.Location = new System.Drawing.Point(0, 0);
            this.constMenu.Name = "constMenu";
            this.constMenu.Size = new System.Drawing.Size(134, 38);
            this.constMenu.Text = "Syncing Data";
            // 
            // lbMode
            // 
            this.lbMode.Location = new System.Drawing.Point(0, 235);
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(240, 20);
            this.lbMode.Text = "Mode : ";
            this.lbMode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(25, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 150);
            // 
            // const1
            // 
            this.const1.Location = new System.Drawing.Point(0, 35);
            this.const1.Name = "const1";
            this.const1.Size = new System.Drawing.Size(240, 20);
            this.const1.Text = "Remaining Audit Data";
            this.const1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(135, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(25, 275);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(80, 30);
            this.btnSendData.TabIndex = 15;
            this.btnSendData.Text = "OK";
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // hhtToolBar1
            // 
            this.hhtToolBar1.Location = new System.Drawing.Point(140, 0);
            this.hhtToolBar1.Name = "hhtToolBar1";
            this.hhtToolBar1.Size = new System.Drawing.Size(100, 20);
            this.hhtToolBar1.TabIndex = 100;
            this.hhtToolBar1.TabStop = false;
            // 
            // SendAndReceiveDataProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbMode);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.const1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSendData);
            this.Controls.Add(this.constMenu);
            this.Controls.Add(this.hhtToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendAndReceiveDataProcess";
            this.Text = "Send And Receive Data Process";
            this.Load += new System.EventHandler(this.SendAndReceiveDataProcess_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label constMenu;
        private HHTToolBar hhtToolBar1;
        private System.Windows.Forms.Label lbMode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label const1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSendData;
    }
}