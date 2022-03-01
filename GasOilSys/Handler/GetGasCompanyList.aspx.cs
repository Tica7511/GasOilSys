using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Handler_GetGasCompanyList : System.Web.UI.Page
{
	GasCompanyInfo_DB db = new GasCompanyInfo_DB();
    GasMasterCompare_DB mc_db = new GasMasterCompare_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
        ///-----------------------------------------------------
        ///功    能: 查詢天然氣業者清單
        ///說    明:
        /// * Request["year"]: 年度
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
		try
		{
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();

            string mGuid = string.Empty;
            if (LogInfo.competence == "01")
                mGuid = LogInfo.mGuid;
            else
                year = string.Empty;

			DataTable dt = db.GetCompanyList(mGuid, year);

			string xmlstr = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
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