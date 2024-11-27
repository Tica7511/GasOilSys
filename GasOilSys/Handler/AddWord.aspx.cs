using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;

public partial class Handler_AddWord : System.Web.UI.Page
{
    OilCompanyInfo_DB db = new OilCompanyInfo_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能:
		///說    明:
		/// * Request["cpid"]: 業者Guid 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		try
		{
			DataTable dt = db.GetExporttable();

			if (dt.Rows.Count > 0)
            {
				string test = string.Empty;
				string str = dt.Rows[0]["strvalues"].ToString().Trim();
				string[] strarry = str.Split('*');
				for(int i = 0; i < strarry.Length; i++)
                {
					string one = string.Empty;
					string two = string.Empty;
					string three = string.Empty;
					string four = string.Empty;
					int k = 0;
					test = strarry[i].Replace("\r\n", "*");
					test = test.Replace("啟", "");
					string[] strooo = test.Split('*');
					for(int j = 0; j < strooo.Length; j++)
                    {
                        if (string.IsNullOrEmpty(strooo[j].Trim()))
                        {
							continue;
                        }
                        else
                        {
							if (k == 0)
								one = strooo[j].Trim();
							if (k == 1)
								two = strooo[j].Trim();
							if (k == 2)
								three = strooo[j].Trim();
							if (k == 3)
								four = strooo[j].Trim();

							k = k + 1;
						}
					}

					db.AddWord(one, two, three, four);

				}

            }

			string xmlstr = string.Empty;
			xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>成功</Response></root>";
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