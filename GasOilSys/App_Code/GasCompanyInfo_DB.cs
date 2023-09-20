using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasCompanyInfo_DB 的摘要描述
/// </summary>
public class GasCompanyInfo_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 公司名稱 = string.Empty;
	string 事業部 = string.Empty;
	string 營業處廠 = string.Empty;
	string 中心庫區儲運課工場 = string.Empty;
	string 地址 = string.Empty;
	string 電話 = string.Empty;
	string fun2 = string.Empty;
	string 資料是否確認 = string.Empty;
	string 建立者 = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;
	#endregion
	#region public
	public string _id { set { id = value; } }
	public string _guid { set { guid = value; } }
	public string _公司名稱 { set { 公司名稱 = value; } }
	public string _事業部 { set { 事業部 = value; } }
	public string _營業處廠 { set { 營業處廠 = value; } }
	public string _中心庫區儲運課工場 { set { 中心庫區儲運課工場 = value; } }
	public string _地址 { set { 地址 = value; } }
	public string _電話 { set { 電話 = value; } }
	public string _fun2 { set { fun2 = value; } }
	public string _資料是否確認 { set { 資料是否確認 = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }
	#endregion

	public DataTable GetCompanyList(string mGuid, string year)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select 
管線數量=(select count(*) from 天然氣_管線基本資料 where 業者guid=c.guid and 資料狀態='A'),
儲槽數量=(select count(*) from 天然氣_儲槽設施資料_儲槽基本資料 where 業者guid=c.guid and 資料狀態='A'),
* from 天然氣_業者基本資料表 c 
left join 天然氣_委員業者年度對應表 m on c.guid=m.業者guid and m.資料狀態='A' and m.委員guid=@mGuid 
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

		sb.Append(@"select c.guid, c.公司名稱, c.事業部, c.營業處廠, c.中心庫區儲運課工場 
from 天然氣_業者基本資料表 c 
left join 天然氣_委員業者年度對應表 m on c.guid=m.業者guid and m.資料狀態='A' and m.委員guid=@mGuid 
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

	public DataTable GetCompanyByAddress()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"SELECT distinct convert(nvarchar(3),Left(地址,3))   COLLATE Chinese_Taiwan_Stroke_CS_AS as showName 
FROM 天然氣_業者基本資料表 where (地址 <>'' and 地址 is not null ) ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetCpName()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select guid, 公司名稱, isnull(事業部,'')+isnull(營業處廠,'')+isnull(中心庫區儲運課工場,'') as cpname, guid, 管線管理不顯示, 儲槽設施不顯示, 單位屬性, 資料是否確認 from 天然氣_業者基本資料表
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

	public DataTable GetCpName2()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select 公司名稱, isnull(營業處廠,'')+isnull(中心庫區儲運課工場,'') as cpname, 代碼 from 天然氣_業者基本資料表
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

        sb.Append(@"select guid, 公司名稱, isnull(營業處廠,'')+isnull(中心庫區儲運課工場,'') as cpname from 天然氣_業者基本資料表
  where 資料狀態='A' and guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetInfo()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_業者基本資料表 where 資料狀態='A' and guid=@guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@guid", guid);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetDistinctCpName(string selectName)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select distinct " + selectName);
		
		sb.Append(@" as showName from 天然氣_業者基本資料表
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

	public DataTable GetCpNameStatistics()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select 
'02' as 類別, guid, 公司名稱, 事業部, 營業處廠, 中心庫區儲運課工場, 
pipeCount=(select count(*) as allCount from 天然氣_管線基本資料 where 業者guid=天然氣_業者基本資料表.guid and 資料狀態='A' ), 
tankCount=(select count(*) as allCount from 天然氣_儲槽設施資料_儲槽基本資料 where 業者guid=天然氣_業者基本資料表.guid and 資料狀態='A' )  
from 天然氣_業者基本資料表 
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

  set @管線數量總合 = (select count(*) from 天然氣_管線基本資料 a left join 天然氣_業者基本資料表 b on a.業者guid=b.guid 
  where (@公司名稱='' or b.公司名稱=@公司名稱) and (@事業部='' or b.事業部=@事業部) and (@營業處廠='' or b.營業處廠=@營業處廠) 
  and (@中心庫區儲運課工場='' or b.中心庫區儲運課工場=@中心庫區儲運課工場) and a.資料狀態='A' and b.資料狀態='A' and (@地址='' or left(b.地址,3) = @地址))  
  set @管線長度總合 = (select isnull(sum(convert(float, a.轄管長度)),0) from 天然氣_管線基本資料 a left join 天然氣_業者基本資料表 b on a.業者guid=b.guid 
  where (@公司名稱='' or b.公司名稱=@公司名稱) and (@事業部='' or b.事業部=@事業部) and (@營業處廠='' or b.營業處廠=@營業處廠) 
  and (@中心庫區儲運課工場='' or b.中心庫區儲運課工場=@中心庫區儲運課工場) and a.資料狀態='A' and b.資料狀態='A' and (@地址='' or left(b.地址,3) = @地址)) 
  set @儲槽數量總合 = (select count(*) from 天然氣_儲槽設施資料_儲槽基本資料 a left join 天然氣_業者基本資料表 b on a.業者guid=b.guid 
  where (@公司名稱='' or b.公司名稱=@公司名稱) and (@事業部='' or b.事業部=@事業部) and (@營業處廠='' or b.營業處廠=@營業處廠) 
  and (@中心庫區儲運課工場='' or b.中心庫區儲運課工場=@中心庫區儲運課工場) and a.資料狀態='A' and b.資料狀態='A' and (@地址='' or left(b.地址,3) = @地址))  
  set @儲槽容量總合 = (select isnull(sum(convert(float, a.容量)),0) from 天然氣_儲槽設施資料_儲槽基本資料 a left join 天然氣_業者基本資料表 b on a.業者guid=b.guid 
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

	public void UpdateData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        update 天然氣_業者基本資料表 set 
        資料是否確認=@資料是否確認, 
        修改者=@修改者, 
        修改日期=@修改日期 
        where guid=@guid and 資料狀態=@資料狀態 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@資料是否確認", 資料是否確認);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }
}