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

public partial class Handler_OilEvaluationReport : System.Web.UI.Page
{
    OilCompanyInfo_DB ocdb = new OilCompanyInfo_DB();
    OilAllSuggestion_DB oasdb = new OilAllSuggestion_DB();
    OilMasterCompare_DB omcdb = new OilMasterCompare_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        // 在新的工作階段啟動時執行的程式碼
        Aspose.Words.License license = new Aspose.Words.License();
        license.SetLicense(Server.MapPath("~/Bin/Aspose.Total.lic"));

        string cpid = (string.IsNullOrEmpty(Request["cp"])) ? "" : Request["cp"].ToString().Trim();
        string cpName = string.Empty;

        ocdb._guid = cpid;
        dt = ocdb.GetCpName2();

        if (dt.Rows.Count > 0)
        {
            cpName = dt.Rows[0]["cpname"].ToString().Trim();
        }
        dt = null;

        //新檔名
        string newName = getTaiwanYear() + "年石油管線及儲槽查核結果與建議_" + cpName;
        //副檔名
        string ext = ".doc";

        Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Sample/OilEvaluation.doc"));
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

        oasdb._年度 = getTaiwanYear();
        oasdb._業者guid = cpid;
        dt = oasdb.GetAllEvaluation();
        DataTable dt05 = oasdb.GetAllEvaluation05();

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

        omcdb._年度 = getTaiwanYear();
        omcdb._業者guid = cpid;
        DataTable dt2 = omcdb.GetCommitteeList();
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

        #region 查核結果及建議事項

        if (dt.Rows.Count > 0)
        {
            int Pno = 0;
            int Tno = 0;
            int Cno = 0;
            int Dno = 0;

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                string type = dt.Rows[k]["分類"].ToString().Trim();
                string code = string.Empty;

                if (type != "5")
                {

                    tmpstr += "<tr>";

                    #region 項目

                    tmpstr += "<td valign='middle'><font face='標楷體'>" + dt.Rows[k]["項目"].ToString().Trim() + "</font></td>";

                    #endregion

                    #region 分類

                    string gName = dt.Rows[k]["石油自評表分類名稱"].ToString().Trim();
                    string[] gArr = Regex.Split(gName, "[(]文");
                    gName = gArr[0];
                    tmpstr += "<td align='center'><font face='標楷體'>" + gName + "</font></td>";

                    #endregion

                    #region 編號

                    string temstr = string.Empty;

                    switch (type)
                    {
                        case "1":
                            code = "P";
                            Pno++;
                            if (Pno.ToString().Length == 1)
                            {
                                if (Pno == 10)
                                {
                                    temstr = Pno.ToString();
                                }
                                else
                                {
                                    temstr = "0" + Pno.ToString();
                                }
                            }
                            else
                            {
                                temstr = Pno.ToString();
                            }
                            break;
                        case "2":
                            code = "T";
                            Tno++;
                            if (Tno.ToString().Length == 1)
                            {
                                if (Tno == 10)
                                {
                                    temstr = Tno.ToString();
                                }
                                else
                                {
                                    temstr = "0" + Tno.ToString();
                                }
                            }
                            else
                            {
                                temstr = Tno.ToString();
                            }
                            break;
                        case "3":
                            code = "C";
                            Cno++;
                            if (Cno.ToString().Length == 1)
                            {
                                if(Cno == 10)
                                {
                                    temstr = Cno.ToString();
                                }
                                else
                                {
                                    temstr = "0" + Cno.ToString();
                                }
                            }
                            else
                            {
                                temstr = Cno.ToString();
                            }
                            break;
                        case "4":
                            code = "D";
                            Dno++;
                            if (Dno.ToString().Length == 1)
                            {
                                if (Dno == 10)
                                {
                                    temstr = Dno.ToString();
                                }
                                else
                                {
                                    temstr = "0" + Dno.ToString();
                                }
                            }
                            else
                            {
                                temstr = Dno.ToString();
                            }
                            break;
                    }

                    sn = getTaiwanYear() + "-" + temstr + code;

                    tmpstr += "<td align='center'><font face='標楷體'>" + sn + "</font></td>";

                    #endregion

                    #region 查核結果及建議事項

                    tmpstr += "<td align='center'><font face='標楷體'>" + dt.Rows[k]["委員意見"].ToString().Trim() + "</font></td>";

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
            tmpstr += "<tr><td align='left' colspan='5'><font face='標楷體'>目前沒有查核結果及建議事項</font></td></tr>";
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