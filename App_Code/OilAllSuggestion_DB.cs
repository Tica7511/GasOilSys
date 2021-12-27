using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilAllSuggestion_DB 的摘要描述
/// </summary>
public class OilAllSuggestion_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }

    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 委員 = string.Empty;
    string 委員guid = string.Empty;
    string 題目guid = string.Empty;
    string 年度 = string.Empty;
    string 檢視文件 = string.Empty;
    string 委員意見 = string.Empty;
    string 是否列入查核意見 = string.Empty;
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
    public string _委員guid { set { 委員guid = value; } }
    public string _委員 { set { 委員 = value; } }
    public string _題目guid { set { 題目guid = value; } }
    public string _年度 { set { 年度 = value; } }
    public string _檢視文件 { set { 檢視文件 = value; } }
    public string _委員意見 { set { 委員意見 = value; } }
    public string _是否列入查核意見 { set { 是否列入查核意見 = value; } }
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

        sb.Append(@"select [guid]
      ,[委員guid]
      ,[委員]
      ,[業者guid]
      ,[題目guid]
      ,[年度]
      ,[檢視文件]
      ,[委員意見]
      ,[是否列入查核意見]
      ,[建立者]
      ,[建立日期]
      ,[修改者]
      , CONVERT(nvarchar(100),[修改日期], 20) as 修改日期
      ,[資料狀態]
from 石油_自評表委員總評 
where 業者guid=@業者guid and 題目guid=@題目guid and 年度=@年度 and 資料狀態=@資料狀態
order by 修改日期 desc ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@題目guid", 題目guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * 
from 石油_自評表委員總評
WHERE guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public void SaveSuggestion(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into  石油_自評表委員總評( 
                    guid,
                    委員guid,
                    委員,
                    業者guid,
                    題目guid,
                    年度,
                    檢視文件,
                    委員意見,
                    是否列入查核意見,
                    建立者,
                    建立日期,
                    修改者,
                    修改日期,
                    資料狀態 
                    ) values (
                    @guid,
                    @委員guid,
                    @委員,
                    @業者guid,
                    @題目guid,
                    @年度,
                    @檢視文件,
                    @委員意見,
                    @是否列入查核意見,
                    @建立者,
                    @建立日期,
                    @修改者,
                    @修改日期,
                    @資料狀態 
                    )");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@委員guid", 委員guid);
        oCmd.Parameters.AddWithValue("@委員", 委員);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@題目guid", 題目guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@檢視文件", 檢視文件);
        oCmd.Parameters.AddWithValue("@委員意見", 委員意見);
        oCmd.Parameters.AddWithValue("@是否列入查核意見", 是否列入查核意見);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateSuggestion(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
update 石油_自評表委員總評 
set 委員意見=@委員意見, 是否列入查核意見=@是否列入查核意見, 
檢視文件=@檢視文件, 修改者=@修改者, 修改日期=@修改日期  
where guid=@guid and 年度=@年度 and 資料狀態=@資料狀態 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@委員意見", 委員意見);
        oCmd.Parameters.AddWithValue("@是否列入查核意見", 是否列入查核意見);
        oCmd.Parameters.AddWithValue("@檢視文件", 檢視文件);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public DataTable DelSuggestion(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
update 石油_自評表委員總評 
set 資料狀態='D', 修改者=@修改者, 修改日期=@修改日期  
where guid=@guid 

select * from 石油_自評表委員總評 
where 業者guid=@業者guid and 年度=@年度 and 題目guid = @題目guid and 資料狀態='A' 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();
        oCmd.Parameters.AddWithValue("@題目guid", 題目guid);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();

        oda.Fill(ds);
        return ds;
    }
}