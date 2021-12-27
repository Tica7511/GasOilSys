using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_ModPwd : System.Web.UI.Page
{
    MemberPwdLog_DB mpl_db = new MemberPwdLog_DB();
    MemberLog_DB ml_db = new MemberLog_DB();
    Member_DB mdb = new Member_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 修改密碼
		///說    明:
		/// * Request["pw"]: 密碼
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string pw = (string.IsNullOrEmpty(Request["pw"])) ? "" : Request["pw"].ToString().Trim();

            string xmlstr = string.Empty;

            #region 確認密碼是否與前三次相同
            mpl_db._會員guid = LogInfo.mGuid;
            mpl_db._修改後密碼 = Server.UrlDecode(pw);
            DataTable dt = mpl_db.CheckRepeatPw();
            if (dt.Rows.Count > 0)
                throw new Exception("密碼不得與前三次相同");
            #endregion

            mdb._guid = LogInfo.mGuid;
            mdb._使用者密碼 = Server.UrlDecode(pw);
            mdb._修改者 = LogInfo.mGuid;
            mdb.UpdatePwd();

            #region Log 
            // Member Log
            ml_db._會員guid = LogInfo.mGuid;
            ml_db._修改類別 = "修改";
            ml_db._IP = Common.GetIP4Address();
            ml_db._建立者 = LogInfo.mGuid;
            ml_db._修改者 = LogInfo.mGuid;
            ml_db.addLog();

            // Member PassWord Log
            mpl_db._會員guid = LogInfo.mGuid;
            mpl_db._修改後密碼 = Server.UrlDecode(pw);
            mpl_db._IP = Common.GetIP4Address();
            mpl_db._建立者 = LogInfo.mGuid;
            mpl_db._修改者 = LogInfo.mGuid;
            mpl_db.addLog();
            #endregion

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>修改成功! 將轉往登入頁面 請重新登入</Response></root>";
            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}