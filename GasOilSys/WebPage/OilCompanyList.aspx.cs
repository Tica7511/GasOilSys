using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebPage_OilCompanyList : System.Web.UI.Page
{
	public string username;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (LogInfo.mGuid == "")
			Response.Write("<script>alert('請重新登入'); location='SignIn.aspx';</script>");
		else
			username = LogInfo.name;

        DataTable isdt = Account.IfAccountPasswordEqual(LogInfo.mGuid);
        if (isdt.Rows.Count > 0)
        {
            if (isdt.Rows[0]["ifEqual"].ToString().Trim() == "Y")
            {
                Response.Redirect("~/WebPage/ChangePassword.aspx");
            }
        }
    }
}