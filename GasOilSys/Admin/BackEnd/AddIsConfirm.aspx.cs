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

public partial class Admin_BackEnd_AddIsConfirm : System.Web.UI.Page
{
    GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
    OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 修改 廠商資料是否確認
        ///說    明:
        /// * Request["guid"]: 業者guid 
        /// * Request["type"]: Gas=天然氣 Oil=石油 
        /// * Request["txt1"]: 資料是否確認 
        /// * Request["confirmType"]: 石油確認類別 01=資料確認 02=年度儲槽確認 03=年度液化石油氣確認
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

            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string confirmType = (string.IsNullOrEmpty(Request["confirmType"])) ? "" : Request["confirmType"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string xmlstr = string.Empty;

            if (Server.UrlDecode(type) == "Gas")
            {
                gdb._guid = guid;
                gdb._資料是否確認 = Server.UrlDecode(txt1);
                gdb._修改者 = LogInfo.mGuid;
                gdb._修改日期 = DateTime.Now;

                gdb.UpdateData(oConn, myTrans);
            }
            else
            {
                odb._guid = guid;
                switch (Server.UrlDecode(confirmType))
                {
                    case "01":
                        odb._資料是否確認 = Server.UrlDecode(txt1);
                        break;
                    case "02":
                        odb._年度儲槽確認 = Server.UrlDecode(txt1);
                        break;
                    case "03":
                        odb._年度液化石油氣儲槽確認 = Server.UrlDecode(txt1);
                        break;
                }
                odb._修改者 = LogInfo.mGuid;
                odb._修改日期 = DateTime.Now;

                odb.UpdateData(oConn, myTrans);
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