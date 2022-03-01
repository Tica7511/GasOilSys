using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasStorageTankInfo_DB 的摘要描述
/// </summary>
public class GasStorageTankInfo_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;
    string 庫區特殊區域 = string.Empty;
    string 庫區特殊區域_其他 = string.Empty;
    string 內容 = string.Empty;
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;

    // 儲槽基本資料
    string 液化天然氣廠 = string.Empty;
    string 儲槽編號 = string.Empty;
    string 容量 = string.Empty;
    string 外徑 = string.Empty;
    string 高度 = string.Empty;
    string 形式 = string.Empty;
    string 啟用日期 = string.Empty;
    string 狀態 = string.Empty;
    string 勞動部檢查 = string.Empty;
    string 代行檢查機構 = string.Empty;

    // 儲槽設備查核資料
    string 儲氣設備 = string.Empty;
    string 查核項目 = string.Empty;
    string 業者填寫 = string.Empty;
    string 佐證資料 = string.Empty;


    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _guid { set { guid = value; } }
    public string _業者guid { set { 業者guid = value; } }
    public string _年度 { set { 年度 = value; } }
    public string _庫區特殊區域 { set { 庫區特殊區域 = value; } }
    public string _庫區特殊區域_其他 { set { 庫區特殊區域_其他 = value; } }
    public string _內容 { set { 內容 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }

    // 儲槽基本資料
    public string _液化天然氣廠 { set { 液化天然氣廠 = value; } }
    public string _儲槽編號 { set { 儲槽編號 = value; } }
    public string _容量 { set { 容量 = value; } }
    public string _外徑 { set { 外徑 = value; } }
    public string _高度 { set { 高度 = value; } }
    public string _形式 { set { 形式 = value; } }
    public string _啟用日期 { set { 啟用日期 = value; } }
    public string _狀態 { set { 狀態 = value; } }
    public string _勞動部檢查 { set { 勞動部檢查 = value; } }
    public string _代行檢查機構 { set { 代行檢查機構 = value; } }

    // 儲槽設備查核資料
    public string _儲氣設備 { set { 儲氣設備 = value; } }
    public string _查核項目 { set { 查核項目 = value; } }
    public string _業者填寫 { set { 業者填寫 = value; } }
    public string _佐證資料 { set { 佐證資料 = value; } }
    #endregion

    public DataTable GetList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 天然氣_儲槽設施資料_儲槽基本資料 where 資料狀態='A' and 業者guid=@業者guid ");
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

    public DataTable GetList2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 天然氣_儲槽設施資料 where 資料狀態='A' and 業者guid=@業者guid ");
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

    public DataTable GetList3()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 天然氣_儲槽設施資料_儲槽設備查核資料 where 資料狀態='A' and 業者guid=@業者guid ");
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

select DISTINCT 年度 into #tmp from 天然氣_儲槽設施資料_儲槽基本資料
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

        sb.Append(@"select * from 天然氣_儲槽設施資料_儲槽基本資料 where guid=@guid and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 天然氣_儲槽設施資料 where guid=@guid and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData3()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 天然氣_儲槽設施資料_儲槽設備查核資料 where guid=@guid and 資料狀態='A' ");

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
        sb.Append(@"insert into 天然氣_儲槽設施資料_儲槽基本資料(  
年度,
業者guid,
液化天然氣廠,
儲槽編號,
容量,
外徑,
高度,
形式,
啟用日期,
狀態,
勞動部檢查,
代行檢查機構,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@液化天然氣廠,
@儲槽編號,
@容量,
@外徑,
@高度,
@形式,
@啟用日期,
@狀態,
@勞動部檢查,
@代行檢查機構,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@年度", "110");
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@液化天然氣廠", 液化天然氣廠);
        oCmd.Parameters.AddWithValue("@儲槽編號", 儲槽編號);
        oCmd.Parameters.AddWithValue("@容量", 容量);
        oCmd.Parameters.AddWithValue("@外徑", 外徑);
        oCmd.Parameters.AddWithValue("@高度", 高度);
        oCmd.Parameters.AddWithValue("@形式", 形式);
        oCmd.Parameters.AddWithValue("@啟用日期", 啟用日期);
        oCmd.Parameters.AddWithValue("@狀態", 狀態);
        oCmd.Parameters.AddWithValue("@勞動部檢查", 勞動部檢查);
        oCmd.Parameters.AddWithValue("@代行檢查機構", 代行檢查機構);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void InsertData2(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 天然氣_儲槽設施資料(  
年度,
業者guid,
庫區特殊區域,
庫區特殊區域_其他,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@庫區特殊區域,
@庫區特殊區域_其他,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@年度", "110");
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@庫區特殊區域", 庫區特殊區域);
        oCmd.Parameters.AddWithValue("@庫區特殊區域_其他", 庫區特殊區域_其他);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void InsertData3(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 天然氣_儲槽設施資料_儲槽設備查核資料(  
年度,
業者guid,
儲氣設備,
查核項目,
業者填寫,
佐證資料,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@儲氣設備,
@查核項目,
@業者填寫,
@佐證資料,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@年度", "110");
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@儲氣設備", 儲氣設備);
        oCmd.Parameters.AddWithValue("@查核項目", 查核項目);
        oCmd.Parameters.AddWithValue("@業者填寫", 業者填寫);
        oCmd.Parameters.AddWithValue("@佐證資料", 佐證資料);
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
        sb.Append(@"update 天然氣_儲槽設施資料_儲槽基本資料 set  
年度=@年度,
液化天然氣廠=@液化天然氣廠,
儲槽編號=@儲槽編號,
容量=@容量,
外徑=@外徑,
高度=@高度,
形式=@形式,
啟用日期=@啟用日期,
狀態=@狀態,
勞動部檢查=@勞動部檢查,
代行檢查機構=@代行檢查機構,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", "110");
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@液化天然氣廠", 液化天然氣廠);
        oCmd.Parameters.AddWithValue("@儲槽編號", 儲槽編號);
        oCmd.Parameters.AddWithValue("@容量", 容量);
        oCmd.Parameters.AddWithValue("@外徑", 外徑);
        oCmd.Parameters.AddWithValue("@高度", 高度);
        oCmd.Parameters.AddWithValue("@形式", 形式);
        oCmd.Parameters.AddWithValue("@啟用日期", 啟用日期);
        oCmd.Parameters.AddWithValue("@狀態", 狀態);
        oCmd.Parameters.AddWithValue("@勞動部檢查", 勞動部檢查);
        oCmd.Parameters.AddWithValue("@代行檢查機構", 代行檢查機構);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData2(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 天然氣_儲槽設施資料 set  
年度=@年度,
庫區特殊區域=@庫區特殊區域,
庫區特殊區域_其他=@庫區特殊區域_其他,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", "110");
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@庫區特殊區域", 庫區特殊區域);
        oCmd.Parameters.AddWithValue("@庫區特殊區域_其他", 庫區特殊區域_其他);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData3(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 天然氣_儲槽設施資料_儲槽設備查核資料 set  
年度=@年度,
儲氣設備=@儲氣設備,
查核項目=@查核項目,
業者填寫=@業者填寫,
佐證資料=@佐證資料,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", "110");
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@儲氣設備", 儲氣設備);
        oCmd.Parameters.AddWithValue("@查核項目", 查核項目);
        oCmd.Parameters.AddWithValue("@業者填寫", 業者填寫);
        oCmd.Parameters.AddWithValue("@佐證資料", 佐證資料);
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
        oCmd.CommandText = @"update 天然氣_儲槽設施資料_儲槽基本資料 set 
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

    public void DeleteData2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 天然氣_儲槽設施資料_儲槽設備查核資料 set 
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