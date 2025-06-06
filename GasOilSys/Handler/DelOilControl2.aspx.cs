﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Handler_DelOilControl2 : System.Web.UI.Page
{
    OilControl_DB db = new OilControl_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
		///功    能: 刪除 控制室_管線輸送/接收資料
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

            string xmlstr = string.Empty;
            db._guid = guid;
            db._修改者 = LogInfo.mGuid;
            db.DeleteData2();

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