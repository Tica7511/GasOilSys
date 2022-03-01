using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

public partial class Handler_GetManager : System.Web.UI.Page
{
    GasAreaSchematicDiagram_DB db = new GasAreaSchematicDiagram_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 管理人員權限
        ///說    明:
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string xmlstr = string.Empty;

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>" + LogInfo.competence + "</Response></root>";
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