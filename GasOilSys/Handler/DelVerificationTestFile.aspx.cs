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

public partial class Handler_DelVerificationTestFile : System.Web.UI.Page
{
    FileTable db = new FileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除 查核與檢測資料報告
		///說    明:
		/// * Request["guid"]: guid 
		/// * Request["type"]: 檔案類型 
		/// * Request["sn"]: 排序 
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
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
            string sn = (string.IsNullOrEmpty(Request["sn"])) ? "" : Request["sn"].ToString().Trim();
            string xmlstr = string.Empty;
            string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];

            db._guid = guid;
            db._檔案類型 = type;
            db._排序 = sn;
            db._修改者 = LogInfo.mGuid;

            DataTable dt = db.GetFileData();
            if (dt.Rows.Count > 0)
            {
                string details = dt.Rows[0]["檔案類型"].ToString().Trim();
                switch (details)
                {
                    case "10":
                        UpLoadPath += "VerificationTest\\Check\\" + dt.Rows[0]["新檔名"].ToString().Trim() + dt.Rows[0]["附檔名"].ToString().Trim();
                        break;
                    case "11":
                        UpLoadPath += "VerificationTest\\Relation\\" + dt.Rows[0]["新檔名"].ToString().Trim() + dt.Rows[0]["附檔名"].ToString().Trim();
                        break;
                }
                FileInfo fi = new FileInfo(UpLoadPath);
                if (fi.Exists)
                {
                    fi.Delete(); //刪除主機路徑內的檔案
                }

                db.DelFileFine(); //更新資料庫此資料列
            }
            else
            {
                throw new Exception("檔案不存在");
            }

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>刪除完成</Response></root>";
            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}