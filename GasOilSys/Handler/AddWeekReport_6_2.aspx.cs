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

public partial class Handler_AddWeekReport_6_2 : System.Web.UI.Page
{
    WeekReport_6_2_DB db = new WeekReport_6_2_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 新增/修改 計劃書6.2
		///說    明:
		/// * Request["type"]: 資料表排序
		/// * Request["mode"]: 新增/修改
		/// * Request["rid"]: guid 
		/// * Request["rpid"]: 父層 guid 
		/// * Request["no"]: 工作項次 
		/// * Request["content"]: 執行內容 
		/// * Request["time"]: 預定日期  
		/// * Request["place"]: 地點 
        /// * Request["mancount"]: 人數 
        /// * Request["filename"]: 文件名稱 
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

            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string rid = (string.IsNullOrEmpty(Request["rid"])) ? Guid.NewGuid().ToString("D") : Request["rid"].ToString().Trim();
            string rpid = (string.IsNullOrEmpty(Request["rpid"])) ? "" : Request["rpid"].ToString().Trim();
            string no = (string.IsNullOrEmpty(Request["no"])) ? "" : Request["no"].ToString().Trim();
            string content = (string.IsNullOrEmpty(Request["content"])) ? "" : Request["content"].ToString().Trim();
            string time = (string.IsNullOrEmpty(Request["time"])) ? "" : Request["time"].ToString().Trim();
            string place = (string.IsNullOrEmpty(Request["place"])) ? "" : Request["place"].ToString().Trim();
            string mancount = (string.IsNullOrEmpty(Request["mancount"])) ? "" : Request["mancount"].ToString().Trim();
            string filename = (string.IsNullOrEmpty(Request["filename"])) ? "" : Request["filename"].ToString().Trim();
            string other = (string.IsNullOrEmpty(Request["other"])) ? "" : Request["other"].ToString().Trim();

            db._guid = rid;
            db._父層guid = rpid;
            db._工作項次 = no;
            db._執行內容 = content;
            db._預定日期 = getyear(time) + getmonth(time) + getdate(time);
            db._季度 = getSeason(getmonth(time));
            db._年度 = getyear(time);
            db._建立者 = LogInfo.mGuid;
            db._修改者 = LogInfo.mGuid;

            switch (type)
            {
                case "1":
                    db._地點 = place;
                    db._人數 = mancount;
                    db._備註 = other;

                    db.SaveReport1(oConn, myTrans, mode);
                    break;
                case "2":
                    db._備註 = other;

                    db.SaveReport2(oConn, myTrans, mode);
                    break;
                case "3":
                    db._人數 = mancount;
                    db._備註 = other;

                    db.SaveReport3(oConn, myTrans, mode);
                    break;
                case "4":
                    db._文件名稱 = filename;

                    db.SaveReport4(oConn, myTrans, mode);
                    break;
                case "5":
                    db._地點 = place;
                    db._人數 = mancount;
                    db._備註 = other;

                    db.SaveReport5(oConn, myTrans, mode);
                    break;
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