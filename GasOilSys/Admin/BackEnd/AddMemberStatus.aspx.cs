using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Admin_BackEnd_AddMemberStatus : System.Web.UI.Page
{
    Member_DB db = new Member_DB();
    MemberLog_DB ml_db = new MemberLog_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 更新 人員資料狀態
		///說    明:
		/// * Request["status"]: status
		/// * Request["guid"]: guid
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            #region 檢查登入資訊
            if (LogInfo.account == "")
            {
                throw new Exception("請重新登入");
            }
            #endregion

            string status = (string.IsNullOrEmpty(Request["status"])) ? "" : Request["status"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string statusName = string.Empty;

            string xmlstr = string.Empty;
            db._資料狀態 = status;
            db._guid = guid;
            db._修改者 = LogInfo.mGuid;
            db.UpdateData();

            if (status == "A")
                statusName = "啟用";
            else
                statusName = "停用";

            // Member Log
            ml_db._會員guid = guid;
            ml_db._修改類別 = statusName;
            ml_db._IP = Common.GetIP4Address();
            ml_db._建立者 = LogInfo.mGuid;
            ml_db._修改者 = LogInfo.mGuid;
            ml_db.addLog();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>" + statusName + "成功</Response></root>";
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