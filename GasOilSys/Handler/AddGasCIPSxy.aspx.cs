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

public partial class Handler_AddGasCIPSxy : System.Web.UI.Page
{
    GasCIPSXY_DB db = new GasCIPSXY_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 緊密電位檢測xy座標
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["pGuid"]: 父層guid 
        /// * Request["guid"]: guid 
        /// * Request["year"]: 年度
        /// * Request["txt1"]: x座標
        /// * Request["txt2"]: y座標 
        /// * Request["txt3"]: 級距 
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
            string pGuid = (string.IsNullOrEmpty(Request["pGuid"])) ? "" : Request["pGuid"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string txt2 = (string.IsNullOrEmpty(Request["txt2"])) ? "" : Request["txt2"].ToString().Trim();
            string txt3 = (string.IsNullOrEmpty(Request["txt3"])) ? "" : Request["txt3"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            db._業者guid = cp;
            db._父層guid = pGuid;
            db._年度 = Server.UrlDecode(year);
            db._x座標 = Server.UrlDecode(txt1);
            db._y座標 = Server.UrlDecode(txt2);
            db._級距 = Server.UrlDecode(txt3);
            db._修改者 = LogInfo.mGuid;
            db._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                db._建立者 = LogInfo.mGuid;
                db._建立日期 = DateTime.Now;

                db.InsertData(oConn, myTrans);
            }
            else
            {
                db._guid = guid;
                db.UpdateData(oConn, myTrans);
            }

            string sn = string.Empty;

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