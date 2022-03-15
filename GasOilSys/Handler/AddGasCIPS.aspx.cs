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

public partial class Handler_AddGasCIPS : System.Web.UI.Page
{
    GasCIPS_DB gdb = new GasCIPS_DB();
    FileTable fdb = new FileTable();
    CodeTable_DB cdb = new CodeTable_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 緊密電位檢測
        ///說    明:
        /// * Request["cp"]: 業者guid
        /// * Request["guid"]: guid
        /// * Request["year"]: 年度
        /// * Request["txt1"]: 長途管線識別碼
        /// * Request["txt2"]: 同時檢測管線數量 
        /// * Request["txt3_1"]: 最近一次執行 年份 
        /// * Request["txt3_2"]: 最近一次執行 月份
        /// * Request["txt4_1"]: 報告產出 年份
        /// * Request["txt4_2"]:報告產出 月份
        /// * Request["txt5"]: 檢測長度
        /// * Request["txt6"]: 合格標準	
        /// * Request["txt7"]: 立即改善 數量
        /// * Request["txt8"]: 立即改善 改善完成數量
        /// * Request["txt9"]: 排程改善 數量
        /// * Request["txt10"]: 排程改善 改善完成數量
        /// * Request["txt11"]: 需監控點 數量
        /// * Request["txt12"]: x座標 
        /// * Request["txt13"]: y座標 
        /// * Request["txt14"]: 備註 
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
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string xmlstr = string.Empty;

            gdb._業者guid = cp;
            gdb._年度 = Server.UrlDecode(year);
            gdb._長途管線識別碼 = Server.UrlDecode(txt1);
            gdb._同時檢測管線數量 = Server.UrlDecode(txt2);
            gdb._最近一次執行年月 = Server.UrlDecode(txt3_1) + "/" + Server.UrlDecode(txt3_2);
            gdb._報告產出年月 = Server.UrlDecode(txt4_1) + "/" + Server.UrlDecode(txt4_2);
            gdb._檢測長度 = Server.UrlDecode(txt5);
            gdb._合格標準請參照填表說明2 = Server.UrlDecode(txt6);
            gdb._立即改善_數量 = Server.UrlDecode(txt7);
            gdb._立即改善_改善完成數量 = Server.UrlDecode(txt8);
            gdb._排程改善_數量 = Server.UrlDecode(txt9);
            gdb._排程改善_改善完成數量 = Server.UrlDecode(txt10);
            gdb._需監控點_數量 = Server.UrlDecode(txt11);
            gdb._x座標 = Server.UrlDecode(txt12);
            gdb._y座標 = Server.UrlDecode(txt13);
            gdb._備註 = Server.UrlDecode(txt14);
            gdb._修改者 = LogInfo.mGuid;
            gdb._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                gdb._建立者 = LogInfo.mGuid;
                gdb._建立日期 = DateTime.Now;

                gdb.InsertData(oConn, myTrans);
            }
            else
            {
                gdb._guid = guid;
                gdb.UpdateData(oConn, myTrans);
            }

            string sn = string.Empty;

            HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "Gas_Upload\\CIPS\\";

                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }

                    cdb._群組代碼 = "021";
                    cdb._項目代碼 = "09";
                    DataTable cdt = cdb.GetList();

                    if (cdt.Rows.Count > 0)
                    {
                        fdb._guid = guid;
                        fdb._業者guid = cp;
                        fdb._年度 = Server.UrlDecode(year);
                        fdb._檔案類型 = "09";

                        if (Server.UrlDecode(mode) == "add")
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