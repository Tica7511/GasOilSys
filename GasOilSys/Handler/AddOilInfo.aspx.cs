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
		/// * Request["year"]: 年度 
		/// * Request["ctel"]: 電話 
		/// * Request["caddr"]: 地址 
		/// * Request["storagetank"]: 儲槽數量 
		/// * Request["storagetankcapacity"]: 儲槽容量 
		/// * Request["pipeline"]: 管線數量 
		/// * Request["pipelinelength"]: 管線長度 
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
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string ctel = (string.IsNullOrEmpty(Request["ctel"])) ? "" : Request["ctel"].ToString().Trim();
            string caddr = (string.IsNullOrEmpty(Request["caddr"])) ? "" : Request["caddr"].ToString().Trim();
            string storagetank = (string.IsNullOrEmpty(Request["storagetank"])) ? "" : Request["storagetank"].ToString().Trim();
            string storagetankcapacity = (string.IsNullOrEmpty(Request["storagetankcapacity"])) ? "" : Request["storagetankcapacity"].ToString().Trim();
            string pipeline = (string.IsNullOrEmpty(Request["pipeline"])) ? "" : Request["pipeline"].ToString().Trim();
            string pipelinelength = (string.IsNullOrEmpty(Request["pipelinelength"])) ? "" : Request["pipelinelength"].ToString().Trim();
            string report = (string.IsNullOrEmpty(Request["report"])) ? "" : Request["report"].ToString().Trim();
            string checkdate = (string.IsNullOrEmpty(Request["checkdate"])) ? "" : Request["checkdate"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string txt2 = (string.IsNullOrEmpty(Request["txt2"])) ? "" : Request["txt2"].ToString().Trim();
            string txt3 = (string.IsNullOrEmpty(Request["txt3"])) ? "" : Request["txt3"].ToString().Trim();
            string txt4 = (string.IsNullOrEmpty(Request["txt4"])) ? "" : Request["txt4"].ToString().Trim();

            db._guid = cid;
            db._年度 = year;
            db._電話 = Server.UrlDecode(ctel);
            db._地址 = Server.UrlDecode(caddr);
            db._儲槽數量 = Server.UrlDecode(storagetank);
            db._儲槽容量 = Server.UrlDecode(storagetankcapacity);
            db._管線數量 = Server.UrlDecode(pipeline);
            db._管線長度 = Server.UrlDecode(pipelinelength);
            db._維運計畫書及成果報告 = Server.UrlDecode(report);
            db._曾執行過查核日期 = Server.UrlDecode(checkdate);
            db._年度查核姓名 = Server.UrlDecode(txt1);
            db._年度查核職稱 = Server.UrlDecode(txt2);
            db._年度查核分機 = Server.UrlDecode(txt3);
            db._年度查核email = Server.UrlDecode(txt4);
            db._修改者 = LogInfo.mGuid;

            DataTable dt = db.GetInfoDetail2();

            if (dt.Rows.Count > 0)
            {
                db.UpdateCompanyInfo(oConn, myTrans);
            }
            else
            {
                db._建立者 = LogInfo.mGuid;
                db._建立日期 = DateTime.Now;

                db.InsertCompanyInfo(oConn, myTrans);
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
}