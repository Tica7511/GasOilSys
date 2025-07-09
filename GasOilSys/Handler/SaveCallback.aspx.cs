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
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Ionic.Zip;
using System.Workflow.Activities;
using NPOI.POIFS.FileSystem;
using System.Xml.Linq;
using System.Text;

public partial class Handler_SaveCallback : System.Web.UI.Page
{
    TrackChangesFileTable db = new TrackChangesFileTable();
    FileTable fdb = new FileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 儲存 onlyoffice
        ///說    明:
        ///-----------------------------------------------------

        /// Transaction
        SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        oConn.Open();
        SqlCommand oCmmd = new SqlCommand();
        oCmmd.Connection = oConn;
        SqlTransaction myTrans = oConn.BeginTransaction();
        oCmmd.Transaction = myTrans;

        try
        {
            string json;
            string fileGuid = string.Empty;
            string PublicFileName = string.Empty;
            string PublicFileNewName = string.Empty;
            string PublicFileExtension = string.Empty;
            string PublicFileFullName = string.Empty;
            string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }

            JObject data = JsonConvert.DeserializeObject<JObject>(json);

            int status = Convert.ToInt32(data["status"]);
            string key = "";
            if (data["key"] != null)
                key = data["key"].ToString();

            string fileName = "";

            fdb._業者guid = key;
            DataTable fdt = fdb.GetOnlyOfficeData();

            if (fdt.Rows.Count > 0)
            {
                fileGuid = fdt.Rows[0]["guid"].ToString().Trim();
                fileName = fdt.Rows[0]["新檔名"].ToString().Trim() + fdt.Rows[0]["附檔名"].ToString().Trim();
                PublicFileName = fdt.Rows[0]["原檔名"].ToString().Trim();
                PublicFileNewName = fdt.Rows[0]["新檔名"].ToString().Trim();
                PublicFileExtension = fdt.Rows[0]["附檔名"].ToString().Trim();
            }

            string fileUrl = "";
            if (data["url"] != null)
                fileUrl = data["url"].ToString();

            string changesurl = "";
            if (data["changesurl"] != null)
                changesurl = data["changesurl"].ToString();

            string editorUser = "unknown";
            if (data["users"] != null && data["users"].Type == JTokenType.Array && data["users"].HasValues)
            {
                editorUser = data["users"][0].ToString();
            }

            key = Guid.NewGuid().ToString("N");

            File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                DateTime.Now + "\nstatus=" + status.ToString() + "\nkey=" + key + "\nfileUrl=" + fileUrl + "\nfileName=" + fileName + 
                "\neditorUser=" + LogInfo.mGuid + "\nchange=" + changesurl + "\n\n");

            if ((status == 2 || status == 6) && !string.IsNullOrEmpty(fileUrl))
            {
                using (var client = new WebClient())
                {
                    string filepath = UpLoadPath + "Oil_Upload\\suggestionimport\\" + fileGuid + "\\" + fileName;
                    byte[] fileBytes = client.DownloadData(fileUrl);


                    string documentXml;
                    using (var zipStream = new MemoryStream(fileBytes))
                    using (var zip = ZipFile.Read(zipStream))
                    {
                        ZipEntry docEntry = null;
                        foreach (ZipEntry e1 in zip.Entries)
                        {
                            if (e1.FileName.Equals("word/document.xml", StringComparison.OrdinalIgnoreCase))
                            {
                                docEntry = e1;
                                break;
                            }
                        }
                        if (docEntry == null)
                        {
                            File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                                DateTime.Now + " 找不到 word/document.xml\r\n");
                            return;
                        }

                        using (var ms2 = new MemoryStream())
                        {
                            docEntry.Extract(ms2);
                            ms2.Position = 0;
                            using (var rdr = new StreamReader(ms2, Encoding.UTF8))
                                documentXml = rdr.ReadToEnd();
                        }
                    }

                    // 解析 document.xml，找出所有 <w:sdt>（Content Control）
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(documentXml);

                    var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    nsmgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

                    // 選所有 content control 節點
                    XmlNodeList sdtList = xmlDoc.SelectNodes("//w:sdt", nsmgr);
                    File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                        DateTime.Now + " 找到 ContentControl 總數=" + sdtList.Count + "\r\n");

                    // 每個 sdt 裡面：
                    foreach (XmlNode sdt in sdtList)
                    {
                        File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                            DateTime.Now + "進入xmlNode迴圈\n");

                        // 1. 取出 <w:tag w:val="YOUR_TAG"/>
                        XmlNode tagNode = sdt.SelectSingleNode(".//w:sdtPr/w:tag", nsmgr);
                        if (tagNode == null || tagNode.Attributes["w:val"] == null)
                            continue;
                        string tag = tagNode.Attributes["w:val"].Value;

                        File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                            DateTime.Now + "有進入下一段\n");

                        // 2. 取出所有 <w:t> 文字
                        StringBuilder sb = new StringBuilder();
                        XmlNodeList textNodes = sdt.SelectNodes(".//w:t", nsmgr);
                        foreach (XmlNode t in textNodes)
                            sb.Append(t.InnerText);
                        string value = sb.ToString();

                        File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                            DateTime.Now + "ContentControl \nTag=\n" + tag + "\nValue=" + value);

                        // 3. 存到資料庫

                        db._guid = key;
                        db._父層guid = fileGuid;
                        db._狀態 = status.ToString();
                        db._檔案類型 = "ContentControl";
                        db._檔案名稱 = tag;
                        db._內容 = fileBytes;
                        db._文字 = value;
                        db._路徑 = changesurl;
                        db._建立者 = editorUser;
                        db._修改者 = editorUser;
                        db.UpdateFile_Trans(oConn, myTrans);
                    }


                    //db._guid = key;
                    //db._父層guid = fileGuid;
                    //db._狀態 = status.ToString();
                    //db._檔案類型 = type;
                    //db._檔案名稱 = fileName;
                    //db._內容 = zipBytes;
                    //db._文字 = text;
                    //db._路徑 = changesurl;
                    //db._建立者 = editorUser;
                    //db._修改者 = editorUser;
                    //db.UpdateFile_Trans(oConn, myTrans);

                    //if (!string.IsNullOrEmpty(changesurl))
                    //{
                    //    string diffZipUrl = data["changesurl"].ToString();
                    //    byte[] zipBytes = client.DownloadData(diffZipUrl);

                    //    using (var zipStream = new MemoryStream(zipBytes))
                    //    using (var zip = ZipFile.Read(zipStream))
                    //    {
                    //        File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + "\n" + " Outer ZIP entries=" + zip.Entries.Count + "\n");

                    //        foreach (var z in zip.Entries)
                    //            File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + "\n" + z.FileName + "\n");

                    //        var jsonEntries = new List<ZipEntry>();
                    //        foreach (ZipEntry z in zip.Entries)
                    //        {
                    //            if (z.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    //            {
                    //                jsonEntries.Add(z);
                    //            }
                    //        }

                    //        if (jsonEntries.Count == 0)
                    //        {
                    //            File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + "\n" + "沒有找到任何 .json 檔 \n");
                    //        }
                    //        else
                    //        {
                    //            foreach (ZipEntry entry2 in jsonEntries)
                    //            {
                    //                File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + "開始解析" + entry2.FileName + "\n");

                    //                using (var ms = new MemoryStream())
                    //                {
                    //                    entry2.Extract(ms);
                    //                    ms.Position = 0;

                    //                    string diffJson;
                    //                    using (var reader = new StreamReader(ms))
                    //                        diffJson = reader.ReadToEnd();

                    //                    JToken rootToken = JToken.Parse(diffJson);

                    //                    JArray changeEvents = null;
                    //                    if (rootToken.Type == JTokenType.Array)
                    //                    {
                    //                        changeEvents = (JArray)rootToken;
                    //                    }
                    //                    else if (rootToken.Type == JTokenType.Object)
                    //                    {
                    //                        // 2. 否則就是個 JObject，才去找 history.changes 或 changeshistory.changes
                    //                        JObject diffObj = (JObject)rootToken;

                    //                        // 嘗試從 history.changes 取
                    //                        JToken historyToken = diffObj["history"];
                    //                        if (historyToken != null)
                    //                        {
                    //                            JToken changesToken = historyToken["changes"];
                    //                            if (changesToken != null && changesToken.Type == JTokenType.Array)
                    //                                changeEvents = (JArray)changesToken;
                    //                        }

                    //                        // 如果還沒拿到，再從 changeshistory.changes 取
                    //                        if (changeEvents == null)
                    //                        {
                    //                            JToken chHistToken = diffObj["changeshistory"];
                    //                            if (chHistToken != null)
                    //                            {
                    //                                JToken changesToken2 = chHistToken["changes"];
                    //                                if (changesToken2 != null && changesToken2.Type == JTokenType.Array)
                    //                                    changeEvents = (JArray)changesToken2;
                    //                            }
                    //                        }
                    //                    }

                    //                    File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + "解析到 changeEvents count="
                    //                    + (changeEvents != null ? changeEvents.Count : 0) + "\n");

                    //                    if (changeEvents != null)
                    //                    {
                    //                        foreach (JToken evtToken in changeEvents)
                    //                        {
                    //                            if (evtToken.Type != JTokenType.Object)
                    //                                continue;

                    //                            JObject evt = (JObject)evtToken;

                    //                            JArray items = evt["changes"] as JArray;
                    //                            if (items == null)
                    //                                continue;

                    //                            File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + "解析到 changes="
                    //                    + items.Count + "\n");

                    //                            foreach (JToken token in items)
                    //                            {
                    //                                if (token.Type != JTokenType.Object)
                    //                                    continue;

                    //                                JObject item = (JObject)token;

                    //                                string type = item["type"] != null ? item["type"].ToString() : "";
                    //                                string what = item["what"] != null ? item["what"].ToString() : "";
                    //                                string text = item["text"] != null ? item["text"].ToString() : "";
                    //                                string dataId = item["dataId"] != null ? item["dataId"].ToString() : "";

                    //                                db._guid = key;
                    //                                db._父層guid = fileGuid;
                    //                                db._狀態 = status.ToString();
                    //                                db._檔案類型 = type;
                    //                                db._檔案名稱 = fileName;
                    //                                db._內容 = zipBytes;
                    //                                db._文字 = text;
                    //                                db._路徑 = changesurl;
                    //                                db._建立者 = editorUser;
                    //                                db._修改者 = editorUser;
                    //                                db.UpdateFile_Trans(oConn, myTrans);

                    //                                File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + " 儲存入追蹤變更了\n\n");
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }                                    
                    //        }
                    //    }
                    //}

                    string sn = string.Empty;

                    fdb._guid = fileGuid;
                    DataTable dtmaxsn = fdb.GetMaxSn(oConn, myTrans);

                    if (dtmaxsn.Rows.Count > 0) { 
                        sn = dtmaxsn.Rows[0]["Sort"].ToString().Trim();
                    }

                    PublicFileFullName = PublicFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    fdb._guid = fileGuid;
                    fdb._年度 = "114";
                    fdb._業者guid = key;
                    fdb._檔案類型 = "17";
                    fdb._原檔名 = PublicFileName;
                    fdb._新檔名 = PublicFileFullName;
                    fdb._附檔名 = PublicFileExtension;
                    fdb._排序 = sn;
                    fdb._檔案大小 = "";
                    fdb._修改者 = editorUser;
                    fdb._修改日期 = DateTime.Now;
                    fdb._建立者 = editorUser;
                    fdb._建立日期 = DateTime.Now;
                    fdb.UpdateFile_Trans(oConn, myTrans);

                    // 2025.06.03 暫時註解追蹤修訂的同意否決log
                    //if (data["changeshistory"] != null && data["changeshistory"]["changes"] != null)
                    //{
                    //    File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + " 有追蹤變更\n\n");

                    //    JArray changes = data["changeshistory"]["changes"] as JArray;
                    //    if (changes != null)
                    //    {
                    //        foreach (JObject changeItem in changes)
                    //        {
                    //            string actionType = changeItem["type"] != null ? changeItem["type"].ToString() : null; // accepted / rejected
                    //            string reviewUserId = null;
                    //            string reviewUserName = null;

                    //            if (changeItem["user"] != null)
                    //            {
                    //                if (changeItem["user"]["id"] != null)
                    //                    reviewUserId = changeItem["user"]["id"].ToString();
                    //                if (changeItem["user"]["name"] != null)
                    //                    reviewUserName = changeItem["user"]["name"].ToString();
                    //            }

                    //            JObject change = changeItem["change"] as JObject;
                    //            if (change != null)
                    //            {
                    //                string changeId = change["id"] != null ? change["id"].ToString() : null;
                    //                string changeType = change["type"] != null ? change["type"].ToString() : null;
                    //                string changeText = change["text"] != null ? change["text"].ToString() : null;

                    //                db._guid = key;
                    //                db._狀態 = status.ToString();
                    //                db._檔案類型 = changeType;
                    //                db._檔案名稱 = changeText;
                    //                db._內容 = fileBytes;
                    //                db._建立者 = changeId;
                    //                db._修改者 = changeId;
                    //                db.UpdateFile_Trans(oConn, myTrans);
                    //            }                                

                    //            File.WriteAllBytes(filepath, fileBytes);

                    //            File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + " 儲存了追蹤變更\n\n");
                    //        }
                    //    }
                    //}

                    //File.WriteAllBytes(filepath, fileBytes);

                    filepath = UpLoadPath + "Oil_Upload\\suggestionimport\\" + fileGuid + "\\" + PublicFileFullName + PublicFileExtension;

                    File.WriteAllBytes(filepath, fileBytes);

                    File.AppendAllText(Server.MapPath("~/log-callback.txt"),DateTime.Now + " 儲存進資料夾了\n\n");
                }

                myTrans.Commit();

                File.AppendAllText(Server.MapPath("~/log-callback.txt"), DateTime.Now + " 儲存進資料庫了\n\n");
            }

            File.WriteAllText(Server.MapPath("~/log-fulljson.txt"), json);

            // 回應 OnlyOffice
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write("{\"error\":0}");
            Context.ApplicationInstance.CompleteRequest(); // 安全結束請求
        }
        catch (Exception ex)
        {
            myTrans.Rollback();

            File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                DateTime.Now + " 錯誤：" + ex.Message + "\n" + ex.StackTrace + "\n\n");

            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write("{\"error\":1}");
            Context.ApplicationInstance.CompleteRequest(); // 不讓 OnlyOffice 掛掉
        }
        finally
        {
            oCmmd.Connection.Close();
            oConn.Close();
        }
    }
}