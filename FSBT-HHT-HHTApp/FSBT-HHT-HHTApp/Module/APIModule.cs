using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace Denso_HHT.Module
{
    class APIModule
    {
        private HttpWebRequest req = null;
        private HttpWebResponse res = null;
        private Encoding encoding = null;
        private StreamWriter writer = null;
        private StreamReader reader = null;

        private Loading loading;

        private static APIModule instance;

        public static APIModule Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new APIModule();
                }
                return instance;
            }
        }


        private APIModule()
        {
            encoding = new UTF8Encoding();
        }

        public void Init()
        {

        }

        public void SendRequestThread(StockTakingModel item, int mode)
        {
            loading = new Loading();

            Thread t = new Thread(delegate()
            {
                if (DatabaseModule.Instance.QuerySelectHaveSendFlagData())
                {

                    List<StockTakingModel> stockList = new List<StockTakingModel>();
                    stockList.Add(item);
                    stockList.AddRange(DatabaseModule.Instance.QuerySelectAllStocktakingSendFlagData());
                    SendRequestList(stockList);
                    loading.DisposeLoading();
                }
                else
                {
                    SendRequest(item, mode);
                    loading.DisposeLoading();
                }
            });
            t.IsBackground = true;
            t.Start();
            loading.ShowDialog();
        }

        public void SendRequestThread(StockTakingModel item, decimal newQuantity, int newUnitCode, int mode)
        {
            loading = new Loading();

            Thread t = new Thread(delegate()
            {
                item.SendFlag = true;
                SendRequest(item, newQuantity, newUnitCode, mode);
                loading.DisposeLoading();
            });
            t.IsBackground = true;
            t.Start();
            loading.ShowDialog();
        }

        private bool SendRequest(StockTakingModel item, int mode)
        {
            bool returnValue = true;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(@"http://" + DatabaseModule.Instance.FtpServer + ":15267/api/stocktakings/InsertStocktaking");
                req.ContentType = "application/json";
                req.Method = "POST";
                req.AllowWriteStreamBuffering = true;
                req.Proxy = null;

                Stocktaking s = new Stocktaking
                {
                    StocktakingID = item.StocktakingID,
                    ScanMode = item.ScanMode,
                    Barcode = item.Barcode,
                    LocationCode = item.LocationCode,
                    Quantity = item.Quantity,
                    UnitCode = item.UnitCode,
                    Flag = item.Flag,
                    Description = item.Description,
                    SKUCode = item.SKUCode,
                    ExBarcode = item.ExBarcode,
                    InBarcode = item.InBarcode,
                    SKUMode = item.SKUMode,
                    HHTName = DatabaseModule.Instance.HHTName,
                    HHTID = DatabaseModule.Instance.HHTID,
                    DepartmentCode = string.IsNullOrEmpty(item.DepartmentCode) ? DatabaseModule.Instance.currentDepartmentCode : item.DepartmentCode,
                    CreateBy = DatabaseModule.Instance.currentUser,
                    UpdateBy = DatabaseModule.Instance.currentUser,
                    SerialNumber = item.SerialNumber,
                    ConversionCounter = item.ConversionCounter
                };
                string pJson = JsonConvert.SerializeObject(s);
                //req.ContentLength = encoding.GetBytes(pJson).Length;
                writer = new StreamWriter(req.GetRequestStream());
                writer.Write(pJson);
                writer.Flush();
                writer.Close();

                res = (HttpWebResponse)req.GetResponse();
                reader = new StreamReader(res.GetResponseStream());
                string responseMessage = reader.ReadToEnd();
                reader.Close();

                if ("F".Equals(responseMessage))
                {
                    throw new APIException("Server cant commit transfered-data to database, please check the server-side.");
                }
                item.SendFlag = true;
            }
            catch (APIException ex)
            {
                item.SendFlag = false;
                loading.ShowMessageLoading(ex.Message);
                returnValue = false;

            }
            catch (Exception ex)
            {
                item.SendFlag = false;
                loading.ShowMessageLoading("The process cant transfer realtime data to server.");
                returnValue = false;

            }

            DatabaseModule.Instance.QueryInsertFromScan(item, mode);
            return returnValue;
        }

        private bool SendRequest(StockTakingModel item, decimal newQuantity, int newUnitCode, int mode)
        {
            bool returnValue = true;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(@"http://" + DatabaseModule.Instance.FtpServer + ":15267/api/stocktakings/UpdateStocktaking");
                req.ContentType = "application/json";
                req.Method = "POST";
                req.AllowWriteStreamBuffering = true;
                req.Proxy = null;

                Stocktaking s = new Stocktaking
                {
                    StocktakingID = item.StocktakingID,
                    ScanMode = item.ScanMode,
                    Barcode = item.Barcode,
                    LocationCode = item.LocationCode,
                    Quantity = newQuantity,
                    UnitCode = newUnitCode,
                    Flag = item.Flag,
                    Description = item.Description,
                    SKUCode = item.SKUCode,
                    ExBarcode = item.ExBarcode,
                    InBarcode = item.InBarcode,
                    SKUMode = item.SKUMode,
                    HHTName = DatabaseModule.Instance.HHTName,
                    HHTID = DatabaseModule.Instance.HHTID,
                    DepartmentCode = string.IsNullOrEmpty(item.DepartmentCode) ? DatabaseModule.Instance.currentDepartmentCode : item.DepartmentCode,
                    CreateBy = DatabaseModule.Instance.currentUser,
                    UpdateBy = DatabaseModule.Instance.currentUser,
                    SerialNumber = item.SerialNumber,
                    ConversionCounter = item.ConversionCounter
                };
                string pJson = JsonConvert.SerializeObject(s);
                //req.ContentLength = encoding.GetBytes(pJson).Length;
                writer = new StreamWriter(req.GetRequestStream());
                writer.Write(pJson);
                writer.Flush();
                writer.Close();

                res = (HttpWebResponse)req.GetResponse();
                reader = new StreamReader(res.GetResponseStream());
                string responseMessage = reader.ReadToEnd();
                reader.Close();

                if ("F".Equals(responseMessage))
                {
                    throw new APIException("Server cant commit transfered-data to database, please check the server-side.");
                }

                item.SendFlag = true;
            }
            catch (APIException ex)
            {
                item.SendFlag = false;
                loading.ShowMessageLoading(ex.Message);
                returnValue = false;
            }
            catch (Exception ex)
            {
                item.SendFlag = false;
                loading.ShowMessageLoading("The process cant transfer realtime data to server.");
                returnValue = false;
            }

            DatabaseModule.Instance.QueryUpdateFromScan(item.StocktakingID, newQuantity, newUnitCode, item.SendFlag, mode);
            return returnValue;
        }

        private void SendRequestList(List<StockTakingModel> stockList)
        {
            loading.ChangeMessageLabelLoading("0/" + stockList.Count);
            int i = 1;
            foreach (StockTakingModel stock in stockList)
            {
                int tempMode = 0;
                switch (stock.ScanMode)
                {
                    case 1:
                        tempMode = 1;
                        break;
                    case 2:
                        tempMode = 1;
                        break;
                    case 3:
                        tempMode = 2;
                        break;
                    case 4:
                        tempMode = 3;
                        break;
                }

                if (i == 1)
                {
                    if (!SendRequest(stock, tempMode))
                    {
                        break;
                    }
                }
                else
                {
                    if (!SendRequest(stock, stock.Quantity, stock.UnitCode, tempMode))
                    {
                        break;
                    }
                }
                loading.ChangeMessageLabelLoading(i + "/" + stockList.Count);
                i++;
            }
        }
    }

    class APIException : Exception
    {
        public APIException()
        {
        }

        public APIException(string message)
            : base(message)
        {
        }

        public APIException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
