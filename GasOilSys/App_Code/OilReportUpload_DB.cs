using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilReportUpload_DB 的摘要描述
/// </summary>
public class OilReportUpload_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;
    string 檔案名稱 = string.Empty;
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
    public string _檔案名稱 { set { 檔案名稱 = value; } }
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

        sb.Append(@"select 
guid
,業者guid
,年度
,檔案名稱
,建立者
,建立日期
,修改者
,修改日期
,CONVERT(nvarchar(100),修改日期, 20) as 上傳日期
,資料狀態 
from 石油_查核簡報上傳 where 資料狀態='A' and 業者guid=@業者guid ");

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

        sb.Append(@"select * from 石油_查核簡報上傳 where 資料狀態='A' and guid=@guid ");

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
--declare @datacount int
--select @datacount= count(*) from 石油_查核簡報上傳 where 年度=@年度 and 業者guid=@業者guid and 資料狀態=@資料狀態
--
--if @datacount>0
--    begin
--        update 石油_查核簡報上傳 set
--        檔案名稱=@檔案名稱,
--        修改者=@修改者,
--        修改日期=@修改日期
--        where 年度=@年度 and 業者guid=@業者guid and 資料狀態=@資料狀態
--    end
--else
--    begin
--        insert into  石油_查核簡報上傳( 
--        guid,
--        業者guid,
--        年度,
--        檔案名稱,
--        建立者,
--        建立日期,
--        修改者,
--        修改日期,
--        資料狀態
--        ) values (
--        @guid,
--        @業者guid,
--        @年度,
--        @檔案名稱,
--        @建立者,
--        @建立日期,
--        @修改者,
--        @修改日期,
--        @資料狀態 
--        )
--    end

insert into  石油_查核簡報上傳( 
        guid,
        業者guid,
        年度,
        檔案名稱,
        建立者,
        建立日期,
        修改者,
        修改日期,
        資料狀態
        ) values (
        @guid,
        @業者guid,
        @年度,
        @檔案名稱,
        @建立者,
        @建立日期,
        @修改者,
        @修改日期,
        @資料狀態 
        )
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@檔案名稱", 檔案名稱);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DelFile(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
        delete 石油_查核簡報上傳 
        where guid=@guid 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();
        
        oCmd.Parameters.AddWithValue("@guid", guid);

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }
}