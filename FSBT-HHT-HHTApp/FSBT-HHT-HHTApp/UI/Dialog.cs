using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Denso_HHT
{
    public partial class Dialog : Form
    {
        delegate void ChangeProgressValueCallback(int newValue);

        public bool isInterrupt = false;

        public Dialog()
        {
            InitializeComponent();
        }

        public void ChangeProgressValue(int newValue)
        {
            if (this.InvokeRequired)
            {
                ChangeProgressValueCallback d = new ChangeProgressValueCallback(ChangeProgressValue);
                this.Invoke(d, new object[] { newValue });
            }
            else
            {
                progressBar1.Value = newValue;
            }
        }

        private void timerInterrupt_Tick(object sender, EventArgs e)
        {
            if (label1.Text.Length == 13)
            {
                label1.Text = "Processing";
            }
            else
            {
                label1.Text += ".";
            }

            if (isInterrupt)
            {
                if (this.progressBar1.Value == 100)
                {
                    this.DialogResult = DialogResult.OK;
                    timerInterrupt.Enabled = false;
                    timerInterrupt.Dispose();
                    this.Dispose();
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    timerInterrupt.Enabled = false;
                    timerInterrupt.Dispose();
                    this.Dispose();
                }
            }
        }
        


    }
}