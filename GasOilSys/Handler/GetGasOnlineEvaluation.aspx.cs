using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetGasOnlineEvaluation : System.Web.UI.Page
{
    GasOnlineEvaluation_DB db = new GasOnlineEvaluation_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢線上查核
		///說    明:
		/// * Request["cpid"]: 業者Guid 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? LogInfo.companyGuid : Request["cpid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? LogInfo.companyGuid : Request["year"].ToString().Trim();

            db._業者guid = cpid;
            db._年度 = year;
            DataTable dt = db.GetList("1");
            DataTable dt2 = db.GetList("2");
            DataTable dt3 = db.GetList("3");
            DataTable dt4 = db.GetList("4");
            DataTable dt5 = db.GetList("5");
            DataTable dt6 = db.GetYearList();
            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;
            string xmlstr3 = string.Empty;
            string xmlstr4 = string.Empty;
            string xmlstr5 = string.Empty;
            string xmlstr6 = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
            xmlstr3 = DataTableToXml.ConvertDatatableToXML(dt3, "dataList3", "data_item3");
            xmlstr4 = DataTableToXml.ConvertDatatableToXML(dt4, "dataList4", "data_item4");
            xmlstr5 = DataTableToXml.ConvertDatatableToXML(dt5, "dataList5", "data_item5");
            xmlstr6 = DataTableToXml.ConvertDatatableToXML(dt6, "dataList6", "data_item6");
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