using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

public partial class WebPage_OilSaveAllSefEvaluation : System.Web.UI.Page
{
    OilAllSuggestion_DB db = new OilAllSuggestion_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 儲存石油自評表查核建議
        ///說    明:
        /// * Request["type"]: add:新增查核建議 edit:編輯查核建議
        /// * Request["guid"]: 查核建議 之 guid
        /// * Request["qid"]: 自評表題目guid
        /// * Request["qOpinions"]: 查核建議委員意見
        /// * Request["qAnswer"]: 查核建議委員答案
        /// * Request["qViewFile"]: 查核建議檢視文件
        /// * Request["qIsop"]: 是否列入查核意見
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
            
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string logguid = (string.IsNullOrEmpty(Request["guid"])) ? Guid.NewGuid().ToString("D").ToUpper() : Request["guid"].ToString().Trim();
            string qid = (string.IsNullOrEmpty(Request["qid"])) ? "" : Request["qid"].ToString().Trim();
            string qOpinions = (string.IsNullOrEmpty(Request["qOpinions"])) ? "" : Request["qOpinions"].ToString().Trim();
            string qAnswer = (string.IsNullOrEmpty(Request["qAnswer"])) ? "" : Request["qAnswer"].ToString().Trim();
            string qViewFile = (string.IsNullOrEmpty(Request["qViewFile"])) ? "" : Request["qViewFile"].ToString().Trim();
            string qIsop = (string.IsNullOrEmpty(Request["qIsop"])) ? "" : Request["qIsop"].ToString().Trim();
            
            db._guid = logguid;
            db._委員guid = LogInfo.mGuid;
            db._委員 = LogInfo.name;
            db._業者guid = cpid;
            db._題目guid = qid;
            db._年度 = taiwanYear();
            db._檢視文件 = qViewFile;
            db._委員意見 = qOpinions;
            db._是否列入查核意見 = qIsop;
            db._建立者 = LogInfo.mGuid;
            db._修改者 = LogInfo.mGuid;

            if (type == "add")
            {
                db.SaveSuggestion(oConn, myTrans);
            }
            else if (type == "edit")
            {
                db.UpdateSuggestion(oConn, myTrans);
            }

            myTrans.Commit();

            string xmlstr = string.Empty;
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response></root>";
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

    public string taiwanYear()
    {
        DateTime nowdate = DateTime.Now;
        string year = nowdate.Year.ToString();
        string taiwanYear = (Convert.ToInt32(year) - 1911).ToString();

        return taiwanYear;
    }
}