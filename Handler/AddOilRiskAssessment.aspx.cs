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

public partial class Handler_AddOilRiskAssessment : System.Web.UI.Page
{
    OilRiskAssessment_DB odb = new OilRiskAssessment_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 風險評估
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 長途管線識別碼 
        /// * Request["txt2_1"]: 最近一次執行日期 年份 
        /// * Request["txt2_2"]: 最近一次執行日期 月份
        /// * Request["txt3"]: 再評估時機
        /// * Request["txt4"]: 管線長度
        /// * Request["txt5"]: 分段數量
        /// * Request["txt6"]: 已納入ILI結果
        /// * Request["txt7"]: 已納入CIPS結果
        /// * Request["txt8"]: 已納入巡管結果
        /// * Request["txt9"]: 各等級風險管段數量_高
        /// * Request["txt10"]: 各等級風險管段數量_中
        /// * Request["txt11"]: 各等級風險管段數量_低
        /// * Request["txt12"]: 文件名稱
        /// * Request["txt13"]: 改善後風險等級
        /// * Request["txt14"]: 備註 
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
            string txt2_1 = (string.IsNullOrEmpty(Request["txt2_1"])) ? "" : Request["txt2_1"].ToString().Trim();
            string txt2_2 = (string.IsNullOrEmpty(Request["txt2_2"])) ? "" : Request["txt2_2"].ToString().Trim();
            string txt3 = (string.IsNullOrEmpty(Request["txt3"])) ? "" : Request["txt3"].ToString().Trim();
            string txt4 = (string.IsNullOrEmpty(Request["txt4"])) ? "" : Request["txt4"].ToString().Trim();
            string txt5 = (string.IsNullOrEmpty(Request["txt5"])) ? "" : Request["txt5"].ToString().Trim();
            string txt6 = (string.IsNullOrEmpty(Request["txt6"])) ? "" : Request["txt6"].ToString().Trim();
            string txt7 = (string.IsNullOrEmpty(Request["txt7"])) ? "" : Request["txt7"].ToString().Trim();
            string txt8 = (string.IsNullOrEmpty(Request["txt8"])) ? "" : Request["txt8"].ToString().Trim();
            string txt9 = (string.IsNullOrEmpty(Request["txt9"])) ? "" : Request["txt9"].ToString().Trim();
            string txt10 = (string.IsNullOrEmpty(Request["txt10"])) ? "" : Request["txt10"].ToString().Trim();
            string txt11 = (string.IsNullOrEmpty(Request["txt11"])) ? "" : Request["txt11"].ToString().Trim();
            string txt12 = (string.IsNullOrEmpty(Request["txt12"])) ? "" : Request["txt12"].ToString().Trim();
            string txt13 = (string.IsNullOrEmpty(Request["txt13"])) ? "" : Request["txt13"].ToString().Trim();
            string txt14 = (string.IsNullOrEmpty(Request["txt14"])) ? "" : Request["txt14"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            odb._業者guid = cp;
            odb._年度 = Server.UrlDecode(year);
            odb._長途管線識別碼 = Server.UrlDecode(txt1);
            odb._最近一次執行日期 = Server.UrlDecode(txt2_1) + "/" + Server.UrlDecode(txt2_2);
            odb._再評估時機 = Server.UrlDecode(txt3);
            odb._管線長度 = Server.UrlDecode(txt4);
            odb._分段數量 = Server.UrlDecode(txt5);
            odb._已納入ILI結果 = Server.UrlDecode(txt6);
            odb._已納入CIPS結果 = Server.UrlDecode(txt7);
            odb._已納入巡管結果 = Server.UrlDecode(txt8);
            odb._各等級風險管段數量_高 = Server.UrlDecode(txt9);
            odb._各等級風險管段數量_中 = Server.UrlDecode(txt10);
            odb._各等級風險管段數量_低 = Server.UrlDecode(txt11);
            odb._文件名稱 = Server.UrlDecode(txt12);
            odb._改善後風險等級 = Server.UrlDecode(txt13);
            odb._備註 = Server.UrlDecode(txt14);
            odb._修改者 = LogInfo.mGuid;
            odb._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                odb._建立者 = LogInfo.mGuid;
                odb._建立日期 = DateTime.Now;

                odb.InsertData(oConn, myTrans);
            }
            else
            {
                odb._guid = guid;
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