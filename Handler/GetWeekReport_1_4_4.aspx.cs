using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetWeekReport_1_4_4 : System.Web.UI.Page
{
    WeekReportList_DB db = new WeekReportList_DB();
    WeekReport_1_4_4_DB db2 = new WeekReport_1_4_4_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢週報計劃書1.4.4
		///說    明:
		/// * Request["rid"]: Guid 
		/// * Request["year"]: 年度 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string rid = (string.IsNullOrEmpty(Request["rid"])) ? LogInfo.companyGuid : Request["rid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? LogInfo.companyGuid : Request["year"].ToString().Trim();

            db._guid = rid;
            db._年份 = year;
            db2._年度 = year;
            DataTable dt = db.GetData();
            DataTable dt2 = db2.GetList();
            DataTable dt3 = db2.GetList2();
            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;
            string xmlstr3 = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
            xmlstr3 = DataTableToXml.ConvertDatatableToXML(dt3, "dataList3", "data_item3");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + xmlstr3 + "</root>";
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