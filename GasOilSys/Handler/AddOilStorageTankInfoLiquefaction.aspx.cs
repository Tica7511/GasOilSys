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

public partial class Handler_AddOilStorageTankInfoLiquefaction : System.Web.UI.Page
{
    OilStorageTankInfoLiquefaction_DB odb = new OilStorageTankInfoLiquefaction_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 儲槽基本資料液化石油氣
        ///說    明:
        /// * Request["cp"]:  業者guid
        /// * Request["guid"]: guid
        /// * Request["companyName"]: 業者名稱
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 轄區儲槽編號
        /// * Request["txt2"]:  能源局編號
        /// * Request["txt3"]:  容量
        /// * Request["txt4"]:  內徑
        /// * Request["txt5"]:  內容物
        /// * Request["txt6"]:  形式
        /// * Request["txt7"]:  狀態
        /// * Request["txt9_1"]:  啟用日期(民國年份)
        /// * Request["txt9_2"]:  啟用日期(月份)
        /// * Request["txt14"]: 油品種類
        /// * Request["mode"]:  new=新增 edit=編輯
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

            string cp = (string.IsNullOrEmpty(Request["cp"])) ? "" : Request["cp"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string companyName = (string.IsNullOrEmpty(Request["companyName"])) ? "" : Request["companyName"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string txt2 = (string.IsNullOrEmpty(Request["txt2"])) ? "" : Request["txt2"].ToString().Trim();
            string txt3 = (string.IsNullOrEmpty(Request["txt3"])) ? "" : Request["txt3"].ToString().Trim();
            string txt4 = (string.IsNullOrEmpty(Request["txt4"])) ? "" : Request["txt4"].ToString().Trim();
            string txt5 = (string.IsNullOrEmpty(Request["txt5"])) ? "" : Request["txt5"].ToString().Trim();
            string txt6 = (string.IsNullOrEmpty(Request["txt6"])) ? "" : Request["txt6"].ToString().Trim();
            string txt7 = (string.IsNullOrEmpty(Request["txt7"])) ? "" : Request["txt7"].ToString().Trim();
            string txt9_1 = (string.IsNullOrEmpty(Request["txt9_1"])) ? "" : Request["txt9_1"].ToString().Trim();
            string txt9_2 = (string.IsNullOrEmpty(Request["txt9_2"])) ? "" : Request["txt9_2"].ToString().Trim();
            string txt14 = (string.IsNullOrEmpty(Request["txt14"])) ? "" : Request["txt14"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            odb._業者guid = cp;
            odb._年度 = Server.UrlDecode(year);
            odb._業者名稱 = Server.UrlDecode(companyName);
            odb._轄區儲槽編號 = Server.UrlDecode(txt1);
            odb._能源局編號 = Server.UrlDecode(txt2);
            odb._容量 = Server.UrlDecode(txt3);
            odb._內徑 = Server.UrlDecode(txt4);
            odb._內容物 = Server.UrlDecode(txt5);
            odb._油品種類 = Server.UrlDecode(txt14);
            odb._形式 = Server.UrlDecode(txt6);
            odb._狀態 = Server.UrlDecode(txt7);
            if(!string.IsNullOrEmpty(Server.UrlDecode(txt9_1)) && !string.IsNullOrEmpty(Server.UrlDecode(txt9_2)))
                odb._啟用日期 = Server.UrlDecode(txt9_1) + "/" + Server.UrlDecode(txt9_2);
            else
                odb._啟用日期 = Server.UrlDecode(txt9_1) + Server.UrlDecode(txt9_2);
            odb._修改者 = LogInfo.mGuid;
            odb._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                odb._建立者 = LogInfo.mGuid;
                odb._建立日期 = DateTime.Now;

                odb.InsertData(oConn, myTrans);
            }
            else
            {
                odb._guid = guid;
                odb.UpdateData(oConn, myTrans);
            }

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response><relogin>N</relogin></root>";

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