using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Denso_HHT.Module;
using System.IO;
using System.Reflection;
using Ionic.Zip;

namespace Denso_HHT
{
    public partial class SendAndReceiveDataProcess : Form
    {
        private SendDataMode sendDataMode;

        private SendFTPMode sendFTPMode;

        private CheckBox allCheckBox;

        private int totalRecord;

        private string[] inputSplit;

        private string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

        public SendAndReceiveDataProcess(SendDataMode mode1, SendFTPMode mode2)
        {
            InitializeComponent();

            this.sendDataMode = mode1;
            this.sendFTPMode = mode2;

            if (this.sendDataMode == SendDataMode.Offline)
            {
                constMenu.Text += " (Cable)";
            }
            else
            {
                constMenu.Text += " (WIFI)";
            }

            if (this.sendFTPMode == SendFTPMode.All)
            {
                lbMode.Text += "All";
            }
            else
            {
                lbMode.Text += "Only New";
            }

            string[] files = Directory.GetFiles(path + @"\temp");
            if (files.Length != 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    File.Delete(files[i]);
                }
            }
        }

        private void SetEnableComponent(bool value)
        {
            foreach (Control item in this.Controls)
            {
                if (item.GetType() == typeof(Button))
                {
                    item.Enabled = value;
                }
            }
        }

        private void SendAndReceiveDataProcess_Load(object sender, EventArgs e)
        {
            Loading2.OpenLoading();

            string distinctLocation = DatabaseModule.Instance.QuerySelectDistinctLocationCodeFromSendData(sendFTPMode);
            if (distinctLocation.Length != 0)
            {
                allCheckBox = new CheckBox();
                allCheckBox.Size = new Size(80, 20);

                allCheckBox.Location = new Point(5, 5);

                allCheckBox.Text = "All";
                allCheckBox.CheckStateChanged += AllCheckBox_CheckStateChanged;
                panel1.Controls.Add(allCheckBox);

                string[] splitLocation = distinctLocation.Split(',');
                for (int i = 0; i < splitLocation.Length; i++)
                {
                    CheckBox tempCheckBox = new CheckBox();
                    tempCheckBox.Size = new Size(80, 20);

                    if (i % 2 == 1)
                    {
                        tempCheckBox.Location = new Point(90, ((i / 2) * 25) + 30);
                    }
                    else
                    {
                        tempCheckBox.Location = new Point(5, ((i / 2) * 25) + 30);
                    }

                    tempCheckBox.Text = splitLocation[i];
                    tempCheckBox.CheckStateChanged += ComponentCheckBox_CheckStateChanged;
                    panel1.Controls.Add(tempCheckBox);
                }
            }

            Loading2.CloseLoading();
        }

        private bool CreateRecordFile(string[] input)
        {
            DateTime startTest = DateTime.Now;

            using (StreamWriter sw = new StreamWriter(new FileStream(path + @"\temp\Record.txt", FileMode.Create), Encoding.UTF8))
            {
                sw.WriteLine(DatabaseModule.Instance.HHTID);
                sw.WriteLine(DatabaseModule.Instance.HHTName);
                sw.WriteLine(sendFTPMode == SendFTPMode.All ? "1" : "2");
                DataTable dt = DatabaseModule.Instance.QuerySelectAllFromSendData(input, sendFTPMode);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        sw.WriteLine(row[0].ToString() + "," + row[1].ToString() + "," + row[2].ToString() + "," +
                            row[3].ToString() + "," + row[4].ToString() + "," + row[5].ToString() + "," +
                            row[6].ToString() + "," + row[7].ToString() + "," + row[8].ToString() + "," +
                            row[9].ToString() + "," + row[10].ToString() + "," +
                            row[12].ToString() + "," + row[13].ToString() + "," + row[14].ToString() + "," +
                            row[15].ToString());
                    }
                    totalRecord = dt.Rows.Count;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void CreateZipFile()
        {
            DateTime startTest = DateTime.Now;

            string remoteFileName = DatabaseModule.Instance.HHTID + "_" + startTest.Year +
                    "" + startTest.Month.ToString().PadLeft(2, '0') +
                    "" + startTest.Day.ToString().PadLeft(2, '0') +
                    "" + startTest.Hour.ToString().PadLeft(2, '0') +
                    "" + startTest.Minute.ToString().PadLeft(2, '0') +
                    "" + startTest.Second.ToString().PadLeft(2, '0') +
                    "_" + totalRecord.ToString();

            using (ZipFile zip = new ZipFile())
            {
                zip.AddItem(path + @"\temp\Record.txt", "");
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.Save(path + @"\temp\" + remoteFileName + ".zip");
                File.Delete(path + @"\temp\Record.txt");
            }

            using (StreamWriter sw = new StreamWriter(new FileStream(path + @"\temp\" + remoteFileName, FileMode.Create), Encoding.UTF8))
            {

            }

            CreateInformationFile(remoteFileName);
        }

        private void CreateInformationFile(string data)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(path + @"\temp\information.txt", FileMode.Create), Encoding.UTF8))
            {
                sw.WriteLine(data);
            }
        }

        private bool GetCheckedState()
        {
            bool returnBool = false;
            foreach (Control item in panel1.Controls)
            {
                if (item.GetType() == typeof(CheckBox))
                {
                    CheckBox tempItem = (CheckBox)item;
                    if (tempItem.Checked)
                    {
                        returnBool = true;
                        break;
                    }
                }
            }
            return returnBool;
        }

        private string GetLocationSelected()
        {
            string returnString = "";
            foreach (Control item in panel1.Controls)
            {
                if (item.GetType() == typeof(CheckBox))
                {
                    CheckBox tempItem = (CheckBox)item;
                    if (tempItem.Checked && !tempItem.Text.Equals("All"))
                    {
                        returnString += item.Text + ",";
                    }
                }
            }
            if (returnString.Length > 0)
            {
                returnString = returnString.Substring(0, returnString.Length - 1);
            }
            return returnString;
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if (!GetCheckedState())
            {
                MessageBox.Show("Please select at least 1 Location", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                SetEnableComponent(true);
                return;
            }

            try
            {
                SetEnableComponent(false);
                string input = GetLocationSelected();
                inputSplit = input.Split(',');
                CreateRecordFile(inputSplit);
                CreateZipFile();
                SendAndReceiveDataResult sendAndReceiveDataResult = new SendAndReceiveDataResult(sendDataMode);
                sendAndReceiveDataResult.ShowDialog();
                this.Dispose();
            }
            catch (Exception ex)
            {
                SetEnableComponent(true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            this.Dispose();
        }

        private void AllCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox tempCheckBox = (CheckBox)sender;
            if (tempCheckBox.Checked)
            {
                foreach (Control item in panel1.Controls)
                {
                    if (item.GetType() == typeof(CheckBox))
                    {
                        CheckBox tempCheckBox2 = (CheckBox)item;
                        if (tempCheckBox2.Text != "All")
                        {
                            tempCheckBox2.Checked = true;
                        }
                    }
                }
            }
            else
            {
                foreach (Control item in panel1.Controls)
                {
                    if (item.GetType() == typeof(CheckBox))
                    {
                        CheckBox tempCheckBox2 = (CheckBox)item;

                        if (tempCheckBox2.Text != "All")
                        {
                            tempCheckBox2.Checked = false;
                        }
                    }
                }
            }
        }

        private void ComponentCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox tempCheckBox = (CheckBox)sender;
            if (tempCheckBox.Checked)
            {
                foreach (Control item in panel1.Controls)
                {
                    if (item.GetType() == typeof(CheckBox))
                    {
                        CheckBox tempCheckBox2 = (CheckBox)item;
                        if (!tempCheckBox2.Checked && tempCheckBox2.Text != "All")
                        {
                            return;
                        }
                    }
                }

                allCheckBox.CheckStateChanged -= AllCheckBox_CheckStateChanged;
                allCheckBox.Checked = true;
                allCheckBox.CheckStateChanged += AllCheckBox_CheckStateChanged;
            }
            else
            {
                allCheckBox.CheckStateChanged -= AllCheckBox_CheckStateChanged;
                allCheckBox.Checked = false;
                allCheckBox.CheckStateChanged += AllCheckBox_CheckStateChanged;
            }
        }
    }
}