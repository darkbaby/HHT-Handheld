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
    public partial class Saving : Form
    {
        private static Thread t;

        private int frame = 1;

        public Saving()
        {
            InitializeComponent();
        }

        public static void OpenSaving()
        {
            t = new Thread(delegate()
            {
                Saving saving = new Saving();
                saving.ShowDialog();
            });

            t.Start();
        }

        public static void CloseSaving()
        {
            t.Abort();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (frame == 12)
            {
                frame = 0;
            }
            frame++;
        }

        private void Saving_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 1), 0, 0, 81, 59);
        }

        
    }
}