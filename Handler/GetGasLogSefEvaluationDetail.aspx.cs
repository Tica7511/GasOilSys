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

public partial class Handler_GetGasLogSefEvaluationDetail : System.Web.UI.Page
{
    GasCommitteeSuggestion_DB cs_db = new GasCommitteeSuggestion_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 取得天然氣自評表意見log詳細資料
        ///說    明:
        /// * Request["qid"]: 自評表委員意見guid
        ///-----------------------------------------------------

        XmlDocument xDoc = new XmlDocument();

        try
        {
            string cpid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string xmlstr = string.Empty;

            cs_db._guid = cpid;
            DataTable dt = cs_db.GetData();

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