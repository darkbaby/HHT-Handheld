using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Denso_HHT
{
    public partial class Loading2 : Form
    {
        private static Thread t;

        private int frame = 1;

        public Loading2()
        {
            InitializeComponent();
        }

        public static void OpenLoading()
        {
            t = new Thread(delegate()
            {
                Loading2 loading2 = new Loading2();
                loading2.ShowDialog();
            });

            t.Start();
        }

        public static void CloseLoading()
        {
            t.Abort();
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

        private void Loading_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 1), 0, 0, 81, 59);
        }
    }
}