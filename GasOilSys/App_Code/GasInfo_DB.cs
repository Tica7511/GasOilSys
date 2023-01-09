using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasInfo_DB 的摘要描述
/// </summary>
public class GasInfo_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 事業名稱 = string.Empty;
	string 電話 = string.Empty;
	string 地址 = string.Empty;
	string 輸氣幹線 = string.Empty;
	string 輸氣環線 = string.Empty;
	string 配氣專管 = string.Empty;
	string 場內成品線 = string.Empty;
	string 海底管線 = string.Empty;
    string LNG管線 = string.Empty;
    string BOG管線 = string.Empty;
    string NG管線 = string.Empty;
    string 供氣對象縣市 = string.Empty;
	string 供應天然氣 = string.Empty;
	string 儲槽 = string.Empty;
	string 注氣站 = string.Empty;
	string 加壓站 = string.Empty;
	string 配氣站 = string.Empty;
	string 隔離站 = string.Empty;
	string 開關站 = string.Empty;
	string 清管站 = string.Empty;
	string 整壓計量站 = string.Empty;
	string 低壓排放塔 = string.Empty;
	string 高壓排放塔 = string.Empty;
	string NG2摻配站 = string.Empty;
	string 年度查核姓名 = string.Empty;
	string 年度查核職稱 = string.Empty;
	string 年度查核分機 = string.Empty;
	string 年度查核email = string.Empty;
	string 建立者 = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;

	// 天然氣_事業單位基本資料表_進口事業轄區場站名稱
	string 場站類別 = string.Empty;
	string 中心名稱 = string.Empty;
	string 排序 = string.Empty;
	#endregion
	#region public
	public string _id { set { id = value; } }
	public string _guid { set { guid = value; } }
	public string _業者guid { set { 業者guid = value; } }
	public string _年度 { set { 年度 = value; } }
	public string _事業名稱 { set { 事業名稱 = value; } }
	public string _電話 { set { 電話 = value; } }
	public string _地址 { set { 地址 = value; } }
	public string _輸氣幹線 { set { 輸氣幹線 = value; } }
	public string _輸氣環線 { set { 輸氣環線 = value; } }
	public string _配氣專管 { set { 配氣專管 = value; } }
	public string _場內成品線 { set { 場內成品線 = value; } }
	public string _海底管線 { set { 海底管線 = value; } }
	public string _LNG管線 { set { LNG管線 = value; } }
	public string _BOG管線 { set { BOG管線 = value; } }
	public string _NG管線 { set { NG管線 = value; } }
	public string _供氣對象縣市 { set { 供氣對象縣市 = value; } }
	public string _供應天然氣 { set { 供應天然氣 = value; } }
	public string _儲槽 { set { 儲槽 = value; } }
	public string _注氣站 { set { 注氣站 = value; } }
	public string _加壓站 { set { 加壓站 = value; } }
	public string _配氣站 { set { 配氣站 = value; } }
	public string _隔離站 { set { 隔離站 = value; } }
	public string _開關站 { set { 開關站 = value; } }
	public string _清管站 { set { 清管站 = value; } }
	public string _整壓計量站 { set { 整壓計量站 = value; } }
	public string _低壓排放塔 { set { 低壓排放塔 = value; } }
	public string _高壓排放塔 { set { 高壓排放塔 = value; } }
	public string _NG2摻配站 { set { NG2摻配站 = value; } }
	public string _年度查核姓名 { set { 年度查核姓名 = value; } }
	public string _年度查核職稱 { set { 年度查核職稱 = value; } }
	public string _年度查核分機 { set { 年度查核分機 = value; } }
	public string _年度查核email { set { 年度查核email = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }

	// 天然氣_事業單位基本資料表_進口事業轄區場站名稱
	public string _場站類別 { set { 場站類別  = value; } }
	public string _中心名稱 { set { 中心名稱 = value; } }
	public string _排序 { set { 排序 = value; } }
	#endregion

	public DataTable GetInfo()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_事業單位基本資料表 where 資料狀態='A' and 業者guid=@業者guid ");

		if(!string.IsNullOrEmpty(年度))
			sb.Append(@" and 年度=@年度 ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
SELECT '' as [戰場類別中心名稱],[配氣站],[開關站],[隔離站],[計量站],[清管站]
FROM    
(   SELECT C.排序,C.[場站類別], C.[中心名稱]
    FROM [天然氣_事業單位基本資料表_進口事業轄區場站名稱] C
    WHERE C.[業者guid] = @業者guid and C.[年度]=@年度 and C.[資料狀態]='A'
) AS P
PIVOT 
(   MAX ([中心名稱]) 
    FOR [場站類別] in ([配氣站],[開關站],[隔離站],[計量站],[清管站])
) AS  PVT
UNION ALL
SELECT  '總計',
CONVERT(nvarchar(10),count(case when [場站類別]='配氣站' then 1 else null end)),
CONVERT(nvarchar(10),count(case when [場站類別]='開關站' then 1 else null end)),
CONVERT(nvarchar(10),count(case when [場站類別]='隔離站' then 1 else null end)),
CONVERT(nvarchar(10),count(case when [場站類別]='計量站' then 1 else null end)),
CONVERT(nvarchar(10),count(case when [場站類別]='清管站' then 1 else null end))
FROM [天然氣_事業單位基本資料表_進口事業轄區場站名稱]
WHERE [業者guid]=@業者guid and [年度]=@年度 and [資料狀態]='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select * from 天然氣_事業單位基本資料表_進口事業轄區場站名稱 
where 業者guid=@業者guid and 年度=@年度 and 資料狀態='A'   
");

        if (!string.IsNullOrEmpty(中心名稱))
            sb.Append(@" and 中心名稱=@中心名稱");
        if (!string.IsNullOrEmpty(場站類別))
            sb.Append(@" and 場站類別=@場站類別");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@中心名稱", 中心名稱);
        oCmd.Parameters.AddWithValue("@場站類別", 場站類別);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetDataGuid()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select * from 天然氣_事業單位基本資料表_進口事業轄區場站名稱 
where guid=@guid 
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetMaxSn()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select ISNULL(MAX(排序), '0') maxSn from 天然氣_事業單位基本資料表_進口事業轄區場站名稱 
where 業者guid=@業者guid and 年度=@年度 and 資料狀態='A' 
");
        if (!string.IsNullOrEmpty(場站類別))
            sb.Append(@" and 場站類別=@場站類別");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@場站類別", 場站類別);

        oda.Fill(ds);
        return ds;
    }

	public void InsertPipeData(SqlConnection oConn, SqlTransaction oTran)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(@"insert into 天然氣_事業單位基本資料表(  
業者guid,
年度,
事業名稱,
電話,
地址,
輸氣幹線,
輸氣環線,
配氣專管,
場內成品線,
海底管線,
LNG管線,
BOG管線,
NG管線,
供氣對象縣市,
供應天然氣,
儲槽,
注氣站,
加壓站,
配氣站,
隔離站,
開關站,
清管站,
整壓計量站,
低壓排放塔,
高壓排放塔,
NG2摻配站,
年度查核姓名,
年度查核職稱,
年度查核分機,
年度查核email,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@業者guid,
@年度,
@事業名稱,
@電話,
@地址,
@輸氣幹線,
@輸氣環線,
@配氣專管,
@場內成品線,
@海底管線,
@LNG管線,
@BOG管線,
@NG管線,
@供氣對象縣市,
@供應天然氣,
@儲槽,
@注氣站,
@加壓站,
@配氣站,
@隔離站,
@開關站,
@清管站,
@整壓計量站,
@低壓排放塔,
@高壓排放塔,
@NG2摻配站,
@年度查核姓名,
@年度查核職稱,
@年度查核分機,
@年度查核email,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態 
) ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@事業名稱", 事業名稱);
		oCmd.Parameters.AddWithValue("@電話", 電話);
		oCmd.Parameters.AddWithValue("@地址", 地址);
		oCmd.Parameters.AddWithValue("@輸氣幹線", 輸氣幹線);
		oCmd.Parameters.AddWithValue("@輸氣環線", 輸氣環線);
		oCmd.Parameters.AddWithValue("@配氣專管", 配氣專管);
		oCmd.Parameters.AddWithValue("@場內成品線", 場內成品線);
		oCmd.Parameters.AddWithValue("@海底管線", 海底管線);
		oCmd.Parameters.AddWithValue("@LNG管線", LNG管線);
		oCmd.Parameters.AddWithValue("@BOG管線", BOG管線);
		oCmd.Parameters.AddWithValue("@NG管線", NG管線);
		oCmd.Parameters.AddWithValue("@供氣對象縣市", 供氣對象縣市);
		oCmd.Parameters.AddWithValue("@供應天然氣", 供應天然氣);
		oCmd.Parameters.AddWithValue("@儲槽", 儲槽);
		oCmd.Parameters.AddWithValue("@注氣站", 注氣站);
		oCmd.Parameters.AddWithValue("@加壓站", 加壓站);
		oCmd.Parameters.AddWithValue("@配氣站", 配氣站);
		oCmd.Parameters.AddWithValue("@隔離站", 隔離站);
		oCmd.Parameters.AddWithValue("@開關站", 開關站);
		oCmd.Parameters.AddWithValue("@清管站", 清管站);
		oCmd.Parameters.AddWithValue("@整壓計量站", 整壓計量站);
		oCmd.Parameters.AddWithValue("@低壓排放塔", 低壓排放塔);
		oCmd.Parameters.AddWithValue("@高壓排放塔", 高壓排放塔);
		oCmd.Parameters.AddWithValue("@NG2摻配站", NG2摻配站);
		oCmd.Parameters.AddWithValue("@年度查核姓名", 年度查核姓名);
		oCmd.Parameters.AddWithValue("@年度查核職稱", 年度查核職稱);
		oCmd.Parameters.AddWithValue("@年度查核分機", 年度查核分機);
		oCmd.Parameters.AddWithValue("@年度查核email", 年度查核email);
		oCmd.Parameters.AddWithValue("@修改者", 修改者);
		oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@建立者", 建立者);
		oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@資料狀態", 'A');

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

	public void InsertData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 天然氣_事業單位基本資料表_進口事業轄區場站名稱(  
年度,
業者guid,
場站類別,
中心名稱,
排序,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@場站類別,
@中心名稱,
@排序,
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
        oCmd.Parameters.AddWithValue("@場站類別", 場站類別);
        oCmd.Parameters.AddWithValue("@中心名稱", 中心名稱);
        oCmd.Parameters.AddWithValue("@排序", 排序);
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

		sb.Append(@"




update 天然氣_事業單位基本資料表 set  
事業名稱=@事業名稱,
電話=@電話,
地址=@地址,
輸氣幹線=@輸氣幹線,
輸氣環線=@輸氣環線,
配氣專管=@配氣專管,
場內成品線=@場內成品線,
海底管線=@海底管線,
LNG管線=@LNG管線,
BOG管線=@BOG管線,
NG管線=@NG管線,
供氣對象縣市=@供氣對象縣市,
供應天然氣=@供應天然氣,
儲槽=@儲槽,
注氣站=@注氣站,
加壓站=@加壓站,
配氣站=@配氣站,
隔離站=@隔離站,
開關站=@開關站,
清管站=@清管站,
整壓計量站=@整壓計量站,
低壓排放塔=@低壓排放塔,
高壓排放塔=@高壓排放塔,
NG2摻配站=@NG2摻配站,
年度查核姓名=@年度查核姓名,
年度查核職稱=@年度查核職稱,
年度查核分機=@年度查核分機,
年度查核email=@年度查核email,
修改者=@修改者, 
修改日期=@修改日期 
where 業者guid=@業者guid and 年度=@年度 and 資料狀態=@資料狀態 
 ");
		SqlCommand oCmd = oConn.CreateCommand();
		oCmd.CommandText = sb.ToString();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);
		oCmd.Parameters.AddWithValue("@事業名稱", 事業名稱);
		oCmd.Parameters.AddWithValue("@電話", 電話);
        oCmd.Parameters.AddWithValue("@地址", 地址);
		oCmd.Parameters.AddWithValue("@輸氣幹線", 輸氣幹線);
        oCmd.Parameters.AddWithValue("@輸氣環線", 輸氣環線);
        oCmd.Parameters.AddWithValue("@配氣專管", 配氣專管);
		oCmd.Parameters.AddWithValue("@場內成品線", 場內成品線);
		oCmd.Parameters.AddWithValue("@海底管線", 海底管線);
		oCmd.Parameters.AddWithValue("@LNG管線", LNG管線);
        oCmd.Parameters.AddWithValue("@BOG管線", BOG管線);
		oCmd.Parameters.AddWithValue("@NG管線", NG管線);
		oCmd.Parameters.AddWithValue("@供氣對象縣市", 供氣對象縣市);
		oCmd.Parameters.AddWithValue("@供應天然氣", 供應天然氣);
		oCmd.Parameters.AddWithValue("@儲槽", 儲槽);
		oCmd.Parameters.AddWithValue("@注氣站", 注氣站);
        oCmd.Parameters.AddWithValue("@加壓站", 加壓站);
        oCmd.Parameters.AddWithValue("@配氣站", 配氣站);
        oCmd.Parameters.AddWithValue("@隔離站", 隔離站);
        oCmd.Parameters.AddWithValue("@開關站", 開關站);
        oCmd.Parameters.AddWithValue("@清管站", 清管站);
		oCmd.Parameters.AddWithValue("@整壓計量站", 整壓計量站);
        oCmd.Parameters.AddWithValue("@低壓排放塔", 低壓排放塔);
        oCmd.Parameters.AddWithValue("@高壓排放塔", 高壓排放塔);
		oCmd.Parameters.AddWithValue("@NG2摻配站", NG2摻配站);
		oCmd.Parameters.AddWithValue("@年度查核姓名", 年度查核姓名);
		oCmd.Parameters.AddWithValue("@年度查核職稱", 年度查核職稱);
		oCmd.Parameters.AddWithValue("@年度查核分機", 年度查核分機);
		oCmd.Parameters.AddWithValue("@年度查核email", 年度查核email);
		oCmd.Parameters.AddWithValue("@修改者", 修改者);
		oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
		oCmd.Parameters.AddWithValue("@資料狀態", 'A');

		oCmd.Transaction = oTran;
		oCmd.ExecuteNonQuery();
	}

    public void UpdateDataSn(string Sn)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 天然氣_事業單位基本資料表_進口事業轄區場站名稱 set  
排序=@排序加一 
where 年度=@年度 and 業者guid=@業者guid and 場站類別=@場站類別 and 中心名稱=@中心名稱 and 資料狀態=@資料狀態 
 ";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@場站類別", 場站類別);
        oCmd.Parameters.AddWithValue("@中心名稱", 中心名稱);
        oCmd.Parameters.AddWithValue("@排序加一", Sn);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public void DeleteData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 天然氣_事業單位基本資料表_進口事業轄區場站名稱 set 
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

	public DataTable GetYearList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"  
declare @yearCount int

select DISTINCT 年度 into #tmp from 天然氣_事業單位基本資料表
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
}