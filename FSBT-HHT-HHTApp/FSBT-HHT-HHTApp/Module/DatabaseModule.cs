using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;

namespace Denso_HHT.Module
{
    class DatabaseModule
    {
        private static DatabaseModule instance;

        public static DatabaseModule Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseModule();
                }
                return instance;
            }
        }

        private DatabaseModule()
        {
            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Database";
            string connectionStringStock = "Data Source=" + appPath + "\\" + "STOCKTAKING_HHT" + ".sdf" + "; Password=" + "1234";
            string connectionStringComputer = "Data Source=" + appPath + "\\" + "COMPUTER_NAME" + ".sdf" + "; Password=" + "1234";
            cnStock = new SqlCeConnection(connectionStringStock);
            cnComputer = new SqlCeConnection(connectionStringComputer);
        }

        private SqlCeConnection cnStock = null;
        private SqlCeConnection cnComputer = null;

        private List<mLocation> listLocation;
        private List<mSKU> listSKU;
        private List<mBarcode> listBarcode;
        private List<mPack> listPack;
        private List<SummaryLocation> listSummaryLocation;
        private List<mSerial> listSerial;
        private DataTable dtUnit;
        private DataTable dtSetting;
        private DataTable dtStocktakingFront;
        private DataTable dtStocktakingWarehouse;
        private DataTable dtStocktakingFreshFood;
        private DataTable dtStocktakingProduct;
        private DataTable dtStocktakingProductPack;
        private DataTable dtComputer;

        private DateTime currentTranDate;
        private int currentCount = -1;

        public string currentUser = null;
        public string currentDepartmentCode = null;
        public string HHTID = null;
        public string HHTName = null;
        public string FtpServer = null;
        public string FtpUsername = null;
        public string FtpPassword = null;
        public string Password = null;

        public void Init()
        {
            CultureInfo defaulCulture = new CultureInfo("en-US");
            Connect(0);
            Connect(1);

            Refresh();

            currentUser = QuerySelectSetting("Username");
            currentDepartmentCode = QuerySelectSetting("DepartmentCode");
            currentTranDate = DateTime.Now.Date;
            currentCount = QueryMaxCurrentCount();
            HHTID = QuerySelectSetting("HHTID");
            HHTName = QuerySelectSetting("HHTName");
            FtpServer = QuerySelectSetting("FTPServer");
            FtpUsername = QuerySelectSetting("FTPUsername");
            FtpPassword = QuerySelectSetting("FTPPassword");
            Password = QuerySelectSetting("Password");
        }

        public void InitWithOutRefresh()
        {
            Connect(0);
            Connect(1);
        }

        public void Refresh()
        {
            SqlCeCommand cmd = null;
            SqlCeDataAdapter da = null;

            listLocation = new List<mLocation>();
            cmd = new SqlCeCommand("SELECT LocationCode,SectionCode,SectionName,BrandCode FROM tb_m_Location", cnStock);
            cmd.CommandType = CommandType.Text;
            SqlCeDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listLocation.Add(new mLocation
                {
                    LocationCode = reader.GetString(0),
                    SectionCode = reader.GetString(1),
                    //ScanMode = reader.GetInt32(2),
                    SectionName = reader.GetString(2),
                    BrandCode = reader.GetString(3)
                });
            }

            listSKU = new List<mSKU>();
            cmd = new SqlCeCommand("SELECT Description,ExBarcode,InBarcode,BrandCode,SKUCode,MKCode FROM tb_m_SKU", cnStock);
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listSKU.Add(new mSKU
                {
                    Description = reader.IsDBNull(0) ? null : reader.GetString(0),
                    ExBarcode = reader.IsDBNull(1) ? null : reader.GetString(1),
                    InBarcode = reader.IsDBNull(2) ? null : reader.GetString(2),
                    BrandCode = reader.IsDBNull(3) ? null : reader.GetString(3),
                    SKUCode = reader.IsDBNull(4) ? null : reader.GetString(4),
                    MKCode = reader.IsDBNull(5) ? null : reader.GetString(5),
                });
            }

            listBarcode = new List<mBarcode>();
            cmd = new SqlCeCommand("SELECT ExBarcode,SKUCode FROM tb_m_Barcode", cnStock);
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listBarcode.Add(new mBarcode
                {
                    ExBarcode = reader.IsDBNull(0) ? null : reader.GetString(0),
                    SKUCode = reader.IsDBNull(1) ? null : reader.GetString(1)
                });
            }

            listPack = new List<mPack>();
            cmd = new SqlCeCommand("SELECT Barcode,SKUCode FROM tb_m_Pack", cnStock);
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listPack.Add(new mPack
                {
                    Barcode = reader.IsDBNull(0) ? null : reader.GetString(0),
                    SKUCode = reader.IsDBNull(1) ? null : reader.GetString(1)
                });
            }

            listSerial = new List<mSerial>();
            cmd = new SqlCeCommand("SELECT SKUCode,Barcode,SerialNumber,StorageLocation FROM tb_m_SerialNumber", cnStock);
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listSerial.Add(new mSerial
                {
                    SKUCode = reader.IsDBNull(0) ? null : reader.GetString(0),
                    BarCode = reader.IsDBNull(1) ? null : reader.GetString(1),
                    SerialNumber = reader.IsDBNull(2) ? null : reader.GetString(2),
                    StorageLocation = reader.IsDBNull(3) ? null : reader.GetString(3)
                    
                });
            }

            cmd = new SqlCeCommand("SELECT * FROM tb_m_Unit ", cnStock);
            cmd.CommandType = CommandType.Text;
            da = new SqlCeDataAdapter(cmd);
            dtUnit = new DataTable("tb_m_Unit");
            da.Fill(dtUnit);

            cmd = new SqlCeCommand("SELECT * FROM tb_s_Setting ", cnStock);
            cmd.CommandType = CommandType.Text;
            da = new SqlCeDataAdapter(cmd);
            dtSetting = new DataTable("tb_s_Setting");
            da.Fill(dtSetting);

            cmd = new SqlCeCommand("SELECT * FROM (SELECT TOP(100) * FROM tb_t_Stocktaking WHERE (ScanMode = 1 OR ScanMode = 2) ORDER BY StocktakingID DESC) AS t1 ORDER BY StocktakingID ASC", cnStock);
            cmd.CommandType = CommandType.Text;
            da = new SqlCeDataAdapter(cmd);
            dtStocktakingFront = new DataTable("tb_t_StocktakingFront");
            da.Fill(dtStocktakingFront);

            cmd = new SqlCeCommand("SELECT * FROM (SELECT TOP(100) * FROM tb_t_Stocktaking WHERE ScanMode = 3 ORDER BY StocktakingID DESC) AS t1 ORDER BY StocktakingID ASC", cnStock);
            cmd.CommandType = CommandType.Text;
            da = new SqlCeDataAdapter(cmd);
            dtStocktakingWarehouse = new DataTable("tb_t_StocktakingWarehouse");
            da.Fill(dtStocktakingWarehouse);

            cmd = new SqlCeCommand("SELECT * FROM (SELECT TOP(100) * FROM tb_t_Stocktaking WHERE ScanMode in (5,6,7 ) ORDER BY StocktakingID DESC) AS t1 ORDER BY StocktakingID ASC", cnStock);
            cmd.CommandType = CommandType.Text;
            da = new SqlCeDataAdapter(cmd);
            dtStocktakingFreshFood = new DataTable("tb_t_StocktakingFreshFood");
            da.Fill(dtStocktakingFreshFood);

            cmd = new SqlCeCommand("SELECT * FROM (SELECT TOP(100) * FROM tb_t_Stocktaking WHERE ScanMode in (1,2 ) ORDER BY StocktakingID DESC) AS t1 ORDER BY StocktakingID ASC", cnStock);
            cmd.CommandType = CommandType.Text;
            da = new SqlCeDataAdapter(cmd);
            dtStocktakingProduct = new DataTable("tb_t_StocktakingProduct");
            da.Fill(dtStocktakingProduct);

            cmd = new SqlCeCommand("SELECT * FROM (SELECT TOP(100) * FROM tb_t_Stocktaking WHERE ScanMode in (3,4 ) ORDER BY StocktakingID DESC) AS t1 ORDER BY StocktakingID ASC", cnStock);
            cmd.CommandType = CommandType.Text;
            da = new SqlCeDataAdapter(cmd);
            dtStocktakingProductPack = new DataTable("tb_t_StocktakingProductPack");
            da.Fill(dtStocktakingProductPack);

            cmd = new SqlCeCommand("SELECT * FROM Computer", cnComputer);
            cmd.CommandType = CommandType.Text;
            da = new SqlCeDataAdapter(cmd);
            dtComputer = new DataTable("Computer");
            da.Fill(dtComputer);

            listSummaryLocation = new List<SummaryLocation>();
            cmd = new SqlCeCommand("SELECT LocationCode,UnitCode,SUM(Quantity) FROM tb_t_Stocktaking GROUP BY LocationCode,UnitCode", cnStock);
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //DataRow row = dtUnit.Select(string.Format("UnitCode = {0}", reader.GetInt32(1))).First();
                listSummaryLocation.Add(new SummaryLocation
                {
                    LocationCode = reader.GetString(0),
                    UnitCode = reader.GetInt32(1),
                    TotalQuantity = reader.GetDecimal(2),
                    TotalRecord = 0
                });
            }

            cmd = new SqlCeCommand("SELECT LocationCode,Count(StocktakingID) FROM tb_t_Stocktaking GROUP BY LocationCode", cnStock);
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string locationCode = reader.GetString(0);
                foreach (SummaryLocation item in listSummaryLocation)
                {
                    if (locationCode.Equals(item.LocationCode))
                    {
                        item.TotalRecord = reader.GetInt32(1);
                        break;
                    }
                }
            }
            reader.Close();
        }

        public void End()
        {
            Disconnect(0);
            Disconnect(1);
        }

        private void Connect(int whichOne)
        {
            try
            {
                switch (whichOne)
                {
                    case 0:
                        if (cnStock.State == ConnectionState.Closed)
                        {
                            cnStock.Open();
                        }
                        break;
                    case 1:
                        if (cnComputer.State == ConnectionState.Closed)
                        {
                            cnComputer.Open();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Disconnect(int whichOne)
        {
            try
            {
                switch (whichOne)
                {
                    case 0:
                        if (cnStock.State != ConnectionState.Closed)
                        {
                            cnStock.Close();
                        }
                        break;
                    case 1:
                        if (cnComputer.State != ConnectionState.Closed)
                        {
                            cnComputer.Close();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string QuerySelectSetting(string key)
        {
            string query = "KeyMap = '{0}'";
            query = string.Format(query, key);
            DataRow row = dtSetting.Select(query).First();
            return string.IsNullOrEmpty(row[1].ToString()) ? "" : row[1].ToString();
        }

        public void QueryUpdateSetting(string key, string newValue)
        {
            if (newValue == null)
            {
                string query = "KeyMap = '{0}'";
                DataRow row = dtSetting.Select(string.Format(query, key)).First();
                row[1] = newValue;

                query = "UPDATE tb_s_Setting SET Value = @newValue WHERE KeyMap = '{0}'";
                SqlCeCommand cmd = new SqlCeCommand(string.Format(query, key), cnStock);
                cmd.Parameters.AddWithValue("@newValue", DBNull.Value);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                switch (key)
                {
                    case "HHTName":
                        HHTName = newValue; break;
                    case "HHTID":
                        HHTID = newValue; break;
                    case "Username":
                        currentUser = newValue; break;
                    case "DepartmentCode":
                        currentDepartmentCode = newValue; break;
                    case "FTPServer":
                        FtpServer = newValue; break;
                    case "FTPUsername":
                        FtpUsername = newValue; break;
                    case "FTPPassword":
                        FtpPassword = newValue; break;
                    case "TranDate":
                        currentTranDate = new DateTime(1990, 1, 1); break;
                    case "Password":
                        Password = newValue; break;
                    //case "TranCount":
                    //    currentCount = 0; break;
                }
            }
            else
            {
                string query = "KeyMap = '{0}'";
                DataRow row = dtSetting.Select(string.Format(query, key)).First();
                row[1] = newValue;

                query = "UPDATE tb_s_Setting SET Value = '{0}' WHERE KeyMap = '{1}'";
                SqlCeCommand cmd = new SqlCeCommand(string.Format(query, newValue, key), cnStock);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                switch (key)
                {
                    case "HHTName":
                        HHTName = newValue; break;
                    case "HHTID":
                        HHTID = newValue; break;
                    case "Username":
                        currentUser = newValue; break;
                    case "DepartmentCode":
                        currentDepartmentCode = newValue; break;
                    case "FTPServer":
                        FtpServer = newValue; break;
                    case "FTPUsername":
                        FtpUsername = newValue; break;
                    case "FTPPassword":
                        FtpPassword = newValue; break;
                    case "TranDate":
                        currentTranDate = DateTime.Parse(newValue); break;
                    case "Password":
                        Password = newValue; break;
                    //case "TranCount":
                    //    currentCount = Int32.Parse(newValue); break;
                }
            }
        }

        private int QueryMaxRecordNumberOfDateTime(DateTime dt)
        {
            string query = "SELECT StocktakingID FROM tb_t_Stocktaking WHERE StocktakingID LIKE '{0}%' ORDER BY StocktakingID DESC";
            string likeSearch = dt.ToString("yy") + dt.ToString("MM") + dt.ToString("dd") + HHTID;

            SqlCeCommand cmd = new SqlCeCommand(string.Format(query, likeSearch), cnStock);
            cmd.CommandType = CommandType.Text;
            object LastStocktaking = cmd.ExecuteScalar();
            if (LastStocktaking != null)
            {
                return Int32.Parse(LastStocktaking.ToString().Substring(9, 7));
            }
            else
            {
                return 0;
            }
        }

        private int QueryMaxCurrentCount()
        {
            DataRow rowTranDate = dtSetting.Select("KeyMap = 'TranDate'").First();
            DataRow rowTranCount = dtSetting.Select("KeyMap = 'TranCount'").First();

            if (rowTranDate[1] == DBNull.Value || rowTranCount[1] == DBNull.Value)
            {
                rowTranDate[1] = currentTranDate.Date;
                rowTranCount[1] = 0;

                QueryUpdateSetting("TranDate", currentTranDate.Date.ToString());
                QueryUpdateSetting("TranCount", "0");

                return 1;
            }
            else
            {
                DateTime dbDT = DateTime.Parse(rowTranDate[1].ToString());
                if (dbDT.Date.CompareTo(currentTranDate.Date) != 0)
                {
                    rowTranDate[1] = currentTranDate.Date;
                    rowTranCount[1] = 0;

                    QueryUpdateSetting("TranDate", currentTranDate.Date.ToString());
                    QueryUpdateSetting("TranCount", "0");

                    return 1;
                }
                else
                {
                    int maxRecord = QueryMaxRecordNumberOfDateTime(currentTranDate);
                    rowTranDate[1] = currentTranDate.Date;
                    rowTranCount[1] = maxRecord;
                    QueryUpdateSetting("TranDate", currentTranDate.Date.ToString());
                    QueryUpdateSetting("TranCount", maxRecord.ToString());
                    return maxRecord + 1;
                }
            }
        }

        #region Scan

        public string[] QuerySelectTotalByLocationFromScan(string locationCode, string unitName)
        {
            string query = "UnitName = '{0}'";
            DataRow row = dtUnit.Select(string.Format(query, unitName)).First();

            foreach (SummaryLocation item in listSummaryLocation)
            {
                if (item.LocationCode.Equals(locationCode) && item.UnitCode == (int)row[0] && unitName != "KG")
                {
                    return new string[] { ((int)item.TotalQuantity).ToString(), ((int)item.TotalRecord).ToString() };
                }
                else if (item.LocationCode.Equals(locationCode) && item.UnitCode == (int)row[0] && unitName == "KG")
                {
                    return new string[] { ((double)item.TotalQuantity).ToString(), ((int)item.TotalRecord).ToString() };
                }
            }

            return new string[] { "0", "0" };
        }

        public void QueryUpdateTotalByLocationFromScan(string locationCode, int newUnitCode, decimal addition, int mode)
        {
            SummaryLocation tempItem = null;
            bool isFound = false;
            foreach (SummaryLocation item in listSummaryLocation)
            {
                if (item.LocationCode.Equals(locationCode) && item.UnitCode.Equals(newUnitCode))
                {
                    isFound = true;
                    tempItem = item;
                }
            }

            if (!isFound)
            {
                listSummaryLocation.Add(new SummaryLocation { LocationCode = locationCode, UnitCode = newUnitCode, TotalQuantity = addition, TotalRecord = 1 });
            }
            else
            {
                switch (mode)
                {
                    case 1: tempItem.TotalQuantity += addition;
                        tempItem.TotalRecord++;
                        break;
                    case 2: tempItem.TotalQuantity -= addition;
                        tempItem.TotalRecord--;
                        break;
                }

            }
        }

        public void QueryUpdateTotalByLocationFromScan(string locationCode, int oldUnitCode, int newUnitCode, decimal originalQty, decimal addition)
        {
            SummaryLocation tempItem = null;
            SummaryLocation tempItem2 = null;
            bool isFound = false;
            foreach (SummaryLocation item in listSummaryLocation)
            {
                if (item.LocationCode.Equals(locationCode) && item.UnitCode.Equals(newUnitCode))
                {
                    isFound = true;
                    tempItem = item;
                }

                if (oldUnitCode.Equals(newUnitCode))
                {
                    continue;
                }
                else
                {
                    if (item.LocationCode.Equals(locationCode) && item.UnitCode.Equals(oldUnitCode))
                    {
                        tempItem2 = item;
                    }
                }
            }

            if (!isFound)
            {
                listSummaryLocation.Add(new SummaryLocation { LocationCode = locationCode, UnitCode = newUnitCode, TotalQuantity = addition, TotalRecord = 1 });
            }
            else
            {
                tempItem.TotalQuantity += addition;
            }

            if (tempItem2 != null)
            {
                tempItem2.TotalQuantity -= originalQty;
            }
        }

        public bool QuerySelectHaveLocationFromScan()
        {
            string query = "SELECT TOP(1) * FROM tb_m_Location";
            SqlCeCommand cmd = new SqlCeCommand(query, cnStock);
            cmd.CommandType = CommandType.Text;
            object value = cmd.ExecuteScalar();
            if (value != null)
            {
                return true;
            }
            else
            {
                return false;
            }

            //if (dtLocation.Rows.Count > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public bool QuerySelectHaveSKUMasterFromScan()
        {
            string query = "SELECT TOP(1) * FROM tb_m_SKU";
            SqlCeCommand cmd = new SqlCeCommand(query, cnStock);
            cmd.CommandType = CommandType.Text;
            object value = cmd.ExecuteScalar();
            if (value != null)
            {
                return true;
            }
            else
            {
                return false;
            }

            //if (dtSKU.Rows.Count > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public List<UnitModel> QuerySelectUnitFromScan(string codeType)
        {
            List<UnitModel> unitList = new List<UnitModel>();

            if (codeType.Equals("F"))
            {
                foreach (DataRow row in dtUnit.Rows)
                {
                    if (row[1].ToString().Equals("PCS") || row[1].ToString().Equals("KG"))
                    {
                        UnitModel addItem = new UnitModel();
                        addItem.UnitCode = Int32.Parse(row[0].ToString());
                        addItem.UnitName = row[1].ToString();
                        addItem.CodeType = row[2].ToString();
                        unitList.Add(addItem);
                    }
                }
                return unitList;
            }

            string query = "CodeType = '{0}'";
            DataRow[] dtRows = dtUnit.Select(string.Format(query, codeType));
            foreach (DataRow row in dtRows)
            {
                if (row[1].ToString().Equals("CS"))
                {
                    continue;
                }
                UnitModel addItem = new UnitModel();
                addItem.UnitCode = Int32.Parse(row[0].ToString());
                addItem.UnitName = row[1].ToString();
                addItem.CodeType = row[2].ToString();
                unitList.Add(addItem);
            }
            return unitList;
        }

        public string QuerySelectUnitNameFromScan(int unitCode)
        {
            string returnValue = dtUnit.Select(string.Format("UnitCode = {0}", unitCode)).First()[1].ToString();

            return returnValue;
        }

        public List<StockTakingModel> QuerySelectPreviousAuditDataFromScan(int mode)
        {
            DataTable dt = null;
            if (mode == 5|| mode == 6 || mode == 7)
            {
                dt = dtStocktakingFreshFood;
                if (dtStocktakingFreshFood.Rows.Count > 0) 
                {
                    try
                    {
                        dt = dtStocktakingFreshFood.AsEnumerable()
                                                .Where(row => row.Field<int>("ScanMode") == mode)
                                                .CopyToDataTable();
                    }
                    catch { dt = dtStocktakingFreshFood; }
                }
            }
            else if (mode == 1 || mode == 2)
            {
                dt = dtStocktakingProduct;
                try
                {
                    dt = dtStocktakingProduct.AsEnumerable()
                                             .Where(row => row.Field<int>("ScanMode") == mode)
                                             .CopyToDataTable();
                }
                catch { dt = dtStocktakingProduct; }

            }
            else if (mode == 3 || mode == 4)
            {
                dt = dtStocktakingProductPack;
                try
                {
                    dt = dtStocktakingProductPack.AsEnumerable()
                                                 .Where(row => row.Field<int>("ScanMode") == mode)
                                                 .CopyToDataTable();
                }
                catch { dt = dtStocktakingProductPack; }
                
            }

            List<StockTakingModel> returnList = new List<StockTakingModel>();
            foreach (DataRow row in dt.Rows)
            {
                StockTakingModel data = new StockTakingModel();

                data.StocktakingID = row[0].ToString();
                data.ScanMode = Int32.Parse(row[1].ToString());
                data.LocationCode = row[2].ToString();
                data.Barcode = row[3].ToString();
                data.Quantity = Convert.ToDecimal(row[4].ToString());
                data.UnitCode = Int32.Parse(row[5].ToString());
                data.Flag = row[6].ToString();
                data.Description = row[7] == DBNull.Value ? null : row[7].ToString();
                data.SKUCode = row[8] == DBNull.Value ? null : row[8].ToString();
                data.ExBarcode = row[9] == DBNull.Value ? null : row[9].ToString();
                data.InBarcode = row[10] == DBNull.Value ? null : row[10].ToString();
                data.BrandCode = row[11] == DBNull.Value ? null : row[11].ToString();
                data.SKUMode = Convert.ToBoolean(row[12]);
                data.DepartmentCode = row[13] == DBNull.Value ? null : row[13].ToString();
                data.SerialNumber = row[17] == DBNull.Value ? null : row[17].ToString();
                data.ConversionCounter = row[18] == DBNull.Value ? null : row[18].ToString();

                returnList.Add(data);
            }
            return returnList;
        }

        public List<SerialNumberModel> QuerySelectSerialNumberDataFromScan(string barcode)
        {
            //List<SerialNumberModel> returnList = returnList.FindAll(x => x.Barcode == barcode);          
            //return returnList;

            List<SerialNumberModel> Serials = new List<SerialNumberModel>();

            var results = Serials.FindAll(
            delegate(SerialNumberModel se)
            {
                return se.Barcode == barcode;
            }
            );

            return results;
        }

        public bool IsHaveSerialNumber(string barcode)
        {
            
            List<mSerial> Serials = listSerial;

            var results = Serials.FindAll(x => x.BarCode == barcode); 

            if (results.Count()>0 )
                return true;
            else
                return false;
        }

        public bool IsRightSerialNumber(string barcode, string Serial)
        {
            List<mSerial> Serials = listSerial;

            var results = Serials.FindAll(x => x.BarCode == barcode);
            if (results.Count > 0)
            {
                var serials = results.FindAll(s => s.SerialNumber == Serial);
                if (serials.Count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }

        //private string QuerySection(string locationCode)
        //{
        //    string query = "SELECT * FROM tb_m_Location WHERE LocationCode = '{0}'";
        //    SqlCeCommand cmd = new SqlCeCommand(string.Format(query, locationCode), cnStock);
        //    cmd.CommandType = CommandType.Text;
        //    SqlCeDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
        //    if (reader.Read())
        //    {
        //        string sectionDescription = reader.GetString(1) + " : " + reader.GetString(3);
        //        reader.Close();
        //        return sectionDescription;
        //    }
        //    else
        //    {
        //        reader.Close();
        //        return "";
        //    }
        //}

        public LocationModel QueryLocationFromScan(string locationBarcode, int mode)
        {
            mLocation tempLocation = null;
            //if (mode == 1)
            //{
            //    tempLocation = listLocation.Where(x => x.LocationCode == locationBarcode && (x.ScanMode == 1 || x.ScanMode == 2)).FirstOrDefault();
            //}
            //else if (mode == 2)
            //{
            //    tempLocation = listLocation.Where(x => x.LocationCode == locationBarcode && x.ScanMode == 3).FirstOrDefault();
            //}
            //else if (mode == 3)
            //{
            //    tempLocation = listLocation.Where(x => x.LocationCode == locationBarcode && x.ScanMode == 4).FirstOrDefault();
            //}
            //else if (mode == 4)
            //{
            //    tempLocation = listLocation.Where(x => x.LocationCode == locationBarcode).FirstOrDefault();
            //}

            tempLocation = listLocation.Where(x => x.LocationCode == locationBarcode).FirstOrDefault();

            if (tempLocation != null)
            {
                LocationModel returnQueryLocation = new LocationModel();
                returnQueryLocation.LocationCode = tempLocation.LocationCode; //LocationCode
                returnQueryLocation.SectionCode = tempLocation.SectionCode; //SectionCode
                returnQueryLocation.SectionName = tempLocation.SectionName;//SectionName
                //returnQueryLocation.ScanMode = tempLocation.ScanMode; //ScanMode
                returnQueryLocation.BrandCode = tempLocation.BrandCode; //BrandCode
                return returnQueryLocation;
            }
            else
            {
                return null;
            }
        }

        public SKUModel QueryProductFromScan(string productBarcode)
        {
            if (productBarcode.Length == 5)
            {
                mSKU tempSKU = listSKU.Where(x => x.MKCode == productBarcode).FirstOrDefault();

                if (tempSKU != null)
                {
                    SKUModel returnQueryProduct = new SKUModel();
                    returnQueryProduct.Description = tempSKU.Description; //Description
                    returnQueryProduct.SKUCode = tempSKU.SKUCode; //SKUCode
                    returnQueryProduct.InBarcode = tempSKU.InBarcode; //InBarCode
                    returnQueryProduct.ExBarcode = tempSKU.ExBarcode; //ExBarCode
                    returnQueryProduct.BrandCode = tempSKU.BrandCode; //BrandCode
                    returnQueryProduct.DepartmentCode = null;
                    //returnQueryProduct.SerialNumber = null;
                    //returnQueryProduct.ConversionCounter = null;
                    return returnQueryProduct;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                mSKU tempSKU = listSKU.Where(x => x.ExBarcode == productBarcode || x.InBarcode == productBarcode).FirstOrDefault();
                if (tempSKU != null)
                {
                    SKUModel returnQueryProduct = new SKUModel();
                    returnQueryProduct.Description = tempSKU.Description; //Description
                    returnQueryProduct.SKUCode = tempSKU.SKUCode; //SKUCode
                    returnQueryProduct.InBarcode = tempSKU.InBarcode; //InBarCode
                    returnQueryProduct.ExBarcode = tempSKU.ExBarcode; //ExBarCode
                    returnQueryProduct.BrandCode = tempSKU.BrandCode; //BrandCode
                    returnQueryProduct.DepartmentCode = null;
                    //returnQueryProduct.SerialNumber = null;
                    //returnQueryProduct.ConversionCounter = null;
                    return returnQueryProduct;
                }
                else
                {
                    mBarcode tempBarcode = listBarcode.Where(x => x.ExBarcode == productBarcode).FirstOrDefault();
                    if (tempBarcode != null)
                    {
                        tempSKU = listSKU.Where(x => x.SKUCode == tempBarcode.SKUCode).FirstOrDefault();

                        if (tempSKU != null)
                        {
                            SKUModel returnQueryProduct = new SKUModel();
                            returnQueryProduct.Description = tempSKU.Description; //Description
                            returnQueryProduct.SKUCode = tempSKU.SKUCode; //SKUCode
                            returnQueryProduct.InBarcode = tempSKU.InBarcode; //InBarCode
                            returnQueryProduct.ExBarcode = tempSKU.ExBarcode; //ExBarCode
                            returnQueryProduct.BrandCode = tempSKU.BrandCode; //BrandCode
                            //returnQueryProduct.DepartmentCode = null;
                            //returnQueryProduct.SerialNumber = null;
                            return returnQueryProduct;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        mPack tempPack = listPack.Where(x => x.Barcode == productBarcode).FirstOrDefault();

                        if (tempPack != null)
                        {
                            tempSKU = listSKU.Where(x => x.SKUCode == tempPack.SKUCode).FirstOrDefault();

                            if (tempSKU != null)
                            {
                                SKUModel returnQueryProduct = new SKUModel();
                                returnQueryProduct.Description = tempSKU.Description; //Description
                                returnQueryProduct.SKUCode = tempSKU.SKUCode; //SKUCode
                                returnQueryProduct.InBarcode = tempSKU.InBarcode; //InBarCode
                                returnQueryProduct.ExBarcode = tempSKU.ExBarcode; //ExBarCode
                                returnQueryProduct.BrandCode = tempSKU.BrandCode; //BrandCode
                                returnQueryProduct.DepartmentCode = null;
                                //returnQueryProduct.SerialNumber = null;
                                //returnQueryProduct.ConversionCounter = null;
                                return returnQueryProduct;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            if (productBarcode.Length < 12)
                            {
                                return null;
                            }

                            string productBarcodeCut = productBarcode.Substring(4, 8);
                            tempSKU = listSKU.Where(x => x.SKUCode == productBarcodeCut).FirstOrDefault();

                            if (tempSKU != null)
                            {
                                SKUModel returnQueryProduct = new SKUModel();
                                returnQueryProduct.Description = tempSKU.Description; //Description
                                returnQueryProduct.SKUCode = tempSKU.SKUCode; //SKUCode
                                returnQueryProduct.InBarcode = tempSKU.InBarcode; //InBarCode
                                returnQueryProduct.ExBarcode = tempSKU.ExBarcode; //ExBarCode
                                returnQueryProduct.BrandCode = tempSKU.BrandCode; //BrandCode
                                returnQueryProduct.DepartmentCode = productBarcode.Substring(2, 2);
                                //returnQueryProduct.SerialNumber = null;
                                //returnQueryProduct.ConversionCounter = null;
                                return returnQueryProduct;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
        }

        public bool QuerySelectHaveSendFlagData()
        {
            string query = "SELECT TOP(1) StocktakingID FROM tb_t_Stocktaking WHERE SendFlag = 0";
            SqlCeCommand cmd = new SqlCeCommand(query, cnStock);
            cmd.CommandType = CommandType.Text;
            object temp = cmd.ExecuteScalar();
            if (temp != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<StockTakingModel> QuerySelectAllStocktakingSendFlagData()
        {
            List<StockTakingModel> returnList = new List<StockTakingModel>();
            string query = "SELECT * FROM tb_t_Stocktaking WHERE SendFlag = 0";
            SqlCeCommand cmd = new SqlCeCommand(query, cnStock);
            cmd.CommandType = CommandType.Text;
            SqlCeDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                StockTakingModel temp = new StockTakingModel();
                temp.StocktakingID = reader.GetString(0);
                temp.ScanMode = reader.GetInt32(1);
                temp.LocationCode = reader.GetString(2);
                temp.Barcode = reader.GetString(3);
                temp.Quantity = reader.GetDecimal(4);
                temp.UnitCode = reader.GetInt32(5);
                temp.Flag = reader.GetString(6);
                temp.Description = reader.IsDBNull(7) ? null : reader.GetString(7);
                temp.SKUCode = reader.IsDBNull(8) ? null : reader.GetString(8);
                temp.ExBarcode = reader.IsDBNull(9) ? null : reader.GetString(9);
                temp.InBarcode = reader.IsDBNull(10) ? null : reader.GetString(10);
                temp.BrandCode = reader.IsDBNull(11) ? null : reader.GetString(11);
                temp.SKUMode = reader.GetBoolean(12);
                temp.DepartmentCode = reader.IsDBNull(13) ? null : reader.GetString(13);
                temp.SendFlag = reader.GetBoolean(14);
                temp.SerialNumber = reader.IsDBNull(17) ? null : reader.GetString(17);
                temp.ConversionCounter = reader.IsDBNull(18) ? null : reader.GetString(18);
                returnList.Add(temp);
            }

            return returnList;
        }

        public void QueryUpdateFromScan(string uniqueID, decimal newQuantity, int newUnitCode, bool newSendFlag, int mode)
        {
            DataTable dt = null;
            //if (mode == 1)
            //{
            //    dt = dtStocktakingFront;
            //}
            //else if (mode == 2)
            //{
            //    dt = dtStocktakingWarehouse;
            //}
            //else if (mode == 3)
            //{
            //    dt = dtStocktakingFreshFood;
            //}

            if (mode == 5 || mode == 6 || mode == 7)
            {
                dt = dtStocktakingFreshFood;
            }
            else if (mode == 1 || mode == 2)
            {
                dt = dtStocktakingProduct;
            }
            else if (mode == 3 || mode == 4)
            {
                dt = dtStocktakingProductPack;
            }

            DataRow row = dt.Select("StocktakingID = '" + uniqueID + "'").FirstOrDefault();
            if (row != null)
            {
                row[4] = newQuantity;
                row[5] = newUnitCode;
            }

            string query = "UPDATE tb_t_Stocktaking SET Quantity = {0},UnitCode = {1}, SendFlag = {2} WHERE StocktakingID = '{3}'";
            SqlCeCommand cmd = new SqlCeCommand(string.Format(query, newQuantity,
                newUnitCode, newSendFlag ? 1 : 0, uniqueID), cnStock);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        public string GetLastStocktakingID()
        {
            CultureInfo defaulCulture = new CultureInfo("en-US");
            DateTime currentDT = DateTime.Now;

            if (currentDT.Date.CompareTo(currentTranDate.Date) != 0)
            {
                currentTranDate = currentDT.Date;
                DataRow tempRowTranDate = dtSetting.Select("KeyMap = 'TranDate'").First();
                DataRow tempRowTranCount = dtSetting.Select("KeyMap = 'TranCount'").First();
                tempRowTranDate[1] = currentDT.Date;
                tempRowTranCount[1] = QueryMaxRecordNumberOfDateTime(currentDT);

                QueryUpdateSetting("TranDate", currentDT.Date.ToString());
                QueryUpdateSetting("TranCount", tempRowTranCount[1].ToString());

                currentCount = Convert.ToInt32(tempRowTranCount[1]) + 1;

                return currentDT.ToString("yy") + currentDT.ToString("MM") + currentDT.ToString("dd") + HHTID + currentCount.ToString().PadLeft(7, '0');
            }
            else
            {
                string compare = currentDT.ToString("yy") + currentDT.ToString("MM") + currentDT.ToString("dd") + HHTID;
                string query = "SELECT TOP(1) StocktakingID FROM tb_t_Stocktaking WHERE StocktakingID LIKE '{0}%' ORDER BY StocktakingID DESC";
                query = string.Format(query, compare);
                SqlCeCommand cmd = new SqlCeCommand(query, cnStock);
                cmd.CommandType = CommandType.Text;
                object temp = cmd.ExecuteScalar();
                if (temp != null)
                {
                    string prepare = temp.ToString().Substring(9, 7);
                    currentCount = Int32.Parse(prepare) + 1;
                    return currentDT.ToString("yy") + currentDT.ToString("MM") + currentDT.ToString("dd") + HHTID + currentCount.ToString().PadLeft(7, '0');
                }
                else
                {
                    currentCount = 1;
                    return currentDT.ToString("yy") + currentDT.ToString("MM") + currentDT.ToString("dd") + HHTID + currentCount.ToString().PadLeft(7, '0');
                }
            }
        }

        public StockTakingModel GetLastInsertedStocktaking()
        {
            StockTakingModel lastInsertedStocktaking = new StockTakingModel();

            string query = "SELECT TOP(1) * FROM tb_t_Stocktaking ORDER BY CreateDate DESC";
            SqlCeCommand cmd = new SqlCeCommand(query, cnStock);
            cmd.CommandType = CommandType.Text;

            SqlCeDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lastInsertedStocktaking.StocktakingID = reader.IsDBNull(0) ? "" : reader.GetString(0);
                lastInsertedStocktaking.ScanMode = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                lastInsertedStocktaking.LocationCode = reader.IsDBNull(2) ? "" : reader.GetString(2);
                lastInsertedStocktaking.Barcode = reader.IsDBNull(3) ? "" : reader.GetString(3);
                lastInsertedStocktaking.Quantity = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4);
                lastInsertedStocktaking.UnitCode = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                lastInsertedStocktaking.Flag = reader.IsDBNull(6) ? "" : reader.GetString(6);
                lastInsertedStocktaking.SKUCode = reader.IsDBNull(8) ? "" : reader.GetString(8);
                lastInsertedStocktaking.ExBarcode = reader.IsDBNull(9) ? null : reader.GetString(9);
                lastInsertedStocktaking.InBarcode = reader.IsDBNull(10) ? null : reader.GetString(10);
                lastInsertedStocktaking.BrandCode = reader.IsDBNull(11) ? null : reader.GetString(11);
                lastInsertedStocktaking.SKUMode = reader.GetBoolean(12);
                lastInsertedStocktaking.SendFlag =  reader.GetBoolean(14);
                lastInsertedStocktaking.SerialNumber = reader.IsDBNull(17) ? null : reader.GetString(17);
                lastInsertedStocktaking.ConversionCounter = reader.IsDBNull(18) ? null : reader.GetString(18);
                return lastInsertedStocktaking;
            }

            return null;
        }

        public bool QueryUpdateLastStocktaking(StockTakingModel lastInsertedStocktaking,
            StockTakingModel data, int mode)
        {
            DataTable dt = null;
            //if (mode == 1)
            //{
            //    dt = dtStocktakingFront;
            //}
            //else if (mode == 2)
            //{
            //    dt = dtStocktakingWarehouse;
            //}
            //else if (mode == 3)
            //{
            //    dt = dtStocktakingFreshFood;
            //}

            if (mode == 5 || mode == 6 || mode == 7)
            {
                dt = dtStocktakingFreshFood;
            }
            else if (mode == 1 || mode == 2)
            {
                dt = dtStocktakingProduct;
            }
            else if (mode == 3 || mode == 4)
            {
                dt = dtStocktakingProductPack;
            }

            if (lastInsertedStocktaking.ScanMode == data.ScanMode
                && lastInsertedStocktaking.LocationCode == data.LocationCode
                && lastInsertedStocktaking.Barcode == data.Barcode
                && lastInsertedStocktaking.UnitCode == data.UnitCode
                && lastInsertedStocktaking.Flag == data.Flag
                && lastInsertedStocktaking.SKUMode == data.SKUMode
                && lastInsertedStocktaking.ExBarcode == data.ExBarcode
                && lastInsertedStocktaking.InBarcode == data.InBarcode
                && lastInsertedStocktaking.BrandCode == data.BrandCode
                && lastInsertedStocktaking.SKUMode == data.SKUMode
                && lastInsertedStocktaking.SendFlag == data.SendFlag
                && lastInsertedStocktaking.SerialNumber == data.SerialNumber
                && lastInsertedStocktaking.ConversionCounter == data.ConversionCounter)
            {
                decimal newQuantity = lastInsertedStocktaking.Quantity + data.Quantity;
                String query = "UPDATE tb_t_Stocktaking SET Quantity = {0} WHERE StocktakingID = {1}";
                SqlCeCommand cmdUpdate = new SqlCeCommand(string.Format(query, newQuantity, lastInsertedStocktaking.StocktakingID), cnStock);
                cmdUpdate.CommandType = CommandType.Text;
                cmdUpdate.ExecuteNonQuery();

                dt.Rows[dt.Rows.Count - 1][4] = newQuantity;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool QueryUpdateLastStocktakingData(string stocktakingid, string serialNumber, string conversion, int mode)
        {
            DataTable dt = null;

            if (mode == 5 || mode == 6 || mode == 7)
            {
                dt = dtStocktakingFreshFood;
            }
            else if (mode == 1 || mode == 2)
            {
                dt = dtStocktakingProduct;
            }
            else if (mode == 3 || mode == 4)
            {
                dt = dtStocktakingProductPack;
            }

            if (!string.IsNullOrEmpty(serialNumber))
            {
                serialNumber = "'" + serialNumber + "'";
            }
            else
            {
                serialNumber = "null";
            }

            if (!string.IsNullOrEmpty(conversion))
            {
                conversion = "'" + conversion + "'";
            }
            else
            {
                conversion = "null";
            }
            try
            {
                String query = "UPDATE tb_t_Stocktaking SET SerialNumber = {0} , ConversionCounter = {1} WHERE StocktakingID = '{2}'";
                SqlCeCommand cmdUpdate = new SqlCeCommand(string.Format(query, serialNumber, conversion, stocktakingid), cnStock);
                cmdUpdate.CommandType = CommandType.Text;
                cmdUpdate.ExecuteNonQuery();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            //}
            //else
            //{
                
           // }
        }

        public void QueryInsertFromScan(StockTakingModel data, int mode)
        {
            DataTable dt = null;
            StockTakingModel lastInsertedStocktaking = null;
            //if (mode == 1)
            //{
            //    dt = dtStocktakingFront;
            //    lastInsertedStocktaking = GetLastInsertedStocktaking();
            //}
            //else if (mode == 2)
            //{
            //    dt = dtStocktakingWarehouse;
            //    lastInsertedStocktaking = GetLastInsertedStocktaking();
            //}
            //else if (mode == 3)
            //{
            //    dt = dtStocktakingFreshFood;
            //}

            if (mode == 5 || mode == 6 || mode == 7)
            {
                dt = dtStocktakingFreshFood;
            }
            else if (mode == 1 || mode == 2)
            {
                dt = dtStocktakingProduct;
                lastInsertedStocktaking = GetLastInsertedStocktaking();
            }
            else if (mode == 3 || mode == 4)
            {
                dt = dtStocktakingProductPack;
                lastInsertedStocktaking = GetLastInsertedStocktaking();
            }
            CultureInfo defaulCulture = new CultureInfo("en-US");
            DateTime currentDT = DateTime.Now;

            DataRow newRow = dt.NewRow();
            newRow[0]  = string.IsNullOrEmpty(data.StocktakingID)       ? null : data.StocktakingID;
            newRow[1]  = data.ScanMode;
            newRow[2]  = string.IsNullOrEmpty(data.LocationCode)        ? null : data.LocationCode;
            newRow[3]  = string.IsNullOrEmpty(data.Barcode)             ? null : data.Barcode;
            newRow[4]  = data.Quantity;
            newRow[5]  = data.UnitCode;
            newRow[6]  = string.IsNullOrEmpty(data.Flag)                ? null : data.Flag;
            newRow[7]  = string.IsNullOrEmpty(data.Description)         ? null : data.Description;
            newRow[8]  = string.IsNullOrEmpty(data.SKUCode)             ? null : data.SKUCode;
            newRow[9]  = string.IsNullOrEmpty(data.ExBarcode)           ? null : data.ExBarcode;
            newRow[10] = string.IsNullOrEmpty(data.InBarcode)           ? null : data.InBarcode;
            newRow[11] = string.IsNullOrEmpty(data.BrandCode)           ? null : data.BrandCode;
            newRow[12] = data.SKUMode;
            newRow[13] = string.IsNullOrEmpty(data.DepartmentCode)      ? currentDepartmentCode : data.DepartmentCode;
            newRow[14] = data.SendFlag;
            newRow[15] = currentDT;
            newRow[16] = currentUser;
            newRow[17] = string.IsNullOrEmpty(data.SerialNumber)        ? null : data.SerialNumber;
            newRow[18] = string.IsNullOrEmpty(data.ConversionCounter)   ? null : data.ConversionCounter;

            dt.Rows.Add(newRow);
            if (dt.Rows.Count == 101)
            {
                dt.Rows.RemoveAt(0);
            }

            SqlCeCommand cmd = BuildStringInsertFormat(data, data.StocktakingID, currentDT);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            //DataRow tempRowTranDate2 = dtSetting.Select("KeyMap = 'TranDate'").First();
            DataRow tempRowTranCount2 = dtSetting.Select("KeyMap = 'TranCount'").First();
            //tempRowTranDate2[1] = currentDT.Date;
            tempRowTranCount2[1] = currentCount;

            //QueryUpdateSetting("TranDate", currentDT.Date.ToString());
            QueryUpdateSetting("TranCount", currentCount.ToString());

            currentCount++;
        }

        private SqlCeCommand BuildStringInsertFormat(StockTakingModel data, string uniqueID, DateTime currentDT)
        {
            string query = "INSERT INTO tb_t_Stocktaking (StocktakingID,ScanMode,LocationCode,Barcode,Quantity,UnitCode,Flag,Description,SKUCode,ExBarcode,InBarcode,BrandCode,SKUMode,DepartmentCode,SendFlag,CreateDate,CreateBy,SerialNumber,ConversionCounter) "
                            + "values(@UniqueID,@ScanMode,@LocationCode,@Barcode,@Quantity,@UnitCode,@Flag,@Description,@SKUCode,@ExBarcode,@InBarcode,@BrandCode,@SKUMode,@DepartmentCode,@SendFlag,@CreateDate,@CreateBy,@SerialNumber,@ConversionCounter)";

            SqlCeCommand cmd = new SqlCeCommand(query, cnStock);

            if (string.IsNullOrEmpty(uniqueID))
            {
                cmd.Parameters.AddWithValue("@UniqueID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UniqueID", uniqueID);
            }

            if (string.IsNullOrEmpty(data.ScanMode.ToString()))
            {
                cmd.Parameters.AddWithValue("@ScanMode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ScanMode", data.ScanMode);
            }
            if (string.IsNullOrEmpty(data.LocationCode))
            {
                cmd.Parameters.AddWithValue("@LocationCode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@LocationCode", data.LocationCode);
            }
            if (string.IsNullOrEmpty(data.Barcode))
            {
                cmd.Parameters.AddWithValue("@Barcode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Barcode", data.Barcode);
            }
            if (string.IsNullOrEmpty(data.Quantity.ToString()))
            {
                cmd.Parameters.AddWithValue("@Quantity", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Quantity", data.Quantity);
            }
            if (string.IsNullOrEmpty(data.UnitCode.ToString()))
            {
                cmd.Parameters.AddWithValue("@UnitCode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UnitCode", data.UnitCode);
            }
            if (string.IsNullOrEmpty(data.Flag))
            {
                cmd.Parameters.AddWithValue("@Flag", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Flag", data.Flag); 
            }            
            
            if (string.IsNullOrEmpty(data.Description))
            {
                cmd.Parameters.AddWithValue("@Description", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Description", data.Description);
            }
            if (string.IsNullOrEmpty(data.SKUCode))
            {
                cmd.Parameters.AddWithValue("@SKUCode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SKUCode", data.SKUCode);
            }
            if (string.IsNullOrEmpty(data.ExBarcode))
            {
                cmd.Parameters.AddWithValue("@ExBarcode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ExBarcode", data.ExBarcode);
            }
            if (string.IsNullOrEmpty(data.InBarcode))
            {
                cmd.Parameters.AddWithValue("@InBarcode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@InBarcode", data.InBarcode);
            }
            if (string.IsNullOrEmpty(data.BrandCode))
            {
                cmd.Parameters.AddWithValue("@BrandCode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BrandCode", data.BrandCode);
            }
            cmd.Parameters.AddWithValue("@SKUMode", data.SKUMode);
            cmd.Parameters.AddWithValue("@CreateDate", currentDT);
            cmd.Parameters.AddWithValue("@CreateBy", currentUser);

            if (string.IsNullOrEmpty(data.DepartmentCode))
            {
                cmd.Parameters.AddWithValue("@DepartmentCode", currentDepartmentCode);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DepartmentCode", data.DepartmentCode);
            }

            cmd.Parameters.AddWithValue("@SendFlag", data.SendFlag);

            if (string.IsNullOrEmpty(data.SerialNumber))
            {
                cmd.Parameters.AddWithValue("@SerialNumber", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SerialNumber", data.SerialNumber);
            }
            if (string.IsNullOrEmpty(data.ConversionCounter))
            {
                cmd.Parameters.AddWithValue("@ConversionCounter", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ConversionCounter", data.ConversionCounter);
            }
            return cmd;
        }

        public void QueryDeleteFromScan(string uniqueID, int mode)
        {
            DataTable dt = null;
            //if (mode == 1)
            //{
            //    dt = dtStocktakingFront;
            //}
            //else if (mode == 2)
            //{
            //    dt = dtStocktakingWarehouse;
            //}
            //else if (mode == 3)
            //{
            //    dt = dtStocktakingFreshFood;
            //}

            if (mode == 5 || mode == 6 || mode == 7)
            {
                dt = dtStocktakingFreshFood;
            }
            else if (mode == 1 || mode == 2)
            {
                dt = dtStocktakingProduct;
            }
            else if (mode == 3 || mode == 4)
            {
                dt = dtStocktakingProductPack;
            }

            DataRow row = dt.Select("StocktakingID = '" + uniqueID + "'").First();
            row.Delete();
            dt.AcceptChanges();

            string query = "DELETE FROM tb_t_Stocktaking WHERE StocktakingID = '{0}'";
            query = string.Format(query, uniqueID);
            SqlCeCommand cmd = new SqlCeCommand(query, cnStock);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        #endregion

        #region Send And Receive Data

        public DataTable QuerySelectAllFromSendData(string[] LocationCode, SendFTPMode mode)
        {
            if (mode == SendFTPMode.All)
            {
                string query = "SELECT StocktakingID,ScanMode,LocationCode,Barcode,Quantity,UnitCode,Flag,Description,SKUCode,ExBarcode,InBarcode,BrandCode,SKUMode,CreateDate,CreateBy,SerialNumber,ConversionCounter FROM tb_t_Stocktaking WHERE LocationCode = '{0}'";
                DataTable dt = new DataTable("tb_t_Stocktaking");
                foreach (string item in LocationCode)
                {
                    SqlCeCommand cmd = new SqlCeCommand(string.Format(query, item), cnStock);
                    cmd.CommandType = CommandType.Text;
                    SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);
                    da.Fill(dt);
                }

                return dt;
            }
            else
            {
                string query = "SELECT StocktakingID,ScanMode,LocationCode,Barcode,Quantity,UnitCode,Flag,Description,SKUCode,ExBarcode,InBarcode,BrandCode,SKUMode,CreateDate,CreateBy,SerialNumber,ConversionCounter FROM tb_t_Stocktaking WHERE LocationCode = '{0}' AND SendFlag = 0";
                DataTable dt = new DataTable("tb_t_Stocktaking");
                foreach (string item in LocationCode)
                {
                    SqlCeCommand cmd = new SqlCeCommand(string.Format(query, item), cnStock);
                    cmd.CommandType = CommandType.Text;
                    SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);
                    da.Fill(dt);
                }

                return dt;
            }
        }

        public string QuerySelectDistinctLocationCodeFromSendData(SendFTPMode mode)
        {
            string query = "";
            if (mode == SendFTPMode.All)
            {
                query = "SELECT DISTINCT LocationCode FROM tb_t_Stocktaking";
            }
            else
            {
                query = "SELECT DISTINCT LocationCode FROM tb_t_Stocktaking WHERE SendFlag = 0";
            }

            SqlCeCommand cmd = new SqlCeCommand(query, cnStock);
            cmd.CommandType = CommandType.Text;
            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);
            DataTable dt = new DataTable("tb_t_Stocktaking");
            da.Fill(dt);

            string returnString = "";
            foreach (DataRow row in dt.Rows)
            {
                returnString += row[0].ToString() + ",";
            }

            if (returnString.Length > 0)
            {
                returnString = returnString.Substring(0, returnString.Length - 1);
            }

            return returnString;
        }

        //Important
        public void QueryUpdateShowStatusFromSendData(bool newValue, string[] LocationCode)
        {
            int newVal;
            if (newValue)
            {
                newVal = 1;
            }
            else
            {
                newVal = 0;
            }

            string query = "UPDATE tb_t_Stocktaking SET SendFlag = {0} WHERE ";
            SqlCeCommand cmd = new SqlCeCommand();
            cmd.Connection = cnStock;
            cmd.CommandType = CommandType.Text;

            string additionQuery = "";
            for (int i = 0; i < LocationCode.Length; i++)
            {
                if (i == 0)
                {
                    additionQuery += "LocationCode = " + "'" + LocationCode[i] + "'";
                }
                else
                {
                    additionQuery += " OR LocationCode = " + "'" + LocationCode[i] + "'";
                }
            }
            query += additionQuery;

            cmd.CommandText = string.Format(query, newVal);
            cmd.ExecuteNonQuery();


            foreach (DataRow row in dtStocktakingFront.Select(additionQuery))
            {
                row[14] = newValue;
            }
            foreach (DataRow row in dtStocktakingWarehouse.Select(additionQuery))
            {
                row[14] = newValue;
            }
            foreach (DataRow row in dtStocktakingFreshFood.Select(additionQuery))
            {
                row[14] = newValue;
            }
        }

        #endregion

        #region Delete

        public List<string[]> QuerySelectAuditFromDeletion()
        {
            List<string[]> returnList = new List<string[]>();

            var groupLocationCount = listSummaryLocation.GroupBy(x => x.LocationCode).Select(y => new { LocationCode = y.Key, SumRecord = (int)y.Sum(z => z.TotalRecord) });

            foreach (var item in groupLocationCount)
            {
                string[] addItem = new string[2];
                addItem[0] = item.LocationCode;
                addItem[1] = item.SumRecord.ToString();
                returnList.Add(addItem);
            }

            return returnList;
        }

        public void QueryDeleteMasterFromDeletion(int whichOne)
        {
            switch (whichOne)
            {
                case 0:
                    listLocation.Clear();
                    SqlCeCommand cmd0 = new SqlCeCommand("DELETE FROM tb_m_Location WHERE 1=1", cnStock);
                    cmd0.CommandType = CommandType.Text;
                    cmd0.ExecuteNonQuery();
                    break;
                case 1:
                    listSKU.Clear();
                    listBarcode.Clear();
                    listPack.Clear();
                    SqlCeCommand cmd1;
                    cmd1 = new SqlCeCommand("DELETE FROM tb_m_SKU WHERE 1=1", cnStock);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.ExecuteNonQuery();
                    cmd1 = new SqlCeCommand("DELETE FROM tb_m_Barcode WHERE 1=1", cnStock);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.ExecuteNonQuery();
                    cmd1 = new SqlCeCommand("DELETE FROM tb_m_Pack WHERE 1=1", cnStock);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.ExecuteNonQuery();
                    cmd1 = new SqlCeCommand("DELETE FROM tb_m_SerialNumber WHERE 1=1", cnStock);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.ExecuteNonQuery();
                    break;
                default:
                    break;
            }
        }

        public void QueryDeleteAuditFromDeletion()
        {
            listSummaryLocation.Clear();
            dtStocktakingFreshFood.Clear();
            dtStocktakingFront.Clear();
            dtStocktakingWarehouse.Clear();

            DataRow tempRowTranCount2 = dtSetting.Select("KeyMap = 'TranCount'").First();
            tempRowTranCount2[1] = 0;
            QueryUpdateSetting("TranCount", "0");
            currentCount = 1;

            SqlCeCommand cmd = new SqlCeCommand("DELETE FROM tb_t_Stocktaking WHERE 1=1", cnStock);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        public void QueryDeleteRestoreFromDeletion()
        {
            QueryDeleteMasterFromDeletion(0);
            QueryDeleteMasterFromDeletion(1);
            QueryDeleteAuditFromDeletion();

            QueryUpdateSetting("TranDate", null);
            QueryUpdateSetting("TranCount", null);
            QueryUpdateSetting("Username", null);
            QueryUpdateSetting("DepartmentCode", null);
        }

        #endregion

        #region Setting

        public void QueryUpdateDateTime(DateTime newDateTime)
        {
            DataRow tempRowTranDate2 = dtSetting.Select("KeyMap = 'TranDate'").First();
            DataRow tempRowTranCount2 = dtSetting.Select("KeyMap = 'TranCount'").First();
            tempRowTranDate2[1] = newDateTime.Date;
            tempRowTranCount2[1] = QueryMaxRecordNumberOfDateTime(newDateTime);

            QueryUpdateSetting("TranDate", newDateTime.Date.ToString());
            QueryUpdateSetting("TranCount", tempRowTranCount2[1].ToString());

            currentCount = Int32.Parse(tempRowTranCount2[1].ToString()) + 1;
        }

        public List<string> QuerySelectComputerNameFromSetting()
        {
            List<string> returnList = new List<string>();
            foreach (DataRow row in dtComputer.Rows)
            {
                string addItem = row[0].ToString();
                returnList.Add(addItem);
            }
            return returnList;
        }

        public void QueryDeleteComputerNameFromSetting(string computerName)
        {
            for (int i = dtComputer.Rows.Count - 1; i >= 0; i--)
            {
                if (dtComputer.Rows[i][0].ToString().Equals(computerName))
                {
                    dtComputer.Rows.RemoveAt(i);
                    break;
                }
            }

            string query = "DELETE FROM Computer WHERE Computer_Name = '{0}'";
            query = string.Format(query, computerName);
            SqlCeCommand cmd = new SqlCeCommand(query, cnComputer);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        public void QueryInsertComputerNameFromSetting(string computerName)
        {
            DataRow newRow = dtComputer.NewRow();
            newRow[0] = computerName;
            dtComputer.Rows.Add(newRow);

            string query = "INSERT INTO Computer(Computer_Name) values('{0}')";
            query = string.Format(query, computerName);
            SqlCeCommand cmd = new SqlCeCommand(query, cnComputer);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        #endregion

        public int QuerySelectCountFromBackdoor(string table)
        {
            switch (table)
            {
                case "tb_m_Location": return listLocation.Count; break;
                case "tb_m_SKU": return listSKU.Count; break;
                default: return 0; break;
            }
        }
    }

    class mLocation
    {
        public string LocationCode { get; set; }
        public string SectionCode { get; set; }
        //public int ScanMode { get; set; }
        public string SectionName { get; set; }
        public string BrandCode { get; set; }
    }

    class mSKU
    {
        public string Description { get; set; }
        public string ExBarcode { get; set; }
        public string InBarcode { get; set; }
        public string BrandCode { get; set; }
        public string SKUCode { get; set; }
        public string MKCode { get; set; }
    }

    class mBarcode
    {
        public string ExBarcode { get; set; }
        public string SKUCode { get; set; }
    }

    class mPack
    {
        public string Barcode { get; set; }
        public string SKUCode { get; set; }
    }

    class SummaryLocation
    {
        public string LocationCode { get; set; }
        public int UnitCode { get; set; }
        public decimal TotalQuantity { get; set; }
        public int TotalRecord { get; set; }
    }

    class mSerial
    {
        public string SKUCode { get; set; }
        public string BarCode { get; set; }
        public string SerialNumber { get; set; }
        public string StorageLocation { get; set; }
    }
}