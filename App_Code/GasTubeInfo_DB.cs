using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasTubeInfo_DB 的摘要描述
/// </summary>
public class GasTubeInfo_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 長途管線識別碼 = string.Empty;
	string 轄區長途管線名稱_公司 = string.Empty;
	string 銜接管線識別碼_上游 = string.Empty;
	string 銜接管線識別碼_下游 = string.Empty;
	string 起點 = string.Empty;
	string 迄點 = string.Empty;
	string 管徑 = string.Empty;
	string 厚度 = string.Empty;
	string 管材 = string.Empty;
	string 包覆材料 = string.Empty;
	string 轄管長度 = string.Empty;
	string 內容物 = string.Empty;
	string 緊急遮斷閥 = string.Empty;
	string 建置年 = string.Empty;
	string 設計壓力 = string.Empty;
	string 使用壓力 = string.Empty;
	string 使用狀態 = string.Empty;
	string 附掛橋樑數量 = string.Empty;
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
	public string _轄區長途管線名稱_公司 { set { 轄區長途管線名稱_公司 = value; } }
	public string _銜接管線識別碼_上游 { set { 銜接管線識別碼_上游 = value; } }
	public string _銜接管線識別碼_下游 { set { 銜接管線識別碼_下游 = value; } }
	public string _起點 { set { 起點 = value; } }
	public string _迄點 { set { 迄點 = value; } }
	public string _管徑 { set { 管徑 = value; } }
	public string _厚度 { set { 厚度 = value; } }
	public string _管材 { set { 管材 = value; } }
	public string _包覆材料 { set { 包覆材料 = value; } }
	public string _轄管長度 { set { 轄管長度 = value; } }
	public string _內容物 { set { 內容物 = value; } }
	public string _緊急遮斷閥 { set { 緊急遮斷閥 = value; } }
	public string _建置年 { set { 建置年 = value; } }
	public string _設計壓力 { set { 設計壓力 = value; } }
	public string _使用壓力 { set { 使用壓力 = value; } }
	public string _使用狀態 { set { 使用狀態 = value; } }
	public string _附掛橋樑數量 { set { 附掛橋樑數量 = value; } }
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

		sb.Append(@"select * from 天然氣_管線基本資料 where 資料狀態='A' and 業者guid=@業者guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}
}