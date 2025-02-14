using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPage_PublicGasInfo : System.Web.UI.Page
{
    public string username;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LogInfo.mGuid == "")
            Response.Write("<script>alert('請重新登入'); location='SignIn.aspx';</script>");
        else
        {
            DataTable isdt = Account.IfAccountPasswordEqual(LogInfo.mGuid);
            if (isdt.Rows.Count > 0)
            {
                if (isdt.Rows[0]["ifEqual"].ToString().Trim() == "Y")
                {
                    Response.Redirect("~/WebPage/ChangePassword.aspx");
                }
            }

            if (LogInfo.competence == "02")
                Response.Redirect("~/WebPage/GasInfo.aspx?cp=" + LogInfo.companyGuid);

            username = LogInfo.name;
        }
    }
}