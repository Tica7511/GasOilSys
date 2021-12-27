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

public partial class Handler_AddWeekReport_6_1 : System.Web.UI.Page
{
    WeekReport_6_1_DB db = new WeekReport_6_1_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 新增/修改 計劃書6.1
		///說    明:
		/// * Request["mode"]: 新增/修改
		/// * Request["rid"]: guid 
		/// * Request["rpid"]: 父層 guid 
		/// * Request["no"]: 工作項次 
		/// * Request["nb1"]: 編號1 
		/// * Request["nb2"]: 編號2 
		/// * Request["nb3"]: 編號3 
		/// * Request["nb4"]: 編號4 
		/// * Request["content"]: 執行內容 
		/// * Request["begintime"]: 預定日期(起)  
		/// * Request["endtime"]: 預定日期(迄) 
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
            string nb1 = (string.IsNullOrEmpty(Request["nb1"])) ? "" : Request["nb1"].ToString().Trim();
            string nb2 = (string.IsNullOrEmpty(Request["nb2"])) ? "" : Request["nb2"].ToString().Trim();
            string nb3 = (string.IsNullOrEmpty(Request["nb3"])) ? "" : Request["nb3"].ToString().Trim();
            string nb4 = (string.IsNullOrEmpty(Request["nb4"])) ? "" : Request["nb4"].ToString().Trim();
            string content = (string.IsNullOrEmpty(Request["content"])) ? "" : Request["content"].ToString().Trim();
            string begintime = (string.IsNullOrEmpty(Request["begintime"])) ? "" : Request["begintime"].ToString().Trim();
            string endtime = (string.IsNullOrEmpty(Request["endtime"])) ? "" : Request["endtime"].ToString().Trim();
            string sn = string.Empty;

            db._guid = rid;
            db._父層guid = rpid;
            db._工作項次 = no;
            db._編號1 = nb1;
            db._編號2 = nb2;
            db._編號3 = nb3;
            db._編號4 = nb4;
            db._執行內容 = content;
            db._預定日期起 = getyear(begintime) + getmonth(begintime) + getdate(begintime);
            db._預定日期迄 = (string.IsNullOrEmpty(endtime)) ? "" : getyear(endtime) + getmonth(endtime) + getdate(endtime);
            db._季度 = getSeason(getmonth(begintime));
            db._年度 = getyear(begintime);
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

    public string getyear(string time)
    {
        string year = time.Substring(0, 3);

        return year;
    }

    public string getmonth(string time)
    {
        string month = time.Substring(3, 2);

        return month;
    }

    public string getdate(string time)
    {
        string date = time.Substring(5, 2);

        return date;
    }

    public string getSeason(string month)
    {
        string season = string.Empty;
        if ((month == "01") || (month == "02") || (month == "03"))
            season = "1";
        else if ((month == "04") || (month == "05") || (month == "06"))
            season = "2";
        else if ((month == "07") || (month == "08") || (month == "09"))
            season = "3";
        else
            season = "4";

        return season;
    }
}