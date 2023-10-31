using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetStatisticsPipeInfo : System.Web.UI.Page
{
    OilTubeInfo_DB odb = new OilTubeInfo_DB();
    GasTubeInfo_DB gdb = new GasTubeInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢管線基本資料
		///說    明:
		/// * Request["type"]: list=查詢資料 dll=查詢管線識別碼 
		/// * Request["ctype"]: 01=石油業者 02=天然氣業者 
		/// * Request["cpname"]: 公司名稱 
        /// * Request["businessOrg"]: 事業部 
        /// * Request["factory"]: 營業處廠 
        /// * Request["workshop"]: 中心庫區儲運課工場 
        /// * Request["psn"]: 管線識別碼 
        /// * Request["setyear"]: 建置年 
        /// * Request["oiltype"]: 八大油品 
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
            string psn = (string.IsNullOrEmpty(Request["psn"])) ? "" : Request["psn"].ToString().Trim();
            string setyear = (string.IsNullOrEmpty(Request["setyear"])) ? "" : Request["setyear"].ToString().Trim();
            string pipediameter = (string.IsNullOrEmpty(Request["pipediameter"])) ? "" : Request["pipediameter"].ToString().Trim();
            string oiltype = (string.IsNullOrEmpty(Request["oiltype"])) ? "" : Request["oiltype"].ToString().Trim();
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
                    odb._長途管線識別碼 = psn;
                    odb._建置年 = setyear;
                    odb._管徑吋 = pipediameter;
                    odb._八大油品 = oiltype;

                    ds = odb.GetStatisticsList(pageStart.ToString(), pageEnd.ToString(), cpname, businessOrg
                        , factory, workshop);
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if (ds.Tables[1].Rows[i]["業者guid"].ToString().Trim() == "FA8387C6-5860-40DB-A260-3B6C08413C59")
                                ds.Tables[1].Rows[i]["業者簡稱"] = ds.Tables[1].Rows[i]["公司名稱"].ToString().Trim();
                        }
                    }

                    totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
                }
                else
                {
                    gdb._長途管線識別碼 = psn;
                    gdb._建置年 = setyear;
                    gdb._管徑 = pipediameter;

                    ds = gdb.GetStatisticsList(pageStart.ToString(), pageEnd.ToString(), cpname, businessOrg
                        , factory, workshop);

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if (ds.Tables[1].Rows[i]["業者guid"].ToString().Trim() == "9E779E2B-C36D-44BF-BED2-11C29D989D53")
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
                    dt = odb.GetStatisticsPipeSnList(cpname, businessOrg, factory, workshop);
                }
                else
                {
                    dt = gdb.GetStatisticsPipeSnList(cpname, businessOrg, factory, workshop);
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