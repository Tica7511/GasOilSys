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

public partial class Handler_AddGasStorageTankInfo : System.Web.UI.Page
{
    GasStorageTankInfo_DB gdb = new GasStorageTankInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 儲槽設施資料 廠區特殊區域
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["year"]: 年度
        /// * Request["checkAreaOther"]: 其它內容 
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
            string checkAreaOther = (string.IsNullOrEmpty(Request["checkAreaOther"])) ? "" : Request["checkAreaOther"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode(year);
            gdb._庫區特殊區域 = string.IsNullOrEmpty(Request["checkArea"]) ? "" : Request["checkArea"].ToString().Trim();
            gdb._庫區特殊區域_其他 = Server.UrlDecode(checkAreaOther);
            gdb._修改者 = LogInfo.mGuid;
            gdb._修改日期 = DateTime.Now;

            DataTable dt = gdb.GetList2();

            if (dt.Rows.Count > 0)
            {
                gdb._guid = dt.Rows[0]["guid"].ToString().Trim();
                gdb.UpdateData2(oConn, myTrans);
            }
            else
            {
                gdb._建立者 = LogInfo.mGuid;
                gdb._建立日期 = DateTime.Now;

                gdb.InsertData2(oConn, myTrans);
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