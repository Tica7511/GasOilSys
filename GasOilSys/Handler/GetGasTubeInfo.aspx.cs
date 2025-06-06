﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetGasTubeInfo : System.Web.UI.Page
{
	GasTubeInfo_DB db = new GasTubeInfo_DB();
	GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢管線基本資料
		///說    明:
		/// * Request["cpid"]: 業者Guid 
		/// * Request["year"]: 年度 
		/// * Request["type"]: list=列表 data=資料列 
		/// * Request["PageNo"]:欲顯示的頁碼, 由零開始
		/// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? LogInfo.companyGuid : Request["cpid"].ToString().Trim();
			string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
			string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
			string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
			string Sno = (string.IsNullOrEmpty(Request["Sno"])) ? "" : Request["Sno"].ToString().Trim();
			string KeyWord = (string.IsNullOrEmpty(Request["KeyWord"])) ? "" : Request["KeyWord"].ToString().Trim();
			string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
			int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;

			if (type == "list")
			{
				// 計算起始與結束
				int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
				int pageStart = pageEnd - PageSize + 1;

				db._業者guid = cpid;
				db._年度 = year;
				db._長途管線識別碼 = Sno;
				db._KeyWord = KeyWord;

				DataTable cdt = new DataTable();
				DataTable ndt = new DataTable();
				DataSet ds = db.GetList(pageStart.ToString(), pageEnd.ToString());
				DataTable dt = ds.Tables[1];
				if (dt.Rows.Count > 0)
				{
					dt.Columns.Add("HavePipe", typeof(string));
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						gdb._guid = dt.Rows[i]["業者guid"].ToString().Trim();
						cdt = gdb.GetCpNameToNcree();
						if (cdt.Rows.Count > 0)
						{
							if (cdt.Rows[0]["cpname"].ToString().Trim() == "天然氣處理廠(原天然氣處理廠-錦水區)")
								cdt.Rows[0]["cpname"] = "天然氣處理廠";

							ndt = db.GetNcreeData(cdt.Rows[0]["cpname"].ToString().Trim(), dt.Rows[i]["長途管線識別碼"].ToString().Trim());
							if (ndt.Rows.Count > 0)
							{
								dt.Rows[i]["HavePipe"] = "Y";
							}
						}
					}
				}
				DataTable dt2 = db.GetYearList();
				DataTable dt3 = db.GetList();
				string xmlstr = string.Empty;
				string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
				string xmlstr2 = string.Empty;
				string xmlstr3 = string.Empty;

				xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
				xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
				xmlstr3 = DataTableToXml.ConvertDatatableToXML(dt3, "dataList3", "data_item3");
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + xmlstr2 + xmlstr3 + "</root>";
				xDoc.LoadXml(xmlstr);
			}
			else
			{
				db._guid = guid;

				DataTable dt = db.GetData();
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