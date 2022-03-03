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

public partial class Handler_GasSaveAllSefEvaluation : System.Web.UI.Page
{
    GasAllSuggestion_DB db = new GasAllSuggestion_DB();
    FileTable fdb = new FileTable();
    CodeTable_DB cdb = new CodeTable_DB();
    GasCompanyInfo_DB cpdb = new GasCompanyInfo_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 儲存天然氣自評表查核建議
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
            string cpName = string.Empty;

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

            cpdb._guid = cpid;
            DataTable cpdt = cpdb.GetCpName2();

            if (cpdt.Rows.Count > 0)
            {
                cpName = cpdt.Rows[0]["cpname"].ToString().Trim() + "_";
            }

            string sn = string.Empty;

            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Gas_Upload\\selfEvaluation\\";

                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }

                    cdb._群組代碼 = "021";
                    cdb._項目代碼 = "07";
                    DataTable cdt = cdb.GetList();

                    if (cdt.Rows.Count > 0)
                    {
                        fdb._guid = logguid;
                        fdb._業者guid = cpid;
                        fdb._年度 = Server.UrlDecode(taiwanYear());
                        fdb._檔案類型 = "07";

                        if (type == "add")
                        {
                            sn = "0" + (i + 1).ToString();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(sn))
                            {
                                if (0 < Convert.ToInt32(sn) && Convert.ToInt32(sn) < 9)
                                {
                                    sn = "0" + (Convert.ToInt32(sn) + 1).ToString();
                                }
                                else
                                {
                                    sn = (Convert.ToInt32(sn) + 1).ToString();
                                }
                            }
                            else
                            {
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
                            }
                        }

                        string typeName = cdt.Rows[0]["項目名稱"].ToString().Trim() + "_";

                        //原檔名
                        string orgName = Path.GetFileNameWithoutExtension(File.FileName);

                        //副檔名
                        string extension = System.IO.Path.GetExtension(File.FileName).ToLower();

                        //新檔名
                        //string newName = Server.UrlDecode(taiwanYear()) + "_" + cpName + LogInfo.name + "_" + typeName + sn;
                        string newName = orgName;

                        string file_size = File.ContentLength.ToString();

                        File.SaveAs(UpLoadPath + newName + extension);

                        #region 儲存檔案進附件檔

                        fdb._guid = logguid;
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
                    }
                }
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