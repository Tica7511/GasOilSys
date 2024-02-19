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

public partial class Handler_AddOilCheckSmartTubeCleaner : System.Web.UI.Page
{
    OilCheckSmartTubeCleaner_DB odb = new OilCheckSmartTubeCleaner_DB();
    FileTable fdb = new FileTable();
    CodeTable_DB cdb = new CodeTable_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 智慧型通管器檢查表
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 長途管線識別碼
        /// * Request["txt2"]: 檢測方法 
        /// * Request["txt3_1"]: 最近一次執行 年份 
        /// * Request["txt3_2"]: 最近一次執行 月份
        /// * Request["txt4_1"]: 報告產出 年份
        /// * Request["txt4_2"]:報告產出 月份
        /// * Request["txt5"]: 檢測長度
        /// * Request["txt6"]: 管壁減薄30%-40%數量 內部腐蝕數量
        /// * Request["txt7"]: 管壁減薄30%-40%數量 內部開挖確認數量
        /// * Request["txt8"]: 管壁減薄30%-40%數量 外部腐蝕數量
        /// * Request["txt9"]: 管壁減薄30%-40%數量 外部開挖確認數量
        /// * Request["txt10"]: 管壁減薄40%-50%數量 內部腐蝕數量
        /// * Request["txt11"]: 管壁減薄40%-50%數量 內部開挖確認數量
        /// * Request["txt12"]: 管壁減薄40%-50%數量 外部腐蝕數量
        /// * Request["txt13"]: 管壁減薄40%-50%數量 外部開挖確認數量
        /// * Request["txt14"]: 管壁減薄50%以上數量 內部腐蝕數量
        /// * Request["txt15"]: 管壁減薄50%以上數量 內部開挖確認數量
        /// * Request["txt16"]: 管壁減薄50%以上數量 外部腐蝕數量
        /// * Request["txt17"]: 管壁減薄50%以上數量 外部開挖確認數量
        /// * Request["txt18"]: Dent 變形量>12%數量
        /// * Request["txt19"]: Dent 開挖確認數量
        /// * Request["txt20"]: 備註
        /// * Request["txt21"]: 外部腐蝕保護電位符合標準要求數量
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
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? Guid.NewGuid().ToString("N") : Request["guid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string txt1 = (string.IsNullOrEmpty(Request["txt1"])) ? "" : Request["txt1"].ToString().Trim();
            string txt2 = (string.IsNullOrEmpty(Request["txt2"])) ? "" : Request["txt2"].ToString().Trim();
            string txt3_1 = (string.IsNullOrEmpty(Request["txt3_1"])) ? "" : Request["txt3_1"].ToString().Trim();
            string txt3_2 = (string.IsNullOrEmpty(Request["txt3_2"])) ? "" : Request["txt3_2"].ToString().Trim();
            string txt4_1 = (string.IsNullOrEmpty(Request["txt4_1"])) ? "" : Request["txt4_1"].ToString().Trim();
            string txt4_2 = (string.IsNullOrEmpty(Request["txt4_2"])) ? "" : Request["txt4_2"].ToString().Trim();
            string txt5 = (string.IsNullOrEmpty(Request["txt5"])) ? "" : Request["txt5"].ToString().Trim();
            string txt6 = (string.IsNullOrEmpty(Request["txt6"])) ? "" : Request["txt6"].ToString().Trim();
            string txt7 = (string.IsNullOrEmpty(Request["txt7"])) ? "" : Request["txt7"].ToString().Trim();
            string txt8 = (string.IsNullOrEmpty(Request["txt8"])) ? "" : Request["txt8"].ToString().Trim();
            string txt9 = (string.IsNullOrEmpty(Request["txt9"])) ? "" : Request["txt9"].ToString().Trim();
            string txt10 = (string.IsNullOrEmpty(Request["txt10"])) ? "" : Request["txt10"].ToString().Trim();
            string txt11 = (string.IsNullOrEmpty(Request["txt11"])) ? "" : Request["txt11"].ToString().Trim();
            string txt12 = (string.IsNullOrEmpty(Request["txt12"])) ? "" : Request["txt12"].ToString().Trim();
            string txt13 = (string.IsNullOrEmpty(Request["txt13"])) ? "" : Request["txt13"].ToString().Trim();
            string txt14 = (string.IsNullOrEmpty(Request["txt14"])) ? "" : Request["txt14"].ToString().Trim();
            string txt15 = (string.IsNullOrEmpty(Request["txt15"])) ? "" : Request["txt15"].ToString().Trim();
            string txt16 = (string.IsNullOrEmpty(Request["txt16"])) ? "" : Request["txt16"].ToString().Trim();
            string txt17 = (string.IsNullOrEmpty(Request["txt17"])) ? "" : Request["txt17"].ToString().Trim();
            string txt18 = (string.IsNullOrEmpty(Request["txt18"])) ? "" : Request["txt18"].ToString().Trim();
            string txt19 = (string.IsNullOrEmpty(Request["txt19"])) ? "" : Request["txt19"].ToString().Trim();
            string txt20 = (string.IsNullOrEmpty(Request["txt20"])) ? "" : Request["txt20"].ToString().Trim();
            string txt21 = (string.IsNullOrEmpty(Request["txt21"])) ? "" : Request["txt21"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            odb._guid = guid;
            odb._業者guid = cp;
            odb._年度 = Server.UrlDecode(year);
            odb._長途管線識別碼 = Server.UrlDecode(txt1);
            odb._檢測方法 = Server.UrlDecode(txt2);
            odb._最近一次執行年月 = (string.IsNullOrEmpty(txt3_1) && string.IsNullOrEmpty(txt3_2)) ? "" : Server.UrlDecode(txt3_1) + "/" + Server.UrlDecode(txt3_2);
            odb._報告產出年月 = (string.IsNullOrEmpty(txt4_1) && string.IsNullOrEmpty(txt4_2)) ? "" : Server.UrlDecode(txt4_1) + "/" + Server.UrlDecode(txt4_2);
            odb._檢測長度公里 = Server.UrlDecode(txt5);
            odb._減薄數量_內1 = Server.UrlDecode(txt6);
            odb._減薄數量_內_開挖確認1 = Server.UrlDecode(txt7);
            odb._減薄數量_外 = Server.UrlDecode(txt8);
            odb._減薄數量_外_開挖確認1 = Server.UrlDecode(txt9);
            odb._減薄數量_內2 = Server.UrlDecode(txt10);
            odb._減薄數量_內_開挖確認2 = Server.UrlDecode(txt11);
            odb._減薄數量_外2 = Server.UrlDecode(txt12);
            odb._減薄數量_外_開挖確認2 = Server.UrlDecode(txt13);
            odb._減薄數量_內3 = Server.UrlDecode(txt14);
            odb._減薄數量_內_開挖確認3 = Server.UrlDecode(txt15);
            odb._減薄數量_外3 = Server.UrlDecode(txt16);
            odb._減薄數量_外_開挖確認3 = Server.UrlDecode(txt17);
            odb._Dent = Server.UrlDecode(txt18);
            odb._Dent_開挖確認 = Server.UrlDecode(txt19);
            odb._備註 = Server.UrlDecode(txt20);
            odb._外部腐蝕保護電位符合標準要求數量 = Server.UrlDecode(txt21);
            odb._修改者 = LogInfo.mGuid;
            odb._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                odb._建立者 = LogInfo.mGuid;
                odb._建立日期 = DateTime.Now;

                odb.InsertData(oConn, myTrans);
            }
            else
            {
                odb._guid = guid;
                odb.UpdateData(oConn, myTrans);
            }

            string sn = string.Empty;

            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Oil_Upload\\ILI\\";

                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }

                    cdb._群組代碼 = "021";
                    cdb._項目代碼 = "12";
                    DataTable cdt = cdb.GetList();

                    if (cdt.Rows.Count > 0)
                    {
                        fdb._guid = guid;
                        fdb._業者guid = cp;
                        fdb._年度 = Server.UrlDecode(year);
                        fdb._檔案類型 = "12";

                        if (Server.UrlDecode(mode) == "new")
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
                        string newName = orgName + "_" + guid + "_" + sn;

                        string file_size = File.ContentLength.ToString();

                        File.SaveAs(UpLoadPath + newName + extension);

                        #region 儲存檔案進附件檔

                        fdb._guid = guid;
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