using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Admin_BackEnd_GetMember : System.Web.UI.Page
{
    Member_DB mdb = new Member_DB();
    GasCompanyInfo_DB gcdb = new GasCompanyInfo_DB();
    OilCompanyInfo_DB ocdb = new OilCompanyInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 成員列表
        ///說明:
        /// * Request["SearchStr"]:關鍵字
        /// * Request["PageNo"]:欲顯示的頁碼, 由零開始
        /// * Request["PageSize"]:每頁顯示的資料筆數, 未指定預設10
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            //string SearchStr = (Request["SearchStr"] != null) ?Request["SearchStr"].ToString().Trim() : "";
            string type = (string.IsNullOrEmpty(Request["type"].ToString().Trim())) ? "" : Request["type"].ToString().Trim();
            //string mguid = (string.IsNullOrEmpty(Request["guid"].ToString().Trim())) ? "" : Request["guid"].ToString().Trim();
            string PageNo = (Request["PageNo"] != null) ? Request["PageNo"].ToString().Trim() : "0";
            int PageSize = (Request["PageSize"] != null) ? int.Parse(Request["PageSize"].ToString().Trim()) : 10;

            if (type == "list")
            {
                //計算起始與結束
                int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
                int pageStart = pageEnd - PageSize + 1;

                //mdb._KeyWord = SearchStr;
                DataSet ds = mdb.GetList(pageStart.ToString(), pageEnd.ToString());

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    ds.Tables[1].Columns.Add("cName", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if(dt.Rows[i]["帳號類別"].ToString() == "02")
                        {
                            switch (dt.Rows[i]["網站類別"].ToString())
                            {
                                //石油
                                case "01":
                                    ocdb._guid = dt.Rows[i]["業者guid"].ToString();
                                    DataTable odt = ocdb.GetCpName3();
                                    dt.Rows[i]["cName"] = odt.Rows[0]["cpname"].ToString();
                                    break;
                                //天然氣
                                case "02":
                                    gcdb._guid = dt.Rows[i]["業者guid"].ToString();
                                    DataTable gdt = gcdb.GetCpName3();
                                    dt.Rows[i]["cName"] = gdt.Rows[0]["cpname"].ToString();
                                    break;
                            }
                        }
                    }
                }

                string xmlstr = string.Empty;
                string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
                xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + "</root>";
                xDoc.LoadXml(xmlstr);
            }
            else
            {
                //mdb._M_Guid = mguid;
                //DataTable dt = mdb.GetData();
                //if (dt.Rows.Count > 0)
                //{
                //    dt.Columns.Add("EncodeGuid", typeof(string));
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        dt.Rows[i]["EncodeGuid"] = Server.UrlEncode(Common.Encrypt(dt.Rows[i]["M_Guid"].ToString()));
                //    }
                //}
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