using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasTubeCheck_DB 的摘要描述
/// </summary>
public class GasTubeCheck_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 依據文件名稱 = string.Empty;
	string 巡管人數 = string.Empty;
	string 巡管工具 = string.Empty;
	string 巡管工具其他 = string.Empty;
	string 主管監督查核 = string.Empty;
	string 主管監督查核次 = string.Empty;
	string 是否有加強巡檢點 = string.Empty;
	string 是否有加強巡檢點敘述 = string.Empty;
	string 建立者 = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;

    //依據文件資料
    string 文件編號 = string.Empty;
    string 文件日期 = string.Empty;
    string 每日巡檢次數 = string.Empty;

    // 異常情形統計資料
    string 管線巡檢情形 = string.Empty;
	string 前兩年 = string.Empty;
	string 前一年 = string.Empty;
	#endregion
	#region public
	public string _id { set { id = value; } }
	public string _guid { set { guid = value; } }
	public string _業者guid { set { 業者guid = value; } }
	public string _年度 { set { 年度 = value; } }
	public string _每日巡檢次數 { set { 每日巡檢次數 = value; } }
	public string _巡管人數 { set { 巡管人數 = value; } }
	public string _巡管工具 { set { 巡管工具 = value; } }
	public string _巡管工具其他 { set { 巡管工具其他 = value; } }
	public string _主管監督查核 { set { 主管監督查核 = value; } }
	public string _主管監督查核次 { set { 主管監督查核次 = value; } }
	public string _是否有加強巡檢點 { set { 是否有加強巡檢點 = value; } }
	public string _是否有加強巡檢點敘述 { set { 是否有加強巡檢點敘述 = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }

    //依據文件資料
    public string _依據文件名稱 { set { 依據文件名稱 = value; } }
    public string _文件編號 { set { 文件編號 = value; } }
    public string _文件日期 { set { 文件日期 = value; } }

    // 異常情形統計資料
    public string _管線巡檢情形 { set { 管線巡檢情形 = value; } }
	public string _前兩年 { set { 前兩年 = value; } }
	public string _前一年 { set { 前一年 = value; } }
	#endregion

	public DataTable GetData()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_管線巡檢 where 資料狀態='A' and 業者guid=@業者guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}

    public DataTable GetList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 天然氣_管線巡檢_依據文件資料 where 資料狀態='A' and 業者guid=@業者guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetList2()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_管線巡檢_異常情形統計資料 where 資料狀態='A' and 業者guid=@業者guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}
}