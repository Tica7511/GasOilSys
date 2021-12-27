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

public partial class Handler_DelWeekReport_6_2 : System.Web.UI.Page
{
    WeekReport_6_2_DB db = new WeekReport_6_2_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除計劃書6.2 工作項次
		///說    明:
		/// * Request["type"]: 資料表排序 
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

            switch (type)
            {
                case "1":
                    db.DeleteReport(oConn, myTrans, "週報_計劃書6_2_總工作報告會議");
                    break;
                case "2":
                    db.DeleteReport(oConn, myTrans, "週報_計劃書6_2_繳交報告及配合事項");
                    break;
                case "3":
                    db.DeleteReport(oConn, myTrans, "週報_計劃書6_2_專家及業者會議");
                    break;
                case "4":
                    db.DeleteReport(oConn, myTrans, "週報_計劃書6_2_臨時交辦議題配合事項");
                    break;
                case "5":
                    db.DeleteReport(oConn, myTrans, "週報_計劃書6_2_工作討論會議統計");
                    break;
            }

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