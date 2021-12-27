using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPage_GasCheckSmartTubeCleaner : System.Web.UI.Page
{
	public string username;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (LogInfo.mGuid == "")
			Response.Write("<script>alert('請重新登入'); location='SignIn.aspx';</script>");
		else
		{
			if (LogInfo.competence != "02")
			{
				if (string.IsNullOrEmpty(Request["cp"]))
					Response.Write("<script>alert('參數錯誤!'); location='GasCompanyList.aspx';</script>");
			}

			// 登入者姓名
			username = LogInfo.name;
		}
	}
}