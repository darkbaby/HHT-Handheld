using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Denso_HHT
{
    public partial class Loading : Form
    {
        public delegate void DisposeThisLoading();

        public delegate void ShowThisMessageLoading(string message);

        public delegate void ChangeThisLabelLoading(string message);

        int frame = 1;

        public Loading()
        {
            InitializeComponent();
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Program.imagelist[0];
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (frame == 12)
            {
                frame = 0;
            }
            pictureBox1.Image = Program.imagelist[frame];
            pictureBox1.BackColor = Color.Transparent;
            frame++;
        }

        public void DisposeLoading()
        {
            if (InvokeRequired)
            {
                this.Invoke(new DisposeThisLoading(DisposeLoading));
            }
            else
            {
                timer1.Enabled = false;
                this.Dispose();
            }
        }

        public void ShowMessageLoading(string message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new ShowThisMessageLoading(ShowMessageLoading), new object[] { message });
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        public void ChangeMessageLabelLoading(string message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new ChangeThisLabelLoading(ChangeMessageLabelLoading), new object[] { message });
            }
            else
            {
                label1.Text = message;
            }
        }

        private void Loading_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 1), 0, 0, 81, 59);
        }
    }
}