using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Handler_TriggerForceSave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string docKey = Request.QueryString["key"]; // 你也可以直接寫死
        if (string.IsNullOrEmpty(docKey))
        {
            docKey = Request.Form["key"];
        }

        string json = "{\"c\":\"forcesave\", \"key\":\"" + docKey + "\"}";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://172.20.10.5:8080/coauthoring/CommandService.ashx");
        request.Method = "POST";
        request.ContentType = "application/json";

        // 若有 JWT token 要加上
        request.Headers.Add("Authorization", "Bearer 你的 token");

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
        }

        try
        {
            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var result = reader.ReadToEnd();

                File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                DateTime.Now + "\nstatus=自動儲存成功" + "\nresult=" + result + "\n\n");

                Response.Write("成功回傳: " + result);

            }
        }
        catch (WebException ex)
        {
            using (var reader = new StreamReader(ex.Response.GetResponseStream()))
            {
                var result = reader.ReadToEnd();

                File.AppendAllText(Server.MapPath("~/log-callback.txt"),
                DateTime.Now + "\nstatus=自動儲存失敗" + "\nresult=" + result + "\n\n");

                Response.StatusCode = 500;
                Response.Write("發生錯誤：" + result);
            }
        }
    }
}