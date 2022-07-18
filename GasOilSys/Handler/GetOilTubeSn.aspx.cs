using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetOilTubeSn : System.Web.UI.Page
{
    OilTubeInfo_DB db = new OilTubeInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢管線基本資料
        ///說    明:
        /// * Request["sn"]: 管線識別碼 
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string sn = (string.IsNullOrEmpty(Request["sn"])) ? "" : Request["sn"].ToString().Trim();

            db._長途管線識別碼 = sn;

            DataTable dt = db.GetData2();
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