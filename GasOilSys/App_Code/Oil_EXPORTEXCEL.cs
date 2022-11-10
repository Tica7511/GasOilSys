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
/// Oil_EXPORTEXCEL 的摘要描述
/// </summary>
namespace ED.HR.Oil_EXPORTEXCEL.WebForm
{
    public partial class Excel : System.Web.UI.Page
    {
        OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
        OilTubeInfo_DB db1 = new OilTubeInfo_DB();
        OilTubeComplete_DB db2 = new OilTubeComplete_DB();
        OilCheckSmartTubeCleaner_DB db3 = new OilCheckSmartTubeCleaner_DB();
        OilCIPS_DB db4 = new OilCIPS_DB();
        OilUnusualRectifier_DB db5 = new OilUnusualRectifier_DB();
        OilTubeCheck_DB db6 = new OilTubeCheck_DB();
        OilTubeMaintain_DB db7 = new OilTubeMaintain_DB();

        protected void Page_Load(object sender, EventArgs e)
        {
            string category = Common.FilterCheckMarxString(Request.QueryString["category"]);
            string year = Common.FilterCheckMarxString(Request.QueryString["year"]);
            string cpid = Common.FilterCheckMarxString(Request.QueryString["cpid"]);
            string cpName = string.Empty;
            string fileName = string.Empty;

            string FilePath = Server.MapPath("~/Sample/Oil_ExportExcel.xls");
            HSSFWorkbook hssfworkbook;
            FileStream sampleFile;

            sampleFile = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

            using (sampleFile)
            {
                //建立Excel
                hssfworkbook = new HSSFWorkbook(sampleFile);
            }

            DataTable dt = new DataTable();

            odb._guid = cpid;
            dt = odb.GetCpName();
            if (dt.Rows.Count > 0)
            {
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

                    hssfworkbook.SetSheetName(0, "管線基本資料");
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
                    sheet.GetRow(0).CreateCell(10).SetCellValue("轄管長度(公里)");
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
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["轄區長途管線名稱"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["銜接管線識別碼_上游"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["銜接管線識別碼_下游"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["起點"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["迄點"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["管徑吋"].ToString().Trim());
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
                    #region 管線完整性管理作為

                    db2._業者guid = cpid;
                    db2._年度 = year;
                    dt = db2.GetList();

                    hssfworkbook.SetSheetName(0, "管線完整性管理作為");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("長途管線識別碼");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("風險評估 年/月");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("智慧型通管器(ILI) 可行性");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("耐壓強度試驗(TP) 可行性");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("緊密電位(CIPS) 年/月");
                    sheet.GetRow(0).CreateCell(5).SetCellValue("電磁包覆(PCM) 年/月");
                    sheet.GetRow(0).CreateCell(6).SetCellValue("智慧型通管器(ILI) 年/月");
                    sheet.GetRow(0).CreateCell(7).SetCellValue("耐壓強度試驗(TP) 年/月");
                    sheet.GetRow(0).CreateCell(8).SetCellValue("耐壓強度試驗(TP) 介質");
                    sheet.GetRow(0).CreateCell(9).SetCellValue("試壓壓力與MOP壓力倍數");
                    sheet.GetRow(0).CreateCell(10).SetCellValue("耐壓強度試驗(TP)持壓時間(小時)");
                    sheet.GetRow(0).CreateCell(11).SetCellValue("受雜散電流影響");
                    sheet.GetRow(0).CreateCell(12).SetCellValue("洩漏偵測系統(LLDS)");
                    sheet.GetRow(0).CreateCell(13).SetCellValue("強化作為");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["長途管線識別碼"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["風險評估年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["智慧型通管器ILI可行性"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["耐壓強度試驗TP可行性"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["緊密電位CIPS年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["電磁包覆PCM年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["智慧型通管器ILI年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["耐壓強度試驗TP年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["耐壓強度試驗TP介質"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["試壓壓力與MOP壓力倍數"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["耐壓強度試驗TP持壓時間"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(11).SetCellValue(dt.Rows[i]["受雜散電流影響"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(12).SetCellValue(dt.Rows[i]["洩漏偵測系統"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(13).SetCellValue(dt.Rows[i]["強化作為"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_管線完整性管理作為.xls";

                    #endregion
                    break;
                case "checksmarttubecleaner":
                    #region 智慧型通管器檢查(ILI)

                    db3._業者guid = cpid;
                    db3._年度 = year;
                    dt = db3.GetList();

                    hssfworkbook.SetSheetName(0, "智慧型通管器檢查(ILI)");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("長途管線識別碼");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("檢測方法");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("最近一次執行 年/月");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("報告產出 年/月");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("檢測長度 公里");
                    sheet.GetRow(0).CreateCell(5).SetCellValue("管壁減薄30%-40%數量_內部腐蝕數量");
                    sheet.GetRow(0).CreateCell(6).SetCellValue("管壁減薄30%-40%數量_內部開挖確認數量");
                    sheet.GetRow(0).CreateCell(7).SetCellValue("管壁減薄30%-40%數量_外部腐蝕數量");
                    sheet.GetRow(0).CreateCell(8).SetCellValue("管壁減薄30%-40%數量_外部開挖確認數量");
                    sheet.GetRow(0).CreateCell(9).SetCellValue("管壁減薄40%-50%數量_內部腐蝕數量");
                    sheet.GetRow(0).CreateCell(10).SetCellValue("管壁減薄40%-50%數量_內部開挖確認數量");
                    sheet.GetRow(0).CreateCell(11).SetCellValue("管壁減薄40%-50%數量_外部腐蝕數量");
                    sheet.GetRow(0).CreateCell(12).SetCellValue("管壁減薄40%-50%數量_外部開挖確認數量");
                    sheet.GetRow(0).CreateCell(13).SetCellValue("管壁減薄50%以上數量_內部腐蝕數量");
                    sheet.GetRow(0).CreateCell(14).SetCellValue("管壁減薄50%以上數量_內部開挖確認數量");
                    sheet.GetRow(0).CreateCell(15).SetCellValue("管壁減薄50%以上數量_外部腐蝕數量");
                    sheet.GetRow(0).CreateCell(16).SetCellValue("管壁減薄50%以上數量_外部開挖確認數量");
                    sheet.GetRow(0).CreateCell(17).SetCellValue("Dent_變形量>12%數量");
                    sheet.GetRow(0).CreateCell(18).SetCellValue("Dent_開挖確認數量");
                    sheet.GetRow(0).CreateCell(19).SetCellValue("備註");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["長途管線識別碼"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["檢測方法"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["最近一次執行年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["報告產出年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["檢測長度公里"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["減薄數量_內1"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["減薄數量_內_開挖確認1"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["減薄數量_外"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["減薄數量_外_開挖確認1"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["減薄數量_內2"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["減薄數量_內_開挖確認2"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(11).SetCellValue(dt.Rows[i]["減薄數量_外2"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(12).SetCellValue(dt.Rows[i]["減薄數量_外_開挖確認2"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(13).SetCellValue(dt.Rows[i]["減薄數量_內3"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(14).SetCellValue(dt.Rows[i]["減薄數量_內_開挖確認3"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(15).SetCellValue(dt.Rows[i]["減薄數量_外3"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(16).SetCellValue(dt.Rows[i]["減薄數量_外_開挖確認3"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(17).SetCellValue(dt.Rows[i]["Dent"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(18).SetCellValue(dt.Rows[i]["Dent_開挖確認"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(19).SetCellValue(dt.Rows[i]["備註"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_智慧型通管器檢查(ILI).xls";

                    #endregion
                    break;
                case "cips":
                    #region 緊密電位檢測(CIPS)

                    db4._業者guid = cpid;
                    db4._年度 = year;
                    dt = db4.GetList();

                    hssfworkbook.SetSheetName(0, "緊密電位檢測(CIPS)");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("長途管線識別碼");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("同時檢測管線數量");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("最近一次執行 年/月");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("報告產出 年/月");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("檢測長度 公里");
                    sheet.GetRow(0).CreateCell(5).SetCellValue("合格標準");
                    sheet.GetRow(0).CreateCell(6).SetCellValue("立即改善_數量");
                    sheet.GetRow(0).CreateCell(7).SetCellValue("立即改善_改善完成數量");
                    sheet.GetRow(0).CreateCell(8).SetCellValue("排程改善_數量");
                    sheet.GetRow(0).CreateCell(9).SetCellValue("排程改善_改善完成數量");
                    sheet.GetRow(0).CreateCell(10).SetCellValue("需監控點_數量");
                    sheet.GetRow(0).CreateCell(11).SetCellValue("備註");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["長途管線識別碼"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["同時檢測管線數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["最近一次執行年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["報告產出年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["檢測長度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["合格標準"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["立即改善_數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["立即改善_改善完成數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["排成改善_數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["排成改善_改善完成數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["需監控點數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(11).SetCellValue(dt.Rows[i]["備註"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_緊密電位檢測(CIPS).xls";

                    #endregion
                    break;
                case "unusualrectifier":
                    #region 異常整流站

                    db5._業者guid = cpid;
                    db5._年度 = year;
                    dt = db5.GetList();

                    hssfworkbook.SetSheetName(0, "異常整流站");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("異常整流站名稱");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("異常起始日期 (年/月)");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("異常狀況");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("整流站修復進度 1.公司報修2.設計中3.向地方主管機關提出申請中4.修復中");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("影響長途管線識別碼");
                    sheet.GetRow(0).CreateCell(5).SetCellValue("預計完成日期");
                    sheet.GetRow(0).CreateCell(6).SetCellValue("備註");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["異常整流站名稱"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["異常起始日期年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["異常狀況"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["整流站修復進度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["影響長途管線識別碼"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["預計完成日期"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["備註"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_異常整流站.xls";

                    #endregion
                    break;
                case "tubecheck":
                    #region 管線巡檢

                    db6._業者guid = cpid;
                    db6._年度 = year;
                    dt = db6.GetList();

                    hssfworkbook.SetSheetName(0, "管線巡檢");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("依據文件名稱");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("文件編號");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("文件日期");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("每日巡檢次數 01:1次 02:2次 03:3次(含)以上");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("巡管人數");
                    sheet.GetRow(0).CreateCell(5).SetCellValue("巡管工具 01:PDA 02:手機 03:其他");
                    sheet.GetRow(0).CreateCell(6).SetCellValue("巡管工具其他");
                    sheet.GetRow(0).CreateCell(7).SetCellValue("主管監督查核 Y:有 N:無");
                    sheet.GetRow(0).CreateCell(8).SetCellValue("主管監督查核次");
                    sheet.GetRow(0).CreateCell(9).SetCellValue("是否有加強巡檢點 Y:有 N:無");
                    sheet.GetRow(0).CreateCell(10).SetCellValue("是否有加強巡檢點敘述");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["依據文件名稱"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["文件編號"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["文件日期"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["每日巡檢次數"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["巡管人數"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["巡管工具"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["巡管工具其他"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["主管監督查核"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["主管監督查核次"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["是否有加強巡檢點"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["是否有加強巡檢點敘述"].ToString().Trim());
                        }
                    }

                    dt.Clear();
                    dt = db6.GetList2();

                    hssfworkbook.CreateSheet("異常情形統計資料");
                    sheet = hssfworkbook.GetSheetAt(1);
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("管線巡檢情形");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("前兩年");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("前一年");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["管線巡檢情形"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["前兩年"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["前一年"].ToString().Trim());
                        }
                    }


                    fileName = cpName + "_風險評估.xls";

                    #endregion
                    break;
                case "tubemaintain":
                    #region 管線維修或開挖

                    db7._業者guid = cpid;
                    db7._年度 = year;
                    dt = db7.GetList();

                    hssfworkbook.SetSheetName(0, "管線維修或開挖");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("長途管線識別碼");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("前一年度 1.維修2.換管3.遷管4.開挖");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("長度(公尺)");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("管段位置");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("備註");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["長途管線識別碼"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["前一年度管線變更性質"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["長度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["管段位置"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["備註"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_管線維修或開挖.xls";

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