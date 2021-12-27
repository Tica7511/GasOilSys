using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Member_DB 的摘要描述
/// </summary>
public class Member_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 使用者帳號 = string.Empty;
    string 使用者密碼 = string.Empty;
    string 姓名 = string.Empty;
    string mail = string.Empty;
    string 電話 = string.Empty;
    string 單位 = string.Empty;
    string 單位名稱 = string.Empty;
    string 角色 = string.Empty;
    string 委員類別 = string.Empty;
    string 帳號類別 = string.Empty;
    string 職稱 = string.Empty;
    string 學歷 = string.Empty;
    string 相關經歷 = string.Empty;
    string 專業領域 = string.Empty;
    string 網站類別 = string.Empty;
    string 密碼需變更 = string.Empty;
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
    public string _使用者帳號 { set { 使用者帳號 = value; } }
    public string _使用者密碼 { set { 使用者密碼 = value; } }
    public string _姓名 { set { 姓名 = value; } }
    public string _mail { set { mail = value; } }
    public string _電話 { set { 電話 = value; } }
    public string _單位 { set { 單位 = value; } }
    public string _單位名稱 { set { 單位名稱 = value; } }
    public string _角色 { set { 角色 = value; } }
    public string _委員類別 { set { 委員類別 = value; } }
    public string _帳號類別 { set { 帳號類別 = value; } }
    public string _職稱 { set { 職稱 = value; } }
    public string _學歷 { set { 學歷 = value; } }
    public string _相關經歷 { set { 相關經歷 = value; } }
    public string _專業領域 { set { 專業領域 = value; } }
    public string _網站類別 { set { 網站類別 = value; } }
    public string _密碼需變更 { set { 密碼需變更 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    #endregion

    public void UpdatePwd()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 會員檔 set
使用者密碼=@使用者密碼,
修改日期=@修改日期,
修改者=@修改者
where guid=@guid
";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@使用者密碼", 使用者密碼);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable GetCommittee()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select ROW_NUMBER() OVER(ORDER BY 姓名) AS 場次,* from 會員檔 where 帳號類別='01' and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }
}