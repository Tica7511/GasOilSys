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

public partial class Handler_AddOilInsideInspect : System.Web.UI.Page
{
    OilInsideInspect_DB odb = new OilInsideInspect_DB();
    FileTable fdb = new FileTable();
    CodeTable_DB cdb = new CodeTable_DB();
    OilCompanyInfo_DB cpdb = new OilCompanyInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 管線內部稽核表
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 內部稽核日期
        /// * Request["txt2"]: 執行單位
        /// * Request["txt3"]: 稽核範圍
        /// * Request["txt4"]: 缺失改善執行狀況	
        /// * Request["txt5"]: 佐證資料	
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

            string cp = (string.IsNullOrEmpty(Request["cp"])) ? "" : Request["cp"].ToString().Trim();
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? "" : Request["guid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string txt2 = (string.IsNullOrEmpty(Request["txt2"])) ? "" : Request["txt2"].ToString().Trim();
            string txt3 = (string.IsNullOrEmpty(Request["txt3"])) ? "" : Request["txt3"].ToString().Trim();
            string txt4 = (string.IsNullOrEmpty(Request["txt4"])) ? "" : Request["txt4"].ToString().Trim();
            string txt5 = (string.IsNullOrEmpty(Request["txt5"])) ? "" : Request["txt5"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string tmpGuid = (Server.UrlDecode(mode) == "new") ? Guid.NewGuid().ToString("N") : guid;
            string cpName = string.Empty;
            string xmlstr = string.Empty;
            DataTable dt = new DataTable();

            odb._guid = tmpGuid;
            odb._業者guid = cp;
            odb._年度 = Server.UrlDecode(year);
            odb._日期 = Server.UrlDecode(txt1);
            odb._執行單位 = Server.UrlDecode(txt2);
            odb._稽核範圍 = Server.UrlDecode(txt3);
            odb._缺失改善執行狀況 = Server.UrlDecode(txt4);
            odb._佐證資料 = Server.UrlDecode(txt5);
            odb._修改者 = LogInfo.mGuid;
            odb._修改日期 = DateTime.Now;

            #region 儲存管線內部稽核表 資料

            if (Server.UrlDecode(mode) == "new")
            {
                odb._建立者 = LogInfo.mGuid;
                odb._建立日期 = DateTime.Now;

                odb.InsertData(oConn, myTrans);
            }
            else
            {
                odb.UpdateData(oConn, myTrans);
            }

            #endregion

            cpdb._guid = cp;
            DataTable cpdt = cpdb.GetCpName2();

            if (cpdt.Rows.Count > 0)
            {
                cpName = cpdt.Rows[0]["cpname"].ToString().Trim() + "_";
            }

            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Oil_Upload\\pipeinspect\\";

                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }

                    cdb._群組代碼 = "021";
                    cdb._項目代碼 = "05";
                    DataTable cdt = cdb.GetList();

                    if (cdt.Rows.Count > 0)
                    {
                        string sn = string.Empty;
                        fdb._業者guid = cp;
                        fdb._年度 = Server.UrlDecode(year);
                        fdb._檔案類型 = "05";

                        DataTable fdt = fdb.GetMaxSn();

                        if (fdt.Rows.Count > 0)
                        {
                            int maxsn = Convert.ToInt32(fdt.Rows[0]["Sort"].ToString().Trim());
                            if (maxsn > 9)
                                sn = maxsn.ToString();
                            else
                                sn = "0" + maxsn.ToString();
                        }
                        else
                        {
                            sn = "01";
                        }

                        string typeName = cdt.Rows[0]["項目名稱"].ToString().Trim() + "_";

                        //原檔名
                        string orgName = Path.GetFileNameWithoutExtension(File.FileName);

                        //副檔名
                        string extension = System.IO.Path.GetExtension(File.FileName).ToLower();

                        //新檔名
                        string newName = Server.UrlDecode(year) + "_" + cpName + typeName + sn;

                        string file_size = File.ContentLength.ToString();

                        File.SaveAs(UpLoadPath + newName + extension);

                        #region 儲存檔案進附件檔

                        fdb._guid = tmpGuid;
                        fdb._原檔名 = orgName;
                        fdb._新檔名 = newName;
                        fdb._附檔名 = extension;
                        fdb._排序 = sn;
                        fdb._檔案大小 = file_size;
                        fdb._修改者 = LogInfo.mGuid;
                        fdb._修改日期 = DateTime.Now;
                        fdb._建立者 = LogInfo.mGuid;
                        fdb._建立日期 = DateTime.Now;

                        fdb.UpdateFile_Trans(oConn, myTrans);

                        #endregion

                        #region 儲存檔案進管線內部稽核表

                        odb._佐證資料檔名 = orgName;
                        odb._佐證資料副檔名 = extension;
                        odb._新檔名 = newName;
                        odb._佐證資料路徑 = UpLoadPath;
                        odb._建立者 = LogInfo.mGuid;
                        odb._修改者 = LogInfo.mGuid;

                        odb.SaveFile(oConn, myTrans);

                        #endregion
                    }
                }
            }

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response><relogin>N</relogin></root>";

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
}