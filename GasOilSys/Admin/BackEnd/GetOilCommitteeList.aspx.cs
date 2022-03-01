using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Admin_BackEnd_GetOilCommitteeList : System.Web.UI.Page
{
    OilMasterCompare_DB db = new OilMasterCompare_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢石油委員對應業者清單
        ///說    明:
        /// * Request["year"]: 年度
        /// * Request["cid"]: 業者guid
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string cid = (string.IsNullOrEmpty(Request["cid"])) ? "" : Request["cid"].ToString().Trim();

            db._年度 = year;
            db._業者guid = cid;

            DataTable dt = db.GetCommitteeList();
            DataTable dt2 = db.GetYearList();

            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
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