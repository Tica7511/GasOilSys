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

public partial class Handler_DelWeekReport_5_7 : System.Web.UI.Page
{
    WeekReport_5_7_DB db = new WeekReport_5_7_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除計劃書5.7 工作項次
		///說    明:
		/// * Request["type"]: Request["type"]: 用於取得不同資料表 list1: 週報_計劃書5_7_審閱進度, list2: 週報_計劃書5_7_其他災防事項 
		/// * Request["rid"]: Guid 
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

            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string rid = (string.IsNullOrEmpty(Request["rid"])) ? "" : Request["rid"].ToString().Trim();
            string xmlstr = string.Empty;

            db._guid = rid;
            db._修改者 = LogInfo.mGuid;

            if(type == "list1")
                db.DeleteReport1(oConn, myTrans);
            else
                db.DeleteReport2(oConn, myTrans);

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>刪除完成</Response></root>";
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