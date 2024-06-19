using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Type_DB 的摘要描述
/// </summary>
public class CodeTable_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region 私用
    string id = string.Empty;
    string 群組名稱 = string.Empty;
    string 群組代碼 = string.Empty;
    string 項目名稱 = string.Empty;
    string 項目代碼 = string.Empty;
    string 排序 = string.Empty;
    #endregion
    #region 公用
    public string _id
    {
        set { id = value; }
    }
    public string _群組名稱
    {
        set { 群組名稱 = value; }
    }
    public string _群組代碼
    {
        set { 群組代碼 = value; }
    }
    public string _項目名稱
    {
        set { 項目名稱 = value; }
    }
    public string _項目代碼
    {
        set { 項目代碼 = value; }
    }
    public string _排序
    {
        set { 排序 = value; }
    }
    #endregion

    public DataTable GetList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"SELECT 項目名稱,項目代碼 from 代碼檔 where 群組代碼=@群組代碼 ");

        if(項目代碼 != "")
            sb.Append(@"and 項目代碼=@項目代碼 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@群組代碼", 群組代碼);
        oCmd.Parameters.AddWithValue("@項目代碼", 項目代碼);

        oda.Fill(ds);
        return ds;
    }

    public bool GetDataOnlyChineseIfExist(string codeGroup, string codeItemName)
    {
        bool status;
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"SELECT 項目名稱,項目代碼 from 代碼檔 where 群組代碼=@群組代碼 ");

        if (codeItemName != "")
            sb.Append(@"and 項目名稱=@項目名稱 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@群組代碼", codeGroup);
        oCmd.Parameters.AddWithValue("@項目名稱", codeItemName);

        oda.Fill(ds);

        if (ds.Rows.Count > 0)
        {
            status = true;
        }
        else
        {
            status = false;
        }

        return status;
    }
}