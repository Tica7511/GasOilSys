using System;
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
                    case "storagetankinfo":
                        #region 儲槽基本資料_常壓地上式儲槽

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

                        #endregion
                        break;
                    case "storagetankinfoliquefaction":
                        #region 儲槽基本資料_液化石油氣儲槽

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

                        #endregion
                        break;
                    case "storagetankBWT":
                        #region 儲槽基礎、壁板、頂板

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
                        #endregion
                        break;
                }
            }
        }

        return msg;
    }

    #endregion

    #region 匯入資料

    public void AddImportData(ISheet sheet, string category, SqlConnection oConn, SqlTransaction oTran, string cpid, string year)
    {
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row != null)
            {
                switch (category)
                {
                    case "storagetankinfo":
                        #region 儲槽基本資料_常壓地上式儲槽

                        db1._業者guid = cpid;
                        db1._年度 = year;
                        db1._轄區儲槽編號 = (sheetRow(sheet, i, 0) == null) ? "" : sheetRow(sheet, i, 0);
                        db1._能源局編號 = (sheetRow(sheet, i, 1) == null) ? "" : sheetRow(sheet, i, 1);
                        db1._容量 = (sheetRow(sheet, i, 2) == null) ? "" : sheetRow(sheet, i, 2);
                        db1._內徑 = (sheetRow(sheet, i, 3) == null) ? "" : sheetRow(sheet, i, 3);
                        db1._內容物 = (sheetRow(sheet, i, 4) == null) ? "" : sheetRow(sheet, i, 4);
                        db1._油品種類 = (sheetRow(sheet, i, 5) == null) ? "" : sheetRow(sheet, i, 5);
                        db1._形式 = (sheetRow(sheet, i, 6) == null) ? "" : sheetRow(sheet, i, 6);
                        db1._啟用日期 = (sheetRow(sheet, i, 7) == null) ? "" : OnlyMonthDay(sheetRow(sheet, i, 7));
                        db1._代行檢查_代檢機構1 = (sheetRow(sheet, i, 8) == null) ? "" : sheetRow(sheet, i, 8);
                        db1._代行檢查_外部日期1 = (sheetRow(sheet, i, 9) == null) ? "" : sheetRow(sheet, i, 9);
                        db1._代行檢查_代檢機構2 = (sheetRow(sheet, i, 10) == null) ? "" : sheetRow(sheet, i, 10);
                        db1._代行檢查_外部日期2 = (sheetRow(sheet, i, 11) == null) ? "" : sheetRow(sheet, i, 11);
                        db1._狀態 = (sheetRow(sheet, i, 12) == null) ? "" : sheetRow(sheet, i, 12);
                        db1._延長開放年限 = (sheetRow(sheet, i, 13) == null) ? "" : sheetRow(sheet, i, 13);
                        db1._差異說明 = (sheetRow(sheet, i, 14) == null) ? "" : sheetRow(sheet, i, 14);
                        db1._建立者 = LogInfo.mGuid;
                        db1._修改者 = LogInfo.mGuid;
                        db1._資料狀態 = "A";

                        db1.InsertData(oConn, oTran);

                        #endregion
                        break;
                    case "storagetankinfoliquefaction":
                        #region 儲槽基本資料_液化石油氣儲槽

                        db3._業者guid = cpid;
                        db3._年度 = year;
                        db3._轄區儲槽編號 = (sheetRow(sheet, i, 0) == null) ? "" : sheetRow(sheet, i, 0);
                        db3._能源局編號 = (sheetRow(sheet, i, 1) == null) ? "" : sheetRow(sheet, i, 1);
                        db3._容量 = (sheetRow(sheet, i, 2) == null) ? "" : sheetRow(sheet, i, 2);
                        db3._內徑 = (sheetRow(sheet, i, 3) == null) ? "" : sheetRow(sheet, i, 3);
                        db3._內容物 = (sheetRow(sheet, i, 4) == null) ? "" : sheetRow(sheet, i, 4);
                        db3._油品種類 = (sheetRow(sheet, i, 5) == null) ? "" : sheetRow(sheet, i, 5);
                        db3._形式 = (sheetRow(sheet, i, 6) == null) ? "" : sheetRow(sheet, i, 6);
                        db3._啟用日期 = (sheetRow(sheet, i, 7) == null) ? "" : OnlyMonthDay(sheetRow(sheet, i, 7));
                        db3._狀態 = (sheetRow(sheet, i, 8) == null) ? "" : sheetRow(sheet, i, 8);
                        db3._差異說明 = (sheetRow(sheet, i, 9) == null) ? "" : sheetRow(sheet, i, 9);
                        db3._建立者 = LogInfo.mGuid;
                        db3._修改者 = LogInfo.mGuid;
                        db3._資料狀態 = "A";

                        db3.InsertData(oConn, oTran);

                        #endregion
                        break;
                    case "storagetankBWT":
                        #region 儲槽基礎、壁板、頂板

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

                        db2.InsertData(oConn, oTran);

                        #endregion
                        break;
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
        return sheet.GetRow(rowNum).GetCell(cellNum).ToString().Trim();
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