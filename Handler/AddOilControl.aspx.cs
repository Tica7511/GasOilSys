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

public partial class Handler_AddOilControl : System.Web.UI.Page
{
    OilControl_DB odb = new OilControl_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 控制室
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["year"]: 年度
        /// * Request["docName"]: 依據文件名稱
        /// * Request["docNo"]: 文件編號
        /// * Request["docDate"]: 文件日期
        /// * Request["pressureHz"]: 壓力計校正頻率
        /// * Request["pressureRecentTime"]: 壓力計校正_最近一次校正時間
        /// * Request["flowHz"]: 流量計校正頻率
        /// * Request["flowRecentTime"]: 流量計校正_最近一次校正時間
        /// * Request["monitorTime"]: 為使監控中心之時鐘、電腦系統、監視器時間一致，定期調整之週期
        /// * Request["TotalOperator"]: 合格操作人員總數
        /// * Request["rbShift"]: 輪班制度
        /// * Request["classPerson"]: 每班人數
        /// * Request["rbClassTime"]: 每班時數
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
            string pressureHz = (string.IsNullOrEmpty(Request["pressureHz"])) ? "" : Request["pressureHz"].ToString().Trim();
            string pressureRecentTime = (string.IsNullOrEmpty(Request["pressureRecentTime"])) ? "" : Request["pressureRecentTime"].ToString().Trim();
            string flowHz = (string.IsNullOrEmpty(Request["flowHz"])) ? "" : Request["flowHz"].ToString().Trim();
            string flowRecentTime = (string.IsNullOrEmpty(Request["flowRecentTime"])) ? "" : Request["flowRecentTime"].ToString().Trim();
            string monitorTime = (string.IsNullOrEmpty(Request["monitorTime"])) ? "" : Request["monitorTime"].ToString().Trim();
            string TotalOperator = (string.IsNullOrEmpty(Request["TotalOperator"])) ? "" : Request["TotalOperator"].ToString().Trim();
            string rbShift = (string.IsNullOrEmpty(Request["rbShift"])) ? "" : Request["rbShift"].ToString().Trim();
            string classPerson = (string.IsNullOrEmpty(Request["classPerson"])) ? "" : Request["classPerson"].ToString().Trim();
            string rbClassTime = (string.IsNullOrEmpty(Request["rbClassTime"])) ? "" : Request["rbClassTime"].ToString().Trim();
            string xmlstr = string.Empty;

            odb._業者guid = cp;
            odb._年度 = Server.UrlDecode(year);
            odb._依據文件名稱 = Server.UrlDecode(docName);
            odb._文件編號 = Server.UrlDecode(docNo);
            odb._文件日期 = Server.UrlDecode(docDate);
            odb._壓力計校正頻率 = Server.UrlDecode(pressureHz);
            odb._壓力計校正_最近一次校正時間 = Server.UrlDecode(pressureRecentTime);
            odb._流量計校正頻率 = Server.UrlDecode(flowHz);
            odb._流量計校正_最近一次校正時間 = Server.UrlDecode(flowRecentTime);
            odb._監控中心定期調整之週期 = Server.UrlDecode(monitorTime);
            odb._合格操作人員總數 = Server.UrlDecode(TotalOperator);
            odb._輪班制度 = string.IsNullOrEmpty(Request["rbShift"]) ? "" : Request["rbShift"].ToString().Trim();
            odb._每班人數 = Server.UrlDecode(classPerson);
            odb._每班時數 = string.IsNullOrEmpty(Request["rbClassTime"]) ? "" : Request["rbClassTime"].ToString().Trim();
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