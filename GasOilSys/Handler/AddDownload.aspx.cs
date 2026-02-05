using System;
using Aspose.Words;
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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using NPOI.SS.Formula.Functions;
using System.Text;
using System.Security.Claims;
using Aspose.Words.Markup;

public partial class Handler_AddDownload : System.Web.UI.Page
{
    OilReportUpload_DB ordb = new OilReportUpload_DB();
    GasReportUpload_DB grdb = new GasReportUpload_DB();
    OilInsideInspect_DB oidb = new OilInsideInspect_DB();
    GasInsideInspect_DB gidb = new GasInsideInspect_DB();
    OilInternalAudit_DB oiadb = new OilInternalAudit_DB();
    GasOnlineEvaluation_DB goedb = new GasOnlineEvaluation_DB();
    OilOnlineEvaluation_DB ooedb = new OilOnlineEvaluation_DB();
    FileTable fdb = new FileTable();
    CodeTable_DB cdb = new CodeTable_DB();
    Oil_CommitteeSuggestionDemoFile_DB ocsdfdb = new Oil_CommitteeSuggestionDemoFile_DB();
    OilCommitteeSuggestionData_DB ocsdb = new OilCommitteeSuggestionData_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增 檔案下載
        ///說    明:
        /// * Request["guid"]: guid 
        /// * Request["cpid"]: 業者Guid 
        /// * Request["category"]: 網頁類別 gas/oil 
        /// * Request["type"]: 檔案類型  
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

            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string category = (string.IsNullOrEmpty(Request["category"])) ? "" : Request["category"].ToString().Trim();
            string filecategory = (string.IsNullOrEmpty(Request["filecategory"])) ? "" : Request["filecategory"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string details = (string.IsNullOrEmpty(Request["details"])) ? "" : Request["details"].ToString().Trim();
            string PublicGuid = string.Empty;
            string PublicOrgName = string.Empty;
            string PublicNewName = string.Empty;
            string PublicExtension = string.Empty;
            string xmlstr = string.Empty;
            DataTable dt = new DataTable();

            Aspose.Words.License license = new Aspose.Words.License();
            license.SetLicense(Server.MapPath("~/Bin/Aspose.Total.lic"));

            #region 檢查資料庫是否有檔案
            switch (category)
            {
                case "gas":
                    switch (type)
                    {
                        //case "report":
                        //    grdb._業者guid = cpid;
                        //    dt = grdb.GetList();
                        //    if (dt.Rows.Count > 0)
                        //        throw new Exception("請先刪除報告再上傳");
                        //    break;
                        case "pipeinspect":
                            gidb._guid = guid;
                            dt = gidb.GetData();
                            if (dt.Rows.Count > 0)
                                if (!string.IsNullOrEmpty(dt.Rows[0]["佐證資料檔名"].ToString().Trim()))
                                    throw new Exception("請先刪除佐證檔案再上傳");
                            break;
                    }                    
                    break;
                case "oil":
                    switch (type)
                    {
                        //case "report":
                        //    ordb._業者guid = cpid;
                        //    dt = ordb.GetList();
                        //    if (dt.Rows.Count > 0)
                        //        throw new Exception("請先刪除簡報再上傳");
                        //    break;
                        case "pipeinspect":
                            oidb._guid = guid;
                            dt = oidb.GetData();
                            if (dt.Rows.Count > 0)
                                if(!string.IsNullOrEmpty(dt.Rows[0]["佐證資料檔名"].ToString().Trim()))
                                    throw new Exception("請先刪除佐證檔案再上傳");
                            break;
                        case "storageinspect":
                            oiadb._guid = guid;
                            dt = oiadb.GetData();
                            if (dt.Rows.Count > 0)
                                if (!string.IsNullOrEmpty(dt.Rows[0]["佐證資料檔名"].ToString().Trim()))
                                    throw new Exception("請先刪除佐證檔案再上傳");
                            break;
                    }                    
                    break;
            }

            dt.Clear();
            #endregion

            string sn = string.Empty;

            if(filecategory != "new")
            {
                string tmpGuid = Guid.NewGuid().ToString("N");
                string UpLoadPath = string.Empty;

                cdb._群組代碼 = "047";
                cdb._項目代碼 = filecategory;
                DataTable cdt = cdb.GetList();

                if (cdt.Rows.Count > 0)
                {
                    ocsdfdb._項目代碼 = cdt.Rows[0]["項目代碼"].ToString().Trim();
                    DataTable ocdt = ocsdfdb.GetList();

                    if (ocdt.Rows.Count > 0)
                    {
                        UpLoadPath += Server.MapPath("~/Sample/" + cdt.Rows[0]["項目名稱"].ToString().Trim() + "/" + ocdt.Rows[0]["項目名稱"].ToString().Trim());
                        //原檔名
                        string orgName = Path.GetFileNameWithoutExtension(UpLoadPath);

                        //副檔名
                        string extension = Path.GetExtension(UpLoadPath);

                        //原檔名完整名稱
                        string orgFullName = orgName + extension;

                        //新檔名
                        string newName = string.Empty;

                        //新檔名完整名稱
                        string newFullName = string.Empty;

                        newName = orgName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        newFullName = newName + extension;

                        string file_size = new FileInfo(UpLoadPath).Length.ToString();

                        string targetDir = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Oil_Upload\\suggestionimport\\" + tmpGuid + "\\";
                        string targetPath = Path.Combine(targetDir, newFullName);

                        if (!Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }

                        File.Copy(UpLoadPath, targetPath, true);

                        Document doc = new Document(targetPath);

                        // 取得自訂屬性集合
                        var customProps = doc.CustomDocumentProperties;

                        if (!customProps.Contains("TemplateID"))
                        {
                            throw new Exception("文件未包含金鑰，禁止上傳。");
                        }

                        string value = customProps["TemplateID"].ToString();
                        if (value != cdt.Rows[0]["項目名稱"].ToString().Trim())
                        {
                            throw new Exception("金鑰不正確，請使用系統範本上傳。");
                        }

                        string osn = string.Empty;

                        ocsdb._類型 = ocdt.Rows[0]["項目代碼"].ToString().Trim();
                        DataTable oscdt = ocsdb.GetMaxSn(oConn, myTrans);
                        if (oscdt.Rows.Count > 0)
                        {
                            if (string.IsNullOrEmpty(oscdt.Rows[0]["Sort"].ToString()))
                            {
                                osn = "1";
                            }
                            else
                            {
                                osn = oscdt.Rows[0]["Sort"].ToString();
                            }                            
                        }
                        else
                        {
                            osn = "1";
                        }

                        //DocumentBuilder builder = new DocumentBuilder(doc);
                        //if (doc.Range.Bookmarks["FormNumber"] != null)
                        //{
                        //    builder.MoveToBookmark("FormNumber");
                        //    builder.Write(cdt.Rows[0]["項目名稱"].ToString().Trim() + "-" + osn);
                        //}

                        NodeCollection allContentControls = doc.GetChildNodes(NodeType.StructuredDocumentTag, true);

                        // 建立一個 DocumentBuilder 實例
                        DocumentBuilder builder = new DocumentBuilder(doc);

                        // 遍歷所有內容控制項
                        foreach (StructuredDocumentTag sdt in allContentControls)
                        {
                            // 在檢查標籤之前先檢查 sdt.Tag 是否為 null，避免 NullReferenceException
                            if (sdt.Tag != null && sdt.Tag == "FormNo")
                            {
                                // 如果找到標籤為 "FormNo" 的內容控制項，則執行以下操作

                                // 清除內容控制項內的現有內容
                                sdt.RemoveAllChildren();

                                // 檢查內容控制項是否為空，如果是則新增一個段落來避免 MoveTo 的 null 錯誤
                                if (sdt.FirstChild == null)
                                {
                                    sdt.AppendChild(new Aspose.Words.Paragraph(doc));
                                }

                                // 將 builder 移動到內容控制項的第一個子節點
                                builder.MoveTo(sdt.FirstChild);

                                // 寫入您想要的文字
                                // 在寫入之前，也確保 cdt 和 osn 不為 null
                                string writeText = "表單編號: " + cdt.Rows[0]["項目名稱"].ToString().Trim() + "-" + osn;
                                builder.Write(writeText);
                            }
                        }

                        doc.Save(targetPath);


                        PublicGuid = tmpGuid;

                        PublicOrgName = orgName;
                        PublicNewName = newName;
                        PublicExtension = extension;

                        fdb._guid = tmpGuid;
                        fdb._年度 = year;
                        fdb._業者guid = tmpGuid;
                        fdb._檔案類型 = details;
                        fdb._原檔名 = orgName;
                        fdb._新檔名 = newName;
                        fdb._附檔名 = extension;
                        fdb._排序 = "1";
                        fdb._檔案大小 = file_size;
                        fdb._修改者 = LogInfo.mGuid;
                        fdb._修改日期 = DateTime.Now;
                        fdb._建立者 = LogInfo.mGuid;
                        fdb._建立日期 = DateTime.Now;

                        fdb.UpdateFile_Trans(oConn, myTrans);

                        ocsdb._guid = tmpGuid;
                        ocsdb._年度 = year;
                        ocsdb._標題 = orgName;
                        ocsdb._狀態 = "0";
                        ocsdb._類型 = ocdt.Rows[0]["項目代碼"].ToString().Trim();
                        ocsdb._排序 = osn;
                        ocsdb._修改者 = LogInfo.mGuid;
                        ocsdb._修改日期 = DateTime.Now;
                        ocsdb._建立者 = LogInfo.mGuid;
                        ocsdb._建立日期 = DateTime.Now;

                        ocsdb.InsertData_Trans(oConn, myTrans);
                    }
                    else
                    {
                        throw new Exception("資料庫內尚無此範例檔，請通知系統管理員新增檔案");
                    }
                }
                else
                {
                    throw new Exception("資料庫內尚無此範例檔，請通知系統管理員新增檔案");
                }
            }
            else
            {
                // 檔案上傳
                HttpFileCollection uploadFiles = Request.Files;
                for (int i = 0; i < uploadFiles.Count; i++)
                {
                    HttpPostedFile File = uploadFiles[i];
                    if (File.FileName.Trim() != "")
                    {
                        string tmpGuid = (string.IsNullOrEmpty(guid)) ? Guid.NewGuid().ToString("N") : guid;
                        string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
                        switch (category)
                        {
                            case "gas":
                                switch (type)
                                {
                                    case "report":
                                        UpLoadPath += "Gas_Upload\\report\\";
                                        break;
                                    case "pipeinspect":
                                        UpLoadPath += "Gas_Upload\\pipeinspect\\";
                                        break;
                                    case "online":
                                        switch (details)
                                        {
                                            case "1":
                                                UpLoadPath += "Gas_Upload\\online\\pipeline\\";
                                                break;
                                            case "2":
                                                UpLoadPath += "Gas_Upload\\online\\storage\\";
                                                break;
                                            case "3":
                                                UpLoadPath += "Gas_Upload\\online\\disaster\\";
                                                break;
                                            case "4":
                                                UpLoadPath += "Gas_Upload\\online\\installation\\";
                                                break;
                                            case "5":
                                                UpLoadPath += "Gas_Upload\\online\\law\\";
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case "oil":
                                switch (type)
                                {
                                    case "report":
                                        UpLoadPath += "Oil_Upload\\report\\";
                                        break;
                                    case "pipeinspect":
                                        UpLoadPath += "Oil_Upload\\pipeinspect\\";
                                        break;
                                    case "storageinspect":
                                        UpLoadPath += "Oil_Upload\\storageinspect\\";
                                        break;
                                    case "suggestionimport":
                                        UpLoadPath += "Oil_Upload\\suggestionimport\\" + tmpGuid + "\\";
                                        break;
                                    case "online":
                                        switch (details)
                                        {
                                            case "1":
                                                UpLoadPath += "Oil_Upload\\online\\pipeline\\";
                                                break;
                                            case "2":
                                                UpLoadPath += "Oil_Upload\\online\\storage\\";
                                                break;
                                            case "3":
                                                UpLoadPath += "Oil_Upload\\online\\disaster\\";
                                                break;
                                            case "4":
                                                UpLoadPath += "Oil_Upload\\online\\installation\\";
                                                break;
                                            case "5":
                                                UpLoadPath += "Oil_Upload\\online\\law\\";
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case "publicgas":
                                switch (type)
                                {
                                    case "info":
                                        switch (details)
                                        {
                                            case "14":
                                                UpLoadPath += "PublicGas\\info\\checkreport\\";
                                                break;
                                            case "15":
                                                UpLoadPath += "PublicGas\\info\\report\\";
                                                break;
                                            case "16":
                                                UpLoadPath += "PublicGas\\info\\resultreport\\";
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }

                        //原檔名
                        string orgName = Path.GetFileNameWithoutExtension(File.FileName);

                        //副檔名
                        string extension = System.IO.Path.GetExtension(File.FileName).ToLower();

                        //原檔名完整名稱
                        string orgFullName = orgName + extension;

                        //新檔名
                        string newName = string.Empty;

                        //新檔名完整名稱
                        string newFullName = string.Empty;

                        if (type == "pipeinspect" || type == "storageinspect")
                        {
                            newName = orgName + "_" + guid;
                            newFullName = orgName + "_" + guid + extension;
                        }
                        else if (type == "suggestionimport")
                        {
                            newName = orgName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            newFullName = newName + extension;
                        }
                        else
                        {
                            newName = orgName;
                            newFullName = orgName + extension;
                        }

                        string file_size = File.ContentLength.ToString();

                        //如果上傳路徑中沒有該目錄，則自動新增
                        if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                        {
                            Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                        }

                        File.SaveAs(UpLoadPath + newFullName);

                        //if (type == "suggestionimport")
                        //{
                        //    string savedFilePath = Path.Combine(UpLoadPath, newFullName);

                        //    Document doc = new Document(savedFilePath);

                        //    // 取得自訂屬性集合
                        //    var customProps = doc.CustomDocumentProperties;

                        //    if (!customProps.Contains("TemplateID"))
                        //    {
                        //        throw new Exception("文件未包含金鑰，禁止上傳。");
                        //    }

                        //    string value = customProps["TemplateID"].ToString();
                        //    if (value != "F-AD-2-10B")
                        //    {
                        //        throw new Exception("金鑰不正確，請使用系統範本上傳。");
                        //    }

                        //    DocumentBuilder builder = new DocumentBuilder(doc);
                        //    if (doc.Range.Bookmarks["FormNumber"] != null)
                        //    {
                        //        builder.MoveToBookmark("FormNumber");
                        //        builder.Write("F-AD-2-10B-01");
                        //    }

                        //    doc.Save(savedFilePath);
                        //}

                        PublicGuid = tmpGuid;

                        switch (category)
                        {
                            case "gas":
                                switch (type)
                                {
                                    case "report":
                                        grdb._guid = tmpGuid;
                                        grdb._業者guid = cpid;
                                        grdb._年度 = year;
                                        grdb._檔案名稱 = orgFullName;
                                        grdb._新檔名 = newFullName;
                                        grdb._建立者 = LogInfo.mGuid;
                                        grdb._修改者 = LogInfo.mGuid;

                                        dt = grdb.GetDataFileName();
                                        if (dt.Rows.Count > 0)
                                            if (!string.IsNullOrEmpty(dt.Rows[0]["檔案名稱"].ToString().Trim()))
                                                throw new Exception("有相同的檔案名稱，請先刪除檔案再上傳");

                                        grdb.SaveFile(oConn, myTrans);
                                        break;
                                    case "pipeinspect":
                                        gidb._guid = guid;
                                        gidb._佐證資料檔名 = orgName;
                                        gidb._佐證資料副檔名 = extension;
                                        gidb._新檔名 = newName;
                                        gidb._佐證資料路徑 = UpLoadPath;
                                        gidb._建立者 = LogInfo.mGuid;
                                        gidb._修改者 = LogInfo.mGuid;

                                        gidb.SaveFile(oConn, myTrans);
                                        break;
                                    case "online":
                                        goedb._guid = tmpGuid;
                                        goedb._業者guid = cpid;
                                        goedb._年度 = year;
                                        goedb._檔案類型 = details;
                                        goedb._檔案名稱 = orgFullName;
                                        goedb._新檔名 = newFullName;
                                        goedb._建立者 = LogInfo.mGuid;
                                        goedb._修改者 = LogInfo.mGuid;

                                        goedb.SaveFile(oConn, myTrans);
                                        break;
                                }
                                break;
                            case "oil":
                                switch (type)
                                {
                                    case "report":
                                        ordb._guid = tmpGuid;
                                        ordb._業者guid = cpid;
                                        ordb._年度 = year;
                                        ordb._檔案名稱 = orgFullName;
                                        ordb._新檔名 = newFullName;
                                        ordb._建立者 = LogInfo.mGuid;
                                        ordb._修改者 = LogInfo.mGuid;

                                        dt = ordb.GetDataFileName();
                                        if (dt.Rows.Count > 0)
                                            if (!string.IsNullOrEmpty(dt.Rows[0]["檔案名稱"].ToString().Trim()))
                                                throw new Exception("有相同的檔案名稱，請先刪除檔案再上傳");

                                        ordb.SaveFile(oConn, myTrans);
                                        break;
                                    case "pipeinspect":
                                        oidb._guid = guid;
                                        oidb._佐證資料檔名 = orgName;
                                        oidb._佐證資料副檔名 = extension;
                                        oidb._新檔名 = newName;
                                        oidb._佐證資料路徑 = UpLoadPath;
                                        oidb._建立者 = LogInfo.mGuid;
                                        oidb._修改者 = LogInfo.mGuid;

                                        oidb.SaveFile(oConn, myTrans);
                                        break;
                                    case "suggestionimport":
                                        PublicOrgName = orgName;
                                        PublicNewName = newName;
                                        PublicExtension = extension;

                                        fdb._guid = tmpGuid;
                                        fdb._年度 = year;
                                        fdb._業者guid = tmpGuid;
                                        fdb._檔案類型 = details;
                                        fdb._原檔名 = orgName;
                                        fdb._新檔名 = newName;
                                        fdb._附檔名 = extension;
                                        fdb._排序 = "1";
                                        fdb._檔案大小 = file_size;
                                        fdb._修改者 = LogInfo.mGuid;
                                        fdb._修改日期 = DateTime.Now;
                                        fdb._建立者 = LogInfo.mGuid;
                                        fdb._建立日期 = DateTime.Now;

                                        fdb.UpdateFile_Trans(oConn, myTrans);                                        
                                        break;
                                    case "storageinspect":
                                        oiadb._guid = guid;
                                        oiadb._佐證資料檔名 = orgName;
                                        oiadb._新檔名 = newName;
                                        oiadb._佐證資料副檔名 = extension;
                                        oiadb._佐證資料路徑 = UpLoadPath;
                                        oiadb._建立者 = LogInfo.mGuid;
                                        oiadb._修改者 = LogInfo.mGuid;

                                        oiadb.SaveFile(oConn, myTrans);
                                        break;
                                    case "online":
                                        ooedb._guid = tmpGuid;
                                        ooedb._業者guid = cpid;
                                        ooedb._年度 = year;
                                        ooedb._檔案類型 = details;
                                        ooedb._檔案名稱 = orgFullName;
                                        ooedb._新檔名 = newFullName;
                                        ooedb._建立者 = LogInfo.mGuid;
                                        ooedb._修改者 = LogInfo.mGuid;

                                        ooedb.SaveFile(oConn, myTrans);
                                        break;
                                }
                                break;
                            case "publicgas":
                                switch (type)
                                {
                                    case "info":
                                        fdb._guid = "";
                                        fdb._業者guid = cpid;
                                        fdb._檔案類型 = details;
                                        DataTable fdt = new DataTable();
                                        fdt = fdb.GetData(oConn, myTrans);

                                        if (fdt.Rows.Count == 0)
                                        {
                                            sn = "0" + (i + 1).ToString();
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(sn))
                                            {
                                                if (0 < Convert.ToInt32(sn) && Convert.ToInt32(sn) < 9)
                                                {
                                                    sn = "0" + (Convert.ToInt32(sn) + 1).ToString();
                                                }
                                                else
                                                {
                                                    sn = (Convert.ToInt32(sn) + 1).ToString();
                                                }
                                            }
                                            else
                                            {
                                                DataTable ffdt = fdb.GetNoYearMaxSn();

                                                if (ffdt.Rows.Count > 0)
                                                {
                                                    int maxsn = Convert.ToInt32(ffdt.Rows[0]["Sort"].ToString().Trim());
                                                    if (maxsn > 9)
                                                        sn = maxsn.ToString();
                                                    else
                                                        sn = "0" + maxsn.ToString();
                                                }
                                                else
                                                {
                                                    sn = "01";
                                                }
                                            }
                                        }

                                        fdb._guid = tmpGuid;
                                        fdb._年度 = year;
                                        fdb._原檔名 = orgName;
                                        fdb._新檔名 = newName;
                                        fdb._附檔名 = extension;
                                        fdb._排序 = sn;
                                        fdb._檔案大小 = file_size;
                                        fdb._修改者 = LogInfo.mGuid;
                                        fdb._修改日期 = DateTime.Now;
                                        fdb._建立者 = LogInfo.mGuid;
                                        fdb._建立日期 = DateTime.Now;

                                        fdb.UpdateFile_Trans(oConn, myTrans);
                                        break;
                                }
                                break;
                        }
                    }
                }
            }            

            myTrans.Commit();

            string jwtToken = GenerateJwt(PublicGuid, PublicOrgName, PublicNewName, PublicExtension);

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response><fileName>" + PublicOrgName + PublicExtension + 
                "</fileName><fileNewName>" + PublicNewName + PublicExtension + "</fileNewName><onlyofficeguid>" + PublicGuid + "</onlyofficeguid><token>" + 
                jwtToken + "</token><mGuid>" + LogInfo.mGuid + "</mGuid><mName>" + LogInfo.name + "</mName><cGuid>" + PublicGuid + "</cGuid></root>";
            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            myTrans.Rollback();
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        finally
        {
            oCmmd.Connection.Close();
            oConn.Close();
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);


    }

    public static string GenerateJwt(string tmpGuid, string fileName, string fileNewName, string fileextension)
    {
        //將 JWT secret 包成 HmacSha256 的加密格式
        var secret = ConfigurationManager.AppSettings["JwtSecret"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //將payload用JSON格式包成跟前端一樣
        var payload = new JwtPayload
        {
            { "document", new Dictionary<string, object> 
                { 
                    { "fileType", "docx" }, 
                    { "key", tmpGuid }, 
                    { "title", fileName + fileextension }, 
                    { "url", "http://172.20.10.5:54315/DOWNLOAD.aspx?category=Oil&type=suggestionimport&cpid=" + tmpGuid + "&v=" + fileNewName + fileextension }
                } 
            },
            { "documentType", "word" },
            { "editorConfig", new Dictionary<string, object>
                {
                    { "mode", "edit" },
                    { "lang", "zh-TW" },
                    { "callbackUrl", "http://172.20.10.5:54315/Handler/SaveCallback.aspx" },
                    { "customization", new Dictionary<string, object>
                        {
                            { "forcesave", true },
                            { "autosave", true },
                            { "autosaveInterval", 60 },
                            { "logo", new Dictionary<string, object>
                                {
                                    { "image", "http://172.20.10.5:54315/images/tccLogo.png" },
                                    { "url", "https://www.cogen.com.tw/tw/" }
                                }
                            },
                            { "layout", new Dictionary<string, object>
                                {
                                    { "leftMenu", new Dictionary<string, object>
                                        {
                                            { "mode", false }
                                        }
                                    },
                                    { "rightMenu", new Dictionary<string, object>
                                        {
                                            { "mode", false }
                                        }
                                    },
                                    { "toolbar", new Dictionary<string, object>
                                        {
                                            { "layout", false },
                                            { "references", false },
                                            { "protect", false },
                                            { "plugins", false }
                                        }
                                    }
                                }
                            }
                            //{ "trackChanges", true },
                        }
                    },
                    { "user", new Dictionary<string, object>
                        {
                            { "id", LogInfo.mGuid },
                            { "name", LogInfo.name }
                        }
                    },
                    { "history", new Dictionary<string, object>
                        {
                            { "serverVersion", true }
                        }
                    }
                }
            },
            { "permissions", new Dictionary<string, object>
                {
                    { "edit", true },
                    //{ "review", true },
                    { "comment", true },
                    { "print", false },
                    { "download", false }
                }
            },
            { "height", "800px" },
            { "width", "100%" },
            { "type", "desktop" }
        };

        //組合 header + payload + signature
        var token = new JwtSecurityToken(new JwtHeader(creds), payload);

        //回傳 token 字串
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}