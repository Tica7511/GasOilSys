using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;

public partial class Handler_AddDownload : System.Web.UI.Page
{
    OilReportUpload_DB ordb = new OilReportUpload_DB();
    GasReportUpload_DB grdb = new GasReportUpload_DB();
    OilInsideInspect_DB oidb = new OilInsideInspect_DB();
    GasInsideInspect_DB gidb = new GasInsideInspect_DB();
    OilInternalAudit_DB oiadb = new OilInternalAudit_DB();
    GasOnlineEvaluation_DB goedb = new GasOnlineEvaluation_DB();
    OilOnlineEvaluation_DB ooedb = new OilOnlineEvaluation_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增 檔案下載
        ///說    明:
        /// * Request["guid"]: guid 
        /// * Request["cpid"]: 業者Guid 
        /// * Request["category"]: 網頁類別 gas/oil 
        /// * Request["type"]: 檔案類型  
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

            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Request["category"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string details = (string.IsNullOrEmpty(Request["details"])) ? "" : Request["details"].ToString().Trim();
            string xmlstr = string.Empty;
            DataTable dt = new DataTable();

            #region 檢查資料庫是否有檔案
            switch (category)
            {
                case "gas":
                    switch (type)
                    {
                        case "report":
                            grdb._業者guid = cpid;
                            dt = grdb.GetList();
                            if (dt.Rows.Count > 0)
                                throw new Exception("請先刪除報告再上傳");
                            break;
                        case "pipeinspect":
                            gidb._guid = guid;
                            dt = gidb.GetData();
                            if (dt.Rows.Count > 0)
                                if (!string.IsNullOrEmpty(dt.Rows[0]["佐證資料檔名"].ToString().Trim()))
                                    throw new Exception("請先刪除佐證檔案再上傳");
                            break;
                    }                    
                    break;
                case "oil":
                    switch (type)
                    {
                        case "report":
                            ordb._業者guid = cpid;
                            dt = ordb.GetList();
                            if (dt.Rows.Count > 0)
                                throw new Exception("請先刪除簡報再上傳");
                            break;
                        case "pipeinspect":
                            oidb._guid = guid;
                            dt = oidb.GetData();
                            if (dt.Rows.Count > 0)
                                if(!string.IsNullOrEmpty(dt.Rows[0]["佐證資料檔名"].ToString().Trim()))
                                    throw new Exception("請先刪除佐證檔案再上傳");
                            break;
                        case "storageinspect":
                            oiadb._guid = guid;
                            dt = oiadb.GetData();
                            if (dt.Rows.Count > 0)
                                if (!string.IsNullOrEmpty(dt.Rows[0]["佐證資料檔名"].ToString().Trim()))
                                    throw new Exception("請先刪除佐證檔案再上傳");
                            break;
                    }                    
                    break;
            }
            #endregion

            // 檔案上傳
            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
                    switch (category)
                    {
                        case "gas":
                            switch (type)
                            {
                                case "report":
                                    UpLoadPath += "Gas_Upload\\report\\";
                                    break;
                                case "pipeinspect":
                                    UpLoadPath += "Gas_Upload\\pipeinspect\\";
                                    break;
                                case "online":
                                    switch (details)
                                    {
                                        case "1":
                                            UpLoadPath += "Gas_Upload\\online\\pipeline\\";
                                            break;
                                        case "2":
                                            UpLoadPath += "Gas_Upload\\online\\storage\\";
                                            break;
                                        case "3":
                                            UpLoadPath += "Gas_Upload\\online\\disaster\\";
                                            break;
                                        case "4":
                                            UpLoadPath += "Gas_Upload\\online\\installation\\";
                                            break;
                                        case "5":
                                            UpLoadPath += "Gas_Upload\\online\\law\\";
                                            break;
                                    }
                                    break;
                            }                                                      
                            break;
                        case "oil":
                            switch (type)
                            {
                                case "report":
                                    UpLoadPath += "Oil_Upload\\report\\";
                                    break;
                                case "pipeinspect":
                                    UpLoadPath += "Oil_Upload\\pipeinspect\\";
                                    break;
                                case "storageinspect":
                                    UpLoadPath += "Oil_Upload\\storageinspect\\";
                                    break;
                                case "online":
                                    switch (details)
                                    {
                                        case "1":
                                            UpLoadPath += "Oil_Upload\\online\\pipeline\\";
                                            break;
                                        case "2":
                                            UpLoadPath += "Oil_Upload\\online\\storage\\";
                                            break;
                                        case "3":
                                            UpLoadPath += "Oil_Upload\\online\\disaster\\";
                                            break;
                                        case "4":
                                            UpLoadPath += "Oil_Upload\\online\\installation\\";
                                            break;
                                        case "5":
                                            UpLoadPath += "Oil_Upload\\online\\law\\";
                                            break;
                                    }
                                    break;
                            }                           
                            break;
                    }

                    //原檔名
                    string orgName = Path.GetFileNameWithoutExtension(File.FileName);

                    //副檔名
                    string extension = System.IO.Path.GetExtension(File.FileName).ToLower();

                    //新檔名
                    string newName = orgName + extension;

                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }

                    File.SaveAs(UpLoadPath + newName);                    

                    switch (category)
                    {
                        case "gas":
                            switch (type)
                            {
                                case "report":
                                    grdb._guid = Guid.NewGuid().ToString("N");
                                    grdb._年度 = "110";
                                    grdb._檔案名稱 = newName;
                                    grdb._建立者 = LogInfo.mGuid;
                                    grdb._修改者 = LogInfo.mGuid;

                                    grdb.SaveFile(oConn, myTrans);
                                    break;
                                case "pipeinspect":
                                    gidb._guid = guid;
                                    gidb._佐證資料檔名 = orgName;
                                    gidb._佐證資料副檔名 = extension;
                                    gidb._佐證資料路徑 = UpLoadPath;
                                    gidb._建立者 = LogInfo.mGuid;
                                    gidb._修改者 = LogInfo.mGuid;

                                    gidb.SaveFile(oConn, myTrans);
                                    break;
                                case "online":
                                    goedb._guid = Guid.NewGuid().ToString("N");
                                    goedb._業者guid = cpid;
                                    goedb._年度 = "110";
                                    goedb._檔案類型 = details;
                                    goedb._檔案名稱 = newName;
                                    goedb._建立者 = LogInfo.mGuid;
                                    goedb._修改者 = LogInfo.mGuid;

                                    goedb.SaveFile(oConn, myTrans);
                                    break;
                            }                            
                            break;
                        case "oil":
                            switch (type)
                            {
                                case "report":
                                    ordb._guid = Guid.NewGuid().ToString("N");
                                    ordb._年度 = "110";
                                    ordb._檔案名稱 = newName;
                                    ordb._建立者 = LogInfo.mGuid;
                                    ordb._修改者 = LogInfo.mGuid;

                                    ordb.SaveFile(oConn, myTrans);
                                    break;
                                case "pipeinspect":
                                    oidb._guid = guid;
                                    oidb._佐證資料檔名 = orgName;
                                    oidb._佐證資料副檔名 = extension;
                                    oidb._佐證資料路徑 = UpLoadPath;
                                    oidb._建立者 = LogInfo.mGuid;
                                    oidb._修改者 = LogInfo.mGuid;

                                    oidb.SaveFile(oConn, myTrans);
                                    break;
                                case "storageinspect":
                                    oiadb._guid = guid;
                                    oiadb._佐證資料檔名 = orgName;
                                    oiadb._佐證資料副檔名 = extension;
                                    oiadb._佐證資料路徑 = UpLoadPath;
                                    oiadb._建立者 = LogInfo.mGuid;
                                    oiadb._修改者 = LogInfo.mGuid;

                                    oiadb.SaveFile(oConn, myTrans);
                                    break;
                                case "online":
                                    ooedb._guid = Guid.NewGuid().ToString("N");
                                    ooedb._業者guid = cpid;
                                    ooedb._年度 = "110";
                                    ooedb._檔案類型 = details;
                                    ooedb._檔案名稱 = newName;
                                    ooedb._建立者 = LogInfo.mGuid;
                                    ooedb._修改者 = LogInfo.mGuid;

                                    ooedb.SaveFile(oConn, myTrans);
                                    break;
                            }                            
                            break;
                    }
                }
            }

            myTrans.Commit();

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
}