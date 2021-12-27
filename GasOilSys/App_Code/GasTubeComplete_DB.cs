using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasTubeComplete_DB 的摘要描述
/// </summary>
public class GasTubeComplete_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 長途管線識別碼 = string.Empty;
	string 風險評估年月 = string.Empty;
	string 智慧型通管器ILI可行性 = string.Empty;
	string 耐壓強度試驗TP可行性 = string.Empty;
	string 緊密電位CIPS年月 = string.Empty;
	string 電磁包覆PCM年月 = string.Empty;
	string 智慧型通管器ILI年月 = string.Empty;
	string 耐壓強度試驗TP年月 = string.Empty;
	string 耐壓強度試驗TP介質 = string.Empty;
	string 試壓壓力與MOP壓力倍數 = string.Empty;
	string 耐壓強度試驗TP持壓時間 = string.Empty;
	string 受雜散電流影響 = string.Empty;
	string 洩漏偵測系統LLDS = string.Empty;
	string 強化作為 = string.Empty;
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
	public string _風險評估年月 { set { 風險評估年月 = value; } }
	public string _智慧型通管器ILI可行性 { set { 智慧型通管器ILI可行性 = value; } }
	public string _耐壓強度試驗TP可行性 { set { 耐壓強度試驗TP可行性 = value; } }
	public string _緊密電位CIPS年月 { set { 緊密電位CIPS年月 = value; } }
	public string _電磁包覆PCM年月 { set { 電磁包覆PCM年月 = value; } }
	public string _智慧型通管器ILI年月 { set { 智慧型通管器ILI年月 = value; } }
	public string _耐壓強度試驗TP年月 { set { 耐壓強度試驗TP年月 = value; } }
	public string _耐壓強度試驗TP介質 { set { 耐壓強度試驗TP介質 = value; } }
	public string _試壓壓力與MOP壓力倍數 { set { 試壓壓力與MOP壓力倍數 = value; } }
	public string _耐壓強度試驗TP持壓時間 { set { 耐壓強度試驗TP持壓時間 = value; } }
	public string _受雜散電流影響 { set { 受雜散電流影響 = value; } }
	public string _洩漏偵測系統LLDS { set { 洩漏偵測系統LLDS = value; } }
	public string _強化作為 { set { 強化作為 = value; } }
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

		sb.Append(@"select * from 天然氣_管線完整性管理作為_幹線及環線管線 where 資料狀態='A' and 業者guid=@業者guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetList_Out()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_管線完整性管理作為_幹線及環線管線以外 where 資料狀態='A' and 業者guid=@業者guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}
}