using System;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Denso_HHT.FTPHelper;
using Denso_HHT.Module;
using Ionic.Zip;
using System.Data;
using System.Threading;
using System.Collections.Generic;

namespace Denso_HHT
{
    public partial class SendDataFTP : Form
    {
        delegate void ShowMessageCallback(string message, int mode);

        private CheckBox allCheckBox;

        private FTP m_ftp;

        private DateTime startTest;
        private DateTime endTest;

        private SendFTPMode mode;

        private int totalRecord;

        public SendDataFTP(SendFTPMode mode)
        {
            InitializeComponent();
            this.mode = mode;
            switch (mode)
            {
                case SendFTPMode.All:
                    lbMode.Text += "All";
                    break;
                case SendFTPMode.OnlyNew:
                    lbMode.Text += "Only New";
                    break;
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

        private void SendDataFTP_Load(object sender, EventArgs e)
        {
            Loading2.OpenLoading();

            string distinctLocation = DatabaseModule.Instance.QuerySelectDistinctLocationCodeFromSendData(mode);
            if (distinctLocation.Length != 0)
            {
                allCheckBox = new CheckBox();

                allCheckBox.Location = new Point(5, 5);

                allCheckBox.Size = new Size(80, 20);
                allCheckBox.Text = "All";
                allCheckBox.CheckStateChanged += AllCheckBox_CheckStateChanged;
                panel1.Controls.Add(allCheckBox);

                string[] splitLocation = distinctLocation.Split(',');
                for (int i = 0; i < splitLocation.Length; i++)
                {
                    CheckBox tempCheckBox = new CheckBox();

                    if (i % 2 == 1)
                    {
                        tempCheckBox.Location = new Point(90, ((i / 2) * 25) + 30);
                    }
                    else
                    {
                        tempCheckBox.Location = new Point(5, ((i / 2) * 25) + 30);
                    }

                    tempCheckBox.Size = new Size(80, 20);
                    tempCheckBox.Text = splitLocation[i];
                    tempCheckBox.CheckStateChanged += ComponentCheckBox_CheckStateChanged;

                    panel1.Controls.Add(tempCheckBox);
                }
            }

            Loading2.CloseLoading();
        }

        private void Connect()
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    m_ftp = new FTP(DatabaseModule.Instance.FtpServer);
                    m_ftp.ConnectionTimeout = 150;
                    //m_ftp = new FTP("192.168.20.83");
                    //m_ftp = new FTP("192.168.10.192");
                    //m_ftp.ResponseReceived += new FTPResponseHandler(m_ftp_ResponseReceived);
                    //m_ftp.Connected += new FTPConnectedHandler(m_ftp_Connected);
                    m_ftp.BeginConnect(DatabaseModule.Instance.FtpUsername, DatabaseModule.Instance.FtpPassword);
                    //m_ftp.BeginConnect("username", "");
                    //m_ftp.BeginConnect("FTPAdmin", "P@ssw0rd");
                    if (m_ftp.IsConnected)
                    {
                        break;
                    }
                }

                if (!m_ftp.IsConnected)
                {
                    throw new FTPException("Cant connect to ftp server");
                }
            }
            catch (FTPException ex)
            {
                throw new FTPException("Cant connect to ftp server");
                //throw new FTPException("Cant connect to ftp server" + "\r\n" + "Exception : " + ex.Message);
            }

        }

        private void Disconnect()
        {
            m_ftp.Disconnect();
        }

        //void m_ftp_Connected(FTP source)
        //{
        //    AppendMessage("Connected");
        //}

        //void m_ftp_ResponseReceived(FTP source, FTPResponse Response)
        //{
        //    AppendMessage(Response.Text);
        //}

        private void ShowMessage(string newMessage, int mode)
        {
            if (this.InvokeRequired)
            {
                ShowMessageCallback d = new ShowMessageCallback(ShowMessage);
                this.Invoke(d, new object[] { newMessage, mode });
            }
            else
            {
                if (mode == 1)
                {
                    if ("".Equals(newMessage) || newMessage == null)
                    {
                        newMessage = "Problem Occured, please try again.";
                    }
                    MessageBox.Show(newMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand,
                        MessageBoxDefaultButton.Button1);
                }
                else
                {
                    TimeSpan usedTime = (startTest - endTest).Duration();
                    string hours = usedTime.Hours.ToString().PadLeft(2, '0');
                    string minutes = usedTime.Minutes.ToString().PadLeft(2, '0');
                    string seconds = usedTime.Seconds.ToString().PadLeft(2, '0');
                    string hms = hours + ":" + minutes + ":" + seconds;
                    MessageBox.Show("Send Data Successful" + "\r\n" + "Used Time : " + hms, "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                }
            }
        }

        //void tbMessage_TextChanged(object sender, EventArgs e)
        //{
        //    tbMessage.SelectionStart = tbMessage.TextLength;
        //    tbMessage.ScrollToCaret();
        //}

        private void ListAndDeleteErrorFile()
        {
            try
            {
                string currentHHID = DatabaseModule.Instance.HHTID;
                string fileList = m_ftp.GetFileList(false);
                string[] fileListSplit = null;
                if (fileList.Length > 0)
                {
                    fileList = fileList.Replace(Environment.NewLine, ",");
                    fileListSplit = fileList.Split(',');
                }

                if (fileListSplit != null)
                {
                    List<string> filesName = new List<string>();

                    for (int i = 0; i < fileListSplit.Length - 1; i++)
                    {
                        if (fileListSplit[i].Length > 3
                            && fileListSplit[i].Substring(0, 3).Equals(DatabaseModule.Instance.HHTID))
                        {
                            if (fileListSplit[i].Contains(".zip"))
                            {
                                m_ftp.DeleteFile(fileListSplit[i]);
                            }
                            else
                            {
                                filesName.Add(fileListSplit[i]);
                            }
                        }
                    }

                    if (filesName.Count > 0)
                    {
                        foreach (string item in filesName)
                        {
                            m_ftp.DeleteFile(item);
                        }
                    }
                }
            }
            catch (FTPException ex)
            {
                throw new FTPException("Cant send command to ftp server");
            }
        }

        private void SendFile()
        {
            try
            {
                string remoteFileName = DatabaseModule.Instance.HHTID + "_" + startTest.Year +
                    "" + startTest.Month.ToString().PadLeft(2, '0') +
                    "" + startTest.Day.ToString().PadLeft(2, '0') +
                    "" + startTest.Hour.ToString().PadLeft(2, '0') +
                    "" + startTest.Minute.ToString().PadLeft(2, '0') +
                    "" + startTest.Second.ToString().PadLeft(2, '0') +
                    "_" + totalRecord.ToString();

                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

                using (FileStream stream = File.OpenRead(path + @"\data.zip"))
                {
                    using (FileStream stream2 = File.OpenRead(path + @"\temp.dat"))
                    {
                        m_ftp.SendFile(stream2, remoteFileName);
                        Thread.Sleep(500);
                        m_ftp.SendFile(stream, remoteFileName + ".zip");
                        Thread.Sleep(500);
                        m_ftp.DeleteFile(remoteFileName);
                    }
                }
            }
            catch (FTPException ex)
            {
                throw new Exception("Cant send file to ftp server" + "\r\n" + "Exception : " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CreateRecordFile(string[] input)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (File.Exists(path + @"\Record.txt"))
            {
                File.Delete(path + @"\Record.txt");
            }

            if (File.Exists(path + @"\temp.dat"))
            {
                File.Delete(path + @"\temp.dat");
            }

            using (StreamWriter sw = new StreamWriter(new FileStream(path + @"\temp.dat", FileMode.Create), Encoding.UTF8))
            {

            }

            using (StreamWriter sw = new StreamWriter(new FileStream(path + @"\Record.txt", FileMode.Create), Encoding.UTF8))
            {
                sw.WriteLine(DatabaseModule.Instance.HHTID);
                sw.WriteLine(DatabaseModule.Instance.HHTName);
                sw.WriteLine(mode == SendFTPMode.All ? "1" : "2");
                DataTable dt = DatabaseModule.Instance.QuerySelectAllFromSendData(input, mode);
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
            using (ZipFile zip = new ZipFile())
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                if (File.Exists(path + @"\data.zip"))
                {
                    File.Delete(path + @"\data.zip");
                }

                zip.AddItem(path + @"\Record.txt", "");
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.Save(path + @"\data.zip");
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
                MessageBox.Show("Please select at least 1 Location", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            DialogResult result = MessageBox.Show("After sending process, the screen will be closed. Are you sure to send stocktaking data to PC?",
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                Dialog d = new Dialog();
                string input = GetLocationSelected();
                string[] inputSplit = input.Split(',');
                Thread t = new Thread(delegate()
                {
                    SendDataThread(d, inputSplit);
                });
                t.IsBackground = true;
                t.Start();

                d.ShowDialog();
                this.Show();

                if (d.DialogResult == DialogResult.OK)
                {
                    this.Dispose();
                }
            }
        }

        private void SendDataThread(Dialog dialog, string[] inputSplit)
        {
            try
            {
                startTest = DateTime.Now;

                Connect();
                ListAndDeleteErrorFile();
                Disconnect();

                dialog.ChangeProgressValue(10);

                if (!CreateRecordFile(inputSplit))
                {
                    throw new Exception("No data needed to send to ftp server");
                }

                dialog.ChangeProgressValue(35);

                CreateZipFile();

                dialog.ChangeProgressValue(50);

                Connect();
                //CreateAndChangeWorkingDirectory();
                SendFile();
                Disconnect();

                dialog.ChangeProgressValue(70);

                //DatabaseModule.Instance.InitWithOutRefresh();
                DatabaseModule.Instance.QueryUpdateShowStatusFromSendData(true, inputSplit);

                dialog.ChangeProgressValue(90);

                //DatabaseModule.Instance.Refresh();
                Thread.Sleep(1000);

                dialog.ChangeProgressValue(100);

                endTest = DateTime.Now;

                ShowMessage("", 2);

                dialog.isInterrupt = true;
            }
            catch (FTPException ex)
            {
                if (m_ftp.IsConnected)
                {
                    m_ftp.Disconnect();
                }
                ShowMessage(ex.Message, 1);
                dialog.isInterrupt = true;
            }

            catch (Exception ex)
            {
                ShowMessage(ex.Message, 1);
                dialog.isInterrupt = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetEnableComponent(false);
            //DatabaseModule.Instance.InitWithOutRefresh();
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

    public enum SendFTPMode
    {
        All, OnlyNew
    }
}