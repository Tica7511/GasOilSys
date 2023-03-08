using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_memberLog : System.Web.UI.Page
{
    public string username;
    public string competence;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LogInfo.mGuid == "")
            Response.Write("<script>alert('請重新登入'); location='../WebPage/SignIn.aspx';</script>");
        else
        {
            if (LogInfo.competence != "03")
            {
                Response.Write("<script>alert('您尚未有此頁面之權限!將返回登入頁面'); location='../Handler/SignOut.aspx';</script>");
            }

            // 登入者姓名
            username = LogInfo.name;
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