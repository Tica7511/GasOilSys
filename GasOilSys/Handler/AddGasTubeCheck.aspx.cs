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

public partial class Handler_AddGasTubeCheck : System.Web.UI.Page
{
    GasTubeCheck_DB gdb = new GasTubeCheck_DB();
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
            string checkPerson = (string.IsNullOrEmpty(Request["checkPerson"])) ? "" : Request["checkPerson"].ToString().Trim();
            string checkToolOther = (string.IsNullOrEmpty(Request["checkToolOther"])) ? "" : Request["checkToolOther"].ToString().Trim();
            string ManagerCount = (string.IsNullOrEmpty(Request["ManagerCount"])) ? "" : Request["ManagerCount"].ToString().Trim();
            string StrengthenTxt = (string.IsNullOrEmpty(Request["StrengthenTxt"])) ? "" : Request["StrengthenTxt"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode(year);
            gdb._每日巡檢次數 = string.IsNullOrEmpty(Request["checkCount"]) ? "" : Request["checkCount"].ToString().Trim();
            gdb._巡管人數 = Server.UrlDecode(checkPerson);
            gdb._巡管工具 = string.IsNullOrEmpty(Request["checkTool"]) ? "" : Request["checkTool"].ToString().Trim();
            gdb._巡管工具其他 = Server.UrlDecode(checkToolOther);
            gdb._主管監督查核 = string.IsNullOrEmpty(Request["managerCheck"]) ? "" : Request["managerCheck"].ToString().Trim();
            gdb._主管監督查核次 = Server.UrlDecode(ManagerCount);
            gdb._是否有加強巡檢點 = string.IsNullOrEmpty(Request["checkStrengthen"]) ? "" : Request["checkStrengthen"].ToString().Trim();
            gdb._是否有加強巡檢點敘述 = Server.UrlDecode(StrengthenTxt);
            gdb._修改者 = LogInfo.mGuid;
            gdb._修改日期 = DateTime.Now;

            DataTable dt = gdb.GetList();

            if (dt.Rows.Count > 0)
            {
                gdb._guid = dt.Rows[0]["guid"].ToString().Trim();
                gdb.UpdateData(oConn, myTrans);
            }
            else
            {
                gdb._建立者 = LogInfo.mGuid;
                gdb._建立日期 = DateTime.Now;

                gdb.InsertData(oConn, myTrans);
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