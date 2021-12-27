using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetWeekReport_6_2 : System.Web.UI.Page
{
    WeekReportList_DB db = new WeekReportList_DB();
    WeekReport_6_2_DB db2 = new WeekReport_6_2_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢週報計劃書2.2.3
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
            DataTable dtA = db2.GetList1();
            DataTable dtB = db2.GetList2();
            DataTable dtC = db2.GetList3();
            DataTable dtD = db2.GetList4();
            DataTable dtE = db2.GetList5();
            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            string xmlstr2 = DataTableToXml.ConvertDatatableToXML(dtA, "dataList2", "data_item2");
            string xmlstr3 = DataTableToXml.ConvertDatatableToXML(dtB, "dataList3", "data_item3");
            string xmlstr4 = DataTableToXml.ConvertDatatableToXML(dtC, "dataList4", "data_item4");
            string xmlstr5 = DataTableToXml.ConvertDatatableToXML(dtD, "dataList5", "data_item5");
            string xmlstr6 = DataTableToXml.ConvertDatatableToXML(dtE, "dataList6", "data_item6");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + xmlstr3 + xmlstr4 + xmlstr5 + xmlstr6 + "</root>";
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