using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasCheckSmartTubeCleaner_DB 的摘要描述
/// </summary>
public class GasCheckSmartTubeCleaner_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 長途管線識別碼 = string.Empty;
	string 檢測方法 = string.Empty;
	string 最近一次執行年月 = string.Empty;
	string 報告產出年月 = string.Empty;
	string 檢測長度 = string.Empty;
	string 減薄3040數量_內 = string.Empty;
	string 減薄3040數量_內開挖確認 = string.Empty;
	string 減薄3040數量_外 = string.Empty;
	string 減薄3040數量_外開挖確認 = string.Empty;
	string 減薄4050數量_內 = string.Empty;
	string 減薄4050數量_內開挖確認 = string.Empty;
	string 減薄4050數量_外 = string.Empty;
	string 減薄4050數量_外開挖確認 = string.Empty;
	string 減薄50以上數量_內 = string.Empty;
	string 減薄50以上數量_內開挖確認 = string.Empty;
	string 減薄50以上數量_外 = string.Empty;
	string 減薄50以上數量_外開挖確認 = string.Empty;
	string Dent_大於12 = string.Empty;
	string Dent_開挖確認 = string.Empty;
	string 備註 = string.Empty;
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
	public string _檢測方法 { set { 檢測方法 = value; } }
	public string _最近一次執行年月 { set { 最近一次執行年月 = value; } }
	public string _報告產出年月 { set { 報告產出年月 = value; } }
	public string _檢測長度 { set { 檢測長度 = value; } }
	public string _減薄3040數量_內 { set { 減薄3040數量_內 = value; } }
	public string _減薄3040數量_內開挖確認 { set { 減薄3040數量_內開挖確認 = value; } }
	public string _減薄3040數量_外 { set { 減薄3040數量_外 = value; } }
	public string _減薄3040數量_外開挖確認 { set { 減薄3040數量_外開挖確認 = value; } }
	public string _減薄4050數量_內 { set { 減薄4050數量_內 = value; } }
	public string _減薄4050數量_內開挖確認 { set { 減薄4050數量_內開挖確認 = value; } }
	public string _減薄4050數量_外 { set { 減薄4050數量_外 = value; } }
	public string _減薄4050數量_外開挖確認 { set { 減薄4050數量_外開挖確認 = value; } }
	public string _減薄50以上數量_內 { set { 減薄50以上數量_內 = value; } }
	public string _減薄50以上數量_內開挖確認 { set { 減薄50以上數量_內開挖確認 = value; } }
	public string _減薄50以上數量_外 { set { 減薄50以上數量_外 = value; } }
	public string _減薄50以上數量_外開挖確認 { set { 減薄50以上數量_外開挖確認 = value; } }
	public string _Dent_大於12 { set { Dent_大於12 = value; } }
	public string _Dent_開挖確認 { set { Dent_開挖確認 = value; } }
	public string _備註 { set { 備註 = value; } }
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

		sb.Append(@"select * from 天然氣_智慧型通管器檢查ILI where 資料狀態='A' and 業者guid=@業者guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}
}