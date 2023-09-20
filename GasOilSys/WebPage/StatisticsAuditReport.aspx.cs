using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebPage_StatisticsAuditReport : System.Web.UI.Page
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
                Response.Write("<script>alert('您尚無權限進入此頁面，將返回系統選擇頁面'); location='Entrance.aspx';</script>");
            }

            // 登入者姓名
            username = LogInfo.name;
            // 登入者權限
            competence = LogInfo.competence;
        }
    }
}