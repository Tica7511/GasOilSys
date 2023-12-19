using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetStatisticsStorageTankInfo : System.Web.UI.Page
{
    OilStorageTankInfo_DB odb = new OilStorageTankInfo_DB();
    GasStorageTankInfo_DB gdb = new GasStorageTankInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢儲槽基本資料
		///說    明:
		/// * Request["type"]: list=查詢資料 dll=查詢管線識別碼 
		/// * Request["ctype"]: 01=石油業者 02=天然氣業者 
		/// * Request["cpname"]: 公司名稱 
        /// * Request["businessOrg"]: 事業部 
        /// * Request["factory"]: 營業處廠 
        /// * Request["workshop"]: 中心庫區儲運課工場 
        /// * Request["ssn"]: 轄區儲槽編號 
        /// * Request["txt7"]: 能源署編號/液化天然氣廠 
        /// * Request["txt8"]: 油品種類/狀態 
        /// * Request["openDateBegin"]: 啟用日期(起) 
        /// * Request["openDateEnd"]: 啟用日期(迄) 
        /// * Request["PageNo"]:欲顯示的頁碼, 由零開始
        /// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string ctype = (string.IsNullOrEmpty(Request["ctype"])) ? "" : Request["ctype"].ToString().Trim();
            string cpname = (string.IsNullOrEmpty(Request["cpname"])) ? "" : Request["cpname"].ToString().Trim();
            string businessOrg = (string.IsNullOrEmpty(Request["businessOrg"])) ? "" : Request["businessOrg"].ToString().Trim();
            string factory = (string.IsNullOrEmpty(Request["factory"])) ? "" : Request["factory"].ToString().Trim();
            string workshop = (string.IsNullOrEmpty(Request["workshop"])) ? "" : Request["workshop"].ToString().Trim();
            string ssn = (string.IsNullOrEmpty(Request["ssn"])) ? "" : Request["ssn"].ToString().Trim();
            string txt7 = (string.IsNullOrEmpty(Request["txt7"])) ? "" : Request["txt7"].ToString().Trim();
            string txt8 = (string.IsNullOrEmpty(Request["txt8"])) ? "" : Request["txt8"].ToString().Trim();
            string openDateBegin = (string.IsNullOrEmpty(Request["openDateBegin"])) ? "" : Request["openDateBegin"].ToString().Trim();
            string openDateEnd = (string.IsNullOrEmpty(Request["openDateEnd"])) ? "" : Request["openDateEnd"].ToString().Trim();
            string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
            int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;
            string xmlstr = string.Empty;
            string totalxml = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            if (type == "list")
            {
                // 計算起始與結束
                int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
                int pageStart = pageEnd - PageSize + 1;

                if (ctype == "01")
                {
                    odb._轄區儲槽編號 = ssn;
                    odb._能源局編號 = txt7;
                    odb._油品種類 = txt8;

                    ds = odb.GetStatisticsList(pageStart.ToString(), pageEnd.ToString(), cpname, businessOrg
                        , factory, workshop, openDateBegin, openDateEnd);
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if (ds.Tables[1].Rows[i]["單獨公司名稱"].ToString().Trim() == "Y")
                                ds.Tables[1].Rows[i]["業者簡稱"] = ds.Tables[1].Rows[i]["公司名稱"].ToString().Trim();
                        }
                    }

                    totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
                }
                else
                {
                    gdb._儲槽編號 = ssn;
                    gdb._液化天然氣廠 = txt7;
                    gdb._狀態 = txt8;

                    ds = gdb.GetStatisticsList(pageStart.ToString(), pageEnd.ToString(), cpname, businessOrg
                        , factory, workshop, openDateBegin, openDateEnd);

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if (ds.Tables[1].Rows[i]["單獨公司名稱"].ToString().Trim() == "Y")
                                ds.Tables[1].Rows[i]["業者簡稱"] = ds.Tables[1].Rows[i]["公司名稱"].ToString().Trim();
                        }
                    }

                    totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
                }

                xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + "</root>";
                xDoc.LoadXml(xmlstr);
            }
            else
            {
                if (ctype == "01")
                {
                    dt = odb.GetStatisticsStorageTankSnList(cpname, businessOrg, factory, workshop);
                }
                else
                {
                    dt = gdb.GetStatisticsStorageTankList(cpname, businessOrg, factory, workshop);
                }

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