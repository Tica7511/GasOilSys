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

public partial class Handler_AddOilTubeInfo : System.Web.UI.Page
{
    OilTubeInfo_DB odb = new OilTubeInfo_DB();
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
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            odb._業者guid = cp;
            odb._年度 = Server.UrlDecode(year);
            odb._長途管線識別碼 = Server.UrlDecode(txt1);
            odb._轄區長途管線名稱 = Server.UrlDecode(txt2);
            odb._銜接管線識別碼_上游 = Server.UrlDecode(txt3);
            odb._銜接管線識別碼_下游 = Server.UrlDecode(txt4);
            odb._起點 = Server.UrlDecode(txt5);
            odb._迄點 = Server.UrlDecode(txt6);
            odb._管徑吋 = Server.UrlDecode(txt7);
            odb._厚度 = Server.UrlDecode(txt8);
            odb._管材 = Server.UrlDecode(txt9);
            odb._包覆材料 = Server.UrlDecode(txt10);
            odb._轄管長度 = Server.UrlDecode(txt11);
            odb._內容物 = Server.UrlDecode(txt12);
            odb._緊急遮斷閥 = Server.UrlDecode(txt13);
            odb._建置年 = Server.UrlDecode(txt14);
            odb._設計壓力 = Server.UrlDecode(txt15);
            odb._使用壓力 = Server.UrlDecode(txt16);
            odb._使用狀態 = Server.UrlDecode(txt17);
            odb._附掛橋樑數量 = Server.UrlDecode(txt18);
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