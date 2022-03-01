using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebPage_Entrance : System.Web.UI.Page
{
	GasMasterCompare_DB gmc_db = new GasMasterCompare_DB();
    OilMasterCompare_DB omc_db = new OilMasterCompare_DB();
	public string name, identity, EnterCtrl;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (LogInfo.mGuid != "")
		{
            DataTable isdt = Account.IfAccountPasswordEqual(LogInfo.mGuid);
            if (isdt.Rows.Count > 0)
            {
                if (isdt.Rows[0]["ifEqual"].ToString().Trim() == "Y")
                {
                    Response.Redirect("~/WebPage/ChangePassword.aspx");
                }
            }

            name = LogInfo.name;
			identity = LogInfo.competence;
			switch (identity)
			{
				default:
                    break;
				case "01": //委員
                    gmc_db._年度 = taiwanYear();
                    gmc_db._委員guid = LogInfo.mGuid;
                    omc_db._年度 = taiwanYear(); 
                    omc_db._委員guid = LogInfo.mGuid;
                    DataTable Gasdt = gmc_db.GetMasterType();
                    DataTable Oildt = omc_db.GetMasterType();
                    bool bGas = false;
                    bool bOil = false;
                    if (Gasdt.Rows.Count > 0)
                        bGas = true;
                    if (Oildt.Rows.Count > 0)
                        bOil = true;                   

                    if (bGas && bOil)
                        EnterCtrl = "all";
                    else if (bOil)
                        EnterCtrl = "oil";
                    else
                        EnterCtrl = "gas";
                    break;
				case "02": // 業者
					if (LogInfo.user == "01")
					{
                        Response.Redirect("~/WebPage/OilInfo.aspx?cp=" + LogInfo.companyGuid);
                    }
                    else
                    {
                        Response.Redirect("~/WebPage/GasInfo.aspx?cp=" + LogInfo.companyGuid);
                    }                        						
					break;
                case "03": // 管理者
                    switch (LogInfo.user)
                    {
                        default:
                            EnterCtrl = "all";
                            break;
                        case "01":
                            EnterCtrl = "oil";
                            break;
                        case "02":
                            EnterCtrl = "gas";
                            break;
                    }
                    break;
                case "04": // 長官
                    switch (LogInfo.user)
                    {
                        default:
                            break;
                        case "01":
                            Response.Redirect("~/WebPage/OilCompanyList.aspx");
                            break;
                        case "02":
                            Response.Redirect("~/WebPage/GasCompanyList.aspx");
                            break;
                    }
                    break;
			}
		}
		else
		{
			Response.Write("<script>alert('請重新登入'); location='SignIn.aspx';</script>");
		}
	}

    public string taiwanYear()
    {
        DateTime nowdate = DateTime.Now;
        string year = nowdate.Year.ToString();
        string taiwanYear = (Convert.ToInt32(year) - 1911).ToString();

        return taiwanYear;
    }
}