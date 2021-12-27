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

public partial class Handler_DelGasReportUpload : System.Web.UI.Page
{
    GasReportUpload_DB db = new GasReportUpload_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除查核簡報上傳
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

            string rid = (string.IsNullOrEmpty(Request["rid"])) ? "" : Request["rid"].ToString().Trim();
            string xmlstr = string.Empty;
            string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];

            db._guid = rid;

            DataTable dt = db.GetData();
            if(dt.Rows.Count > 0)
            {
                FileInfo fi = new FileInfo(UpLoadPath + "Gas_Upload\\report\\" + dt.Rows[0]["檔案名稱"].ToString().Trim());
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