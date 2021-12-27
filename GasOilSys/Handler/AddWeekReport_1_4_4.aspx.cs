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

public partial class Handler_AddWeekReport_1_4_4 : System.Web.UI.Page
{
    WeekReport_1_4_4_DB db = new WeekReport_1_4_4_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 新增/修改 計劃書1.4.4
		///說    明:
		/// * Request["mode"]: 新增/修改
		/// * Request["type"]: list1=週報_計劃書1_4_4_列表 list2=週報_計劃書1_4_4_檢測列表
		/// * Request["rid"]: guid 
		/// * Request["rpid"]: 父層 guid 
		/// * Request["no"]: 工作項次 
		/// * Request["unit"]: 受查單位 
		/// * Request["unitname"]: 站場名稱 
		/// * Request["time"]: 預定日期 
		/// * Request["place"]: 地點 
		/// * Request["location"]: 洩漏位置 
		/// * Request["concentration"]: 洩漏源甲烷濃度 
		/// * Request["situation"]: 改善情形 
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
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string rid = (string.IsNullOrEmpty(Request["rid"])) ? Guid.NewGuid().ToString("D") : Request["rid"].ToString().Trim();
            string rpid = (string.IsNullOrEmpty(Request["rpid"])) ? "" : Request["rpid"].ToString().Trim();
            string no = (string.IsNullOrEmpty(Request["no"])) ? "" : Request["no"].ToString().Trim();
            string unit = (string.IsNullOrEmpty(Request["unit"])) ? "" : Request["unit"].ToString().Trim();
            string unitname = (string.IsNullOrEmpty(Request["unitname"])) ? "" : Request["unitname"].ToString().Trim();
            string time = (string.IsNullOrEmpty(Request["time"])) ? "" : Request["time"].ToString().Trim();
            string place = (string.IsNullOrEmpty(Request["place"])) ? "" : Request["place"].ToString().Trim();
            string location = (string.IsNullOrEmpty(Request["location"])) ? "" : Request["location"].ToString().Trim();
            string concentration = (string.IsNullOrEmpty(Request["concentration"])) ? "" : Request["concentration"].ToString().Trim();
            string situation = (string.IsNullOrEmpty(Request["situation"])) ? "" : Request["situation"].ToString().Trim();
            string other = (string.IsNullOrEmpty(Request["other"])) ? "" : Request["other"].ToString().Trim();
            string sn = string.Empty;
            string season = string.Empty;

            //取得年月日
            string year = time.Substring(0, 3);
            string month = time.Substring(3, 2);
            string date = time.Substring(5, 2);

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
            db._季度 = season;
            db._年度 = year;
            db._建立者 = LogInfo.mGuid;
            db._修改者 = LogInfo.mGuid;

            if (type == "list1")
            {
                db._受查單位 = unit;
                db._預定日期 = year + month + date;
                db._地點 = place;
                db._備註 = other;

                //儲存
                db.SaveReport(oConn, myTrans, mode);
            }
            else
            {
                db._檢測單位 = unit;
                db._站場名稱 = unitname;
                db._預定日期 = year + month + date;
                db._洩漏位置 = location;
                db._洩漏源甲烷濃度 = concentration;
                db._改善情形 = situation;

                //儲存
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
}