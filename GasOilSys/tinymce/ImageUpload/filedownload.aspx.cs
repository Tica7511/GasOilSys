using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

public partial class tinymce_ImageUpload_filedownload : System.Web.UI.Page
{
	string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
    protected void Page_Load(object sender, EventArgs e)
	{
        try
        {
			string FileName = (Request.QueryString["v"] != null) ? Common.FilterCheckMarxString(Request.QueryString["v"].ToString().Trim()) : "";
			string category = (Request.QueryString["category"] != null) ? Common.FilterCheckMarxString(Request.QueryString["category"].ToString().Trim()) : "";
			string type = (Request.QueryString["type"] != null) ? Common.FilterCheckMarxString(Request.QueryString["type"].ToString().Trim()) : "";
			string cpName = (Request.QueryString["cpName"] != null) ? Common.FilterCheckMarxString(Request.QueryString["cpName"].ToString().Trim()) : "";

            UpLoadPath = UpLoadPath + category + "\\" + type + "\\" + cpName + "\\";

            #region 檢查 SQL Injection
            if (!CheckSQLInjection(FileName))
                throw new Exception("請勿輸入非法字元");
            #endregion
			
			// 附件目錄
			DirectoryInfo dir = new DirectoryInfo(UpLoadPath);

			//列舉全部檔案再比對檔名
			FileInfo file = dir.EnumerateFiles().FirstOrDefault(m => m.Name == FileName);

			string strContentType = string.Empty;
            switch (file.Extension)
            {
                case ".asf":
                    strContentType = "video/x-ms-asf";
                    break;
                case ".avi":
                    strContentType = "video/avi";
                    break;
                case ".doc":
                    strContentType = "application/msword";
                    break;
                case ".zip":
                    strContentType = "application/zip";
                    break;
                case ".xls":
                    strContentType = "application/vnd.ms-excel";
                    break;
                case ".csv":
                    strContentType = "application/vnd.ms-excel";
                    break;
                case ".gif":
                    strContentType = "image/gif";
                    break;
                case ".jpg":
                    strContentType = "image/jpeg";
                    break;
                case "jpeg":
                    strContentType = "image/jpeg";
                    break;
                case ".wav":
                    strContentType = "audio/wav";
                    break;
                case ".mp3":
                    strContentType = "audio/mpeg3";
                    break;
                case ".mpg":
                    strContentType = "video/mpeg";
                    break;
                case "mpeg":
                    strContentType = "video/mpeg";
                    break;
                case ".htm":
                    strContentType = "text/html";
                    break;
                case ".html":
                    strContentType = "text/html";
                    break;
                case ".asp":
                    strContentType = "text/asp";
                    break;

                default:
                    strContentType = "application/octet-stream";
                    break;
            }

            if (file.Exists)
            {
                Response.ContentType = strContentType;
                string strDownloadName = HttpUtility.UrlEncode(Request.QueryString["v"]);
                Response.AddHeader("content-disposition", "attachment;filename=" + strDownloadName);
                Response.WriteFile(file.FullName);
                Response.Flush();
                Response.End();
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.Message);
            Response.End();
        }
    }

    public static bool CheckSQLInjection(string checkValue)
    {
        //「%27」:「'」(單引號)
        //「%2B」:「+」(加號)
        //「%3D」:「=」(等號)
        //「%7C」:「|」(｜)
        //「ALERT(」
        //「--」
        //「%2F*」:「/*」
        //「*%2F」:「*/」
        //「%26」:「&」
        //「%40」:「@」
        //「%25」:「%」
        //「%3B」:「;」
        //「%24」:「$」
        //「%26」:「*」
        //「%22」:「"」
        //「%2C」:「,」
        //「%2f」:「/」
        //「%5c」:「\」
        //「%22」:「"」
        //「%3C」:「<」
        //「%3E」:「>」

        if (checkValue.Length > 0 && (checkValue.ToUpper().IndexOf("%27") >= 0 || checkValue.ToUpper().IndexOf("%2B") >= 0
          || checkValue.ToUpper().IndexOf("'") >= 0) || checkValue.ToUpper().IndexOf("ALERT(") >= 0
          || checkValue.ToUpper().IndexOf("%3C") >= 0 || checkValue.ToUpper().IndexOf("%3E") >= 0
          || checkValue.ToUpper().IndexOf("%3D") >= 0 || checkValue.ToUpper().IndexOf("=") >= 0
          || checkValue.ToUpper().IndexOf("--") >= 0 || checkValue.ToUpper().IndexOf("%7C") >= 0
          || checkValue.ToUpper().IndexOf("%2F*") >= 0 || checkValue.ToUpper().IndexOf("*%2F") >= 0
          || checkValue.ToUpper().IndexOf("%26") >= 0
          || checkValue.ToUpper().IndexOf("%25") >= 0 || checkValue.ToUpper().IndexOf("%3B") >= 0
          || checkValue.ToUpper().IndexOf("%24") >= 0 || checkValue.ToUpper().IndexOf("*") >= 0
          || checkValue.ToUpper().IndexOf("%22") >= 0 || checkValue.ToUpper().IndexOf("%2C") >= 0
          || checkValue.ToUpper().IndexOf("%2F") >= 0 || checkValue.ToUpper().IndexOf("%5C") >= 0
          || checkValue.ToUpper().IndexOf("%40") >= 0
          || checkValue.ToUpper().IndexOf("../") >= 0 || checkValue.ToUpper().IndexOf("%") >= 0 || checkValue.ToUpper().IndexOf("@") >= 0
          || checkValue.ToUpper().IndexOf("&") >= 0 || checkValue.ToUpper().IndexOf("..\\") >= 0 || checkValue.ToUpper().IndexOf("$") >= 0
          || checkValue.ToUpper().IndexOf("?") >= 0
          )
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}