using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetOilOnlineEvaluation : System.Web.UI.Page
{
    OilOnlineEvaluation_DB db = new OilOnlineEvaluation_DB();
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

            db._業者guid = cpid;
            DataTable dt = db.GetList("1");
            DataTable dt2 = db.GetList("2");
            DataTable dt3 = db.GetList("3");
            DataTable dt4 = db.GetList("4");
            DataTable dt5 = db.GetList("5");
            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;
            string xmlstr3 = string.Empty;
            string xmlstr4 = string.Empty;
            string xmlstr5 = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
            xmlstr3 = DataTableToXml.ConvertDatatableToXML(dt3, "dataList3", "data_item3");
            xmlstr4 = DataTableToXml.ConvertDatatableToXML(dt4, "dataList4", "data_item4");
            xmlstr5 = DataTableToXml.ConvertDatatableToXML(dt5, "dataList5", "data_item5");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + xmlstr3 + xmlstr4 + xmlstr5 + "</root>";
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