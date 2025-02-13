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

public partial class Handler_AddGasControlFile : System.Web.UI.Page
{
    GasControl_DB gdb = new GasControl_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 控制室 依據文件資料
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["type"]: 01=control 02=stress 03=pipe
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 依據文件名稱
        /// * Request["txt2"]: 文件編號		 
        /// * Request["txt3"]: 文件日期	 
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
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
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
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode(year);
            gdb._依據文件名稱 = Server.UrlDecode(txt1);
            gdb._文件編號 = Server.UrlDecode(txt2);
            gdb._文件日期 = Server.UrlDecode(txt3);
            gdb._修改者 = LogInfo.mGuid;
            gdb._修改日期 = DateTime.Now;

            //壓力計及流量計資料
            gdb._管線識別碼 = Server.UrlDecode(txt1);
            gdb._自有端是否有設置壓力計 = Server.UrlDecode(txt2);
            gdb._壓力計校正週期 = Server.UrlDecode(txt3);
            gdb._壓力計最近一次校正日期 = Server.UrlDecode(txt4);
            gdb._壓力計最近一次校正結果 = Server.UrlDecode(txt5);
            gdb._自有端是否有設置流量計 = Server.UrlDecode(txt6);
            gdb._流量計型式 = Server.UrlDecode(txt7);
            gdb._流量計最小精度 = Server.UrlDecode(txt8);
            gdb._流量計校正週期 = Server.UrlDecode(txt9);
            gdb._流量計最近一次校正日期 = Server.UrlDecode(txt10);
            gdb._流量計最近一次校正結果 = Server.UrlDecode(txt11);

            //管線輸送接受資料
            gdb._負責泵送或接收之控制室名稱 = Server.UrlDecode(txt2);
            gdb._操作壓力 = Server.UrlDecode(txt3);
            gdb._歷史操作壓力變動範圍 = Server.UrlDecode(txt4);
            gdb._壓力計警報設定值 = Server.UrlDecode(txt5);
            gdb._流量計警報設定值 = Server.UrlDecode(txt6);
            gdb._前一年度警報發生頻率 = Server.UrlDecode(txt7);

            if (Server.UrlDecode(mode) == "new")
            {
                gdb._建立者 = LogInfo.mGuid;
                gdb._建立日期 = DateTime.Now;

                switch (type)
                {
                    case "01":
                        gdb.InsertData2(oConn, myTrans);
                        break;
                    case "02":
                        gdb.InsertDataStress(oConn, myTrans);
                        break;
                    case "03":
                        gdb.InsertDataPipe(oConn, myTrans);
                        break;
                }
            }
            else
            {
                gdb._guid = guid;

                switch (type)
                {
                    case "01":
                        gdb.UpdateData2(oConn, myTrans);
                        break;
                    case "02":
                        gdb.UpdateDataStress(oConn, myTrans);
                        break;
                    case "03":
                        gdb.UpdateDataPipe(oConn, myTrans);
                        break;
                }
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