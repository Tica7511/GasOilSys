﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetGasTubeCheckPlanAndResult : System.Web.UI.Page
{
	GasTubeCheckPlanAndResult_DB db = new GasTubeCheckPlanAndResult_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢用戶管線定期檢查計畫及檢查結果
		///說    明:
		/// * Request["cpid"]: 業者Guid 
		/// * Request["year"]: 年度 
		/// * Request["type"]: list=列表 data=資料列 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? LogInfo.companyGuid : Request["cpid"].ToString().Trim();
			string year = (string.IsNullOrEmpty(Request["year"])) ? LogInfo.companyGuid : Request["year"].ToString().Trim();
			string guid = (string.IsNullOrEmpty(Request["guid"])) ? LogInfo.companyGuid : Request["guid"].ToString().Trim();
			string type = (string.IsNullOrEmpty(Request["type"])) ? LogInfo.companyGuid : Request["type"].ToString().Trim();

			if (type == "list")
			{
				db._業者guid = cpid;
				db._年度 = year;

				DataTable dt = db.GetList();
				DataSet dt2 = db.GetList2();
				DataTable dt3 = db.GetYearList();
				string xmlstr = string.Empty;
				string xmlstr2 = string.Empty;
				string xmlstr3 = string.Empty;
				string totalxml = "<total>" + dt2.Tables[0].Rows[0]["total"].ToString() + "</total>";

				xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
				xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2.Tables[1], "dataList2", "data_item2");
				xmlstr3 = DataTableToXml.ConvertDatatableToXML(dt3, "dataList3", "data_item3");
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + xmlstr2 + xmlstr3 + "</root>";
				xDoc.LoadXml(xmlstr);
			}
			else
			{
				db._guid = guid;

				DataTable dt = new DataTable();

				dt = db.GetData();

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