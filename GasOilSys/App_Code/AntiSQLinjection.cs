using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Xml;

/// <summary>
/// AntiSQLinjection 的摘要描述
/// </summary>
public class AntiSQLinjection : IHttpModule
{
    public void Dispose()
    {

    }

    public void Init(HttpApplication context)
    {
        //context_AcquireRequestState為委託的方法
        context.BeginRequest += new EventHandler(context_AcquireRequestState);
    }
    

    private void context_AcquireRequestState(object sender, EventArgs e)
    {
        //一定要將sender對象的Context轉化為HttpContext的對象
        //其中包含請求的基本對象，如request,response等
        HttpContext context = ((HttpApplication)sender).Context;
        
        string errorPage = "~/errorPage.aspx";
        string keys = "";
        string values = "";

		/// GET
		if (context.Request.QueryString != null)
        {
            for (int i = 0; i < context.Request.QueryString.Count; i++)
            {
                keys = context.Request.QueryString.Keys[i];
                values = context.Server.UrlDecode(context.Request.QueryString[keys]);

                /// 若有非法字元，Show Error Message
                if (!this.ProcessSqlStrGET(values))
                {
                    context.Response.ContentEncoding = Encoding.GetEncoding("BIG5");
                    context.Response.Write("Error Message：請勿輸入非法字元");
                    context.Response.End();
                    break;
                }
            }
        }

        /// POST
        if (context.Request.Form != null)
        {
            for (int i = 0; i < context.Request.Form.Count; i++)
            {
                keys = context.Request.Form.Keys[i];
                values = context.Server.HtmlDecode(context.Request.Form[i]);
                if (keys == "__VIEWSTATE") continue;
                if (keys == "__EVENTTARGET") continue;
                if (keys == "__EVENTARGUMENT") continue;
                if (keys == "__LASTFOCUS") continue;
                if (keys == "__VIEWSTATEGENERATOR") continue;
                if (keys == "__EVENTVALIDATION") continue;
                if (keys == null) continue;//reportviewer 會有null
                //if (keys.ToLower().IndexOf("desc") !=-1) continue; //排除tineymce插入圖片的路徑被阻擋的問題("=")

                /// 若有非法字元
                if (!this.ProcessSqlStrPOST(values))
                {
                    try
                    {
                        throw new ApplicationException("請勿輸入非法字元");
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }
    }

    
    private bool ProcessSqlStrGET(string str)
    {
        bool bResult = true;
        try
        {
            str = Regex.Replace(str, "[\\s]{1,}", "");    //two or more spaces
            str = Regex.Replace(str, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");    //<br>

            string[] UnSafeArray = new string[27];
            UnSafeArray[0] = "'";
            UnSafeArray[1] = " xp_cmdshell ";
            UnSafeArray[2] = " declare ";
            UnSafeArray[3] = " netlocalgroupadministrators ";
            UnSafeArray[4] = " delete ";
            UnSafeArray[5] = " truncate ";
            UnSafeArray[6] = " netuser ";
            UnSafeArray[7] = " add ";
            UnSafeArray[8] = " drop ";
            UnSafeArray[9] = " update ";
            UnSafeArray[10] = " select ";
            UnSafeArray[11] = " union ";
            UnSafeArray[12] = " exec ";
            UnSafeArray[13] = " create ";
            UnSafeArray[14] = " insertinto ";
            UnSafeArray[15] = "sp_";
            UnSafeArray[16] = " exec ";
            UnSafeArray[17] = " create ";
            UnSafeArray[18] = " masterdbo ";
            UnSafeArray[19] = ";--";
            UnSafeArray[20] = "1=";
            UnSafeArray[21] = " and ";
            UnSafeArray[22] = " alert";
            UnSafeArray[23] = "\"";
            UnSafeArray[24] = "--";
            UnSafeArray[25] = "||";
            UnSafeArray[26] = "eval";

            foreach (string strValue in UnSafeArray)
            {
                if (str.ToLower().IndexOf(strValue) > -1)
                {
                    bResult = false;
                    break;
                }
            }
        }
        catch
        {
            bResult = false;
        }
        return bResult;
    }


    private bool ProcessSqlStrPOST(string str)
    {
        bool bResult = true;
        str = Regex.Replace(str, "[\\s]{1,}", "");    //two or more spaces
        str = Regex.Replace(str, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");    //<br>

        string[] UnSafeArray = new string[26];
        UnSafeArray[0] = " xp_cmdshell ";
        UnSafeArray[1] = " declare ";
        UnSafeArray[2] = " netlocalgroupadministrators ";
        UnSafeArray[3] = " delete ";
        UnSafeArray[4] = " truncate ";
        UnSafeArray[5] = " netuser ";
        UnSafeArray[6] = " add ";
        UnSafeArray[7] = " drop ";
        UnSafeArray[8] = " update ";
        UnSafeArray[9] = " select ";
        UnSafeArray[10] = " union ";
        UnSafeArray[11] = " exec ";
        UnSafeArray[12] = " create ";
        UnSafeArray[13] = " insertinto ";
        UnSafeArray[14] = "sp_";
        UnSafeArray[15] = " exec ";
        UnSafeArray[16] = " create ";
        UnSafeArray[17] = " masterdbo ";
        UnSafeArray[18] = ";--";
        UnSafeArray[19] = "1=";
        UnSafeArray[20] = " and ";
        UnSafeArray[21] = " alert";
        UnSafeArray[22] = "\"";
        UnSafeArray[23] = "--";
        UnSafeArray[24] = "||";
        UnSafeArray[25] = "eval";
							  
        foreach (string strValue in UnSafeArray)
        {
            if (str.ToLower().IndexOf(strValue) > -1)
            {
                bResult = false;
                break;
            }
        }
        return bResult;
    }
}