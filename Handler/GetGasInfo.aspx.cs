using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetGasInfo : System.Web.UI.Page
{
	GasInfo_DB db = new GasInfo_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢天然氣業者基本資料
		///說    明:
		/// * Request["cpid"]: 業者Guid 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? LogInfo.companyGuid : Request["cpid"].ToString().Trim();

			db._年度 = "110";
			db._業者guid = cpid;
			DataTable dt = db.GetInfo();
			DataTable dt2 = db.GetList();
			string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + "</root>";
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