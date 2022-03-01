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

public partial class Handler_DelOilAllSefEvaluationFile : System.Web.UI.Page
{
    FileTable db = new FileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除 石油自評表查核建議附件
		///說    明:
		/// * Request["guid"]: guid
		/// * Request["sn"]: 排序
		/// * Request["cpid"]: 業者guid
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
            string sn = (string.IsNullOrEmpty(Request["sn"])) ? "" : Request["sn"].ToString().Trim();
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"];

            string xmlstr = string.Empty;

            db._guid = guid;
            db._排序 = sn;
            db._業者guid = cpid;
            db._檔案類型 = "06";
            DataTable dt = db.GetFileData();

            if (dt.Rows.Count > 0)
            {
                FileInfo fi = new FileInfo(UpLoadPath + "Oil_Upload\\selfEvaluation\\" + dt.Rows[0]["新檔名"].ToString().Trim() + dt.Rows[0]["附檔名"].ToString().Trim());
                if (fi.Exists)
                {
                    fi.Delete(); //刪除主機路徑內的檔案
                }

                db._修改者 = LogInfo.mGuid;
                db.DelFile2();
            }
            else
            {
                throw new Exception("檔案不存在");
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