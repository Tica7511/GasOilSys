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

public partial class Handler_AddGasUnusualRectifier : System.Web.UI.Page
{
    GasUnusualRectifier_DB gdb = new GasUnusualRectifier_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 異常整流站
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 異常整流站名稱
        /// * Request["txt2_1"]: 異常起始日期 年份 
        /// * Request["txt2_2"]: 異常起始日期 月份
        /// * Request["txt3"]: 異常狀況
        /// * Request["txt4"]: 整流站修復進度
        /// * Request["txt5"]: 影響長途管線識別碼
        /// * Request["txt6"]: 預計完成日期 
        /// * Request["txt7"]: 備註
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
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string txt2_1 = (string.IsNullOrEmpty(Request["txt2_1"])) ? "" : Request["txt2_1"].ToString().Trim();
            string txt2_2 = (string.IsNullOrEmpty(Request["txt2_2"])) ? "" : Request["txt2_2"].ToString().Trim();
            string txt3 = (string.IsNullOrEmpty(Request["txt3"])) ? "" : Request["txt3"].ToString().Trim();
            string txt4 = (string.IsNullOrEmpty(Request["txt4"])) ? "" : Request["txt4"].ToString().Trim();
            string txt5 = (string.IsNullOrEmpty(Request["txt5"])) ? "" : Request["txt5"].ToString().Trim();
            string txt6 = (string.IsNullOrEmpty(Request["txt6"])) ? "" : Request["txt6"].ToString().Trim();
            string txt7 = (string.IsNullOrEmpty(Request["txt7"])) ? "" : Request["txt7"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode(year);
            gdb._異常整流站名稱 = Server.UrlDecode(txt1);
            gdb._異常起始日期年月 = Server.UrlDecode(txt2_1) + "/" + Server.UrlDecode(txt2_2);
            gdb._異常狀況 = Server.UrlDecode(txt3);
            gdb._整流站修復進度 = Server.UrlDecode(txt4);
            gdb._影響長途管線識別碼 = Server.UrlDecode(txt5);
            gdb._預計完成日期 = Server.UrlDecode(txt6);
            gdb._備註 = Server.UrlDecode(txt7);
            gdb._修改者 = LogInfo.mGuid;
            gdb._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                gdb._建立者 = LogInfo.mGuid;
                gdb._建立日期 = DateTime.Now;

                gdb.InsertData(oConn, myTrans);
            }
            else
            {
                gdb._guid = guid;
                gdb.UpdateData(oConn, myTrans);
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