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

public partial class Handler_ExportAsposeReport : System.Web.UI.Page
{
    #region 資料表

    Week_DB db = new Week_DB();
    WeekReportList_DB rdb = new WeekReportList_DB();
    WeekReport_1_1_DB db1_1 = new WeekReport_1_1_DB();
    WeekReport_1_2_DB db1_2 = new WeekReport_1_2_DB();
    WeekReport_1_3_1_DB db1_3_1 = new WeekReport_1_3_1_DB();
    WeekReport_1_3_2_DB db1_3_2 = new WeekReport_1_3_2_DB();
    WeekReport_1_3_3_DB db1_3_3 = new WeekReport_1_3_3_DB();
    WeekReport_1_3_4_DB db1_3_4 = new WeekReport_1_3_4_DB();
    WeekReport_1_3_5_DB db1_3_5 = new WeekReport_1_3_5_DB();
    WeekReport_1_3_6_DB db1_3_6 = new WeekReport_1_3_6_DB();
    WeekReport_1_3_7_DB db1_3_7 = new WeekReport_1_3_7_DB();
    WeekReport_1_4_1_DB db1_4_1 = new WeekReport_1_4_1_DB();
    WeekReport_1_4_2_DB db1_4_2 = new WeekReport_1_4_2_DB();
    WeekReport_1_4_3_DB db1_4_3 = new WeekReport_1_4_3_DB();
    WeekReport_1_4_4_DB db1_4_4 = new WeekReport_1_4_4_DB();
    WeekReport_1_4_5_DB db1_4_5 = new WeekReport_1_4_5_DB();
    WeekReport_1_5_DB db1_5 = new WeekReport_1_5_DB();
    WeekReport_2_1_DB db2_1 = new WeekReport_2_1_DB();
    WeekReport_2_2_1_DB db2_2_1 = new WeekReport_2_2_1_DB();
    WeekReport_2_2_2_DB db2_2_2 = new WeekReport_2_2_2_DB();
    WeekReport_2_2_3_DB db2_2_3 = new WeekReport_2_2_3_DB();
    WeekReport_2_3_1_DB db2_3_1 = new WeekReport_2_3_1_DB();
    WeekReport_2_3_2_DB db2_3_2 = new WeekReport_2_3_2_DB();
    WeekReport_2_3_3_DB db2_3_3 = new WeekReport_2_3_3_DB();
    WeekReport_2_4_DB db2_4 = new WeekReport_2_4_DB();
    WeekReport_3_1_DB db3_1 = new WeekReport_3_1_DB();
    WeekReport_3_2_DB db3_2 = new WeekReport_3_2_DB();
    WeekReport_3_3_DB db3_3 = new WeekReport_3_3_DB();
    WeekReport_3_4_DB db3_4 = new WeekReport_3_4_DB();
    WeekReport_3_5_DB db3_5 = new WeekReport_3_5_DB();
    WeekReport_3_6_DB db3_6 = new WeekReport_3_6_DB();
    WeekReport_3_7_DB db3_7 = new WeekReport_3_7_DB();
    WeekReport_3_8_DB db3_8 = new WeekReport_3_8_DB();
    WeekReport_3_9_DB db3_9 = new WeekReport_3_9_DB();
    WeekReport_4_1_DB db4_1 = new WeekReport_4_1_DB();
    WeekReport_4_2_DB db4_2 = new WeekReport_4_2_DB();
    WeekReport_4_3_DB db4_3 = new WeekReport_4_3_DB();
    WeekReport_5_1_DB db5_1 = new WeekReport_5_1_DB();
    WeekReport_5_2_DB db5_2 = new WeekReport_5_2_DB();
    WeekReport_5_3_DB db5_3 = new WeekReport_5_3_DB();
    WeekReport_5_4_DB db5_4 = new WeekReport_5_4_DB();
    WeekReport_5_5_DB db5_5 = new WeekReport_5_5_DB();
    WeekReport_5_6_DB db5_6 = new WeekReport_5_6_DB();
    WeekReport_5_7_DB db5_7 = new WeekReport_5_7_DB();
    WeekReport_5_8_DB db5_8 = new WeekReport_5_8_DB();
    WeekReport_6_1_DB db6_1 = new WeekReport_6_1_DB();
    WeekReport_6_2_DB db6_2 = new WeekReport_6_2_DB();

    public string color = string.Empty;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        // 在新的工作階段啟動時執行的程式碼
        Aspose.Words.License license = new Aspose.Words.License();
        license.SetLicense(Server.MapPath("~/Bin/Aspose.Total.lic"));

        //日期區間
        string Year = (string.IsNullOrEmpty(Request["Year"])) ? "" : Request["Year"].ToString().Trim();
        string ThisWeekFirstDay = (string.IsNullOrEmpty(Request["ThisWeekFirstDay"])) ? "" : Request["ThisWeekFirstDay"].ToString().Trim();
        string ThisWeekFinalDay = (string.IsNullOrEmpty(Request["ThisWeekFinalDay"])) ? "" : Request["ThisWeekFinalDay"].ToString().Trim();
        string NextWeekFirstDay = (string.IsNullOrEmpty(Request["NextWeekFirstDay"])) ? "" : Request["NextWeekFirstDay"].ToString().Trim();
        string NextWeekFinalDay = (string.IsNullOrEmpty(Request["NextWeekFinalDay"])) ? "" : Request["NextWeekFinalDay"].ToString().Trim();
        string thisYear = (Convert.ToInt32(Year) - 1911).ToString();

        //新檔名
        string newName = (Convert.ToInt32(Year)-1911).ToString() + "-B0202週報" + ThisWeekFirstDay;
        string FileName = string.Empty;

        Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Sample/ExportReprot.doc"));
        DocumentBuilder builder = new DocumentBuilder(doc);
        builder.ParagraphFormat.Style.Font.Name = "標楷體";
        builder.ParagraphFormat.Style.Font.Size = 14;

        rdb._年份 = thisYear;

        //副檔名
        string ext = ".doc";

        #region 本年

        doc.Range.Replace("@Year", thisYear, false, false);

        #endregion

        #region 本週週報

        doc.Range.Replace("@ThisWeekFirstDay", getWeekDay(thisYear + ThisWeekFirstDay), false, false);
        doc.Range.Replace("@ThisWeekFinalDay", getWeekDay(thisYear + ThisWeekFinalDay), false, false);

        dt = db.GetWeekExport(Year+ThisWeekFirstDay, Year+ThisWeekFinalDay);
        builder.MoveToBookmark("thisWeek");

        if (dt.Rows.Count > 0)
        {
            string dtstr = WeekPage(dt);
            builder.InsertHtml(dtstr);
        }

        dt = null;

        #endregion

        #region 下週週報

        doc.Range.Replace("@NextWeekFirstDay", getWeekDay(thisYear + NextWeekFirstDay), false, false);
        doc.Range.Replace("@NextWeekFinalDay", getWeekDay(thisYear + NextWeekFinalDay), false, false);

        dt = db.GetWeekExport(Year + NextWeekFirstDay, Year + NextWeekFinalDay);
        builder.MoveToBookmark("nextWeek");

        if (dt.Rows.Count > 0)
        {
            string dtstr = WeekPage(dt);
            builder.InsertHtml(dtstr);
        }

        dt = null;

        #endregion

        #region 計劃書1

        exportItemName(doc, "1");

        #region 計劃書1.1

        db1_1._年度 = thisYear;

        exportItemName(doc, "1.1");

        exportReport(builder, db1_1.GetList(), "Week1_1", Week1_1(db1_1.GetList()));

        #endregion

        #region 計劃書1.2

        db1_2._年度 = thisYear;

        exportItemName(doc, "1.2");

        exportReport(builder, db1_2.GetList(), "Week1_2", Week1_2(db1_2.GetList()));

        #endregion

        #region 計劃書1.3

        exportItemName(doc, "1.3");

        #region 計劃書1.3.1

        db1_3_1._年度 = thisYear;

        exportItemName(doc, "1.3.1");

        exportReport(builder, db1_3_1.GetList(), "Week1_3_1", Week1_3_1(db1_3_1.GetList()));

        #endregion

        #region 計劃書1.3.2

        db1_3_2._年度 = thisYear;

        exportItemName(doc, "1.3.2");

        exportReport(builder, db1_3_2.GetList(), "Week1_3_2", Week1_3_2(db1_3_2.GetList()));

        #endregion

        #region 計劃書1.3.3

        db1_3_3._年度 = thisYear;

        exportItemName(doc, "1.3.3");

        exportReport(builder, db1_3_3.GetList(), "Week1_3_3", Week1_3_3(db1_3_3.GetList()));

        #endregion

        #region 計劃書1.3.4

        db1_3_4._年度 = thisYear;

        exportItemName(doc, "1.3.4");

        exportReport(builder, db1_3_4.GetList(), "Week1_3_4", Week1_3_4(db1_3_4.GetList()));

        #endregion

        #region 計劃書1.3.5

        db1_3_5._年度 = thisYear;

        exportItemName(doc, "1.3.5");

        exportReport(builder, db1_3_5.GetList(), "Week1_3_5", Week1_3_5(db1_3_5.GetList()));

        #endregion

        #region 計劃書1.3.6

        db1_3_6._年度 = thisYear;

        exportItemName(doc, "1.3.6");

        exportReport(builder, db1_3_6.GetList(), "Week1_3_6", Week1_3_6(db1_3_6.GetList()));

        #endregion

        #region 計劃書1.3.7

        db1_3_7._年度 = thisYear;

        exportItemName(doc, "1.3.7");

        exportReport(builder, db1_3_7.GetList(), "Week1_3_7", Week1_3_7(db1_3_7.GetList()));

        #endregion

        #endregion

        #region 計劃書1.4

        exportItemName(doc, "1.4");

        #region 計劃書1.4.1

        db1_4_1._年度 = thisYear;

        exportItemName(doc, "1.4.1");

        exportReport(builder, db1_4_1.GetList(), "Week1_4_1", Week1_4_1(db1_4_1.GetList()));

        #endregion

        #region 計劃書1.4.2

        db1_4_2._年度 = thisYear;

        exportItemName(doc, "1.4.2");

        exportReport(builder, db1_4_2.GetList(), "Week1_4_2", Week1_4_2(db1_4_2.GetList()));

        #endregion

        #region 計劃書1.4.3

        db1_4_3._年度 = thisYear;

        exportItemName(doc, "1.4.3");

        exportReport(builder, db1_4_3.GetList(), "Week1_4_3", Week1_4_3(db1_4_3.GetList()));

        #endregion

        #region 計劃書1.4.4

        db1_4_4._年度 = thisYear;

        exportItemName(doc, "1.4.4");

        #region 計劃書1.4.4列表

        exportReport(builder, db1_4_4.GetList(), "Week1_4_4_A", Week1_4_4_A(db1_4_4.GetList()));

        #endregion

        #region 計劃書1.4.4檢測列表

        exportReport(builder, db1_4_4.GetList2(), "Week1_4_4_B", Week1_4_4_B(db1_4_4.GetList2()));

        #endregion

        #endregion

        #region 計劃書1.4.5

        db1_4_5._年度 = thisYear;

        exportItemName(doc, "1.4.5");

        exportReport(builder, db1_4_5.GetList(), "Week1_4_5", Week1_4_5(db1_4_5.GetList()));

        #endregion

        #endregion

        #region 計劃書1.5

        db1_5._年度 = thisYear;

        exportItemName(doc, "1.5");

        exportReport(builder, db1_5.GetList(), "Week1_5", Week1_5(db1_5.GetList()));

        #endregion

        #endregion

        #region 計劃書2

        exportItemName(doc, "2");

        #region 計劃書2.1

        db2_1._年度 = thisYear;

        exportItemName(doc, "2.1");

        exportReport(builder, db2_1.GetList(), "Week2_1", Week2_1(db2_1.GetList()));

        #endregion

        #region 計劃書2.2

        exportItemName(doc, "2.2");

        #region 計劃書2.2.1

        db2_2_1._年度 = thisYear;

        exportItemName(doc, "2.2.1");

        exportReport(builder, db2_2_1.GetList(), "Week2_2_1", Week2_2_1(db2_2_1.GetList()));

        #endregion

        #region 計劃書2.2.2

        db2_2_2._年度 = thisYear;

        exportItemName(doc, "2.2.2");

        exportReport(builder, db2_2_2.GetList(), "Week2_2_2", Week2_2_2(db2_2_2.GetList()));

        #endregion

        #region 計劃書2.2.3

        db2_2_3._年度 = thisYear;

        exportItemName(doc, "2.2.3");

        exportReport(builder, db2_2_3.GetList(), "Week2_2_3", Week2_2_3(db2_2_3.GetList()));

        #endregion

        #endregion

        #region 計劃書2.3

        exportItemName(doc, "2.3");

        #region 計劃書2.3.1

        db2_3_1._年度 = thisYear;

        exportItemName(doc, "2.3.1");

        exportReport(builder, db2_3_1.GetList(), "Week2_3_1", Week2_3_1(db2_3_1.GetList()));

        #endregion

        #region 計劃書2.3.2

        db2_3_2._年度 = thisYear;

        exportItemName(doc, "2.3.2");

        exportReport(builder, db2_3_2.GetList(), "Week2_3_2", Week2_3_2(db2_3_2.GetList()));

        #endregion

        #region 計劃書2.3.3

        db2_3_3._年度 = thisYear;

        exportItemName(doc, "2.3.3");

        exportReport(builder, db2_3_3.GetList(), "Week2_3_3", Week2_3_3(db2_3_3.GetList()));

        #endregion

        #endregion

        #region 計劃書2.4

        db2_4._年度 = thisYear;

        exportItemName(doc, "2.4");

        exportReport(builder, db2_4.GetList(), "Week2_4", Week2_4(db2_4.GetList()));

        #endregion

        #endregion

        #region 計劃書3

        exportItemName(doc, "3");

        #region 計劃書3.1

        db3_1._年度 = thisYear;

        exportItemName(doc, "3.1");

        exportReport(builder, db3_1.GetList(), "Week3_1", Week3_1(db3_1.GetList()));

        #endregion

        #region 計劃書3.2

        db3_2._年度 = thisYear;

        exportItemName(doc, "3.2");

        exportReport(builder, db3_2.GetList(), "Week3_2", Week3_2(db3_2.GetList()));

        #endregion

        #region 計劃書3.3

        db3_3._年度 = thisYear;

        exportItemName(doc, "3.3");

        exportReport(builder, db3_3.GetList(), "Week3_3", Week3_3(db3_3.GetList()));

        #endregion

        #region 計劃書3.4

        db3_4._年度 = thisYear;

        exportItemName(doc, "3.4");

        exportReport(builder, db3_4.GetList(), "Week3_4", Week3_4(db3_4.GetList()));

        #endregion

        #region 計劃書3.5

        db3_5._年度 = thisYear;

        exportItemName(doc, "3.5");

        exportReport(builder, db3_5.GetList(), "Week3_5", Week3_5(db3_5.GetList()));

        #endregion

        #region 計劃書3.6

        db3_6._年度 = thisYear;

        exportItemName(doc, "3.6");

        exportReport(builder, db3_6.GetList(), "Week3_6", Week3_6(db3_6.GetList()));

        #endregion

        #region 計劃書3.7

        db3_7._年度 = thisYear;

        exportItemName(doc, "3.7");

        exportReport(builder, db3_7.GetList(), "Week3_7", Week3_7(db3_7.GetList()));

        #endregion

        #region 計劃書3.8

        db3_8._年度 = thisYear;

        exportItemName(doc, "3.8");

        exportReport(builder, db3_8.GetList(), "Week3_8", Week3_8(db3_8.GetList()));

        #endregion

        #region 計劃書3.9

        db3_9._年度 = thisYear;

        exportItemName(doc, "3.9");

        exportReport(builder, db3_9.GetList(), "Week3_9", Week3_9(db3_9.GetList()));

        #endregion

        #endregion

        #region 計劃書4

        exportItemName(doc, "4");

        #region 計劃書4.1

        db4_1._年度 = thisYear;

        exportItemName(doc, "4.1");

        exportReport(builder, db4_1.GetList(), "Week4_1", Week4_1(db4_1.GetList()));

        #endregion

        #region 計劃書4.2

        db4_2._年度 = thisYear;

        exportItemName(doc, "4.2");

        exportReport(builder, db4_2.GetList(), "Week4_2", Week4_2(db4_2.GetList()));

        #endregion

        #region 計劃書4.3

        db4_3._年度 = thisYear;

        exportItemName(doc, "4.3");

        exportReport(builder, db4_3.GetList(), "Week4_3", Week4_3(db4_3.GetList()));

        #endregion

        #endregion

        #region 計劃書5

        exportItemName(doc, "5");

        #region 計劃書5.1

        db5_1._年度 = thisYear;

        exportItemName(doc, "5.1");

        exportReport(builder, db5_1.GetList(), "Week5_1", Week5_1(db5_1.GetList()));

        #endregion

        #region 計劃書5.2

        db5_2._年度 = thisYear;

        exportItemName(doc, "5.2");

        exportReport(builder, db5_2.GetList(), "Week5_2", Week5_2(db5_2.GetList()));

        #endregion

        #region 計劃書5.3

        db5_3._年度 = thisYear;

        exportItemName(doc, "5.3");

        exportReport(builder, db5_3.GetList(), "Week5_3", Week5_3(db5_3.GetList()));

        #endregion

        #region 計劃書5.4

        db5_4._年度 = thisYear;

        exportItemName(doc, "5.4");

        exportReport(builder, db5_4.GetList(), "Week5_4", Week5_4(db5_4.GetList()));

        #endregion

        #region 計劃書5.5

        db5_5._年度 = thisYear;

        exportItemName(doc, "5.5");

        exportReport(builder, db5_5.GetList(), "Week5_5", Week5_5(db5_5.GetList()));

        #endregion

        #region 計劃書5.6

        db5_6._年度 = thisYear;

        exportItemName(doc, "5.6");

        exportReport(builder, db5_6.GetList(), "Week5_6", Week5_6(db5_6.GetList()));

        #endregion

        #region 計劃書5.7

        db5_7._年度 = thisYear;

        exportItemName(doc, "5.7");

        #region 計劃書5.7審閱進度

        exportReport(builder, db5_7.GetExportList(), "Week5_7_A", Week5_7_A(db5_7.GetExportList()));

        #endregion

        #region 計劃書5.7其他災防事項

        exportReport(builder, db5_7.GetExportList2(), "Week5_7_B", Week5_7_B(db5_7.GetExportList2()));

        #endregion

        #endregion

        #region 計劃書5.8

        db5_8._年度 = thisYear;

        exportItemName(doc, "5.8");

        exportReport(builder, db5_8.GetList(), "Week5_8", Week5_8(db5_8.GetList()));

        #endregion

        #endregion

        #region 計劃書6

        exportItemName(doc, "6");

        #region 計劃書6.1

        db6_1._年度 = thisYear;

        exportItemName(doc, "6.1");

        exportReport(builder, db6_1.GetList(), "Week6_1", Week6_1(db6_1.GetList()));

        #endregion

        #region 計劃書6.2

        db6_2._年度 = thisYear;

        exportItemName(doc, "6.2");

        #region 計劃書6.2總工作報告會議

        exportReport(builder, db6_2.GetList1(), "Week6_2_A", Week6_2_A(db6_2.GetList1()));

        #endregion

        #region 計劃書6.2繳交報告及配合事項

        exportReport(builder, db6_2.GetList2(), "Week6_2_B", Week6_2_B(db6_2.GetList2()));

        #endregion

        #region 計劃書6.2專家及業者會議

        exportReport(builder, db6_2.GetList3(), "Week6_2_C", Week6_2_C(db6_2.GetList3()));

        #endregion

        #region 計劃書6.2臨時交辦議題配合事項

        exportReport(builder, db6_2.GetList4(), "Week6_2_D", Week6_2_D(db6_2.GetList4()));

        #endregion

        #region 計劃書6.2工作討論會議統計

        exportReport(builder, db6_2.GetList5(), "Week6_2_E", Week6_2_E(db6_2.GetList5()));

        #endregion

        #endregion

        #endregion

        doc.Save(Server.MapPath("~/Sample/" + newName + ".doc"));


        Response.AddHeader("content-disposition", "attachment;filename=" + Server.UrlEncode(newName + ext));
        Response.ContentType = "application/octet-stream";
        Response.WriteFile(Server.MapPath("~/Sample/" + newName + ext));
        Response.Flush();
        File.Delete(Server.MapPath("~/Sample/" + newName + ext));
        Response.End();
    }

    #region 週報表格

    private string WeekPage(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
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

    #endregion

    #region 計劃書1表格

    #region 計劃書1.1表格

    private string Week1_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.2表格

    private string Week1_2(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.3表格

    #region 計劃書1.3.1表格

    private string Week1_3_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.3.2表格

    private string Week1_3_2(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查核單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>管線</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>儲槽</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>災害救防</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>能源署</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>中油</font></br><font face='標楷體'>儲運處</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["管線"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["儲槽"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["災害救防"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["能源局"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["中油儲運處"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.3.3表格

    private string Week1_3_3(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查核單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.3.4表格

    private string Week1_3_4(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查核單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.3.5表格

    private string Week1_3_5(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查核單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.3.6表格

    private string Week1_3_6(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查核單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.3.7表格

    private string Week1_3_7(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書1.4表格

    #region 計劃書1.4.1表格

    private string Week1_4_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.4.2表格

    private string Week1_4_2(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.4.3表格

    private string Week1_4_3(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.4.4表格

    #region 計劃書1.4.4列表表格

    private string Week1_4_4_A(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>預定日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書1.4.4檢測列表表格

    private string Week1_4_4_B(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>檢測單位</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>站場名稱</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>預定日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>洩漏位置</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>洩漏源甲烷濃度</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>改善情形</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["檢測單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["站場名稱"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["洩漏位置"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["洩漏源甲烷濃度"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["改善情形"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書1.4.5表格

    private string Week1_4_5(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書1.5表格

    private string Week1_5(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書2表格

    #region 計劃書2.1表格

    private string Week2_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書2.2表格

    #region 計劃書2.2.1表格

    private string Week2_2_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書2.2.2表格

    private string Week2_2_2(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查單位</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>管線名稱</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>管線識別碼</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>起點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>起點</font></td>";
        tmpstr += "<td align='center' width='150'><font face='標楷體'>檢測期程</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>檢測長度</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期起"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["管線名稱"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["管線識別碼"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["起點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["迄點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期起"].ToString().Trim()) + "~" + getWeekDay(dt.Rows[j]["預定日期迄"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["檢測長度"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書2.2.3表格

    private string Week2_2_3(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期起"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期起"].ToString().Trim()) + "~" + getWeekDay(dt.Rows[j]["預定日期迄"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書2.3表格

    #region 計劃書2.3.1表格

    private string Week2_3_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>受查單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書2.3.2表格

    private string Week2_3_2(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>受查單位</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>管線名稱</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>管線識別碼</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>起點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>起點</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>檢測期程</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>檢測長度</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期起"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["受查單位"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["管線名稱"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["管線識別碼"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["起點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["迄點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期起"].ToString().Trim()) + "~" + getWeekDay(dt.Rows[j]["預定日期迄"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["檢測長度"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書2.3.3表格

    private string Week2_3_3(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>執行日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書2.4表格

    private string Week2_4(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書3表格

    #region 計劃書3.1表格

    private string Week3_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書3.2表格

    private string Week3_2(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書3.3表格

    private string Week3_3(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書3.4表格

    private string Week3_4(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書3.5表格

    private string Week3_5(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書3.6表格

    private string Week3_6(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書3.7表格

    private string Week3_7(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書3.8表格

    private string Week3_8(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書3.9表格

    private string Week3_9(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書4表格

    #region 計劃書4.1表格

    private string Week4_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書4.2表格

    private string Week4_2(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書4.3表格

    private string Week4_3(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書5表格

    #region 計劃書5.1表格

    private string Week5_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>委員</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["委員"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書5.2表格

    private string Week5_2(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書5.3表格

    private string Week5_3(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書5.4表格

    private string Week5_4(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書5.5表格

    private string Week5_5(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書5.6表格

    private string Week5_6(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>場次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書5.7表格

    #region 計劃書5.7審閱進度表格

    private string Week5_7_A(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>公用天然氣事業</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>能源署發文日期</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>業者繳交日期</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>工研院審閱日期</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>補正情形</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["工研院審閱日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["公用天然氣事業"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["能源局發文日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["業者繳交日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["工研院審閱日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["補正情形"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書5.7其他災防事項

    private string Week5_7_B(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>交辦日期</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>完成日期</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期迄"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期起"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期迄"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書5.8表格

    private string Week5_8(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>執行內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #region 計劃書6表格

    #region 計劃書6.1表格

    private string Week6_1(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='70'><font face='標楷體'>編號</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>查核點內容說明</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>預定完成日期</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>完成日期</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            string tmpno = string.Empty;

            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期起"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            if (dt.Rows[j]["編號3"].ToString().Trim() != "0")
                tmpno += "." + dt.Rows[j]["編號3"].ToString().Trim();
            if (dt.Rows[j]["編號4"].ToString().Trim() != "0")
                tmpno += "." + dt.Rows[j]["編號4"].ToString().Trim();
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["編號1"].ToString().Trim() + "." + dt.Rows[j]["編號2"].ToString().Trim() + tmpno + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期起"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期迄"].ToString().Trim()) + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書6.2表格

    #region 計劃書6.2總工作報告會議表格

    private string Week6_2_A(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td colspan='7' align='center'><font face='標楷體'>總工作報告會議</font></td>";
        tmpstr += "</tr>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>工作內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書6.2繳交報告及配合事項表格

    private string Week6_2_B(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td colspan='5' align='center'><font face='標楷體'>繳交報告及配合事項</font></td>";
        tmpstr += "</tr>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>工作內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書6.2專家及業者會議表格

    private string Week6_2_C(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td colspan='6' align='center'><font face='標楷體'>臨時交辦議題配合事項(專家及業者會議)</font></td>";
        tmpstr += "</tr>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>工作內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書6.2臨時交辦議題配合事項表格

    private string Week6_2_D(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td colspan='5' align='center'><font face='標楷體'>臨時交辦議題配合事項</font></td>";
        tmpstr += "</tr>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>工作內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>文件名稱</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["文件名稱"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #region 計劃書6.2工作討論會議統計表格

    private string Week6_2_E(DataTable dt)
    {
        string tmpstr = string.Empty;
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td colspan='7' align='center'><font face='標楷體'>工作討論會議統</font></td>";
        tmpstr += "</tr>";
        tmpstr += "<tr style='background-color:#C4E1FF'>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>項次</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>工作內容</font></td>";
        tmpstr += "<td align='center' width='100'><font face='標楷體'>時間</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>地點</font></td>";
        tmpstr += "<td align='center' width='50'><font face='標楷體'>人數</font></td>";
        tmpstr += "<td align='center'><font face='標楷體'>備註</font></td>";
        tmpstr += "</tr>";

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if ((Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19110000) > Convert.ToInt32(dt.Rows[j]["預定日期"].ToString().Trim()))
                color = "#E8FFE8";
            else
                color = "#FFFFFF";

            tmpstr += "<tr style='background-color:" + color + "'>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["場次"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["執行內容"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + getWeekDay(dt.Rows[j]["預定日期"].ToString().Trim()) + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["地點"].ToString().Trim() + "</font></td>";
            tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[j]["人數"].ToString().Trim() + "</font></td>";
            tmpstr += "<td><font face='標楷體'>" + dt.Rows[j]["備註"].ToString().Trim() + "</font></td>";
            tmpstr += "</tr>";
        }
        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    #endregion

    #endregion

    public void exportItemName(Document doc, string sn)
    {
        DataTable dt = new DataTable();
        dt = rdb.GetWeekReportExport(sn);

        if (dt.Rows.Count > 0)
        {
            doc.Range.Replace("@" + sn + "@", dt.Rows[0]["名稱"].ToString().Trim(), false, false);
        }

        dt = null;
    }

    public void exportReport(DocumentBuilder builder, DataTable dt, string sn, string dtstr)
    {
        builder.MoveToBookmark(sn);

        if (dt.Rows.Count > 0)
        {
            builder.InsertHtml(dtstr);
        }

        dt = null;
    }

    //轉換日期 ex: 1100505 => 05.05(二)
    public string getWeekDay(string fulldate)
    {
        if (!string.IsNullOrEmpty(fulldate))
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
        else
        {
            return "";
        }        
    }
}