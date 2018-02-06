using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Denso_HHT
{
    public partial class HHTToolBar : UserControl
    {
        private byte batteryLevel;
        private byte BatteryLevel
        {
            get { return this.batteryLevel; }
            set
            {
                if (!this.batteryLevel.Equals(value))
                {
                    this.batteryLevel = value;
                }
            }
        }

        public HHTToolBar()
        {
            InitializeComponent();
            this.clockLbl.Text = DateTime.Now.ToString("HH:mm:ss");
            BatteryLevel = BatteryInfo.GetBatteryLifePercent();
            batteryBar.Value = BatteryLevel;
            if (BatteryLevel < 20)
            {
                this.batteryBar.ForeColor = Color.Red;
            }
        }

        public void HideDateTime()
        {
            this.clockLbl.Visible = false;
        }

        private void timerBattery_Tick(object sender, EventArgs e)
        {
            this.clockLbl.Text = DateTime.Now.ToString("HH:mm:ss");
            BatteryLevel = BatteryInfo.GetBatteryLifePercent();
            batteryBar.Value = BatteryLevel;
            if (BatteryLevel < 20)
            {
                this.batteryBar.ForeColor = Color.Red;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            timerBattery.Enabled = false;
            timerBattery.Dispose();
            this.Dispose();
        }
    }
}
