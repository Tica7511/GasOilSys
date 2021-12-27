using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetDDLlist : System.Web.UI.Page
{
    CodeTable_DB db = new CodeTable_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢代碼檔列表
		///說    明:
        /// * Request["gNo"]: 群組代碼
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        DataTable dt = new DataTable();
        try
        {
            string gNo = (string.IsNullOrEmpty(Request["gNo"])) ? LogInfo.companyGuid : Request["gNo"].ToString().Trim();

            db._群組代碼 = gNo;
            dt = db.GetList();

            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
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