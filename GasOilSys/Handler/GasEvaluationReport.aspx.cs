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
using System.Globalization;
using System.Text.RegularExpressions;

public partial class Handler_GasEvaluationReport : System.Web.UI.Page
{
    GasCompanyInfo_DB gcdb = new GasCompanyInfo_DB();
    GasAllSuggestion_DB gasdb = new GasAllSuggestion_DB();
    GasMasterCompare_DB gmcdb = new GasMasterCompare_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        // 在新的工作階段啟動時執行的程式碼
        Aspose.Words.License license = new Aspose.Words.License();
        license.SetLicense(Server.MapPath("~/Bin/Aspose.Total.lic"));

        string cpid = (string.IsNullOrEmpty(Request["cp"])) ? "" : Request["cp"].ToString().Trim();
        string cpName = string.Empty;

        gcdb._guid = cpid;
        dt = gcdb.GetCpName2();

        if (dt.Rows.Count > 0)
        {
            if (cpid == "9E779E2B-C36D-44BF-BED2-11C29D989D53")
                cpName = dt.Rows[0]["公司名稱"].ToString().Trim();
            else
                cpName = dt.Rows[0]["cpname"].ToString().Trim();
        }
        dt = null;

        //新檔名
        string newName = getTaiwanYear() + "年天然氣管線及儲槽查核結果與建議_" + cpName;
        //副檔名
        string ext = ".doc";

        Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Sample/GasEvaluation.doc"));
        DocumentBuilder builder = new DocumentBuilder(doc);
        builder.ParagraphFormat.Style.Font.Name = "標楷體";
        builder.ParagraphFormat.Style.Font.Size = 14;

        #region 查核日期

        doc.Range.Replace("@year", getTaiwanYear(), false, false);

        #endregion

        #region 業者名稱

        doc.Range.Replace("@companyName", cpName, false, false);

        #endregion

        #region 報告

        gasdb._年度 = getTaiwanYear();
        gasdb._業者guid = cpid;
        dt = gasdb.GetAllEvaluation();
        DataTable dt05 = gasdb.GetAllEvaluation05();

        builder.MoveToBookmark("ThisPage");

        string dtstr = SheetBuild(dt, dt05, cpid);
        builder.InsertHtml(dtstr);

        dt = null;

        #endregion

        doc.Save(Server.MapPath("~/Sample/" + newName + ".doc"));

        Response.AddHeader("content-disposition", "attachment;filename=" + Server.UrlEncode(newName + ext));
        Response.ContentType = "application/octet-stream";
        Response.WriteFile(Server.MapPath("~/Sample/" + newName + ext));
        Response.Flush();
        File.Delete(Server.MapPath("~/Sample/" + newName + ext));
        Response.End();
    }

    #region 表格內容

    private string SheetBuild(DataTable dt, DataTable dt05, string cpid)
    {
        string committee = string.Empty;
        string tmpstr = string.Empty;
        string sn = string.Empty;

        #region 表頭列表

        gmcdb._年度 = getTaiwanYear();
        gmcdb._業者guid = cpid;
        DataTable dt2 = gmcdb.GetCommitteeList();
        for (int j = 0; j < dt2.Rows.Count; j++)
        {
            if (j > 0)
                committee += "、" + dt2.Rows[j]["委員姓名"].ToString().Trim();
            else
                committee += dt2.Rows[j]["委員姓名"].ToString().Trim();
        }
        tmpstr += "<table border='1' cellpadding='0' cellspacing='0' width='100%'>";
        tmpstr += "<tr style='background-color:#CAFFFF'>";
        tmpstr += "<td align='center' colspan='3'><font face='標楷體'>查核<br>日期</font></td>";
        tmpstr += "<td align='left' colspan='2'><font face='標楷體'>" + getTaiwanYear() + "年" + DateTime.Now.Month.ToString() + "月" +
            DateTime.Now.Day.ToString() + "日" + "</font></td></tr>";
        tmpstr += "<tr style='background-color:#CAFFFF'>";
        tmpstr += "<td align='center' colspan='3'><font face='標楷體'>查核委員</font></td>";
        tmpstr += "<td align='left' colspan='2'><font face='標楷體'>" + committee + "</font></td></tr>";
        tmpstr += "<tr style='background-color:#CAFFFF'>";
        tmpstr += "<td align='center' colspan='3'><font face='標楷體'>主管機關出席人員</font></td>";
        tmpstr += "<td colspan='2'><font face='標楷體'></font></td></tr>";
        tmpstr += "<tr style='background-color:#CAFFFF'>";
        tmpstr += "<td align='center' colspan='3'><font face='標楷體'>事業單位陪檢人員</font></td>";
        tmpstr += "<td colspan='2'><font face='標楷體'></font></td></tr>";
        tmpstr += "<tr style='background-color:#CAFFFF'>";
        tmpstr += "<td align='center' width='5%'><font face='標楷體'>項<br>目</font></td>";
        tmpstr += "<td align='center' width='5%'><font face='標楷體'>分類</font></td>";
        tmpstr += "<td align='center' width='5%'><font face='標楷體'>編號</font></td>";
        tmpstr += "<td align='center' width='75%'><font face='標楷體'>查核結果及建議事項</font></td>";
        tmpstr += "<td align='center' width='10%'><font face='標楷體'>查核建<br>議等級</font></td>";

        #endregion

        #region 一、書面及現場查核

        tmpstr += "<tr><td align='left' colspan='5'><font face='標楷體'>一、書面及現場查核";

        if (dt.Rows.Count > 0)
        {
            tmpstr += "</font></td></tr>";

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                string type = dt.Rows[k]["分類"].ToString().Trim();

                if (type != "6")
                {
                    tmpstr += "<tr>";

                    #region 項目

                    tmpstr += "<td valign='middle'><font face='標楷體'>" + dt.Rows[k]["項目"].ToString().Trim() + "</font></td>";

                    #endregion

                    #region 分類

                    string gName = dt.Rows[k]["天然氣自評表分類名稱"].ToString().Trim();
                    string[] gArr = Regex.Split(gName, "[(]文");
                    gName = gArr[0];
                    tmpstr += "<td align='center'><font face='標楷體'>" + gName + "</font></td>";

                    #endregion

                    #region 編號

                    gcdb._guid = cpid;
                    DataTable dt3 = gcdb.GetCpName2();

                    if (dt3.Rows.Count > 0)
                    {
                        string temstr = string.Empty;

                        if (k.ToString().Length == 1)
                        {
                            temstr = "0" + (k + 1).ToString();
                        }
                        else
                        {
                            temstr = (k + 1).ToString();
                        }

                        sn = getTaiwanYear() + dt3.Rows[0]["代碼"].ToString().Trim() + "-0A" + temstr;
                    }

                    tmpstr += "<td align='center'><font face='標楷體'>" + sn + "</font></td>";

                    #endregion

                    #region 查核結果及建議事項

                    tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[k]["委員意見"].ToString().Trim() + "</font></td>";

                    #endregion

                    #region 查核建議等級

                    tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[k]["委員"].ToString().Trim() + "</font></td>";

                    #endregion

                    tmpstr += "</tr>";
                }
            }
        }
        else
        {
            tmpstr += "<br>&nbsp;&nbsp;&nbsp;&nbsp;經查核結果：<u>目前尚未有查核意見</u></font></td></tr>";
        }

        #endregion

        #region 二、法規面查核法規面查核

        tmpstr += "<tr><td align='left' colspan='5'><font face='標楷體'>二、法規面查核法規面查核";

        if (dt05.Rows.Count > 0)
        {
            tmpstr += "</font></td></tr>";

            for (int w = 0; w < dt05.Rows.Count; w++)
            {
                string type = dt05.Rows[w]["分類"].ToString().Trim();

                if (type == "6")
                {
                    tmpstr += "<tr>";

                    #region 項目

                    tmpstr += "<td valign='middle'><font face='標楷體'>" + dt05.Rows[w]["項目"].ToString().Trim() + "</font></td>";

                    #endregion

                    #region 分類

                    string gName = dt05.Rows[w]["天然氣自評表分類名稱"].ToString().Trim();
                    string[] gArr = Regex.Split(gName, "[(]文");
                    gName = gArr[0];
                    tmpstr += "<td align='center'><font face='標楷體'>" + gName + "</font></td>";

                    #endregion

                    #region 編號

                    gcdb._guid = cpid;
                    DataTable dt3 = gcdb.GetCpName2();

                    if (dt3.Rows.Count > 0)
                    {
                        string temstr = string.Empty;

                        if (w.ToString().Length == 1)
                        {
                            temstr = "0" + (w + 1).ToString();
                        }

                        sn = getTaiwanYear() + dt3.Rows[0]["代碼"].ToString().Trim() + "OA" + temstr;
                    }

                    tmpstr += "<td align='center'><font face='標楷體'>" + sn + "</font></td>";

                    #endregion

                    #region 查核結果及建議事項

                    tmpstr += "<td align='center'><font face='標楷體'>" + dt05.Rows[w]["委員意見"].ToString().Trim() + "</font></td>";

                    #endregion

                    #region 查核建議等級

                    tmpstr += "<td align='center'><font face='標楷體'></font></td>";

                    #endregion

                    tmpstr += "</tr>";
                }
            }
        }
        else
        {
            tmpstr += "<br>&nbsp;&nbsp;&nbsp;&nbsp;經查核結果：<u>未發現不符合法規之情形</u></font></td></tr>";
        }

        #endregion

        tmpstr += "</table>";

        return tmpstr;
    }

    #endregion

    public string getTaiwanYear()
    {
        TaiwanCalendar taiwanDate = new TaiwanCalendar();
        string taiwanYear = taiwanDate.GetYear(DateTime.Now).ToString();

        return taiwanYear;
    }
}