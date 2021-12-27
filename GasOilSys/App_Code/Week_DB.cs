using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Week_DB 的摘要描述
/// </summary>
public class Week_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 工作項次 = string.Empty;
    string 執行內容 = string.Empty;
    string 預定日期 = string.Empty;
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _guid { set { guid = value; } }
    public string _工作項次 { set { 工作項次 = value; } }
    public string _執行內容 { set { 執行內容 = value; } }
    public string _預定日期 { set { 預定日期 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    #endregion

    public DataTable GetWeekExport(string firstday, string endday)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select * from 週報列表 
where CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) between CONVERT(datetime,@firstday) and CONVERT(datetime,@endday) and 資料狀態='A' 
order by 工作項次, CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) 
");

        oCmd.CommandText = sb.ToString();
        oCmd.Parameters.AddWithValue("@firstday", firstday);
        oCmd.Parameters.AddWithValue("@endday", endday);
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetListThisWeek()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @當日日期 datetime
declare @當周第一天 datetime
declare @當周最後一天 datetime

set @當日日期=GETDATE()
set @當周第一天= dateadd(day,2-(datepart(weekday,@當日日期)),CONVERT(datetime, @當日日期))
set @當周最後一天= dateadd(day,2-(datepart(weekday,@當日日期))+6,@當日日期)

select CONVERT(varchar, @當周第一天, 102) as 星期一, CONVERT(varchar, @當周最後一天, 102) as 星期日

select * from 週報列表 
where CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) between @當周第一天 and @當周最後一天 and 資料狀態='A' 
order by 工作項次, CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) 
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetListNextWeek()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @當日日期 datetime
declare @下周第一天 datetime
declare @下周最後一天 datetime

set @當日日期= DATEADD(WEEK, 1, GETDATE())
set @下周第一天= dateadd(day,2-(datepart(weekday,@當日日期)),CONVERT(datetime, @當日日期))
set @下周最後一天= dateadd(day,2-(datepart(weekday,@當日日期))+6,@當日日期)

select CONVERT(varchar, @下周第一天, 102) as 星期一, CONVERT(varchar, @下周最後一天, 102) as 星期日

select * from 週報列表 
where CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) between @下周第一天 and @下周最後一天 and 資料狀態='A' 
order by 工作項次, CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) 
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oda.Fill(ds);
        return ds;
    }

    public DataTable SearchWeek(string startTime, string endTime)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 週報列表 where "); 

        if(!string.IsNullOrEmpty(startTime) || !string.IsNullOrEmpty(endTime))
        {
            sb.Append(@"
CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) between 
CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , @startTime)+19110000)) and 
CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , @endTime)+19110000)) and 資料狀態 = 'A' 
");
        }
        else
        {
            sb.Append(@"資料狀態 = 'A' ");
        }

        sb.Append(@" 
order by 工作項次, CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) 
");

        oCmd.CommandText = sb.ToString();
        oCmd.Parameters.AddWithValue("@startTime", startTime);
        oCmd.Parameters.AddWithValue("@endTime", endTime);
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetMonthList(string startTime)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @當月第一天 datetime
declare @當月最後一天 datetime

set @當月第一天 = CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , @startTime)+19110000))
set @當月最後一天 = dateadd(day ,-1, dateadd(m, datediff(m,0,@當月第一天)+1,0))

select * from 週報列表 where 
CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) between 
@當月第一天 and @當月最後一天 and 資料狀態 = 'A' 
order by 工作項次, CONVERT(datetime, CONVERT(nvarchar, CONVERT(int, 預定日期) + 19110000)) 
");

        oCmd.CommandText = sb.ToString();
        oCmd.Parameters.AddWithValue("@startTime", startTime);
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetSeasonList(string season, string year)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @當季第一天 datetime
declare @當季最後一天 datetime

if(@season = '01')
    begin
        set @當季第一天 = CONVERT(nvarchar, CONVERT(nvarchar, CONVERT(int, @year) + 1911) + '0101')
        set @當季最後一天 = CONVERT(nvarchar, CONVERT(nvarchar, CONVERT(int, @year) + 1911) + '0331')
    end
if(@season = '02')
    begin
        set @當季第一天 = CONVERT(nvarchar, CONVERT(nvarchar, CONVERT(int, @year) + 1911) + '0401')
        set @當季最後一天 = CONVERT(nvarchar, CONVERT(nvarchar, CONVERT(int, @year) + 1911) + '0630')
    end
if(@season = '03')
    begin
        set @當季第一天 = CONVERT(nvarchar, CONVERT(nvarchar, CONVERT(int, @year) + 1911) + '0701')
        set @當季最後一天 = CONVERT(nvarchar, CONVERT(nvarchar, CONVERT(int, @year) + 1911) + '0930')
    end
if(@season = '04')
    begin
        set @當季第一天 = CONVERT(nvarchar, CONVERT(nvarchar, CONVERT(int, @year) + 1911) + '1001')
        set @當季最後一天 = CONVERT(nvarchar, CONVERT(nvarchar, CONVERT(int, @year) + 1911) + '1231')
    end

select * from 週報列表 where 
CONVERT(datetime, CONVERT(nvarchar, CONVERT(int , 預定日期)+19110000)) between 
@當季第一天 and @當季最後一天 and 資料狀態 = 'A' 
order by 工作項次, CONVERT(datetime, CONVERT(nvarchar, CONVERT(int, 預定日期) + 19110000)) 
");

        oCmd.CommandText = sb.ToString();
        oCmd.Parameters.AddWithValue("@season", season);
        oCmd.Parameters.AddWithValue("@year", year);
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetYearList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select SUBSTRING(預定日期, 1, 3) as year 
from 週報列表
group by SUBSTRING(預定日期, 1, 3) 
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public void SaveWeek(SqlConnection oConn, SqlTransaction oTran, string mode)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
if @mode = 'new'
    begin
        insert into 週報列表(guid,
        工作項次,
        執行內容,
        預定日期,
        建立者,
        建立日期,
        修改者,
        修改日期,
        資料狀態 ) values (@guid,
        @工作項次,
        @執行內容,
        @預定日期,
        @建立者,
        @建立日期,
        @修改者,
        @修改日期,
        @資料狀態 )
    end
else
    begin
        update 週報列表 set
        執行內容=@執行內容,
        預定日期=@預定日期,
        修改者=@修改者,
        修改日期=@修改日期
        where guid=@guid and 資料狀態=@資料狀態
    end 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@mode", mode);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@工作項次", 工作項次);
        oCmd.Parameters.AddWithValue("@執行內容", 執行內容);
        oCmd.Parameters.AddWithValue("@預定日期", 預定日期);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DeleteWeek(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 週報列表 set
            資料狀態=@資料狀態,
            修改者=@修改者,
            修改日期=@修改日期
            where guid=@guid  
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "D");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }
}