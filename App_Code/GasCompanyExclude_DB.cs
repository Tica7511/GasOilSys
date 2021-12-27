using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasCompanyExclude_DB 的摘要描述
/// </summary>
public class GasCompanyExclude_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }

	#region private
	string id = string.Empty;
	string 業者guid = string.Empty;
	string 年份 = string.Empty;
	string 排除題目guid = string.Empty;
	string 排除分類guid = string.Empty;
	string 建立者 = string.Empty;
	DateTime 建立日期;
	string 修改者 = string.Empty;
	DateTime 修改日期;
	string 資料狀態 = string.Empty;
	#endregion
	#region public
	public string _id { set { id = value; } }
	public string _業者guid { set { 業者guid = value; } }
	public string _年份 { set { 年份 = value; } }
	public string _排除題目guid { set { 排除題目guid = value; } }
	public string _排除分類guid { set { 排除分類guid = value; } }
	public string _建立者 { set { 建立者 = value; } }
	public DateTime _建立日期 { set { 建立日期 = value; } }
	public string _修改者 { set { 修改者 = value; } }
	public DateTime _修改日期 { set { 修改日期 = value; } }
	public string _資料狀態 { set { 資料狀態 = value; } }
	#endregion

	public DataTable GetList()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
select a.年份,b.天然氣自評表分類名稱,a.排除分類guid,a.排除題目guid
from 天然氣_自評表業者排除表 a
left join 天然氣_自評表分類檔 b on a.排除分類guid=b.天然氣自評表分類guid
where 業者guid=@業者guid and 年份=@年份 and 資料狀態='A'
union
select
a.年份,b.天然氣自評表分類名稱,case when c.天然氣自評表分類guid is null then a.排除分類guid else c.天然氣自評表分類guid end as 排除分類guid
,a.排除題目guid
from 天然氣_自評表業者排除表 a
left join 天然氣_自評表分類檔 b on a.排除分類guid=b.天然氣自評表分類guid
left join 天然氣_自評表分類檔 c on a.排除分類guid=c.天然氣自評表分類父層guid and b.天然氣自評表分類階層=c.天然氣自評表分類階層-1
where 業者guid=@業者guid and 年份=@年份 and 資料狀態='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
		oCmd.Parameters.AddWithValue("@年份", 年份);

		oda.Fill(ds);
		return ds;
	}
}