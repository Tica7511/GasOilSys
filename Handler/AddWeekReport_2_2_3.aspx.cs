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

public partial class Handler_AddWeekReport_2_2_3 : System.Web.UI.Page
{
    WeekReport_2_2_3_DB db = new WeekReport_2_2_3_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 新增/修改 計劃書2.2.3
		///說    明:
		/// * Request["mode"]: 新增/修改
		/// * Request["rid"]: guid 
		/// * Request["rpid"]: 父層 guid 
		/// * Request["no"]: 工作項次 
		/// * Request["content"]: 執行內容 
		/// * Request["begintime"]: 預定日期(起)  
		/// * Request["endtime"]: 預定日期(迄) 
		/// * Request["other"]: 備註 
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

            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string rid = (string.IsNullOrEmpty(Request["rid"])) ? Guid.NewGuid().ToString("D") : Request["rid"].ToString().Trim();
            string rpid = (string.IsNullOrEmpty(Request["rpid"])) ? "" : Request["rpid"].ToString().Trim();
            string no = (string.IsNullOrEmpty(Request["no"])) ? "" : Request["no"].ToString().Trim();
            string content = (string.IsNullOrEmpty(Request["content"])) ? "" : Request["content"].ToString().Trim();
            string begintime = (string.IsNullOrEmpty(Request["begintime"])) ? "" : Request["begintime"].ToString().Trim();
            string endtime = (string.IsNullOrEmpty(Request["endtime"])) ? "" : Request["endtime"].ToString().Trim();
            string other = (string.IsNullOrEmpty(Request["other"])) ? "" : Request["other"].ToString().Trim();
            string sn = string.Empty;
            string season = string.Empty;

            //取得年月日(起)
            string year = begintime.Substring(0, 3);
            string month = begintime.Substring(3, 2);
            string date = begintime.Substring(5, 2);

            //取得年月日(迄)
            string year2 = endtime.Substring(0, 3);
            string month2 = endtime.Substring(3, 2);
            string date2 = endtime.Substring(5, 2);

            //取得季度
            if ((month == "01") || (month == "02") || (month == "03"))
                season = "1";
            else if ((month == "04") || (month == "05") || (month == "06"))
                season = "2";
            else if ((month == "07") || (month == "08") || (month == "09"))
                season = "3";
            else
                season = "4";

            db._guid = rid;
            db._父層guid = rpid;
            db._工作項次 = no;
            db._執行內容 = content;
            db._預定日期起 = year + month + date;
            db._預定日期迄 = year2 + month2 + date2;
            db._備註 = other;
            db._季度 = season;
            db._年度 = year;
            db._建立者 = LogInfo.mGuid;
            db._修改者 = LogInfo.mGuid;

            //儲存
            db.SaveReport(oConn, myTrans, mode);

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