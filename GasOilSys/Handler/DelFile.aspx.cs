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

public partial class Handler_DelFile : System.Web.UI.Page
{
    FileTable db = new FileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除檔案
		///說    明:
		/// * Request["guid"]: Guid 
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

            string fguid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Request["category"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string details = (string.IsNullOrEmpty(Request["details"])) ? "" : Request["details"].ToString().Trim();
            string xmlstr = string.Empty;
            string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
            string NewName = string.Empty;

            db._guid = fguid;
            db._修改者 = LogInfo.mGuid;

            DataTable dt = db.GetFileData();
            if (dt.Rows.Count > 0)
            {
                switch (category)
                {
                    case "Oil":
                        switch (type)
                        {
                            //庫區基本資料
                            case "01":
                                UpLoadPath += "Oil_Upload\\reservoir\\" + dt.Rows[0]["原檔名"].ToString().Trim() + dt.Rows[0]["附檔名"].ToString().Trim();
                                break;
                        }
                        break;
                    case "Gas":
                        break;
                    case "PublicGas":
                        UpLoadPath += "PublicGas\\";
                        switch (type)
                        {
                            case "Info":
                                switch (details)
                                {
                                    case "14":
                                        UpLoadPath += "info\\checkreport\\";
                                        break;
                                    case "15":
                                        UpLoadPath += "info\\report\\";
                                        break;
                                    case "16":
                                        UpLoadPath += "info\\resultreport\\";
                                        break;
                                }

                                db._guid = fguid;
                                db._檔案類型 = details;
                                dt = db.GetFileData();
                                if (dt.Rows.Count > 0)
                                {
                                    NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                }

                                UpLoadPath += NewName;
                                break;
                        }
                        break;
                }
                FileInfo fi = new FileInfo(UpLoadPath);
                if (fi.Exists)
                {
                    fi.Delete(); //刪除主機路徑內的檔案
                }

                db.DelFile(oConn, myTrans); //更新資料庫此資料列
            }
            else
            {
                throw new Exception("檔案不存在");
            }

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>刪除完成</Response></root>";
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