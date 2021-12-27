using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Handler_GetWeekReportList : System.Web.UI.Page
{
    WeekReportList_DB db = new WeekReportList_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢計劃書列表
        ///說    明:
        /// * Request["year"]: 年度
        ///-----------------------------------------------------

        XmlDocument xDoc = new XmlDocument();
        try
        {
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();

            string xmlstr = string.Empty;
            DataTable dt = new DataTable();

            db._年份 = year;
            dt = db.GetList();

            if (dt.Rows[0]["xmlDoc"].ToString().Trim() != "")
                xDoc.LoadXml(dt.Rows[0]["xmlDoc"].ToString());
            else
                xDoc.LoadXml("<?xml version='1.0' encoding='utf-8'?><root></root>");
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}