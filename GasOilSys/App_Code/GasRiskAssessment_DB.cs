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
		if (!string.IsNullOrEmpty(年度))
			sb.Append(@" and 年度=@年度");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);

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

select DISTINCT 年度 into #tmp from 天然氣_風險評估
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

        sb.Append(@"select * from 天然氣_風險評估 where guid=@guid and 資料狀態='A' ");

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
        sb.Append(@"insert into 天然氣_風險評估(  
年度,
業者guid,
長途管線識別碼,
最近一次執行日期,
再評估時機,
管線長度,
分段數量,
已納入ILI結果,
已納入CIPS結果,
已納入巡管結果,
各等級風險管段數量_高,
各等級風險管段數量_中,
各等級風險管段數量_低,
降低中高風險管段之相關作為文件名稱,
改善後風險等級高中低,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@長途管線識別碼,
@最近一次執行日期,
@再評估時機,
@管線長度,
@分段數量,
@已納入ILI結果,
@已納入CIPS結果,
@已納入巡管結果,
@各等級風險管段數量_高,
@各等級風險管段數量_中,
@各等級風險管段數量_低,
@降低中高風險管段之相關作為文件名稱,
@改善後風險等級高中低,
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
        oCmd.Parameters.AddWithValue("@最近一次執行日期", 最近一次執行日期);
        oCmd.Parameters.AddWithValue("@再評估時機", 再評估時機);
        oCmd.Parameters.AddWithValue("@管線長度", 管線長度);
        oCmd.Parameters.AddWithValue("@分段數量", 分段數量);
        oCmd.Parameters.AddWithValue("@已納入ILI結果", 已納入ILI結果);
        oCmd.Parameters.AddWithValue("@已納入CIPS結果", 已納入CIPS結果);
        oCmd.Parameters.AddWithValue("@已納入巡管結果", 已納入巡管結果);
        oCmd.Parameters.AddWithValue("@各等級風險管段數量_高", 各等級風險管段數量_高);
        oCmd.Parameters.AddWithValue("@各等級風險管段數量_中", 各等級風險管段數量_中);
        oCmd.Parameters.AddWithValue("@各等級風險管段數量_低", 各等級風險管段數量_低);
        oCmd.Parameters.AddWithValue("@降低中高風險管段之相關作為文件名稱", 降低中高風險管段之相關作為文件名稱);
        oCmd.Parameters.AddWithValue("@改善後風險等級高中低", 改善後風險等級高中低);
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
        sb.Append(@"update 天然氣_風險評估 set  
年度=@年度,
長途管線識別碼=@長途管線識別碼,
最近一次執行日期=@最近一次執行日期,
再評估時機=@再評估時機,
管線長度=@管線長度,
分段數量=@分段數量,
已納入ILI結果=@已納入ILI結果,
已納入CIPS結果=@已納入CIPS結果,
已納入巡管結果=@已納入巡管結果,
各等級風險管段數量_高=@各等級風險管段數量_高,
各等級風險管段數量_中=@各等級風險管段數量_中,
各等級風險管段數量_低=@各等級風險管段數量_低,
降低中高風險管段之相關作為文件名稱=@降低中高風險管段之相關作為文件名稱,
改善後風險等級高中低=@改善後風險等級高中低,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@最近一次執行日期", 最近一次執行日期);
        oCmd.Parameters.AddWithValue("@再評估時機", 再評估時機);
        oCmd.Parameters.AddWithValue("@管線長度", 管線長度);
        oCmd.Parameters.AddWithValue("@分段數量", 分段數量);
        oCmd.Parameters.AddWithValue("@已納入ILI結果", 已納入ILI結果);
        oCmd.Parameters.AddWithValue("@已納入CIPS結果", 已納入CIPS結果);
        oCmd.Parameters.AddWithValue("@已納入巡管結果", 已納入巡管結果);
        oCmd.Parameters.AddWithValue("@各等級風險管段數量_高", 各等級風險管段數量_高);
        oCmd.Parameters.AddWithValue("@各等級風險管段數量_中", 各等級風險管段數量_中);
        oCmd.Parameters.AddWithValue("@各等級風險管段數量_低", 各等級風險管段數量_低);
        oCmd.Parameters.AddWithValue("@降低中高風險管段之相關作為文件名稱", 降低中高風險管段之相關作為文件名稱);
        oCmd.Parameters.AddWithValue("@改善後風險等級高中低", 改善後風險等級高中低);
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
        oCmd.CommandText = @"update 天然氣_風險評估 set 
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