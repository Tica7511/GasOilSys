using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilCathodicProtection_DB 的摘要描述
/// </summary>
public class OilCathodicProtection_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 年度 = string.Empty;    
    string 電位量測週期 = string.Empty;
    string 整流站量測週期 = string.Empty;
    string 電位量測單位 = string.Empty;    
    string 建立者 = string.Empty;
    DateTime 建立日期;
    string 修改者 = string.Empty;
    DateTime 修改日期;
    string 資料狀態 = string.Empty;

    //明細表
    string 儲槽guid = string.Empty;
    string 轄區儲槽編號 = string.Empty;
    string 設置 = string.Empty;
    string 整流站名稱 = string.Empty;
    string 合格標準 = string.Empty;
    string 整流站狀態 = string.Empty;
    string 系統狀態 = string.Empty;
    string 設置長效型參考電極種類 = string.Empty;
    string 測試點數量 = string.Empty;
    string 陽極地床種類 = string.Empty;
    string 備註 = string.Empty;
    #endregion
    #region public
    public string _id { set { id = value; } }
    public string _guid { set { guid = value; } }
    public string _業者guid { set { 業者guid = value; } }
    public string _年度 { set { 年度 = value; } }
    public string _電位量測週期 { set { 電位量測週期 = value; } }
    public string _整流站量測週期 { set { 整流站量測週期 = value; } }
    public string _電位量測單位 { set { 電位量測單位 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }

    //明細表
    public string _儲槽guid { set { 儲槽guid = value; } }
    public string _轄區儲槽編號 { set { 轄區儲槽編號 = value; } }
    public string _設置 { set { 設置 = value; } }
    public string _整流站名稱 { set { 整流站名稱 = value; } }
    public string _合格標準 { set { 合格標準 = value; } }
    public string _整流站狀態 { set { 整流站狀態 = value; } }
    public string _系統狀態 { set { 系統狀態 = value; } }
    public string _設置長效型參考電極種類 { set { 設置長效型參考電極種類 = value; } }
    public string _測試點數量 { set { 測試點數量 = value; } }
    public string _陽極地床種類 { set { 陽極地床種類 = value; } }
    public string _備註 { set { 備註 = value; } }
    #endregion

    public DataTable GetList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 石油_陰極防蝕系統 where 資料狀態='A' and 業者guid=@業者guid ");
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

        sb.Append(@"select * from 石油_陰極防蝕系統_明細表 where 資料狀態='A' and 業者guid=@業者guid ");
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

select DISTINCT 年度 into #tmp from 石油_陰極防蝕系統
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

        sb.Append(@"select * from 石油_陰極防蝕系統_明細表 where guid=@guid and 資料狀態='A' ");

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
        sb.Append(@"insert into 石油_陰極防蝕系統(  
年度,
業者guid,
電位量測週期,
整流站量測週期,
電位量測單位,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@電位量測週期,
@整流站量測週期,
@電位量測單位,
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
        oCmd.Parameters.AddWithValue("@電位量測週期", 電位量測週期);
        oCmd.Parameters.AddWithValue("@整流站量測週期", 整流站量測週期);
        oCmd.Parameters.AddWithValue("@電位量測單位", 電位量測單位);
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
        sb.Append(@"update 石油_陰極防蝕系統 set  
年度=@年度,
電位量測週期=@電位量測週期,
整流站量測週期=@整流站量測週期,
電位量測單位=@電位量測單位,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@電位量測週期", 電位量測週期);
        oCmd.Parameters.AddWithValue("@整流站量測週期", 整流站量測週期);
        oCmd.Parameters.AddWithValue("@電位量測單位", 電位量測單位);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void InsertData2(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 石油_陰極防蝕系統_明細表(  
年度,
業者guid,
轄區儲槽編號,
設置,
整流站名稱,
合格標準,
整流站狀態,
系統狀態,
設置長效型參考電極種類,
測試點數量,
陽極地床種類,
備註,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@轄區儲槽編號,
@設置,
@整流站名稱,
@合格標準,
@整流站狀態,
@系統狀態,
@設置長效型參考電極種類,
@測試點數量,
@陽極地床種類,
@備註,
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
        oCmd.Parameters.AddWithValue("@設置", 設置);
        oCmd.Parameters.AddWithValue("@整流站名稱", 整流站名稱);
        oCmd.Parameters.AddWithValue("@合格標準", 合格標準);
        oCmd.Parameters.AddWithValue("@整流站狀態", 整流站狀態);
        oCmd.Parameters.AddWithValue("@系統狀態", 系統狀態);
        oCmd.Parameters.AddWithValue("@設置長效型參考電極種類", 設置長效型參考電極種類);
        oCmd.Parameters.AddWithValue("@測試點數量", 測試點數量);
        oCmd.Parameters.AddWithValue("@陽極地床種類", 陽極地床種類);
        oCmd.Parameters.AddWithValue("@備註", 備註);
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
        sb.Append(@"update 石油_陰極防蝕系統_明細表 set  
年度=@年度,
轄區儲槽編號=@轄區儲槽編號,
設置=@設置,
整流站名稱=@整流站名稱,
合格標準=@合格標準,
整流站狀態=@整流站狀態,
系統狀態=@系統狀態,
設置長效型參考電極種類=@設置長效型參考電極種類,
測試點數量=@測試點數量,
陽極地床種類=@陽極地床種類,
備註=@備註,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@轄區儲槽編號", 轄區儲槽編號);
        oCmd.Parameters.AddWithValue("@設置", 設置);
        oCmd.Parameters.AddWithValue("@整流站名稱", 整流站名稱);
        oCmd.Parameters.AddWithValue("@合格標準", 合格標準);
        oCmd.Parameters.AddWithValue("@整流站狀態", 整流站狀態);
        oCmd.Parameters.AddWithValue("@系統狀態", 系統狀態);
        oCmd.Parameters.AddWithValue("@設置長效型參考電極種類", 設置長效型參考電極種類);
        oCmd.Parameters.AddWithValue("@測試點數量", 測試點數量);
        oCmd.Parameters.AddWithValue("@陽極地床種類", 陽極地床種類);
        oCmd.Parameters.AddWithValue("@備註", 備註);
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
        oCmd.CommandText = @"update 石油_陰極防蝕系統_明細表 set 
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