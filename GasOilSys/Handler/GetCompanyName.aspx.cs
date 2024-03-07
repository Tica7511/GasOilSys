using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetCompanyName : System.Web.UI.Page
{
	GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
	OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢事業名稱(麵包屑)
		///說    明:
		/// * Request["cpid"]: 業者Guid 
		/// * Request["type"]: 網站類別(Oil:石油頁面 Gas: 天然氣頁面) 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? LogInfo.companyGuid : Request["cpid"].ToString().Trim();
			string type = (string.IsNullOrEmpty(Request["type"])) ? LogInfo.companyGuid : Request["type"].ToString().Trim();
			string cpname = string.Empty;
            DataTable dt = new DataTable();
			DataTable dt2 = new DataTable();
			DataTable dt3 = new DataTable();

            if(type == "Gas")
            {
                gdb._guid = cpid;
                dt = gdb.GetCpName();
                if (dt.Rows.Count > 0)
                {
					for(int i = 0; i < dt.Rows.Count; i++)
                    {
						if (dt.Rows[i]["單獨公司名稱"].ToString().Trim() == "Y")
							dt.Rows[i]["cpname"] = dt.Rows[i]["公司名稱"].ToString().Trim();
					}					

                    dt.Columns.Add("competence", typeof(string));
                    dt.Rows[0]["competence"] = LogInfo.competence;
                }

                if (!string.IsNullOrEmpty(cpid))
                {
					dt2 = gdb.GetCpNameToNcree();
					if (dt2.Rows.Count > 0)
					{
						for (int i = 0; i < dt.Rows.Count; i++)
						{
							cpname = dt2.Rows[i]["cpname"].ToString().Trim();
							if (dt2.Rows[i]["cpname"].ToString().Trim() == "天然氣處理廠(原天然氣處理廠-錦水區)")
								cpname = "天然氣處理廠";

							dt3 = gdb.GetCpNameToNcreeList(cpname);
						}

					}
				}				
			}
            else
            {
                odb._guid = cpid;
                dt = odb.GetCpName();
                if (dt.Rows.Count > 0)
                {
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						if (dt.Rows[i]["單獨公司名稱"].ToString().Trim() == "Y")
							dt.Rows[i]["cpname"] = dt.Rows[i]["公司名稱"].ToString().Trim();
					}

					dt.Columns.Add("competence", typeof(string));
                    dt.Rows[0]["competence"] = LogInfo.competence;
                }

                if (!string.IsNullOrEmpty(cpid))
                {
					dt2 = odb.GetCpNameToNcree();
					if (dt2.Rows.Count > 0)
					{
						for (int i = 0; i < dt.Rows.Count; i++)
						{
							if (dt2.Rows[i]["cpname"].ToString().Trim() == "深澳港供輸中心")
								dt2.Rows[i]["cpname"] = "深澳港輸中心";

							dt3 = odb.GetCpNameToNcreeList(dt2.Rows[i]["cpname"].ToString().Trim());
						}

					}
				}
			}                

            string xmlstr = string.Empty;
			string xmlstr2 = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt3, "dataList2", "data_item2");
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