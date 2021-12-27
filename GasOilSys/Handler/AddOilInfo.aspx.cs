using System;
using System.Globalization;
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

public partial class Handler_AddOilInfo : System.Web.UI.Page
{
    OilCompanyInfo_DB db = new OilCompanyInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 修改 石油業者基本資料
		///說    明:
		/// * Request["cid"]: guid 
		/// * Request["ctel"]: 電話 
		/// * Request["caddr"]: 地址 
		/// * Request["storagetank"]: 儲槽數量 
		/// * Request["pipeline"]: 管線數量 
		/// * Request["report"]: 維運計畫書及成果報告 
		/// * Request["checkdate"]: 曾執行過查核日期 
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

            string cid = (string.IsNullOrEmpty(Request["cid"])) ? "" : Request["cid"].ToString().Trim();
            string ctel = (string.IsNullOrEmpty(Request["ctel"])) ? "" : Request["ctel"].ToString().Trim();
            string caddr = (string.IsNullOrEmpty(Request["caddr"])) ? "" : Request["caddr"].ToString().Trim();
            string storagetank = (string.IsNullOrEmpty(Request["storagetank"])) ? "" : Request["storagetank"].ToString().Trim();
            string pipeline = (string.IsNullOrEmpty(Request["pipeline"])) ? "" : Request["pipeline"].ToString().Trim();
            string report = (string.IsNullOrEmpty(Request["report"])) ? "" : Request["report"].ToString().Trim();
            string checkdate = (string.IsNullOrEmpty(Request["checkdate"])) ? "" : Request["checkdate"].ToString().Trim();

            db._guid = cid;
            db._電話 = ctel;
            db._地址 = caddr;
            db._儲槽數量 = storagetank;
            db._管線數量 = pipeline;
            db._維運計畫書及成果報告 = report;
            db._曾執行過查核日期 = checkdate;
            db._修改者 = LogInfo.mGuid;

            db.UpdateCompanyInfo(oConn, myTrans);

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
}