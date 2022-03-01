using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasCIPS_DB 的摘要描述
/// </summary>
public class GasCIPS_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 長途管線識別碼 = string.Empty;
	string 同時檢測管線數量 = string.Empty;
	string 最近一次執行年月 = string.Empty;
	string 報告產出年月 = string.Empty;
	string 檢測長度 = string.Empty;
	string 合格標準請參照填表說明2 = string.Empty;
	string 立即改善_數量 = string.Empty;
	string 立即改善_改善完成數量 = string.Empty;
	string 排程改善_數量 = string.Empty;
	string 排程改善_改善完成數量 = string.Empty;
	string 需監控點_數量 = string.Empty;
    string x座標 = string.Empty;
    string y座標 = string.Empty;
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
	public string _同時檢測管線數量 { set { 同時檢測管線數量 = value; } }
	public string _最近一次執行年月 { set { 最近一次執行年月 = value; } }
	public string _報告產出年月 { set { 報告產出年月 = value; } }
	public string _檢測長度 { set { 檢測長度 = value; } }
	public string _合格標準請參照填表說明2 { set { 合格標準請參照填表說明2 = value; } }
	public string _立即改善_數量 { set { 立即改善_數量 = value; } }
	public string _立即改善_改善完成數量 { set { 立即改善_改善完成數量 = value; } }
	public string _排程改善_數量 { set { 排程改善_數量 = value; } }
	public string _排程改善_改善完成數量 { set { 排程改善_改善完成數量 = value; } }
	public string _需監控點_數量 { set { 需監控點_數量 = value; } }
    public string _x座標 { set { x座標 = value; } }
    public string _y座標 { set { y座標 = value; } }
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

		sb.Append(@"select * from 天然氣_緊密電位檢測CIPS where 資料狀態='A' and 業者guid=@業者guid ");
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

select DISTINCT 年度 into #tmp from 天然氣_緊密電位檢測CIPS
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

        sb.Append(@"select * from 天然氣_緊密電位檢測CIPS where guid=@guid and 資料狀態='A' ");

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
        sb.Append(@"insert into 天然氣_緊密電位檢測CIPS(  
年度,
業者guid,
長途管線識別碼,
同時檢測管線數量,
最近一次執行年月,
報告產出年月,
檢測長度,
合格標準請參照填表說明2,
立即改善_數量,
立即改善_改善完成數量,
排程改善_數量,
排程改善_改善完成數量,
需監控點_數量,
x座標,
y座標,
備註,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@長途管線識別碼,
@同時檢測管線數量,
@最近一次執行年月,
@報告產出年月,
@檢測長度,
@合格標準請參照填表說明2,
@立即改善_數量,
@立即改善_改善完成數量,
@排程改善_數量,
@排程改善_改善完成數量,
@需監控點_數量,
x座標,
y座標,
@備註,
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
        oCmd.Parameters.AddWithValue("@同時檢測管線數量", 同時檢測管線數量);
        oCmd.Parameters.AddWithValue("@最近一次執行年月", 最近一次執行年月);
        oCmd.Parameters.AddWithValue("@報告產出年月", 報告產出年月);
        oCmd.Parameters.AddWithValue("@檢測長度", 檢測長度);
        oCmd.Parameters.AddWithValue("@合格標準請參照填表說明2", 合格標準請參照填表說明2);
        oCmd.Parameters.AddWithValue("@立即改善_數量", 立即改善_數量);
        oCmd.Parameters.AddWithValue("@立即改善_改善完成數量", 立即改善_改善完成數量);
        oCmd.Parameters.AddWithValue("@排程改善_數量", 排程改善_數量);
        oCmd.Parameters.AddWithValue("@排程改善_改善完成數量", 排程改善_改善完成數量);
        oCmd.Parameters.AddWithValue("@需監控點_數量", 需監控點_數量);
        oCmd.Parameters.AddWithValue("@x座標", x座標);
        oCmd.Parameters.AddWithValue("@y座標", y座標);
        oCmd.Parameters.AddWithValue("@備註", 備註);
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
        sb.Append(@"update 天然氣_緊密電位檢測CIPS set  
長途管線識別碼=@長途管線識別碼,
同時檢測管線數量=@同時檢測管線數量,
最近一次執行年月=@最近一次執行年月,
報告產出年月=@報告產出年月,
檢測長度=@檢測長度,
合格標準請參照填表說明2=@合格標準請參照填表說明2,
立即改善_數量=@立即改善_數量,
立即改善_改善完成數量=@立即改善_改善完成數量,
排程改善_數量=@排程改善_數量,
排程改善_改善完成數量=@排程改善_改善完成數量,
需監控點_數量=@需監控點_數量,
x座標=@x座標,
y座標=@y座標,
備註=@備註,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@同時檢測管線數量", 同時檢測管線數量);
        oCmd.Parameters.AddWithValue("@最近一次執行年月", 最近一次執行年月);
        oCmd.Parameters.AddWithValue("@報告產出年月", 報告產出年月);
        oCmd.Parameters.AddWithValue("@檢測長度", 檢測長度);
        oCmd.Parameters.AddWithValue("@合格標準請參照填表說明2", 合格標準請參照填表說明2);
        oCmd.Parameters.AddWithValue("@立即改善_數量", 立即改善_數量);
        oCmd.Parameters.AddWithValue("@立即改善_改善完成數量", 立即改善_改善完成數量);
        oCmd.Parameters.AddWithValue("@排程改善_數量", 排程改善_數量);
        oCmd.Parameters.AddWithValue("@排程改善_改善完成數量", 排程改善_改善完成數量);
        oCmd.Parameters.AddWithValue("@需監控點_數量", 需監控點_數量);
        oCmd.Parameters.AddWithValue("@x座標", x座標);
        oCmd.Parameters.AddWithValue("@y座標", y座標);
        oCmd.Parameters.AddWithValue("@備註", 備註);
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
        oCmd.CommandText = @"update 天然氣_緊密電位檢測CIPS set 
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