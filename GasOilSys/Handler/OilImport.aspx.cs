﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using NPOI.XSSF.UserModel; // For Excel 2007 and newer (.xlsx) files
using NPOI.HSSF.UserModel; // For Excel 2003 and older (.xls) files
using NPOI.SS.UserModel;//-- v.1.2.4起 新增的。

public partial class Handler_OilImport : System.Web.UI.Page
{
    CodeTable_DB cdb = new CodeTable_DB();
    OilTubeCheck_DB odb = new OilTubeCheck_DB();
    OilStorageTankInfo_DB db1 = new OilStorageTankInfo_DB();
    OilStorageTankBWT_DB db2 = new OilStorageTankBWT_DB();
    OilStorageTankInfoLiquefaction_DB db3 = new OilStorageTankInfoLiquefaction_DB();
    OilStorageTankButton_DB db4 = new OilStorageTankButton_DB();
    OilButtonChange_DB db5 = new OilButtonChange_DB();
    OilCathodicProtection_DB db6 = new OilCathodicProtection_DB();
    OilTankPipeline_DB db7 = new OilTankPipeline_DB();
    OilTubeInfo_DB db9 = new OilTubeInfo_DB();
    OilTubeEnvironment_DB db10 = new OilTubeEnvironment_DB();
    public string filePath = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 石油 匯入
        ///說    明:
        /// * Request["cpid"]: 業者guid
        /// * Request["year"]: 年度
        /// * Request["category"]: 類別 
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();

        /// Transaction
        SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        oConn.Open();
        SqlCommand oCmmd = new SqlCommand();
        oCmmd.Connection = oConn;
        SqlTransaction myTrans = oConn.BeginTransaction();
        oCmmd.Transaction = myTrans;
        try
        {
            #region 檢查登入資訊
            if (LogInfo.mGuid == "")
            {
                throw new Exception("請重新登入");
            }
            #endregion

            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Request["category"].ToString().Trim();
            string xmlstr = string.Empty;

            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Oil_Upload\\ImportTemp\\";
                    string fileName = Path.GetFileName(File.FileName);

                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }
                    filePath = UpLoadPath + fileName;
                    //將上傳的匯入檔案暫存到暫存用的資料夾
                    File.SaveAs(filePath);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook;
                        if (Path.GetExtension(filePath) == ".xlsx")
                            workbook = new XSSFWorkbook(fileStream); // For Excel 2007 and newer (.xlsx) files
                        else if (Path.GetExtension(filePath) == ".xls")
                            workbook = new HSSFWorkbook(fileStream); // For Excel 2003 and older (.xls) files
                        else
                            throw new Exception("匯入格式不正確，請修改後再重新上傳！");

                        ISheet sheet = workbook.GetSheetAt(0); // Assuming you want to read the first sheet

                        if(!string.IsNullOrEmpty(checkValid(sheet, category, cpid, year)))
                        {
                            throw new Exception("匯入格式不正確，請修改以下問題後再重新上傳:\r\n" + checkValid(sheet, category, cpid, year));
                        }
                        else
                        {
                            AddImportData(sheet, category, oConn, myTrans, cpid, year);
                        }
                    }
                }
            }

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>匯入完成</Response><relogin>N</relogin></root>";

            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            myTrans.Rollback();
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        finally
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                fi.Delete(); //刪除暫存的匯入檔案
            }
            oCmmd.Connection.Close();
            oConn.Close();
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }

    #region 確認匯入資料格式

    public string checkValid(ISheet sheet, string category, string cpid, string year)
    {
        string msg = string.Empty;

        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row != null)
            {
                switch (category)
                {
                    #region 儲槽基本資料_常壓地上式儲槽
                    case "storagetankinfo":                        

                        if (sheet.GetRow(i).GetCell(0) != null)
                        {
                            if (sheetRow(sheet, i, 0).Length > 50)
                            {
                                msg += "【轄區儲槽編號】字數不可大於50\r\n";
                            }
                            else
                            {
                                db1._轄區儲槽編號 = sheetRow(sheet, i, 0);
                                db1._業者guid = cpid;
                                DataTable dt = db1.GetList();

                                if (dt.Rows.Count > 0)
                                {
                                    msg += "已有此轄區儲槽編號【" + sheetRow(sheet, i, 0) + "】，請修正後再重新匯入\r\n";
                                }
                            }
                        }

                        if (sheetRow(sheet, i, 1) != null)
                            if (sheetRow(sheet, i, 1).Length > 20)
                                msg += "【能源署編號】字數不可大於20\r\n";
                        if (sheetRow(sheet, i, 2) != null)
                            if (sheetRow(sheet, i, 2).Length > 8)
                                msg += "【設計容量(公秉)】字數不可大於8\r\n";
                        if (sheetRow(sheet, i, 3) != null)
                            if (sheetRow(sheet, i, 3).Length > 8)
                                msg += "【儲槽內徑(公尺)】字數不可大於8\r\n";
                        if (sheetRow(sheet, i, 4) != null)
                            if (sheetRow(sheet, i, 4).Length > 50)
                                msg += "【內容物(中文)】字數不可大於50\r\n";
                        if (sheetRow(sheet, i, 5) != null)
                            if (sheetRow(sheet, i, 5).Length > 50)
                                msg += "【油品種類】字數不可大於50\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("023", sheetRow(sheet, i, 5)) == false)
                                    msg += "【油品種類】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 6) != null)
                            if (sheetRow(sheet, i, 6).Length > 50)
                                msg += "【形式】字數不可大於50\r\n"; 
                            else
                                if (cdb.GetDataOnlyChineseIfExist("014", sheetRow(sheet, i, 6)) == false)
                                    msg += "【形式】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 7) != null)
                        {
                            if (!ContainsOnlyAlphanumeric(sheetRow(sheet, i, 7)))
                            {
                                msg += "【啟用日期】字串內不可包含特殊符號\r\n";
                            }
                            if (sheetRow(sheet, i, 7).Length > 10)
                            {
                                msg += "【啟用日期】字數不可大於10\r\n";
                            }
                            else
                            {
                                if ((sheetRow(sheet, i, 7).Length < 4) || (sheetRow(sheet, i, 7).Length > 5))
                                {
                                    msg += "【啟用日期】字數需4或5個字\r\n";
                                }
                            }        
                        }
                            
                        if (sheetRow(sheet, i, 8) != null)
                            if (sheetRow(sheet, i, 8).Length > 50)
                                msg += "【代行檢查有效期限 代檢機構(填表說明)】字數不可大於50\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("016", sheetRow(sheet, i, 8)) == false)
                                    msg += "【代行檢查有效期限 代檢機構(填表說明)】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 9) != null)
                        {
                            if (!ContainsOnlyAlphanumeric(sheetRow(sheet, i, 9)))
                            {
                                msg += "【代行檢查有效期限 外部 年/月/日】字串內不可包含特殊符號\r\n";
                            }
                            if (sheetRow(sheet, i, 9).Length > 10)
                            {
                                msg += "【代行檢查有效期限 外部 年/月/日】字數不可大於10\r\n";
                            }
                            else
                            {
                                if ((sheetRow(sheet, i, 9).Length < 6) || (sheetRow(sheet, i, 9).Length > 7))
                                {
                                    msg += "【代行檢查有效期限 外部 年/月/日】字數需6或7個字\r\n";
                                }                                    
                            }
                                
                        }                            
                        if (sheetRow(sheet, i, 10) != null)
                            if (sheetRow(sheet, i, 10).Length > 50)
                                msg += "【代行檢查有效期限 代檢機構(填表說明)】字數不可大於50\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("016", sheetRow(sheet, i, 10)) == false)
                                    msg += "【代行檢查有效期限 代檢機構(填表說明)】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 11) != null)
                        {
                            if (!ContainsOnlyAlphanumeric(sheetRow(sheet, i, 11)))
                            {
                                msg += "【代行檢查有效期限 內部 年/月/日】字串內不可包含特殊符號\r\n";
                            }
                            if (sheetRow(sheet, i, 11).Length > 10)
                            {
                                msg += "【代行檢查有效期限 內部 年/月/日】字數不可大於10\r\n";
                            }
                            else
                            {
                                if ((sheetRow(sheet, i, 11).Length < 6) || (sheetRow(sheet, i, 11).Length > 7))
                                {
                                    msg += "【代行檢查有效期限 內部 年/月/日】字數需6或7個字\r\n";
                                }                                    
                            }                                
                        }   
                        if (sheetRow(sheet, i, 12) != null)
                            if (sheetRow(sheet, i, 12).Length > 20)
                                msg += "【狀態】字數不可大於20\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("015", sheetRow(sheet, i, 12)) == false)
                                    msg += "【狀態】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 13) != null)
                            if (sheetRow(sheet, i, 13).Length > 10)
                                msg += "【延長開放年限多?年】字數不可大於10\r\n";
                                                
                        break;
                    #endregion

                    #region 儲槽基本資料_液化石油氣儲槽

                    case "storagetankinfoliquefaction":                        

                        if (sheet.GetRow(i).GetCell(0) != null)
                        {
                            if (sheetRow(sheet, i, 0).Length > 50)
                            {
                                msg += "【轄區儲槽編號】字數不可大於50\r\n";
                            }
                            else
                            {
                                db3._轄區儲槽編號 = sheetRow(sheet, i, 0);
                                db3._業者guid = cpid;
                                DataTable dt = db3.GetList();

                                if (dt.Rows.Count > 0)
                                {
                                    msg += "已有此轄區儲槽編號【" + sheetRow(sheet, i, 0) + "】，請修正後再重新匯入\r\n";
                                }
                            }
                        }

                        if (sheetRow(sheet, i, 1) != null)
                            if (sheetRow(sheet, i, 1).Length > 20)
                                msg += "【能源署編號】字數不可大於20\r\n";
                        if (sheetRow(sheet, i, 2) != null)
                            if (sheetRow(sheet, i, 2).Length > 8)
                                msg += "【設計容量(公秉)】字數不可大於8\r\n";
                        if (sheetRow(sheet, i, 3) != null)
                            if (sheetRow(sheet, i, 3).Length > 8)
                                msg += "【儲槽內徑(公尺)】字數不可大於8\r\n";
                        if (sheetRow(sheet, i, 4) != null)
                            if (sheetRow(sheet, i, 4).Length > 50)
                                msg += "【內容物】字數不可大於50\r\n";
                        if (sheetRow(sheet, i, 5) != null)
                            if (sheetRow(sheet, i, 5).Length > 50)
                                msg += "【油品種類】字數不可大於50\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("023", sheetRow(sheet, i, 5)) == false)
                                    msg += "【油品種類】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 6) != null)
                            if (sheetRow(sheet, i, 6).Length > 50)
                                msg += "【形式】字數不可大於50\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("014", sheetRow(sheet, i, 6)) == false)
                                    msg += "【形式】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 7) != null)
                        {
                            if (!ContainsOnlyAlphanumeric(sheetRow(sheet, i, 7)))
                            {
                                msg += "【啟用日期】字串內不可包含特殊符號\r\n";
                            }
                            if (sheetRow(sheet, i, 7).Length > 10)
                            {
                                msg += "【啟用日期】字數不可大於10\r\n";
                            }
                            else
                            {
                                if ((sheetRow(sheet, i, 7).Length < 4) || (sheetRow(sheet, i, 7).Length > 5))
                                {
                                    msg += "【啟用日期】字數需4或5個字\r\n";
                                }
                            }
                        }

                        if (sheetRow(sheet, i, 8) != null)
                            if (sheetRow(sheet, i, 8).Length > 50)
                                msg += "【狀態】字數不可大於50\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("015", sheetRow(sheet, i, 8)) == false)
                                    msg += "【狀態】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";

                        break;
                    #endregion

                    #region 儲槽基礎、壁板、頂板

                    case "storagetankBWT":                        

                        if (sheetRow(sheet, i, 0) != null)
                        {
                            if (sheetRow(sheet, i, 0).Length > 50)
                            {
                                msg += "【轄區儲槽編號】字數不可大於50\r\n";
                            }                            
                            else
                            {
                                db1._轄區儲槽編號 = sheetRow(sheet, i, 0);
                                db1._業者guid = cpid;
                                DataTable dt = db1.GetList();

                                if (dt.Rows.Count < 1)
                                {
                                    msg += "儲槽基本資料內並沒有此編號【" + sheetRow(sheet, i, 0) + "】，請至儲槽基本資料頁面新增後再重新匯入\r\n";
                                }
                            }
                        }                            
                                
                        if (sheetRow(sheet, i, 1) != null)
                            if (sheetRow(sheet, i, 1).Length > 10)
                                msg += "【基礎與底板間是否具防水包覆層設計】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 1)) == false)
                                    msg += "【基礎與底板間是否具防水包覆層設計】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 2) != null)
                            if (sheetRow(sheet, i, 2).Length > 10)
                                msg += "【沈陷量測點數】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 3) != null)
                        {
                            if (!ContainsOnlyAlphanumeric(sheetRow(sheet, i, 3)))
                            {
                                msg += "【沈陷量測日期】字串內不可包含特殊符號\r\n";
                            }
                            if (sheetRow(sheet, i, 3).Length > 10)
                            {
                                msg += "【沈陷量測日期】字數不可大於10\r\n";
                            }                                
                            else
                            {
                                if ((sheetRow(sheet, i, 3).Length < 6) || (sheetRow(sheet, i, 3).Length > 7))
                                {
                                    msg += "【沈陷量測日期】字數需6或7個字\r\n";
                                }                                
                            }
                        }     
                        if (sheetRow(sheet, i, 4) != null)
                            if (sheetRow(sheet, i, 4).Length > 10)
                                msg += "【接地電阻<10Ω】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 4)) == false)
                                    msg += "【接地電阻<10Ω】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 5) != null)
                            if (sheetRow(sheet, i, 5).Length > 10)
                                msg += "【壁板是否具包附層】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 5)) == false)
                                    msg += "【壁板是否具包附層】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 6) != null)
                            if (sheetRow(sheet, i, 6).Length > 10)
                                msg += "【壁板外部嚴重腐蝕或點蝕】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 6)) == false)
                                    msg += "【壁板外部嚴重腐蝕或點蝕】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 7) != null)
                            if (sheetRow(sheet, i, 7).Length > 10)
                                msg += "【第一層壁板內部下方腐蝕】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 7)) == false)
                                    msg += "【第一層壁板內部下方腐蝕】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 8) != null)
                            if (sheetRow(sheet, i, 8).Length > 10)
                                msg += "【維修方式是否有符合API653】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 8)) == false)
                                    msg += "【維修方式是否有符合API653】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 9) != null)
                            if (sheetRow(sheet, i, 9).Length > 10)
                                msg += "【外浮頂之等電位裝置性能】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("033", sheetRow(sheet, i, 9)) == false)
                                    msg += "【外浮頂之等電位裝置性能】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";                        
                        break;
                    #endregion

                    #region 儲槽底板

                    case "storagetankbutton":

                        if (sheetRow(sheet, i, 0) != null)
                        {
                            if (sheetRow(sheet, i, 0).Length > 50)
                            {
                                msg += "【轄區儲槽編號】字數不可大於50\r\n";
                            }
                            else
                            {
                                db1._轄區儲槽編號 = sheetRow(sheet, i, 0);
                                db1._業者guid = cpid;
                                DataTable dt = db1.GetList();

                                if (dt.Rows.Count < 1)
                                {
                                    msg += "儲槽基本資料內並沒有此編號【" + sheetRow(sheet, i, 0) + "】，請至儲槽基本資料頁面新增後再重新匯入\r\n";
                                }
                            }
                        }

                        if (sheetRow(sheet, i, 1) != null)
                            if (sheetRow(sheet, i, 1).Length > 20)
                                msg += "【執行土壤側檢測 1.全部 2.部分 3.無】字數不可大於20\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("035", sheetRow(sheet, i, 1)) == false)
                                    msg += "【執行土壤側檢測 1.全部 2.部分 3.無】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 2) != null)
                            if (sheetRow(sheet, i, 2).Length > 20)
                                msg += "【防蝕塗層 1.無 2.FRP 3.EPOXY 4.其他】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("036", sheetRow(sheet, i, 2)) == false)
                                    msg += "【防蝕塗層 1.無 2.FRP 3.EPOXY 4.其他】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 3) != null)
                        {
                            if (sheetRow(sheet, i, 3).Length > 10)
                            {
                                msg += "【塗層全面重新施加日期 年/月】字數不可大於10\r\n";
                            }
                            else
                            {
                                if ((sheetRow(sheet, i, 3).Length < 5) || (sheetRow(sheet, i, 3).Length > 6))
                                {
                                    msg += "【塗層全面重新施加日期 年/月】字數需5或6個字\r\n";
                                }
                            }
                        }
                        if (sheetRow(sheet, i, 4) != null)
                            if (sheetRow(sheet, i, 4).Length > 10)
                                msg += "【最近一次開放塗層維修情形 1.全部 2.部分 3.無】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("035", sheetRow(sheet, i, 4)) == false)
                                    msg += "【最近一次開放塗層維修情形 1.全部 2.部分 3.無】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 5) != null)
                            if (sheetRow(sheet, i, 5).Length > 10)
                                msg += "【銲道腐蝕 1.有 2.無】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 5)) == false)
                                msg += "【銲道腐蝕 1.有 2.無】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 6) != null)
                            if (sheetRow(sheet, i, 6).Length > 10)
                                msg += "【局部變形 1.有 2.無】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 6)) == false)
                                msg += "【局部變形 1.有 2.無】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 7) != null)
                            if (sheetRow(sheet, i, 7).Length > 10)
                                msg += "【最近一次開放是否有維修 1.有 2.無】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 7)) == false)
                                    msg += "【最近一次開放是否有維修 1.有 2.無】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 8) != null)
                            if (sheetRow(sheet, i, 8).Length > 10)
                                msg += "【內容物側最小剩餘厚度(mm)】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 9) != null)
                            if (sheetRow(sheet, i, 9).Length > 20)
                                msg += "【內容物側最大腐蝕速率(mm/yr)】字數不可大於20\r\n";
                        if (sheetRow(sheet, i, 10) != null)
                            if (sheetRow(sheet, i, 10).Length > 20)
                                msg += "【土壤側最小剩餘厚度(mm)】字數不可大於20\r\n";
                        if (sheetRow(sheet, i, 11) != null)
                            if (sheetRow(sheet, i, 11).Length > 20)
                                msg += "【土壤側最大腐蝕速率(mm/yr)】字數不可大於20\r\n";
                        if (sheetRow(sheet, i, 12) != null)
                            if (sheetRow(sheet, i, 12).Length > 10)
                                msg += "【是否有更換過底板 1.有 2.無】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 12)) == false)
                                msg += "【是否有更換過底板 1.有 2.無】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 13) != null)
                            if (sheetRow(sheet, i, 13).Length > 10)
                                msg += "【綜合判定 1.良好 2.須持續追蹤】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("037", sheetRow(sheet, i, 13)) == false)
                                msg += "【綜合判定 1.良好 2.須持續追蹤】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        break;

                    #endregion

                    #region 底板更換紀錄

                    case "buttonchange":

                        if (sheetRow(sheet, i, 0) != null)
                        {
                            if (sheetRow(sheet, i, 0).Length > 50)
                            {
                                msg += "【轄區儲槽編號】字數不可大於50\r\n";
                            }
                            else
                            {
                                db1._轄區儲槽編號 = sheetRow(sheet, i, 0);
                                db1._業者guid = cpid;
                                DataTable dt = db1.GetList();

                                if (dt.Rows.Count < 1)
                                {
                                    msg += "儲槽基本資料內並沒有此編號【" + sheetRow(sheet, i, 0) + "】，請至儲槽基本資料頁面新增後再重新匯入\r\n";
                                }
                            }
                        }
                        if (sheetRow(sheet, i, 1) != null)
                        {
                            if (!ContainsOnlyAlphanumeric(sheetRow(sheet, i, 1)))
                            {
                                msg += "【更換日期(YYYMMDD)】字串內不可包含特殊符號\r\n";
                            }
                            if (sheetRow(sheet, i, 1).Length > 10)
                            {
                                msg += "【更換日期(YYYMMDD)】字數不可大於10\r\n";
                            }
                            else
                            {
                                if ((sheetRow(sheet, i, 1).Length < 6) || (sheetRow(sheet, i, 1).Length > 7))
                                {
                                    msg += "【更換日期(YYYMMDD)】字數需6或7個字\r\n";
                                }
                            }
                        }
                        if (sheetRow(sheet, i, 2) != null)
                            if (sheetRow(sheet, i, 2).Length > 10)
                                msg += "【更換面積(M2)】字數不可大於10\r\n";
                        break;

                    #endregion

                    #region 陰極防蝕系統

                    case "cathodicprotection":

                        if (sheetRow(sheet, i, 0) != null)
                        {
                            if (sheetRow(sheet, i, 0).Length > 50)
                            {
                                msg += "【轄區儲槽編號】字數不可大於50\r\n";
                            }
                            else
                            {
                                db1._轄區儲槽編號 = sheetRow(sheet, i, 0);
                                db1._業者guid = cpid;
                                DataTable dt = db1.GetList();

                                if (dt.Rows.Count < 1)
                                {
                                    msg += "儲槽基本資料內並沒有此編號【" + sheetRow(sheet, i, 0) + "】，請至儲槽基本資料頁面新增後再重新匯入\r\n";
                                }
                            }
                        }
                        if (sheetRow(sheet, i, 1) != null)
                            if (sheetRow(sheet, i, 1).Length > 2)
                                msg += "【設置 1.有 2.無】字數不可大於2\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 1)) == false)
                                msg += "【設置 1.有 2.無】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 2) != null)
                            if (sheetRow(sheet, i, 2).Length > 50)
                                msg += "【整流站名稱】字數不可大於50\r\n";
                        if (sheetRow(sheet, i, 3) != null)
                            if (sheetRow(sheet, i, 3).Length > 50)
                                msg += "【合格標準 1. 通電電位< -850mVCSE 2.極化電位< -850mVCSE 3.極化量>100mV 4.其他】字數不可大於50\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("038", sheetRow(sheet, i, 3)) == false)
                                msg += "【合格標準 1. 通電電位< -850mVCSE 2.極化電位< -850mVCSE 3.極化量>100mV 4.其他】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 4) != null)
                            if (sheetRow(sheet, i, 4).Length > 10)
                                msg += "【整流站狀態 1.正常 2.異常】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("039", sheetRow(sheet, i, 4)) == false)
                                msg += "【整流站狀態 1.正常 2.異常】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 5) != null)
                            if (sheetRow(sheet, i, 5).Length > 10)
                                msg += "【系統狀態 1.正常 2.異常】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("039", sheetRow(sheet, i, 5)) == false)
                                msg += "【系統狀態 1.正常 2.異常】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 6) != null)
                            if (sheetRow(sheet, i, 6).Length > 50)
                                msg += "【設置長效型參考電極種類 1.鋅 2.飽和硫酸銅 3.無】字數不可大於50\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("040", sheetRow(sheet, i, 6)) == false)
                                msg += "【設置長效型參考電極種類 1.鋅 2.飽和硫酸銅 3.無】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 7) != null)
                            if (sheetRow(sheet, i, 7).Length > 10)
                                msg += "【測試點數量】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 8) != null)
                            if (sheetRow(sheet, i, 8).Length > 10)
                                msg += "【陽極地床種類 1.深井 2.淺井】字數不可大於10\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("041", sheetRow(sheet, i, 8)) == false)
                                msg += "【陽極地床種類 1.深井 2.淺井】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        break;

                    #endregion

                    #region 槽區管線

                    case "tankpipeline":

                        if (sheetRow(sheet, i, 0) != null)
                        {
                            if (sheetRow(sheet, i, 0).Length > 50)
                            {
                                msg += "【轄區儲槽編號】字數不可大於50\r\n";
                            }
                            else
                            {
                                db1._轄區儲槽編號 = sheetRow(sheet, i, 0);
                                db1._業者guid = cpid;
                                DataTable dt = db1.GetList();

                                if (dt.Rows.Count < 1)
                                {
                                    msg += "儲槽基本資料內並沒有此編號【" + sheetRow(sheet, i, 0) + "】，請至儲槽基本資料頁面新增後再重新匯入\r\n";
                                }
                            }
                        }
                        if (sheetRow(sheet, i, 1) != null)
                            if (sheetRow(sheet, i, 1).Length > 2)
                                msg += "【具保溫層管線 1.有 2.無】字數不可大於2\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 1)) == false)
                                msg += "【具保溫層管線 1.有 2.無】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 2) != null)
                            if (sheetRow(sheet, i, 2).Length > 2)
                                msg += "【管線支撐座腐蝕疑慮 1.是 2.否】字數不可大於2\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("042", sheetRow(sheet, i, 2)) == false)
                                msg += "【管線支撐座腐蝕疑慮 1.是 2.否】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        break;

                    #endregion

                    #region 管線基本資料

                    case "tubeinfo":

                        if (sheetRow(sheet, i, 0) != null)
                            if (sheetRow(sheet, i, 0).Length > 50)
                                msg += "【長途管線識別碼】字數不可大於50\r\n";
                        if (sheetRow(sheet, i, 1) != null)
                            if (sheetRow(sheet, i, 1).Length > 250)
                                msg += "【轄區長途管線名稱(公司)】字數不可大於250\r\n";
                        if (sheetRow(sheet, i, 2) != null)
                            if (sheetRow(sheet, i, 2).Length > 150)
                                msg += "【銜接管線識別碼(上游)】字數不可大於150\r\n";
                        if (sheetRow(sheet, i, 3) != null)
                            if (sheetRow(sheet, i, 3).Length > 150)
                                msg += "【銜接管線識別碼(下游)】字數不可大於150\r\n";
                        if (sheetRow(sheet, i, 4) != null)
                            if (sheetRow(sheet, i, 4).Length > 150)
                                msg += "【起點】字數不可大於150\r\n";
                        if (sheetRow(sheet, i, 5) != null)
                            if (sheetRow(sheet, i, 5).Length > 150)
                                msg += "【迄點】字數不可大於150\r\n";
                        if (sheetRow(sheet, i, 6) != null)
                            if (sheetRow(sheet, i, 6).Length > 10)
                                msg += "【管徑(吋)】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 7) != null)
                            if (sheetRow(sheet, i, 7).Length > 10)
                                msg += "【厚度(mm)】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 8) != null)
                            if (sheetRow(sheet, i, 8).Length > 15)
                                msg += "【管材(詳細規格)】字數不可大於15\r\n";
                        if (sheetRow(sheet, i, 9) != null)
                            if (sheetRow(sheet, i, 9).Length > 10)
                                msg += "【包覆材料】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 10) != null)
                            if (sheetRow(sheet, i, 10).Length > 10)
                                msg += "【轄管長度(公里)】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 11) != null)
                            if (sheetRow(sheet, i, 11).Length > 10)
                                msg += "【內容物】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 12) != null)
                            if (sheetRow(sheet, i, 12).Length > 20)
                                msg += "【八大油品】字數不可大於20\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("030", sheetRow(sheet, i, 12)) == false)
                                msg += "【八大油品】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 13) != null)
                            if (sheetRow(sheet, i, 13).Length > 10)
                                msg += "【緊急遮斷閥(處)】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 14) != null)
                            if (sheetRow(sheet, i, 14).Length > 3)
                                msg += "【建置年】字數不可大於3\r\n";
                        if (sheetRow(sheet, i, 15) != null)
                            if (sheetRow(sheet, i, 15).Length > 10)
                                msg += "【設計壓力(Kg/cm2)】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 16) != null)
                            if (sheetRow(sheet, i, 16).Length > 10)
                                msg += "【使用壓力(Kg/cm2)】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 17) != null)
                            if (sheetRow(sheet, i, 17).Length > 20)
                                msg += "【使用狀態】字數不可大於20\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("043", sheetRow(sheet, i, 17)) == false)
                                msg += "【使用狀態】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 18) != null)
                            if (sheetRow(sheet, i, 18).Length > 10)
                                msg += "【附掛橋樑數量】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 19) != null)
                            if (sheetRow(sheet, i, 19).Length > 10)
                                msg += "【管線穿越箱涵數量】字數不可大於10\r\n";
                        if (sheetRow(sheet, i, 20) != null)
                            if (sheetRow(sheet, i, 20).Length > 2)
                                msg += "【活動斷層敏感區】字數不可大於2\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 20)) == false)
                                msg += "【活動斷層敏感區】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 21) != null)
                            if (sheetRow(sheet, i, 21).Length > 2)
                                msg += "【土壤液化區】字數不可大於2\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 21)) == false)
                                msg += "【土壤液化區】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 22) != null)
                            if (sheetRow(sheet, i, 22).Length > 2)
                                msg += "【土石流潛勢區】字數不可大於2\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 22)) == false)
                                msg += "【土石流潛勢區】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        if (sheetRow(sheet, i, 23) != null)
                            if (sheetRow(sheet, i, 23).Length > 2)
                                msg += "【淹水潛勢區】字數不可大於2\r\n";
                            else
                                if (cdb.GetDataOnlyChineseIfExist("032", sheetRow(sheet, i, 23)) == false)
                                msg += "【淹水潛勢區】有數字或標點符號或是文字格式不正確，請參照excel範例欄位\r\n";
                        break;

                        #endregion
                }
            }

            if (!string.IsNullOrEmpty(msg))
            {
                break;
            }
        }

        return msg;
    }

    #endregion

    #region 匯入資料

    public void AddImportData(ISheet sheet, string category, SqlConnection oConn, SqlTransaction oTran, string cpid, string year)
    {
        DataTable dt = new DataTable();

        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row != null)
            {
                switch (category)
                {
                    #region 儲槽基本資料_常壓地上式儲槽

                    case "storagetankinfo":                       

                        db1._業者guid = cpid;
                        db1._年度 = year;
                        db1._轄區儲槽編號 = sheetRow(sheet, i, 0);
                        db1._能源局編號 = sheetRow(sheet, i, 1);
                        db1._容量 = sheetRow(sheet, i, 2);
                        db1._內徑 = sheetRow(sheet, i, 3);
                        db1._內容物 = sheetRow(sheet, i, 4);
                        db1._油品種類 = sheetRow(sheet, i, 5);
                        db1._形式 = sheetRow(sheet, i, 6);
                        db1._啟用日期 = OnlyMonthDay(sheetRow(sheet, i, 7));
                        db1._代行檢查_代檢機構1 = sheetRow(sheet, i, 8);
                        db1._代行檢查_外部日期1 = sheetRow(sheet, i, 9);
                        db1._代行檢查_代檢機構2 = sheetRow(sheet, i, 10);
                        db1._代行檢查_外部日期2 = sheetRow(sheet, i, 11);
                        db1._狀態 = sheetRow(sheet, i, 12);
                        db1._延長開放年限 = sheetRow(sheet, i, 13);
                        db1._差異說明 = sheetRow(sheet, i, 14);
                        db1._建立者 = LogInfo.mGuid;
                        db1._修改者 = LogInfo.mGuid;
                        db1._資料狀態 = "A";

                        dt = db1.GetDataBySPNO(oConn, oTran);

                        if (dt.Rows.Count > 0)
                        {
                            db1._guid = dt.Rows[0]["guid"].ToString().Trim();
                            db1.UpdateData(oConn, oTran);
                        }
                        else
                        {
                            db1.InsertData(oConn, oTran);
                        }

                        dt.Clear();

                        break;

                    #endregion

                    #region 儲槽基本資料_液化石油氣儲槽

                    case "storagetankinfoliquefaction":                       

                        db3._業者guid = cpid;
                        db3._年度 = year;
                        db3._轄區儲槽編號 = sheetRow(sheet, i, 0);
                        db3._能源局編號 = sheetRow(sheet, i, 1);
                        db3._容量 = sheetRow(sheet, i, 2);
                        db3._內徑 = sheetRow(sheet, i, 3);
                        db3._內容物 = sheetRow(sheet, i, 4);
                        db3._油品種類 = sheetRow(sheet, i, 5);
                        db3._形式 = sheetRow(sheet, i, 6);
                        db3._啟用日期 = OnlyMonthDay(sheetRow(sheet, i, 7));
                        db3._狀態 = sheetRow(sheet, i, 8);
                        db3._差異說明 = sheetRow(sheet, i, 9);
                        db3._建立者 = LogInfo.mGuid;
                        db3._修改者 = LogInfo.mGuid;
                        db3._資料狀態 = "A";

                        dt = db3.GetDataBySPNO(oConn, oTran);

                        if (dt.Rows.Count > 0)
                        {
                            db3._guid = dt.Rows[0]["guid"].ToString().Trim();
                            db3.UpdateData(oConn, oTran);
                        }
                        else
                        {
                            db3.InsertData(oConn, oTran);
                        }

                        dt.Clear();

                        break;

                    #endregion

                    #region 儲槽基礎、壁板、頂板

                    case "storagetankBWT":                        

                        db2._業者guid = cpid;
                        db2._年度 = year;
                        db2._轄區儲槽編號 = (sheetRow(sheet, i, 0) == null) ? "" : sheetRow(sheet, i, 0);
                        db2._防水包覆層設計 = (sheetRow(sheet, i, 1) == null) ? "" : sheetRow(sheet, i, 1);
                        db2._沈陷量測點數 = (sheetRow(sheet, i, 2) == null) ? "" : sheetRow(sheet, i, 2);
                        db2._沈陷量測日期 = (sheetRow(sheet, i, 3) == null) ? "" : sheetRow(sheet, i, 3);
                        db2._儲槽接地電阻 = (sheetRow(sheet, i, 4) == null) ? "" : sheetRow(sheet, i, 4);
                        db2._壁板是否具包覆層 = (sheetRow(sheet, i, 5) == null) ? "" : sheetRow(sheet, i, 5);
                        db2._壁板外部嚴重腐蝕或點蝕 = (sheetRow(sheet, i, 6) == null) ? "" : sheetRow(sheet, i, 6);
                        db2._第一層壁板內部下方腐蝕 = (sheetRow(sheet, i, 7) == null) ? "" : sheetRow(sheet, i, 7);
                        db2._壁板維修方式是否有符合API653 = (sheetRow(sheet, i, 8) == null) ? "" : sheetRow(sheet, i, 8);
                        db2._設置等導電良好度 = (sheetRow(sheet, i, 9) == null) ? "" : sheetRow(sheet, i, 9);
                        db2._建立者 = LogInfo.mGuid;
                        db2._修改者 = LogInfo.mGuid;
                        db2._資料狀態 = "A";

                        dt = db2.GetDataBySPNO(oConn, oTran);

                        if (dt.Rows.Count > 0)
                        {
                            db2._guid = dt.Rows[0]["guid"].ToString().Trim();
                            db2.UpdateData(oConn, oTran);
                        }
                        else
                        {
                            db2.InsertData(oConn, oTran);
                        }

                        dt.Clear();

                        break;

                    #endregion

                    #region 儲槽底板

                    case "storagetankbutton":                        

                        db4._業者guid = cpid;
                        db4._年度 = year;
                        db4._轄區儲槽編號 = sheetRow(sheet, i, 0);
                        db4._執行MFL檢測 = sheetRow(sheet, i, 1);
                        db4._防蝕塗層 = sheetRow(sheet, i, 2);
                        db4._塗層全面重新施加日期 = sheetRow(sheet, i, 3);
                        db4._最近一次開放塗層維修情形 = sheetRow(sheet, i, 4);
                        db4._銲道腐蝕 = sheetRow(sheet, i, 5);
                        db4._局部變形 = sheetRow(sheet, i, 6);
                        db4._最近一次開放是否有維修 = sheetRow(sheet, i, 7);
                        db4._內容物側最小剩餘厚度 = sheetRow(sheet, i, 8);
                        db4._內容物側最大腐蝕速率 = sheetRow(sheet, i, 9);
                        db4._土壤側最小剩餘厚度 = sheetRow(sheet, i, 10);
                        db4._土壤側最大腐蝕速率 = sheetRow(sheet, i, 11);
                        db4._是否有更換過底板 = sheetRow(sheet, i, 12);
                        db4._綜合判定 = sheetRow(sheet, i, 13);
                        db4._建立者 = LogInfo.mGuid;
                        db4._修改者 = LogInfo.mGuid;
                        db4._資料狀態 = "A";

                        dt = db4.GetDataBySPNO(oConn, oTran);

                        if (dt.Rows.Count > 0)
                        {
                            db4._guid = dt.Rows[0]["guid"].ToString().Trim();
                            db4.UpdateData(oConn, oTran);
                        }
                        else
                        {
                            db4.InsertData(oConn, oTran);
                        }

                        dt.Clear();
                        
                        break;
                    #endregion

                    #region 底板更換紀錄

                    case "buttonchange":

                        db5._業者guid = cpid;
                        db5._年度 = year;
                        db5._轄區儲槽編號 = sheetRow(sheet, i, 0);
                        db5._更換日期 = sheetRow(sheet, i, 1);
                        db5._更換面積 = sheetRow(sheet, i, 2);
                        db5._更換原因 = sheetRow(sheet, i, 3);
                        db5._建立者 = LogInfo.mGuid;
                        db5._修改者 = LogInfo.mGuid;
                        db5._資料狀態 = "A";

                        dt = db5.GetDataBySPNO(oConn, oTran);

                        if (dt.Rows.Count > 0)
                        {
                            db5._guid = dt.Rows[0]["guid"].ToString().Trim();
                            db5.UpdateData(oConn, oTran);
                        }
                        else
                        {
                            db5.InsertData(oConn, oTran);
                        }

                        dt.Clear();

                        break;
                    #endregion

                    #region 陰極防蝕系統

                    case "cathodicprotection":

                        db6._業者guid = cpid;
                        db6._年度 = year;
                        db6._轄區儲槽編號 = sheetRow(sheet, i, 0);
                        db6._設置 = sheetRow(sheet, i, 1);
                        db6._整流站名稱 = sheetRow(sheet, i, 2);
                        db6._合格標準 = sheetRow(sheet, i, 3);
                        db6._整流站狀態 = sheetRow(sheet, i, 4);
                        db6._系統狀態 = sheetRow(sheet, i, 5);
                        db6._設置長效型參考電極種類 = sheetRow(sheet, i, 6);
                        db6._測試點數量 = sheetRow(sheet, i, 7);
                        db6._陽極地床種類 = sheetRow(sheet, i, 8);
                        db6._備註 = sheetRow(sheet, i, 9);
                        db6._建立者 = LogInfo.mGuid;
                        db6._修改者 = LogInfo.mGuid;
                        db6._資料狀態 = "A";

                        dt = db6.GetDataBySPNO(oConn, oTran);

                        if (dt.Rows.Count > 0)
                        {
                            db6._guid = dt.Rows[0]["guid"].ToString().Trim();
                            db6.UpdateData2(oConn, oTran);
                        }
                        else
                        {
                            db6.InsertData2(oConn, oTran);
                        }

                        dt.Clear();

                        break;
                    #endregion

                    #region 槽區管線

                    case "tankpipeline":

                        db7._業者guid = cpid;
                        db7._年度 = year;
                        db7._轄區儲槽編號 = sheetRow(sheet, i, 0);
                        db7._管線具保溫層 = sheetRow(sheet, i, 1);
                        db7._管線支撐座腐蝕疑慮 = sheetRow(sheet, i, 2);
                        db7._備註 = sheetRow(sheet, i, 3);
                        db7._建立者 = LogInfo.mGuid;
                        db7._修改者 = LogInfo.mGuid;
                        db7._資料狀態 = "A";

                        dt = db7.GetDataBySPNO(oConn, oTran);

                        if (dt.Rows.Count > 0)
                        {
                            db7._guid = dt.Rows[0]["guid"].ToString().Trim();
                            db7.UpdateData(oConn, oTran);
                        }
                        else
                        {
                            db7.InsertData(oConn, oTran);
                        }

                        dt.Clear();

                        break;
                    #endregion

                    #region 管線基本資料

                    case "tubeinfo":

                        db9._業者guid = cpid;
                        db9._年度 = "110";
                        db9._長途管線識別碼 = sheetRow(sheet, i, 0);
                        db9._轄區長途管線名稱 = sheetRow(sheet, i, 1);
                        db9._銜接管線識別碼_上游 = sheetRow(sheet, i, 2);
                        db9._銜接管線識別碼_下游 = sheetRow(sheet, i, 3);
                        db9._起點 = sheetRow(sheet, i, 4);
                        db9._迄點 = sheetRow(sheet, i, 5);
                        db9._管徑吋 = sheetRow(sheet, i, 6);
                        db9._厚度 = OnlyMonthDay(sheetRow(sheet, i, 7));
                        db9._管材 = sheetRow(sheet, i, 8);
                        db9._包覆材料 = sheetRow(sheet, i, 9);
                        db9._轄管長度 = sheetRow(sheet, i, 10);
                        db9._內容物 = sheetRow(sheet, i, 11);
                        db9._八大油品 = sheetRow(sheet, i, 12);
                        db9._緊急遮斷閥 = sheetRow(sheet, i, 13);
                        db9._建置年 = sheetRow(sheet, i, 14);
                        db9._設計壓力 = sheetRow(sheet, i, 15);
                        db9._使用壓力 = sheetRow(sheet, i, 16);
                        db9._使用狀態 = sheetRow(sheet, i, 17);
                        db9._附掛橋樑數量 = sheetRow(sheet, i, 18);
                        db9._管線穿越箱涵數量 = sheetRow(sheet, i, 19);
                        db9._建立者 = LogInfo.mGuid;
                        db9._修改者 = LogInfo.mGuid;
                        db9._資料狀態 = "A";

                        dt = db9.GetDataBySPNO(oConn, oTran);

                        if (dt.Rows.Count > 0)
                        {
                            db9._guid = dt.Rows[0]["guid"].ToString().Trim();
                            db9.UpdateData(oConn, oTran);
                        }
                        else
                        {
                            db9.InsertData(oConn, oTran);
                        }

                        dt.Clear();

                        db10._業者guid = cpid;
                        db10._年度 = "110";
                        db10._長途管線識別碼 = sheetRow(sheet, i, 0);
                        db10._轄區長途管線名稱 = sheetRow(sheet, i, 1);
                        db10._活動斷層敏感區 = sheetRow(sheet, i, 20);
                        db10._土壤液化區 = sheetRow(sheet, i, 21);
                        db10._土石流潛勢區 = sheetRow(sheet, i, 22);
                        db10._淹水潛勢區 = sheetRow(sheet, i, 23);
                        db10._建立者 = LogInfo.mGuid;
                        db10._修改者 = LogInfo.mGuid;
                        db10._資料狀態 = "A";

                        dt = db10.GetDataBySPNO(oConn, oTran);

                        if (dt.Rows.Count > 0)
                        {
                            db10._guid = dt.Rows[0]["guid"].ToString().Trim();
                            db10.UpdateData(oConn, oTran);
                        }
                        else
                        {
                            db10.InsertData(oConn, oTran);
                        }

                        dt.Clear();

                        break;

                    #endregion
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// 傳回該列的值
    /// </summary>
    public string sheetRow(ISheet sheet, int rowNum, int cellNum)
    {
        return sheet.GetRow(rowNum).GetCell(cellNum, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
    }

    /// <summary>
    /// 確認字串是否有特殊符號
    /// </summary>
    public bool ContainsOnlyAlphanumeric(string str)
    {
        // 使用正規表達式檢查字串是否只包含字母和數字
        return Regex.IsMatch(str, @"^[a-zA-Z0-9]+$");
    }

    /// <summary>
    /// 字串是只有月份+日期回傳 月份/日期
    /// </summary>
    public string OnlyMonthDay(string str)
    {
        if(str.Length == 4)
        {
            str = str.Substring(0, 2) + "/" + str.Substring(2, 2);
        }
        else
        {
            str = str.Substring(0, 3) + "/" + str.Substring(3, 2);
        }

        return str;
    }
}