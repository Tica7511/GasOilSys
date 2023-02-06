using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// OilStorageTankBWT_DB 的摘要描述
/// </summary>
public class OilStorageTankBWT_DB
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
    string 防水包覆層設計 = string.Empty;
    string 沈陷量測點數 = string.Empty;
    string 沈陷量測日期 = string.Empty;
    string 儲槽接地電阻 = string.Empty;
    string 壁板是否具包覆層 = string.Empty;
    string 壁板外部嚴重腐蝕或點蝕 = string.Empty;
    string 第一層壁板內部下方腐蝕 = string.Empty;
    string 壁板維修方式是否有符合API653 = string.Empty;
    string 設置等導電良好度 = string.Empty;    
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
    public string _防水包覆層設計 { set { 防水包覆層設計 = value; } }
    public string _沈陷量測點數 { set { 沈陷量測點數 = value; } }
    public string _沈陷量測日期 { set { 沈陷量測日期 = value; } }
    public string _儲槽接地電阻 { set { 儲槽接地電阻 = value; } }
    public string _壁板是否具包覆層 { set { 壁板是否具包覆層 = value; } }
    public string _壁板外部嚴重腐蝕或點蝕 { set { 壁板外部嚴重腐蝕或點蝕 = value; } }
    public string _第一層壁板內部下方腐蝕 { set { 第一層壁板內部下方腐蝕 = value; } }
    public string _壁板維修方式是否有符合API653 { set { 壁板維修方式是否有符合API653 = value; } }
    public string _設置等導電良好度 { set { 設置等導電良好度 = value; } }
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

        sb.Append(@"select * from 石油_儲槽基礎壁板頂板 where 資料狀態='A' and 業者guid=@業者guid ");
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

select DISTINCT 年度 into #tmp from 石油_儲槽基礎壁板頂板
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

        sb.Append(@"select * from 石油_儲槽基礎壁板頂板 where guid=@guid and 資料狀態='A' ");

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
        sb.Append(@"insert into 石油_儲槽基礎壁板頂板(  
年度,
業者guid,
轄區儲槽編號,
防水包覆層設計,
沈陷量測點數,
沈陷量測日期,
儲槽接地電阻,
壁板是否具包覆層,
壁板外部嚴重腐蝕或點蝕,
第一層壁板內部下方腐蝕,
壁板維修方式是否有符合API653,
設置等導電良好度,
修改者, 
修改日期, 
建立者, 
建立日期, 
資料狀態 ) values ( 
@年度,
@業者guid,
@轄區儲槽編號,
@防水包覆層設計,
@沈陷量測點數,
@沈陷量測日期,
@儲槽接地電阻,
@壁板是否具包覆層,
@壁板外部嚴重腐蝕或點蝕,
@第一層壁板內部下方腐蝕,
@壁板維修方式是否有符合API653,
@設置等導電良好度,
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
        oCmd.Parameters.AddWithValue("@防水包覆層設計", 防水包覆層設計);
        oCmd.Parameters.AddWithValue("@沈陷量測點數", 沈陷量測點數);
        oCmd.Parameters.AddWithValue("@沈陷量測日期", 沈陷量測日期);
        oCmd.Parameters.AddWithValue("@儲槽接地電阻", 儲槽接地電阻);
        oCmd.Parameters.AddWithValue("@壁板是否具包覆層", 壁板是否具包覆層);
        oCmd.Parameters.AddWithValue("@壁板外部嚴重腐蝕或點蝕", 壁板外部嚴重腐蝕或點蝕);
        oCmd.Parameters.AddWithValue("@第一層壁板內部下方腐蝕", 第一層壁板內部下方腐蝕);
        oCmd.Parameters.AddWithValue("@壁板維修方式是否有符合API653", 壁板維修方式是否有符合API653);
        oCmd.Parameters.AddWithValue("@設置等導電良好度", 設置等導電良好度);
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
        sb.Append(@"update 石油_儲槽基礎壁板頂板 set 
年度=@年度,
轄區儲槽編號=@轄區儲槽編號,
防水包覆層設計=@防水包覆層設計,
沈陷量測點數=@沈陷量測點數,
沈陷量測日期=@沈陷量測日期,
儲槽接地電阻=@儲槽接地電阻,
壁板是否具包覆層=@壁板是否具包覆層,
壁板外部嚴重腐蝕或點蝕=@壁板外部嚴重腐蝕或點蝕,
第一層壁板內部下方腐蝕=@第一層壁板內部下方腐蝕,
壁板維修方式是否有符合API653=@壁板維修方式是否有符合API653,
設置等導電良好度=@設置等導電良好度,
修改者=@修改者, 
修改日期=@修改日期 
where guid=@guid and 資料狀態=@資料狀態 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@年度", 年度);
        oCmd.Parameters.AddWithValue("@轄區儲槽編號", 轄區儲槽編號);
        oCmd.Parameters.AddWithValue("@防水包覆層設計", 防水包覆層設計);
        oCmd.Parameters.AddWithValue("@沈陷量測點數", 沈陷量測點數);
        oCmd.Parameters.AddWithValue("@沈陷量測日期", 沈陷量測日期);
        oCmd.Parameters.AddWithValue("@儲槽接地電阻", 儲槽接地電阻);
        oCmd.Parameters.AddWithValue("@壁板是否具包覆層", 壁板是否具包覆層);
        oCmd.Parameters.AddWithValue("@壁板外部嚴重腐蝕或點蝕", 壁板外部嚴重腐蝕或點蝕);
        oCmd.Parameters.AddWithValue("@第一層壁板內部下方腐蝕", 第一層壁板內部下方腐蝕);
        oCmd.Parameters.AddWithValue("@壁板維修方式是否有符合API653", 壁板維修方式是否有符合API653);
        oCmd.Parameters.AddWithValue("@設置等導電良好度", 設置等導電良好度);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@建立日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@資料狀態", 'A');

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DeleteData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 石油_儲槽基礎壁板頂板 set 
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