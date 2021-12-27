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

public partial class Handler_OilDelLogSefEvaluation : System.Web.UI.Page
{
    OilCommitteeSuggestion_DB cs_db = new OilCommitteeSuggestion_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 刪除石油自評表意見log
        ///說    明:
        /// * Request["cpid"]: 業者guid
        /// * Request["qguid"]: 題目guid
        /// * Request["guid"]: 自評表意見log guid
        ///-----------------------------------------------------

        XmlDocument xDoc = new XmlDocument();

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

            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? "" : Request["cpid"].ToString().Trim();
            string qguid = (string.IsNullOrEmpty(Request["qguid"])) ? "" : Request["qguid"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string strIsNull = string.Empty;

            cs_db._guid = guid;
            cs_db._業者guid = cpid;
            cs_db._題目guid = qguid;
            cs_db._年度 = taiwanYear();
            cs_db._修改者 = LogInfo.mGuid;

            cs_db.UpdateSelfEvaluation(oConn, myTrans);
            DataTable dt = cs_db.DelSuggestion(oConn, myTrans);

            myTrans.Commit();

            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
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