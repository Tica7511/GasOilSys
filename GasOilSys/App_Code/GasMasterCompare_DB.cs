using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasMasterCompare_DB 的摘要描述
/// </summary>
public class GasMasterCompare_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }

	#region private
	string id = string.Empty;
	string 年度 = string.Empty;
	string 委員guid = string.Empty;
	string 委員姓名 = string.Empty;
	string 業者guid = string.Empty;
	string 業者單位名稱 = string.Empty;
	string 負責評分類別 = string.Empty;
	string 建立者 = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;
	#endregion
	#region public
	public string _id { set { id = value; } }
	public string _年度 { set { 年度 = value; } }
	public string _委員guid { set { 委員guid = value; } }
	public string _委員姓名 { set { 委員姓名 = value; } }
	public string _業者guid { set { 業者guid = value; } }
	public string _業者單位名稱 { set { 業者單位名稱 = value; } }
	public string _負責評分類別 { set { 負責評分類別 = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }
	#endregion

	public DataTable GetMasterType()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select a.業者Guid,b.網站類別 from 天然氣_委員業者年度對應表 a
left join 會員檔 b on b.業者guid=a.業者Guid
where a.資料狀態='A' and a.委員guid=@委員guid and a.年度=@年度
group by a.業者Guid,b.網站類別 ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@委員guid", 委員guid);
		oCmd.Parameters.AddWithValue("@年度", 年度);

		oda.Fill(ds);
		return ds;
	}

    public DataTable GetMasterList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select 
       b.guid
      ,b.父層guid
      ,b.排序編號
      ,b.公司名稱
      ,b.事業部
      ,b.營業處廠
      ,b.中心庫區儲運課工場
      ,b.天然氣組織階層
      ,b.地址
      ,b.電話
      ,b.fun2
      ,b.建立者
      ,b.建立日期
      ,b.修改者
      ,b.修改日期
      ,b.資料狀態
      ,b.列表是否顯示
from 天然氣_委員業者年度對應表 a
left join 天然氣_業者基本資料表 b on b.guid=a.業者Guid
where a.資料狀態='A' and a.委員guid=@委員guid and a.年度=@年度 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@委員guid", 委員guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }
}