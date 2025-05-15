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

public partial class Handler_GasImprot : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 天然氣 匯入
        ///說    明:
        /// * Request["cpid"]: 業者guid
        /// * Request["year"]: 年度
        /// * Request["category"]: 類別 
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();

        /// Transaction
        //SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        //oConn.Open();
        //SqlCommand oCmmd = new SqlCommand();
        //oCmmd.Connection = oConn;
        //SqlTransaction myTrans = oConn.BeginTransaction();
        //oCmmd.Transaction = myTrans;
        //try
        //{
        //    #region 檢查登入資訊
        //    if (LogInfo.mGuid == "")
        //    {
        //        throw new Exception("請重新登入");
        //    }
        //    #endregion

        //    string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
        //    string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
        //    string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Request["category"].ToString().Trim();
        //    string xmlstr = string.Empty;

        //    HttpFileCollection uploadFiles = Request.Files;
        //    for (int i = 0; i < uploadFiles.Count; i++)
        //    {
        //        HttpPostedFile File = uploadFiles[i];
        //        if (File.FileName.Trim() != "")
        //        {
        //            string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Oil_Upload\\ImportTemp\\";
        //            string fileName = Path.GetFileName(File.FileName);

        //            //如果上傳路徑中沒有該目錄，則自動新增
        //            if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
        //            {
        //                Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
        //            }
        //            filePath = UpLoadPath + fileName;
        //            //將上傳的匯入檔案暫存到暫存用的資料夾
        //            File.SaveAs(filePath);

        //            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //            {
        //                IWorkbook workbook;
        //                if (Path.GetExtension(filePath) == ".xlsx")
        //                    workbook = new XSSFWorkbook(fileStream); // For Excel 2007 and newer (.xlsx) files
        //                else if (Path.GetExtension(filePath) == ".xls")
        //                    workbook = new HSSFWorkbook(fileStream); // For Excel 2003 and older (.xls) files
        //                else
        //                    throw new Exception("匯入格式不正確，請修改後再重新上傳！");

        //                ISheet sheet = workbook.GetSheetAt(0); // Assuming you want to read the first sheet

        //                if (!string.IsNullOrEmpty(checkValid(sheet, category, cpid, year)))
        //                {
        //                    throw new Exception("匯入格式不正確，請修改以下問題後再重新上傳:\r\n" + checkValid(sheet, category, cpid, year));
        //                }
        //                else
        //                {
        //                    AddImportData(sheet, category, oConn, myTrans, cpid, year);
        //                }
        //            }
        //        }
        //    }

        //    myTrans.Commit();

        //    xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>匯入完成</Response><relogin>N</relogin></root>";

        //    xDoc.LoadXml(xmlstr);
        //}
        //catch (Exception ex)
        //{
        //    myTrans.Rollback();
        //    xDoc = ExceptionUtil.GetExceptionDocument(ex);
        //}
        //finally
        //{
        //    FileInfo fi = new FileInfo(filePath);
        //    if (fi.Exists)
        //    {
        //        fi.Delete(); //刪除暫存的匯入檔案
        //    }
        //    oCmmd.Connection.Close();
        //    oConn.Close();
        //}
        //Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        //xDoc.Save(Response.Output);
    }
}