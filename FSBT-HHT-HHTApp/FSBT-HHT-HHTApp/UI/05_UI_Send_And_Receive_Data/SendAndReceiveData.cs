using System;
using System.Windows.Forms;
using System.Net;
using OpenNETCF.Net.NetworkInformation;
using Denso_HHT.Module;
using Microsoft.Win32;
using System.IO;
using System.Reflection;

namespace Denso_HHT
{
    public partial class SendAndReceiveData : Form
    {
        private SendDataMode sendDataMode;

        private long firstLastSize;

        private string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

        public SendAndReceiveData(SendDataMode sendDataMode)
        {
            InitializeComponent();
            this.sendDataMode = sendDataMode;
            DatabaseModule.Instance.End();

            FileInfo f = new FileInfo(path + @"\Database\STOCKTAKING_HHT.sdf");
            firstLastSize = f.Length;
        }

        private void SendData_Load(object sender, EventArgs e)
        {
            if (this.sendDataMode == SendDataMode.Online)
            {
                SendData_Load_Online();
            }
            else
            {
                SendData_Load_Offline();
            }
        }


        private void SendData_Load_Online()
        {
            constMenu.Text = "Syncing Data (WIFI)";

            bool isfoundWireless = false;
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    lbIPAddress.Text = item.CurrentIpAddress.ToString();
                    isfoundWireless = true;       
                    break;
                }
            }
            if (isfoundWireless)
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Comm\FTPD", true);
                key.SetValue("AllowAnonymous", 1);
            }
            else
            {
                MessageBox.Show("Your Device is not connected to wifi", "Warning");
                this.Dispose();
            }
        }

        private void SendData_Load_Offline()
        {
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostByName("PPP_PEER");
                if (ipHostEntry != null)
                {
                    constMenu.Text = "Syncing (Cable)";
                    const1.Text = "Ready for syncing data";
                    lbIPAddress.Text = "";
                }
                else
                {
                    throw new Exception("not connected");
                }
            }
            catch (Exception ex)
            {
                const1.Text = "Please connect your device to PC";
            }
            finally
            {
                timerCheckAS.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (sendDataMode == SendDataMode.Offline)
            {
                if (const1.Text.Contains("Ready"))
                {
                    DialogResult result = MessageBox.Show("If your device is under process, Exiting might harm it. Do you want to Exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        this.Dispose();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    this.Dispose();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("If your device is under process, Exiting might harm it. Do you want to Exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    this.Dispose();
                }
                else
                {
                    return;
                }
            }
        }

        private void timerCheckAS_Tick(object sender, EventArgs e)
        {
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostByName("PPP_PEER");
                if (ipHostEntry != null)
                {
                    constMenu.Text = "Send Data (Cable)";
                    const1.Text = "Ready for syncing data";
                    lbIPAddress.Text = "";
                }
                else
                {
                    throw new Exception("not connected");
                }
            }
            catch (Exception ex)
            {
                const1.Text = "Please connect your device to PC";
            }
        }
    }

    //public enum SendDataMode
    //{
    //    Online, Offline
    //}
}