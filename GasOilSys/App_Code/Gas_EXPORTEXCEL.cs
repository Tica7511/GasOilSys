using System;
using System.Web;
using System.Configuration;
using System.Net;
using System.Data;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

/// <summary>
/// Gas_EXPORTEXCEL 的摘要描述
/// </summary>
namespace ED.HR.Gas_EXPORTEXCEL.WebForm
{
    public partial class Excel : System.Web.UI.Page
    {
        GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
        GasTubeInfo_DB db1 = new GasTubeInfo_DB();
        GasTubeComplete_DB db2 = new GasTubeComplete_DB();
        protected void Page_Load(object sender, EventArgs e)
        {
            string category = Common.FilterCheckMarxString(Request.QueryString["category"]);
            string year = Common.FilterCheckMarxString(Request.QueryString["year"]);
            string cpid = Common.FilterCheckMarxString(Request.QueryString["cpid"]);
            string cpName = string.Empty;
            string fileName = string.Empty;

            string FilePath = Server.MapPath("~/Sample/Gas_ExportExcel.xls");
            HSSFWorkbook hssfworkbook;
            FileStream sampleFile;

            sampleFile = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

            using (sampleFile)
            {
                //建立Excel
                hssfworkbook = new HSSFWorkbook(sampleFile);
            }

            DataTable dt = new DataTable();

            gdb._guid = cpid;
            dt = gdb.GetCpName();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["guid"].ToString().Trim() == "9E779E2B-C36D-44BF-BED2-11C29D989D53")
                    dt.Rows[0]["cpname"] = dt.Rows[0]["公司名稱"].ToString().Trim();

                cpName = dt.Rows[0]["cpname"].ToString().Trim();
            }

            dt.Clear();

            Sheet sheet = hssfworkbook.GetSheetAt(0);

            switch (category)
            {
                case "tubeinfo":
                    #region 管線基本資料

                    db1._業者guid = cpid;
                    dt = db1.GetExportList();

                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("長途管線識別碼");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("轄區長途管線名稱(公司)");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("銜接管線識別碼(上游)");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("銜接管線識別碼(下游)");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("起點");
                    sheet.GetRow(0).CreateCell(5).SetCellValue("迄點");
                    sheet.GetRow(0).CreateCell(6).SetCellValue("管徑(吋)");
                    sheet.GetRow(0).CreateCell(7).SetCellValue("厚度(mm)");
                    sheet.GetRow(0).CreateCell(8).SetCellValue("管材(詳細規格)");
                    sheet.GetRow(0).CreateCell(9).SetCellValue("包覆材料");
                    sheet.GetRow(0).CreateCell(10).SetCellValue("轄管長度(公尺)");
                    sheet.GetRow(0).CreateCell(11).SetCellValue("內容物");
                    sheet.GetRow(0).CreateCell(12).SetCellValue("緊急遮斷閥(處)");
                    sheet.GetRow(0).CreateCell(13).SetCellValue("建置年(民國年月)");
                    sheet.GetRow(0).CreateCell(14).SetCellValue("設計壓力(Kg/cm2)");
                    sheet.GetRow(0).CreateCell(15).SetCellValue("使用壓力(Kg/cm2)");
                    sheet.GetRow(0).CreateCell(16).SetCellValue("使用狀態1.使用中2.停用3.備用");
                    sheet.GetRow(0).CreateCell(17).SetCellValue("附掛橋樑數量");
                    sheet.GetRow(0).CreateCell(18).SetCellValue("管線穿越箱涵數量");
                    sheet.GetRow(0).CreateCell(19).SetCellValue("活動斷層敏感區1.有2.無");
                    sheet.GetRow(0).CreateCell(20).SetCellValue("土壤液化區1.有2.無");
                    sheet.GetRow(0).CreateCell(21).SetCellValue("土石流潛勢區1.有2.無");
                    sheet.GetRow(0).CreateCell(22).SetCellValue("淹水潛勢區1.有2.無");
                    sheet.GetRow(0).CreateCell(23).SetCellValue("備註");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["長途管線識別碼"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["轄區長途管線名稱_公司"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["銜接管線識別碼_上游"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["銜接管線識別碼_下游"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["起點"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["迄點"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["管徑"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["厚度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["管材"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["包覆材料"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["轄管長度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(11).SetCellValue(dt.Rows[i]["內容物"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(12).SetCellValue(dt.Rows[i]["緊急遮斷閥"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(13).SetCellValue(dt.Rows[i]["建置年"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(14).SetCellValue(dt.Rows[i]["設計壓力"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(15).SetCellValue(dt.Rows[i]["使用壓力"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(16).SetCellValue(dt.Rows[i]["使用狀態"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(17).SetCellValue(dt.Rows[i]["附掛橋樑數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(18).SetCellValue(dt.Rows[i]["管線穿越箱涵數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(19).SetCellValue(dt.Rows[i]["活動斷層敏感區"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(20).SetCellValue(dt.Rows[i]["土壤液化區"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(21).SetCellValue(dt.Rows[i]["土石流潛勢區"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(22).SetCellValue(dt.Rows[i]["淹水潛勢區"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(23).SetCellValue(dt.Rows[i]["備註"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_管線基本資料.xls";

                    #endregion
                    break;
                case "tubecomplete":
                    #region 管線完整性管理作為_幹線及環線管線

                    db2._業者guid = cpid;
                    db2._年度 = year;
                    dt = db2.GetExportList();

                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("長途管線識別碼");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("耐壓強度試驗(TP)可行性");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("緊密電位(CIPS) 年/月");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("電磁包覆(PCM) 年/月");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("智慧型通管器(ILI) 年/月");
                    sheet.GetRow(0).CreateCell(5).SetCellValue("耐壓強度試驗(TP) 年/月");
                    sheet.GetRow(0).CreateCell(6).SetCellValue("耐壓強度試驗(TP) 介質");
                    sheet.GetRow(0).CreateCell(7).SetCellValue("試壓壓力與MOP壓力倍數");
                    sheet.GetRow(0).CreateCell(8).SetCellValue("耐壓強度試驗(TP)持壓時間(小時)");
                    sheet.GetRow(0).CreateCell(9).SetCellValue("受雜散電流影響");
                    sheet.GetRow(0).CreateCell(10).SetCellValue("洩漏偵測系統(LLDS)");
                    sheet.GetRow(0).CreateCell(11).SetCellValue("強化作為");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["長途管線識別碼"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["轄區長途管線名稱_公司"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["銜接管線識別碼_上游"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["銜接管線識別碼_下游"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["起點"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["迄點"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["管徑"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["厚度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["管材"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["包覆材料"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["轄管長度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(11).SetCellValue(dt.Rows[i]["內容物"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(12).SetCellValue(dt.Rows[i]["緊急遮斷閥"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(13).SetCellValue(dt.Rows[i]["建置年"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(14).SetCellValue(dt.Rows[i]["設計壓力"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(15).SetCellValue(dt.Rows[i]["使用壓力"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(16).SetCellValue(dt.Rows[i]["使用狀態"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(17).SetCellValue(dt.Rows[i]["附掛橋樑數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(18).SetCellValue(dt.Rows[i]["管線穿越箱涵數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(19).SetCellValue(dt.Rows[i]["活動斷層敏感區"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(20).SetCellValue(dt.Rows[i]["土壤液化區"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(21).SetCellValue(dt.Rows[i]["土石流潛勢區"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(22).SetCellValue(dt.Rows[i]["淹水潛勢區"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(23).SetCellValue(dt.Rows[i]["備註"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_管線基本資料.xls";

                    #endregion
                    break;
                case "tubecomplete2":
                    #region 管線完整性管理作為_幹線及環線管線以外

                    #endregion
                    break;
            }

            Response.ContentType = "application / vnd.ms - excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.Clear();

            using (MemoryStream ms = new MemoryStream())
            {
                hssfworkbook.Write(ms);

                Response.BinaryWrite(ms.GetBuffer());
                Response.Flush();
                Response.End();
            }
        }
    }
}