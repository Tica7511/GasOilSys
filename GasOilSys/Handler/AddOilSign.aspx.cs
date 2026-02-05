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
using Aspose.Words;
using NPOI.SS.Formula.Functions;
using System.Diagnostics;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography;
using NPOI.XSSF.Streaming.Values;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

public partial class Handler_AddOilSign : System.Web.UI.Page
{
    OilCommitteeSuggestionData_DB ocsdb = new OilCommitteeSuggestionData_DB();
    Sign_DB sdb = new Sign_DB();
    FileTable fdb = new FileTable();
    CodeTable_DB cdb = new CodeTable_DB();
    Oil_CommitteeSuggestionDemoFile_DB ocsdfdb = new Oil_CommitteeSuggestionDemoFile_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 簽核表
        ///說    明:
        /// * Request["guid"]: guid
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
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string status = (string.IsNullOrEmpty(Request["status"])) ? "" : Request["status"].ToString().Trim();
            string content = (string.IsNullOrEmpty(Request["content"])) ? "" : Request["content"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string ftype = (string.IsNullOrEmpty(Request["ftype"])) ? "" : Request["ftype"].ToString().Trim();
            string sn = string.Empty;
            string osn = string.Empty;
            string xmlstr = string.Empty;

            #region 更新查核建議表狀態

            ocsdb._guid = guid;
            ocsdb._狀態 = status;
            ocsdb._修改者 = LogInfo.mGuid;
            ocsdb._修改日期 = DateTime.Now;

            ocsdb.UpdateData(oConn, myTrans);

            #endregion

            #region 新增簽核表紀錄

            if(type != "sign")
            {
                File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                                    DateTime.Now + " " + guid + "\r\n");

                sdb._guid = guid;
                sdb._類型 = "01"; // 簽核表
                DataTable sdt = sdb.GetMaxSn();

                if (sdt.Rows.Count > 0)
                {
                    File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                                    DateTime.Now + " " + sdt.Rows[0]["排序"].ToString().Trim() + "\r\n");

                    osn = sdt.Rows[0]["排序"].ToString().Trim();

                    //簽核人員若為第一位，就需新增表單標號至文件內
                    if (osn == "1")
                    {
                        sn = "1";

                        fdb._guid = guid;
                        fdb._檔案類型 = "17";
                        DataTable fdt = fdb.GetMaxOnlyOfficeData();

                        if (fdt.Rows.Count > 0)
                        {
                            File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                                    DateTime.Now + " 附件檔有資料\r\n");

                            string scripUrl = "http://172.20.10.5:54315/Sample/fill.docbuilder";
                            string targetDir = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Oil_Upload\\suggestionimport\\" + guid + "\\";
                            string targetPath = Path.Combine(targetDir, fdt.Rows[0]["新檔名"].ToString().Trim() + fdt.Rows[0]["附檔名"].ToString().Trim());

                            string templateUrl =
                            "http://172.20.10.5:54315/DOWNLOAD.aspx?category=Oil&type=suggestionimport&cpid="
                            + fdt.Rows[0]["業者guid"].ToString().Trim()
                            + "&v=" + fdt.Rows[0]["新檔名"].ToString().Trim() + fdt.Rows[0]["附檔名"].ToString().Trim();

                            string temopUrl = targetDir + "test1.docx";



                            cdb._群組代碼 = "047";
                            cdb._項目代碼 = ftype;
                            DataTable cdt = cdb.GetList();

                            if (cdt.Rows.Count > 0)
                            {
                                ocsdb._類型 = ftype;
                                DataTable oscdtMax = ocsdb.GetMaxSn(oConn, myTrans);
                                if (oscdtMax.Rows.Count > 0) osn = oscdtMax.Rows[0]["Sort"].ToString();
                                else osn = "1";

                                string formNo = cdt.Rows[0]["項目名稱"].ToString().Trim() + "-" + osn;
                                string textToWrite = "表單編號: " + formNo;

                                string scriptWithParams = scripUrl + "?template=" + Uri.EscapeDataString(templateUrl) + "&formno=" + Uri.EscapeDataString(textToWrite);

                                GenerateDocumentWithJwt(temopUrl, textToWrite, fdt.Rows[0]["業者guid"].ToString().Trim());

                                //WriteCcByTag_WithBuilderService(targetPath, "FormNo", textToWrite);

                                File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                                    DateTime.Now + " Document Builder 服務寫入完成 (FormNo='" + textToWrite + "')：" + temopUrl + "\r\n");
                            }


                        }
                    }
                }

                sdb._狀態 = status;
                sdb._內容 = content;
                sdb._年度 = year;
                sdb._排序 = sn;
                sdb._簽核id = LogInfo.mGuid;
                sdb._簽核人員 = LogInfo.name;
                sdb._建立者 = LogInfo.mGuid;
                sdb._建立日期 = DateTime.Now;
                sdb._修改者 = LogInfo.mGuid;
                sdb._修改日期 = DateTime.Now;

                sdb.InsertData(oConn, myTrans);
            }
            //else if (type == "double")
            //{
            //    sdb._guid = guid;
            //    sdb._類型 = "01"; // 簽核表
            //    DataTable sdt = sdb.GetMaxSn();

            //    if (sdt.Rows.Count > 0)
            //    {
            //        File.AppendAllText(Server.MapPath("~/log-callback.txt"),
            //                        DateTime.Now + " " + sdt.Rows[0]["排序"].ToString().Trim() + "\r\n");

            //        osn = sdt.Rows[0]["排序"].ToString().Trim();

            //        //簽核人員若為第一位，就需新增表單標號至文件內
            //        if (osn == "1")
            //        {
            //            sn = "1";

            //            fdb._guid = guid;
            //            fdb._檔案類型 = "17";
            //            DataTable fdt = fdb.GetMaxOnlyOfficeData();

            //            if (fdt.Rows.Count > 0)
            //            {
            //                File.AppendAllText(Server.MapPath("~/log-callback.txt"),
            //                        DateTime.Now + " 附件檔有資料\r\n");

            //                string targetDir = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Oil_Upload\\suggestionimport\\" + guid + "\\";
            //                string targetPath = Path.Combine(targetDir, fdt.Rows[0]["新檔名"].ToString().Trim() + fdt.Rows[0]["附檔名"].ToString().Trim());

            //                File.AppendAllText(Server.MapPath("~/log-callback.txt"),
            //                        DateTime.Now + " " + targetPath + "\r\n");

            //                Document doc = new Document(targetPath);

            //                DocumentBuilder builder = new DocumentBuilder(doc);
            //                if (doc.Range.Bookmarks["FormNumber"] != null)
            //                {
            //                    File.AppendAllText(Server.MapPath("~/log-callback.txt"),
            //                        DateTime.Now + " 有書籤\r\n");

            //                    cdb._群組代碼 = "047";
            //                    cdb._項目代碼 = ftype;
            //                    DataTable cdt = cdb.GetList();

            //                    if (cdt.Rows.Count > 0)
            //                    {
            //                        File.AppendAllText(Server.MapPath("~/log-callback.txt"),
            //                        DateTime.Now + " 代碼檔有資料\r\n");

            //                        ocsdb._類型 = ftype;
            //                        DataTable oscdtMax = ocsdb.GetMaxSn(oConn, myTrans);
            //                        if (oscdtMax.Rows.Count > 0)
            //                        {
            //                            osn = oscdtMax.Rows[0]["Sort"].ToString();
            //                        }
            //                        else
            //                        {
            //                            osn = "1";
            //                        }


            //                        var bk = doc.Range.Bookmarks["FormNumber"];
            //                        if (bk != null)
            //                        {
            //                            string formNo = cdt.Rows[0]["項目名稱"].ToString().Trim() + "-" + osn;
            //                            bk.Text = "表單編號: " + formNo;                  // 取代書籤內既有內容
            //                            doc.Save(targetPath);              // ★ 必須存檔
            //                        }

            //                        File.AppendAllText(Server.MapPath("~/log-callback.txt"),
            //                        DateTime.Now + " " + cdt.Rows[0]["項目名稱"].ToString().Trim() + "-" + osn + "\r\n");

            //                        fdb._guid = guid;
            //                        fdb._排序 = fdt.Rows[0]["排序"].ToString().Trim();
            //                        fdb._業者guid = Guid.NewGuid().ToString("N");
            //                        fdb._修改者 = LogInfo.mGuid;
            //                        fdb.UpdateFileByOnlyOffice();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            #endregion

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response><relogin>N</relogin></root>";

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

    public string GenerateDocumentWithJwt(string templatePath, string textToWrite, string fileKey)
    {
        // ... (日誌路徑和JWT密鑰讀取邏輯保持不變)
        string jwtSecret = ConfigurationManager.AppSettings["JwtSecret"];
        string logPath = HttpContext.Current.Server.MapPath("~/log-callback.txt");

        // 讀取範本檔案
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException("Template file not found. path: " + templatePath, templatePath);
        }
        byte[] templateBytes = File.ReadAllBytes(templatePath);
        string templateBase64 = Convert.ToBase64String(templateBytes);

        // 建立 JWT payload
        // 這部分現在只包含必要的追蹤資訊，不再包含檔案內容
        var jwtPayload = new JwtPayload
    {
        { "url", "http://172.20.10.5:54315/Handler/SaveCallback.aspx" }, // 這個 URL 是 Document Builder 完成後回傳的地址
        { "key", fileKey } // 一個唯一的識別碼，用於追蹤這個文件
    };

        // 建立 JWT token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(new JwtHeader(creds), jwtPayload);
        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // 建立 Document Builder 的腳本
        var script = new Dictionary<string, object>
    {
        { "method", "replaceContentControl" },
        { "arguments", new Dictionary<string, object>
            {
                { "tagName", "FormNo" },
                { "text", "Hello World" }
            }
        }
    };

        // 建立最終的請求 JSON
        // 這裡的 token 是作為一個獨立參數，而不是包含在 payload 裡
        var finalRequestBody = new Dictionary<string, object>
    {
        { "async", false },
        { "fileData", templateBase64 },
        { "script", new List<object> { script } },
        { "token", tokenString }
    };

        string jsonContent = JsonConvert.SerializeObject(finalRequestBody);

        // 在這裡加入日誌
        File.AppendAllText(logPath, DateTime.Now + " 準備發送的 JSON 請求: " + jsonContent + "\r\n");

        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        string docBuilderUrl = "http://172.20.10.5:8080/docbuilder";

        // ... (後續 HttpClient 呼叫邏輯與之前相同)
        using (var http = new HttpClient())
        {
            try
            {
                HttpResponseMessage resp = http.PostAsync(docBuilderUrl, httpContent).Result;
                resp.EnsureSuccessStatusCode();

                string responseContent = resp.Content.ReadAsStringAsync().Result;
                File.AppendAllText(logPath, DateTime.Now + " DocumentBuilder 回傳內容: " + responseContent + "\r\n");

                byte[] fileBytes = resp.Content.ReadAsByteArrayAsync().Result;
                string newFilePath = Path.Combine(Path.GetDirectoryName(templatePath), "generated_document.docx");
                File.WriteAllBytes(newFilePath, fileBytes);
                return newFilePath;
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath, DateTime.Now + " 發生錯誤：" + ex.ToString() + "\r\n");
                throw new Exception("Document Builder request failed", ex);
            }
        }
    }

    //public string GenerateDocumentWithJwt(string templatePath, string textToWrite)
    //{
    //    // 從 Web.config 讀取 JWT Secret
    //    string jwtSecret = ConfigurationManager.AppSettings["JwtSecret"];
    //    if (string.IsNullOrEmpty(jwtSecret))
    //    {
    //        throw new Exception("JWT Secret is not configured in Web.config.");
    //    }

    //    // 將範本文件讀取為 base64 字串
    //    if (!File.Exists(templatePath))
    //    {
    //        throw new FileNotFoundException("Template file not found.path: " + templatePath, templatePath);
    //    }
    //    byte[] templateBytes = File.ReadAllBytes(templatePath);
    //    string templateBase64 = Convert.ToBase64String(templateBytes);

    //    // 建立要傳給 Document Builder 的腳本
    //    var script = new Dictionary<string, object>
    //{
    //    { "method", "replaceContentControl" },
    //    { "arguments", new Dictionary<string, object>
    //        {
    //            { "tagName", "FormNo" }, // 請確認標籤名稱
    //            { "text", textToWrite }
    //        }
    //    }
    //};

    //    // 建立 JWT Payload
    //    // 這部分完全沿用你的寫法，只修改內容
    //    var payload = new JwtPayload
    //{
    //    { "async", false },
    //    { "fileData", templateBase64 },
    //    { "script", new List<object> { script } } // script 參數必須是陣列
    //};

    //    // 簽署 JWT
    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    //    var token = new JwtSecurityToken(new JwtHeader(creds), payload);
    //    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    //    // 建立最終的請求 JSON，並將 token 作為一個屬性加入
    //    var finalRequestBody = new Dictionary<string, object>
    //{
    //    { "async", false },
    //    { "fileData", templateBase64 },
    //    { "script", new List<object> { script } },
    //    { "token", tokenString }
    //};

    //    // 將字典轉換為 JSON 字串
    //    string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(finalRequestBody);
    //    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

    //    // 設定 Document Builder 服務的 URL
    //    string docBuilderUrl = "http://172.20.10.5:8080/docbuilder";

    //    using (var http = new HttpClient())
    //    {
    //        try
    //        {
    //            // 使用 .Result 同步等待，避免 async/await
    //            HttpResponseMessage resp = http.PostAsync(docBuilderUrl, httpContent).Result;
    //            resp.EnsureSuccessStatusCode();

    //            // 成功，將二進位文件內容讀取並返回
    //            byte[] fileBytes = resp.Content.ReadAsByteArrayAsync().Result;

    //            // 這裡可以將檔案寫入或返回給前端
    //            string newFilePath = Path.Combine(Path.GetDirectoryName(templatePath), "generated_document.docx");
    //            File.WriteAllBytes(newFilePath, fileBytes);

    //            return newFilePath; // 返回新檔案的路徑
    //        }
    //        catch (Exception ex)
    //        {
    //            // 這裡可以加入更詳細的錯誤日誌
    //            throw new Exception("Document Builder request failed. Error: {ex.Message}", ex);
    //        }
    //    }
    //}

    public static void WriteCcByTag_WithBuilderService(string inputPath, string tag, string text)
    {
        // 不寫入任何檔案日誌；僅拋出例外供上層處理

        if (!File.Exists(inputPath))
            throw new FileNotFoundException("找不到要處理的檔案", inputPath);

        // === 基本環境參數 ===
        string baseUrl = (ConfigurationManager.AppSettings["BaseUrl"] ?? "").TrimEnd('/') + "/";             // e.g. http://172.20.10.5/
        string virtualDirAlias = (ConfigurationManager.AppSettings["VirtualDirAlias"] ?? "").Trim('/');      // e.g. OilgasSys_Upload
        string virtualDirPhysicalPath = ConfigurationManager.AppSettings["VirtualDirPhysicalPath"] ?? "";    // e.g. D:\WebUpLoad\OilgasSys_Upload\
        bool keepScript = string.Equals(ConfigurationManager.AppSettings["DocBuilderKeepScript"], "true", StringComparison.OrdinalIgnoreCase);

        if (!virtualDirPhysicalPath.EndsWith("\\") && !virtualDirPhysicalPath.EndsWith("/"))
            virtualDirPhysicalPath += "\\";

        if (!inputPath.StartsWith(virtualDirPhysicalPath, StringComparison.OrdinalIgnoreCase))
            throw new Exception("目標檔案不在虛擬目錄底下，無法組成公開 URL。請確認 VirtualDirPhysicalPath 與 inputPath 一致。");

        string fileName = Path.GetFileName(inputPath);
        string targetDir = Path.GetDirectoryName(inputPath);

        // 相對虛擬目錄的目錄（不編碼目錄，僅編碼檔名）
        string directoryPathRelativeToVirtualDir = targetDir
            .Substring(virtualDirPhysicalPath.Length)
            .TrimStart('\\')
            .Replace("\\", "/");

        // 目錄公開 URL（以 / 結尾）
        string dirUrl = baseUrl + virtualDirAlias + "/" + directoryPathRelativeToVirtualDir + "/";

        // 重要：檔名用 URL 編碼，避免中文/空白導致 404
        string inputUrl = dirUrl + fileName;

        // === 動態產生 .docbuilder 腳本（與 docx 同資料夾） ===
        string scriptFileName = "upd_" + Guid.NewGuid().ToString("N") + ".docbuilder";
        string scriptPath = Path.Combine(targetDir, scriptFileName);
        string scriptUrl = dirUrl + scriptFileName;

        File.AppendAllText(HttpContext.Current.Server.MapPath("~/log-callback.txt"),
                            DateTime.Now + " \r\n scriptUrl = " + scriptUrl + "\r\n inputUrl = " + inputUrl + "\r\n");

        string jsScript =
    @"var A = Argument;
builder.OpenFile(A.input);
builder.SaveFile('docx', A.output);
builder.CloseFile();";

        File.WriteAllText(scriptPath, jsScript, Encoding.UTF8);
        if (!File.Exists(scriptPath))
            throw new IOException("無法建立 .docbuilder 腳本檔：" + scriptPath);

        File.AppendAllText(HttpContext.Current.Server.MapPath("~/log-callback.txt"),
                            DateTime.Now + " 成功建立 .docbuilder 腳本檔\r\n");

        // === 組 payload（同步執行） ===
        var serializer = new JavaScriptSerializer();
        var payload = new Dictionary<string, object>
    {
        { "async", false },
        { "url", scriptUrl },
        { "argument", new Dictionary<string, object> {
            { "input",  inputUrl },   // builder.OpenFile 讀取的完整 HTTP URL
            { "output", fileName },   // 回應 urls 的鍵
            { "tag",    tag   ?? "" },
            { "text",   text  ?? "" }
        }}
    };

        string secret = ConfigurationManager.AppSettings["JwtSecret"] ?? "";
        string bodyJson = string.IsNullOrEmpty(secret)
            ? serializer.Serialize(payload)
            : serializer.Serialize(new Dictionary<string, object> { { "token", GenerateOnlyOfficeJwt(payload, secret) } });

        byte[] bodyBytes = Encoding.UTF8.GetBytes(bodyJson);

        // === 呼叫 /docbuilder 端點（小寫） ===
        string builderUrl = ConfigurationManager.AppSettings["OO_DS_BUILDER_URL"] ?? "";   // e.g. http://172.20.10.5:8080/docbuilder
        var request = (HttpWebRequest)WebRequest.Create(builderUrl);
        request.Method = "POST";
        request.ContentType = "application/json; charset=utf-8";
        request.ContentLength = bodyBytes.Length;
        request.Timeout = 120000;

        using (var reqStream = request.GetRequestStream())
            reqStream.Write(bodyBytes, 0, bodyBytes.Length);

        try
        {
            string respText;
            using (var resp = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(resp.GetResponseStream()))
                respText = reader.ReadToEnd();

            var respObj = serializer.DeserializeObject(respText) as IDictionary<string, object>;
            if (respObj == null) throw new Exception("DocBuilder 回傳不是 JSON：" + respText);

            if (respObj.ContainsKey("error"))
            {
                int err = Convert.ToInt32(respObj["error"]);
                if (err == -4)
                    throw new Exception("Document Builder 錯誤 -4：DS 抓不到檔案（.docbuilder 或 .docx）。請確認兩個 URL 對 DS 可達、IIS 已放行 .docbuilder 並啟用匿名存取。");
                if (err != 0)
                    throw new Exception("Document Builder 服務錯誤碼：" + err);
            }

            if (!respObj.ContainsKey("urls"))
                throw new Exception("回應缺少 urls 欄位：" + respText);

            var urls = respObj["urls"] as IDictionary<string, object>;
            if (urls == null || !urls.ContainsKey(fileName))
                throw new Exception("回應未包含預期輸出檔：" + fileName + "；原始回應：" + respText);

            // 下載輸出並覆蓋原檔
            string downloadUrl = urls[fileName].ToString();
            using (var wc = new WebClient())
            {
                var bytes = wc.DownloadData(downloadUrl);
                File.WriteAllBytes(inputPath, bytes);
            }
        }
        catch (WebException ex)
        {
            string errorBody = "";
            if (ex.Response != null)
            {
                using (var s = ex.Response.GetResponseStream())
                using (var r = new StreamReader(s))
                    errorBody = r.ReadToEnd();
            }
            throw new Exception("與 Document Builder 服務連線或處理失敗：" + errorBody, ex);
        }
        finally
        {
            // 視設定保留或刪除 .docbuilder 腳本檔
            if (!keepScript)
            {
                try { if (File.Exists(scriptPath)) File.Delete(scriptPath); } catch { /* ignore */ }
            }
        }
    }

    // ====== 產生 ONLYOFFICE 用 JWT（HS256） ======
    private static string GenerateOnlyOfficeJwt(object payloadObject, string secret)
    {
        var serializer = new JavaScriptSerializer();
        string headerJson = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
        string payloadJson = serializer.Serialize(payloadObject);

        string headerB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
        string payloadB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));
        string toSign = headerB64 + "." + payloadB64;

        using (var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret)))
        {
            byte[] sig = hmac.ComputeHash(Encoding.UTF8.GetBytes(toSign));
            string sigB64 = Base64UrlEncode(sig);
            return toSign + "." + sigB64;
        }
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }





    // 新的非同步方法來發送 HTTP 請求給 Document Builder 服務
    //    public static void WriteCcByTag_WithBuilderService(string inputPath, string tag, string text)
    //    {
    //        // 假設 inputPath 是 "D:\WebUpLoad\OilgasSys_Upload\suggestionimport\...\檔案.docx"
    //        string targetPath = inputPath;

    //        // 取得檔案名稱
    //        string fileName = Path.GetFileName(targetPath);

    //        // 假設你的虛擬目錄實體路徑是 "D:\WebUpLoad\OilgasSys_Upload\"
    //        // 注意：這裡應該使用 ConfigurationManager 來避免硬編碼
    //        string virtualDirPhysicalPath = ConfigurationManager.AppSettings["VirtualDirPhysicalPath"];

    //        // 取得檔案所在的目錄路徑，相對於虛擬目錄
    //        string directoryPathRelativeToVirtualDir = Path.GetDirectoryName(targetPath).Substring(
    //            virtualDirPhysicalPath.Length
    //        ).Replace("\\", "/");

    //        // 這是你的公開 URL 前綴
    //        string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
    //        string virtualDirAlias = ConfigurationManager.AppSettings["VirtualDirAlias"];

    //        // 組合成完整的公開 URL，指向 "目錄"
    //        string fileUrl = baseUrl + virtualDirAlias + "/" + directoryPathRelativeToVirtualDir + "/";

    //        // 1. 組裝 Document Builder 腳本
    //        string jsScript =
    //@"var A = Argument;
    //builder.OpenFile(A.input);
    //var doc = Api.GetDocument();
    //var ccs = doc.GetAllContentControls();
    //for (var i = 0; i < ccs.length; i++) {
    //  var cc = ccs[i];
    //  if (cc.GetTag && cc.GetTag() === A.tag) {
    //    cc.RemoveAllElements();
    //    cc.AddText(A.text);
    //  }
    //}
    //builder.SaveFile('docx', A.output);
    //builder.CloseFile();
    //";

    //        // 2. 組裝要發送的 JSON 請求
    //        // 在方法開始處只需要一個 JavaScriptSerializer 物件
    //        var serializer = new JavaScriptSerializer();
    //        var requestData = new
    //        {
    //            async = false, // 同步處理，等待結果
    //            fileUrl = fileUrl,
    //            token = ConfigurationManager.AppSettings["JwtSecret"],
    //            script = jsScript,
    //            argument = new
    //            {
    //                input = fileName, // 這裡的文件名是 Document Builder 服務內部使用的名稱
    //                output = fileName,
    //                tag = tag,
    //                text = text
    //            }
    //        };

    //        string jsonPayload = serializer.Serialize(requestData);
    //        byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);

    //        // 3. 發送 HTTP POST 請求給 Document Builder 服務
    //        string builderUrl = ConfigurationManager.AppSettings["OO_DS_BUILDER_URL"];
    //        var request = (HttpWebRequest)WebRequest.Create(builderUrl);
    //        request.Method = "POST";
    //        request.ContentType = "application/json; charset=utf-8";
    //        request.ContentLength = byteArray.Length;
    //        request.Timeout = 60000; // 設定超時時間 (以毫秒為單位)
    //        request.Headers["Authorization"] = "Bearer " + ConfigurationManager.AppSettings["JwtSecret"];

    //        // 寫入請求資料 (同步)
    //        using (var dataStream = request.GetRequestStream())
    //        {
    //            dataStream.Write(byteArray, 0, byteArray.Length);
    //        }

    //        // 4. 接收回應並處理
    //        try
    //        {
    //            var response = (HttpWebResponse)request.GetResponse();
    //            using (var responseStream = new StreamReader(response.GetResponseStream()))
    //            {
    //                string responseString = responseStream.ReadToEnd();
    //                // 使用最初定義的 serializer 物件
    //                var responseData = serializer.Deserialize<dynamic>(responseString);

    //                if (responseData["error"] != 0)
    //                {
    //                    throw new Exception("Document Builder 服務處理失敗: " + responseData["message"]);
    //                }

    //                // 為了讓這個靜態方法能夠存取 HttpContext，必須傳入
    //                // 這裡假設你在 Page_Load 呼叫時傳入 HttpContext.Current
    //                File.AppendAllText(HttpContext.Current.Server.MapPath("~/log-callback.txt"),
    //                    DateTime.Now + " Document Builder 服務回傳成功\r\n");
    //            }
    //        }
    //        catch (WebException ex)
    //        {
    //            string responseText = "";
    //            if (ex.Response != null)
    //            {
    //                using (var stream = ex.Response.GetResponseStream())
    //                {
    //                    using (var reader = new StreamReader(stream))
    //                    {
    //                        responseText = reader.ReadToEnd();
    //                    }
    //                }
    //            }
    //            throw new Exception("與 Document Builder 服務連線失敗: " + responseText);
    //        }
    //    }


    // 讀取 appSettings
    //    public static string Cfg(string key)
    //    {
    //        object v = ConfigurationManager.AppSettings[key];
    //        return v == null ? "" : v.ToString();
    //    }

    //    public static string EnsureTmpDir()
    //    {
    //        string dir = Cfg("OO_TMP_DIR");
    //        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
    //        return dir;
    //    }

    //    public static void RunProcess(string fileName, string arguments, out string stdout, out string stderr, int timeoutMs)
    //    {
    //        ProcessStartInfo psi = new ProcessStartInfo();
    //        psi.FileName = fileName;
    //        psi.Arguments = arguments;
    //        psi.UseShellExecute = false;
    //        psi.RedirectStandardOutput = true;
    //        psi.RedirectStandardError = true;
    //        psi.CreateNoWindow = true;

    //        using (Process p = new Process())
    //        {
    //            p.StartInfo = psi;
    //            p.Start();
    //            stdout = p.StandardOutput.ReadToEnd();
    //            stderr = p.StandardError.ReadToEnd();
    //            bool ok = p.WaitForExit(timeoutMs);
    //            if (!ok)
    //            {
    //                try { p.Kill(); } catch { }
    //                throw new Exception("Timeout running: " + fileName + " " + arguments);
    //            }
    //            if (p.ExitCode != 0)
    //                throw new Exception(fileName + " exit " + p.ExitCode + ". " + stderr + " " + stdout);
    //        }
    //    }

    //    // 產生只處理「單一 Tag」的 .docbuilder 腳本（覆蓋內容）
    //    public static string BuildDocBuilderScript_ForSingleCc(string scriptPath)
    //    {
    //        string js =
    //    @"// --argument: { input, output, tag, text }
    //var A = Argument;
    //builder.OpenFile(A.input);
    //var doc = Api.GetDocument();
    //var ccs = doc.GetAllContentControls();
    //for (var i = 0; i < ccs.length; i++) {
    //  var cc = ccs[i];
    //  if (cc.GetTag && cc.GetTag() === A.tag) {
    //    if (cc.RemoveAllElements) cc.RemoveAllElements();
    //    if (cc.AddText) cc.AddText('' + A.text);
    //    else if (cc.SetText) cc.SetText('' + A.text);
    //  }
    //}
    //builder.SaveFile('docx', A.output);
    //builder.CloseFile();
    //";
    //        File.WriteAllText(scriptPath, js, Encoding.UTF8);
    //        return scriptPath;
    //    }

    //    // JSON 參數轉義（避免引號/跳行破壞 --argument）
    //    public static string EscJson(string s)
    //    {
    //        if (s == null) return "";
    //        return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
    //    }

    //    // 用本機 docker 呼叫容器內的 documentbuilder：把 Tag=tag 的 SDT 寫成 text
    //    public static void WriteCcByTag_WithLocalDocker(string containerName, string inputPath, string tag, string text)
    //    {
    //        string tmp = EnsureTmpDir();
    //        string inHost = Path.Combine(tmp, "in_" + Guid.NewGuid().ToString("N") + ".docx");
    //        string outHost = Path.Combine(tmp, "out_" + Guid.NewGuid().ToString("N") + ".docx");
    //        string script = Path.Combine(tmp, "upd_" + Guid.NewGuid().ToString("N") + ".docbuilder");

    //        File.Copy(inputPath, inHost, true);
    //        BuildDocBuilderScript_ForSingleCc(script);

    //        string inCtr = "/tmp/in.docx";
    //        string outCtr = "/tmp/out.docx";
    //        string jsCtr = "/tmp/update.docbuilder";

    //        // 1) docker cp 檔案與腳本到容器
    //        string o, e;
    //        RunProcess("docker", "cp \"" + inHost + "\" " + containerName + ":" + inCtr, out o, out e, 120000);
    //        RunProcess("docker", "cp \"" + script + "\" " + containerName + ":" + jsCtr, out o, out e, 120000);

    //        // 2) 組 --argument JSON
    //        string argJson = "{\"input\":\"" + EscJson(inCtr) + "\",\"output\":\"" + EscJson(outCtr) +
    //                         "\",\"tag\":\"" + EscJson(tag) + "\",\"text\":\"" + EscJson(text) + "\"}";

    //        // 3) 執行 documentbuilder
    //        RunProcess("docker", "exec " + containerName + " documentbuilder \"--argument=" + argJson + "\" \"" + jsCtr + "\"", out o, out e, 180000);

    //        // 4) 拉回結果覆蓋原檔
    //        RunProcess("docker", "cp " + containerName + ":" + outCtr + " \"" + outHost + "\"", out o, out e, 120000);
    //        File.Copy(outHost, inputPath, true);

    //        // 5) 清理暫存
    //        try { File.Delete(inHost); File.Delete(outHost); File.Delete(script); } catch { }
    //    }
}