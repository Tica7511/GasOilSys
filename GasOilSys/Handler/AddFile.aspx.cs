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

public partial class Handler_AddFile : System.Web.UI.Page
{
    OilReportUpload_DB odb = new OilReportUpload_DB();
    GasReportUpload_DB gdb = new GasReportUpload_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增 檔案下載
        ///說    明:
        /// * Request["cpid"]: 業者Guid 
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

            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string xmlstr = string.Empty;

            #region 檢查資料庫是否有檔案
            switch (type)
            {
                case "gas":
                    gdb._業者guid = cpid;
                    DataTable gdt = gdb.GetList();
                    if (gdt.Rows.Count > 0)
                        throw new Exception("請先刪除報告再上傳");
                    break;
                case "oil":
                    odb._業者guid = cpid;
                    DataTable odt = odb.GetList();
                    if (odt.Rows.Count > 0)
                        throw new Exception("請先刪除報告再上傳");
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
                    switch (type)
                    {
                        case "gas":
                            UpLoadPath += "Gas_Upload\\report\\";
                            break;
                        case "oil":
                            UpLoadPath += "Oil_Upload\\report\\";
                            break;
                    }

                    //原檔名
                    string orgName = Path.GetFileName(File.FileName);

                    File.SaveAs(UpLoadPath + orgName);

                    switch (type)
                    {
                        case "gas":
                            gdb._guid = Guid.NewGuid().ToString("N");
                            gdb._年度 = "110";
                            gdb._檔案名稱 = orgName;
                            gdb._建立者 = LogInfo.mGuid;
                            gdb._修改者 = LogInfo.mGuid;

                            gdb.SaveFile(oConn, myTrans);
                            break;
                        case "oil":
                            odb._guid = Guid.NewGuid().ToString("N");
                            odb._年度 = "110";
                            odb._檔案名稱 = orgName;
                            odb._建立者 = LogInfo.mGuid;
                            odb._修改者 = LogInfo.mGuid;

                            odb.SaveFile(oConn, myTrans);
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