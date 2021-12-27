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

public partial class Handler_AddWeekReport_5_7 : System.Web.UI.Page
{
    WeekReport_5_7_DB db = new WeekReport_5_7_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 計劃書5.7
        ///說    明:
        /// * Request["type"]: 用於取得不同資料表 list1: 週報_計劃書5_7_審閱進度, list2: 週報_計劃書5_7_其他災防事項
        /// * Request["mode"]: 新增/修改
        /// * Request["rid"]: guid 
        /// * Request["rpid"]: 父層 guid 
        /// * Request["no"]: 工作項次 
        /// * Request["company"]: 公用天然氣事業
        /// * Request["time1"]: 能源局發文日期 
        /// * Request["time2"]: 業者繳交日期 
        /// * Request["time3"]: 工研院審閱日期 
        /// * Request["time4"]: 補正情形 
        /// * Request["content"]: 執行內容
        /// * Request["begintime"]: 預定日期起
        /// * Request["endtime"]: 預定日期迄
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
            string company = (string.IsNullOrEmpty(Request["company"])) ? "" : Request["company"].ToString().Trim();
            string time1 = (string.IsNullOrEmpty(Request["time1"])) ? "" : Request["time1"].ToString().Trim();
            string time2 = (string.IsNullOrEmpty(Request["time2"])) ? "" : Request["time2"].ToString().Trim();
            string time3 = (string.IsNullOrEmpty(Request["time3"])) ? "" : Request["time3"].ToString().Trim();
            string time4 = (string.IsNullOrEmpty(Request["time4"])) ? "" : Request["time4"].ToString().Trim();
            string content = (string.IsNullOrEmpty(Request["content"])) ? "" : Request["content"].ToString().Trim();
            string begintime = (string.IsNullOrEmpty(Request["begintime"])) ? "" : Request["begintime"].ToString().Trim();
            string endtime = (string.IsNullOrEmpty(Request["endtime"])) ? "" : Request["endtime"].ToString().Trim();
            string other = (string.IsNullOrEmpty(Request["other"])) ? "" : Request["other"].ToString().Trim();
            string year = string.Empty;
            string month = string.Empty;
            string date = string.Empty;

            db._guid = rid;
            db._父層guid = rpid;
            db._工作項次 = no;
            db._備註 = other;
            db._建立者 = LogInfo.mGuid;
            db._修改者 = LogInfo.mGuid;

            if (type == "list1")
            {
                db._公用天然氣事業 = company;
                db._能源局發文日期 = getyear(time1) + getmonth(time1) + getdate(time1);
                db._業者繳交日期 = getyear(time2) + getmonth(time2) + getdate(time2);
                db._工研院審閱日期 = getyear(time3) + getmonth(time3) + getdate(time3);
                db._補正情形 = (string.IsNullOrEmpty(time4)) ? "" : getyear(time4) + getmonth(time4) + getdate(time4);
                db._季度 = getseason(getmonth(time3));
                db._年度 = getyear(time3);

                db.SaveReport1(oConn, myTrans, mode);
            }
            else
            {
                db._執行內容 = content;
                db._預定日期起 = getyear(begintime) + getmonth(begintime) + getdate(begintime);
                db._預定日期迄 = getyear(endtime) + getmonth(endtime) + getdate(endtime);
                db._季度 = getseason(getmonth(begintime));
                db._年度 = getyear(begintime);

                db.SaveReport2(oConn, myTrans, mode);
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

    public string getseason(string month)
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