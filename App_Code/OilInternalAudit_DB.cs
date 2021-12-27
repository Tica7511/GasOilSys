using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilInternalAudit_DB 的摘要描述
/// </summary>
public class OilInternalAudit_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;
    string 內部稽核日期 = string.Empty;
    string 執行單位 = string.Empty;
    string 稽核範圍 = string.Empty;
    string 缺失改善執行狀況 = string.Empty;
    string 佐證資料 = string.Empty;
    string 佐證資料檔名 = string.Empty;
    string 佐證資料副檔名 = string.Empty;
    string 佐證資料路徑 = string.Empty;
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
    public string _內部稽核日期 { set { 內部稽核日期 = value; } }
    public string _執行單位 { set { 執行單位 = value; } }
    public string _稽核範圍 { set { 稽核範圍 = value; } }
    public string _缺失改善執行狀況 { set { 缺失改善執行狀況 = value; } }
    public string _佐證資料 { set { 佐證資料 = value; } }
    public string _佐證資料檔名 { set { 佐證資料檔名 = value; } }
    public string _佐證資料副檔名 { set { 佐證資料副檔名 = value; } }
    public string _佐證資料路徑 { set { 佐證資料路徑 = value; } }
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

        sb.Append(@"select * from 石油_儲槽內部稽核表 where 資料狀態='A' and 業者guid=@業者guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_儲槽內部稽核表 where 資料狀態='A' and guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public void SaveFile(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        update 石油_儲槽內部稽核表 set
        佐證資料檔名=@佐證資料檔名,
        佐證資料副檔名=@佐證資料副檔名,
        佐證資料路徑=@佐證資料路徑,
        修改者=@修改者,
        修改日期=@修改日期
        where guid=@guid and 資料狀態=@資料狀態
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@佐證資料檔名", 佐證資料檔名);
        oCmd.Parameters.AddWithValue("@佐證資料副檔名", 佐證資料副檔名);
        oCmd.Parameters.AddWithValue("@佐證資料路徑", 佐證資料路徑);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public DataTable GetFileName()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_儲槽內部稽核表 where 資料狀態='A' and guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public void DelFile(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        update 石油_儲槽內部稽核表 
        set 佐證資料檔名='', 佐證資料副檔名='', 佐證資料路徑=''
        where guid=@guid 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }
}