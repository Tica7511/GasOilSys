using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetOilStorageTankInfo : System.Web.UI.Page
{
    OilStorageTankInfo_DB db = new OilStorageTankInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢儲槽基本資料
		///說    明:
		/// * Request["cpid"]: 業者Guid 
		/// * Request["type"]: list=列表 data=資料列 
        /// * Request["PageNo"]:欲顯示的頁碼, 由零開始
        /// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? LogInfo.companyGuid : Request["cpid"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? LogInfo.companyGuid : Request["guid"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? LogInfo.companyGuid : Request["type"].ToString().Trim();
            string Sno = (string.IsNullOrEmpty(Request["Sno"])) ? "" : Request["Sno"].ToString().Trim();
            string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
            int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;

            if (type == "list")
            {
                // 計算起始與結束
                int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
                int pageStart = pageEnd - PageSize + 1;

                db._業者guid = cpid;
                db._轄區儲槽編號 = Sno;

                DataSet ds = db.GetList(pageStart.ToString(), pageEnd.ToString());
                DataTable dt2 = db.GetYearList();
                DataTable dt3 = db.GetList();

                string xmlstr = string.Empty;
                string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
                string xmlstr2 = string.Empty;
                string xmlstr3 = string.Empty;

                xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
                xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
                xmlstr3 = DataTableToXml.ConvertDatatableToXML(dt3, "dataList3", "data_item3");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + xmlstr2 + xmlstr3 + "</root>";
                xDoc.LoadXml(xmlstr);
            }
            else
            {
                db._guid = guid;

                DataTable dt = db.GetData();
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