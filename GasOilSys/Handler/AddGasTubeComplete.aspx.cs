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

public partial class Handler_AddGasTubeComplete : System.Web.UI.Page
{
    GasTubeComplete_DB gdb = new GasTubeComplete_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 管線完整性管理作為
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["no"]: 1=幹線及環線管線 2=幹線及環線管線以外
        /// * Request["txt1"]: 長途管線識別碼
        /// * Request["txt2_1"]: 風險評估 年份
        /// * Request["txt2_2"]: 風險評估 月份
        /// * Request["txt3"]: 智慧型通管器ILI可行性
        /// * Request["txt4"]: 耐壓強度試驗TP可行性	
        /// * Request["txt5_1"]: 緊密電位CIPS 年份	
        /// * Request["txt5_2"]: 緊密電位CIPS 月份	
        /// * Request["txt6_1"]: 電磁包覆PCM 年份	
        /// * Request["txt6_2"]: 電磁包覆PCM 月份	
        /// * Request["txt7_1"]: 智慧型通管器ILI 年份	
        /// * Request["txt7_2"]: 智慧型通管器ILI 月份	
        /// * Request["txt8_1"]: 耐壓強度試驗TP 年份	
        /// * Request["txt8_2"]: 耐壓強度試驗TP 月份	
        /// * Request["txt9"]: 耐壓強度試驗TP介質	
        /// * Request["txt10"]: 試壓壓力與MOP壓力倍數	
        /// * Request["txt11"]: 耐壓強度試驗TP持壓時間	
        /// * Request["txt12"]: 受雜散電流影響	
        /// * Request["txt13"]: 洩漏偵測系統LLDS	
        /// * Request["txt14"]: 強化作為	
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
            string no = (string.IsNullOrEmpty(Request["no"])) ? "" : Request["no"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string txt2_1 = (string.IsNullOrEmpty(Request["txt2_1"])) ? "" : Request["txt2_1"].ToString().Trim();
            string txt2_2 = (string.IsNullOrEmpty(Request["txt2_2"])) ? "" : Request["txt2_2"].ToString().Trim();
            string txt3 = (string.IsNullOrEmpty(Request["txt3"])) ? "" : Request["txt3"].ToString().Trim();
            string txt4 = (string.IsNullOrEmpty(Request["txt4"])) ? "" : Request["txt4"].ToString().Trim();
            string txt5_1 = (string.IsNullOrEmpty(Request["txt5_1"])) ? "" : Request["txt5_1"].ToString().Trim();
            string txt5_2 = (string.IsNullOrEmpty(Request["txt5_2"])) ? "" : Request["txt5_2"].ToString().Trim();
            string txt6_1 = (string.IsNullOrEmpty(Request["txt6_1"])) ? "" : Request["txt6_1"].ToString().Trim();
            string txt6_2 = (string.IsNullOrEmpty(Request["txt6_2"])) ? "" : Request["txt6_2"].ToString().Trim();
            string txt7_1 = (string.IsNullOrEmpty(Request["txt7_1"])) ? "" : Request["txt7_1"].ToString().Trim();
            string txt7_2 = (string.IsNullOrEmpty(Request["txt7_2"])) ? "" : Request["txt7_2"].ToString().Trim();
            string txt8_1 = (string.IsNullOrEmpty(Request["txt8_1"])) ? "" : Request["txt8_1"].ToString().Trim();
            string txt8_2 = (string.IsNullOrEmpty(Request["txt8_2"])) ? "" : Request["txt8_2"].ToString().Trim();
            string txt9 = (string.IsNullOrEmpty(Request["txt9"])) ? "" : Request["txt9"].ToString().Trim();
            string txt10 = (string.IsNullOrEmpty(Request["txt10"])) ? "" : Request["txt10"].ToString().Trim();
            string txt11 = (string.IsNullOrEmpty(Request["txt11"])) ? "" : Request["txt11"].ToString().Trim();
            string txt12 = (string.IsNullOrEmpty(Request["txt12"])) ? "" : Request["txt12"].ToString().Trim();
            string txt13 = (string.IsNullOrEmpty(Request["txt13"])) ? "" : Request["txt13"].ToString().Trim();
            string txt14 = (string.IsNullOrEmpty(Request["txt14"])) ? "" : Request["txt14"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode(year);
            gdb._長途管線識別碼 = Server.UrlDecode(txt1);
            gdb._風險評估年月 = Server.UrlDecode(txt2_1) + "/" + Server.UrlDecode(txt2_2);
            gdb._智慧型通管器ILI可行性 = Server.UrlDecode(txt3);
            gdb._耐壓強度試驗TP可行性 = Server.UrlDecode(txt4);
            gdb._緊密電位CIPS年月 = Server.UrlDecode(txt5_1) + "/" + Server.UrlDecode(txt5_2);
            gdb._電磁包覆PCM年月 = Server.UrlDecode(txt6_1) + "/" + Server.UrlDecode(txt6_2);
            gdb._智慧型通管器ILI年月 = Server.UrlDecode(txt7_1) + "/" + Server.UrlDecode(txt7_2);
            gdb._耐壓強度試驗TP年月 = Server.UrlDecode(txt8_1) + "/" + Server.UrlDecode(txt8_2);
            gdb._耐壓強度試驗TP介質 = Server.UrlDecode(txt9);
            gdb._試壓壓力與MOP壓力倍數 = Server.UrlDecode(txt10);
            gdb._耐壓強度試驗TP持壓時間 = Server.UrlDecode(txt11);
            gdb._受雜散電流影響 = Server.UrlDecode(txt12);
            gdb._洩漏偵測系統LLDS = Server.UrlDecode(txt13);
            gdb._強化作為 = Server.UrlDecode(txt14);
            gdb._修改者 = LogInfo.mGuid;
            gdb._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                gdb._建立者 = LogInfo.mGuid;
                gdb._建立日期 = DateTime.Now;

                if (no == "1")
                    gdb.InsertData(oConn, myTrans);
                else
                    gdb.InsertData_Out(oConn, myTrans);
            }
            else
            {
                gdb._guid = guid;

                if (no == "1")
                    gdb.UpdateData(oConn, myTrans);
                else
                    gdb.UpdateData_Out(oConn, myTrans);
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