using System;
using System.Web;
using System.Configuration;
using System.Net;
using System.Data;
using System.IO;
using System.Linq;
using Ionic.Zip;
using System.Collections.Generic;

namespace ED.HR.DOWNLOAD.WebForm
{
	public partial class DownloadImage : System.Web.UI.Page
    {
        string OrgName = string.Empty;
        string NewName = string.Empty;
        bool isWord = false;
        string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
        FileTable Fdb = new FileTable();
        GasCompanyInfo_DB GCdb = new GasCompanyInfo_DB();
        OilCompanyInfo_DB OCdb = new OilCompanyInfo_DB();
		GasCheckReport_DB CRGdb = new GasCheckReport_DB();
		OilCheckReport_DB CROdb = new OilCheckReport_DB();
		GasInsideInspect_DB IIGdb = new GasInsideInspect_DB();
		OilInsideInspect_DB IIOdb = new OilInsideInspect_DB();
        OilInternalAudit_DB IAOdb = new OilInternalAudit_DB();
        GasOnlineEvaluation_DB OEGdb = new GasOnlineEvaluation_DB();
        OilOnlineEvaluation_DB OEOdb = new OilOnlineEvaluation_DB();
        //File_DB fdb = new File_DB();
        protected void Page_Load(object sender, EventArgs e)
        {
			try
			{
                string isZip = (string.IsNullOrEmpty(Request["isZip"])) ? "" : Common.FilterCheckMarxString(Request["isZip"].ToString().Trim());
                string cpid = Common.FilterCheckMarxString(Request.QueryString["cpid"]);
                string category = Common.FilterCheckMarxString(Request.QueryString["category"]);
                string type = Common.FilterCheckMarxString(Request.QueryString["type"]);
                string details = Common.FilterCheckMarxString(Request.QueryString["details"]);
                string sn = Common.FilterCheckMarxString(Request.QueryString["sn"]);
                string dirPath = string.Empty;
                DataTable dt = new DataTable();

                if (!string.IsNullOrEmpty(isZip))
                {
                    saveZip();
                }
                else
                {
                    switch (category)
                    {
                        case "Gas":
                            dirPath = "Gas_Upload\\";
                            switch (type)
                            {
                                case "image":
                                    dirPath += "image\\";
                                    OrgName = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    NewName = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    break;
                                case "report":
                                    dirPath += "report\\";
                                    CRGdb._guid = (string.IsNullOrEmpty(Common.FilterCheckMarxString(Request["rid"].ToString().Trim()))) ? "" : Common.FilterCheckMarxString(Request["rid"].ToString().Trim());
                                    dt = CRGdb.GetFileName();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["檔案名稱"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString());
                                    }                                        
                                    break;
                                case "pipeinspect":
                                    dirPath += "pipeinspect\\";
                                    IIGdb._guid = (string.IsNullOrEmpty(Common.FilterCheckMarxString(Request["v"].ToString().Trim()))) ? "" : Common.FilterCheckMarxString(Request["v"].ToString().Trim());
                                    dt = IIGdb.GetFileName();
                                    if (dt.Rows.Count > 0)
                                    {
                                       OrgName = Common.FilterCheckMarxString(dt.Rows[0]["佐證資料檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["佐證資料副檔名"].ToString());
                                       NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["佐證資料副檔名"].ToString());
                                    }                                       
                                    break;
                                case "online":
                                    dirPath += "online\\";
                                    switch (details)
                                    {
                                        case "1":
                                            dirPath += "pipeline\\";
                                            break;
                                        case "2":
                                            dirPath += "storage\\";
                                            break;
                                        case "3":
                                            dirPath += "disaster\\";
                                            break;
                                        case "4":
                                            dirPath += "installation\\";
                                            break;
                                        case "5":
                                            dirPath += "law\\";
                                            break;
                                    }
                                    OEGdb._guid = (string.IsNullOrEmpty(Common.FilterCheckMarxString(Request["rid"].ToString().Trim()))) ? LogInfo.companyGuid : Common.FilterCheckMarxString(Request["rid"].ToString().Trim());
                                    dt = OEGdb.GetFileName();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["檔案名稱"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString());
                                    }                                        
                                    break;
                                case "selfEvaluation":
                                    dirPath += "selfEvaluation\\";
                                    Fdb._guid = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    Fdb._排序 = sn;
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                                case "CIPS":
                                    dirPath += "CIPS\\";
                                    Fdb._guid = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    Fdb._排序 = sn;
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                                case "ILI":
                                    dirPath += "ILI\\";
                                    Fdb._guid = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    Fdb._排序 = sn;
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                            }
                            break;
                        case "Oil":
                            dirPath = "Oil_Upload\\";
                            switch (type)
                            {
                                case "image":
                                    dirPath += "image\\";
                                    OrgName = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    NewName = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    break;
                                case "reservoir":
                                    dirPath += "reservoir\\";
                                    Fdb._guid = Common.Decrypt(Common.FilterCheckMarxString(Request.QueryString["v"]));
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                                case "report":
                                    dirPath += "report\\";
                                    CROdb._guid = (string.IsNullOrEmpty(Common.FilterCheckMarxString(Request["rid"].ToString().Trim()))) ? LogInfo.companyGuid : Common.FilterCheckMarxString(Request["rid"].ToString().Trim());
                                    dt = CROdb.GetFileName();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["檔案名稱"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString());
                                    }                                        
                                    break;
                                case "pipeinspect":
                                    dirPath += "pipeinspect\\";
                                    IIOdb._guid = (string.IsNullOrEmpty(Common.FilterCheckMarxString(Request["v"].ToString().Trim()))) ? "" : Common.FilterCheckMarxString(Request["v"].ToString().Trim());
                                    dt = IIOdb.GetFileName();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["佐證資料檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["佐證資料副檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["佐證資料副檔名"].ToString());
                                    }                                        
                                    break;
                                case "storageinspect":
                                    dirPath += "storageinspect\\";
                                    IAOdb._guid = (string.IsNullOrEmpty(Common.FilterCheckMarxString(Request["v"].ToString().Trim()))) ? "" : Common.FilterCheckMarxString(Request["v"].ToString().Trim());
                                    dt = IAOdb.GetFileName();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["佐證資料檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["佐證資料副檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["佐證資料副檔名"].ToString());
                                    }                                        
                                    break;
                                case "suggestionimport":
                                    Fdb._業者guid = cpid;
                                    dt = Fdb.GetOnlyOfficeData();

                                    if (dt.Rows.Count > 0)
                                    {
                                        dirPath += "suggestionimport\\" + dt.Rows[0]["guid"].ToString().Trim() + "\\";
                                    }

                                    OrgName = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    NewName = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    isWord = true;
                                    break;
                                case "online":
                                    dirPath += "online\\";
                                    switch (details)
                                    {
                                        case "1":
                                            dirPath += "pipeline\\";
                                            break;
                                        case "2":
                                            dirPath += "storage\\";
                                            break;
                                        case "3":
                                            dirPath += "disaster\\";
                                            break;
                                        case "4":
                                            dirPath += "installation\\";
                                            break;
                                        case "5":
                                            dirPath += "law\\";
                                            break;
                                    }
                                    OEOdb._guid = (string.IsNullOrEmpty(Common.FilterCheckMarxString(Request["rid"].ToString().Trim()))) ? LogInfo.companyGuid : Common.FilterCheckMarxString(Request["rid"].ToString().Trim());
                                    dt = OEOdb.GetFileName();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["檔案名稱"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString());
                                    }                                        
                                    break;
                                case "selfEvaluation":
                                    dirPath += "selfEvaluation\\";
                                    Fdb._guid = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    Fdb._排序 = sn;
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                                case "CIPS":
                                    dirPath += "CIPS\\";
                                    Fdb._guid = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    Fdb._排序 = sn;
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                                case "ILI":
                                    dirPath += "ILI\\";
                                    Fdb._guid = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    Fdb._排序 = sn;
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                            }
                            break;
                        case "PublicGas":
                            dirPath += "PublicGas\\";
                            switch (type)
                            {
                                case "Info":
                                    switch (details)
                                    {
                                        case "14":
                                            dirPath += "info\\checkreport\\";
                                            break;
                                        case "15":
                                            dirPath += "info\\report\\";
                                            break;
                                        case "16":
                                            dirPath += "info\\resultreport\\";
                                            break;
                                    }

                                    Fdb._guid = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    Fdb._檔案類型 = details;
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                            }
                            break;
                        case "WeekReport":

                            break;
                        case "VerificationTest":
                            dirPath += "VerificationTest\\";
                            switch (type)
                            {
                                case "Check":
                                    dirPath += "Check\\";
                                    Fdb._guid = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    Fdb._排序 = sn;
                                    Fdb._檔案類型 = details;
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                                case "Relation":
                                    dirPath += "Relation\\";
                                    Fdb._guid = Common.FilterCheckMarxString(Request.QueryString["v"]);
                                    Fdb._排序 = sn;
                                    Fdb._檔案類型 = details;
                                    dt = Fdb.GetFileData();
                                    if (dt.Rows.Count > 0)
                                    {
                                        OrgName = Common.FilterCheckMarxString(dt.Rows[0]["原檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                        NewName = Common.FilterCheckMarxString(dt.Rows[0]["新檔名"].ToString()) + Common.FilterCheckMarxString(dt.Rows[0]["附檔名"].ToString());
                                    }
                                    break;
                            }
                            break;
                    }

                    //原檔名
                    //OrgName = Common.FilterCheckMarxString(Request.QueryString["v"]);

                    // 附件目錄
                    DirectoryInfo dir = new DirectoryInfo(UpLoadPath + dirPath);

                    //列舉全部檔案再比對檔名
                    string FileName = NewName;

                    FileInfo file = dir.EnumerateFiles().FirstOrDefault(m => m.Name == FileName);

                    // 判斷檔案是否存在
                    if (file != null && file.Exists)
                    {
                        if (isWord == true)
                        {
                            StreamDocxForOnlyOffice(file);
                        }
                        else
                        {
                            Download(file);
                        }
                    }
                    else
                    {
                        throw new Exception("File not exist");
                    }                        
                }
			}
			catch (Exception ex)
			{
                string logPath = @"D:\log.txt";
                File.AppendAllText(logPath, string.Format("[{0}] 錯誤：{1}\n", DateTime.Now, ex.Message));

                Response.StatusCode = 500;
                Response.Write("Download error");
                Response.End();
            }
        }


        private void Download(FileInfo DownloadFile)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Response.ContentType = getMineType(DownloadFile.Extension);
            string DownloadName = (OrgName == "") ? DownloadFile.Name : OrgName;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(DownloadName, System.Text.Encoding.UTF8));
            Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            Response.HeaderEncoding = System.Text.Encoding.GetEncoding("Big5");
            Response.WriteFile(DownloadFile.FullName);
            Response.Flush();
            Response.End();
        }

        private void StreamDocxForOnlyOffice(FileInfo docFile)
        {
            File.AppendAllText(@"D:\log.txt", string.Format("[{0}] 觸發 OnlyOffice 檔案串流：{1}\n", DateTime.Now, docFile.FullName));
            string encodedFileName = Uri.EscapeDataString(docFile.Name);

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearHeaders();
            response.ClearContent();
            response.Buffer = false;

            response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            response.AddHeader("Content-Disposition", "inline; filename=\"test1.docx\""); // 避免 filename*= 問題
            response.AddHeader("Content-Length", docFile.Length.ToString());
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AppendHeader("Content-Encoding", "identity");

            // 建議使用 TransmitFile 比 WriteFile 穩定
            response.TransmitFile(docFile.FullName);
            response.Flush();

            // 正確結束流程，不要用 End()
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #region 傳回 ContentType
        /// <summary>
        /// 傳回 ContentType
        /// </summary>
        public static string getMineType(string FileExtension)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(FileExtension);
            if (rk != null && rk.GetValue("Content Type") != null)
                return rk.GetValue("Content Type").ToString();
            else
                return "application/octet-stream";
        }
        #endregion

        #region 將檔案壓縮成ZIP並下載
        public void saveZip()
        {
            string cid = Common.FilterCheckMarxString(Request.QueryString["cid"]);
            string year = Common.FilterCheckMarxString(Request.QueryString["year"]);
            string category = Common.FilterCheckMarxString(Request.QueryString["category"]);
            string details = Common.FilterCheckMarxString(Request.QueryString["details"]);
            string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
            string ZipName = "";
            string dirPath = "";
            DataTable cdt = new DataTable();

            switch (category)
            {
                case "Gas":
                    dirPath += "Gas_Upload\\online\\";

                    GCdb._guid = cid;
                    cdt = GCdb.GetInfo();

                    if (cdt.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(cdt.Rows[0]["中心庫區儲運課工場"].ToString().Trim()))
                        {
                            ZipName += cdt.Rows[0]["營業處廠"].ToString().Trim();
                        }
                        else
                        {
                            ZipName += cdt.Rows[0]["中心庫區儲運課工場"].ToString().Trim();
                        }
                    }

                    switch (details)
                    {
                        case "1":
                            dirPath += "pipeline\\";
                            ZipName += "_查核佐證資料上傳_管線管理";
                            break;
                        case "2":
                            dirPath += "storage\\";
                            ZipName += "_查核佐證資料上傳_儲槽管理";
                            break;
                        case "3":
                            dirPath += "disaster\\";
                            ZipName += "_查核佐證資料上傳_災害防救";
                            break;
                        case "4":
                            dirPath += "installation\\";
                            ZipName += "_查核佐證資料上傳_關鍵基礎設施";
                            break;
                        case "5":
                            dirPath += "law\\";
                            ZipName += "_查核佐證資料上傳_法規";
                            break;
                    }
                    break;
                case "Oil":
                    dirPath += "Oil_Upload\\online\\";

                    OCdb._guid = cid;
                    cdt = OCdb.GetCpName();

                    if (cdt.Rows.Count > 0)
                    {
                        ZipName += cdt.Rows[0]["cpname"].ToString().Trim();
                    }

                    switch (details)
                    {
                        case "1":
                            dirPath += "pipeline\\";
                            ZipName += "_查核佐證資料上傳_管線管理";
                            break;
                        case "2":
                            dirPath += "storage\\";
                            ZipName += "_查核佐證資料上傳_儲槽管理";
                            break;
                        case "3":
                            dirPath += "disaster\\";
                            ZipName += "_查核佐證資料上傳_災害防救";
                            break;
                        case "4":
                            dirPath += "installation\\";
                            ZipName += "_查核佐證資料上傳_關鍵基礎設施";
                            break;
                        case "5":
                            dirPath += "law\\";
                            ZipName += "_查核佐證資料上傳_法規";
                            break;
                    }
                    break;
            }

            DataTable dt = new DataTable();

            if (category == "Gas")
            {
                OEGdb._檔案類型 = details;
                OEGdb._年度 = year;
                OEGdb._業者guid = cid;
                dt = OEGdb.GetThisTypeFile();
            }
            else
            {
                OEOdb._檔案類型 = details;
                OEOdb._年度 = year;
                OEOdb._業者guid = cid;
                dt = OEOdb.GetThisTypeFile();
            }

            if (dt.Rows.Count > 0)
            {
                List<FileInfo> lstFileInfo = new List<FileInfo>();

                for(int i=0; i < dt.Rows.Count; i++)
                {
                    DirectoryInfo dir = new DirectoryInfo(UpLoadPath + dirPath);
                    string FileName = dt.Rows[i]["新檔名"].ToString().Trim();
                    FileInfo file = dir.EnumerateFiles().FirstOrDefault(m => m.Name == FileName);
                    file.MoveTo(UpLoadPath + dirPath + dt.Rows[i]["檔案名稱"].ToString().Trim());

                    lstFileInfo.Add(file);
                }

                //DotNetZip壓縮輸出檔案
                if (lstFileInfo.Count > 0)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.Buffer = false;
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "filename=" + ZipName + ".zip");
                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(System.Text.Encoding.Default))
                    {
                        foreach (FileInfo file in lstFileInfo)
                        {
                            zip.AddFile(file.FullName, "");
                        }
                        zip.Save(Response.OutputStream);
                        zip.Dispose();
                    }
                    Response.Flush();
                    Response.End();
                }
            }
        }
        #endregion

        #region 全檔案壓縮成ZIP
        //public void ZipFileDownload()
        //{
        //    if (Request.QueryString["cid"] != null && Request.QueryString["type"] != null && Request.QueryString["category"] != null && Request.QueryString["details"] != null)
        //    {
        //        string cid = Common.FilterCheckMarxString(Request.QueryString["cid"]);
        //        string category = Common.FilterCheckMarxString(Request.QueryString["category"]);
        //        string type = Common.FilterCheckMarxString(Request.QueryString["type"]);
        //        string details = Common.FilterCheckMarxString(Request.QueryString["details"]);
        //        string ZipName = "";
        //        string dirPath = "";

        //        switch (category)
        //        {
        //            case "Gas":
        //                dirPath += "Gas_Upload\\online\\";
        //                switch (details)
        //                {
        //                    case "1":
        //                        dirPath += "pipeline\\";
        //                        ZipName += "查核佐證資料上傳_管線管理";
        //                        break;
        //                    case "2":
        //                        dirPath += "storage\\";
        //                        ZipName += "查核佐證資料上傳_儲槽管理";
        //                        break;
        //                    case "3":
        //                        dirPath += "disaster\\";
        //                        ZipName += "查核佐證資料上傳_災害防救";
        //                        break;
        //                    case "4":
        //                        dirPath += "installation\\";
        //                        ZipName += "查核佐證資料上傳_關鍵基礎設施";
        //                        break;
        //                    case "5":
        //                        dirPath += "law\\";
        //                        ZipName += "查核佐證資料上傳_法規";
        //                        break;
        //                }
        //                break;
        //            case "Oil":
        //                dirPath += "Oil_Upload\\online\\";
        //                switch (details)
        //                {
        //                    case "1":
        //                        dirPath += "pipeline\\";
        //                        ZipName += "查核佐證資料上傳_管線管理";
        //                        break;
        //                    case "2":
        //                        dirPath += "storage\\";
        //                        ZipName += "查核佐證資料上傳_儲槽管理";
        //                        break;
        //                    case "3":
        //                        dirPath += "disaster\\";
        //                        ZipName += "查核佐證資料上傳_災害防救";
        //                        break;
        //                    case "4":
        //                        dirPath += "installation\\";
        //                        ZipName += "查核佐證資料上傳_關鍵基礎設施";
        //                        break;
        //                    case "5":
        //                        dirPath += "law\\";
        //                        ZipName += "查核佐證資料上傳_法規";
        //                        break;
        //                }
        //                break;
        //        }

        //        Xceed.Zip.ReaderWriter.Licenser.LicenseKey = "ZRT51-L1WSL-4KWJJ-GBEA";
        //        using (Stream zipFileStream = new FileStream(ConfigurationManager.AppSettings["UploadFileRootDir"] + dirPath + ZipName + ".zip", FileMode.Create, FileAccess.Write))
        //        {
        //            DataTable dt = new DataTable();

        //            if (category == "Gas")
        //            {
        //                OEGdb._檔案類型 = details;
        //                OEGdb._業者guid = cid;
        //                dt = OEGdb.GetThisTypeFile();
        //            }
        //            else
        //            {
        //                OEOdb._檔案類型 = details;
        //                OEOdb._業者guid = cid;
        //                dt = OEOdb.GetThisTypeFile();
        //            }

        //            if (dt.Rows.Count > 0)
        //            {
        //                ZipWriter zipWriter = new ZipWriter(zipFileStream);
        //                for (int i = 0; i < dt.Rows.Count; i++)
        //                {

        //                    ZipItemLocalHeader localHeader;
        //                    //一般
        //                    localHeader = new ZipItemLocalHeader(
        //                      dt.Rows[i]["檔案名稱"].ToString(),
        //                      CompressionMethod.Deflated64,
        //                      CompressionLevel.Highest
        //                    );

        //                    zipWriter.WriteItemLocalHeader(localHeader);

        //                    using (Stream sourceStream = new FileStream(ConfigurationManager.AppSettings["UploadFileRootDir"] + dirPath + dt.Rows[i]["檔案名稱"].ToString(), FileMode.Open, FileAccess.Read))
        //                    {
        //                        zipWriter.WriteItemData(sourceStream);
        //                    }
        //                }
        //                zipWriter.CloseZipFile();
        //                zipWriter.Dispose();
        //            }
        //        }

        //        System.IO.FileInfo files = new System.IO.FileInfo(UpLoadPath + dirPath + ZipName + ".zip");
        //        bool filestat = files.Exists;
        //        if (filestat)
        //            Download(files);
        //    }
        //}
        #endregion
    }
}