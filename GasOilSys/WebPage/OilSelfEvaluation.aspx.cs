using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebPage_OilSelfEvaluation : System.Web.UI.Page
{
	public string username, identity, companyName;
	OilCompanyInfo_DB oilInfo_db = new OilCompanyInfo_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		if (LogInfo.mGuid == "")
			Response.Write("<script>alert('請重新登入'); location='SignIn.aspx';</script>");
		else
		{
            username = LogInfo.name;
            identity = LogInfo.competence;
            if (LogInfo.competence != "02")
            {
                if (string.IsNullOrEmpty(Request.QueryString["cp"]))
                    Response.Write("<script>alert('參數錯誤'); location='OilCompanyList.aspx';</script>");
            }
            
            string cpid = (string.IsNullOrEmpty(Request["cp"])) ? LogInfo.companyGuid : Request["cp"].ToString().Trim();

			oilInfo_db._guid = cpid;
			DataTable dt = oilInfo_db.GetInfo();
			if (dt.Rows.Count > 0)
			{
                string cpNameTmp = string.Empty;
                if (string.IsNullOrEmpty(dt.Rows[0]["組"].ToString()))
                {
                    cpNameTmp = dt.Rows[0]["事業部"].ToString() + dt.Rows[0]["營業處廠"].ToString() + dt.Rows[0]["中心庫區儲運課工場"].ToString();
                }
                else
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["中心庫區儲運課工場"].ToString()))
                    {
                        cpNameTmp = dt.Rows[0]["事業部"].ToString() + dt.Rows[0]["營業處廠"].ToString() + dt.Rows[0]["組"].ToString();
                    }
                    else
                    {
                        cpNameTmp = dt.Rows[0]["事業部"].ToString() + dt.Rows[0]["營業處廠"].ToString() + dt.Rows[0]["組"].ToString() + dt.Rows[0]["中心庫區儲運課工場"].ToString();
                    }
                }
                companyName = cpNameTmp;
            }
		}
	}
}