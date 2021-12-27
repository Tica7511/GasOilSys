using System;
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

    public DataTable GetData()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select * from 天然氣_控制室 where 資料狀態='A' and 業者guid=@業者guid ");

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

        sb.Append(@"select * from 天然氣_控制室_依據文件資料 where 資料狀態='A' and 業者guid=@業者guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

        oda.Fill(ds);
        return ds;
    }
}