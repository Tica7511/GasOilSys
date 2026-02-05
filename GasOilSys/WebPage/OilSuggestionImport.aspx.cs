using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPage_OilSuggestionImport : System.Web.UI.Page
{
    public string username;
    public string userid;
    public string competence;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LogInfo.mGuid == "")
        {
            Response.Write("<script>alert('請重新登入'); location.href='SignIn.aspx';</script>");
        }
        else
        {
            // 登入者姓名
            username = LogInfo.name;
            // 登入者id
            userid = LogInfo.mGuid;
            // 登入者權限
            competence = LogInfo.competence;
        }            

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