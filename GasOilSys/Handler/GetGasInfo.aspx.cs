using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetGasInfo : System.Web.UI.Page
{
	GasInfo_DB db = new GasInfo_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
        ///-----------------------------------------------------
        ///功    能: 查詢天然氣業者基本資料
        ///說    明:
        /// * Request["cpid"]: 業者Guid 
        /// * Request["type"]: list=列表 data=資料 
        /// * Request["centralType"]: 場站類別 
        /// * Request["centralName"]: 中心名稱 
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
		try
		{
			string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? LogInfo.companyGuid : Request["cpid"].ToString().Trim();
			string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
			string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
			string centralType = (string.IsNullOrEmpty(Request["centralType"])) ? "" : Request["centralType"].ToString().Trim();
			string centralName = (string.IsNullOrEmpty(Request["centralName"])) ? "" : Request["centralName"].ToString().Trim();
            string dtCount = string.Empty;

            if(type == "list")
            {
                db._年度 = year;
                db._業者guid = cpid;
                DataTable dt = db.GetInfo();
                DataTable dt2 = db.GetList();
                DataTable ydt = db.GetYearList();

                dtCount = dt2.Rows.Count.ToString();

                if (dt2.Rows.Count > 0)
                {
                    dt2.Columns.Add("年度", typeof(string));
                    dt2.Columns.Add("配氣站guid", typeof(string));
                    dt2.Columns.Add("開關站guid", typeof(string));
                    dt2.Columns.Add("隔離站guid", typeof(string));
                    dt2.Columns.Add("計量站guid", typeof(string));
                    dt2.Columns.Add("清管站guid", typeof(string));

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        if (i != (dt2.Rows.Count - 1))
                        {
                            DataTable dt3 = new DataTable();

                            #region 配氣站

                            db._中心名稱 = dt2.Rows[i]["配氣站"].ToString();
                            dt3 = db.GetData();
                            if (dt3.Rows.Count > 0)
                            {
                                dt2.Rows[i]["配氣站guid"] = dt3.Rows[0]["guid"].ToString();
                            }

                            #endregion

                            #region 開關站

                            db._中心名稱 = dt2.Rows[i]["開關站"].ToString();
                            dt3 = db.GetData();
                            if (dt3.Rows.Count > 0)
                            {
                                dt2.Rows[i]["開關站guid"] = dt3.Rows[0]["guid"].ToString();
                            }

                            #endregion

                            #region 隔離站

                            db._中心名稱 = dt2.Rows[i]["隔離站"].ToString();
                            dt3 = db.GetData();
                            if (dt3.Rows.Count > 0)
                            {
                                dt2.Rows[i]["隔離站guid"] = dt3.Rows[0]["guid"].ToString();
                            }

                            #endregion

                            #region 計量站

                            db._中心名稱 = dt2.Rows[i]["計量站"].ToString();
                            dt3 = db.GetData();
                            if (dt3.Rows.Count > 0)
                            {
                                dt2.Rows[i]["計量站guid"] = dt3.Rows[0]["guid"].ToString();
                            }

                            #endregion

                            #region 清管站

                            db._中心名稱 = dt2.Rows[i]["清管站"].ToString();
                            dt3 = db.GetData();
                            if (dt3.Rows.Count > 0)
                            {
                                dt2.Rows[i]["清管站guid"] = dt3.Rows[0]["guid"].ToString();
                            }

                            #endregion

                            dt2.Rows[i]["年度"] = year;
                        }
                    }

                }


                string xmlstr = string.Empty;
                string xmlstr2 = string.Empty;
                string xmlstr3 = string.Empty;
                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
                xmlstr3 = DataTableToXml.ConvertDatatableToXML(ydt, "dataList3", "data_item3");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><dtCount>" + dtCount + "</dtCount>" + xmlstr + xmlstr2 + xmlstr3 + "</root>";
                xDoc.LoadXml(xmlstr);
            }
            else
            {
                db._業者guid = cpid;
                db._年度 = year;
                db._場站類別 = centralType;
                db._中心名稱 = centralName;

                DataTable dt = db.GetData();

                dtCount = dt.Rows.Count.ToString();

                string xmlstr = string.Empty;
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><dtCount>" + dtCount + "</dtCount></root>";
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