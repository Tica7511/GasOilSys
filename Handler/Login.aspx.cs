using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Manage_BackEnd_Login : System.Web.UI.Page
{
    LoginLog_DB ldb = new LoginLog_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 登入
		///說    明:
		/// * Request["acc"]: 帳號
		/// * Request["pw"]: 密碼
		/// * Request["vCode"]: 驗證碼
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		string acc = (string.IsNullOrEmpty(Request["acc"])) ? "" : Request["acc"].ToString().Trim();
		string pw = (string.IsNullOrEmpty(Request["pw"])) ? "" : Request["pw"].ToString().Trim();
		string vCode = (string.IsNullOrEmpty(Request["vCode"])) ? "" : Request["vCode"].ToString().Trim();
        string reStatus = string.Empty;

        try
		{
			#region 檢查驗証碼
			if (Session["ValidateNumber"] != null)
			{
				if (Session["ValidateNumber"].ToString() != vCode.ToUpper())
				{
					throw new Exception("驗證碼輸入錯誤");
				}
			}
            #endregion

            #region 登入失敗驗證
            DataTable lsDt = ldb.CheckLoginStatus(acc);
            if (lsDt.Rows.Count > 0)
            {
                reStatus = lsDt.Rows[0]["reStatus"].ToString();
                if (reStatus == "X")
                    throw new Exception("登入失敗累計三次，帳號15分鐘內不可使用");
            }
            #endregion

            string xmlstr = string.Empty;
			Account.ExecSignIn(acc, Server.UrlDecode(pw));

            #region Log
            ldb._帳號 = acc;
            ldb._登入結果 = "Success";
            ldb._登入IP = Common.GetIP4Address();
            ldb._建立者 = LogInfo.mGuid;
            ldb._修改者 = LogInfo.mGuid;
            ldb.addLog();
            #endregion

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Success</Response></root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
            // 帳號鎖定登入時,不進LOG
            if (reStatus != "X" && acc != "")
            {
                #region Log
                ldb._帳號 = acc;
                ldb._登入結果 = "Fail";
                ldb._登入IP = Common.GetIP4Address();
                ldb._建立者 = Common.GetIP4Address();
                ldb._修改者 = Common.GetIP4Address();
                ldb.addLog();
                #endregion
            }
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}
}