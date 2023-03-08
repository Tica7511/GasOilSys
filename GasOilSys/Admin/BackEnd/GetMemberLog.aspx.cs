using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Admin_BackEnd_GetMemberLog : System.Web.UI.Page
{
    Member_DB mdb = new Member_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 成員登入歷史紀錄列表
        ///說明:
        /// * Request["txt1"]:帳號類別
        /// * Request["PageNo"]:欲顯示的頁碼, 由零開始
        /// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10 
        /// * Request["txt2"]:姓名關鍵字
        /// * Request["beginDate"]:登入日期起
        /// * Request["endDate"]:登入日期迄
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
            int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;
            string txt1 = (Request["txt1"] != null) ? Request["txt1"].ToString().Trim() : "";
            string txt2 = (Request["txt2"] != null) ? Request["txt2"].ToString().Trim() : "";
            string beginDate = (Request["beginDate"] != null) ? Request["beginDate"].ToString().Trim() : "";
            string endDate = (Request["endDate"] != null) ? Request["endDate"].ToString().Trim() : "";

            if (!string.IsNullOrEmpty(beginDate))
                beginDate = (Convert.ToInt32(beginDate) + 19110000).ToString();
            if (!string.IsNullOrEmpty(endDate))
                endDate = (Convert.ToInt32(endDate) + 19110000).ToString();

            //計算起始與結束
            int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
            int pageStart = pageEnd - PageSize + 1;

            mdb._帳號類別 = txt1;
            mdb._姓名 = txt2;
            DataSet ds = mdb.GetMemberLogData(pageStart.ToString(), pageEnd.ToString(), beginDate, endDate);

            string xmlstr = string.Empty;
            string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
            xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + "</root>";
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