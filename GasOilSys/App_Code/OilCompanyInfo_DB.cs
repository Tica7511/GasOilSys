using System;
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
	string 管線數量 = string.Empty;
	string 維運計畫書及成果報告 = string.Empty;
	string 曾執行過查核日期 = string.Empty;
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
	public string _管線數量 { set { 管線數量 = value; } }
	public string _維運計畫書及成果報告 { set { 維運計畫書及成果報告 = value; } }
	public string _曾執行過查核日期 { set { 曾執行過查核日期 = value; } }
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

    public DataTable GetCpName()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select isnull(處,'')+isnull(事業部,'')+isnull(營業處廠,'')+isnull(組,'')+isnull(中心庫區儲運課工場,'') as cpname from 石油_業者基本資料
  where 資料狀態='A' and 列表是否顯示='Y' and guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

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

    public DataTable GetCompanyList(string mGuid)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 石油_業者基本資料 c
left join 石油_委員業者年度對應表 m on c.guid=m.業者guid and m.資料狀態='A' and m.委員guid=@mGuid 
where c.資料狀態='A' and c.列表是否顯示='Y' ");

        if (mGuid != "")
            sb.Append(@"and m.委員guid=@mGuid ");
        sb.Append(@"order by 排序編號 ");

        oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@mGuid", mGuid);

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

    public void UpdateCompanyInfo(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        update 石油_業者基本資料 set
        電話=@電話, 
        地址=@地址, 
        儲槽數量=@儲槽數量, 
        管線數量=@管線數量,  
        維運計畫書及成果報告=@維運計畫書及成果報告, 
        曾執行過查核日期=@曾執行過查核日期,
        修改者=@修改者,
        修改日期=@修改日期
        where guid=@guid and 資料狀態=@資料狀態 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@電話", 電話);
        oCmd.Parameters.AddWithValue("@地址", 地址);
        oCmd.Parameters.AddWithValue("@儲槽數量", 儲槽數量);
        oCmd.Parameters.AddWithValue("@管線數量", 管線數量);
        oCmd.Parameters.AddWithValue("@維運計畫書及成果報告", 維運計畫書及成果報告);
        oCmd.Parameters.AddWithValue("@曾執行過查核日期", 曾執行過查核日期);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }
}