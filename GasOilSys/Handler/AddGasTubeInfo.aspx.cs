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

public partial class Handler_AddGasTubeInfo : System.Web.UI.Page
{
    GasTubeInfo_DB gdb = new GasTubeInfo_DB();
    GasTubeEnvironment_DB gdb2 = new GasTubeEnvironment_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 管線基本資料
        ///說    明:
        /// * Request["cp"]:  業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 長途管線識別碼
        /// * Request["txt2"]: 轄區長途管線名稱
        /// * Request["txt3"]: 銜接管線識別碼_上游
        /// * Request["txt4"]: 銜接管線識別碼_下游
        /// * Request["txt5"]: 起點
        /// * Request["txt6"]: 迄點
        /// * Request["txt7"]: 管徑吋
        /// * Request["txt8"]: 厚度
        /// * Request["txt9"]: 管材
        /// * Request["txt10"]: 包覆材料
        /// * Request["txt11"]: 轄管長度
        /// * Request["txt12"]: 內容物
        /// * Request["txt13"]: 緊急遮斷閥
        /// * Request["txt14"]: 建置年
        /// * Request["txt15"]: 設計壓力
        /// * Request["txt16"]: 使用壓力
        /// * Request["txt17"]: 使用狀態
        /// * Request["txt18"]: 轄管長度
        /// * Request["txt19"]: 活動斷層敏感區
        /// * Request["txt20"]: 土壤液化區
        /// * Request["txt21"]: 土石流潛勢區
        /// * Request["txt22"]: 淹水潛勢區
        /// * Request["txt23"]: 管線穿越箱涵數量 
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
            string txt22 = (string.IsNullOrEmpty(Request["txt22"])) ? "" : Request["txt22"].ToString().Trim();
            string txt23 = (string.IsNullOrEmpty(Request["txt23"])) ? "" : Request["txt23"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode("110");
            gdb._長途管線識別碼 = Server.UrlDecode(txt1);
            gdb._轄區長途管線名稱_公司 = Server.UrlDecode(txt2);
            gdb._銜接管線識別碼_上游 = Server.UrlDecode(txt3);
            gdb._銜接管線識別碼_下游 = Server.UrlDecode(txt4);
            gdb._起點 = Server.UrlDecode(txt5);
            gdb._迄點 = Server.UrlDecode(txt6);
            gdb._管徑 = Server.UrlDecode(txt7);
            gdb._厚度 = Server.UrlDecode(txt8);
            gdb._管材 = Server.UrlDecode(txt9);
            gdb._包覆材料 = Server.UrlDecode(txt10);
            gdb._轄管長度 = Server.UrlDecode(txt11);
            gdb._內容物 = Server.UrlDecode(txt12);
            gdb._緊急遮斷閥 = Server.UrlDecode(txt13);
            gdb._建置年 = Server.UrlDecode(txt14);
            gdb._設計壓力 = Server.UrlDecode(txt15);
            gdb._使用壓力 = Server.UrlDecode(txt16);
            gdb._使用狀態 = Server.UrlDecode(txt17);
            gdb._附掛橋樑數量 = Server.UrlDecode(txt18);
            gdb._管線穿越箱涵數量 = Server.UrlDecode(txt23);
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

            gdb2._業者guid = cp;
            gdb2._年度 = "110";
            gdb2._長途管線識別碼 = Server.UrlDecode(txt1);
            gdb2._轄區長途管線編號名稱公司 = Server.UrlDecode(txt2);
            gdb2._活動斷層敏感區 = Server.UrlDecode(txt19);
            gdb2._土壤液化區 = Server.UrlDecode(txt20);
            gdb2._土石流潛勢區 = Server.UrlDecode(txt21);
            gdb2._淹水潛勢區 = Server.UrlDecode(txt22);
            gdb2._修改者 = LogInfo.mGuid;
            gdb2._修改日期 = DateTime.Now;

            DataTable dt = gdb2.GetData2();

            if (dt.Rows.Count > 0)
            {
                gdb2.UpdateData(oConn, myTrans);
            }
            else
            {
                gdb2._建立者 = LogInfo.mGuid;
                gdb2._建立日期 = DateTime.Now;

                gdb2.InsertData(oConn, myTrans);
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