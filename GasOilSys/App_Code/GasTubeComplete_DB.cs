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

	public DataSet GetList(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * into #tmp 
from 天然氣_管線完整性管理作為_幹線及環線管線 where 資料狀態='A' and 業者guid=@業者guid ");
		if (!string.IsNullOrEmpty(年度))
			sb.Append(@" and 年度=@年度");

		sb.Append(@"
select count(*) as total from #tmp

select * from (
           select ROW_NUMBER() over (order by 長途管線識別碼) itemNo,* from #tmp
)#tmp where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		oda.Fill(ds);
		return ds;
	}

	public DataSet GetList_Out(string pStart, string pEnd)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * into #tmp 
from 天然氣_管線完整性管理作為_幹線及環線管線以外 where 資料狀態='A' and 業者guid=@業者guid ");
		if (!string.IsNullOrEmpty(年度))
			sb.Append(@" and 年度=@年度");
		if (!string.IsNullOrEmpty(長途管線識別碼))
			sb.Append(@" and 長途管線識別碼=@長途管線識別碼");

		sb.Append(@"
select count(*) as total from #tmp

select * from (
           select ROW_NUMBER() over (order by 長途管線識別碼) itemNo,* from #tmp
)#tmp where itemNo between @pStart and @pEnd ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataSet ds = new DataSet();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
		oCmd.Parameters.AddWithValue("@pStart", pStart);
		oCmd.Parameters.AddWithValue("@pEnd", pEnd);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetYearList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"  
declare @yearCount int

select DISTINCT 年度 into #tmp from 天然氣_管線完整性管理作為_幹線及環線管線
where 業者guid=@業者guid and 資料狀態='A' 

select @yearCount=COUNT(*) from #tmp where 年度=@年度 

if(@yearCount > 0)
	begin
		select * from #tmp order by 年度 asc
	end
else
	begin
		insert into #tmp(年度)
		values(@年度)

		select * from #tmp order by 年度 asc
	end ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetYearList_Out()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"  
declare @yearCount int

select DISTINCT 年度 into #tmp from 天然氣_管線完整性管理作為_幹線及環線管線以外
where 業者guid=@業者guid and 資料狀態='A' 

select @yearCount=COUNT(*) from #tmp where 年度=@年度 

if(@yearCount > 0)
	begin
		select * from #tmp order by 年度 asc
	end
else
	begin
		insert into #tmp(年度)
		values(@年度)

		select * from #tmp order by 年度 asc
	end ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetData()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_管線完整性管理作為_幹線及環線管線 where guid=@guid and 資料狀態='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@guid", guid);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetData_Out()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_管線完整性管理作為_幹線及環線管線以外 where guid=@guid and 資料狀態='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@guid", guid);

		oda.Fill(ds);
		return ds;
	}

	public void InsertData(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into 天然氣_管線完整性管理作為_幹線及環線管線(  
年度,
業者guid,
長途管線識別碼,
風險評估年月,
智慧型通管器ILI可行性,
耐壓強度試驗TP可行性,
緊密電位CIPS年月,
電磁包覆PCM年月,
智慧型通管器ILI年月,
耐壓強度試驗TP年月,
耐壓強度試驗TP介質,
試壓壓力與MOP壓力倍數,
耐壓強度試驗TP持壓時間,
受雜散電流影響,
洩漏偵測系統LLDS,
強化作為,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@長途管線識別碼,
@風險評估年月,
@智慧型通管器ILI可行性,
@耐壓強度試驗TP可行性,
@緊密電位CIPS年月,
@電磁包覆PCM年月,
@智慧型通管器ILI年月,
@耐壓強度試驗TP年月,
@耐壓強度試驗TP介質,
@試壓壓力與MOP壓力倍數,
@耐壓強度試驗TP持壓時間,
@受雜散電流影響,
@洩漏偵測系統LLDS,
@強化作為,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
		oCmd.Parameters.AddWithValue("@風險評估年月", 風險評估年月);
		oCmd.Parameters.AddWithValue("@智慧型通管器ILI可行性", 智慧型通管器ILI可行性);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP可行性", 耐壓強度試驗TP可行性);
		oCmd.Parameters.AddWithValue("@緊密電位CIPS年月", 緊密電位CIPS年月);
		oCmd.Parameters.AddWithValue("@電磁包覆PCM年月", 電磁包覆PCM年月);
		oCmd.Parameters.AddWithValue("@智慧型通管器ILI年月", 智慧型通管器ILI年月);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP年月", 耐壓強度試驗TP年月);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP介質", 耐壓強度試驗TP介質);
		oCmd.Parameters.AddWithValue("@試壓壓力與MOP壓力倍數", 試壓壓力與MOP壓力倍數);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP持壓時間", 耐壓強度試驗TP持壓時間);
		oCmd.Parameters.AddWithValue("@受雜散電流影響", 受雜散電流影響);
		oCmd.Parameters.AddWithValue("@洩漏偵測系統LLDS", 洩漏偵測系統LLDS);
		oCmd.Parameters.AddWithValue("@強化作為", 強化作為);
		oCmd.Parameters.AddWithValue("@修改者", 修改者);
		oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@建立者", 建立者);
		oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@資料狀態", 'A');

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void InsertData_Out(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into 天然氣_管線完整性管理作為_幹線及環線管線以外(  
年度,
業者guid,
長途管線識別碼,
風險評估年月,
智慧型通管器ILI可行性,
耐壓強度試驗TP可行性,
緊密電位CIPS年月,
電磁包覆PCM年月,
智慧型通管器ILI年月,
耐壓強度試驗TP年月,
耐壓強度試驗TP介質,
試壓壓力與MOP壓力倍數,
耐壓強度試驗TP持壓時間,
受雜散電流影響,
洩漏偵測系統LLDS,
強化作為,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@長途管線識別碼,
@風險評估年月,
@智慧型通管器ILI可行性,
@耐壓強度試驗TP可行性,
@緊密電位CIPS年月,
@電磁包覆PCM年月,
@智慧型通管器ILI年月,
@耐壓強度試驗TP年月,
@耐壓強度試驗TP介質,
@試壓壓力與MOP壓力倍數,
@耐壓強度試驗TP持壓時間,
@受雜散電流影響,
@洩漏偵測系統LLDS,
@強化作為,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
		oCmd.Parameters.AddWithValue("@風險評估年月", 風險評估年月);
		oCmd.Parameters.AddWithValue("@智慧型通管器ILI可行性", 智慧型通管器ILI可行性);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP可行性", 耐壓強度試驗TP可行性);
		oCmd.Parameters.AddWithValue("@緊密電位CIPS年月", 緊密電位CIPS年月);
		oCmd.Parameters.AddWithValue("@電磁包覆PCM年月", 電磁包覆PCM年月);
		oCmd.Parameters.AddWithValue("@智慧型通管器ILI年月", 智慧型通管器ILI年月);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP年月", 耐壓強度試驗TP年月);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP介質", 耐壓強度試驗TP介質);
		oCmd.Parameters.AddWithValue("@試壓壓力與MOP壓力倍數", 試壓壓力與MOP壓力倍數);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP持壓時間", 耐壓強度試驗TP持壓時間);
		oCmd.Parameters.AddWithValue("@受雜散電流影響", 受雜散電流影響);
		oCmd.Parameters.AddWithValue("@洩漏偵測系統LLDS", 洩漏偵測系統LLDS);
		oCmd.Parameters.AddWithValue("@強化作為", 強化作為);
		oCmd.Parameters.AddWithValue("@修改者", 修改者);
		oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@建立者", 建立者);
		oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@資料狀態", 'A');

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void UpdateData(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update 天然氣_管線完整性管理作為_幹線及環線管線 set  
年度=@年度,
長途管線識別碼=@長途管線識別碼,
風險評估年月=@風險評估年月,
智慧型通管器ILI可行性=@智慧型通管器ILI可行性,
耐壓強度試驗TP可行性=@耐壓強度試驗TP可行性,
緊密電位CIPS年月=@緊密電位CIPS年月,
電磁包覆PCM年月=@電磁包覆PCM年月,
智慧型通管器ILI年月=@智慧型通管器ILI年月,
耐壓強度試驗TP年月=@耐壓強度試驗TP年月,
耐壓強度試驗TP介質=@耐壓強度試驗TP介質,
試壓壓力與MOP壓力倍數=@試壓壓力與MOP壓力倍數,
耐壓強度試驗TP持壓時間=@耐壓強度試驗TP持壓時間,
受雜散電流影響=@受雜散電流影響,
洩漏偵測系統LLDS=@洩漏偵測系統LLDS,
強化作為=@強化作為,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@guid", guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
		oCmd.Parameters.AddWithValue("@風險評估年月", 風險評估年月);
		oCmd.Parameters.AddWithValue("@智慧型通管器ILI可行性", 智慧型通管器ILI可行性);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP可行性", 耐壓強度試驗TP可行性);
		oCmd.Parameters.AddWithValue("@緊密電位CIPS年月", 緊密電位CIPS年月);
		oCmd.Parameters.AddWithValue("@電磁包覆PCM年月", 電磁包覆PCM年月);
		oCmd.Parameters.AddWithValue("@智慧型通管器ILI年月", 智慧型通管器ILI年月);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP年月", 耐壓強度試驗TP年月);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP介質", 耐壓強度試驗TP介質);
		oCmd.Parameters.AddWithValue("@試壓壓力與MOP壓力倍數", 試壓壓力與MOP壓力倍數);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP持壓時間", 耐壓強度試驗TP持壓時間);
		oCmd.Parameters.AddWithValue("@受雜散電流影響", 受雜散電流影響);
		oCmd.Parameters.AddWithValue("@洩漏偵測系統LLDS", 洩漏偵測系統LLDS);
		oCmd.Parameters.AddWithValue("@強化作為", 強化作為);
		oCmd.Parameters.AddWithValue("@修改者", 修改者);
		oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@資料狀態", 'A');

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void UpdateData_Out(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"update 天然氣_管線完整性管理作為_幹線及環線管線以外 set  
年度=@年度,
長途管線識別碼=@長途管線識別碼,
風險評估年月=@風險評估年月,
智慧型通管器ILI可行性=@智慧型通管器ILI可行性,
耐壓強度試驗TP可行性=@耐壓強度試驗TP可行性,
緊密電位CIPS年月=@緊密電位CIPS年月,
電磁包覆PCM年月=@電磁包覆PCM年月,
智慧型通管器ILI年月=@智慧型通管器ILI年月,
耐壓強度試驗TP年月=@耐壓強度試驗TP年月,
耐壓強度試驗TP介質=@耐壓強度試驗TP介質,
試壓壓力與MOP壓力倍數=@試壓壓力與MOP壓力倍數,
耐壓強度試驗TP持壓時間=@耐壓強度試驗TP持壓時間,
受雜散電流影響=@受雜散電流影響,
洩漏偵測系統LLDS=@洩漏偵測系統LLDS,
強化作為=@強化作為,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@guid", guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
		oCmd.Parameters.AddWithValue("@風險評估年月", 風險評估年月);
		oCmd.Parameters.AddWithValue("@智慧型通管器ILI可行性", 智慧型通管器ILI可行性);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP可行性", 耐壓強度試驗TP可行性);
		oCmd.Parameters.AddWithValue("@緊密電位CIPS年月", 緊密電位CIPS年月);
		oCmd.Parameters.AddWithValue("@電磁包覆PCM年月", 電磁包覆PCM年月);
		oCmd.Parameters.AddWithValue("@智慧型通管器ILI年月", 智慧型通管器ILI年月);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP年月", 耐壓強度試驗TP年月);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP介質", 耐壓強度試驗TP介質);
		oCmd.Parameters.AddWithValue("@試壓壓力與MOP壓力倍數", 試壓壓力與MOP壓力倍數);
		oCmd.Parameters.AddWithValue("@耐壓強度試驗TP持壓時間", 耐壓強度試驗TP持壓時間);
		oCmd.Parameters.AddWithValue("@受雜散電流影響", 受雜散電流影響);
		oCmd.Parameters.AddWithValue("@洩漏偵測系統LLDS", 洩漏偵測系統LLDS);
		oCmd.Parameters.AddWithValue("@強化作為", 強化作為);
		oCmd.Parameters.AddWithValue("@修改者", 修改者);
		oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@資料狀態", 'A');

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void DeleteData()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update 天然氣_管線完整性管理作為_幹線及環線管線 set 
修改日期=@修改日期, 
修改者=@修改者, 
資料狀態='D' 
where guid=@guid ";

		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@guid", guid);
		oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@修改者", 修改者);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}

	public void DeleteData_Out()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		oCmd.CommandText = @"update 天然氣_管線完整性管理作為_幹線及環線管線以外 set 
修改日期=@修改日期, 
修改者=@修改者, 
資料狀態='D' 
where guid=@guid ";

		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		oCmd.Parameters.AddWithValue("@guid", guid);
		oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@修改者", 修改者);

		oCmd.Connection.Open();
		oCmd.ExecuteNonQuery();
		oCmd.Connection.Close();
	}
}