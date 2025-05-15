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
    OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
    GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
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
            DataTable dt2 = new DataTable();

            if (filetype == "list")
            {
                dt = db.GetList();
                dt2 = db.GetYearList();

                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("業者簡稱", typeof(string));

                    for (int i=0; i < dt.Rows.Count; i++)
                    {
                        if (type == "017")
                        {
                            odb._guid = dt.Rows[i]["業者guid"].ToString().Trim();
                            DataTable odt = odb.GetCpName();

                            if (odt.Rows.Count > 0)
                            {
                                dt.Rows[i]["業者簡稱"] = odt.Rows[0]["cpname"].ToString().Trim();
                            }
                        }
                        else if (type == "018")
                        {
                            gdb._guid = dt.Rows[i]["業者guid"].ToString().Trim();
                            DataTable gdt = gdb.GetCpName();

                            if (gdt.Rows.Count > 0)
                            {
                                dt.Rows[i]["業者簡稱"] = gdt.Rows[0]["cpname"].ToString().Trim();
                            }
                        }
                    }                    
                }

                string xmlstr = string.Empty;
                string xmlstr2 = string.Empty;

                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + "</root>";
                xDoc.LoadXml(xmlstr);
            }
            else {
                dt = db.GetData();
                string xmlstr = string.Empty;

                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
                xDoc.LoadXml(xmlstr);
            }
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}