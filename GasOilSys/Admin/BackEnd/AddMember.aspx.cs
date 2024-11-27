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

public partial class Admin_BackEnd_AddMember : System.Web.UI.Page
{
    Member_DB mdb = new Member_DB();
    MemberLog_DB ml_db = new MemberLog_DB();
    MemberPwdLog_DB mpl_db = new MemberPwdLog_DB();
    CodeTable_DB c_db = new CodeTable_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 會員管理
        ///說    明:
        /// * Request["guid"]:  guid 
        /// * Request["txt1"]: 姓名 
        /// * Request["txt2"]: 帳號類別	
        /// * Request["txt3"]: 網站類別	
        /// * Request["txt4"]: 業者名稱	
        /// * Request["txt5"]: 帳號	
        /// * Request["txt6"]: 密碼 
        /// * Request["txt7"]: email 
        /// * Request["txt8"]: 電話 
        /// * Request["txt9"]: 單位名稱	
        /// * Request["mode"]: new 新增 / mod 修改 
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

            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
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
            string tmpGuid = (mode == "new") ? Guid.NewGuid().ToString("N") : guid;
            string sqlAccount = string.Empty;
            string sqlPassword = string.Empty;
            string xmlstr = string.Empty;

            mdb._guid = tmpGuid;

            mdb._姓名 = Server.UrlDecode(txt1);
            mdb._帳號類別 = Server.UrlDecode(txt2);

            c_db._群組代碼 = "001";
            c_db._項目代碼 = Server.UrlDecode(txt2);
            DataTable dt = c_db.GetList();

            if (dt.Rows.Count > 0)
                mdb._角色 = dt.Rows[0]["項目名稱"].ToString().Trim();

            mdb._網站類別 = Server.UrlDecode(txt3);
            mdb._業者guid = Server.UrlDecode(txt4);
            mdb._使用者帳號 = Server.UrlDecode(txt5);
            mdb._使用者密碼 = Server.UrlDecode(txt6);
            mdb._mail = Server.UrlDecode(txt7);
            mdb._電話 = Server.UrlDecode(txt8);
            mdb._單位名稱 = Server.UrlDecode(txt9);
            mdb._建立者 = LogInfo.mGuid;
            mdb._修改者 = LogInfo.mGuid;

            if (mode == "new")
            {
                #region 檢查帳號是否重複
                if (checkAcc(Server.UrlDecode(txt5)))
                    throw new Exception("此帳號已存在");
                #endregion

                mdb._guid = tmpGuid;
                mdb.addMember(oConn, myTrans);

                #region Log
                // Member Log
                ml_db._會員guid = tmpGuid;
                ml_db._修改類別 = "新增";
                ml_db._IP = Common.GetIP4Address();
                ml_db._建立者 = LogInfo.mGuid;
                ml_db._修改者 = LogInfo.mGuid;
                ml_db.addLog();

                // Member PassWord Log
                mpl_db._會員guid = tmpGuid;
                mpl_db._修改後密碼 = Server.UrlDecode(txt6);
                mpl_db._IP = Common.GetIP4Address();
                mpl_db._建立者 = LogInfo.mGuid;
                mpl_db._修改者 = LogInfo.mGuid;
                mpl_db.addLog();
                #endregion
            }
            else
            {
                mdb._guid = tmpGuid;

                DataTable mdt = mdb.GetData();

                if (mdt.Rows.Count > 0)
                {
                    sqlAccount = mdt.Rows[0]["使用者帳號"].ToString().Trim();
                    sqlPassword = mdt.Rows[0]["使用者密碼"].ToString().Trim();
                }

                mdb.UpdateMember(oConn, myTrans);

                #region Log
                // Member Log
                ml_db._會員guid = tmpGuid;
                ml_db._修改類別 = "修改";
                ml_db._IP = Common.GetIP4Address();
                ml_db._建立者 = LogInfo.mGuid;
                ml_db._修改者 = LogInfo.mGuid;
                ml_db.addLog();

                // Member PassWord Log
                mpl_db._會員guid = tmpGuid;
                //暫時註解不准更改密碼
                //mpl_db._修改後密碼 = Server.UrlDecode(txt6);
                mpl_db._IP = Common.GetIP4Address();
                mpl_db._建立者 = LogInfo.mGuid;
                mpl_db._修改者 = LogInfo.mGuid;
                mpl_db.addLog();
                #endregion
            }

            myTrans.Commit();

            if (mode == "new")
            {
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response><relogin>N</relogin></root>";
            }
            else
            {
                if (tmpGuid == LogInfo.mGuid)
                {
                    if ((Server.UrlDecode(txt5) != sqlAccount) || (Server.UrlDecode(txt6) != sqlPassword))
                    {
                        xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>修改成功! 將轉往登入頁面 請重新登入</Response><relogin>Y</relogin></root>";
                    }
                    else
                    {
                        xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response><relogin>N</relogin></root>";
                    }
                }
                else
                {
                    xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response><relogin>N</relogin></root>";
                }
            }

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

    private bool checkAcc(string acc)
    {
        bool status = false;
        mdb._使用者帳號 = acc;
        DataTable dt = mdb.CheckMember();
        if (dt.Rows.Count > 0)
            status = true;

        return status;
    }
}