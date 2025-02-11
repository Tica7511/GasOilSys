﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasControl_DB 的摘要描述
/// </summary>
public class GasControl_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 壓力計校正頻率 = string.Empty;
	string 壓力計校正_最近一次校正時間 = string.Empty;
	string 流量計校正頻率 = string.Empty;
	string 流量計校正_最近一次校正時間 = string.Empty;
	string 監控中心定期調整之週期 = string.Empty;
	string 合格操作人員總數 = string.Empty;
	string 輪班制度 = string.Empty;
	string 每班人數 = string.Empty;
	string 每班時數 = string.Empty;
	string 建立者 = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;

    //依據文件資料
    string 依據文件名稱 = string.Empty;
    string 文件編號 = string.Empty;
    string 文件日期 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
	public string _guid { set { guid = value; } }
	public string _業者guid { set { 業者guid = value; } }
	public string _年度 { set { 年度 = value; } }
	public string _壓力計校正頻率 { set { 壓力計校正頻率 = value; } }
	public string _壓力計校正_最近一次校正時間 { set { 壓力計校正_最近一次校正時間 = value; } }
	public string _流量計校正頻率 { set { 流量計校正頻率 = value; } }
	public string _流量計校正_最近一次校正時間 { set { 流量計校正_最近一次校正時間 = value; } }
	public string _監控中心定期調整之週期 { set { 監控中心定期調整之週期 = value; } }
	public string _合格操作人員總數 { set { 合格操作人員總數 = value; } }
	public string _輪班制度 { set { 輪班制度 = value; } }
	public string _每班人數 { set { 每班人數 = value; } }
	public string _每班時數 { set { 每班時數 = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }

    //依據文件資料
    public string _依據文件名稱 { set { 依據文件名稱 = value; } }
    public string _文件編號 { set { 文件編號 = value; } }
    public string _文件日期 { set { 文件日期 = value; } }
    #endregion

    public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_控制室 where 資料狀態='A' and 業者guid=@業者guid ");
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

        sb.Append(@"select * from 天然氣_控制室_依據文件資料 where 資料狀態='A' and 業者guid=@業者guid ");
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

select DISTINCT 年度 into #tmp from 天然氣_控制室
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

		sb.Append(@"select * from 天然氣_控制室_依據文件資料 where guid=@guid and 資料狀態='A' ");

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
        sb.Append(@"insert into 天然氣_控制室(  
年度,
業者guid,
壓力計校正頻率,
壓力計校正_最近一次校正時間,
流量計校正頻率,
流量計校正_最近一次校正時間,
監控中心定期調整之週期,
合格操作人員總數,
輪班制度,
每班人數,
每班時數,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@壓力計校正頻率,
@壓力計校正_最近一次校正時間,
@流量計校正頻率,
@流量計校正_最近一次校正時間,
@監控中心定期調整之週期,
@合格操作人員總數,
@輪班制度,
@每班人數,
@每班時數,
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
        oCmd.Parameters.AddWithValue("@壓力計校正頻率", 壓力計校正頻率);
        oCmd.Parameters.AddWithValue("@壓力計校正_最近一次校正時間", 壓力計校正_最近一次校正時間);
        oCmd.Parameters.AddWithValue("@流量計校正頻率", 流量計校正頻率);
        oCmd.Parameters.AddWithValue("@流量計校正_最近一次校正時間", 流量計校正_最近一次校正時間);
        oCmd.Parameters.AddWithValue("@監控中心定期調整之週期", 監控中心定期調整之週期);
        oCmd.Parameters.AddWithValue("@合格操作人員總數", 合格操作人員總數);
        oCmd.Parameters.AddWithValue("@輪班制度", 輪班制度);
        oCmd.Parameters.AddWithValue("@每班人數", 每班人數);
        oCmd.Parameters.AddWithValue("@每班時數", 每班時數);
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
        sb.Append(@"update 天然氣_控制室 set  
年度=@年度,
壓力計校正頻率=@壓力計校正頻率,
壓力計校正_最近一次校正時間=@壓力計校正_最近一次校正時間,
流量計校正頻率=@流量計校正頻率,
流量計校正_最近一次校正時間=@流量計校正_最近一次校正時間,
監控中心定期調整之週期=@監控中心定期調整之週期,
合格操作人員總數=@合格操作人員總數,
輪班制度=@輪班制度,
每班人數=@每班人數,
每班時數=@每班時數,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@壓力計校正頻率", 壓力計校正頻率);
        oCmd.Parameters.AddWithValue("@壓力計校正_最近一次校正時間", 壓力計校正_最近一次校正時間);
        oCmd.Parameters.AddWithValue("@流量計校正頻率", 流量計校正頻率);
        oCmd.Parameters.AddWithValue("@流量計校正_最近一次校正時間", 流量計校正_最近一次校正時間);
        oCmd.Parameters.AddWithValue("@監控中心定期調整之週期", 監控中心定期調整之週期);
        oCmd.Parameters.AddWithValue("@合格操作人員總數", 合格操作人員總數);
        oCmd.Parameters.AddWithValue("@輪班制度", 輪班制度);
        oCmd.Parameters.AddWithValue("@每班人數", 每班人數);
        oCmd.Parameters.AddWithValue("@每班時數", 每班時數);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void InsertData2(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 天然氣_控制室_依據文件資料(  
年度,
業者guid,
依據文件名稱,
文件編號,
文件日期,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@依據文件名稱,
@文件編號,
@文件日期,
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
        oCmd.Parameters.AddWithValue("@依據文件名稱", 依據文件名稱);
        oCmd.Parameters.AddWithValue("@文件編號", 文件編號);
        oCmd.Parameters.AddWithValue("@文件日期", 文件日期);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData2(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 天然氣_控制室_依據文件資料 set  
年度=@年度, 
依據文件名稱=@依據文件名稱,
文件編號=@文件編號,
文件日期=@文件日期, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@依據文件名稱", 依據文件名稱);
        oCmd.Parameters.AddWithValue("@文件編號", 文件編號);
        oCmd.Parameters.AddWithValue("@文件日期", 文件日期);
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
        oCmd.CommandText = @"update 天然氣_控制室_依據文件資料 set 
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