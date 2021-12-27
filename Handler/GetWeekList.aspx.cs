using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetWeekList : System.Web.UI.Page
{
    Week_DB db = new Week_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢週報列表
		///說    明:
        ///
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            DataSet ds = db.GetListThisWeek();
            DataSet ds2 = db.GetListNextWeek();
            string daystr = string.Empty;
            string daystr2 = string.Empty;
            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;
            daystr = "<ThisMonday>" + ds.Tables[0].Rows[0]["星期一"].ToString() + "</ThisMonday><ThisSunday>" + ds.Tables[0].Rows[0]["星期日"].ToString() + "</ThisSunday>";
            xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
            daystr2 = "<NextMonday>" + ds2.Tables[0].Rows[0]["星期一"].ToString() + "</NextMonday><NextSunday>" + ds2.Tables[0].Rows[0]["星期日"].ToString() + "</NextSunday>";
            xmlstr2 = DataTableToXml.ConvertDatatableToXML(ds2.Tables[1], "dataList2", "data_item2");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + daystr + xmlstr + daystr2 + xmlstr2 + "</root>";
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