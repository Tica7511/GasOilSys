using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebPage_GasSelfEvaluation : System.Web.UI.Page
{
	public string username, identity, companyName;
	GasCompanyInfo_DB gasInfo_db = new GasCompanyInfo_DB();
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
                    Response.Write("<script>alert('參數錯誤'); location='GasCompanyList_temp.aspx';</script>");
            }

            string cpid = (string.IsNullOrEmpty(Request["cp"])) ? LogInfo.companyGuid : Request["cp"].ToString().Trim();

			gasInfo_db._guid = cpid;
			DataTable dt = gasInfo_db.GetInfo();
			if (dt.Rows.Count > 0)
			{
                string cpNameTmp = string.Empty;
                if (string.IsNullOrEmpty(dt.Rows[0]["營業處廠"].ToString()))
                {
                    if (dt.Rows[0]["單獨公司名稱"].ToString().Trim() == "Y")
                        cpNameTmp = dt.Rows[0]["公司名稱"].ToString();
                    else
                        cpNameTmp = dt.Rows[0]["事業部"].ToString() + dt.Rows[0]["中心庫區儲運課工場"].ToString();
                }
                else
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["中心庫區儲運課工場"].ToString()))
                    {
                        cpNameTmp = dt.Rows[0]["事業部"].ToString() + dt.Rows[0]["營業處廠"].ToString();
                    }
                    else
                    {
                        cpNameTmp = dt.Rows[0]["事業部"].ToString() + dt.Rows[0]["營業處廠"].ToString() + dt.Rows[0]["中心庫區儲運課工場"].ToString();
                    }
                }
                companyName = cpNameTmp;
            }
		}
	}
}