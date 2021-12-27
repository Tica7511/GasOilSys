using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;


/// <summary>
/// WeekReport_6_1_DB 的摘要描述
/// </summary>
public class WeekReport_6_1_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 父層guid = string.Empty;
    string 工作項次 = string.Empty;
    string 編號1 = string.Empty;
    string 編號2 = string.Empty;
    string 編號3 = string.Empty;
    string 編號4 = string.Empty;
    string 執行內容 = string.Empty;
    string 預定日期起 = string.Empty;
    string 預定日期迄 = string.Empty;
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
    public string _編號1 { set { 編號1 = value; } }
    public string _編號2 { set { 編號2 = value; } }
    public string _編號3 { set { 編號3 = value; } }
    public string _編號4 { set { 編號4 = value; } }
    public string _執行內容 { set { 執行內容 = value; } }
    public string _預定日期起 { set { 預定日期起 = value; } }
    public string _預定日期迄 { set { 預定日期迄 = value; } }
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

        sb.Append(@"select ROW_NUMBER() OVER(ORDER BY convert(int, 編號1), convert(int, 編號2), convert(int, 編號3), convert(int, 編號4)) AS 場次
,* from 週報_計劃書6_1 where 年度=@年度 and 資料狀態='A' ");

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
        insert into 週報_計劃書6_1(guid,
        父層guid,
        工作項次,
        編號1,
        編號2,
        編號3,
        編號4,
        執行內容,
        預定日期起,
        預定日期迄,
        季度,
        年度,
        建立者,
        建立日期,
        修改者,
        修改日期,
        資料狀態 ) values (@guid,
        @父層guid,
        @工作項次,
        @編號1,
        @編號2,
        @編號3,
        @編號4,
        @執行內容,
        @預定日期起,
        @預定日期迄,
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
        update 週報_計劃書6_1 set
        編號1=@編號1,
        編號2=@編號2,
        編號3=@編號3,
        編號4=@編號4,
        執行內容=@執行內容,
        預定日期起=@預定日期起,
        預定日期迄=@預定日期迄,
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
        oCmd.Parameters.AddWithValue("@編號1", 編號1);
        oCmd.Parameters.AddWithValue("@編號2", 編號2);
        oCmd.Parameters.AddWithValue("@編號3", 編號3);
        oCmd.Parameters.AddWithValue("@編號4", 編號4);
        oCmd.Parameters.AddWithValue("@執行內容", 執行內容);
        oCmd.Parameters.AddWithValue("@預定日期起", 預定日期起);
        oCmd.Parameters.AddWithValue("@預定日期迄", 預定日期迄);
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
        sb.Append(@"update 週報_計劃書6_1 set
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