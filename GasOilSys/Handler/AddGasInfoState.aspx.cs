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

public partial class Handler_AddGasInfoState : System.Web.UI.Page
{
    GasInfo_DB db = new GasInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 事業單位基本資料場站中心
        ///說    明:
        /// * Request["cid"]: 業者guid 
        /// * Request["txt1"]: 場站類別
        /// * Request["txt2"]: 中心名稱 
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

            string cid = (string.IsNullOrEmpty(Request["cid"])) ? "" : Request["cid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string txt2 = (string.IsNullOrEmpty(Request["txt2"])) ? "" : Request["txt2"].ToString().Trim();
            string sn = string.Empty;
            string xmlstr = string.Empty;

            db._業者guid = cid;
            db._年度 = year;
            db._場站類別 = txt1;
            db._中心名稱 = Server.UrlDecode(txt2);

            DataTable dt = db.GetMaxSn();

            if (dt.Rows.Count > 0)
            {
                sn = (Convert.ToInt32(dt.Rows[0]["maxSn"].ToString().Trim()) + 1).ToString();
            }
            else
            {
                sn = "1";
            }

            db._排序 = sn;
            db._建立者 = LogInfo.mGuid;
            db._建立日期 = DateTime.Now;
            db._修改者 = LogInfo.mGuid;
            db._修改日期 = DateTime.Now;

            db.InsertData(oConn, myTrans);

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