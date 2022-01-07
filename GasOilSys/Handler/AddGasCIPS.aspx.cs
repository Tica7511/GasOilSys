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

public partial class Handler_AddGasCIPS : System.Web.UI.Page
{
    GasCIPS_DB gdb = new GasCIPS_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 緊密電位檢測
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 長途管線識別碼
        /// * Request["txt2"]: 同時檢測管線數量 
        /// * Request["txt3_1"]: 最近一次執行 年份 
        /// * Request["txt3_2"]: 最近一次執行 月份
        /// * Request["txt4_1"]: 報告產出 年份
        /// * Request["txt4_2"]:報告產出 月份
        /// * Request["txt5"]: 檢測長度
        /// * Request["txt6"]: 合格標準	
        /// * Request["txt7"]: 立即改善 數量
        /// * Request["txt8"]: 立即改善 改善完成數量
        /// * Request["txt9"]: 排程改善 數量
        /// * Request["txt10"]: 排程改善 改善完成數量
        /// * Request["txt11"]: 需監控點 數量
        /// * Request["txt12"]: 備註 
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
            string txt2 = (string.IsNullOrEmpty(Request["txt2"])) ? "" : Request["txt2"].ToString().Trim();
            string txt3_1 = (string.IsNullOrEmpty(Request["txt3_1"])) ? "" : Request["txt3_1"].ToString().Trim();
            string txt3_2 = (string.IsNullOrEmpty(Request["txt3_2"])) ? "" : Request["txt3_2"].ToString().Trim();
            string txt4_1 = (string.IsNullOrEmpty(Request["txt4_1"])) ? "" : Request["txt4_1"].ToString().Trim();
            string txt4_2 = (string.IsNullOrEmpty(Request["txt4_2"])) ? "" : Request["txt4_2"].ToString().Trim();
            string txt5 = (string.IsNullOrEmpty(Request["txt5"])) ? "" : Request["txt5"].ToString().Trim();
            string txt6 = (string.IsNullOrEmpty(Request["txt6"])) ? "" : Request["txt6"].ToString().Trim();
            string txt7 = (string.IsNullOrEmpty(Request["txt7"])) ? "" : Request["txt7"].ToString().Trim();
            string txt8 = (string.IsNullOrEmpty(Request["txt8"])) ? "" : Request["txt8"].ToString().Trim();
            string txt9 = (string.IsNullOrEmpty(Request["txt9"])) ? "" : Request["txt9"].ToString().Trim();
            string txt10 = (string.IsNullOrEmpty(Request["txt10"])) ? "" : Request["txt10"].ToString().Trim();
            string txt11 = (string.IsNullOrEmpty(Request["txt11"])) ? "" : Request["txt11"].ToString().Trim();
            string txt12 = (string.IsNullOrEmpty(Request["txt12"])) ? "" : Request["txt12"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode(year);
            gdb._長途管線識別碼 = Server.UrlDecode(txt1);
            gdb._同時檢測管線數量 = Server.UrlDecode(txt2);
            gdb._最近一次執行年月 = Server.UrlDecode(txt3_1) + "/" + Server.UrlDecode(txt3_2);
            gdb._報告產出年月 = Server.UrlDecode(txt4_1) + "/" + Server.UrlDecode(txt4_2);
            gdb._檢測長度 = Server.UrlDecode(txt5);
            gdb._合格標準請參照填表說明2 = Server.UrlDecode(txt6);
            gdb._立即改善_數量 = Server.UrlDecode(txt7);
            gdb._立即改善_改善完成數量 = Server.UrlDecode(txt8);
            gdb._排程改善_數量 = Server.UrlDecode(txt9);
            gdb._排程改善_改善完成數量 = Server.UrlDecode(txt10);
            gdb._需監控點_數量 = Server.UrlDecode(txt11);
            gdb._備註 = Server.UrlDecode(txt12);
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