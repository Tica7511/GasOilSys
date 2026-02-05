using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IO;

public partial class Handler_GetFile : System.Web.UI.Page
{
    FileTable db = new FileTable();
    OilCompanyInfo_DB odb = new OilCompanyInfo_DB();
    GasCompanyInfo_DB gdb = new GasCompanyInfo_DB();
    TrackChangesFileTable tdb = new TrackChangesFileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 查詢附件檔
		///說    明:
		/// * Request["cpid"]: 業者Guid 
        /// * Request["guid"]: 查核建議guid 
		/// * Request["type"]: 附件類別 
		/// * Request["year"]: 年份 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string filetype = (string.IsNullOrEmpty(Request["filetype"])) ? "" : Request["filetype"].ToString().Trim();
            string pagetype = (string.IsNullOrEmpty(Request["pagetype"])) ? "" : Request["pagetype"].ToString().Trim();
            string newGuid = Guid.NewGuid().ToString("N");

            db._guid = guid;
            db._業者guid = cpid;
            db._檔案類型 = type;
            db._年度 = year;

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            if (filetype == "list")
            {
                if(type == "17" || type == "18")
                {
                    db._排序 = "1";
                }

                dt = db.GetList();
                dt2 = db.GetYearList();

                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("業者簡稱", typeof(string));

                    for (int i=0; i < dt.Rows.Count; i++)
                    {
                        if (type == "17")
                        {
                            odb._guid = dt.Rows[i]["業者guid"].ToString().Trim();
                            DataTable odt = odb.GetCpName();

                            if (odt.Rows.Count > 0)
                            {
                                dt.Rows[i]["業者簡稱"] = odt.Rows[0]["cpname"].ToString().Trim();
                            }
                        }
                        else if (type == "18")
                        {
                            gdb._guid = dt.Rows[i]["業者guid"].ToString().Trim();
                            DataTable gdt = gdb.GetCpName();

                            if (gdt.Rows.Count > 0)
                            {
                                dt.Rows[i]["業者簡稱"] = gdt.Rows[0]["cpname"].ToString().Trim();
                            }
                        }
                    }                    
                }

                string xmlstr = string.Empty;
                string xmlstr2 = string.Empty;

                xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList2", "data_item2");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + "</root>";
                xDoc.LoadXml(xmlstr);
            }
            else {                
                string uGuid = string.Empty;
                string fileName = string.Empty;
                string fileNewName = string.Empty;
                string fileExtension = string.Empty;
                string jwtToken = string.Empty;

                if(type == "17" || type == "18")
                {
                    //File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                    //            DateTime.Now + "\r\n guid:" + guid + "\r\n業者guid:" + cpid + "\r\n檔案類型:"
                    //            + type + "\r\n年度:" + year + "\r\n");


                    db._guid = guid;
                    dt = db.GetMaxOnlyOfficeData();

                    if (dt.Rows.Count > 0)
                    {
                        uGuid = dt.Rows[0]["業者guid"].ToString().Trim();
                        fileName = dt.Rows[0]["原檔名"].ToString().Trim();
                        fileNewName = dt.Rows[0]["新檔名"].ToString().Trim();
                        fileExtension = dt.Rows[0]["附檔名"].ToString().Trim();
                        jwtToken = GenerateJwt(guid, uGuid, fileName, fileNewName, fileExtension, pagetype);

                        //db._業者guid = newGuid;
                        //db.UpdateFileByOnlyOffice();
                    }

                    string xmlstr = string.Empty;

                    xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                    xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "<fileName>" + fileName + fileExtension + 
                "</fileName><onlyofficeguid>" + uGuid + "</onlyofficeguid><token>" + jwtToken + "</token><mGuid>" + LogInfo.mGuid 
                + "</mGuid><mName>" + LogInfo.name + "</mName><cGuid>" + guid + "</cGuid></root>";
                    xDoc.LoadXml(xmlstr);
                }
                else
                {
                    if(type == "00")
                    {
                        tdb._guid = guid;
                        dt = tdb.GetList();
                    }
                    else
                    {
                        dt = db.GetData();
                    }


                    string xmlstr = string.Empty;

                    xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
                    xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
                    xDoc.LoadXml(xmlstr);
                }                
            }
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }

    public static string GenerateJwt(string guid, string tmpGuid, string fileName, string fileNewname, string fileextension, string pagetype)
    {
        bool status = true;

        if(pagetype == "view")
        {
            status = false;
        }

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
                    { "url", "http://172.20.10.5:54315/DOWNLOAD.aspx?category=Oil&type=suggestionimport&cpid=" + tmpGuid + "&v=" + fileNewname + fileextension }
                }
            },
            { "documentType", "word" },
            { "editorConfig", new Dictionary<string, object>
                {
                    { "mode", pagetype },
                    { "lang", "zh-TW" },
                    { "callbackUrl", "http://172.20.10.5:54315/Handler/SaveCallback.aspx" },
                    { "canUseHistory", true },
                    { "customization", new Dictionary<string, object>
                        {
                            { "forcesave", true },
                            { "autosave", true },
                            { "autosaveInterval", 60 },
                            //{ "trackChanges", true },
                            { "buttons", new Dictionary<string, object>
                                {
                                    { "print", false },
                                    { "download", false }
                                }
                            },
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
                            },
                            { "features", new Dictionary<string, object>
                                {
                                    { "watermark", false }
                                }
                            }
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
                    { "edit", status },
                    //{ "review", true },
                    { "comment", status },
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