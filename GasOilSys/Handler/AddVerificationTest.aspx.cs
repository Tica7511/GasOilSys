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

public partial class Handler_AddVerificationTest : System.Web.UI.Page
{
    VerificationTest_DB db = new VerificationTest_DB();
    FileTable fdb = new FileTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增/修改 查核與檢測資料
        ///說    明:
        /// * Request["cp"]:  業者guid
        /// * Request["guid"]: guid
        /// * Request["isCheck"]: 是否為查核報告
        /// * Request["type"]: 類別
        /// * Request["objectName"]: 對象名稱
        /// * Request["timeBegin"]:  查核日期(起)
        /// * Request["timeEnd"]:  查核日期(迄)
        /// * Request["session"]:  場次 
        /// * Request["situation"]:  改善情形 
        /// * Request["mode"]:  new=新增 edit=編輯
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
            string guid = (string.IsNullOrEmpty(Request["guid"])) ? Guid.NewGuid().ToString("D").ToUpper() : Request["guid"].ToString().Trim();
            string year = (string.IsNullOrEmpty(Request["year"])) ? "" : Request["year"].ToString().Trim();
            string isCheck = (string.IsNullOrEmpty(Request["isCheck"])) ? "" : Request["isCheck"].ToString().Trim();
            string type = (string.IsNullOrEmpty(Request["type"])) ? "" : Request["type"].ToString().Trim();
            string objectName = (string.IsNullOrEmpty(Request["objectName"])) ? "" : Request["objectName"].ToString().Trim();
            string timeBegin = (string.IsNullOrEmpty(Request["timeBegin"])) ? "" : Request["timeBegin"].ToString().Trim();
            string timeEnd = (string.IsNullOrEmpty(Request["timeEnd"])) ? "" : Request["timeEnd"].ToString().Trim();
            string session = (string.IsNullOrEmpty(Request["session"])) ? "" : Request["session"].ToString().Trim();
            //string situation = (string.IsNullOrEmpty(Request["situation"])) ? "" : Request["situation"].ToString().Trim();
            string mode = (string.IsNullOrEmpty(Request["mode"])) ? "" : Request["mode"].ToString().Trim();
            string sn = string.Empty;
            string xmlstr = string.Empty;
            DataTable dt = new DataTable();

            db._業者guid = Server.UrlDecode(cp);
            db._年度 = Server.UrlDecode(year);
            db._類別 = Server.UrlDecode(type);
            db._場次 = Server.UrlDecode(session);
            //db._改善情形 = Server.UrlDecode(situation);
            db._報告編號 = Server.UrlDecode(timeBegin.Substring(0, 3)) + "-" + Server.UrlDecode(type) + "-" + Server.UrlDecode(session);
            db._對象 = Server.UrlDecode(objectName);
            db._查核日期起 = Server.UrlDecode(timeBegin);
            db._查核日期迄 = Server.UrlDecode(timeEnd);
            db._修改者 = LogInfo.mGuid;
            db._修改日期 = DateTime.Now;

            if (Server.UrlDecode(mode) == "new")
            {
                dt = db.GetSession(oConn, myTrans);
                if (dt.Rows.Count > 0)
                {
                    throw new Exception("場次不可重複請重新填寫");
                }

                db._guid = guid;
                db._建立者 = LogInfo.mGuid;
                db._建立日期 = DateTime.Now;

                db.InsertData(oConn, myTrans);
            }
            else
            {
                db._guid = guid;
                dt = db.GetUpdateSession(oConn, myTrans);
                if (dt.Rows.Count > 0)
                {
                    if (Server.UrlDecode(session) != dt.Rows[0]["場次"].ToString().Trim())
                    {
                        DataTable sdt = db.GetSession(oConn, myTrans);

                        if (sdt.Rows.Count > 0)
                        {
                            throw new Exception("場次不可重複請重新填寫");
                        }
                    }

                    string[] reportNoAr = dt.Rows[0]["報告編號"].ToString().Trim().Split('-');
                    db._報告編號 = reportNoAr[0] + "-" + reportNoAr[1] + "-" + Server.UrlDecode(session);
                }                
                 
                db.UpdateData(oConn, myTrans);
            }

            string typeName = string.Empty;

           //檔案上傳
           HttpFileCollection uploadFiles = Request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile File = uploadFiles[i];
                if (File.FileName.Trim() != "")
                {
                    string UpLoadPath = ConfigurationManager.AppSettings["UploadFileRootDir"] + "VerificationTest\\";
                    fdb._guid = guid;
                    fdb._業者guid = cp;
                    fdb._年度 = Server.UrlDecode(year);

                    if (!string.IsNullOrEmpty(Server.UrlDecode(isCheck)) && i == 0)
                    {
                        if (i == 0)
                        {
                            UpLoadPath += "Check\\";
                            fdb._檔案類型 = "10";
                            typeName = "10";
                            sn = "01";
                        }
                    }
                    else
                    {
                        UpLoadPath += "Relation\\";
                        fdb._檔案類型 = "11";
                        typeName = "11";

                        if (Server.UrlDecode(mode) == "new")
                        {
                            if ((i == 1) && (Server.UrlDecode(isCheck) == "Y"))
                            {
                                sn = "0" + i.ToString();
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
                                    sn = "0" + i.ToString();
                                }
                            }
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
                    }

                    //如果上傳路徑中沒有該目錄，則自動新增
                    if (!Directory.Exists(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(UpLoadPath.Substring(0, UpLoadPath.LastIndexOf("\\")));
                    }

                    //原檔名
                    string orgName = Path.GetFileNameWithoutExtension(File.FileName);

                    //副檔名
                    string extension = System.IO.Path.GetExtension(File.FileName).ToLower();

                    //新檔名
                    //string newName = Server.UrlDecode(taiwanYear()) + "_" + cpName + LogInfo.name + "_" + typeName + sn;
                    string newName = orgName + "_" + guid + "_" + typeName + "_" + sn;

                    string file_size = File.ContentLength.ToString();

                    File.SaveAs(UpLoadPath + newName + extension);

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
                }
            }

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>儲存完成</Response><vguid>" + guid + "</vguid></root>";

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