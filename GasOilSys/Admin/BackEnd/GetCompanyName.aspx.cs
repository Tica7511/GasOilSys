﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Admin_BackEnd_GetCompanyName : System.Web.UI.Page
{
	GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
	OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢事業名稱(麵包屑)
		///說    明:
		/// * Request["type"]: 網站類別(Oil:石油頁面 Gas: 天然氣頁面) 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string type = (string.IsNullOrEmpty(Request["type"])) ? LogInfo.companyGuid : Request["type"].ToString().Trim();
			DataTable dt = new DataTable();

			if (type == "Gas")
			{
				dt = gdb.GetCpName();
				if (dt.Rows.Count > 0)
				{
					dt.Columns.Add("competence", typeof(string));
					dt.Rows[0]["competence"] = LogInfo.competence;
				}
			}
			else
			{
				dt = odb.GetCpName();
				if (dt.Rows.Count > 0)
				{
					dt.Columns.Add("competence", typeof(string));
					dt.Rows[0]["competence"] = LogInfo.competence;
				}
			}

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