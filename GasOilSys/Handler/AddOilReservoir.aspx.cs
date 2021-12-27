using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Globalization;

public partial class Handler_AddOilReservoir : System.Web.UI.Page
{
    OilReservoir_DB db = new OilReservoir_DB();
    FileTable fdb = new FileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 儲存庫區基本資料
        ///說    明:
        ///-----------------------------------------------------

        XmlDocument xDoc = new XmlDocument();

        /// Transaction
        SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        oConn.Open();
        SqlCommand oCmmd = new SqlCommand();
        oCmmd.Connection = oConn;
        SqlTransaction myTrans = oConn.BeginTransaction();
        oCmmd.Transaction = myTrans;
        try
        {
            #region 檢查登入資訊
            if (LogInfo.mGuid == "")
            {
                throw new Exception("請重新登入");
            }
            #endregion

            string cguid = string.IsNullOrEmpty(Request["cguid"]) ? "" : Request["cguid"].ToString().Trim();
            string mode = string.IsNullOrEmpty(Request["mode"]) ? "" : Request["mode"].ToString().Trim();
            string checkAreaOther = string.IsNullOrEmpty(Request["checkAreaOther"]) ? "" : Request["checkAreaOther"].ToString().Trim();
            string tmpGuid = Guid.NewGuid().ToString("N");

            db._業者guid = cguid;
            db._庫區特殊區域 = string.IsNullOrEmpty(Request["checkArea"]) ? "" : Request["checkArea"].ToString().Trim();
            db._庫區特殊區域_其他 = checkAreaOther;
            db._年度 = taiwanYear(DateTime.Now);
            db._建立者 = LogInfo.mGuid;
            db._修改者 = LogInfo.mGuid;

            if (mode == "new")
                db._guid = tmpGuid;

            db.InsertData(oConn, myTrans);

            // 檔案上傳
            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Oil_Upload\\reservoir\\";

                    //副檔名
                    string extension = Path.GetExtension(File.FileName);
                    //原檔名
                    string orgName = Path.GetFileName(File.FileName).Replace(extension, "");
                    //檔案大小
                    string file_size = File.ContentLength.ToString();
                    //if ((float.Parse(file_size) / 1024) > 2048)
                    //{
                    //    throw new Exception("檔案大小限制 2MB");
                    //}
                    //取得TIME與GUID
                    string timeguid = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N");
                    //儲存的名稱
                    //string newName = timeguid.Replace("..", "").Replace("\\", "") + extension.Replace("..", "").Replace("\\", "");
                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }

                    if (extension.ToLower() == ".jpeg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".gif" || extension.ToLower() == ".svg")
                    {
                        File.SaveAs(UpLoadPath + orgName + extension);
                        //進資料庫前, 儲存名稱要去除副檔名
                        //newName = newName.Replace(extension, "");

                        fdb._guid = Guid.NewGuid().ToString("N");
                        fdb._業者guid = cguid;
                        fdb._檔案類型 = "01";
                        fdb._年度 = taiwanYear(DateTime.Now);
                        fdb._原檔名 = orgName;
                        fdb._新檔名 = orgName;
                        fdb._附檔名 = extension;
                        fdb._檔案大小 = file_size;
                        fdb._建立者 = LogInfo.mGuid;
                        fdb._修改者 = LogInfo.mGuid;

                        fdb.UpdateFile_Trans(oConn, myTrans);
                    }
                    else
                    {
                        throw new Exception("圖片格式限制: .jpeg .png .jpg .gif .svg");
                    }
                }
            }

            myTrans.Commit();

            string xmlstr = string.Empty;
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response></root>";
            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            myTrans.Rollback();
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        finally
        {
            oCmmd.Connection.Close();
            oConn.Close();
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }

    public string taiwanYear(DateTime date)
    {
        TaiwanCalendar taiwanDate = new TaiwanCalendar();
        string taiwanYear = taiwanDate.GetYear(date).ToString();

        return taiwanYear;
    }
}