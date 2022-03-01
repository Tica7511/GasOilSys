using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
/// <summary>
/// OilMasterCompare_DB 的摘要描述
/// </summary>
public class OilMasterCompare_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }

    #region private
    string id = string.Empty;
    string 年度 = string.Empty;
    string 委員guid = string.Empty;
    string 委員姓名 = string.Empty;
    string 業者guid = string.Empty;
    string 業者單位名稱 = string.Empty;
    string 負責評分類別 = string.Empty;
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _年度 { set { 年度 = value; } }
    public string _委員guid { set { 委員guid = value; } }
    public string _委員姓名 { set { 委員姓名 = value; } }
    public string _業者guid { set { 業者guid = value; } }
    public string _業者單位名稱 { set { 業者單位名稱 = value; } }
    public string _負責評分類別 { set { 負責評分類別 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    #endregion

    public DataTable GetMasterType()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select a.業者Guid,b.網站類別 from 石油_委員業者年度對應表 a
left join 會員檔 b on b.業者guid=a.業者Guid
where a.資料狀態='A' and a.委員guid=@委員guid and a.年度=@年度
group by a.業者Guid,b.網站類別 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@委員guid", 委員guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetMasterList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select 
       b.guid
      ,b.父層guid
      ,b.排序編號
      ,b.公司名稱
      ,b.處
      ,b.事業部
      ,b.營業處廠
      ,b.組
      ,b.中心庫區儲運課工場
      ,b.石油組織階層
      ,b.電話
      ,b.地址
      ,b.儲槽數量
      ,b.管線數量
      ,b.維運計畫書及成果報告
      ,b.曾執行過查核日期
      ,b.建立者
      ,b.建立日期
      ,b.修改者
      ,b.修改日期
      ,b.資料狀態
      ,b.列表是否顯示
from 石油_委員業者年度對應表 a
left join 石油_業者基本資料 b on b.guid=a.業者Guid
where a.資料狀態='A' and a.委員guid=@委員guid and a.年度=年度 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@委員guid", 委員guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCommitteeList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select ROW_NUMBER() over (order by 委員姓名) itemNo,*
from 石油_委員業者年度對應表 
where 資料狀態='A' and 業者guid=@業者guid and 年度=@年度 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCommitteeData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_委員業者年度對應表 
where 資料狀態='A' and 業者guid=@業者guid and 委員guid=@委員guid and 年度=@年度 ");

        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@委員guid", 委員guid);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oCmd.Transaction = oTran;
        oda.Fill(ds);
        oCmd.ExecuteNonQuery();
        return ds;
    }

    public DataTable GetYearList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"  
declare @yearCount int

select DISTINCT 年度 into #tmp from 石油_委員業者年度對應表
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

    public void InsertData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 石油_委員業者年度對應表(  
年度,
委員guid,
委員姓名,
業者guid,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@委員guid,
@委員姓名,
@業者guid,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@委員guid", 委員guid);
        oCmd.Parameters.AddWithValue("@委員姓名", 委員姓名);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DeleteData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"
        update 石油_委員業者年度對應表 set 
        修改日期=@修改日期, 
        修改者=@修改者, 
        資料狀態='D' 
        where 業者guid=@業者guid and 委員guid=@委員guid and 年度=@年度 ";

        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@委員guid", 委員guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }
}