using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Handler_GetOilCompanyList : System.Web.UI.Page
{
	OilCompanyInfo_DB db = new OilCompanyInfo_DB();
    OilMasterCompare_DB mc_db = new OilMasterCompare_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢石油業者清單
		///說    明:
		/// * Request[""]: 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
            //string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Request["SearchStr"].ToString().Trim();

            string mGuid = string.Empty;
            if (LogInfo.competence == "01")
                mGuid = LogInfo.mGuid;

            DataTable dt = new DataTable();

            if(LogInfo.competence == "05")
            {
                dt = db.Get05CompanyList();
            }
            else if(LogInfo.competence == "06")
            {
                dt = db.Get06CompanyList();
            }
            else
            {
                dt = db.GetCompanyList(mGuid);
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