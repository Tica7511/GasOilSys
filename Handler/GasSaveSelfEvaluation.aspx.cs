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

public partial class Handler_GasSaveSelfEvaluation : System.Web.UI.Page
{
	GasSelfEvaluation_DB q_db = new GasSelfEvaluation_DB();
	GasEvaluationAnswer_DB ans_db = new GasEvaluationAnswer_DB();
    GasCommitteeSuggestion_DB cs_db = new GasCommitteeSuggestion_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 儲存天然氣自評表答案
		///說    明:
		/// * Request["cpid"]: 業者Guid		
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

            string cpid = (string.IsNullOrEmpty(Request["cpid"])) ? LogInfo.companyGuid : Request["cpid"].ToString().Trim();            

            DataTable qdt = q_db.GetQuestionGuid();
			if (qdt.Rows.Count > 0)
			{
				for (int i = 0; i < qdt.Rows.Count; i++)
				{
					string cAns = (string.IsNullOrEmpty(Request["cg_" + qdt.Rows[i]["天然氣自評表題目guid"].ToString()])) ? "" : Request["cg_" + qdt.Rows[i]["天然氣自評表題目guid"].ToString()].ToString().Trim();
					string mAns = (string.IsNullOrEmpty(Request["mg_" + qdt.Rows[i]["天然氣自評表題目guid"].ToString()])) ? "" : Request["mg_" + qdt.Rows[i]["天然氣自評表題目guid"].ToString()].ToString().Trim();
					string PsStr = (string.IsNullOrEmpty(Request["ps_" + qdt.Rows[i]["天然氣自評表題目guid"].ToString()])) ? "" : Request["ps_" + qdt.Rows[i]["天然氣自評表題目guid"].ToString()].ToString().Trim();
					string vfStr = (string.IsNullOrEmpty(Request["fv_" + qdt.Rows[i]["天然氣自評表題目guid"].ToString()])) ? "" : Request["fv_" + qdt.Rows[i]["天然氣自評表題目guid"].ToString()].ToString().Trim();

					if (LogInfo.competence == "02") // 業者
					{
                        if (cAns != "")
                        {
                            #region 自評表答案
                            ans_db._業者guid = cpid;
                            ans_db._答案 = cAns;
                            ans_db._委員意見 = "";

                            ans_db._題目guid = qdt.Rows[i]["天然氣自評表題目guid"].ToString();
                            ans_db._年度 = qdt.Rows[i]["天然氣自評表題目年份"].ToString();
                            ans_db._填寫人員類別 = LogInfo.competence;
                            ans_db._建立者 = LogInfo.mGuid;
                            ans_db._修改者 = LogInfo.mGuid;
                            #endregion

                            ans_db.SaveAnswer(oConn, myTrans);
                        }                        
                    }
					else if(LogInfo.competence == "01") //委員
                    {
                        if (mAns != "")
                        {
                            #region 自評表答案
                            ans_db._業者guid = cpid;
                            ans_db._答案 = mAns;
                            ans_db._檢視文件 = vfStr;
                            string pStr = PsStr;
                            ans_db._委員意見 = pStr;


                            ans_db._題目guid = qdt.Rows[i]["天然氣自評表題目guid"].ToString();
                            ans_db._年度 = qdt.Rows[i]["天然氣自評表題目年份"].ToString();
                            ans_db._填寫人員類別 = LogInfo.competence;
                            ans_db._建立者 = LogInfo.mGuid;
                            ans_db._修改者 = LogInfo.mGuid;
                            #endregion                            

                            ans_db.SaveAnswer(oConn, myTrans);
                        }                        
					}
                    else //管理者
                    {
                        #region 業者答案
                        if (cAns != "")
                        {
                            ans_db._業者guid = cpid;
                            ans_db._答案 = cAns;
                            ans_db._委員意見 = "";
                            ans_db._檢視文件 = "";
                            ans_db._題目guid = qdt.Rows[i]["天然氣自評表題目guid"].ToString();
                            ans_db._年度 = qdt.Rows[i]["天然氣自評表題目年份"].ToString();
                            ans_db._填寫人員類別 = "02";
                            ans_db._建立者 = LogInfo.mGuid;
                            ans_db._修改者 = LogInfo.mGuid;

                            ans_db.SaveAnswer(oConn, myTrans);
                        }
                        #endregion

                        #region 委員答案&意見
                        if (mAns != "")
                        {
                            #region 自評表答案
                            ans_db._業者guid = cpid;
                            ans_db._答案 = mAns;
                            ans_db._檢視文件 = vfStr;
                            string pStr = PsStr;
                            ans_db._委員意見 = pStr;

                            ans_db._題目guid = qdt.Rows[i]["天然氣自評表題目guid"].ToString();
                            ans_db._年度 = qdt.Rows[i]["天然氣自評表題目年份"].ToString();
                            ans_db._填寫人員類別 = "01"; //for 0323
                            ans_db._建立者 = LogInfo.mGuid;
                            ans_db._修改者 = LogInfo.mGuid;
                            #endregion

                            #region 委員意見log
                            cs_db._委員guid = LogInfo.mGuid;
                            cs_db._委員 = LogInfo.name;
                            cs_db._業者guid = LogInfo.companyGuid;
                            cs_db._題目guid = qdt.Rows[i]["天然氣自評表題目guid"].ToString();
                            cs_db._年度 = qdt.Rows[i]["天然氣自評表題目年份"].ToString();
                            cs_db._答案 = mAns;
                            cs_db._檢視文件 = vfStr;
                            cs_db._委員意見 = pStr;
                            cs_db._建立者 = LogInfo.mGuid;
                            cs_db._修改者 = LogInfo.mGuid;
                            #endregion

                            ans_db.SaveAnswer(oConn, myTrans);
                        }
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
}