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
        GasCheckSmartTubeCleaner_DB db3 = new GasCheckSmartTubeCleaner_DB();
        GasCIPS_DB db4 = new GasCIPS_DB();
        GasUnusualRectifier_DB db5 = new GasUnusualRectifier_DB();
        GasTubeMaintain_DB db6 = new GasTubeMaintain_DB();
        GasRiskAssessment_DB db7 = new GasRiskAssessment_DB();
        GasTubeCheck_DB db8 = new GasTubeCheck_DB();
        GasControl_DB db9 = new GasControl_DB();
        GasAccidentLearning_DB db10 = new GasAccidentLearning_DB();
        GasTubeCheckPlanAndResult_DB db11 = new GasTubeCheckPlanAndResult_DB();

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
                case "tubecheckplanandresult":
                    #region 用戶管線定期檢查計畫及檢查結果

                    db11._業者guid = cpid;
                    db11._年度 = year;
                    dt = db11.GetList2().Tables[1];

                    hssfworkbook.SetSheetName(0, "用戶管線定期檢查計畫及檢查結果");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("用戶名稱");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("檢查期限 01:符合 02:不符合");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("檢查結果 01:符合 02:不符合 03:拒檢");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["用戶名稱"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["檢查期限是否符合"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["檢查結果是否符合"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_用戶管線定期檢查計畫及檢查結果.xls";

                    #endregion
                    break;
                case "tubecomplete":
                    #region 管線完整性管理作為_幹線及環線管線

                    db2._業者guid = cpid;
                    db2._年度 = year;
                    dt = db2.GetExportList();

                    hssfworkbook.SetSheetName(0, "管線完整性管理作為_幹線及環線管線");
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
                            sheet.GetRow(i + 1).CreateCell(12).SetCellValue(dt.Rows[i]["洩漏偵測系統LLDS"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(13).SetCellValue(dt.Rows[i]["強化作為"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_管線完整性管理作為_幹線及環線管線.xls";

                    #endregion
                    break;
                case "tubecomplete2":
                    #region 管線完整性管理作為_幹線及環線管線以外

                    db2._業者guid = cpid;
                    db2._年度 = year;
                    dt = db2.GetExportList_Out();

                    hssfworkbook.SetSheetName(0, "管線完整性管理作為_幹線及環線管線以外");
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
                            sheet.GetRow(i + 1).CreateCell(12).SetCellValue(dt.Rows[i]["洩漏偵測系統LLDS"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(13).SetCellValue(dt.Rows[i]["強化作為"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_管線完整性管理作為_幹線及環線管線以外.xls";

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
                    sheet.GetRow(0).CreateCell(19).SetCellValue("外部腐蝕保護電位符合標準要求數量");
                    sheet.GetRow(0).CreateCell(20).SetCellValue("備註");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["長途管線識別碼"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["檢測方法"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["最近一次執行年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["報告產出年月"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["檢測長度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["減薄3040數量_內"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["減薄3040數量_內開挖確認"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["減薄3040數量_外"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["減薄3040數量_外開挖確認"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["減薄4050數量_內"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["減薄4050數量_內開挖確認"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(11).SetCellValue(dt.Rows[i]["減薄4050數量_外"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(12).SetCellValue(dt.Rows[i]["減薄4050數量_外開挖確認"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(13).SetCellValue(dt.Rows[i]["減薄50以上數量_內"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(14).SetCellValue(dt.Rows[i]["減薄50以上數量_內開挖確認"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(15).SetCellValue(dt.Rows[i]["減薄50以上數量_外"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(16).SetCellValue(dt.Rows[i]["減薄50以上數量_外開挖確認"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(17).SetCellValue(dt.Rows[i]["Dent_大於12"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(18).SetCellValue(dt.Rows[i]["Dent_開挖確認"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(19).SetCellValue(dt.Rows[i]["外部腐蝕保護電位符合標準要求數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(20).SetCellValue(dt.Rows[i]["備註"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_智慧型通管器檢查(ILI).xls";

                    #endregion
                    break;
                case "CIPS":
                    #region 緊密電位檢測(CIPS)

                    db4._業者guid = cpid;
                    db4._年度 = year;
                    dt = db4.GetList();

                    hssfworkbook.SetSheetName(0, "緊密電位檢測(CIPS)");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("長途管線識別碼");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("同時檢測管線數量");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("最近一次執行 年/月");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("折線圖 年/月");
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
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["合格標準請參照填表說明2"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["立即改善_數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["立即改善_改善完成數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["排程改善_數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["排程改善_改善完成數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["需監控點_數量"].ToString().Trim());
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
                case "tubemaintain":
                    #region 管線維修或開挖

                    db6._業者guid = cpid;
                    db6._年度 = year;
                    dt = db6.GetList();

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
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["前一年度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["長度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["管線位置"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["備註"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_管線維修或開挖.xls";

                    #endregion
                    break;
                case "riskassessment":
                    #region 風險評估

                    db7._業者guid = cpid;
                    db7._年度 = year;
                    dt = db7.GetList();

                    hssfworkbook.SetSheetName(0, "風險評估");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("長途管線識別碼");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("最近一次執行日期 (年/月)");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("再評估時機 1.定期(5年) 2.風險因子異動");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("管線長度 (公里)");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("分段數量");
                    sheet.GetRow(0).CreateCell(5).SetCellValue("已納入ILI結果");
                    sheet.GetRow(0).CreateCell(6).SetCellValue("已納入CIPS結果");
                    sheet.GetRow(0).CreateCell(7).SetCellValue("已納入巡管結果 1.是 2.否");
                    sheet.GetRow(0).CreateCell(8).SetCellValue("各等級風險管段數量 (高)");
                    sheet.GetRow(0).CreateCell(9).SetCellValue("各等級風險管段數量 (中)");
                    sheet.GetRow(0).CreateCell(10).SetCellValue("各等級風險管段數量 (低)");
                    sheet.GetRow(0).CreateCell(11).SetCellValue("降低中高風險管段之相關作為文件名稱");
                    sheet.GetRow(0).CreateCell(12).SetCellValue("改善後風險等級高、中、低");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["長途管線識別碼"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["最近一次執行日期"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["再評估時機"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["管線長度"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["分段數量"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["已納入ILI結果"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["已納入CIPS結果"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["已納入巡管結果"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(8).SetCellValue(dt.Rows[i]["各等級風險管段數量_高"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["各等級風險管段數量_中"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["各等級風險管段數量_低"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(11).SetCellValue(dt.Rows[i]["降低中高風險管段之相關作為文件名稱"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(12).SetCellValue(dt.Rows[i]["改善後風險等級高中低"].ToString().Trim());
                        }
                    }
                    fileName = cpName + "_風險評估.xls";

                    #endregion
                    break;
                case "tubecheck":
                    #region 管線巡檢

                    db8._業者guid = cpid;
                    db8._年度 = year;
                    dt = db8.GetList();

                    hssfworkbook.SetSheetName(0, "管線巡檢");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("每日巡檢次數 01:1次 02:2次 03:3次(含)以上");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("巡管人數");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("巡管工具 01:PDA 02:手機 03:其他");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("巡管工具其他");
                    sheet.GetRow(0).CreateCell(4).SetCellValue("主管監督查核 Y:有 N:無");
                    sheet.GetRow(0).CreateCell(5).SetCellValue("主管監督查核次");
                    sheet.GetRow(0).CreateCell(6).SetCellValue("是否有加強巡檢點 Y:有 N:無");
                    sheet.GetRow(0).CreateCell(7).SetCellValue("是否有加強巡檢點敘述");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["每日巡檢次數"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["巡管人數"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["巡管工具"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["巡管工具其他"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["主管監督查核"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["主管監督查核次"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["是否有加強巡檢點"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(7).SetCellValue(dt.Rows[i]["是否有加強巡檢點敘述"].ToString().Trim());
                        }
                    }

                    dt.Clear();
                    dt = db8.GetList2();

                    hssfworkbook.CreateSheet("依據文件資料");
                    sheet = hssfworkbook.GetSheetAt(1);
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("依據文件名稱");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("文件編號");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("文件日期");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["依據文件名稱"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["文件編號"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["文件日期"].ToString().Trim());
                        }
                    }

                    dt.Clear();
                    dt = db8.GetList3();

                    hssfworkbook.CreateSheet("異常情形統計資料");
                    sheet = hssfworkbook.GetSheetAt(2);
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
                case "control":
                    #region 控制室

                    db9._業者guid = cpid;
                    db9._年度 = year;
                    dt = db9.GetList2();

                    hssfworkbook.SetSheetName(0, "控制室 依據文件資料");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("依據文件名稱");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("文件編號");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("文件日期");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["依據文件名稱"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["文件編號"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["文件日期"].ToString().Trim());
                        }
                    }

                    fileName = cpName + "_控制室依據文件資料.xls";

                    #endregion
                    break;
                case "accidentlearning":
                    #region 事故學習

                    db10._業者guid = cpid;
                    db10._年度 = year;
                    dt = db10.GetList();

                    hssfworkbook.SetSheetName(0, "事故學習");
                    sheet.CreateRow(0);
                    sheet.GetRow(0).CreateCell(0).SetCellValue("事故日期");
                    sheet.GetRow(0).CreateCell(1).SetCellValue("事故名稱");
                    sheet.GetRow(0).CreateCell(2).SetCellValue("事故原因");
                    sheet.GetRow(0).CreateCell(3).SetCellValue("改善作為");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(i + 1);
                            sheet.GetRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["事故日期"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["事故名稱"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["事故原因"].ToString().Trim());
                            sheet.GetRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["改善作為"].ToString().Trim());
                        }
                    }

                    fileName = cpName + "_事故學習.xls";

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