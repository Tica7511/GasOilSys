using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// FileTable 的摘要描述
/// </summary>
public class FileTable
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;
    string 檔案類型 = string.Empty;
    string 原檔名 = string.Empty;
    string 新檔名 = string.Empty;
    string 附檔名 = string.Empty;
    string 排序 = string.Empty;
    string 檔案大小 = string.Empty;
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
    public string _檔案類型 { set { 檔案類型 = value; } }
    public string _原檔名 { set { 原檔名 = value; } }
    public string _新檔名 { set { 新檔名 = value; } }
    public string _附檔名 { set { 附檔名 = value; } }
    public string _排序 { set { 排序 = value; } }
    public string _檔案大小 { set { 檔案大小 = value; } }
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

        sb.Append(@"select *, CONVERT(nvarchar(100),建立日期, 20) as 上傳日期 from 附件檔 where 資料狀態='A' and (@業者guid='' or 業者guid=@業者guid) and (@年度='' or 年度=@年度) and (@guid='' or guid=@guid) ");
        if (!string.IsNullOrEmpty(檔案類型))
            sb.Append(@"and 檔案類型=@檔案類型 ");
        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select *, CONVERT(nvarchar(100),建立日期, 20) as 上傳日期 from 附件檔 where 資料狀態='A' and (@業者guid='' or 業者guid=@業者guid) and (@年度='' or 年度=@年度) ");
        if (!string.IsNullOrEmpty(檔案類型))
            sb.Append(@"and 檔案類型=@檔案類型 ");
        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid=@guid ");
        else
            sb.Append(@"and guid='' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select *, CONVERT(nvarchar(100),建立日期, 20) as 上傳日期 from 附件檔 where 資料狀態='A' and (@業者guid='' or 業者guid=@業者guid) and (@年度='' or 年度=@年度) ");
        if (!string.IsNullOrEmpty(檔案類型))
            sb.Append(@"and 檔案類型=@檔案類型 ");
        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid=@guid ");

        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();
        oCmd.Transaction = oTran;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetFileData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 附件檔 where 資料狀態='A' and guid=@guid ");
        if (!string.IsNullOrEmpty(檔案類型))
            sb.Append(@"and 檔案類型=@檔案類型 ");
        if (!string.IsNullOrEmpty(排序))
            sb.Append(@"and 排序=@排序 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@排序", 排序);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetMaxSn()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @sortCount int
declare @maxSort int 

select @sortCount=COUNT(*) from 附件檔 
where 資料狀態='A' and 業者guid=@業者guid and 年度=@年度 and 檔案類型=@檔案類型 ");

        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid=@guid ");

        sb.Append(@"
select @maxSort=max(CONVERT(int, 排序)) from 附件檔 
where 資料狀態='A' and 業者guid=@業者guid and 年度=@年度 and 檔案類型=@檔案類型 ");

        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid=@guid ");

        sb.Append(@"
        if (@sortCount>0)
    begin
        SELECT CONVERT(int, @maxSort)+1 as Sort
    end
else
    begin
        SELECT '01' as Sort
    end "
);

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetYearList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"  
declare @yearCount int

select DISTINCT 年度 into #tmp from 附件檔
where 檔案類型=@檔案類型 and 資料狀態='A' 

select @yearCount=COUNT(*) from #tmp where 年度=@年度 

if(@yearCount > 0)
	begin
		select * from #tmp order by 年度 asc
	end
else
	begin
		insert into #tmp(年度)
		values(@年度)

		select * from #tmp order by 年度 asc
	end ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetNoYearMaxSn()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @sortCount int
declare @maxSort int 

select @sortCount=COUNT(*) from 附件檔 
where 資料狀態='A' and 業者guid=@業者guid and 檔案類型=@檔案類型 ");

        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid=@guid ");

        sb.Append(@"
select @maxSort=max(CONVERT(int, 排序)) from 附件檔 
where 資料狀態='A' and 業者guid=@業者guid and 檔案類型=@檔案類型 ");

        if (!string.IsNullOrEmpty(guid))
            sb.Append(@"and guid=@guid ");

        sb.Append(@"
        if (@sortCount>0)
    begin
        SELECT CONVERT(int, @maxSort)+1 as Sort
    end
else
    begin
        SELECT '01' as Sort
    end "
);

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);

        oda.Fill(ds);
        return ds;
    }

    public void UpdateFile_Trans(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
insert into 附件檔 (
guid,
業者guid,
年度,
檔案類型,
原檔名,
新檔名, 
附檔名, 
排序, 
檔案大小,
建立者,
修改者,
資料狀態
) values (
@guid,
@業者guid,
@年度,
@檔案類型,
@原檔名,
@新檔名, 
@附檔名, 
@排序, 
@檔案大小,
@建立者,
@修改者,
@資料狀態 
) 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);
        oCmd.Parameters.AddWithValue("@原檔名", 原檔名);
        oCmd.Parameters.AddWithValue("@新檔名", 新檔名);
        oCmd.Parameters.AddWithValue("@附檔名", 附檔名);
        oCmd.Parameters.AddWithValue("@排序", 排序);
        oCmd.Parameters.AddWithValue("@檔案大小", 檔案大小);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DelFile(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 附件檔 set
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

    public DataTable DelFile2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"update 附件檔 set
            資料狀態=@資料狀態,
            修改者=@修改者,
            修改日期=@修改日期
            where guid=@guid and 業者guid=@業者guid and 排序=@排序 and 檔案類型=@檔案類型 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@排序", 排序);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "D");

        oda.Fill(ds);
        return ds;
    }

    public DataTable DelFileFine()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"update 附件檔 set 
            資料狀態=@資料狀態,
            修改者=@修改者,
            修改日期=@修改日期
            where guid=@guid ");

        if (!string.IsNullOrEmpty(檔案類型))
            sb.Append(@" and 檔案類型=@檔案類型 ");

        if (!string.IsNullOrEmpty(排序))
            sb.Append(@" and 排序=@排序 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@排序", 排序);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", "D");

        oda.Fill(ds);
        return ds;
    }
}