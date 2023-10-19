using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// VerificationTest_DB 的摘要描述
/// </summary>
public class VerificationTest_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 年度 = string.Empty;
	string 業者guid = string.Empty;
	string 類別 = string.Empty;
	string 場次 = string.Empty;
	string 改善情形 = string.Empty;
	string 報告編號 = string.Empty;
	string 查核日期起 = string.Empty;
	string 查核日期迄 = string.Empty;
	string 對象 = string.Empty;
	string 建立者 = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;
	#endregion
	#region public
	public string _id { set { id = value; } }
	public string _guid { set { guid = value; } }
	public string _年度 { set { 年度 = value; } }
	public string _業者guid { set { 業者guid = value; } }
	public string _類別 { set { 類別 = value; } }
	public string _場次 { set { 場次 = value; } }
	public string _改善情形 { set { 改善情形 = value; } }
	public string _報告編號 { set { 報告編號 = value; } }
	public string _查核日期起 { set { 查核日期起 = value; } }
	public string _查核日期迄 { set { 查核日期迄 = value; } }
	public string _對象 { set { 對象 = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }
	#endregion

	public DataTable GetList(string beginTime, string endTime)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select *, 
類別_V=(select 項目名稱 from 代碼檔 where 群組代碼='028' and 項目代碼=查核與檢測資料_基本資料表.類別),  
改善情形_V=(select 項目名稱 from 代碼檔 where 群組代碼='029' and 項目代碼=查核與檢測資料_基本資料表.改善情形)  
from 查核與檢測資料_基本資料表 where 資料狀態='A' and (@業者guid='' or 業者guid=@業者guid) 
and (@類別='' or 類別=@類別) and (@報告編號='' or  報告編號 like '%'+@報告編號+'%') 
and (@改善情形='' or 改善情形=@改善情形) 
");
		if (!string.IsNullOrEmpty(beginTime) && !string.IsNullOrEmpty(endTime))
			sb.Append(@" and 查核日期起 between @beginTime and @endTime ");

		sb.Append(@" ORDER BY convert(int, 年度) asc, convert(int, 類別) asc, convert(int, 場次) asc, convert(int, 查核日期起) asc ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@類別", 類別);
		oCmd.Parameters.AddWithValue("@報告編號", 報告編號);
		oCmd.Parameters.AddWithValue("@改善情形", 改善情形);
		oCmd.Parameters.AddWithValue("@beginTime", beginTime);
		oCmd.Parameters.AddWithValue("@endTime", endTime);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetCountList(string beginTime, string endTime)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select *, 
類別_V=(select 項目名稱 from 代碼檔 where 群組代碼='028' and 項目代碼=查核與檢測資料_基本資料表.類別), 
改善情形_V=(select 項目名稱 from 代碼檔 where 群組代碼='029' and 項目代碼=查核與檢測資料_基本資料表.改善情形), 
查核報告總和=(select count(*) as countall from 附件檔 where 檔案類型='10' and guid=查核與檢測資料_基本資料表.guid and 資料狀態='A' ), 
相關報告總和=(select count(*) as countall from 附件檔 where 檔案類型='11' and guid=查核與檢測資料_基本資料表.guid and 資料狀態='A' ) 
from 查核與檢測資料_基本資料表 where 資料狀態='A' and (@業者guid='' or 業者guid=@業者guid) 
and (@類別='' or 類別=@類別) and (@報告編號='' or  報告編號 like '%'+@報告編號+'%') 
and (@改善情形='' or 改善情形=@改善情形) 
");
		if (!string.IsNullOrEmpty(beginTime) && !string.IsNullOrEmpty(endTime))
			sb.Append(@" and 查核日期起 between @beginTime and @endTime ");

		sb.Append(@" ORDER BY 查核日期起 desc ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@類別", 類別);
		oCmd.Parameters.AddWithValue("@報告編號", 報告編號);
		oCmd.Parameters.AddWithValue("@改善情形", 改善情形);
		oCmd.Parameters.AddWithValue("@beginTime", beginTime);
		oCmd.Parameters.AddWithValue("@endTime", endTime);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetData()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * 
from 查核與檢測資料_基本資料表 where 資料狀態='A' and guid=@guid 
");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@guid", guid);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetSession(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"select 場次 from 查核與檢測資料_基本資料表 where 資料狀態='A' and 類別=@類別 and 年度=@年度 and 場次=@場次 ");

		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@類別", 類別);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@場次", 場次);

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetUpdateSession(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"select 場次 from 查核與檢測資料_基本資料表 where 資料狀態='A' and guid=@guid ");

		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@guid", guid);
		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@場次", 場次);

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();

		oda.Fill(ds);
		return ds;
	}

	public void InsertData(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into 查核與檢測資料_基本資料表(  
guid,
年度,
業者guid,
類別,
場次,
改善情形,
報告編號,
查核日期起,
查核日期迄,
對象,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@guid,
@年度,
@業者guid,
@類別,
@場次,
@改善情形,
@報告編號,
@查核日期起,
@查核日期迄,
@對象,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@guid", guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@類別", 類別);
		oCmd.Parameters.AddWithValue("@場次", 場次);
		oCmd.Parameters.AddWithValue("@改善情形", 改善情形);
		oCmd.Parameters.AddWithValue("@報告編號", 報告編號);
		oCmd.Parameters.AddWithValue("@查核日期起", 查核日期起);
		oCmd.Parameters.AddWithValue("@查核日期迄", 查核日期迄);
		oCmd.Parameters.AddWithValue("@對象", 對象);
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
		sb.Append(@"update 查核與檢測資料_基本資料表 set  
場次=@場次,
改善情形=@改善情形,
查核日期起=@查核日期起,
查核日期迄=@查核日期迄,
修改者=@修改者, 
修改日期=@修改日期 
where 資料狀態=@資料狀態 and guid=@guid 
");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@guid", guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@類別", 類別);
		oCmd.Parameters.AddWithValue("@場次", 場次);
		oCmd.Parameters.AddWithValue("@改善情形", 改善情形);
		oCmd.Parameters.AddWithValue("@報告編號", 報告編號);
		oCmd.Parameters.AddWithValue("@查核日期起", 查核日期起);
		oCmd.Parameters.AddWithValue("@查核日期迄", 查核日期迄);
		oCmd.Parameters.AddWithValue("@對象", 對象);
		oCmd.Parameters.AddWithValue("@修改者", 修改者);
		oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@資料狀態", 'A');

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public DataTable DeleteData()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"update 查核與檢測資料_基本資料表 set 
資料狀態='D' 
where guid=@guid and 資料狀態='A' 
");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@guid", guid);

		oda.Fill(ds);
		return ds;
	}
}