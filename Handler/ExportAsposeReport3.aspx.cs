using System;
using Aspose.Words;
using System.Drawing;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

public partial class Handler_ExportAsposeReport3 : System.Web.UI.Page
{
    Week_DB db = new Week_DB();

    protected void Page_Load(object sender, EventArgs e)
    {
        // 在新的工作階段啟動時執行的程式碼
        Aspose.Words.License license = new Aspose.Words.License();
        license.SetLicense(Server.MapPath("~/Bin/Aspose.Total.lic"));

        //日期區間
        string Year = (string.IsNullOrEmpty(Request["Year"])) ? "" : Request["Year"].ToString().Trim();
        string Season = (string.IsNullOrEmpty(Request["Season"])) ? "" : Request["Season"].ToString().Trim();
        string SeasonCn = string.Empty;

        switch (Season)
        {
            case "01":
                SeasonCn = "一";
                break;
            case "02":
                SeasonCn = "二";
                break;
            case "03":
                SeasonCn = "三";
                break;
            case "04":
                SeasonCn = "四";
                break;
        }

        //新檔名
        string newName = (Convert.ToInt32(Year) - 1911).ToString() + "-B0202季報第" + SeasonCn + "季";
        string FileName = string.Empty;

        Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Sample/ExportReprot3.doc"));
        DocumentBuilder builder = new DocumentBuilder(doc);
        builder.ParagraphFormat.Style.Font.Name = "標楷體";
        builder.ParagraphFormat.Style.Font.Size = 14;

        //副檔名
        string ext = ".doc";

        doc.Range.Replace("@Year", (Convert.ToInt32(Year) - 1911).ToString(), false, false);
        doc.Range.Replace("@ThisSeason", SeasonCn, false, false);

        #region 本月月報

        DataTable dt = db.GetSeasonList(Season, (Convert.ToInt32(Year) - 1911).ToString());
        builder.MoveToBookmark("thisSeason");

        if (dt.Rows.Count > 0)
        {
            string dtstr = WeekPage(dt);
            builder.InsertHtml(dtstr);
        }

        #endregion

        doc.Save(Server.MapPath("~/Sample/" + newName + ".doc"));


        Response.AddHeader("content-disposition", "attachment;filename=" + Server.UrlEncode(newName + ext));
        Response.ContentType = "application/octet-stream";
        Response.WriteFile(Server.MapPath("~/Sample/" + newName + ext));
        Response.Flush();
        File.Delete(Server.MapPath("~/Sample/" + newName + ext));
        Response.End();
    }

    //表格
    private string WeekPage(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#ACD6FF'>";
        tmpstr += "<td align='center'><font face='標楷體'>工作項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>預定日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>預定執行內容</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            tmpstr += "<tr>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["工作項次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    //轉換日期 ex: 1100505 => 05.05(二)
    public string getWeekDay(string fulldate)
    {
        string year = (Convert.ToInt32(fulldate.Substring(0, 3)) + 1911).ToString();
        string month = fulldate.Substring(3, 2);
        string date = fulldate.Substring(5, 2);
        DateTime dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(date));
        string dtstr = dt.DayOfWeek.ToString("d");
        string dayname = string.Empty;

        switch (dtstr)
        {
            case "0":
                dayname = "(日)";
                break;
            case "1":
                dayname = "(一)";
                break;
            case "2":
                dayname = "(二)";
                break;
            case "3":
                dayname = "(三)";
                break;
            case "4":
                dayname = "(四)";
                break;
            case "5":
                dayname = "(五)";
                break;
            case "6":
                dayname = "(六)";
                break;
        }
        return month + "/" + date + dayname;
    }
}