using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Handler_DelGasInfoState : System.Web.UI.Page
{
    GasInfo_DB db = new GasInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除 事業單位基本資料場站中心
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
            string cpid = string.Empty;
            string cType = string.Empty;
            string sn = string.Empty;
            string xmlstr = string.Empty;

            db._guid = guid;
            db._年度 = "110";
            db._修改者 = LogInfo.mGuid;
            DataTable dt = db.GetDataGuid();

            if (dt.Rows.Count > 0)
            {
                db._業者guid = dt.Rows[0]["業者guid"].ToString().Trim();
                db._場站類別 = dt.Rows[0]["場站類別"].ToString().Trim();
                sn = dt.Rows[0]["排序"].ToString().Trim();

                DataTable dt2 = db.GetData();

                for(int i=(Convert.ToInt32(sn)+1); i<= dt2.Rows.Count; i++)
                {
                    db._中心名稱 = dt2.Rows[i-1]["中心名稱"].ToString().Trim();
                    db.UpdateDataSn((i - 1).ToString());
                }
            }
            
            db.DeleteData();

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