﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilCompanyInfo_DB 的摘要描述
/// </summary>
public class OilCompanyInfo_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 父層guid = string.Empty;
	string 年度 = string.Empty;
	int 排序編號;
	string 公司名稱 = string.Empty;
	string 處 = string.Empty;
	string 事業部 = string.Empty;
	string 營業處廠 = string.Empty;
	string 組 = string.Empty;
	string 中心庫區儲運課工場 = string.Empty;
	string 石油組織階層 = string.Empty;
	string 電話 = string.Empty;
	string 地址 = string.Empty;
	string 儲槽數量 = string.Empty;
	string 儲槽容量 = string.Empty;
	string 管線數量 = string.Empty;
	string 管線長度 = string.Empty;
	string 維運計畫書及成果報告 = string.Empty;
	string 曾執行過查核日期 = string.Empty;
	string 年度查核姓名 = string.Empty;
	string 年度查核職稱 = string.Empty;
	string 年度查核分機 = string.Empty;
	string 年度查核email = string.Empty;
    string 年度檢測姓名 = string.Empty;
    string 年度檢測職稱 = string.Empty;
    string 年度檢測分機 = string.Empty;
    string 年度檢測email = string.Empty;
    string 資料是否確認 = string.Empty;
	string 年度儲槽確認 = string.Empty;
	string 年度液化石油氣儲槽確認 = string.Empty;
	string 建立者 = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;
	#endregion
	#region public
	public string _id { set { id = value; } }
	public string _guid { set { guid = value; } }
	public string _父層guid { set { 父層guid = value; } }
	public string _年度 { set { 年度 = value; } }
	public int _排序編號 { set { 排序編號 = value; } }
	public string _公司名稱 { set { 公司名稱 = value; } }
	public string _處 { set { 處 = value; } }
	public string _事業部 { set { 事業部 = value; } }
	public string _營業處廠 { set { 營業處廠 = value; } }
	public string _組 { set { 組 = value; } }
	public string _中心庫區儲運課工場 { set { 中心庫區儲運課工場 = value; } }
	public string _石油組織階層 { set { 石油組織階層 = value; } }
	public string _電話 { set { 電話 = value; } }
	public string _地址 { set { 地址 = value; } }
	public string _儲槽數量 { set { 儲槽數量 = value; } }
	public string _儲槽容量 { set { 儲槽容量 = value; } }
	public string _管線數量 { set { 管線數量 = value; } }
	public string _管線長度 { set { 管線長度 = value; } }
	public string _維運計畫書及成果報告 { set { 維運計畫書及成果報告 = value; } }
	public string _曾執行過查核日期 { set { 曾執行過查核日期 = value; } }
	public string _年度查核姓名 { set { 年度查核姓名 = value; } }
	public string _年度查核職稱 { set { 年度查核職稱 = value; } }
	public string _年度查核分機 { set { 年度查核分機 = value; } }
	public string _年度查核email { set { 年度查核email = value; } }
    public string _年度檢測姓名 { set { 年度檢測姓名 = value; } }
    public string _年度檢測職稱 { set { 年度檢測職稱 = value; } }
    public string _年度檢測分機 { set { 年度檢測分機 = value; } }
    public string _年度檢測email { set { 年度檢測email = value; } }
    public string _資料是否確認 { set { 資料是否確認 = value; } }
	public string _年度儲槽確認 { set { 年度儲槽確認 = value; } }
	public string _年度液化石油氣儲槽確認 { set { 年度液化石油氣儲槽確認 = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }
	#endregion

	public DataTable GetInfo()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 石油_業者基本資料 where 資料狀態='A' and guid=@guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@guid", guid);

		oda.Fill(ds);
		return ds;
	}

    public DataTable GetInfoDetail()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select [guid]
      ,isnull(處,'')+isnull(事業部,'')+isnull(營業處廠,'')+isnull(組,'')+isnull(中心庫區儲運課工場,'') as 事業名稱
      ,[電話]
      ,[地址]
      ,[儲槽數量]
      ,[管線數量]
      ,[維運計畫書及成果報告]
      ,[曾執行過查核日期]
      ,[建立者]
      ,[建立日期]
      ,[修改者]
      ,[修改日期]
      ,[資料狀態]
      ,[列表是否顯示]
        from 石油_業者基本資料 where 資料狀態='A' and guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetInfoDetail2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * 
        from 石油_事業單位基本資料表  
		where 資料狀態='A' and 年度=@年度 and 業者guid=@業者guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", guid);
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

select DISTINCT 年度 into #tmp from 石油_事業單位基本資料表
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

        oCmd.Parameters.AddWithValue("@業者guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCpName()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select 公司名稱, isnull(處,'')+isnull(事業部,'')+isnull(營業處廠,'')+isnull(組,'')+isnull(中心庫區儲運課工場,'') as cpname, guid, 管線管理不顯示, 儲槽設施不顯示, 單位屬性, 資料是否確認, 單獨公司名稱, 年度儲槽確認, 年度液化石油氣儲槽確認 from 石油_業者基本資料
  where 資料狀態='A' and 列表是否顯示='Y' ");
        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid = @guid ");
        if (!string.IsNullOrEmpty(公司名稱))
            sb.Append(@"and 公司名稱 = @公司名稱 ");
        if (!string.IsNullOrEmpty(事業部))
            sb.Append(@"and 事業部 = @事業部 ");
        if (!string.IsNullOrEmpty(營業處廠))
            sb.Append(@"and 營業處廠 = @營業處廠 ");
        if (!string.IsNullOrEmpty(中心庫區儲運課工場))
            sb.Append(@"and 中心庫區儲運課工場 = @中心庫區儲運課工場 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@公司名稱", 公司名稱);
        oCmd.Parameters.AddWithValue("@事業部", 事業部);
        oCmd.Parameters.AddWithValue("@營業處廠", 營業處廠);
        oCmd.Parameters.AddWithValue("@中心庫區儲運課工場", 中心庫區儲運課工場);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetDistinctCpName(string selectName)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select distinct " + selectName);

        sb.Append(@" as showName from 石油_業者基本資料
  where 資料狀態='A' and 列表是否顯示='Y' and " + selectName + " <> '' and " + selectName + " is not null ");
        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid = @guid ");
        if (!string.IsNullOrEmpty(公司名稱))
            sb.Append(@"and 公司名稱 = @公司名稱 ");
        if (!string.IsNullOrEmpty(事業部))
            sb.Append(@"and 事業部 = @事業部 ");
        if (!string.IsNullOrEmpty(營業處廠))
            sb.Append(@"and 營業處廠 = @營業處廠 ");
        if (!string.IsNullOrEmpty(中心庫區儲運課工場))
            sb.Append(@"and 中心庫區儲運課工場 = @中心庫區儲運課工場 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@公司名稱", 公司名稱);
        oCmd.Parameters.AddWithValue("@事業部", 事業部);
        oCmd.Parameters.AddWithValue("@營業處廠", 營業處廠);
        oCmd.Parameters.AddWithValue("@中心庫區儲運課工場", 中心庫區儲運課工場);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCompanyByAddress()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"SELECT distinct convert(nvarchar(3),Left(地址,3)) COLLATE Chinese_Taiwan_Stroke_CS_AS as showName
FROM 石油_業者基本資料 where (地址 <>'' and 地址 is not null ) ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCountList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
  declare @管線數量總合 int
  declare @管線長度總合 float
  declare @儲槽數量總合 int
  declare @儲槽容量總合 float

  set @管線數量總合 = (select count(*) from 石油_管線基本資料 a left join 石油_業者基本資料 b on a.業者guid=b.guid 
  where (@公司名稱='' or b.公司名稱=@公司名稱) and (@事業部='' or b.事業部=@事業部) and (@營業處廠='' or b.營業處廠=@營業處廠) 
  and (@中心庫區儲運課工場='' or b.中心庫區儲運課工場=@中心庫區儲運課工場) and a.資料狀態='A' and b.資料狀態='A' and (@地址='' or left(b.地址,3) = @地址))  
  set @管線長度總合 = (select isnull(sum(convert(float, a.轄管長度)),0) from 石油_管線基本資料 a left join 石油_業者基本資料 b on a.業者guid=b.guid 
  where (@公司名稱='' or b.公司名稱=@公司名稱) and (@事業部='' or b.事業部=@事業部) and (@營業處廠='' or b.營業處廠=@營業處廠) 
  and (@中心庫區儲運課工場='' or b.中心庫區儲運課工場=@中心庫區儲運課工場) and a.資料狀態='A' and b.資料狀態='A' and (@地址='' or left(b.地址,3) = @地址)) 
  set @儲槽數量總合 = (select count(*) from 石油_儲槽基本資料 a left join 石油_業者基本資料 b on a.業者guid=b.guid 
  where (@公司名稱='' or b.公司名稱=@公司名稱) and (@事業部='' or b.事業部=@事業部) and (@營業處廠='' or b.營業處廠=@營業處廠) 
  and (@中心庫區儲運課工場='' or b.中心庫區儲運課工場=@中心庫區儲運課工場) and a.資料狀態='A' and b.資料狀態='A' and (@地址='' or left(b.地址,3) = @地址)) 
  set @儲槽容量總合 = (select isnull(sum(convert(float, a.容量)),0) from 石油_儲槽基本資料 a left join 石油_業者基本資料 b on a.業者guid=b.guid 
  where (@公司名稱='' or b.公司名稱=@公司名稱) and (@事業部='' or b.事業部=@事業部) and (@營業處廠='' or b.營業處廠=@營業處廠) 
  and (@中心庫區儲運課工場='' or b.中心庫區儲運課工場=@中心庫區儲運課工場) and a.資料狀態='A' and b.資料狀態='A' and (@地址='' or left(b.地址,3) = @地址)) 

  select @管線數量總合 as 管線數量總合, @管線長度總合 as 管線長度總合, @儲槽數量總合 as 儲槽數量總合, @儲槽容量總合 as 儲槽容量總合 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@公司名稱", 公司名稱);
        oCmd.Parameters.AddWithValue("@事業部", 事業部);
        oCmd.Parameters.AddWithValue("@營業處廠", 營業處廠);
        oCmd.Parameters.AddWithValue("@中心庫區儲運課工場", 中心庫區儲運課工場);
        oCmd.Parameters.AddWithValue("@地址", 地址);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCountAllList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
  declare @石油管線數量總合 int
  declare @石油管線長度總合 float
  declare @石油儲槽數量總合 int
  declare @石油儲槽容量總合 float
  declare @天然氣管線數量總合 int
  declare @天然氣管線長度總合 float
  declare @天然氣儲槽數量總合 int
  declare @天然氣儲槽容量總合 float

  set @石油管線數量總合 = (select count(*) from 石油_管線基本資料 a left join 石油_業者基本資料 b on a.業者guid=b.guid 
  where a.資料狀態='A' and b.資料狀態='A' )
  set @石油管線長度總合 = (select isnull(sum(convert(float, a.轄管長度)),0) from 石油_管線基本資料 a left join 石油_業者基本資料 b on a.業者guid=b.guid 
  where a.資料狀態='A' and b.資料狀態='A' )
  set @石油儲槽數量總合 = (select count(*) from 石油_儲槽基本資料 a left join 石油_業者基本資料 b on a.業者guid=b.guid 
  where a.資料狀態='A' and b.資料狀態='A' )
  set @石油儲槽容量總合 = (select isnull(sum(convert(float, a.容量)),0) from 石油_儲槽基本資料 a left join 石油_業者基本資料 b on a.業者guid=b.guid 
  where a.資料狀態='A' and b.資料狀態='A' )

  set @天然氣管線數量總合 = (select count(*) from 天然氣_管線基本資料 a left join 天然氣_業者基本資料表 b on a.業者guid=b.guid 
  where a.資料狀態='A' and b.資料狀態='A' )
  set @天然氣管線長度總合 = (select isnull(sum(convert(float, a.轄管長度)),0) from 天然氣_管線基本資料 a left join 天然氣_業者基本資料表 b on a.業者guid=b.guid 
  where a.轄管長度 not like '%~%' and  a.資料狀態='A' and b.資料狀態='A' )
  set @天然氣儲槽數量總合 = (select count(*) from 天然氣_儲槽設施資料_儲槽基本資料 a left join 天然氣_業者基本資料表 b on a.業者guid=b.guid 
  where a.資料狀態='A' and b.資料狀態='A' )
  set @天然氣儲槽容量總合 = (select isnull(sum(convert(float, a.容量)),0) from 天然氣_儲槽設施資料_儲槽基本資料 a left join 天然氣_業者基本資料表 b on a.業者guid=b.guid 
  where a.容量 not like '%~%' and  a.資料狀態='A' and b.資料狀態='A' )

  select @石油管線數量總合+@天然氣管線數量總合 as 管線數量總合, 
         @石油管線長度總合+@天然氣管線長度總合 as 管線長度總合, 
         @石油儲槽數量總合+@天然氣儲槽數量總合 as 儲槽數量總合,
         @石油儲槽容量總合+@天然氣儲槽容量總合 as 儲槽容量總合 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@公司名稱", 公司名稱);
        oCmd.Parameters.AddWithValue("@事業部", 事業部);
        oCmd.Parameters.AddWithValue("@營業處廠", 營業處廠);
        oCmd.Parameters.AddWithValue("@中心庫區儲運課工場", 中心庫區儲運課工場);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCpName2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select 公司名稱,單獨公司名稱, isnull(營業處廠,'')+isnull(組,'')+isnull(中心庫區儲運課工場,'') as cpname from 石油_業者基本資料
  where 資料狀態='A' and 列表是否顯示='Y' and guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCpName3()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select guid,公司名稱, isnull(營業處廠,'')+isnull(組,'')+isnull(中心庫區儲運課工場,'') as cpname, 單獨公司名稱 from 石油_業者基本資料
  where 資料狀態='A' and guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCpNameStatistics()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select 
'01' as 類別, guid, 公司名稱, 事業部, 營業處廠, 中心庫區儲運課工場, 
pipeCount=(select count(*) as allCount from 石油_管線基本資料 where 業者guid=石油_業者基本資料.guid and 資料狀態='A' ), 
tankCount=(select count(*) as allCount from 石油_儲槽基本資料 where 業者guid=石油_業者基本資料.guid and 資料狀態='A' )  
from 石油_業者基本資料 
where 資料狀態='A' and 列表是否顯示='Y' ");
        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid = @guid ");
        if (!string.IsNullOrEmpty(公司名稱))
            sb.Append(@"and 公司名稱 = @公司名稱 ");
        if (!string.IsNullOrEmpty(事業部))
            sb.Append(@"and 事業部 = @事業部 ");
        if (!string.IsNullOrEmpty(營業處廠))
            sb.Append(@"and 營業處廠 = @營業處廠 ");
        if (!string.IsNullOrEmpty(中心庫區儲運課工場))
            sb.Append(@"and 中心庫區儲運課工場 = @中心庫區儲運課工場 ");
        if (!string.IsNullOrEmpty(地址))
            sb.Append(@"and left(地址,3) = @地址 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@公司名稱", 公司名稱);
        oCmd.Parameters.AddWithValue("@事業部", 事業部);
        oCmd.Parameters.AddWithValue("@營業處廠", 營業處廠);
        oCmd.Parameters.AddWithValue("@中心庫區儲運課工場", 中心庫區儲運課工場);
        oCmd.Parameters.AddWithValue("@地址", 地址);

        oda.Fill(ds);
        return ds;
    }

    //在列表點選檢視重新更新業者guid用
    public DataTable GetCompany()
    {
        HttpContext.Current.Session["companyGuid"] = null;
        HttpContext.Current.Session.Remove("companyGuid");
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_業者基本資料 where guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();
        oCmd.Parameters.AddWithValue("@guid", guid);
        oda.Fill(ds);

        if (ds.Rows.Count > 0)
        {
            LogInfo.companyGuid = ds.Rows[0]["guid"].ToString();
        }

        return ds;
    }

    public DataTable GetCompanyList(string mGuid, string year)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 石油_業者基本資料 c 
left join 石油_委員業者年度對應表 m on c.guid=m.業者guid and m.資料狀態='A' and m.委員guid=@mGuid 
where c.資料狀態='A' and c.列表是否顯示='Y' ");

        if (mGuid != "")
            sb.Append(@"and m.委員guid=@mGuid ");
        if (year != "")
            sb.Append(@"and m.年度=@年度 ");

        sb.Append(@"order by 排序編號 ");

        oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@mGuid", mGuid);
        oCmd.Parameters.AddWithValue("@年度", year);

        oda.Fill(ds);
		return ds;
	}

    public DataTable GetCompanyListVerification(string mGuid, string year)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select c.guid, c.公司名稱, c.事業部, c.營業處廠, c.中心庫區儲運課工場 from 石油_業者基本資料 c 
left join 石油_委員業者年度對應表 m on c.guid=m.業者guid and m.資料狀態='A' and m.委員guid=@mGuid 
where c.資料狀態='A' and c.列表是否顯示='Y' ");

        if (mGuid != "")
            sb.Append(@"and m.委員guid=@mGuid ");
        if (year != "")
            sb.Append(@"and m.年度=@年度 ");

        sb.Append(@"order by 排序編號 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@mGuid", mGuid);
        oCmd.Parameters.AddWithValue("@年度", year);

        oda.Fill(ds);
        return ds;
    }

    public DataTable Get05CompanyList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_業者基本資料 
where 公司名稱='台灣中油' and 資料狀態='A' and 列表是否顯示='Y' order by 排序編號 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable Get05CompanyListVerification()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select guid, 公司名稱, 事業部, 營業處廠, 中心庫區儲運課工場 from 石油_業者基本資料 
where 公司名稱='台灣中油' and 資料狀態='A' and 列表是否顯示='Y' order by 排序編號 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable Get06CompanyList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_業者基本資料 
where 公司名稱='台塑石化' and 資料狀態='A' and 列表是否顯示='Y' order by 排序編號 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable Get06CompanyListVerification()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select guid, 公司名稱, 事業部, 營業處廠, 中心庫區儲運課工場 from 石油_業者基本資料 
where 公司名稱='台塑石化' and 資料狀態='A' and 列表是否顯示='Y' order by 排序編號 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCpNameToNcree()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select case when 事業部 is not null and 營業處廠 is null and 中心庫區儲運課工場 is null then 事業部
  when 事業部 is not null and 營業處廠 is not null and 中心庫區儲運課工場 is null then 營業處廠
  when 事業部 is not null and 營業處廠 is not null and 中心庫區儲運課工場 is not null then 中心庫區儲運課工場 end as cpname
  from 石油_業者基本資料
  where 管線管理不顯示 is null and 列表是否顯示='Y' ");
        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid = @guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetCpNameToNcreeList(string cpname)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        if (cpname == "桃園儲運站")
        {
            sb.Append(@"select distinct 場站 from ncree..vw_fpg_oil
where 場站 like '%' + @場站 + '%'");
        }
        else
        {
            sb.Append(@"select distinct 場站 from ncree..vw_cpc_oil
where 場站 like '%' + @場站 + '%'");
        }

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@場站", cpname);

        oda.Fill(ds);
        return ds;
    }

    public void InsertCompanyInfo(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        insert into 石油_事業單位基本資料表(
業者guid, 
年度, 
電話, 
地址, 
儲槽數量, 
儲槽容量, 
管線數量, 
管線長度, 
維運計畫書及成果報告,
曾執行過查核日期,
年度查核姓名,
年度查核職稱,
年度查核分機,
年度查核email,
年度檢測姓名,
年度檢測職稱,
年度檢測分機,
年度檢測email,
建立者,
建立日期,
修改者,
修改日期,
資料狀態 ) values (
@業者guid, 
@年度, 
@電話, 
@地址, 
@儲槽數量, 
@儲槽容量, 
@管線數量, 
@管線長度, 
@維運計畫書及成果報告,
@曾執行過查核日期,
@年度查核姓名,
@年度查核職稱,
@年度查核分機,
@年度查核email,
@年度檢測姓名,
@年度檢測職稱,
@年度檢測分機,
@年度檢測email,
@建立者,
@建立日期,
@修改者,
@修改日期,
@資料狀態 ) 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@業者guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@電話", 電話);
        oCmd.Parameters.AddWithValue("@地址", 地址);
        oCmd.Parameters.AddWithValue("@儲槽數量", 儲槽數量);
        oCmd.Parameters.AddWithValue("@儲槽容量", 儲槽容量);
        oCmd.Parameters.AddWithValue("@管線數量", 管線數量);
        oCmd.Parameters.AddWithValue("@管線長度", 管線長度);
        oCmd.Parameters.AddWithValue("@維運計畫書及成果報告", 維運計畫書及成果報告);
        oCmd.Parameters.AddWithValue("@曾執行過查核日期", 曾執行過查核日期);
        oCmd.Parameters.AddWithValue("@年度查核姓名", 年度查核姓名);
        oCmd.Parameters.AddWithValue("@年度查核職稱", 年度查核職稱);
        oCmd.Parameters.AddWithValue("@年度查核分機", 年度查核分機);
        oCmd.Parameters.AddWithValue("@年度查核email", 年度查核email);
        oCmd.Parameters.AddWithValue("@年度檢測姓名", 年度檢測姓名);
        oCmd.Parameters.AddWithValue("@年度檢測職稱", 年度檢測職稱);
        oCmd.Parameters.AddWithValue("@年度檢測分機", 年度檢測分機);
        oCmd.Parameters.AddWithValue("@年度檢測email", 年度檢測email);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateCompanyInfo(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        update 石油_事業單位基本資料表 set
        電話=@電話, 
        地址=@地址, 
        儲槽數量=@儲槽數量, 
        儲槽容量=@儲槽容量, 
        管線數量=@管線數量,  
        管線長度=@管線長度,  
        維運計畫書及成果報告=@維運計畫書及成果報告, 
        曾執行過查核日期=@曾執行過查核日期,
        年度查核姓名=@年度查核姓名,
        年度查核職稱=@年度查核職稱,
        年度查核分機=@年度查核分機,
        年度查核email=@年度查核email,
        年度檢測姓名=@年度檢測姓名,
        年度檢測職稱=@年度檢測職稱,
        年度檢測分機=@年度檢測分機,
        年度檢測email=@年度檢測email,
        修改者 =@修改者,
        修改日期=@修改日期 
        where 業者guid=@業者guid and 年度=@年度 and 資料狀態=@資料狀態 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@業者guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@電話", 電話);
        oCmd.Parameters.AddWithValue("@地址", 地址);
        oCmd.Parameters.AddWithValue("@儲槽數量", 儲槽數量);
        oCmd.Parameters.AddWithValue("@儲槽容量", 儲槽容量);
        oCmd.Parameters.AddWithValue("@管線數量", 管線數量);
        oCmd.Parameters.AddWithValue("@管線長度", 管線長度);
        oCmd.Parameters.AddWithValue("@維運計畫書及成果報告", 維運計畫書及成果報告);
        oCmd.Parameters.AddWithValue("@曾執行過查核日期", 曾執行過查核日期);
        oCmd.Parameters.AddWithValue("@年度查核姓名", 年度查核姓名);
        oCmd.Parameters.AddWithValue("@年度查核職稱", 年度查核職稱);
        oCmd.Parameters.AddWithValue("@年度查核分機", 年度查核分機);
        oCmd.Parameters.AddWithValue("@年度查核email", 年度查核email);
        oCmd.Parameters.AddWithValue("@年度檢測姓名", 年度檢測姓名);
        oCmd.Parameters.AddWithValue("@年度檢測職稱", 年度檢測職稱);
        oCmd.Parameters.AddWithValue("@年度檢測分機", 年度檢測分機);
        oCmd.Parameters.AddWithValue("@年度檢測email", 年度檢測email);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateCompanyInfoOnlyContact(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        update 石油_事業單位基本資料表 set
        年度查核姓名=@年度查核姓名,
        年度查核職稱=@年度查核職稱,
        年度查核分機=@年度查核分機,
        年度查核email=@年度查核email,
        年度檢測姓名=@年度檢測姓名,
        年度檢測職稱=@年度檢測職稱,
        年度檢測分機=@年度檢測分機,
        年度檢測email=@年度檢測email,
        修改者 =@修改者,
        修改日期=@修改日期 
        where 業者guid=@業者guid and 年度=@年度 and 資料狀態=@資料狀態 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@業者guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@年度查核姓名", 年度查核姓名);
        oCmd.Parameters.AddWithValue("@年度查核職稱", 年度查核職稱);
        oCmd.Parameters.AddWithValue("@年度查核分機", 年度查核分機);
        oCmd.Parameters.AddWithValue("@年度查核email", 年度查核email);
        oCmd.Parameters.AddWithValue("@年度檢測姓名", 年度檢測姓名);
        oCmd.Parameters.AddWithValue("@年度檢測職稱", 年度檢測職稱);
        oCmd.Parameters.AddWithValue("@年度檢測分機", 年度檢測分機);
        oCmd.Parameters.AddWithValue("@年度檢測email", 年度檢測email);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        update 石油_業者基本資料 set ");
        if (!string.IsNullOrEmpty(資料是否確認))
            if(資料是否確認 == "N")
                sb.Append(@" 資料是否確認='',");
            else
                sb.Append(@" 資料是否確認=@資料是否確認,");
        if (!string.IsNullOrEmpty(年度儲槽確認))
            if (年度儲槽確認 == "N")
                sb.Append(@" 年度儲槽確認='',");
            else
                sb.Append(@" 年度儲槽確認=@年度儲槽確認,");
        if (!string.IsNullOrEmpty(年度液化石油氣儲槽確認))
            if (年度液化石油氣儲槽確認 == "N")
                sb.Append(@" 年度液化石油氣儲槽確認='',");
            else
                sb.Append(@" 年度液化石油氣儲槽確認=@年度液化石油氣儲槽確認,");
        sb.Append(@" 
        修改者=@修改者, 
        修改日期=@修改日期 
        where guid=@guid and 資料狀態=@資料狀態 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@資料是否確認", 資料是否確認);
        oCmd.Parameters.AddWithValue("@年度儲槽確認", 年度儲槽確認);
        oCmd.Parameters.AddWithValue("@年度液化石油氣儲槽確認", 年度液化石油氣儲槽確認);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateConfirmData(SqlConnection oConn, SqlTransaction oTran, string type)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        update 石油_業者基本資料 set ");
        if (type == "1")
            sb.Append(@" 年度儲槽確認=@年度儲槽確認,");
        else
            sb.Append(@" 年度液化石油氣儲槽確認=@年度液化石油氣儲槽確認,");
        sb.Append(@" 
        修改者=@修改者, 
        修改日期=@修改日期 
        where guid=@guid and 資料狀態=@資料狀態 ");

        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度儲槽確認", 年度儲槽確認);
        oCmd.Parameters.AddWithValue("@年度液化石油氣儲槽確認", 年度液化石油氣儲槽確認);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public DataTable GetExporttable()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from exportTable ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable AddWord(string address, string company, string name, string signatral)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"insert into word匯入列表(地址, 公司部門, 姓名, 簽名)
        values(
        @地址, 
        @公司部門,
        @姓名,
        @簽名) 
        ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@地址", address);
        oCmd.Parameters.AddWithValue("@公司部門", company);
        oCmd.Parameters.AddWithValue("@姓名", name);
        oCmd.Parameters.AddWithValue("@簽名", signatral);

        oda.Fill(ds);
        return ds;
    }
}