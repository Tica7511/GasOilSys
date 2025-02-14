using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilControl_DB 的摘要描述
/// </summary>
public class OilControl_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;
    string 長途管線識別碼 = string.Empty;
    string 依據文件名稱 = string.Empty;
    string 文件編號 = string.Empty;
    string 文件日期 = string.Empty;
    string 壓力計校正頻率 = string.Empty;
    string 壓力計校正_最近一次校正時間 = string.Empty;
    string 流量計校正頻率 = string.Empty;
    string 流量計校正_最近一次校正時間 = string.Empty;
    string 監控中心定期調整之週期 = string.Empty;
    string 合格操作人員總數 = string.Empty;
    string 輪班制度 = string.Empty;
    string 每班人數 = string.Empty;
    string 每班時數 = string.Empty;
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;

    //壓力計及流量計資料
    string 自有端是否有設置壓力計 = string.Empty;
    string 壓力計校正週期 = string.Empty;
    string 壓力計最近一次校正日期 = string.Empty;
    string 壓力計最近一次校正結果 = string.Empty;
    string 自有端是否有設置流量計 = string.Empty;
    string 流量計型式 = string.Empty;
    string 流量計最小精度 = string.Empty;
    string 流量計校正週期 = string.Empty;
    string 流量計最近一次校正日期 = string.Empty;
    string 流量計最近一次校正結果 = string.Empty;

    //儲槽泵送接收資料
    string 轄區儲槽編號 = string.Empty;
    string 洩漏監控系統 = string.Empty;
    string 控制室名稱 = string.Empty;
    string 液位監測方式 = string.Empty;
    string 液位監測靈敏度 = string.Empty;
    string 高液位警報設定基準 = string.Empty;
    string 前一年度高液位警報發生頻率 = string.Empty;
    string 液位異常下降警報設定基準 = string.Empty;
    string 前一年度異常下降警報發生頻率 = string.Empty;

    //管線輸送接收資料
    string 管線編號 = string.Empty;
    string 接收泵送路過 = string.Empty;
    string 歷史操作壓力變動範圍 = string.Empty;
    string 起泵至穩態之時間 = string.Empty;
    string 自有端是否有設置壓力 = string.Empty;
    string 自有端是否有設置流量 = string.Empty;
    string 操作壓力值 = string.Empty;
    string 壓力警報設定值 = string.Empty;
    string 流量警報設定值 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _guid { set { guid = value; } }
    public string _業者guid { set { 業者guid = value; } }
    public string _年度 { set { 年度 = value; } }
    public string _長途管線識別碼 { set { 長途管線識別碼 = value; } }
    public string _依據文件名稱 { set { 依據文件名稱 = value; } }
    public string _文件編號 { set { 文件編號 = value; } }
    public string _文件日期 { set { 文件日期 = value; } }
    public string _壓力計校正頻率 { set { 壓力計校正頻率 = value; } }
    public string _壓力計校正_最近一次校正時間 { set { 壓力計校正_最近一次校正時間 = value; } }
    public string _流量計校正頻率 { set { 流量計校正頻率 = value; } }
    public string _流量計校正_最近一次校正時間 { set { 流量計校正_最近一次校正時間 = value; } }
    public string _監控中心定期調整之週期 { set { 監控中心定期調整之週期 = value; } }
    public string _合格操作人員總數 { set { 合格操作人員總數 = value; } }
    public string _輪班制度 { set { 輪班制度 = value; } }
    public string _每班人數 { set { 每班人數 = value; } }
    public string _每班時數 { set { 每班時數 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }

    //壓力計及流量計資料
    public string _自有端是否有設置壓力計 { set { 自有端是否有設置壓力計 = value; } }
    public string _壓力計校正週期 { set { 壓力計校正週期 = value; } }
    public string _壓力計最近一次校正日期 { set { 壓力計最近一次校正日期 = value; } }
    public string _壓力計最近一次校正結果 { set { 壓力計最近一次校正結果 = value; } }
    public string _自有端是否有設置流量計 { set { 自有端是否有設置流量計 = value; } }
    public string _流量計型式 { set { 流量計型式 = value; } }
    public string _流量計最小精度 { set { 流量計最小精度 = value; } }
    public string _流量計校正週期 { set { 流量計校正週期 = value; } }
    public string _流量計最近一次校正日期 { set { 流量計最近一次校正日期 = value; } }
    public string _流量計最近一次校正結果 { set { 流量計最近一次校正結果 = value; } }

    //儲槽泵送接收資料
    public string _轄區儲槽編號 { set { 轄區儲槽編號 = value; } }
    public string _洩漏監控系統 { set { 洩漏監控系統 = value; } }
    public string _液位監測方式 { set { 液位監測方式 = value; } }
    public string _液位監測靈敏度 { set { 液位監測靈敏度 = value; } }
    public string _高液位警報設定基準 { set { 高液位警報設定基準 = value; } }
    public string _前一年度高液位警報發生頻率 { set { 前一年度高液位警報發生頻率 = value; } }
    public string _液位異常下降警報設定基準 { set { 液位異常下降警報設定基準 = value; } }
    public string _前一年度異常下降警報發生頻率 { set { 前一年度異常下降警報發生頻率 = value; } }

    //管線輸送接收資料
    public string _管線編號 { set { 管線編號 = value; } }
    public string _接收泵送路過 { set { 接收泵送路過 = value; } }
    public string _歷史操作壓力變動範圍 { set { 歷史操作壓力變動範圍 = value; } }
    public string _起泵至穩態之時間 { set { 起泵至穩態之時間 = value; } }
    public string _控制室名稱 { set { 控制室名稱 = value; } }
    public string _自有端是否有設置壓力 { set { 自有端是否有設置壓力 = value; } }
    public string _自有端是否有設置流量 { set { 自有端是否有設置流量 = value; } }
    public string _操作壓力值 { set { 操作壓力值 = value; } }
    public string _壓力警報設定值 { set { 壓力警報設定值 = value; } }
    public string _流量警報設定值 { set { 流量警報設定值 = value; } }
    #endregion

    public DataTable GetList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_控制室 where 資料狀態='A' and 業者guid=@業者guid ");
        if (!string.IsNullOrEmpty(年度))
            sb.Append(@" and 年度=@年度");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetList2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_控制室_儲槽泵送接收資料 where 資料狀態='A' and 業者guid=@業者guid ");
        if (!string.IsNullOrEmpty(年度))
            sb.Append(@" and 年度=@年度 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetList3()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_控制室_管線輸送接收資料 where 資料狀態='A' and 業者guid=@業者guid ");
        if (!string.IsNullOrEmpty(年度))
            sb.Append(@" and 年度=@年度 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetList4()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_控制室_壓力計及流量計資料 where 資料狀態='A' and 業者guid=@業者guid ");
        if (!string.IsNullOrEmpty(年度))
            sb.Append(@" and 年度=@年度 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);

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

select DISTINCT 年度 into #tmp from 石油_控制室
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

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_控制室_儲槽泵送接收資料 where guid=@guid and 資料狀態='A' ");

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

        sb.Append(@"select * from 石油_控制室_管線輸送接收資料 where guid=@guid and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetDataStress()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_控制室_壓力計及流量計資料 where guid=@guid and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public void InsertData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 石油_控制室(  
年度,
業者guid,
長途管線識別碼,
依據文件名稱,
文件編號,
文件日期,
壓力計校正頻率,
壓力計校正_最近一次校正時間,
流量計校正頻率,
流量計校正_最近一次校正時間,
監控中心定期調整之週期,
合格操作人員總數,
輪班制度,
每班人數,
每班時數,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@長途管線識別碼,
@依據文件名稱,
@文件編號,
@文件日期,
@壓力計校正頻率,
@壓力計校正_最近一次校正時間,
@流量計校正頻率,
@流量計校正_最近一次校正時間,
@監控中心定期調整之週期,
@合格操作人員總數,
@輪班制度,
@每班人數,
@每班時數,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@依據文件名稱", 依據文件名稱);
        oCmd.Parameters.AddWithValue("@文件編號", 文件編號);
        oCmd.Parameters.AddWithValue("@文件日期", 文件日期);
        oCmd.Parameters.AddWithValue("@壓力計校正頻率", 壓力計校正頻率);
        oCmd.Parameters.AddWithValue("@壓力計校正_最近一次校正時間", 壓力計校正_最近一次校正時間);
        oCmd.Parameters.AddWithValue("@流量計校正頻率", 流量計校正頻率);
        oCmd.Parameters.AddWithValue("@流量計校正_最近一次校正時間", 流量計校正_最近一次校正時間);
        oCmd.Parameters.AddWithValue("@監控中心定期調整之週期", 監控中心定期調整之週期);
        oCmd.Parameters.AddWithValue("@合格操作人員總數", 合格操作人員總數);
        oCmd.Parameters.AddWithValue("@輪班制度", 輪班制度);
        oCmd.Parameters.AddWithValue("@每班人數", 每班人數);
        oCmd.Parameters.AddWithValue("@每班時數", 每班時數);
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
        sb.Append(@"update 石油_控制室 set  
年度=@年度,
長途管線識別碼=@長途管線識別碼,
依據文件名稱=@依據文件名稱,
文件編號=@文件編號,
文件日期=@文件日期,
壓力計校正頻率=@壓力計校正頻率,
壓力計校正_最近一次校正時間=@壓力計校正_最近一次校正時間,
流量計校正頻率=@流量計校正頻率,
流量計校正_最近一次校正時間=@流量計校正_最近一次校正時間,
監控中心定期調整之週期=@監控中心定期調整之週期,
合格操作人員總數=@合格操作人員總數,
輪班制度=@輪班制度,
每班人數=@每班人數,
每班時數=@每班時數,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@長途管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@依據文件名稱", 依據文件名稱);
        oCmd.Parameters.AddWithValue("@文件編號", 文件編號);
        oCmd.Parameters.AddWithValue("@文件日期", 文件日期);
        oCmd.Parameters.AddWithValue("@壓力計校正頻率", 壓力計校正頻率);
        oCmd.Parameters.AddWithValue("@壓力計校正_最近一次校正時間", 壓力計校正_最近一次校正時間);
        oCmd.Parameters.AddWithValue("@流量計校正頻率", 流量計校正頻率);
        oCmd.Parameters.AddWithValue("@流量計校正_最近一次校正時間", 流量計校正_最近一次校正時間);
        oCmd.Parameters.AddWithValue("@監控中心定期調整之週期", 監控中心定期調整之週期);
        oCmd.Parameters.AddWithValue("@合格操作人員總數", 合格操作人員總數);
        oCmd.Parameters.AddWithValue("@輪班制度", 輪班制度);
        oCmd.Parameters.AddWithValue("@每班人數", 每班人數);
        oCmd.Parameters.AddWithValue("@每班時數", 每班時數);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void InsertData2(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 石油_控制室_儲槽泵送接收資料(  
年度,
業者guid,
轄區儲槽編號,
控制室名稱,
液位監測方式,
液位監測靈敏度,
高液位警報設定基準,
前一年度高液位警報發生頻率,
液位異常下降警報設定基準,
前一年度異常下降警報發生頻率,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@轄區儲槽編號,
@控制室名稱,
@液位監測方式,
@液位監測靈敏度,
@高液位警報設定基準,
@前一年度高液位警報發生頻率,
@液位異常下降警報設定基準,
@前一年度異常下降警報發生頻率,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@轄區儲槽編號", 轄區儲槽編號);
        oCmd.Parameters.AddWithValue("@控制室名稱", 控制室名稱);
        oCmd.Parameters.AddWithValue("@液位監測方式", 液位監測方式);
        oCmd.Parameters.AddWithValue("@液位監測靈敏度", 液位監測靈敏度);
        oCmd.Parameters.AddWithValue("@高液位警報設定基準", 高液位警報設定基準);
        oCmd.Parameters.AddWithValue("@前一年度高液位警報發生頻率", 前一年度高液位警報發生頻率);
        oCmd.Parameters.AddWithValue("@液位異常下降警報設定基準", 液位異常下降警報設定基準);
        oCmd.Parameters.AddWithValue("@前一年度異常下降警報發生頻率", 前一年度異常下降警報發生頻率);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData2(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 石油_控制室_儲槽泵送接收資料 set  
年度=@年度, 
轄區儲槽編號=@轄區儲槽編號,
控制室名稱=@控制室名稱,
液位監測方式=@液位監測方式,
液位監測靈敏度=@液位監測靈敏度,
高液位警報設定基準=@高液位警報設定基準,
前一年度高液位警報發生頻率=@前一年度高液位警報發生頻率,
液位異常下降警報設定基準=@液位異常下降警報設定基準,
前一年度異常下降警報發生頻率=@前一年度異常下降警報發生頻率, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@轄區儲槽編號", 轄區儲槽編號);
        oCmd.Parameters.AddWithValue("@控制室名稱", 控制室名稱);
        oCmd.Parameters.AddWithValue("@液位監測方式", 液位監測方式);
        oCmd.Parameters.AddWithValue("@液位監測靈敏度", 液位監測靈敏度);
        oCmd.Parameters.AddWithValue("@高液位警報設定基準", 高液位警報設定基準);
        oCmd.Parameters.AddWithValue("@前一年度高液位警報發生頻率", 前一年度高液位警報發生頻率);
        oCmd.Parameters.AddWithValue("@液位異常下降警報設定基準", 液位異常下降警報設定基準);
        oCmd.Parameters.AddWithValue("@前一年度異常下降警報發生頻率", 前一年度異常下降警報發生頻率);
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
        oCmd.CommandText = @"update 石油_控制室_儲槽泵送接收資料 set 
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

    public void InsertData3(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 石油_控制室_管線輸送接收資料(  
年度,
業者guid,
管線編號,
接收泵送路過,
歷史操作壓力變動範圍,
起泵至穩態之時間,
控制室名稱,
洩漏監控系統,
自有端是否有設置壓力,
自有端是否有設置流量,
操作壓力值,
壓力警報設定值,
流量警報設定值,
前一年度異常下降警報發生頻率,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@管線編號,
@接收泵送路過,
@歷史操作壓力變動範圍,
@起泵至穩態之時間,
@控制室名稱,
@洩漏監控系統,
@自有端是否有設置壓力,
@自有端是否有設置流量,
@操作壓力值,
@壓力警報設定值,
@流量警報設定值,
@前一年度異常下降警報發生頻率,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@管線編號", 管線編號);
        oCmd.Parameters.AddWithValue("@接收泵送路過", 接收泵送路過);
        oCmd.Parameters.AddWithValue("@歷史操作壓力變動範圍", 歷史操作壓力變動範圍);
        oCmd.Parameters.AddWithValue("@起泵至穩態之時間", 起泵至穩態之時間);
        oCmd.Parameters.AddWithValue("@控制室名稱", 控制室名稱);
        oCmd.Parameters.AddWithValue("@洩漏監控系統", 洩漏監控系統);
        oCmd.Parameters.AddWithValue("@自有端是否有設置壓力", 自有端是否有設置壓力);
        oCmd.Parameters.AddWithValue("@自有端是否有設置流量", 自有端是否有設置流量);
        oCmd.Parameters.AddWithValue("@操作壓力值", 操作壓力值);
        oCmd.Parameters.AddWithValue("@壓力警報設定值", 壓力警報設定值);
        oCmd.Parameters.AddWithValue("@流量警報設定值", 流量警報設定值);
        oCmd.Parameters.AddWithValue("@前一年度異常下降警報發生頻率", 前一年度異常下降警報發生頻率);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateData3(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 石油_控制室_管線輸送接收資料 set  
年度=@年度, 
管線編號=@管線編號,
接收泵送路過=@接收泵送路過,
歷史操作壓力變動範圍=@歷史操作壓力變動範圍,
起泵至穩態之時間=@起泵至穩態之時間,
控制室名稱=@控制室名稱,
洩漏監控系統=@洩漏監控系統,
自有端是否有設置壓力=@自有端是否有設置壓力,
自有端是否有設置流量=@自有端是否有設置流量,
操作壓力值=@操作壓力值,
壓力警報設定值=@壓力警報設定值,
流量警報設定值=@流量警報設定值,
前一年度異常下降警報發生頻率=@前一年度異常下降警報發生頻率,
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@管線編號", 管線編號);
        oCmd.Parameters.AddWithValue("@接收泵送路過", 接收泵送路過);
        oCmd.Parameters.AddWithValue("@歷史操作壓力變動範圍", 歷史操作壓力變動範圍);
        oCmd.Parameters.AddWithValue("@起泵至穩態之時間", 起泵至穩態之時間);
        oCmd.Parameters.AddWithValue("@控制室名稱", 控制室名稱);
        oCmd.Parameters.AddWithValue("@自有端是否有設置壓力", 自有端是否有設置壓力);
        oCmd.Parameters.AddWithValue("@自有端是否有設置流量", 自有端是否有設置流量);
        oCmd.Parameters.AddWithValue("@洩漏監控系統", 洩漏監控系統);
        oCmd.Parameters.AddWithValue("@操作壓力值", 操作壓力值);
        oCmd.Parameters.AddWithValue("@壓力警報設定值", 壓力警報設定值);
        oCmd.Parameters.AddWithValue("@流量警報設定值", 流量警報設定值);
        oCmd.Parameters.AddWithValue("@前一年度異常下降警報發生頻率", 前一年度異常下降警報發生頻率);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DeleteData2()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 石油_控制室_管線輸送接收資料 set 
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

    public void InsertDataStress(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 石油_控制室_壓力計及流量計資料(  
年度,
業者guid,
管線識別碼,
洩漏監控系統,
自有端是否有設置壓力計,
壓力計校正週期,
壓力計最近一次校正日期,
壓力計最近一次校正結果,
自有端是否有設置流量計,
流量計型式,
流量計最小精度,
流量計校正週期,
流量計最近一次校正日期,
流量計最近一次校正結果,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@管線識別碼,
@洩漏監控系統,
@自有端是否有設置壓力計,
@壓力計校正週期,
@壓力計最近一次校正日期,
@壓力計最近一次校正結果,
@自有端是否有設置流量計,
@流量計型式,
@流量計最小精度,
@流量計校正週期,
@流量計最近一次校正日期,
@流量計最近一次校正結果,
@修改者, 
@修改日期, 
@建立者, 
@建立日期, 
@資料狀態  
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@洩漏監控系統", 洩漏監控系統);
        oCmd.Parameters.AddWithValue("@自有端是否有設置壓力計", 自有端是否有設置壓力計);
        oCmd.Parameters.AddWithValue("@壓力計校正週期", 壓力計校正週期);
        oCmd.Parameters.AddWithValue("@壓力計最近一次校正日期", 壓力計最近一次校正日期);
        oCmd.Parameters.AddWithValue("@壓力計最近一次校正結果", 壓力計最近一次校正結果);
        oCmd.Parameters.AddWithValue("@自有端是否有設置流量計", 自有端是否有設置流量計);
        oCmd.Parameters.AddWithValue("@流量計型式", 流量計型式);
        oCmd.Parameters.AddWithValue("@流量計最小精度", 流量計最小精度);
        oCmd.Parameters.AddWithValue("@流量計校正週期", 流量計校正週期);
        oCmd.Parameters.AddWithValue("@流量計最近一次校正日期", 流量計最近一次校正日期);
        oCmd.Parameters.AddWithValue("@流量計最近一次校正結果", 流量計最近一次校正結果);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateDataStress(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 石油_控制室_壓力計及流量計資料 set  
年度=@年度,
管線識別碼=@管線識別碼,
洩漏監控系統=@洩漏監控系統,
自有端是否有設置壓力計=@自有端是否有設置壓力計,
壓力計校正週期=@壓力計校正週期,
壓力計最近一次校正日期=@壓力計最近一次校正日期,
壓力計最近一次校正結果=@壓力計最近一次校正結果,
自有端是否有設置流量計=@自有端是否有設置流量計,
流量計型式=@流量計型式,
流量計最小精度=@流量計最小精度,
流量計校正週期=@流量計校正週期,
流量計最近一次校正日期=@流量計最近一次校正日期,
流量計最近一次校正結果=@流量計最近一次校正結果,
修改者=@修改者, 
修改日期=@修改日期
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@管線識別碼", 長途管線識別碼);
        oCmd.Parameters.AddWithValue("@洩漏監控系統", 洩漏監控系統);
        oCmd.Parameters.AddWithValue("@自有端是否有設置壓力計", 自有端是否有設置壓力計);
        oCmd.Parameters.AddWithValue("@壓力計校正週期", 壓力計校正週期);
        oCmd.Parameters.AddWithValue("@壓力計最近一次校正日期", 壓力計最近一次校正日期);
        oCmd.Parameters.AddWithValue("@壓力計最近一次校正結果", 壓力計最近一次校正結果);
        oCmd.Parameters.AddWithValue("@自有端是否有設置流量計", 自有端是否有設置流量計);
        oCmd.Parameters.AddWithValue("@流量計型式", 流量計型式);
        oCmd.Parameters.AddWithValue("@流量計最小精度", 流量計最小精度);
        oCmd.Parameters.AddWithValue("@流量計校正週期", 流量計校正週期);
        oCmd.Parameters.AddWithValue("@流量計最近一次校正日期", 流量計最近一次校正日期);
        oCmd.Parameters.AddWithValue("@流量計最近一次校正結果", 流量計最近一次校正結果);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DeleteDataStress()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 石油_控制室_壓力計及流量計資料 set 
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
}