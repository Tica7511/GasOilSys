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
            DataTable dt = new DataTable();

            if(type == "Gas")
            {
                gdb._guid = cpid;
                dt = gdb.GetCpName();
                if (dt.Rows.Count > 0)
                {
					for(int i = 0; i < dt.Rows.Count; i++)
                    {
						if (dt.Rows[i]["guid"].ToString().Trim() == "9E779E2B-C36D-44BF-BED2-11C29D989D53")
							dt.Rows[i]["cpname"] = dt.Rows[i]["公司名稱"].ToString().Trim();
					}					

                    dt.Columns.Add("competence", typeof(string));
                    dt.Rows[0]["competence"] = LogInfo.competence;
                }
            }
            else
            {
                odb._guid = cpid;
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