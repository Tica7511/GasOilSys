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

public partial class Handler_AddOilControlTubeData : System.Web.UI.Page
{
    OilControl_DB odb = new OilControl_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 控制室 管線輸送/接收資料
        ///說    明:
        /// * Request["cp"]:  業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 管線編號
        /// * Request["txt2"]:  控制室名稱
        /// * Request["txt3"]:  洩漏監控系統
        /// * Request["txt4"]:  操作壓力值
        /// * Request["txt5"]:  壓力警報設定值
        /// * Request["txt6"]:  流量警報設定值
        /// * Request["txt7"]:  前一年度異常下降警報發生頻率 
        /// * Request["txt8"]:  自有端是否有設置壓力 
        /// * Request["txt9"]:  自有端是否有設置流量 
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
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            odb._業者guid = cp;
            odb._年度 = Server.UrlDecode(year);
            odb._管線編號 = Server.UrlDecode(txt1);
            odb._控制室名稱 = Server.UrlDecode(txt2);
            odb._洩漏監控系統 = Server.UrlDecode(txt3);
            odb._自有端是否有設置壓力 = Server.UrlDecode(txt8);
            odb._自有端是否有設置流量 = Server.UrlDecode(txt9);
            odb._操作壓力值 = Server.UrlDecode(txt4);
            odb._壓力警報設定值 = Server.UrlDecode(txt5);
            odb._流量警報設定值 = Server.UrlDecode(txt6);
            odb._前一年度異常下降警報發生頻率 = Server.UrlDecode(txt7);
            odb._修改者 = LogInfo.mGuid;
            odb._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                odb._建立者 = LogInfo.mGuid;
                odb._建立日期 = DateTime.Now;

                odb.InsertData3(oConn, myTrans);
            }
            else
            {
                odb._guid = guid;
                odb.UpdateData3(oConn, myTrans);
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