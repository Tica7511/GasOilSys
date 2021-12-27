using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasUnusualRectifier_DB 的摘要描述
/// </summary>
public class GasUnusualRectifier_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 異常整流站名稱 = string.Empty;
	string 異常起始日期年月 = string.Empty;
	string 異常狀況 = string.Empty;
	string 整流站修復進度 = string.Empty;
	string 影響長途管線識別碼 = string.Empty;
	string 預計完成日期 = string.Empty;
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
	public string _異常整流站名稱 { set { 異常整流站名稱 = value; } }
	public string _異常起始日期年月 { set { 異常起始日期年月 = value; } }
	public string _異常狀況 { set { 異常狀況 = value; } }
	public string _整流站修復進度 { set { 整流站修復進度 = value; } }
	public string _影響長途管線識別碼 { set { 影響長途管線識別碼 = value; } }
	public string _預計完成日期 { set { 預計完成日期 = value; } }
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

		sb.Append(@"select * from 天然氣_異常整流站 where 資料狀態='A' and 業者guid=@業者guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}
}