using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetOilCommitteeSuggestionData : System.Web.UI.Page
{
    OilCommitteeSuggestionData_DB db = new OilCommitteeSuggestionData_DB();
    Sign_DB sdb = new Sign_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢 幹線及環線管線&幹線及環線管線以外
        ///說    明:
        /// * Request["cpid"]: 業者Guid 
        /// * Request["year"]: 年度 
        /// * Request["type"]: list=列表 data=資料列 
        /// * Request["no"]: 1=幹線及環線管線 2=幹線及環線管線以外 
        /// * Request["PageNo"]:欲顯示的頁碼, 由零開始
        /// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string year = (string.IsNullOrEmpty(Request["year"])) ? LogInfo.companyGuid : Request["year"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? LogInfo.companyGuid : Request["guid"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? LogInfo.companyGuid : Request["type"].ToString().Trim();

            db._年度 = year;

            if (type == "list")
            {
                DataTable dt = db.GetList();

                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("簽核id", typeof(string));
                    for (int i=0; i < dt.Rows.Count; i++)
                    {
                        sdb._類型 = "01";
                        sdb._guid = dt.Rows[i]["guid"].ToString().Trim();
                        DataTable sdt = sdb.GetSignCount();

                        if (sdt.Rows.Count > 0)
                        {
                            dt.Rows[i]["簽核id"] = sdt.Rows[0]["簽核id"].ToString().Trim();
                        }                    
                    }
                }

                string xmlstr = string.Empty;

                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
                xDoc.LoadXml(xmlstr);
            }
            else
            {
                //db._guid = guid;

                //DataTable dt = db.GetData();
                //string xmlstr = string.Empty;

                //xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                //xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
                //xDoc.LoadXml(xmlstr);
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