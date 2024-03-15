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
using NPOI.XSSF.UserModel; // For Excel 2007 and newer (.xlsx) files
using NPOI.HSSF.UserModel; // For Excel 2003 and older (.xls) files
using NPOI.SS.UserModel;//-- v.1.2.4起 新增的。

public partial class Handler_OilImport : System.Web.UI.Page
{
    OilTubeCheck_DB odb = new OilTubeCheck_DB();
    OilStorageTankInfo_DB db1 = new OilStorageTankInfo_DB();
    OilStorageTankBWT_DB db2 = new OilStorageTankBWT_DB();
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
                    case "storagetankBWT":
                        #region 儲槽基礎、壁板、頂板

                        if (sheet.GetRow(i).GetCell(0) != null)
                        {
                            if (sheet.GetRow(i).GetCell(0).ToString().Trim().Length > 50)
                            {
                                msg += "【轄區儲槽編號】字數不可大於50\r\n";
                            }                            
                            else
                            {
                                db1._轄區儲槽編號 = sheet.GetRow(i).GetCell(0).ToString().Trim();
                                db1._業者guid = cpid;
                                DataTable dt = db1.GetList();

                                if (dt.Rows.Count < 1)
                                {
                                    msg += "儲槽基本資料內並沒有此編號【" + sheet.GetRow(i).GetCell(0).ToString().Trim() + "】，請至儲槽基本資料頁面新增後再重新匯入\r\n";
                                }
                            }
                        }                            
                                
                        if (sheet.GetRow(i).GetCell(1) != null)
                            if (sheet.GetRow(i).GetCell(1).ToString().Trim().Length > 10)
                                msg += "【基礎與底板間是否具防水包覆層設計】字數不可大於10\r\n";
                        if (sheet.GetRow(i).GetCell(2) != null)
                            if (sheet.GetRow(i).GetCell(2).ToString().Trim().Length > 10)
                                msg += "【沈陷量測點數】字數不可大於10\r\n";
                        if (sheet.GetRow(i).GetCell(3) != null)
                            if (sheet.GetRow(i).GetCell(3).ToString().Trim().Length > 10)
                                msg += "【沈陷量測點數】字數不可大於10\r\n";
                        if (sheet.GetRow(i).GetCell(4) != null)
                            if (sheet.GetRow(i).GetCell(4).ToString().Trim().Length > 10)
                                msg += "【接地電阻<10Ω】字數不可大於10\r\n";
                        if (sheet.GetRow(i).GetCell(5) != null)
                            if (sheet.GetRow(i).GetCell(5).ToString().Trim().Length > 10)
                                msg += "【壁板是否具包附層】字數不可大於10\r\n";
                        if (sheet.GetRow(i).GetCell(6) != null)
                            if (sheet.GetRow(i).GetCell(6).ToString().Trim().Length > 10)
                                msg += "【壁板外部嚴重腐蝕或點蝕】字數不可大於10\r\n";
                        if (sheet.GetRow(i).GetCell(7) != null)
                            if (sheet.GetRow(i).GetCell(7).ToString().Trim().Length > 10)
                                msg += "【第一層壁板內部下方腐蝕】字數不可大於10\r\n";
                        if (sheet.GetRow(i).GetCell(8) != null)
                            if (sheet.GetRow(i).GetCell(8).ToString().Trim().Length > 10)
                                msg += "【維修方式是否有符合API653】字數不可大於10\r\n";
                        if (sheet.GetRow(i).GetCell(9) != null)
                            if (sheet.GetRow(i).GetCell(9).ToString().Trim().Length > 10)
                                msg += "【外浮頂之等電位裝置性能】字數不可大於10\r\n";

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
                    case "storagetankBWT":
                        #region 儲槽基礎、壁板、頂板

                        db2._業者guid = cpid;
                        db2._年度 = year;
                        db2._轄區儲槽編號 = (sheet.GetRow(i).GetCell(0) == null) ? "" : sheet.GetRow(i).GetCell(0).ToString().Trim();
                        db2._防水包覆層設計 = (sheet.GetRow(i).GetCell(1) == null) ? "" : sheet.GetRow(i).GetCell(1).ToString().Trim();
                        db2._沈陷量測點數 = (sheet.GetRow(i).GetCell(2) == null) ? "" : sheet.GetRow(i).GetCell(2).ToString().Trim();
                        db2._沈陷量測日期 = (sheet.GetRow(i).GetCell(3) == null) ? "" : (sheet.GetRow(i).GetCell(3).ToString().Trim().Contains("/") ? sheet.GetRow(i).GetCell(3).ToString().Trim().Replace("/", "") : sheet.GetRow(i).GetCell(3).ToString().Trim());
                        db2._儲槽接地電阻 = (sheet.GetRow(i).GetCell(4) == null) ? "" : sheet.GetRow(i).GetCell(4).ToString().Trim();
                        db2._壁板是否具包覆層 = (sheet.GetRow(i).GetCell(5) == null) ? "" : sheet.GetRow(i).GetCell(5).ToString().Trim();
                        db2._壁板外部嚴重腐蝕或點蝕 = (sheet.GetRow(i).GetCell(6) == null) ? "" : sheet.GetRow(i).GetCell(6).ToString().Trim();
                        db2._第一層壁板內部下方腐蝕 = (sheet.GetRow(i).GetCell(7) == null) ? "" : sheet.GetRow(i).GetCell(7).ToString().Trim();
                        db2._壁板維修方式是否有符合API653 = (sheet.GetRow(i).GetCell(8) == null) ? "" : sheet.GetRow(i).GetCell(8).ToString().Trim();
                        db2._設置等導電良好度 = (sheet.GetRow(i).GetCell(9) == null) ? "" : sheet.GetRow(i).GetCell(9).ToString().Trim();
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
}