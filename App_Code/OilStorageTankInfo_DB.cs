using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilStorageTankInfo_DB 的摘要描述
/// </summary>
public class OilStorageTankInfo_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 業者名稱 = string.Empty;
    string 年度 = string.Empty;
    string 儲槽guid = string.Empty;
    string 轄區儲槽編號 = string.Empty;
    string 能源局編號 = string.Empty;
    string 容量 = string.Empty;
    string 內徑 = string.Empty;
    string 內容物 = string.Empty;
    string 形式 = string.Empty;
    string 啟用日期 = string.Empty;
    string 代行檢查_代檢機構1 = string.Empty;
    string 代行檢查_外部日期1 = string.Empty;
    string 代行檢查_代檢機構2 = string.Empty;
    string 代行檢查_外部日期2 = string.Empty;
    string 狀態 = string.Empty;
    string 延長開放年限 = string.Empty;
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
    public string _業者名稱 { set { 業者名稱 = value; } }
    public string _年度 { set { 年度 = value; } }
    public string _儲槽guid { set { 儲槽guid = value; } }
    public string _轄區儲槽編號 { set { 轄區儲槽編號 = value; } }
    public string _能源局編號 { set { 能源局編號 = value; } }
    public string _容量 { set { 容量 = value; } }
    public string _內徑 { set { 內徑 = value; } }
    public string _內容物 { set { 內容物 = value; } }
    public string _形式 { set { 形式 = value; } }
    public string _啟用日期 { set { 啟用日期 = value; } }
    public string _代行檢查_代檢機構1 { set { 代行檢查_代檢機構1 = value; } }
    public string _代行檢查_外部日期1 { set { 代行檢查_外部日期1 = value; } }
    public string _代行檢查_代檢機構2 { set { 代行檢查_代檢機構2 = value; } }
    public string _代行檢查_外部日期2 { set { 代行檢查_外部日期2 = value; } }
    public string _狀態 { set { 狀態 = value; } }
    public string _延長開放年限 { set { 延長開放年限 = value; } }
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

        sb.Append(@"select * from 石油_儲槽基本資料 where 資料狀態='A' and 業者guid=@業者guid ");
        if (!string.IsNullOrEmpty(年度))
            sb.Append(@" and 年度=@年度");
        if (!string.IsNullOrEmpty(轄區儲槽編號))
            sb.Append(@" and 轄區儲槽編號=@轄區儲槽編號");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@轄區儲槽編號", 轄區儲槽編號);

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

select DISTINCT 年度 into #tmp from 石油_儲槽基本資料
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

        sb.Append(@"select * from 石油_儲槽基本資料 where guid=@guid and 資料狀態='A' ");

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
        sb.Append(@"insert into 石油_儲槽基本資料(  
年度,
業者guid,
業者名稱,
轄區儲槽編號,
能源局編號,
容量,
內徑,
內容物,
形式,
啟用日期,
代行檢查_代檢機構1,
代行檢查_外部日期1,
代行檢查_代檢機構2,
代行檢查_外部日期2,
狀態,
延長開放年限,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@業者名稱,
@轄區儲槽編號,
@能源局編號,
@容量,
@內徑,
@內容物,
@形式,
@啟用日期,
@代行檢查_代檢機構1,
@代行檢查_外部日期1,
@代行檢查_代檢機構2,
@代行檢查_外部日期2,
@狀態,
@延長開放年限,
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
        oCmd.Parameters.AddWithValue("@業者名稱", 業者名稱);
        oCmd.Parameters.AddWithValue("@轄區儲槽編號", 轄區儲槽編號);
        oCmd.Parameters.AddWithValue("@能源局編號", 能源局編號);
        oCmd.Parameters.AddWithValue("@容量", 容量);
        oCmd.Parameters.AddWithValue("@內徑", 內徑);
        oCmd.Parameters.AddWithValue("@內容物", 內容物);
        oCmd.Parameters.AddWithValue("@形式", 形式);
        oCmd.Parameters.AddWithValue("@啟用日期", 啟用日期);
        oCmd.Parameters.AddWithValue("@代行檢查_代檢機構1", 代行檢查_代檢機構1);
        oCmd.Parameters.AddWithValue("@代行檢查_外部日期1", 代行檢查_外部日期1);
        oCmd.Parameters.AddWithValue("@代行檢查_代檢機構2", 代行檢查_代檢機構2);
        oCmd.Parameters.AddWithValue("@代行檢查_外部日期2", 代行檢查_外部日期2);
        oCmd.Parameters.AddWithValue("@狀態", 狀態);
        oCmd.Parameters.AddWithValue("@延長開放年限", 延長開放年限);
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
        sb.Append(@"update 石油_儲槽基本資料 set  
年度=@年度,
轄區儲槽編號=@轄區儲槽編號,
能源局編號=@能源局編號,
容量=@容量,
內徑=@內徑,
內容物=@內容物,
形式=@形式,
啟用日期=@啟用日期,
代行檢查_代檢機構1=@代行檢查_代檢機構1,
代行檢查_外部日期1=@代行檢查_外部日期1,
代行檢查_代檢機構2=@代行檢查_代檢機構2,
代行檢查_外部日期2=@代行檢查_外部日期2,
狀態=@狀態,
延長開放年限=@延長開放年限,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@轄區儲槽編號", 轄區儲槽編號);
        oCmd.Parameters.AddWithValue("@能源局編號", 能源局編號);
        oCmd.Parameters.AddWithValue("@容量", 容量);
        oCmd.Parameters.AddWithValue("@內徑", 內徑);
        oCmd.Parameters.AddWithValue("@內容物", 內容物);
        oCmd.Parameters.AddWithValue("@形式", 形式);
        oCmd.Parameters.AddWithValue("@啟用日期", 啟用日期);
        oCmd.Parameters.AddWithValue("@代行檢查_代檢機構1", 代行檢查_代檢機構1);
        oCmd.Parameters.AddWithValue("@代行檢查_外部日期1", 代行檢查_外部日期1);
        oCmd.Parameters.AddWithValue("@代行檢查_代檢機構2", 代行檢查_代檢機構2);
        oCmd.Parameters.AddWithValue("@代行檢查_外部日期2", 代行檢查_外部日期2);
        oCmd.Parameters.AddWithValue("@狀態", 狀態);
        oCmd.Parameters.AddWithValue("@延長開放年限", 延長開放年限);
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
        oCmd.CommandText = @"update 石油_儲槽基本資料 set 
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