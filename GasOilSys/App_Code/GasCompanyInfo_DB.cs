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

	public DataTable GetCpName()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select guid, 公司名稱, isnull(事業部,'')+isnull(營業處廠,'')+isnull(中心庫區儲運課工場,'') as cpname, guid, 管線管理不顯示, 儲槽設施不顯示, 單位屬性, 資料是否確認 from 天然氣_業者基本資料表
  where 資料狀態='A' and 列表是否顯示='Y' ");
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