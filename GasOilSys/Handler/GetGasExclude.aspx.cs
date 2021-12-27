using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetGasExclude : System.Web.UI.Page
{
	GasCompanyExclude_DB db = new GasCompanyExclude_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢天然氣自評表業者排除題目
		///說    明:
		/// * Request[""]: 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			if (LogInfo.competence == "01")
			{

			}
			else if (LogInfo.competence == "02")
			{
				db._年份 = "110";
				db._業者guid = LogInfo.companyGuid;
				DataTable dt = db.GetList();
				string xmlstr = string.Empty;
				xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
				xDoc.LoadXml(xmlstr);
			}
		}
		catch (Exception ex)
		{
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}
}