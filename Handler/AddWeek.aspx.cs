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

public partial class WebPage_AddWeek : System.Web.UI.Page
{
    Week_DB db = new Week_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 新增/修改 週報
		///說    明:
		/// * Request["mode"]: 新增/修改
		/// * Request["guid"]: guid 
		/// * Request["nb1"]: 編號1 
		/// * Request["nb2"]: 編號2 
		/// * Request["nb3"]: 編號3 
		/// * Request["content"]: 執行內容 
		/// * Request["time"]: 預定日期 
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
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? Guid.NewGuid().ToString("D") : Request["guid"].ToString().Trim();
            string nb1 = (string.IsNullOrEmpty(Request["nb1"])) ? "" : Request["nb1"].ToString().Trim();
            string nb2 = (string.IsNullOrEmpty(Request["nb2"])) ? "" : Request["nb2"].ToString().Trim();
            string nb3 = (string.IsNullOrEmpty(Request["nb3"])) ? "" : Request["nb3"].ToString().Trim();
            string content = (string.IsNullOrEmpty(Request["content"])) ? "" : Request["content"].ToString().Trim();
            string time = (string.IsNullOrEmpty(Request["time"])) ? "" : Request["time"].ToString().Trim();

            db._guid = guid;
            if(string.IsNullOrEmpty(nb3))
                db._工作項次 = nb1 + "." + nb2;
            else
                db._工作項次 = nb1 + "." + nb2 + "." + nb3;
            db._執行內容 = content;
            db._預定日期 = time;
            db._建立者 = LogInfo.mGuid;
            db._修改者 = LogInfo.mGuid;

            //儲存
            db.SaveWeek(oConn, myTrans, mode);

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