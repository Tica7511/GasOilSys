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

public partial class Handler_DelVerificationTest : System.Web.UI.Page
{
    VerificationTest_DB db = new VerificationTest_DB();
    FileTable fdb = new FileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除 查核與檢測資料
		///說    明:
		/// * Request["guid"]: guid
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();
        try
        {
            #region 檢查登入資訊
            if (LogInfo.account == "")
            {
                throw new Exception("請重新登入");
            }
            #endregion

            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];
            string xmlstr = string.Empty;

            db._guid = guid;
            db._修改者 = LogInfo.mGuid;
            db.DeleteData();
            DataTable dt = fdb.GetFileData();

            if (dt.Rows.Count > 0)
            {
                //for(int i = 0; i < dt.Rows.Count; i++)
                //{
                //    string details = dt.Rows[i]["檔案類型"].ToString().Trim();
                //    switch (details)
                //    {
                //        case "10":
                //            UpLoadPath += "VerificationTest\\Check\\" + dt.Rows[i]["新檔名"].ToString().Trim() + dt.Rows[i]["附檔名"].ToString().Trim();
                //            break;
                //        case "11":
                //            UpLoadPath += "VerificationTest\\Relation\\" + dt.Rows[i]["新檔名"].ToString().Trim() + dt.Rows[i]["附檔名"].ToString().Trim();
                //            break;
                //    }

                //    FileInfo fi = new FileInfo(UpLoadPath);
                //    if (fi.Exists)
                //    {
                //        fi.Delete(); //刪除主機路徑內的檔案
                //    }
                //}

                fdb.DelFileFine(); //更新資料庫此資料列
            }

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>刪除成功</Response></root>";
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