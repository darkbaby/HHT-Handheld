namespace Denso_HHT
{
    partial class HHTToolBar
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.batteryBar = new System.Windows.Forms.ProgressBar();
            this.clockLbl = new System.Windows.Forms.Label();
            this.timerBattery = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // batteryBar
            // 
            this.batteryBar.Location = new System.Drawing.Point(72, 2);
            this.batteryBar.Name = "batteryBar";
            this.batteryBar.Size = new System.Drawing.Size(25, 16);
            // 
            // clockLbl
            // 
            this.clockLbl.Location = new System.Drawing.Point(3, 2);
            this.clockLbl.Name = "clockLbl";
            this.clockLbl.Size = new System.Drawing.Size(65, 16);
            this.clockLbl.Text = "HH:MM:SS";
            this.clockLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timerBattery
            // 
            this.timerBattery.Enabled = true;
            this.timerBattery.Interval = 1000;
            this.timerBattery.Tick += new System.EventHandler(this.timerBattery_Tick);
            // 
            // HHTToolBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.clockLbl);
            this.Controls.Add(this.batteryBar);
            this.Name = "HHTToolBar";
            this.Size = new System.Drawing.Size(100, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar batteryBar;
        private System.Windows.Forms.Label clockLbl;
        public System.Windows.Forms.Timer timerBattery;
    }
}
