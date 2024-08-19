using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// LoginLog_DB 的摘要描述
/// </summary>
public class LoginLog_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string 帳號 = string.Empty;
    string 登入結果 = string.Empty;
    string 登入IP = string.Empty;
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _帳號 { set { 帳號 = value; } }
    public string _登入結果 { set { 登入結果 = value; } }
    public string _登入IP { set { 登入IP = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    #endregion

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select top 1 * from 會員登入Log 
where 帳號=@帳號 and 登入結果=@登入結果 and 
資料狀態=@資料狀態 
order by 建立日期 desc ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@帳號", 帳號);
        oCmd.Parameters.AddWithValue("@登入結果", "Success");
        oCmd.Parameters.AddWithValue("@資料狀態", "A");
        oda.Fill(ds);
        return ds;
    }

    public void addLog()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"insert into 會員登入Log (
帳號,
登入結果,
登入IP,
建立者,
修改者,
資料狀態
) values (
@帳號,
@登入結果,
@登入IP,
@建立者,
@修改者,
@資料狀態
) ";

        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@帳號", 帳號);
        oCmd.Parameters.AddWithValue("@登入結果", 登入結果);
        oCmd.Parameters.AddWithValue("@登入IP", 登入IP);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public void updateLog()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 
會員登入Log set 
資料狀態=@資料狀態,
修改日期=@修改日期 
where 帳號=@帳號 
) ";

        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@帳號", 帳號);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "D");


        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable CheckLoginStatus(string Account)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @checkTime int =15;--登入失敗要等幾分鐘
declare @continueTime int =15;--幾分鐘內連續登入失敗  @continueTime不能>@checkTime

declare @chkLastFail datetime;--最後一次登入失敗時間
declare @LCount int=0;--15分鐘內連續錯誤次數

select @chkLastFail = 建立日期
from 會員登入Log
where 帳號 = @acc
and 登入結果='Fail' --登入失敗

if DATEDIFF(MINUTE, @chkLastFail,getdate())  <= @checkTime
 begin
 select @LCount = count(*) 
  from 會員登入Log
  where 帳號 = @acc
  and 登入結果='Fail' --登入失敗
  and DATEDIFF(MINUTE, 建立日期,getdate())  <= @continueTime

  if @LCount>=3
   begin
    --15分鐘內累計登入失敗3次 回傳X
    select 'X' as reStatus
   end
  else
   begin
    --15分鐘內沒有累計登入失敗3次
    select 'Y' as reStatus
  end
 end
else
 begin
  select 'Y' as reStatus
 end ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@acc", Account);
        oda.Fill(ds);
        return ds;
    }
}