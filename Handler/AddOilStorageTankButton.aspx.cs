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

public partial class Handler_AddOilStorageTankButton : System.Web.UI.Page
{
    OilStorageTankButton_DB odb = new OilStorageTankButton_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 儲槽底板
        ///說    明:
        /// * Request["cp"]:  業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 轄區儲槽編號
        /// * Request["txt2"]:  執行MFL檢測
        /// * Request["txt3"]:  防蝕塗層
        /// * Request["txt4_1"]:  塗層全面重新施加日期 年份
        /// * Request["txt4_2"]:  塗層全面重新施加日期 月份
        /// * Request["txt5"]:  最近一次開放塗層維修情形
        /// * Request["txt6"]:  銲道腐蝕
        /// * Request["txt7"]:  局部變形
        /// * Request["txt8"]:  最近一次開放是否有維修
        /// * Request["txt9"]:  內容物側最小剩餘厚度 
        /// * Request["txt10"]: 內容物側最大腐蝕速率
        /// * Request["txt11"]: 土壤側最小剩餘厚度
        /// * Request["txt12"]: 土壤側最大腐蝕速率
        /// * Request["txt13"]: 是否有更換過底板
        /// * Request["txt14"]: 綜合判定
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
            string txt4_1 = (string.IsNullOrEmpty(Request["txt4_1"])) ? "" : Request["txt4_1"].ToString().Trim();
            string txt4_2 = (string.IsNullOrEmpty(Request["txt4_2"])) ? "" : Request["txt4_2"].ToString().Trim();
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
            odb._轄區儲槽編號 = Server.UrlDecode(txt1);
            odb._執行MFL檢測 = Server.UrlDecode(txt2);
            odb._防蝕塗層 = Server.UrlDecode(txt3);
            odb._塗層全面重新施加日期 = Server.UrlDecode(txt4_1) + "/" + Server.UrlDecode(txt4_2);
            odb._最近一次開放塗層維修情形 = Server.UrlDecode(txt5);
            odb._銲道腐蝕 = Server.UrlDecode(txt6);
            odb._局部變形 = Server.UrlDecode(txt7);
            odb._最近一次開放是否有維修 = Server.UrlDecode(txt8);
            odb._內容物側最小剩餘厚度 = Server.UrlDecode(txt9);
            odb._內容物側最大腐蝕速率 = Server.UrlDecode(txt10);
            odb._土壤側最小剩餘厚度 = Server.UrlDecode(txt11);
            odb._土壤側最大腐蝕速率 = Server.UrlDecode(txt12);
            odb._是否有更換過底板 = Server.UrlDecode(txt13);
            odb._綜合判定 = Server.UrlDecode(txt14);
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