﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// MemberPwdLog_DB 的摘要描述
/// </summary>
public class MemberPwdLog_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string 會員guid = string.Empty;
    string 修改後密碼 = string.Empty;
    string IP = string.Empty;
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _會員guid { set { 會員guid = value; } }
    public string _修改後密碼 { set { 修改後密碼 = value; } }
    public string _IP { set { IP = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    #endregion

    public void addLog()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"insert into 會員密碼修改Log (
會員guid,
修改後密碼,
IP,
建立者,
修改者,
資料狀態
) values (
@會員guid,
@修改後密碼,
@IP,
@建立者,
@修改者,
@資料狀態
) ";

        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@會員guid", 會員guid);
        oCmd.Parameters.AddWithValue("@修改後密碼", 修改後密碼);
        oCmd.Parameters.AddWithValue("@IP", IP);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable CheckRepeatPw()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select top 3 * into #tmp from 會員密碼修改Log where 會員guid=@會員guid and 資料狀態='A' order by 建立日期 desc

select * from #tmp where 修改後密碼=@修改後密碼 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@會員guid", 會員guid);
        oCmd.Parameters.AddWithValue("@修改後密碼", 修改後密碼);
        oda.Fill(ds);
        return ds;
    }
}