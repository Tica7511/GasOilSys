using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetWeekReport_5_7 : System.Web.UI.Page
{
    WeekReportList_DB db = new WeekReportList_DB();
    WeekReport_5_7_DB db2 = new WeekReport_5_7_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢週報計劃書5.7
		///說    明:
		/// * Request["rid"]: Guid 
        /// * Request["year"]: 年度 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string rid = (string.IsNullOrEmpty(Request["rid"])) ? "" : Request["rid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["type"].ToString().Trim();
            int PageNo = (Request["PageNo"] != null) ? int.Parse(Request["PageNo"].ToString().Trim()) : 0;
            int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;

            //計算起始與結束
            int pageEnd = (PageNo + 1) * PageSize;
            int pageStart = pageEnd - PageSize + 1;

            db._guid = rid;
            db._年份 = year;
            db2._年度 = year;
            DataTable dt = db.GetData();
            DataSet ds = new DataSet();

            //取得資料表是 週報_計劃書5_7_審閱進度 or 週報_計劃書5_7_其他災防事項
            if (type == "list1")
                ds = db2.GetList1(pageStart.ToString(), pageEnd.ToString());
            else
                ds = db2.GetList2(pageStart.ToString(), pageEnd.ToString());

            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;
            string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            xmlstr2 = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList2", "data_item2");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + totalxml + xmlstr2 + "</root>";
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