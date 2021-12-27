using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// WeekReport_2_2_2_DB 的摘要描述
/// </summary>
public class WeekReport_2_2_2_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 父層guid = string.Empty;
    string 工作項次 = string.Empty;
    string 受查單位 = string.Empty;
    string 管線名稱 = string.Empty;
    string 管線識別碼 = string.Empty;
    string 起點 = string.Empty;
    string 迄點 = string.Empty;
    string 預定日期起 = string.Empty;
    string 預定日期迄 = string.Empty;
    string 檢測長度 = string.Empty;
    string 備註 = string.Empty;
    string 季度 = string.Empty;
    string 年度 = string.Empty;
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _guid { set { guid = value; } }
    public string _父層guid { set { 父層guid = value; } }
    public string _工作項次 { set { 工作項次 = value; } }
    public string _受查單位 { set { 受查單位 = value; } }
    public string _管線名稱 { set { 管線名稱 = value; } }
    public string _管線識別碼 { set { 管線識別碼 = value; } }
    public string _起點 { set { 起點 = value; } }
    public string _迄點 { set { 迄點 = value; } }
    public string _預定日期起 { set { 預定日期起 = value; } }
    public string _預定日期迄 { set { 預定日期迄 = value; } }
    public string _檢測長度 { set { 檢測長度 = value; } }
    public string _備註 { set { 備註 = value; } }
    public string _季度 { set { 季度 = value; } }
    public string _年度 { set { 年度 = value; } }
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

        sb.Append(@"select ROW_NUMBER() OVER(ORDER BY 預定日期起) AS 場次,* from 週報_計劃書2_2_2 where 年度=@年度 and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public void SaveReport(SqlConnection oConn, SqlTransaction oTran, string mode)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
if @mode = 'new'
    begin
        insert into 週報_計劃書2_2_2(guid,
        父層guid,
        工作項次,
        受查單位,
        管線名稱,
        管線識別碼,
        起點,
        迄點,
        預定日期起,
        預定日期迄,
        檢測長度,
        備註,
        季度,
        年度,
        建立者,
        建立日期,
        修改者,
        修改日期,
        資料狀態 ) values (@guid,
        @父層guid,
        @工作項次,
        @受查單位,
        @管線名稱,
        @管線識別碼,
        @起點,
        @迄點,
        @預定日期起,
        @預定日期迄,
        @檢測長度,
        @備註,
        @季度,
        @年度,
        @建立者,
        @建立日期,
        @修改者,
        @修改日期,
        @資料狀態 )
    end
else
    begin
        update 週報_計劃書2_2_2 set
        受查單位=@受查單位,
        管線名稱=@管線名稱,
        管線識別碼=@管線識別碼,
        起點=@起點,
        迄點=@迄點,
        預定日期起=@預定日期起,
        預定日期迄=@預定日期迄,
        檢測長度=@檢測長度,
        備註=@備註,
        季度=@季度,
        年度=@年度,
        修改者=@修改者,
        修改日期=@修改日期
        where guid=@guid and 資料狀態=@資料狀態
    end 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@mode", mode);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@父層guid", 父層guid);
        oCmd.Parameters.AddWithValue("@工作項次", 工作項次);
        oCmd.Parameters.AddWithValue("@受查單位", 受查單位);
        oCmd.Parameters.AddWithValue("@管線名稱", 管線名稱);
        oCmd.Parameters.AddWithValue("@管線識別碼", 管線識別碼);
        oCmd.Parameters.AddWithValue("@起點", 起點);
        oCmd.Parameters.AddWithValue("@迄點", 迄點);
        oCmd.Parameters.AddWithValue("@預定日期起", 預定日期起);
        oCmd.Parameters.AddWithValue("@預定日期迄", 預定日期迄);
        oCmd.Parameters.AddWithValue("@檢測長度", 檢測長度);
        oCmd.Parameters.AddWithValue("@備註", 備註);
        oCmd.Parameters.AddWithValue("@季度", 季度);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DeleteReport(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 週報_計劃書2_2_2 set
            資料狀態=@資料狀態,
            修改者=@修改者,
            修改日期=@修改日期
            where guid=@guid  
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "D");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }
}