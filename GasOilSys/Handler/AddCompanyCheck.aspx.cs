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

public partial class Handler_AddCompanyCheck : System.Web.UI.Page
{
    OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
    GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 事故學習表
        ///說    明:
        /// * Request["cpid"]: 業者guid
        /// * Request["type"]: Oil=石油 Gas=天然氣 
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

            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string xmlstr = string.Empty;

            if(type == "Oil")
            {
                odb._guid = cpid;
                odb._資料是否確認 = "是";
                odb._修改者 = LogInfo.mGuid;
                odb._修改日期 = DateTime.Now;

                odb.UpdateData(oConn, myTrans);
            }
            else
            {
                gdb._guid = cpid;
                gdb._資料是否確認 = "是";
                gdb._修改者 = LogInfo.mGuid;
                gdb._修改日期 = DateTime.Now;

                gdb.UpdateData(oConn, myTrans);
            }

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>資料確認完成</Response><relogin>N</relogin></root>";

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