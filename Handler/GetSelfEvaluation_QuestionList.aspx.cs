using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Handler_GetSelfEvaluation_QuestionList : System.Web.UI.Page
{
	GasSelfEvaluation_DB gdb = new GasSelfEvaluation_DB();
	OilSelfEvaluation_DB odb = new OilSelfEvaluation_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 查詢自評表題目
		///說    明:
		/// * Request["category"]: 石油or天然氣
		/// * Request["year"]: 年度
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Request["category"].ToString().Trim();
			string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();

			string xmlstr = string.Empty;
			DataTable dt = new DataTable();
			if (category == "gas")
				dt = gdb.GetQuestionList(year);
			else
				dt = odb.GetQuestionList(year);

			if (dt.Rows[0]["xmlDoc"].ToString().Trim() != "")
				xDoc.LoadXml(dt.Rows[0]["xmlDoc"].ToString());
			else
				throw new Exception("查無資料!");
		}
		catch (Exception ex)
		{
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}
}