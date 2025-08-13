using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Oil_CommitteeSuggestionDemoFile_DB 的摘要描述
/// </summary>
public class Oil_CommitteeSuggestionDemoFile_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region 私用
    string id = string.Empty;
    string 項目名稱 = string.Empty;
    string 項目代碼 = string.Empty;
    #endregion
    #region 公用
    public string _id
    {
        set { id = value; }
    }
    public string _項目名稱
    {
        set { 項目名稱 = value; }
    }
    public string _項目代碼
    {
        set { 項目代碼 = value; }
    }
    #endregion

    public DataTable GetList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"SELECT * from 石油_查核建議範本 where 項目代碼=@項目代碼 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@項目代碼", 項目代碼);

        oda.Fill(ds);
        return ds;
    }
}