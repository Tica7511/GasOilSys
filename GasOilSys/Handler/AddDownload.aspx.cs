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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using NPOI.SS.Formula.Functions;
using System.Text;
using System.Security.Claims;

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
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string details = (string.IsNullOrEmpty(Request["details"])) ? "" : Request["details"].ToString().Trim();
            string PublicGuid = string.Empty;
            string PublicOrgName = string.Empty;
            string PublicNewName = string.Empty;
            string PublicExtension = string.Empty;
            string xmlstr = string.Empty;
            DataTable dt = new DataTable();

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
                            { "autosave", true }
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
            { "height", "100%" },
            { "width", "100%" },
            { "type", "desktop" }
        };

        //組合 header + payload + signature
        var token = new JwtSecurityToken(new JwtHeader(creds), payload);

        //回傳 token 字串
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}