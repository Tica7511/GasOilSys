using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetVerificationTest : System.Web.UI.Page
{
	OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
	GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
	AviationCompanyInfo adb = new AviationCompanyInfo();
	VerificationTest_DB vdb = new VerificationTest_DB();
	FileTable fdb = new FileTable();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢 查核及檢測
		///說    明:
		/// * Request["cpid"]: 業者Guid 
		/// * Request["year"]: 年度 
		/// * Request["type"]: list=列表 data=資料列 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
			string dataType = (string.IsNullOrEmpty(Request["dataType"])) ? "" : Request["dataType"].ToString().Trim();
			string sType = (string.IsNullOrEmpty(Request["sType"])) ? "" : Request["sType"].ToString().Trim();
			string tobject = (string.IsNullOrEmpty(Request["tobject"])) ? "" : Request["tobject"].ToString().Trim();
			string timeBegin = (string.IsNullOrEmpty(Request["timeBegin"])) ? "" : Request["timeBegin"].ToString().Trim();
			string timeEnd = (string.IsNullOrEmpty(Request["timeEnd"])) ? "" : Request["timeEnd"].ToString().Trim();
			string reportNum = (string.IsNullOrEmpty(Request["reportNum"])) ? "" : Request["reportNum"].ToString().Trim();
			string situation = (string.IsNullOrEmpty(Request["situation"])) ? "" : Request["situation"].ToString().Trim();
			string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
			string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
			string mGuid = string.Empty;
			string timeBeginYear = string.Empty;
			DataSet ds = new DataSet();
			DataTable dt = new DataTable();
			DataTable dt2 = new DataTable();

            if (type == "list")
            {
				vdb._類別 = sType;
				vdb._業者guid = tobject;
				vdb._報告編號 = reportNum;
				vdb._改善情形 = situation;

				ds = vdb.GetList(timeBegin, timeEnd);

				string xmlstr = string.Empty;
				string totalxml = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";

				xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + totalxml + xmlstr + "</root>";
				xDoc.LoadXml(xmlstr);
			}
			else if (type == "object")
			{
                switch (dataType)
                {
					case "1":
						if (LogInfo.competence == "01")
							mGuid = LogInfo.mGuid;
						else
							year = string.Empty;

						dt = new DataTable();

						if (LogInfo.competence == "05")
						{
							dt = odb.Get05CompanyListVerification();
						}
						else if (LogInfo.competence == "06")
						{
							dt = odb.Get06CompanyListVerification();
						}
						else
						{
							dt = odb.GetCompanyListVerification(mGuid, year);
						}
						break;
					case "2":
						if (LogInfo.competence == "01")
							mGuid = LogInfo.mGuid;
						else
							year = string.Empty;

						dt = gdb.GetCompanyListVerification(mGuid, year);
						break;
					case "3":
						if (LogInfo.competence == "01")
							mGuid = LogInfo.mGuid;
						else
							year = string.Empty;

						dt = new DataTable();

						if (LogInfo.competence == "05")
						{
							dt = odb.Get05CompanyListVerification();
						}
						else if (LogInfo.competence == "06")
						{
							dt = odb.Get06CompanyListVerification();
						}
						else
						{
							dt = odb.GetCompanyListVerification(mGuid, year);
						}

						dt2 = gdb.GetCompanyListVerification(mGuid, year);

						dt.Merge(dt2);

						break;
					case "4":
						dt = adb.GetCompanyList();
						break;
				}

                if (dt.Rows.Count > 0)
                {
					dt.Columns.Add("CompanyFullName", typeof(string));

					for(int i = 0; i < dt.Rows.Count; i++)
                    {
						if(dataType != "4")
                        {
							dt.Rows[i]["CompanyFullName"] = dt.Rows[i]["公司名稱"].ToString().Trim() + dt.Rows[i]["事業部"].ToString().Trim() +
								dt.Rows[i]["事業部"].ToString().Trim() + dt.Rows[i]["中心庫區儲運課工場"].ToString().Trim();
						}
                        else
                        {
							dt.Rows[i]["CompanyFullName"] = dt.Rows[i]["公司名稱"].ToString().Trim() + dt.Rows[i]["場佔位置"].ToString().Trim();
						}						
					}
				}

				string xmlstr = string.Empty;

				xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
				xDoc.LoadXml(xmlstr);
			}
			else if (type == "statistics")
            {
				vdb._類別 = sType;
				vdb._業者guid = tobject;
				vdb._報告編號 = reportNum;
				vdb._改善情形 = situation;

				dt = vdb.GetCountList(timeBegin, timeEnd);

                if (dt.Rows.Count > 0)
                {
					dt.Columns.Add("報告總和", typeof(string));

					for(int i=0; i < dt.Rows.Count; i++)
                    {
						dt.Rows[i]["報告總和"] = (Convert.ToInt32(dt.Rows[i]["查核報告總和"].ToString().Trim()) + Convert.ToInt32(dt.Rows[i]["相關報告總和"].ToString().Trim())).ToString().Trim();
					}
				}

				string xmlstr = string.Empty;

				xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
				xDoc.LoadXml(xmlstr);
			}
			else if (type == "session")
            {
				string MaxSn = string.Empty;

				//vdb._類別 = sType;
				//vdb._業者guid = tobject;
				timeBeginYear = timeBegin.Substring(0, 3);
				vdb._年度 = timeBeginYear;

				dt = vdb.GetLastYearSession();
                if (dt.Rows.Count > 0)
                {
					MaxSn = dt.Rows[0]["最新場次"].ToString().Trim();
                    if (MaxSn.Length == 1) {
						MaxSn = "0" + MaxSn;
					}
				}

				string xmlstr = string.Empty;
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>" + MaxSn + "</Response></root>";
				xDoc.LoadXml(xmlstr);
			}
			else
			{
                vdb._guid = guid;
                dt = vdb.GetData();

                if (dt.Rows.Count > 0)
                {
					dt.Columns.Add("新檔名", typeof(string));
					dt.Columns.Add("排序", typeof(string));

					for(int i = 0; i < dt.Rows.Count; i++)
                    {
						fdb._guid = guid;
						fdb._檔案類型 = "10";

						DataTable fdt = fdb.GetData();

						if (fdt.Rows.Count > 0)
						{
							dt.Rows[i]["新檔名"] = fdt.Rows[0]["新檔名"].ToString().Trim();
							dt.Rows[i]["排序"] = fdt.Rows[0]["排序"].ToString().Trim();
						}
					}
                }


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