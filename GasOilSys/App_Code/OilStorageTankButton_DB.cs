using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilStorageTankButton_DB 的摘要描述
/// </summary>
public class OilStorageTankButton_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;
    string 儲槽guid = string.Empty;
    string 轄區儲槽編號 = string.Empty;
    string 執行MFL檢測 = string.Empty;
    string 防蝕塗層 = string.Empty;
    string 塗層全面重新施加日期 = string.Empty;
    string 最近一次開放塗層維修情形 = string.Empty;
    string 銲道腐蝕 = string.Empty;
    string 局部變形 = string.Empty;
    string 最近一次開放是否有維修 = string.Empty;
    string 內容物側最小剩餘厚度 = string.Empty;
    string 內容物側最大腐蝕速率 = string.Empty;
    string 土壤側最小剩餘厚度 = string.Empty;
    string 土壤側最大腐蝕速率 = string.Empty;
    string 是否有更換過底板 = string.Empty;
    string 綜合判定 = string.Empty;
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
    public string _儲槽guid { set { 儲槽guid = value; } }
    public string _轄區儲槽編號 { set { 轄區儲槽編號 = value; } }
    public string _執行MFL檢測 { set { 執行MFL檢測 = value; } }
    public string _防蝕塗層 { set { 防蝕塗層 = value; } }
    public string _塗層全面重新施加日期 { set { 塗層全面重新施加日期 = value; } }
    public string _最近一次開放塗層維修情形 { set { 最近一次開放塗層維修情形 = value; } }
    public string _銲道腐蝕 { set { 銲道腐蝕 = value; } }
    public string _局部變形 { set { 局部變形 = value; } }
    public string _最近一次開放是否有維修 { set { 最近一次開放是否有維修 = value; } }
    public string _內容物側最小剩餘厚度 { set { 內容物側最小剩餘厚度 = value; } }
    public string _內容物側最大腐蝕速率 { set { 內容物側最大腐蝕速率 = value; } }
    public string _土壤側最小剩餘厚度 { set { 土壤側最小剩餘厚度 = value; } }
    public string _土壤側最大腐蝕速率 { set { 土壤側最大腐蝕速率 = value; } }
    public string _是否有更換過底板 { set { 是否有更換過底板 = value; } }
    public string _綜合判定 { set { 綜合判定 = value; } }
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

        sb.Append(@"select * from 石油_儲槽底板 where 資料狀態='A' and 業者guid=@業者guid ");
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

    public DataTable GetYearList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"  
declare @yearCount int

select DISTINCT 年度 into #tmp from 石油_儲槽底板
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

        sb.Append(@"select * from 石油_儲槽底板 where guid=@guid and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetDataBySPNO()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_儲槽底板 where 年度=@年度 and 業者guid=@業者guid and 轄區儲槽編號=@轄區儲槽編號 and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@轄區儲槽編號", 轄區儲槽編號);

        oda.Fill(ds);
        return ds;
    }

    public void InsertData(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 石油_儲槽底板(  
年度,
業者guid,
轄區儲槽編號,
執行MFL檢測,
防蝕塗層,
塗層全面重新施加日期,
最近一次開放塗層維修情形,
銲道腐蝕,
局部變形,
最近一次開放是否有維修,
內容物側最小剩餘厚度,
內容物側最大腐蝕速率,
土壤側最小剩餘厚度,
土壤側最大腐蝕速率,
是否有更換過底板,
綜合判定,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@轄區儲槽編號,
@執行MFL檢測,
@防蝕塗層,
@塗層全面重新施加日期,
@最近一次開放塗層維修情形,
@銲道腐蝕,
@局部變形,
@最近一次開放是否有維修,
@內容物側最小剩餘厚度,
@內容物側最大腐蝕速率,
@土壤側最小剩餘厚度,
@土壤側最大腐蝕速率,
@是否有更換過底板,
@綜合判定,
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
        oCmd.Parameters.AddWithValue("@執行MFL檢測", 執行MFL檢測);
        oCmd.Parameters.AddWithValue("@防蝕塗層", 防蝕塗層);
        oCmd.Parameters.AddWithValue("@塗層全面重新施加日期", 塗層全面重新施加日期);
        oCmd.Parameters.AddWithValue("@最近一次開放塗層維修情形", 最近一次開放塗層維修情形);
        oCmd.Parameters.AddWithValue("@銲道腐蝕", 銲道腐蝕);                                
        oCmd.Parameters.AddWithValue("@局部變形", 局部變形);
        oCmd.Parameters.AddWithValue("@最近一次開放是否有維修", 最近一次開放是否有維修);
        oCmd.Parameters.AddWithValue("@內容物側最小剩餘厚度", 內容物側最小剩餘厚度);                    
        oCmd.Parameters.AddWithValue("@內容物側最大腐蝕速率", 內容物側最大腐蝕速率);
        oCmd.Parameters.AddWithValue("@土壤側最小剩餘厚度", 土壤側最小剩餘厚度);                      
        oCmd.Parameters.AddWithValue("@土壤側最大腐蝕速率", 土壤側最大腐蝕速率);
        oCmd.Parameters.AddWithValue("@是否有更換過底板", 是否有更換過底板);
        oCmd.Parameters.AddWithValue("@綜合判定", 綜合判定);
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
        sb.Append(@"update 石油_儲槽底板 set 
年度=@年度,
轄區儲槽編號=@轄區儲槽編號,
執行MFL檢測=@執行MFL檢測,
防蝕塗層=@防蝕塗層,
塗層全面重新施加日期=@塗層全面重新施加日期,
最近一次開放塗層維修情形=@最近一次開放塗層維修情形,
銲道腐蝕=@銲道腐蝕,
局部變形=@局部變形,
最近一次開放是否有維修=@最近一次開放是否有維修,
內容物側最小剩餘厚度=@內容物側最小剩餘厚度,
內容物側最大腐蝕速率=@內容物側最大腐蝕速率,
土壤側最小剩餘厚度=@土壤側最小剩餘厚度,
土壤側最大腐蝕速率=@土壤側最大腐蝕速率,
是否有更換過底板=@是否有更換過底板,
綜合判定=@綜合判定,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@轄區儲槽編號", 轄區儲槽編號);
        oCmd.Parameters.AddWithValue("@執行MFL檢測", 執行MFL檢測);
        oCmd.Parameters.AddWithValue("@防蝕塗層", 防蝕塗層);
        oCmd.Parameters.AddWithValue("@塗層全面重新施加日期", 塗層全面重新施加日期);
        oCmd.Parameters.AddWithValue("@最近一次開放塗層維修情形", 最近一次開放塗層維修情形);
        oCmd.Parameters.AddWithValue("@銲道腐蝕", 銲道腐蝕);
        oCmd.Parameters.AddWithValue("@局部變形", 局部變形);
        oCmd.Parameters.AddWithValue("@最近一次開放是否有維修", 最近一次開放是否有維修);
        oCmd.Parameters.AddWithValue("@內容物側最小剩餘厚度", 內容物側最小剩餘厚度);
        oCmd.Parameters.AddWithValue("@內容物側最大腐蝕速率", 內容物側最大腐蝕速率);
        oCmd.Parameters.AddWithValue("@土壤側最小剩餘厚度", 土壤側最小剩餘厚度);
        oCmd.Parameters.AddWithValue("@土壤側最大腐蝕速率", 土壤側最大腐蝕速率);
        oCmd.Parameters.AddWithValue("@是否有更換過底板", 是否有更換過底板);
        oCmd.Parameters.AddWithValue("@綜合判定", 綜合判定);
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
        oCmd.CommandText = @"update 石油_儲槽底板 set 
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