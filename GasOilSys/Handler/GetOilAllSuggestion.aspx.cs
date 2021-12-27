using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

public partial class Handler_GetOilAllSuggestion : System.Web.UI.Page
{
    OilAllSuggestion_DB db = new OilAllSuggestion_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 取得石油自評表委員意見log列表
        ///說    明:
        /// * Request["guid"]: guid
        /// * Request["cpid"]: 業者guid
        /// * Request["qid"]: 題目Guid        
        ///-----------------------------------------------------

        XmlDocument xDoc = new XmlDocument();
        try
        {
            //string SearchStr = (string.IsNullOrEmpty(Request["SearchStr"])) ? "" : Request["SearchStr"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string qid = (string.IsNullOrEmpty(Request["qid"])) ? "" : Request["qid"].ToString().Trim();
            string xmlstr = string.Empty;
            DataTable dt = new DataTable();

            db._業者guid = cpid;
            db._題目guid = qid;
            db._年度 = taiwanYear();

            //db._KeyWord = SearchStr;

            if (string.IsNullOrEmpty(guid))
            {
                dt = db.GetList();
            }
            else
            {
                db._guid = guid;
                dt = db.GetData();
            }
                

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

    public string taiwanYear()
    {
        DateTime nowdate = DateTime.Now;
        string year = nowdate.Year.ToString();
        string taiwanYear = (Convert.ToInt32(year) - 1911).ToString();

        return taiwanYear;
    }
}