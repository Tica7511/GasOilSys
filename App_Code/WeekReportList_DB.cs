using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// WeekReportList_DB 的摘要描述
/// </summary>
public class WeekReportList_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 工作項次 = string.Empty;
    string 名稱 = string.Empty;
    string 父層guid = string.Empty;
    string 階層 = string.Empty;
    string 排序 = string.Empty;
    string 版本 = string.Empty;
    string 年份 = string.Empty;
    string 編輯顯示 = string.Empty;
    string 建立者 = string.Empty;
    string 建立日期 = string.Empty;
    string 修改者 = string.Empty;
    string 修改日期 = string.Empty;
    string 資料狀態 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _guid { set { guid = value; } }
    public string _工作項次 { set { 工作項次 = value; } }
    public string _名稱 { set { 名稱 = value; } }
    public string _父層guid { set { 父層guid = value; } }
    public string _階層 { set { 階層 = value; } }
    public string _排序 { set { 排序 = value; } }
    public string _版本 { set { 版本 = value; } }
    public string _年份 { set { 年份 = value; } }
    public string _編輯顯示 { set { 編輯顯示 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public string _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public string _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    #endregion

    public DataTable GetList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @cYear nvarchar(4)=@Year;

select (
select lv1.guid as lvGuid,lv1.工作項次 as lvNo,lv1.名稱 as lvName,lv1.階層 as lv, lv1.編輯顯示 as Dis
,lv2.guid as lvGuid,lv2.工作項次 as lvNo,lv2.名稱 as lvName,lv2.父層guid as pGuid,lv2.階層 as lv, lv2.編輯顯示 as Dis
,lv3.guid as lvGuid,lv3.工作項次 as lvNo,lv3.名稱 as lvName,lv3.父層guid as pGuid,lv3.階層 as lv, lv3.編輯顯示 as Dis
from 週報_計劃書分類檔 lv1
left join 週報_計劃書分類檔 lv2 on lv1.guid=lv2.父層guid and lv2.資料狀態='A' and lv2.年份=@cYear
left join 週報_計劃書分類檔 lv3 on lv2.guid=lv3.父層guid and lv3.資料狀態='A' and lv3.年份=@cYear
where lv1.階層='1' and lv1.資料狀態='A' and lv1.年份=@cYear
order by CONVERT(int,lv1.排序),CONVERT(int,lv2.排序),CONVERT(int,lv3.排序)
for xml auto,root('root')
) as xmlDoc ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@Year", 年份);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetWeekReportExport(string sn)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from  週報_計劃書分類檔 where 工作項次=@工作項次 and 年份=@年份 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@工作項次", sn);
        oCmd.Parameters.AddWithValue("@年份", 年份);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from  週報_計劃書分類檔 where guid=@guid and 年份=@年份 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年份", 年份);

        oda.Fill(ds);
        return ds;
    }

    public DataTable UpdateReport(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
update 週報_計劃書分類檔 
set 名稱=@名稱, 修改者=@修改者, 修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 and 年份=@年份 

select * from 週報_計劃書分類檔 
where guid=@guid and 年份=@年份 and 資料狀態=@資料狀態
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@名稱", 名稱);
        oCmd.Parameters.AddWithValue("@年份", 年份);
        oCmd.Parameters.AddWithValue("@修改者", LogInfo.mGuid);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetYearList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select 年份 as year 
from 週報_計劃書分類檔
group by 年份 
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }
}