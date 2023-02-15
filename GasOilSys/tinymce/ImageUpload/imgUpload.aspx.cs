using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Data;

public partial class tinymce_ImageUpload_imgUpload : System.Web.UI.Page
{
	OilCompanyInfo_DB ocdb = new OilCompanyInfo_DB();
	GasCompanyInfo_DB gcdb = new GasCompanyInfo_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		XmlDocument xDoc = new XmlDocument();
		try
        {
            string ImgUpLoadPath = string.Empty;
            if (Request.Files.Count > 0)
            {
				string category = string.IsNullOrEmpty(Request["category"]) ? "" : Common.FilterCheckMarxString(Request["category"].ToString().Trim());
				string type = string.IsNullOrEmpty(Request["type"]) ? "" : Common.FilterCheckMarxString(Request["type"].ToString().Trim());
				string cpName = string.IsNullOrEmpty(Request["cpName"]) ? "" : Common.FilterCheckMarxString(Request["cpName"].ToString().Trim());
				string cpguid = string.IsNullOrEmpty(Request["cpguid"]) ? "" : Common.FilterCheckMarxString(Request["cpguid"].ToString().Trim());
				string typeName = string.Empty;

				DataTable dt = new DataTable();

				if(category == "Oil_Upload")
                {
					ocdb._guid = cpguid;
					dt = ocdb.GetCpName2();

					if (dt.Rows.Count > 0)
					{
						if (cpguid == "FA8387C6-5860-40DB-A260-3B6C08413C59")
							cpName = dt.Rows[0]["公司名稱"].ToString().Trim();
						else
							cpName = dt.Rows[0]["cpname"].ToString().Trim();
					}
				}
                else
                {
					gcdb._guid = cpguid;
					dt = gcdb.GetCpName2();

					if (dt.Rows.Count > 0)
					{
						if (cpguid == "9E779E2B-C36D-44BF-BED2-11C29D989D53")
							cpName = dt.Rows[0]["公司名稱"].ToString().Trim();
						else
							cpName = dt.Rows[0]["cpname"].ToString().Trim();
					}
				}

                switch (type)
                {
					case "reservoir":
						typeName = "儲槽配置圖";
						break;
					case "arealongpipeline":
						typeName = "轄區長途管線方塊圖";
						break;
					case "longpipeline":
						typeName = "長途管線路徑圖";
						break;
					case "station":
						typeName = "場站概況分佈圖";
						break;
				}

				//圖片路徑
				ImgUpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + category + "\\"+ type + "\\" + cpName + "\\";

                HttpFileCollection files = Request.Files;
                HttpPostedFile afile = files[0];
                
                //如果上傳路徑中沒有該目錄，則自動新增
                if (!Directory.Exists(ImgUpLoadPath.Substring(0, ImgUpLoadPath.LastIndexOf("\\"))))
                {
                    Directory.CreateDirectory(ImgUpLoadPath.Substring(0, ImgUpLoadPath.LastIndexOf("\\")));
                }

				string fileNo = string.Empty;
				int fileCount = Directory.GetFiles(ImgUpLoadPath.Substring(0, ImgUpLoadPath.LastIndexOf("\\"))).Length;

				if (fileCount == 0)
					fileNo = "01";
				else
					if(fileCount >= 9)
						fileNo = (fileCount + 1).ToString();
				    else
						fileNo = "0" + (fileCount + 1).ToString();


				//副檔名
				string extension = System.IO.Path.GetExtension(afile.FileName);
				//產生新檔名
				string newName = cpName + "_" + typeName + "_" + fileNo + extension;

				//驗證副檔名
				if (extension.ToLower() != ".jpg" && extension.ToLower() != ".jpeg" && extension.ToLower() != ".png" && extension.ToLower() != ".bmp" && extension.ToLower() != ".gif")
					throw new Exception("圖片格式限制: .jpg .jpeg .png .bmp .gif");

                //上傳檔案
                afile.SaveAs(ImgUpLoadPath + newName);

                string ReturnPath = Request.Url.Scheme + "://" + Request.Url.Authority.ToString() + Request.ApplicationPath + "/tinymce/ImageUpload/filedownload.aspx?v=" + newName;

				string xmlstr = string.Empty;
				xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>" + ReturnPath + "</Response><category>" + category + 
					"</category><type>" + type + "</type><cpName>" + cpName + "</cpName></root>";
				xDoc.LoadXml(xmlstr);
			}
        }
		catch (Exception ex)
		{
			xDoc = GetExceptionDocument(ex);
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}

	public static XmlDocument GetExceptionDocument(Exception err)
	{
		///---------------------------------------------------------------------------
		/// 作    者: Nick
		/// 功    能: 把應用程式錯誤訊息封裝為 XmlDocument
		/// 說    明: 
		///---------------------------------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
		xDoc.LoadXml(@"<?xml version='1.0' encoding='utf-8'?><root />");
		XmlElement xNode = xDoc.CreateElement("Error");
		xNode.SetAttribute("Message", (string.IsNullOrWhiteSpace(err.Message) ? "" : err.Message.Trim().Replace("'", "")));
		xDoc.DocumentElement.AppendChild(xNode);
		return xDoc;
	}

	public string taiwanYear()
	{
		DateTime nowdate = DateTime.Now;
		string year = nowdate.Year.ToString();
		string taiwanYear = (Convert.ToInt32(year) - 1911).ToString();

		return taiwanYear;
	}
}