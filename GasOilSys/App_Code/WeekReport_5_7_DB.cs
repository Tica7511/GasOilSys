using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// WeekReport_5_7_DB 的摘要描述
/// </summary>
public class WeekReport_5_7_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 父層guid = string.Empty;
    string 工作項次 = string.Empty;
    string 備註 = string.Empty;
    string 季度 = string.Empty;
    string 年度 = string.Empty;
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;

    //審閱進度
    string 公用天然氣事業 = string.Empty;
    string 能源局發文日期 = string.Empty;
    string 業者繳交日期 = string.Empty;
    string 工研院審閱日期 = string.Empty;
    string 補正情形 = string.Empty;

    //其他防災事項
    string 執行內容 = string.Empty;
    string 預定日期起 = string.Empty;
    string 預定日期迄 = string.Empty;

    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _guid { set { guid = value; } }
    public string _父層guid { set { 父層guid = value; } }
    public string _工作項次 { set { 工作項次 = value; } }
    public string _備註 { set { 備註 = value; } }
    public string _季度 { set { 季度 = value; } }
    public string _年度 { set { 年度 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }

    //審閱進度
    public string _公用天然氣事業 { set { 公用天然氣事業 = value; } }
    public string _能源局發文日期 { set { 能源局發文日期 = value; } }
    public string _業者繳交日期 { set { 業者繳交日期 = value; } }
    public string _工研院審閱日期 { set { 工研院審閱日期 = value; } }
    public string _補正情形 { set { 補正情形 = value; } }

    //其他防災事項
    public string _執行內容 { set { 執行內容 = value; } }
    public string _預定日期起 { set { 預定日期起 = value; } }
    public string _預定日期迄 { set { 預定日期迄 = value; } }

    #endregion

    public DataTable GetExportList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"SELECT ROW_NUMBER() OVER(ORDER BY 工研院審閱日期) AS 場次,* from 週報_計劃書5_7_審閱進度 where 年度=@年度 and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetExportList2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"SELECT ROW_NUMBER() OVER(ORDER BY 預定日期起) AS 場次,* from 週報_計劃書5_7_其他災防事項 where 年度=@年度 and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetList1(string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"SELECT * into #tmp from 週報_計劃書5_7_審閱進度 where 年度=@年度 and 資料狀態='A' ");

        sb.Append(@"
select count(*) as total from #tmp

select * from (
           select ROW_NUMBER() over (order by 工研院審閱日期) 場次,* from #tmp
)#tmp where 場次 between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetList2(string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"SELECT * into #tmp from 週報_計劃書5_7_其他災防事項 where 年度=@年度 and 資料狀態='A' ");

        sb.Append(@"
select count(*) as total from #tmp

select * from (
           select ROW_NUMBER() over (order by 預定日期起) 場次,* from #tmp
)#tmp where 場次 between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }

    public void SaveReport1(SqlConnection oConn, SqlTransaction oTran, string mode)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
if @mode = 'new'
    begin
        insert into 週報_計劃書5_7_審閱進度(guid,
        父層guid,
        工作項次,
        公用天然氣事業,
        能源局發文日期,
        業者繳交日期,
        工研院審閱日期,
        補正情形,
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
        @公用天然氣事業,
        @能源局發文日期,
        @業者繳交日期,
        @工研院審閱日期,
        @補正情形,
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
        update 週報_計劃書5_7_審閱進度 set
        公用天然氣事業=@公用天然氣事業,
        能源局發文日期=@能源局發文日期,
        業者繳交日期=@業者繳交日期,
        工研院審閱日期=@工研院審閱日期,
        補正情形=@補正情形,
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
        oCmd.Parameters.AddWithValue("@公用天然氣事業", 公用天然氣事業);
        oCmd.Parameters.AddWithValue("@能源局發文日期", 能源局發文日期);
        oCmd.Parameters.AddWithValue("@業者繳交日期", 業者繳交日期);
        oCmd.Parameters.AddWithValue("@工研院審閱日期", 工研院審閱日期);
        oCmd.Parameters.AddWithValue("@補正情形", 補正情形);
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

    public void SaveReport2(SqlConnection oConn, SqlTransaction oTran, string mode)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
if @mode = 'new'
    begin
        insert into 週報_計劃書5_7_其他災防事項(guid,
        父層guid,
        工作項次,
        執行內容,
        預定日期起,
        預定日期迄,
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
        @執行內容,
        @預定日期起,
        @預定日期迄,
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
        update 週報_計劃書5_7_其他災防事項 set
        執行內容=@執行內容,
        預定日期起=@預定日期起,
        預定日期迄=@預定日期迄,
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
        oCmd.Parameters.AddWithValue("@執行內容", 執行內容);
        oCmd.Parameters.AddWithValue("@預定日期起", 預定日期起);
        oCmd.Parameters.AddWithValue("@預定日期迄", 預定日期迄);
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

    public void DeleteReport1(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 週報_計劃書5_7_審閱進度 set
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

    public void DeleteReport2(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 週報_計劃書5_7_其他災防事項 set
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