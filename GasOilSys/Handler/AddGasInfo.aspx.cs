using System;
using System.Globalization;
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

public partial class Handler_AddGasInfo : System.Web.UI.Page
{
    GasInfo_DB db = new GasInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 修改 天然氣業者基本資料
        ///說    明:
        /// * Request["cid"]: guid 
        /// * Request["txt1"]: 事業名稱
        /// * Request["txt2"]: 地址
        /// * Request["txt3"]: 電話
        /// * Request["txt4"]: 輸氣幹線
        /// * Request["txt5"]: 輸氣環線
        /// * Request["txt6"]: 配氣專管
        /// * Request["txt7"]: 場內成品線
        /// * Request["txt8"]: 海底管線
        /// * Request["txt9"]: LNG 管線
        /// * Request["txt10"]: BOG 管線
        /// * Request["txt11"]: NG 管線
        /// * Request["txt12"]: 供氣對象(縣市)
        /// * Request["supplygas"]: 供應天然氣
        /// * Request["txt13"]: 儲槽
        /// * Request["txt14"]: 注氣站
        /// * Request["txt15"]: 加壓站
        /// * Request["txt16"]: 配氣站
        /// * Request["txt17"]: 隔離站
        /// * Request["txt18"]: 開關站
        /// * Request["txt19"]: 清管站
        /// * Request["txt20"]: 整壓計量站
        /// * Request["txt21"]: 低壓排放塔
        /// * Request["txt22"]: 高壓排放塔
        /// * Request["txt23"]: NG2 熱值 調整摻配站
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

            string cid = (string.IsNullOrEmpty(Request["cid"])) ? "" : Request["cid"].ToString().Trim();
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

            db._業者guid = cid;
            db._事業名稱 = Server.UrlDecode(txt1);
            db._地址 = Server.UrlDecode(txt2);
            db._電話 = Server.UrlDecode(txt3);
            db._輸氣幹線 = Server.UrlDecode(txt4);
            db._輸氣環線 = Server.UrlDecode(txt5);
            db._配氣專管 = Server.UrlDecode(txt6);
            db._場內成品線 = Server.UrlDecode(txt7);
            db._海底管線 = Server.UrlDecode(txt8);
            db._LNG管線 = Server.UrlDecode(txt9);
            db._BOG管線 = Server.UrlDecode(txt10);
            db._NG管線 = Server.UrlDecode(txt11);
            db._供氣對象縣市 = Server.UrlDecode(txt12);
            db._供應天然氣 = string.IsNullOrEmpty(Request["supplygas"]) ? "" : Request["supplygas"].ToString().Trim();
            db._儲槽 = Server.UrlDecode(txt13);
            db._注氣站 = Server.UrlDecode(txt14);
            db._加壓站 = Server.UrlDecode(txt15);
            db._配氣站 = Server.UrlDecode(txt16);
            db._隔離站 = Server.UrlDecode(txt17);
            db._開關站 = Server.UrlDecode(txt18);
            db._清管站 = Server.UrlDecode(txt19);
            db._整壓計量站 = Server.UrlDecode(txt20);
            db._低壓排放塔 = Server.UrlDecode(txt21);
            db._高壓排放塔 = Server.UrlDecode(txt22);
            db._NG2摻配站 = Server.UrlDecode(txt23);
            db._修改者 = LogInfo.mGuid;
            db._修改日期 = DateTime.Now;

            db.UpdateData(oConn, myTrans);

            myTrans.Commit();

            string xmlstr = string.Empty;
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response></root>";
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