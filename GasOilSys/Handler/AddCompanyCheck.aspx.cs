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

public partial class Handler_AddCompanyCheck : System.Web.UI.Page
{
    OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
    GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
    GasInfo_DB gidb = new GasInfo_DB();

    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 事故學習表
        ///說    明:
        /// * Request["cpid"]: 業者guid
        /// * Request["type"]: Oil=石油 Gas=天然氣 
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

            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Request["category"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string txt2 = (string.IsNullOrEmpty(Request["txt2"])) ? "" : Request["txt2"].ToString().Trim();
            string txt3 = (string.IsNullOrEmpty(Request["txt3"])) ? "" : Request["txt3"].ToString().Trim();
            string txt4 = (string.IsNullOrEmpty(Request["txt4"])) ? "" : Request["txt4"].ToString().Trim();
            string txt5 = (string.IsNullOrEmpty(Request["txt5"])) ? "" : Request["txt5"].ToString().Trim();
            string txt6 = (string.IsNullOrEmpty(Request["txt6"])) ? "" : Request["txt6"].ToString().Trim();
            string txt7 = (string.IsNullOrEmpty(Request["txt7"])) ? "" : Request["txt7"].ToString().Trim();
            string txt8 = (string.IsNullOrEmpty(Request["txt8"])) ? "" : Request["txt8"].ToString().Trim();
            string xmlstr = string.Empty;
            string Reponse = string.Empty;

            if(type == "Oil")
            {
                if(category == "confirm")
                {
                    Reponse = "資料確認完成";
                    odb._guid = cpid;
                    odb._資料是否確認 = "是";
                    odb._修改者 = LogInfo.mGuid;
                    odb._修改日期 = DateTime.Now;

                    odb.UpdateData(oConn, myTrans);

                    //odb._年度 = year;
                    //odb._年度查核姓名 = Server.UrlDecode(txt1);
                    //odb._年度查核職稱 = Server.UrlDecode(txt2);
                    //odb._年度查核分機 = Server.UrlDecode(txt3);
                    //odb._年度查核email = Server.UrlDecode(txt4);
                    //odb._年度檢測姓名 = Server.UrlDecode(txt5);
                    //odb._年度檢測職稱 = Server.UrlDecode(txt6);
                    //odb._年度檢測分機 = Server.UrlDecode(txt7);
                    //odb._年度檢測email = Server.UrlDecode(txt8);

                    //odb.UpdateCompanyInfoOnlyContact(oConn, myTrans);
                }
                else
                {
                    Reponse = "儲存完成";
                    odb._guid = cpid;
                    odb._年度 = year;
                    odb._年度查核姓名 = Server.UrlDecode(txt1);
                    odb._年度查核職稱 = Server.UrlDecode(txt2);
                    odb._年度查核分機 = Server.UrlDecode(txt3);
                    odb._年度查核email = Server.UrlDecode(txt4);
                    odb._年度檢測姓名 = Server.UrlDecode(txt5);
                    odb._年度檢測職稱 = Server.UrlDecode(txt6);
                    odb._年度檢測分機 = Server.UrlDecode(txt7);
                    odb._年度檢測email = Server.UrlDecode(txt8);
                    odb._修改者 = LogInfo.mGuid;
                    odb._修改日期 = DateTime.Now;

                    odb.UpdateCompanyInfoOnlyContact(oConn, myTrans);
                }                
            }
            else
            {
                if (category == "confirm")
                {
                    Reponse = "資料確認完成";
                    gdb._guid = cpid;
                    gdb._資料是否確認 = "是";
                    gdb._修改者 = LogInfo.mGuid;
                    gdb._修改日期 = DateTime.Now;

                    gdb.UpdateData(oConn, myTrans);

                    //gidb._年度 = year;
                    //gidb._業者guid = cpid;
                    //gidb._年度查核姓名 = Server.UrlDecode(txt1);
                    //gidb._年度查核職稱 = Server.UrlDecode(txt2);
                    //gidb._年度查核分機 = Server.UrlDecode(txt3);
                    //gidb._年度查核email = Server.UrlDecode(txt4);
                    //gidb._年度檢測姓名 = Server.UrlDecode(txt5);
                    //gidb._年度檢測職稱 = Server.UrlDecode(txt6);
                    //gidb._年度檢測分機 = Server.UrlDecode(txt7);
                    //gidb._年度檢測email = Server.UrlDecode(txt8);
                    //gidb._修改者 = LogInfo.mGuid;
                    //gidb._修改日期 = DateTime.Now;

                    //gidb.UpdateCompanyInfoOnlyContact(oConn, myTrans);
                }
                else
                {
                    Reponse = "儲存完成";
                    gidb._年度 = year;
                    gidb._業者guid = cpid;
                    gidb._年度查核姓名 = Server.UrlDecode(txt1);
                    gidb._年度查核職稱 = Server.UrlDecode(txt2);
                    gidb._年度查核分機 = Server.UrlDecode(txt3);
                    gidb._年度查核email = Server.UrlDecode(txt4);
                    gidb._年度檢測姓名 = Server.UrlDecode(txt5);
                    gidb._年度檢測職稱 = Server.UrlDecode(txt6);
                    gidb._年度檢測分機 = Server.UrlDecode(txt7);
                    gidb._年度檢測email = Server.UrlDecode(txt8);
                    gidb._修改者 = LogInfo.mGuid;
                    gidb._修改日期 = DateTime.Now;

                    gidb.UpdateCompanyInfoOnlyContact(oConn, myTrans);
                }                
            }

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>" + Reponse + "</Response><relogin>N</relogin></root>";

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