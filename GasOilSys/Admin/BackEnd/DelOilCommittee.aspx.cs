using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Admin_BackEnd_DelOilCommittee : System.Web.UI.Page
{
    OilMasterCompare_DB db = new OilMasterCompare_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除 石油委員對業者資料
		///說    明:
		/// * Request["year"]: 年度 
		/// * Request["cpguid"]: 業者guid 
		/// * Request["cguid"]: 委員guid 
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

            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string cpguid = (string.IsNullOrEmpty(Request["cpguid"])) ? "" : Request["cpguid"].ToString().Trim();
            string cguid = (string.IsNullOrEmpty(Request["cguid"])) ? "" : Request["cguid"].ToString().Trim();

            string xmlstr = string.Empty;
            db._年度 = year;
            db._業者guid = cpguid;
            db._委員guid = cguid;
            db._修改者 = LogInfo.mGuid;
            db.DeleteData();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>刪除成功</Response></root>";
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