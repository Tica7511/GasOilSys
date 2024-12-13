using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilTubeInfo_DB 的摘要描述
/// </summary>
public class OilTubeInfo_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;
    string 長途管線識別碼 = string.Empty;
    string 轄區長途管線名稱 = string.Empty;
    string 銜接管線識別碼_上游 = string.Empty;
    string 銜接管線識別碼_下游 = string.Empty;
    string 起點 = string.Empty;
    string 迄點 = string.Empty;
    string 管徑吋 = string.Empty;
    string 厚度 = string.Empty;
    string 管材 = string.Empty;
    string 包覆材料 = string.Empty;
    string 轄管長度 = string.Empty;
    string 內容物 = string.Empty;
    string 緊急遮斷閥 = string.Empty;
    string 建置年 = string.Empty;
    string 設計壓力 = string.Empty;
    string 使用壓力 = string.Empty;
    string 使用狀態 = string.Empty;
    string 附掛橋樑數量 = string.Empty;
    string 管線穿越箱涵數量 = string.Empty;
    string 備註 = string.Empty;
    string 八大油品 = string.Empty;
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
    public string _長途管線識別碼 { set { 長途管線識別碼 = value; } }
    public string _轄區長途管線名稱 { set { 轄區長途管線名稱 = value; } }
    public string _銜接管線識別碼_上游 { set { 銜接管線識別碼_上游 = value; } }
    public string _銜接管線識別碼_下游 { set { 銜接管線識別碼_下游 = value; } }
    public string _起點 { set { 起點 = value; } }
    public string _迄點 { set { 迄點 = value; } }
    public string _管徑吋 { set { 管徑吋 = value; } }
    public string _厚度 { set { 厚度 = value; } }
    public string _管材 { set { 管材 = value; } }
    public string _包覆材料 { set { 包覆材料 = value; } }
    public string _轄管長度 { set { 轄管長度 = value; } }
    public string _內容物 { set { 內容物 = value; } }
    public string _緊急遮斷閥 { set { 緊急遮斷閥 = value; } }
    public string _建置年 { set { 建置年 = value; } }
    public string _設計壓力 { set { 設計壓力 = value; } }
    public string _使用壓力 { set { 使用壓力 = value; } }
    public string _使用狀態 { set { 使用狀態 = value; } }
    public string _附掛橋樑數量 { set { 附掛橋樑數量 = value; } }
    public string _管線穿越箱涵數量 { set { 管線穿越箱涵數量 = value; } }
    public string _備註 { set { 備註 = value; } }
    public string _八大油品 { set { 八大油品 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    #endregion

    public DataSet GetList(string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select a.*, b.活動斷層敏感區, b.土壤液化區, b.土石流潛勢區, b.淹水潛勢區, 
  八大油品_V=(select 項目名稱 from 代碼檔 where 群組代碼='030' and 項目代碼=a.八大油品) 
  into #tmp from 石油_管線基本資料 a  
  left join 石油_管線路徑環境特質表 b on a.長途管線識別碼=b.長途管線識別碼 and a.業者guid=b.業者guid and a.年度=b.年度  
  where a.業者guid=@業者guid and a.年度=@年度 and a.資料狀態='A' and b.資料狀態='A' ");

        if(!string.IsNullOrEmpty(長途管線識別碼))
            sb.Append(@" and a.長途管線識別碼=@長途管線識別碼");
        if (!string.IsNullOrEmpty(KeyWord))
            sb.Append(@" and a.長途管線識別碼 like '%'+@KeyWord+'%'");

        sb.Append(@"
select count(*) as total from #tmp

select * from (
           select ROW_NUMBER() over (order by 長途管線識別碼) itemNo,* from #tmp
)#tmp where itemNo between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);
        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@年度", "110");
        oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from  石油_管線基本資料 
where 業者guid=@業者guid and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetExportList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select a.*, b.活動斷層敏感區, b.土壤液化區, b.土石流潛勢區, b.淹水潛勢區,
  八大油品_V=(select 項目名稱 from 代碼檔 where 群組代碼='030' and 項目代碼=a.八大油品) 
  from 石油_管線基本資料 a  
  left join 石油_管線路徑環境特質表 b on a.長途管線識別碼=b.長途管線識別碼 and a.業者guid=b.業者guid and a.年度=b.年度  
  where a.業者guid=@業者guid and a.年度=@年度 and a.資料狀態='A' and b.資料狀態='A' 
  order by 長途管線識別碼 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", "110");

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

select DISTINCT 年度 into #tmp from 石油_管線基本資料
where 業者guid=@業者guid and 資料狀態='A' 

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

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetStatisticsList(string pStart, string pEnd, string cpname, string businessOrg,
        string factory, string workshop)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select a.*, b.活動斷層敏感區, b.土壤液化區, b.土石流潛勢區, b.淹水潛勢區, 
  八大油品_V=(select 項目名稱 from 代碼檔 where 群組代碼='030' and 項目代碼=a.八大油品), c.公司名稱, c.單獨公司名稱,  
  業者簡稱=(isnull(c.處,'')+isnull(c.事業部,'')+isnull(c.營業處廠,'')+isnull(c.組,'')+isnull(c.中心庫區儲運課工場,'')) 
  into #tmp from 石油_管線基本資料 a  
  left join 石油_管線路徑環境特質表 b on a.長途管線識別碼=b.長途管線識別碼 and a.業者guid=b.業者guid and a.年度=b.年度  
  left join 石油_業者基本資料 c on a.業者guid=c.guid 
  where a.資料狀態='A' and (@公司名稱='' or c.公司名稱=@公司名稱) and (@事業部='' or c.事業部=@事業部)
  and (@營業處廠='' or c.營業處廠=@營業處廠) and (@中心庫區儲運課工場='' or c.中心庫區儲運課工場=@中心庫區儲運課工場) 
  and (@長途管線識別碼='' or a.長途管線識別碼=@長途管線識別碼) and (@建置年='' or  建置年 like '%'+@建置年+'%') 
  and (@管徑吋='' or  管徑吋 like '%'+@管徑吋+'%') and (@八大油品='' or a.八大油品=@八大油品) ");

        sb.Append(@"
select count(*) as total from #tmp

select * from (
           select ROW_NUMBER() over (order by 長途管線識別碼) itemNo,* from #tmp
)#tmp where itemNo between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);
        oCmd.Parameters.AddWithValue("@公司名稱", cpname);
        oCmd.Parameters.AddWithValue("@事業部", businessOrg);
        oCmd.Parameters.AddWithValue("@營業處廠", factory);
        oCmd.Parameters.AddWithValue("@中心庫區儲運課工場", workshop);
        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@建置年", 建置年);
        oCmd.Parameters.AddWithValue("@管徑吋", 管徑吋);
        oCmd.Parameters.AddWithValue("@八大油品", 八大油品);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetStatisticsPipeSnList(string cpname, string businessOrg, string factory, string workshop)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select a.*, b.活動斷層敏感區, b.土壤液化區, b.土石流潛勢區, b.淹水潛勢區, 
  八大油品_V=(select 項目名稱 from 代碼檔 where 群組代碼='030' and 項目代碼=a.八大油品), c.公司名稱,  
  業者簡稱=(isnull(c.處,'')+isnull(c.事業部,'')+isnull(c.營業處廠,'')+isnull(c.組,'')+isnull(c.中心庫區儲運課工場,'')) 
  from 石油_管線基本資料 a  
  left join 石油_管線路徑環境特質表 b on a.長途管線識別碼=b.長途管線識別碼 and a.業者guid=b.業者guid and a.年度=b.年度  
  left join 石油_業者基本資料 c on a.業者guid=c.guid 
  where a.資料狀態='A' and (@公司名稱='' or c.公司名稱=@公司名稱) and (@事業部='' or c.事業部=@事業部)
  and (@營業處廠='' or c.營業處廠=@營業處廠) and (@中心庫區儲運課工場='' or c.中心庫區儲運課工場=@中心庫區儲運課工場)  
  order by 長途管線識別碼 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@公司名稱", cpname);
        oCmd.Parameters.AddWithValue("@事業部", businessOrg);
        oCmd.Parameters.AddWithValue("@營業處廠", factory);
        oCmd.Parameters.AddWithValue("@中心庫區儲運課工場", workshop);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select a.*, b.活動斷層敏感區, b.土壤液化區, b.土石流潛勢區, b.淹水潛勢區  
  from 石油_管線基本資料 a  
  left join 石油_管線路徑環境特質表 b on a.長途管線識別碼=b.長途管線識別碼 and a.年度=b.年度 
  where a.guid=@guid and a.資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select *  
  from 石油_管線基本資料  
  where 長途管線識別碼=@長途管線識別碼 and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetNcreeData(string cpname, string psn)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        if (cpname == "桃園儲運站")
        {
            sb.Append(@"select distinct 場站 from ncree..vw_fpg_oil
where 識別碼 like '%' + @識別碼 + '%'");
        }
        else
        {
            sb.Append(@"select distinct 場站 from ncree..vw_cpc_oil
where 識別碼 like '%' + @識別碼 + '%'");
        }

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@識別碼", psn);

        oda.Fill(ds);
        return ds;
    }

    public void InsertData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 石油_管線基本資料(  
年度,
業者guid,
長途管線識別碼,
轄區長途管線名稱,
銜接管線識別碼_上游,
銜接管線識別碼_下游,
起點,
迄點,
管徑吋,
厚度,
管材,
包覆材料,
轄管長度,
內容物,
緊急遮斷閥,
建置年,
設計壓力,
使用壓力,
使用狀態,
附掛橋樑數量,
管線穿越箱涵數量,
備註,
八大油品,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@長途管線識別碼,
@轄區長途管線名稱,
@銜接管線識別碼_上游,
@銜接管線識別碼_下游,
@起點,
@迄點,
@管徑吋,
@厚度,
@管材,
@包覆材料,
@轄管長度,
@內容物,
@緊急遮斷閥,
@建置年,
@設計壓力,
@使用壓力,
@使用狀態,
@附掛橋樑數量,
@管線穿越箱涵數量,
@備註,
@八大油品,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@年度", "110");
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@轄區長途管線名稱", 轄區長途管線名稱);
        oCmd.Parameters.AddWithValue("@銜接管線識別碼_上游", 銜接管線識別碼_上游);
        oCmd.Parameters.AddWithValue("@銜接管線識別碼_下游", 銜接管線識別碼_下游);
        oCmd.Parameters.AddWithValue("@起點", 起點);
        oCmd.Parameters.AddWithValue("@迄點", 迄點);
        oCmd.Parameters.AddWithValue("@管徑吋", 管徑吋);
        oCmd.Parameters.AddWithValue("@厚度", 厚度);
        oCmd.Parameters.AddWithValue("@管材", 管材);
        oCmd.Parameters.AddWithValue("@包覆材料", 包覆材料);
        oCmd.Parameters.AddWithValue("@轄管長度", 轄管長度);
        oCmd.Parameters.AddWithValue("@內容物", 內容物);
        oCmd.Parameters.AddWithValue("@緊急遮斷閥", 緊急遮斷閥);
        oCmd.Parameters.AddWithValue("@建置年", 建置年);
        oCmd.Parameters.AddWithValue("@設計壓力", 設計壓力);
        oCmd.Parameters.AddWithValue("@使用壓力", 使用壓力);
        oCmd.Parameters.AddWithValue("@使用狀態", 使用狀態);
        oCmd.Parameters.AddWithValue("@附掛橋樑數量", 附掛橋樑數量);
        oCmd.Parameters.AddWithValue("@管線穿越箱涵數量", 管線穿越箱涵數量);
        oCmd.Parameters.AddWithValue("@備註", 備註);
        oCmd.Parameters.AddWithValue("@八大油品", 八大油品);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 石油_管線基本資料 set  
年度=@年度,
長途管線識別碼=@長途管線識別碼,
轄區長途管線名稱=@轄區長途管線名稱,
銜接管線識別碼_上游=@銜接管線識別碼_上游,
銜接管線識別碼_下游=@銜接管線識別碼_下游,
起點=@起點,
迄點=@迄點,
管徑吋=@管徑吋,
厚度=@厚度,
管材=@管材,
包覆材料=@包覆材料,
轄管長度=@轄管長度,
內容物=@內容物,
緊急遮斷閥=@緊急遮斷閥,
建置年=@建置年,
設計壓力=@設計壓力,
使用壓力=@使用壓力,
使用狀態=@使用狀態,
附掛橋樑數量=@附掛橋樑數量,
管線穿越箱涵數量=@管線穿越箱涵數量,
備註=@備註,
八大油品=@八大油品,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", "110");
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@轄區長途管線名稱", 轄區長途管線名稱);
        oCmd.Parameters.AddWithValue("@銜接管線識別碼_上游", 銜接管線識別碼_上游);
        oCmd.Parameters.AddWithValue("@銜接管線識別碼_下游", 銜接管線識別碼_下游);
        oCmd.Parameters.AddWithValue("@起點", 起點);
        oCmd.Parameters.AddWithValue("@迄點", 迄點);
        oCmd.Parameters.AddWithValue("@管徑吋", 管徑吋);
        oCmd.Parameters.AddWithValue("@厚度", 厚度);
        oCmd.Parameters.AddWithValue("@管材", 管材);
        oCmd.Parameters.AddWithValue("@包覆材料", 包覆材料);
        oCmd.Parameters.AddWithValue("@轄管長度", 轄管長度);
        oCmd.Parameters.AddWithValue("@內容物", 內容物);
        oCmd.Parameters.AddWithValue("@緊急遮斷閥", 緊急遮斷閥);
        oCmd.Parameters.AddWithValue("@建置年", 建置年);
        oCmd.Parameters.AddWithValue("@設計壓力", 設計壓力);
        oCmd.Parameters.AddWithValue("@使用壓力", 使用壓力);
        oCmd.Parameters.AddWithValue("@使用狀態", 使用狀態);
        oCmd.Parameters.AddWithValue("@附掛橋樑數量", 附掛橋樑數量);
        oCmd.Parameters.AddWithValue("@管線穿越箱涵數量", 管線穿越箱涵數量);
        oCmd.Parameters.AddWithValue("@備註", 備註);
        oCmd.Parameters.AddWithValue("@八大油品", 八大油品);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DeleteData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"
declare @sno nvarchar(50)
declare @year nvarchar(4)
declare @cpid nvarchar(50)
declare @nCount int

select @cpid=業者guid, @sno=長途管線識別碼, @year=年度 from 石油_管線基本資料 where guid=@guid and 資料狀態='A' 
select @nCount=count(*) from 石油_管線路徑環境特質表 where 業者guid=@cpid and 長途管線識別碼=@sno and 年度=@year and 資料狀態='A'

if(@nCount>0)
    begin
        update 石油_管線路徑環境特質表 set 
        修改日期=@修改日期, 
        修改者=@修改者, 
        資料狀態='D' 
        where 業者guid=@cpid and 長途管線識別碼=@sno and 年度=@year
    end

update 石油_管線基本資料 set 
修改日期=@修改日期, 
修改者=@修改者, 
資料狀態='D' 
where guid=@guid ";

        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public void DeleteDataAll()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"
update 石油_儲槽底板 set 
修改日期=@修改日期, 
修改者=@修改者, 
資料狀態='D' 
where 年度=@年度 and 業者guid=@業者guid and 資料狀態='A' 

update 石油_管線路徑環境特質表 set 
修改日期=@修改日期, 
修改者=@修改者, 
資料狀態='D' 
where 年度=@年度 and 業者guid=@業者guid and 資料狀態='A' 

";

        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }
}