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
using System.Web.Script.Serialization;
using System.Security.Policy;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Protocols.WSTrust;

public partial class Handler_DocumentHistoryHandler : System.Web.UI.Page
{
    FileTable db = new FileTable();
    Member_DB mdb = new Member_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // 確保響應從這裡開始是乾淨的
            Response.Clear(); // 清除所有之前可能緩衝的內容
            Response.BufferOutput = true; // 確保輸出會被緩衝，在 CompleteRequest 前完成

            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";

            string maxSn = string.Empty;
            string userName = string.Empty;
            string type = Request.QueryString["type"];
            string guid = Request.QueryString["guid"];
            string version = Request.QueryString["version"];

            DataTable dt = new DataTable(); 

            if(type == "01")
            {
                db._guid = guid;
                dt = db.GetData();
                var historyList = new List<object>();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string sGuid = dt.Rows[i]["guid"].ToString().Trim();
                        string mGuid = dt.Rows[i]["建立者"].ToString().Trim();
                        int sn = Convert.ToInt32(dt.Rows[i]["排序"].ToString().Trim());
                        string key = "ver" + sn + "-" + sGuid;
                        string createDate = dt.Rows[i]["上傳日期"].ToString().Trim();

                        mdb._guid = dt.Rows[i]["建立者"].ToString().Trim();
                        DataTable mdt = mdb.GetData();

                        if (mdt.Rows.Count > 0)
                        {
                            userName = mdt.Rows[0]["姓名"].ToString().Trim();
                        }

                        var item = new
                        {
                            created = createDate,
                            key = key,
                            user = new { id = mGuid, name = userName },
                            version = sn
                        };

                        historyList.Add(item);
                    }
                }

                dt.Clear();
                db._guid = guid;
                dt = db.GetMaxOnlyOfficeData();

                if (dt.Rows.Count > 0)
                {
                    maxSn = dt.Rows[0]["排序"].ToString().Trim();
                }

                var resultList = new
                {
                    currentVersion = Convert.ToInt32(maxSn),
                    history = historyList.ToArray()
                };

                // 序列化為 JSON
                string json = new JavaScriptSerializer().Serialize(resultList);
                Response.Write(json);
            }
            else if (type == "02")
            {
                var historyList = new List<object>();

                //將 JWT secret 包成 HmacSha256 的加密格式
                var secret = ConfigurationManager.AppSettings["JwtSecret"];
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                db._guid = guid;
                db._排序 = version;
                dt = db.GetData();

                if (dt.Rows.Count > 0)
                {
                    string sGuid = dt.Rows[0]["guid"].ToString().Trim();
                    string pGuid = dt.Rows[0]["業者guid"].ToString().Trim();
                    string fileNewName = dt.Rows[0]["新檔名"].ToString().Trim() + dt.Rows[0]["附檔名"].ToString().Trim();
                    string key = "ver" + version + "-" + sGuid;
                    string createDate = dt.Rows[0]["上傳日期"].ToString().Trim();

                    var payload = new JwtPayload
                    {
                        { "document", new Dictionary<string, object>
                            {
                                { "key", key },
                                { "url", "http://172.20.10.5:54315/DOWNLOAD.aspx?category=Oil&type=suggestionimport&cpid=" + pGuid + "&v=" + fileNewName }
                            }
                        },
                        { "editorConfig", new Dictionary<string, object>
                            {
                                { "callbackUrl", "http://172.20.10.5:54315/Handler/SaveCallback.aspx" }
                            }
                        }
                    };

                    var jwt = new JwtSecurityToken(new JwtHeader(creds), payload);
                    string jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);

                    var item = new
                    {
                        fileType = "docx",
                        key = key,
                        token = jwtString,
                        url = "http://172.20.10.5:54315/DOWNLOAD.aspx?category=Oil&type=suggestionimport&cpid=" + pGuid + "&v=" + fileNewName,
                        version = version
                    };

                    historyList.Add(item);

                }

                var singleItem = historyList[0];

                // 序列化為 JSON
                string json = new JavaScriptSerializer().Serialize(singleItem);
                Response.Write(json);
            }
            else
            {
                try
                {
                    db._guid = guid;
                    db._排序 = version;
                    dt = db.GetData();

                    if (dt.Rows.Count > 0)
                    {
                        string newGuid = Guid.NewGuid().ToString("N");
                        string pGuid = dt.Rows[0]["業者guid"].ToString().Trim();
                        string fileName = dt.Rows[0]["新檔名"].ToString().Trim();
                        string fileOrgName = dt.Rows[0]["原檔名"].ToString().Trim();
                        string fileNewName = fileOrgName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"); 
                        string fileExtension = dt.Rows[0]["附檔名"].ToString().Trim();
                        string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
                        string sourcePath = UpLoadPath + "Oil_Upload\\suggestionimport\\" + guid + "\\" + fileName + fileExtension;

                        if (!File.Exists(sourcePath))
                        {
                            Response.Write("{\"success\":false,\"message\":\"找不到歷史檔案\"}");
                            return;
                        }

                        File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                DateTime.Now + "\nguid=" + guid + "\nsourcePath=" + sourcePath + "\nversion=" + version);

                        string newPath = UpLoadPath + "Oil_Upload\\suggestionimport\\" + guid + "\\" + fileNewName + fileExtension;
                        File.Copy(sourcePath, newPath);

                        string sn = string.Empty;

                        db._guid = guid;
                        DataTable dtmaxsn = db.GetOnlyOfficeFileMax();

                        if (dtmaxsn.Rows.Count > 0)
                        {
                            sn = dtmaxsn.Rows[0]["Sort"].ToString().Trim();
                        }

                        db._guid = guid;
                        db._年度 = "114";
                        db._業者guid = newGuid;
                        db._檔案類型 = "17";
                        db._原檔名 = fileOrgName;
                        db._新檔名 = fileNewName;
                        db._附檔名 = fileExtension;
                        db._排序 = sn;
                        db._檔案大小 = "";
                        db._修改者 = LogInfo.mGuid;
                        db._修改日期 = DateTime.Now;
                        db._建立者 = LogInfo.mGuid;
                        db._建立日期 = DateTime.Now;
                        db.InsertFile();

                        Response.ContentType = "application/json";
                        Response.Write("{\"success\":true,\"newguid\":\"" + newGuid + "\"}");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("{\"success\":false,\"message\":\"" + ex.Message + "\"}");
                }
            }

            // 使用 CompleteRequest 結束請求，比 Response.End() 更優雅
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex) 
        {
            File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                DateTime.Now + " 錯誤：" + ex.Message + "\n" + ex.StackTrace + "\n\n");

            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write("{\"error\":1}");
            Context.ApplicationInstance.CompleteRequest(); // 不讓 OnlyOffice 掛掉
        }

        
    }
}