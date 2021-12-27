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

public partial class Handler_AddOilTubeCheck : System.Web.UI.Page
{
    OilTubeCheck_DB odb = new OilTubeCheck_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 管線巡檢
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["year"]: 年度
        /// * Request["docName"]: 依據文件名稱
        /// * Request["docNo"]: 文件編號
        /// * Request["docDate"]: 文件日期
        /// * Request["checkCount"]: 每日巡檢次數
        /// * Request["checkPerson"]: 巡管人數
        /// * Request["checkTool"]: 巡管工具
        /// * Request["checkToolOther"]: 巡管工具其他
        /// * Request["managerCheck"]: 主管監督查核
        /// * Request["ManagerCount"]: 主管監督查核次
        /// * Request["checkStrengthen"]: 是否有加強巡檢點
        /// * Request["StrengthenTxt"]: 是否有加強巡檢點敘述
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
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string docName = (string.IsNullOrEmpty(Request["docName"])) ? "" : Request["docName"].ToString().Trim();
            string docNo = (string.IsNullOrEmpty(Request["docNo"])) ? "" : Request["docNo"].ToString().Trim();
            string docDate = (string.IsNullOrEmpty(Request["docDate"])) ? "" : Request["docDate"].ToString().Trim();
            string checkPerson = (string.IsNullOrEmpty(Request["checkPerson"])) ? "" : Request["checkPerson"].ToString().Trim();
            string checkToolOther = (string.IsNullOrEmpty(Request["checkToolOther"])) ? "" : Request["checkToolOther"].ToString().Trim();
            string ManagerCount = (string.IsNullOrEmpty(Request["ManagerCount"])) ? "" : Request["ManagerCount"].ToString().Trim();
            string StrengthenTxt = (string.IsNullOrEmpty(Request["StrengthenTxt"])) ? "" : Request["StrengthenTxt"].ToString().Trim();
            string xmlstr = string.Empty;

            odb._業者guid = cp;
            odb._年度 = Server.UrlDecode(year);
            odb._依據文件名稱 = Server.UrlDecode(docName);
            odb._文件編號 = Server.UrlDecode(docNo);
            odb._文件日期 = Server.UrlDecode(docDate);
            odb._每日巡檢次數 = string.IsNullOrEmpty(Request["checkCount"]) ? "" : Request["checkCount"].ToString().Trim();
            odb._巡管人數 = Server.UrlDecode(checkPerson);
            odb._巡管工具 = string.IsNullOrEmpty(Request["checkTool"]) ? "" : Request["checkTool"].ToString().Trim();
            odb._巡管工具其他 = Server.UrlDecode(checkToolOther);
            odb._主管監督查核 = string.IsNullOrEmpty(Request["managerCheck"]) ? "" : Request["managerCheck"].ToString().Trim();
            odb._主管監督查核次 = Server.UrlDecode(ManagerCount);
            odb._是否有加強巡檢點 = string.IsNullOrEmpty(Request["checkStrengthen"]) ? "" : Request["checkStrengthen"].ToString().Trim();
            odb._是否有加強巡檢點敘述 = Server.UrlDecode(StrengthenTxt);
            odb._修改者 = LogInfo.mGuid;
            odb._修改日期 = DateTime.Now;

            DataTable dt = odb.GetList();

            if (dt.Rows.Count > 0)
            {
                odb._guid = dt.Rows[0]["guid"].ToString().Trim();
                odb.UpdateData(oConn, myTrans);
            }
            else
            {
                odb._建立者 = LogInfo.mGuid;
                odb._建立日期 = DateTime.Now;

                odb.InsertData(oConn, myTrans);
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