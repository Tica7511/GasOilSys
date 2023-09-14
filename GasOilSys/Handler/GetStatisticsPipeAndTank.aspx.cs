using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetStatisticsPipeAndTank : System.Web.UI.Page
{
    OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
    GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢槽區管線
		///說    明:
		/// * Request["stype"]: 類別 
        /// * Request["cpname"]: 公司名稱 
        /// * Request["businessOrg"]: 事業部 
        /// * Request["factory"]: 營業處廠 
        /// * Request["workshop"]: 中心庫區儲運課工場 
        /// * Request["dlltype"]: 下拉選單類別 
		/// * Request["type"]: dll=下拉選單 list=列表 data=資料列 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string stype = (string.IsNullOrEmpty(Request["stype"])) ? "" : Request["stype"].ToString().Trim();
            string cpname = (string.IsNullOrEmpty(Request["cpname"])) ? "" : Request["cpname"].ToString().Trim();
            string businessOrg = (string.IsNullOrEmpty(Request["businessOrg"])) ? "" : Request["businessOrg"].ToString().Trim();
            string factory = (string.IsNullOrEmpty(Request["factory"])) ? "" : Request["factory"].ToString().Trim();
            string workshop = (string.IsNullOrEmpty(Request["workshop"])) ? "" : Request["workshop"].ToString().Trim();
            string dlltype = (string.IsNullOrEmpty(Request["dlltype"])) ? "" : Request["dlltype"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;

            if (type == "dll")
            {
                switch (dlltype)
                {
                    case "01":
                        if (stype == "01")
                        {
                            dt = odb.GetDistinctCpName("公司名稱");
                        }
                        else
                        {
                            dt = gdb.GetDistinctCpName("公司名稱");
                        }
                        break;
                    case "02":
                        if (stype == "01")
                        {
                            odb._公司名稱 = cpname;
                            dt = odb.GetDistinctCpName("事業部");
                        }
                        else
                        {
                            gdb._公司名稱 = cpname;
                            dt = gdb.GetDistinctCpName("事業部");
                        }
                        break;
                    case "03":
                        if (stype == "01")
                        {
                            odb._公司名稱 = cpname;
                            odb._事業部 = businessOrg;
                            dt = odb.GetDistinctCpName("營業處廠");
                        }
                        else
                        {
                            gdb._公司名稱 = cpname;
                            gdb._事業部 = businessOrg;
                            dt = gdb.GetDistinctCpName("營業處廠");
                        }
                        break;
                    case "04":
                        if (stype == "01")
                        {
                            odb._公司名稱 = cpname;
                            odb._事業部 = businessOrg;
                            odb._營業處廠 = factory;
                            dt = odb.GetDistinctCpName("中心庫區儲運課工場");
                        }
                        else
                        {
                            gdb._公司名稱 = cpname;
                            gdb._事業部 = businessOrg;
                            gdb._營業處廠 = factory;
                            dt = gdb.GetDistinctCpName("中心庫區儲運課工場");
                        }
                        break;
                }

                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
                xDoc.LoadXml(xmlstr);
            }
            else if (type == "list")
            {
                if (string.IsNullOrEmpty(stype))
                {
                    DataTable gdt = new DataTable();
                    dt = odb.GetCpNameStatistics();
                    gdt = gdb.GetCpNameStatistics();
                    dt2 = odb.GetCountAllList();

                    dt.Merge(gdt);
                }
                else
                {
                    if (stype == "01")
                    {
                        odb._公司名稱 = cpname;
                        odb._事業部 = businessOrg;
                        odb._營業處廠 = factory;
                        odb._中心庫區儲運課工場 = workshop;
                        dt = odb.GetCpNameStatistics();
                        dt2 = odb.GetCountList();
                    }
                    else
                    {
                        gdb._公司名稱 = cpname;
                        gdb._事業部 = businessOrg;
                        gdb._營業處廠 = factory;
                        gdb._中心庫區儲運課工場 = workshop;
                        dt = gdb.GetCpNameStatistics();
                        dt2 = gdb.GetCountList();
                    }
                }               

                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + "</root>";
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