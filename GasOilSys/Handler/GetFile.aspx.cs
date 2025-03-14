using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetFile : System.Web.UI.Page
{
    FileTable db = new FileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢附件檔
		///說    明:
		/// * Request["cpid"]: 業者Guid 
        /// * Request["guid"]: 查核建議guid 
		/// * Request["type"]: 附件類別 
		/// * Request["year"]: 年份 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? LogInfo.companyGuid : Request["cpid"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? LogInfo.companyGuid : Request["guid"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? LogInfo.companyGuid : Request["type"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? LogInfo.companyGuid : Request["year"].ToString().Trim();
            string filetype = (string.IsNullOrEmpty(Request["filetype"])) ? LogInfo.companyGuid : Request["filetype"].ToString().Trim();

            db._guid = guid;
            db._業者guid = cpid;
            db._檔案類型 = type;
            db._年度 = year;

            DataTable dt = new DataTable();

            if (filetype == "list")
            {
                dt = db.GetList();
            }
            else {
                dt = db.GetData();
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