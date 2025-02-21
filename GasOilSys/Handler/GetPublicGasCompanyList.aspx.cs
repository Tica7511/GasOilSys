using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
public partial class Handler_GetPublicGasCompanyList : System.Web.UI.Page
{
    PublicGas_DB db = new PublicGas_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢 公用天然氣列表
		///說    明:
		/// * Request["year"]: 年份 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string year = (string.IsNullOrEmpty(Request["year"])) ? LogInfo.companyGuid : Request["year"].ToString().Trim();

            DataTable dt = db.GetCompanyList();
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