using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasRiskAssessment_DB 的摘要描述
/// </summary>
public class GasRiskAssessment_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 長途管線識別碼 = string.Empty;
	string 最近一次執行日期 = string.Empty;
	string 再評估時機 = string.Empty;
	string 管線長度 = string.Empty;
	string 分段數量 = string.Empty;
	string 已納入ILI結果 = string.Empty;
	string 已納入CIPS結果 = string.Empty;
	string 已納入巡管結果 = string.Empty;
	string 各等級風險管段數量_高 = string.Empty;
	string 各等級風險管段數量_中 = string.Empty;
	string 各等級風險管段數量_低 = string.Empty;
	string 降低中高風險管段之相關作為文件名稱 = string.Empty;
	string 改善後風險等級高中低 = string.Empty;
	string 建立者 = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;
	#endregion
	#region public
	public string _id { set { id = value; } }
	public string _guid { set { guid = value; } }
	public string _業者guid { set { 業者guid = value; } }
	public string _年度 { set { 年度 = value; } }
	public string _長途管線識別碼 { set { 長途管線識別碼 = value; } }
	public string _最近一次執行日期 { set { 最近一次執行日期 = value; } }
	public string _再評估時機 { set { 再評估時機 = value; } }
	public string _管線長度 { set { 管線長度 = value; } }
	public string _分段數量 { set { 分段數量 = value; } }
	public string _已納入ILI結果 { set { 已納入ILI結果 = value; } }
	public string _已納入CIPS結果 { set { 已納入CIPS結果 = value; } }
	public string _已納入巡管結果 { set { 已納入巡管結果 = value; } }
	public string _各等級風險管段數量_高 { set { 各等級風險管段數量_高 = value; } }
	public string _各等級風險管段數量_中 { set { 各等級風險管段數量_中 = value; } }
	public string _各等級風險管段數量_低 { set { 各等級風險管段數量_低 = value; } }
	public string _降低中高風險管段之相關作為文件名稱 { set { 降低中高風險管段之相關作為文件名稱 = value; } }
	public string _改善後風險等級高中低 { set { 改善後風險等級高中低 = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_風險評估 where 資料狀態='A' and 業者guid=@業者guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}
}