using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// SelfEvaluaion_DB 的摘要描述
/// </summary>
public class GasSelfEvaluation_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region 天然氣_自評表分類檔 private
	string 天然氣自評表分類id = string.Empty;
	string 天然氣自評表分類guid = string.Empty;
	string 天然氣自評表分類負責類別 = string.Empty;
	string 天然氣自評表分類名稱 = string.Empty;
	string 天然氣自評表分類父層guid = string.Empty;
	string 天然氣自評表分類階層 = string.Empty;
	string 天然氣自評表分類排序 = string.Empty;
	string 天然氣自評表分類狀態 = string.Empty;
	string 天然氣自評表分類版本 = string.Empty;
	string 天然氣自評表分類年份 = string.Empty;
	#endregion
	#region 天然氣_自評表分類檔 public
	public string _天然氣自評表分類id { set { 天然氣自評表分類id = value; } }
	public string _天然氣自評表分類guid { set { 天然氣自評表分類guid = value; } }
	public string _天然氣自評表分類負責類別 { set { 天然氣自評表分類負責類別 = value; } }
	public string _天然氣自評表分類名稱 { set { 天然氣自評表分類名稱 = value; } }
	public string _天然氣自評表分類父層guid { set { 天然氣自評表分類父層guid = value; } }
	public string _天然氣自評表分類階層 { set { 天然氣自評表分類階層 = value; } }
	public string _天然氣自評表分類排序 { set { 天然氣自評表分類排序 = value; } }
	public string _天然氣自評表分類狀態 { set { 天然氣自評表分類狀態 = value; } }
	public string _天然氣自評表分類版本 { set { 天然氣自評表分類版本 = value; } }
	public string _天然氣自評表分類年份 { set { 天然氣自評表分類年份 = value; } }
	#endregion

	#region 天然氣_自評表題目檔 private
	string 天然氣自評表題目id = string.Empty;
	string 天然氣自評表題目guid = string.Empty;
	string 天然氣自評表題目分類guid = string.Empty;
	string 天然氣自評表題目年份 = string.Empty;
	string 天然氣自評表題目名稱 = string.Empty;
	string 天然氣自評表題目排序 = string.Empty;
	string 天然氣自評表題目狀態 = string.Empty;
	string 天然氣自評表題目版本 = string.Empty;
	#endregion
	#region 天然氣_自評表題目檔 public
	public string _天然氣自評表題目id { set { 天然氣自評表題目id = value; } }
	public string _天然氣自評表題目guid { set { 天然氣自評表題目guid = value; } }
	public string _天然氣自評表題目分類guid { set { 天然氣自評表題目分類guid = value; } }
	public string _天然氣自評表題目年份 { set { 天然氣自評表題目年份 = value; } }
	public string _天然氣自評表題目名稱 { set { 天然氣自評表題目名稱 = value; } }
	public string _天然氣自評表題目排序 { set { 天然氣自評表題目排序 = value; } }
	public string _天然氣自評表題目狀態 { set { 天然氣自評表題目狀態 = value; } }
	public string _天然氣自評表題目版本 { set { 天然氣自評表題目版本 = value; } }
	#endregion

	public DataTable GetQuestionList(string Year)
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"
declare @cYear nvarchar(4)=@Year;

select (
select lv1.天然氣自評表分類guid as lvGuid,lv1.天然氣自評表分類名稱 as lvName,lv1.天然氣自評表分類階層 as lv,isnull(lv1.天然氣自評表分類參考資料,'') as ref, isnull(lv1.天然氣自評表是否總評,'') as psall, isnull(lv1.天然氣自評表是否紅字,'') as psred
,lv2.天然氣自評表分類guid as lvGuid,lv2.天然氣自評表分類名稱 as lvName,lv2.天然氣自評表分類父層guid as pGuid,lv2.天然氣自評表分類階層 as lv,isnull(lv2.天然氣自評表分類參考資料,'') as ref, isnull(lv2.天然氣自評表是否總評,'') as psall, isnull(lv2.天然氣自評表是否紅字,'') as psred
,lv3.天然氣自評表分類guid as lvGuid,lv3.天然氣自評表分類名稱 as lvName,lv3.天然氣自評表分類父層guid as pGuid,lv3.天然氣自評表分類階層 as lv,isnull(lv3.天然氣自評表分類參考資料,'') as ref, isnull(lv3.天然氣自評表是否總評,'') as psall, isnull(lv3.天然氣自評表是否紅字,'') as psred
,q.天然氣自評表題目guid as qGuid,q.天然氣自評表題目名稱 as qTitle,q.天然氣自評表題目分類guid as pGuid
from 天然氣_自評表分類檔 lv1
left join 天然氣_自評表分類檔 lv2 on lv1.天然氣自評表分類guid=lv2.天然氣自評表分類父層guid and lv2.天然氣自評表分類狀態='A' and lv2.天然氣自評表分類年份=@cYear
left join 天然氣_自評表分類檔 lv3 on lv2.天然氣自評表分類guid=lv3.天然氣自評表分類父層guid and lv3.天然氣自評表分類狀態='A' and lv3.天然氣自評表分類年份=@cYear
left join 天然氣_自評表題目檔 q on (
    case when lv3.[天然氣自評表分類guid] is not null then lv3.[天然氣自評表分類guid]
         when lv3.[天然氣自評表分類guid] is null and lv2.[天然氣自評表分類guid] is not null then lv2.[天然氣自評表分類guid]
         when lv3.[天然氣自評表分類guid] is null and lv2.[天然氣自評表分類guid] is null and lv1.[天然氣自評表分類guid] is not null then lv1.[天然氣自評表分類guid]
    else '' end
) = q.天然氣自評表題目分類guid and q.天然氣自評表題目狀態='A'
where lv1.天然氣自評表分類階層='1' and lv1.天然氣自評表分類狀態='A' and lv1.天然氣自評表分類年份=@cYear
order by CONVERT(int,lv1.天然氣自評表分類排序),CONVERT(int,lv2.天然氣自評表分類排序),CONVERT(int,lv3.天然氣自評表分類排序),CONVERT(int,q.天然氣自評表題目排序)
for xml auto,root('root')
) as xmlDoc ");
		
		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@Year", Year);

		oda.Fill(ds);
		return ds;
	}

	public DataTable GetQuestionGuid()
	{
		SqlCommand oCmd = new SqlCommand();
		oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
		StringBuilder sb = new StringBuilder();

		sb.Append(@"select 天然氣自評表題目guid,天然氣自評表題目年份 from 天然氣_自評表題目檔 where 天然氣自評表題目狀態='A' ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		//oCmd.Parameters.AddWithValue("@A", A);

		oda.Fill(ds);
		return ds;
	}
}