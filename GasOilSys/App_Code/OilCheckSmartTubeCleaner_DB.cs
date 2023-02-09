using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilCheckSmartTubeCleaner_DB 的摘要描述
/// </summary>
public class OilCheckSmartTubeCleaner_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 年度 = string.Empty;
    string 業者guid = string.Empty;
    string 長途管線識別碼 = string.Empty;
    string 檢測方法 = string.Empty;
    string 最近一次執行年月 = string.Empty;
    string 報告產出年月 = string.Empty;
    string 檢測長度公里 = string.Empty;
    string 減薄數量_內1 = string.Empty;
    string 減薄數量_內_開挖確認1 = string.Empty;
    string 減薄數量_外 = string.Empty;
    string 減薄數量_外_開挖確認1 = string.Empty;
    string 減薄數量_內2 = string.Empty;
    string 減薄數量_內_開挖確認2 = string.Empty;
    string 減薄數量_外2 = string.Empty;
    string 減薄數量_外_開挖確認2 = string.Empty;
    string 減薄數量_內3 = string.Empty;
    string 減薄數量_內_開挖確認3 = string.Empty;
    string 減薄數量_外3 = string.Empty;
    string 減薄數量_外_開挖確認3 = string.Empty;
    string Dent = string.Empty;
    string Dent_開挖確認 = string.Empty;
    string 外部腐蝕保護電位符合標準要求數量 = string.Empty;
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
    public string _年度 { set { 年度 = value; } }
    public string _業者guid { set { 業者guid = value; } }
    public string _長途管線識別碼 { set { 長途管線識別碼 = value; } }
    public string _檢測方法 { set { 檢測方法 = value; } }
    public string _最近一次執行年月 { set { 最近一次執行年月 = value; } }
    public string _報告產出年月 { set { 報告產出年月 = value; } }
    public string _檢測長度公里 { set { 檢測長度公里 = value; } }
    public string _減薄數量_內1 { set { 減薄數量_內1 = value; } }
    public string _減薄數量_內_開挖確認1 { set { 減薄數量_內_開挖確認1 = value; } }
    public string _減薄數量_外 { set { 減薄數量_外 = value; } }
    public string _減薄數量_外_開挖確認1 { set { 減薄數量_外_開挖確認1 = value; } }
    public string _減薄數量_內2 { set { 減薄數量_內2 = value; } }
    public string _減薄數量_內_開挖確認2 { set { 減薄數量_內_開挖確認2 = value; } }
    public string _減薄數量_外2 { set { 減薄數量_外2 = value; } }
    public string _減薄數量_外_開挖確認2 { set { 減薄數量_外_開挖確認2 = value; } }
    public string _減薄數量_內3 { set { 減薄數量_內3 = value; } }
    public string _減薄數量_內_開挖確認3 { set { 減薄數量_內_開挖確認3 = value; } }
    public string _減薄數量_外3 { set { 減薄數量_外3 = value; } }
    public string _減薄數量_外_開挖確認3 { set { 減薄數量_外_開挖確認3 = value; } }
    public string _Dent { set { Dent = value; } }
    public string _Dent_開挖確認 { set { Dent_開挖確認 = value; } }
    public string _外部腐蝕保護電位符合標準要求數量 { set { 外部腐蝕保護電位符合標準要求數量 = value; } }
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

        sb.Append(@"select * from 石油_智慧型通管器檢查表 where 資料狀態='A' and 業者guid=@業者guid ");
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

select DISTINCT 年度 into #tmp from 石油_智慧型通管器檢查表
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

        sb.Append(@"select * from 石油_智慧型通管器檢查表 where guid=@guid and 資料狀態='A' ");

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
        sb.Append(@"insert into 石油_智慧型通管器檢查表(  
年度,
業者guid,
長途管線識別碼,
檢測方法,
最近一次執行年月,
報告產出年月,
檢測長度公里,
減薄數量_內1,
減薄數量_內_開挖確認1,
減薄數量_外,
減薄數量_外_開挖確認1,
減薄數量_內2,
減薄數量_內_開挖確認2,
減薄數量_外2,
減薄數量_外_開挖確認2,
減薄數量_內3,
減薄數量_內_開挖確認3,
減薄數量_外3,
減薄數量_外_開挖確認3,
Dent,
Dent_開挖確認,
外部腐蝕保護電位符合標準要求數量,
備註,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@長途管線識別碼,
@檢測方法,
@最近一次執行年月,
@報告產出年月,
@檢測長度公里,
@減薄數量_內1,
@減薄數量_內_開挖確認1,
@減薄數量_外,
@減薄數量_外_開挖確認1,
@減薄數量_內2,
@減薄數量_內_開挖確認2,
@減薄數量_外2,
@減薄數量_外_開挖確認2,
@減薄數量_內3,
@減薄數量_內_開挖確認3,
@減薄數量_外3,
@減薄數量_外_開挖確認3,
@Dent,
@Dent_開挖確認,
@外部腐蝕保護電位符合標準要求數量,
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
        oCmd.Parameters.AddWithValue("@檢測方法", 檢測方法);
        oCmd.Parameters.AddWithValue("@最近一次執行年月", 最近一次執行年月);
        oCmd.Parameters.AddWithValue("@報告產出年月", 報告產出年月);
        oCmd.Parameters.AddWithValue("@檢測長度公里", 檢測長度公里);
        oCmd.Parameters.AddWithValue("@減薄數量_內1", 減薄數量_內1);
        oCmd.Parameters.AddWithValue("@減薄數量_內_開挖確認1", 減薄數量_內_開挖確認1);
        oCmd.Parameters.AddWithValue("@減薄數量_外", 減薄數量_外);
        oCmd.Parameters.AddWithValue("@減薄數量_外_開挖確認1", 減薄數量_外_開挖確認1);
        oCmd.Parameters.AddWithValue("@減薄數量_內2", 減薄數量_內2);
        oCmd.Parameters.AddWithValue("@減薄數量_內_開挖確認2", 減薄數量_內_開挖確認2);
        oCmd.Parameters.AddWithValue("@減薄數量_外2", 減薄數量_外2);
        oCmd.Parameters.AddWithValue("@減薄數量_外_開挖確認2", 減薄數量_外_開挖確認2);
        oCmd.Parameters.AddWithValue("@減薄數量_內3", 減薄數量_內3);
        oCmd.Parameters.AddWithValue("@減薄數量_內_開挖確認3", 減薄數量_內_開挖確認3);
        oCmd.Parameters.AddWithValue("@減薄數量_外3", 減薄數量_外3);
        oCmd.Parameters.AddWithValue("@減薄數量_外_開挖確認3", 減薄數量_外_開挖確認3);
        oCmd.Parameters.AddWithValue("@Dent", Dent);
        oCmd.Parameters.AddWithValue("@Dent_開挖確認", Dent_開挖確認);
        oCmd.Parameters.AddWithValue("@外部腐蝕保護電位符合標準要求數量", 外部腐蝕保護電位符合標準要求數量);
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
        sb.Append(@"update 石油_智慧型通管器檢查表 set  
年度=@年度,
長途管線識別碼=@長途管線識別碼,
檢測方法=@檢測方法,
最近一次執行年月=@最近一次執行年月,
報告產出年月=@報告產出年月,
檢測長度公里=@檢測長度公里,
減薄數量_內1=@減薄數量_內1,
減薄數量_內_開挖確認1=@減薄數量_內_開挖確認1,
減薄數量_外=@減薄數量_外,
減薄數量_外_開挖確認1=@減薄數量_外_開挖確認1,
減薄數量_內2=@減薄數量_內2,
減薄數量_內_開挖確認2=@減薄數量_內_開挖確認2,
減薄數量_外2=@減薄數量_外2,
減薄數量_外_開挖確認2=@減薄數量_外_開挖確認2,
減薄數量_內3=@減薄數量_內3,
減薄數量_內_開挖確認3=@減薄數量_內_開挖確認3,
減薄數量_外3=@減薄數量_外3,
減薄數量_外_開挖確認3=@減薄數量_外_開挖確認3,
Dent=@Dent,
Dent_開挖確認=@Dent_開挖確認,
外部腐蝕保護電位符合標準要求數量=@外部腐蝕保護電位符合標準要求數量,
備註=@備註,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@檢測方法", 檢測方法);
        oCmd.Parameters.AddWithValue("@最近一次執行年月", 最近一次執行年月);
        oCmd.Parameters.AddWithValue("@報告產出年月", 報告產出年月);
        oCmd.Parameters.AddWithValue("@檢測長度公里", 檢測長度公里);
        oCmd.Parameters.AddWithValue("@減薄數量_內1", 減薄數量_內1);
        oCmd.Parameters.AddWithValue("@減薄數量_內_開挖確認1", 減薄數量_內_開挖確認1);
        oCmd.Parameters.AddWithValue("@減薄數量_外", 減薄數量_外);
        oCmd.Parameters.AddWithValue("@減薄數量_外_開挖確認1", 減薄數量_外_開挖確認1);
        oCmd.Parameters.AddWithValue("@減薄數量_內2", 減薄數量_內2);
        oCmd.Parameters.AddWithValue("@減薄數量_內_開挖確認2", 減薄數量_內_開挖確認2);
        oCmd.Parameters.AddWithValue("@減薄數量_外2", 減薄數量_外2);
        oCmd.Parameters.AddWithValue("@減薄數量_外_開挖確認2", 減薄數量_外_開挖確認2);
        oCmd.Parameters.AddWithValue("@減薄數量_內3", 減薄數量_內3);
        oCmd.Parameters.AddWithValue("@減薄數量_內_開挖確認3", 減薄數量_內_開挖確認3);
        oCmd.Parameters.AddWithValue("@減薄數量_外3", 減薄數量_外3);
        oCmd.Parameters.AddWithValue("@減薄數量_外_開挖確認3", 減薄數量_外_開挖確認3);
        oCmd.Parameters.AddWithValue("@Dent", Dent);
        oCmd.Parameters.AddWithValue("@Dent_開挖確認", Dent_開挖確認);
        oCmd.Parameters.AddWithValue("@外部腐蝕保護電位符合標準要求數量", 外部腐蝕保護電位符合標準要求數量);
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
        oCmd.CommandText = @"update 石油_智慧型通管器檢查表 set 
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