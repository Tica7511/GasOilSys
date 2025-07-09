using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// PublicGas_DB 的摘要描述
/// </summary>
public class PublicGas_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 父層guid = string.Empty;
	string 排序編號 = string.Empty;
	string 公司名稱 = string.Empty;
	string 場佔位置 = string.Empty;
	string 組織階層 = string.Empty;
	string 地址 = string.Empty;
	string 電話 = string.Empty;
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
	public string _排序編號 { set { 排序編號 = value; } }
	public string _公司名稱 { set { 公司名稱 = value; } }
	public string _場佔位置 { set { 場佔位置 = value; } }
	public string _組織階層 { set { 組織階層 = value; } }
	public string _地址 { set { 地址 = value; } }
	public string _電話 { set { 電話 = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }
	#endregion

	public DataTable GetCompanyList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select 
* from 公用天然氣_業者基本資料表 
where 資料狀態='A' order by 公司名稱 ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oda.Fill(ds);
		return ds;
	}

    public DataTable GetCompanyDetail()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select 
* from 公用天然氣_業者基本資料表 
where 資料狀態='A' and guid=@guid 
order by 公司名稱 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oda.Fill(ds);
        return ds;
    }
}