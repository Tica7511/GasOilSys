using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// PublicGasDoorRecord_DB 的摘要描述
/// </summary>
public class PublicGasDoorRecord_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;
    string 序號 = string.Empty;
    string 編號 = string.Empty;
    string 設置年月 = string.Empty;
    string 設施種類 = string.Empty;
    string 位置描述 = string.Empty;
    string 壓力別 = string.Empty;
    string 口徑 = string.Empty;
    string 隸屬管線 = string.Empty;
    string 檢查日期 = string.Empty;
    string 漏氣檢測_正常 = string.Empty;
    string 漏氣檢測_檢查值 = string.Empty;
    string 漏氣檢測_漏氣處理 = string.Empty;
    string 抽水 = string.Empty;
    string 閥門試轉 = string.Empty;
    string 油漆 = string.Empty;
    string 汙泥 = string.Empty;
    string 防滑蓋改善 = string.Empty;
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
    public string _序號 { set { 序號 = value; } }
    public string _編號 { set { 編號 = value; } }
    public string _設置年月 { set { 設置年月 = value; } }
    public string _設施種類 { set { 設施種類 = value; } }
    public string _位置描述 { set { 位置描述 = value; } }
    public string _壓力別 { set { 壓力別 = value; } }
    public string _口徑 { set { 口徑 = value; } }
    public string _隸屬管線 { set { 隸屬管線 = value; } }
    public string _檢查日期 { set { 檢查日期 = value; } }
    public string _漏氣檢測_正常 { set { 漏氣檢測_正常 = value; } }
    public string _漏氣檢測_檢查值 { set { 漏氣檢測_檢查值 = value; } }
    public string _漏氣檢測_漏氣處理 { set { 漏氣檢測_漏氣處理 = value; } }
    public string _抽水 { set { 抽水 = value; } }
    public string _閥門試轉 { set { 閥門試轉 = value; } }
    public string _油漆 { set { 油漆 = value; } }
    public string _汙泥 { set { 汙泥 = value; } }
    public string _防滑蓋改善 { set { 防滑蓋改善 = value; } }
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

        sb.Append(@"select * from 公用天然氣_各類型之閥紀錄表 where 資料狀態='A' and 業者guid=@業者guid ");
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

select DISTINCT 年度 into #tmp from 公用天然氣_各類型之閥紀錄表 
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

        sb.Append(@"select * from 公用天然氣_各類型之閥紀錄表] where guid=@guid and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

//    public void InsertData(SqlConnection oConn, SqlTransaction oTran)
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.Append(@"insert into 天然氣_異常整流站(  
//年度,
//業者guid,
//異常整流站名稱,
//異常起始日期年月,
//異常狀況,
//整流站修復進度,
//影響長途管線識別碼,
//預計完成日期,
//備註,
//修改者, 
//修改日期, 
//建立者, 
//建立日期, 
//資料狀態 ) values ( 
//@年度,
//@業者guid,
//@異常整流站名稱,
//@異常起始日期年月,
//@異常狀況,
//@整流站修復進度,
//@影響長途管線識別碼,
//@預計完成日期,
//@備註,
//@修改者, 
//@修改日期, 
//@建立者, 
//@建立日期, 
//@資料狀態 
//) ");
//        SqlCommand oCmd = oConn.CreateCommand();
//        oCmd.CommandText = sb.ToString();

//        oCmd.Parameters.AddWithValue("@年度", 年度);
//        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
//        oCmd.Parameters.AddWithValue("@異常整流站名稱", 異常整流站名稱);
//        oCmd.Parameters.AddWithValue("@異常起始日期年月", 異常起始日期年月);
//        oCmd.Parameters.AddWithValue("@異常狀況", 異常狀況);
//        oCmd.Parameters.AddWithValue("@整流站修復進度", 整流站修復進度);
//        oCmd.Parameters.AddWithValue("@影響長途管線識別碼", 影響長途管線識別碼);
//        oCmd.Parameters.AddWithValue("@預計完成日期", 預計完成日期);
//        oCmd.Parameters.AddWithValue("@備註", 備註);
//        oCmd.Parameters.AddWithValue("@修改者", 修改者);
//        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
//        oCmd.Parameters.AddWithValue("@建立者", 建立者);
//        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
//        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

//        oCmd.Transaction = oTran;
//        oCmd.ExecuteNonQuery();
//    }

//    public void UpdateData(SqlConnection oConn, SqlTransaction oTran)
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.Append(@"update 天然氣_異常整流站 set  
//年度=@年度,
//異常整流站名稱=@異常整流站名稱,
//異常起始日期年月=@異常起始日期年月,
//異常狀況=@異常狀況,
//整流站修復進度=@整流站修復進度,
//影響長途管線識別碼=@影響長途管線識別碼,
//預計完成日期=@預計完成日期,
//備註=@備註,
//修改者=@修改者, 
//修改日期=@修改日期 
//where guid=@guid and 資料狀態=@資料狀態 
// ");
//        SqlCommand oCmd = oConn.CreateCommand();
//        oCmd.CommandText = sb.ToString();

//        oCmd.Parameters.AddWithValue("@guid", guid);
//        oCmd.Parameters.AddWithValue("@年度", 年度);
//        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
//        oCmd.Parameters.AddWithValue("@異常整流站名稱", 異常整流站名稱);
//        oCmd.Parameters.AddWithValue("@異常起始日期年月", 異常起始日期年月);
//        oCmd.Parameters.AddWithValue("@異常狀況", 異常狀況);
//        oCmd.Parameters.AddWithValue("@整流站修復進度", 整流站修復進度);
//        oCmd.Parameters.AddWithValue("@影響長途管線識別碼", 影響長途管線識別碼);
//        oCmd.Parameters.AddWithValue("@預計完成日期", 預計完成日期);
//        oCmd.Parameters.AddWithValue("@備註", 備註);
//        oCmd.Parameters.AddWithValue("@修改者", 修改者);
//        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
//        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

//        oCmd.Transaction = oTran;
//        oCmd.ExecuteNonQuery();
//    }

//    public void DeleteData()
//    {
//        SqlCommand oCmd = new SqlCommand();
//        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
//        oCmd.CommandText = @"update 天然氣_異常整流站 set 
//修改日期=@修改日期, 
//修改者=@修改者, 
//資料狀態='D' 
//where guid=@guid ";

//        oCmd.CommandType = CommandType.Text;
//        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
//        oCmd.Parameters.AddWithValue("@guid", guid);
//        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
//        oCmd.Parameters.AddWithValue("@修改者", 修改者);

//        oCmd.Connection.Open();
//        oCmd.ExecuteNonQuery();
//        oCmd.Connection.Close();
//    }
}