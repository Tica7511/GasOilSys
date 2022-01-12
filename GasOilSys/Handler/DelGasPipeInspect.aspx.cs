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

public partial class Handler_DelGasPipeInspect : System.Web.UI.Page
{
    GasInsideInspect_DB db = new GasInsideInspect_DB();
    FileTable fdb = new FileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除天然氣管線內部稽核檔案
		///說    明:
		/// * Request["guid"]: guid 
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
            string xmlstr = string.Empty;
            string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];

            db._guid = guid;
            db._修改者 = LogInfo.mGuid;
            fdb._guid = guid;
            fdb._修改者 = LogInfo.mGuid;

            DataTable dt = db.GetData();
            if (dt.Rows.Count > 0)
            {
                FileInfo fi = new FileInfo(UpLoadPath + "Gas_Upload\\pipeinspect\\" + dt.Rows[0]["佐證資料檔名"].ToString().Trim() + dt.Rows[0]["佐證資料副檔名"].ToString().Trim());
                if (fi.Exists)
                {
                    fi.Delete(); //刪除主機路徑內的檔案
                }

                db.DelFile(oConn, myTrans); //更新資料庫此資料列
                fdb.DelFile(oConn, myTrans); //更新附件檔此資料列為'D'
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