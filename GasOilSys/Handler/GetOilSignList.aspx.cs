using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetOilSignList : System.Web.UI.Page
{
    Sign_DB db = new Sign_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢簽核列表
		///說    明:
        /// * Request["gNo"]: 群組代碼
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        DataTable dt = new DataTable();
        try
        {
            string signtype = (string.IsNullOrEmpty(Request["signtype"])) ? LogInfo.companyGuid : Request["signtype"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? LogInfo.companyGuid : Request["guid"].ToString().Trim();

            db._guid = guid;
            db._類型 = signtype;
            dt = db.GetList();

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