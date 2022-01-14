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

public partial class Handler_AddGasStorageTankInfoInfo : System.Web.UI.Page
{
    GasStorageTankInfo_DB gdb = new GasStorageTankInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 儲槽基本資料
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 液化天然氣廠
        /// * Request["txt2"]: 儲槽編號
        /// * Request["txt3"]: 容量
        /// * Request["txt4"]: 外徑
        /// * Request["txt5"]: 高度
        /// * Request["txt6"]: 形式
        /// * Request["txt7"]: 啟用日期
        /// * Request["txt8"]: 狀態
        /// * Request["txt9"]: 勞動部檢查
        /// * Request["txt10"]: 代行檢查機構
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
            string txt3 = (string.IsNullOrEmpty(Request["txt3"])) ? "" : Request["txt3"].ToString().Trim();
            string txt4 = (string.IsNullOrEmpty(Request["txt4"])) ? "" : Request["txt4"].ToString().Trim();
            string txt5 = (string.IsNullOrEmpty(Request["txt5"])) ? "" : Request["txt5"].ToString().Trim();
            string txt6 = (string.IsNullOrEmpty(Request["txt6"])) ? "" : Request["txt6"].ToString().Trim();
            string txt7 = (string.IsNullOrEmpty(Request["txt7"])) ? "" : Request["txt7"].ToString().Trim();
            string txt8 = (string.IsNullOrEmpty(Request["txt8"])) ? "" : Request["txt8"].ToString().Trim();
            string txt9 = (string.IsNullOrEmpty(Request["txt9"])) ? "" : Request["txt9"].ToString().Trim();
            string txt10 = (string.IsNullOrEmpty(Request["txt10"])) ? "" : Request["txt10"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode(year);
            gdb._液化天然氣廠 = Server.UrlDecode(txt1);
            gdb._儲槽編號 = Server.UrlDecode(txt2);
            gdb._容量 = Server.UrlDecode(txt3);
            gdb._外徑 = Server.UrlDecode(txt4);
            gdb._高度 = Server.UrlDecode(txt5);
            gdb._形式 = Server.UrlDecode(txt6);
            gdb._啟用日期 = Server.UrlDecode(txt7);
            gdb._狀態 = Server.UrlDecode(txt8);
            gdb._勞動部檢查 = Server.UrlDecode(txt9);
            gdb._代行檢查機構 = Server.UrlDecode(txt10);
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