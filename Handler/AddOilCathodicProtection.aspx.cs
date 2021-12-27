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

public partial class Handler_AddOilCathodicProtection : System.Web.UI.Page
{
    OilCathodicProtection_DB odb = new OilCathodicProtection_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 陰極防蝕系統
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["year"]: 年度
        /// * Request["PMPeriod"]: 儲槽陰極防蝕系統電位量測週期
        /// * Request["PSMPeriod"]: 整流站量測週期
        /// * Request["checkUnit"]: 儲槽陰極防蝕系統電位量測單位
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
            string PMPeriod = (string.IsNullOrEmpty(Request["PMPeriod"])) ? "" : Request["PMPeriod"].ToString().Trim();
            string PSMPeriod = (string.IsNullOrEmpty(Request["PSMPeriod"])) ? "" : Request["PSMPeriod"].ToString().Trim();
            string xmlstr = string.Empty;

            odb._業者guid = cp;
            odb._年度 = Server.UrlDecode(year);
            odb._電位量測週期 = Server.UrlDecode(PMPeriod);
            odb._整流站量測週期 = Server.UrlDecode(PSMPeriod);
            odb._電位量測單位 = string.IsNullOrEmpty(Request["checkUnit"]) ? "" : Request["checkUnit"].ToString().Trim();
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