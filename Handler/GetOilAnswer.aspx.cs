using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Handler_GetOilAnswer : System.Web.UI.Page
{
	OilEvaluationAnswer_DB db = new OilEvaluationAnswer_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢自評表答案
		///說    明:
		/// * Request["cpid"]: 業者Guid 
		/// * Request["year"]: 年度 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();

            db._業者guid = cpid;
			db._年度 = year;
			DataTable dt = db.GetCompanyAns();
			string xmlstr = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXmlByAttribute(dt, "dataList", "data_item");
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