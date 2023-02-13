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

public partial class Handler_AddGasCheckSmartTubeCleaner : System.Web.UI.Page
{
    GasCheckSmartTubeCleaner_DB gdb = new GasCheckSmartTubeCleaner_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 智慧型通管器檢查表
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 長途管線識別碼
        /// * Request["txt2"]: 檢測方法 
        /// * Request["txt3_1"]: 最近一次執行 年份 
        /// * Request["txt3_2"]: 最近一次執行 月份
        /// * Request["txt4_1"]: 報告產出 年份
        /// * Request["txt4_2"]:報告產出 月份
        /// * Request["txt5"]: 檢測長度
        /// * Request["txt6"]: 管壁減薄30%-40%數量 內部腐蝕數量
        /// * Request["txt7"]: 管壁減薄30%-40%數量 內部開挖確認數量
        /// * Request["txt8"]: 管壁減薄30%-40%數量 外部腐蝕數量
        /// * Request["txt9"]: 管壁減薄30%-40%數量 外部開挖確認數量
        /// * Request["txt10"]: 管壁減薄40%-50%數量 內部腐蝕數量
        /// * Request["txt11"]: 管壁減薄40%-50%數量 內部開挖確認數量
        /// * Request["txt12"]: 管壁減薄40%-50%數量 外部腐蝕數量
        /// * Request["txt13"]: 管壁減薄40%-50%數量 外部開挖確認數量
        /// * Request["txt14"]: 管壁減薄50%以上數量 內部腐蝕數量
        /// * Request["txt15"]: 管壁減薄50%以上數量 內部開挖確認數量
        /// * Request["txt16"]: 管壁減薄50%以上數量 外部腐蝕數量
        /// * Request["txt17"]: 管壁減薄50%以上數量 外部開挖確認數量
        /// * Request["txt18"]: Dent 變形量>12%數量
        /// * Request["txt19"]: Dent 開挖確認數量
        /// * Request["txt20"]: 備註
        /// * Request["txt21"]: 外部腐蝕保護電位符合標準要求數量
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
            string txt3_1 = (string.IsNullOrEmpty(Request["txt3_1"])) ? "" : Request["txt3_1"].ToString().Trim();
            string txt3_2 = (string.IsNullOrEmpty(Request["txt3_2"])) ? "" : Request["txt3_2"].ToString().Trim();
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
            string txt15 = (string.IsNullOrEmpty(Request["txt15"])) ? "" : Request["txt15"].ToString().Trim();
            string txt16 = (string.IsNullOrEmpty(Request["txt16"])) ? "" : Request["txt16"].ToString().Trim();
            string txt17 = (string.IsNullOrEmpty(Request["txt17"])) ? "" : Request["txt17"].ToString().Trim();
            string txt18 = (string.IsNullOrEmpty(Request["txt18"])) ? "" : Request["txt18"].ToString().Trim();
            string txt19 = (string.IsNullOrEmpty(Request["txt19"])) ? "" : Request["txt19"].ToString().Trim();
            string txt20 = (string.IsNullOrEmpty(Request["txt20"])) ? "" : Request["txt20"].ToString().Trim();
            string txt21 = (string.IsNullOrEmpty(Request["txt21"])) ? "" : Request["txt21"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode(year);
            gdb._長途管線識別碼 = Server.UrlDecode(txt1);
            gdb._檢測方法 = Server.UrlDecode(txt2);
            gdb._最近一次執行年月 = Server.UrlDecode(txt3_1) + "/" + Server.UrlDecode(txt3_2);
            gdb._報告產出年月 = Server.UrlDecode(txt4_1) + "/" + Server.UrlDecode(txt4_2);
            gdb._檢測長度 = Server.UrlDecode(txt5);
            gdb._減薄3040數量_內 = Server.UrlDecode(txt6);
            gdb._減薄3040數量_內開挖確認 = Server.UrlDecode(txt7);
            gdb._減薄3040數量_外 = Server.UrlDecode(txt8);
            gdb._減薄3040數量_外開挖確認 = Server.UrlDecode(txt9);
            gdb._減薄4050數量_內 = Server.UrlDecode(txt10);
            gdb._減薄4050數量_內開挖確認 = Server.UrlDecode(txt11);
            gdb._減薄4050數量_外 = Server.UrlDecode(txt12);
            gdb._減薄4050數量_外開挖確認 = Server.UrlDecode(txt13);
            gdb._減薄50以上數量_內 = Server.UrlDecode(txt14);
            gdb._減薄50以上數量_內開挖確認 = Server.UrlDecode(txt15);
            gdb._減薄50以上數量_外 = Server.UrlDecode(txt16);
            gdb._減薄50以上數量_外開挖確認 = Server.UrlDecode(txt17);
            gdb._Dent_大於12 = Server.UrlDecode(txt18);
            gdb._Dent_開挖確認 = Server.UrlDecode(txt19);
            gdb._備註 = Server.UrlDecode(txt20);
            gdb._外部腐蝕保護電位符合標準要求數量 = Server.UrlDecode(txt21);
            gdb._修改者 = LogInfo.mGuid;
            gdb._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                gdb._建立者 = LogInfo.mGuid;
                gdb._建立日期 = DateTime.Now;

                gdb.InsertData(oConn, myTrans);
            }
            else
            {
                gdb._guid = guid;
                gdb.UpdateData(oConn, myTrans);
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