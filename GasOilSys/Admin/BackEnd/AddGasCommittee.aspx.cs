using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Admin_BackEnd_AddGasCommittee : System.Web.UI.Page
{
    GasMasterCompare_DB db = new GasMasterCompare_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 委員對業者列表儲存
        ///說    明:
        /// * Request["xStr"]: XML Data
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();

        /// Transaction
        SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        oConn.Open();
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = oConn;
        SqlTransaction myTrans = oConn.BeginTransaction();
        oCmd.Transaction = myTrans;
        try
        {
            #region 檢查登入資訊
            if (LogInfo.mGuid == "")
            {
                throw new Exception("請重新登入");
            }
            #endregion

            string xStr = (string.IsNullOrEmpty(Request["xStr"])) ? "" : Server.UrlDecode(Request["xStr"].ToString().Trim());

            string xmlstr = string.Empty;
            XmlDocument rootDoc = new XmlDocument();
            rootDoc.LoadXml(xStr);
            XmlNodeList xNodeList = rootDoc.SelectNodes("result/item");
            DataTable dt = new DataTable();
            if (xNodeList.Count > 0)
            {
                for (int i = 0; i < xNodeList.Count; i++)
                {
                    db._委員guid = xNodeList[i].Attributes["cguid"].InnerText;
                    db._年度 = xNodeList[i].Attributes["year"].InnerText;
                    db._業者guid = xNodeList[i].Attributes["cpguid"].InnerText;
                    db._委員姓名 = xNodeList[i].Attributes["cname"].InnerText;
                    dt = db.GetCommitteeData(oConn, myTrans);
                    if (dt.Rows.Count > 0)
                        throw new Exception("已重複委員: " + xNodeList[i].Attributes["cname"].InnerText);

                    db._建立者= LogInfo.mGuid;
                    db._修改者 = LogInfo.mGuid;
                    db.InsertData(oConn, myTrans);
                }
            }

            myTrans.Commit();

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
            oCmd.Connection.Close();
            oConn.Close();
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}