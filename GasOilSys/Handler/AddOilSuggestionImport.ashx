<%@ WebHandler Language="C#" Class="AddOilSuggestionImport" %>

using System;
using System.Web;
using System.Configuration;
using System.Net;
using System.Data;
using System.IO;
using System.Linq;
using Ionic.Zip;
using System.Collections.Generic;


public class AddOilSuggestionImport : IHttpHandler
{
    string OrgName = string.Empty;
    string NewName = string.Empty;
    bool isWord = false;
    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
    FileTable Fdb = new FileTable();

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Clear();
        context.Response.Buffer = false;

        string fileName = context.Request["v"]; // 檔案名稱
        string filePath = Path.Combine(UpLoadPath + "storageinspect\\", fileName); // 調整為你實際的檔案路徑

        if (!File.Exists(filePath))
        {
            context.Response.StatusCode = 404;
            context.Response.Write("File not found");
            return;
        }

        FileInfo file = new FileInfo(filePath);

        // 設定 Content-Type（以 docx 為例）
        context.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        // 編碼檔名，處理中文檔名
        string rawFileName = file.Name;
        string encodedFileName = Uri.EscapeDataString(rawFileName);

        // 同時提供 filename 與 filename*（支援 OnlyOffice 與瀏覽器下載）
        context.Response.AddHeader("Content-Disposition",
            string.Format("inline; filename=\"{0}\"; filename*=UTF-8''{1}", rawFileName, encodedFileName));

        context.Response.AddHeader("Content-Length", file.Length.ToString());

        // CORS 跨域支援（OnlyOffice 必須）
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");

        context.Response.WriteFile(file.FullName);
        context.Response.Flush();
        context.Response.End();
    }

    public bool IsReusable
    {
        get { return false; }
    }
}