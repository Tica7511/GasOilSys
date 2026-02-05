using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilCommitteeSuggestionData_DB 的摘要描述
/// </summary>
public class OilCommitteeSuggestionData_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;
    string 標題 = string.Empty;
    string 狀態 = string.Empty;
    string 類型 = string.Empty;
    string 排序 = string.Empty;
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _guid { set { guid = value; } }
    public string _業者guid { set { 業者guid = value; } }
    public string _年度 { set { 年度 = value; } }
    public string _標題 { set { 標題 = value; } }
    public string _狀態 { set { 狀態 = value; } }
    public string _類型 { set { 類型 = value; } }
    public string _排序 { set { 排序 = value; } }
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
select A.guid, A.年度, A.標題, A.狀態, A.類型, A.排序, A.資料狀態, A.修改日期, 
狀態_V=(select 項目名稱 from 代碼檔 where 群組代碼='048' and 項目代碼=A.狀態),
B.業者guid, CONVERT(nvarchar(100),B.建立日期, 20) as 上傳日期 
from 石油_查核建議表 A 
left join 附件檔 B on A.guid=B.guid
where A.資料狀態='A' and B.排序= '1' 
order by 修改日期 desc 
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select * 
from 石油_查核建議表  
where 資料狀態='A' and guid=@guid
");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oda.Fill(ds);
        return ds;
    }

    public DataTable GetMaxSn(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @maxSort int 

select @maxSort=max(CONVERT(int, 排序)) from 石油_查核建議表 
where 資料狀態='A' and 類型=@類型 

SELECT CONVERT(int, @maxSort)+1 as Sort
");

        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();
        oCmd.Transaction = oTran;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@類型", 類型);

        oda.Fill(ds);
        return ds;
    }

    public void InsertData_Trans(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
insert into 石油_查核建議表 (
guid,
業者guid,
年度,
標題,
狀態,
類型,
排序,
建立者,
修改者,
資料狀態
) values (
@guid,
@業者guid,
@年度,
@標題,
@狀態,
@類型, 
@排序,
@建立者,
@修改者,
@資料狀態 
) 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@標題", 標題);
        oCmd.Parameters.AddWithValue("@狀態", 狀態);
        oCmd.Parameters.AddWithValue("@類型", 類型);
        oCmd.Parameters.AddWithValue("@排序", 排序);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 石油_查核建議表 set  
guid=@guid,
狀態=@狀態,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@狀態", 狀態);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DelOnlyOfficeFile(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 石油_查核建議表 set
            資料狀態=@資料狀態,
            修改者=@修改者,
            修改日期=@修改日期
            where guid=@guid  
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "D");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }
}