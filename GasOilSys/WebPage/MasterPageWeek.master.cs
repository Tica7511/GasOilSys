using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPage_MasterPageWeek : System.Web.UI.MasterPage
{
    public string username;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LogInfo.mGuid == "")
            Response.Write("<script>alert('請重新登入'); location='SignIn.aspx';</script>");
        else
        {

            // 登入者姓名
            username = LogInfo.name;
        }
    }
}
