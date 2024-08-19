using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_CheckLoginStatus : System.Web.UI.Page
{
    LoginLog_DB db = new LoginLog_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 確認是否有相同帳號登入
		///說    明:
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string isLog = string.Empty;
            db._帳號 = LogInfo.account;

            DataTable dt = db.GetData();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["登入IP"].ToString().Trim() == Common.GetIP4Address())
                {
                    isLog = "N";
                }
                else
                {
                    isLog = "Y";
                }
            }
            else
            {
                isLog = "Y";
            }


            string xmlstr = string.Empty;

            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>" + isLog + "</Response></root>";
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