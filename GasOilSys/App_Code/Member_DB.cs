using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Member_DB 的摘要描述
/// </summary>
public class Member_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 業者guid = string.Empty;
    string 使用者帳號 = string.Empty;
    string 使用者密碼 = string.Empty;
    string 姓名 = string.Empty;
    string mail = string.Empty;
    string 電話 = string.Empty;
    string 單位 = string.Empty;
    string 單位名稱 = string.Empty;
    string 角色 = string.Empty;
    string 委員類別 = string.Empty;
    string 帳號類別 = string.Empty;
    string 職稱 = string.Empty;
    string 學歷 = string.Empty;
    string 相關經歷 = string.Empty;
    string 專業領域 = string.Empty;
    string 網站類別 = string.Empty;
    string 密碼需變更 = string.Empty;
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
    public string _使用者帳號 { set { 使用者帳號 = value; } }
    public string _使用者密碼 { set { 使用者密碼 = value; } }
    public string _姓名 { set { 姓名 = value; } }
    public string _mail { set { mail = value; } }
    public string _電話 { set { 電話 = value; } }
    public string _單位 { set { 單位 = value; } }
    public string _單位名稱 { set { 單位名稱 = value; } }
    public string _角色 { set { 角色 = value; } }
    public string _委員類別 { set { 委員類別 = value; } }
    public string _帳號類別 { set { 帳號類別 = value; } }
    public string _職稱 { set { 職稱 = value; } }
    public string _學歷 { set { 學歷 = value; } }
    public string _相關經歷 { set { 相關經歷 = value; } }
    public string _專業領域 { set { 專業領域 = value; } }
    public string _網站類別 { set { 網站類別 = value; } }
    public string _密碼需變更 { set { 密碼需變更 = value; } }
    public string _建立者 { set { 建立者 = value; } }
    public DateTime _建立日期 { set { 建立日期 = value; } }
    public string _修改者 { set { 修改者 = value; } }
    public DateTime _修改日期 { set { 修改日期 = value; } }
    public string _資料狀態 { set { 資料狀態 = value; } }
    #endregion

    public DataTable CheckMember()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"SELECT * from 會員檔 where 資料狀態='A' ");
        if (使用者帳號 != "")
            sb.Append(@"and 使用者帳號=@使用者帳號 ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@使用者帳號", 使用者帳號);
        oda.Fill(ds);
        return ds;
    }

    public DataSet GetList(string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * into #tmp from 會員檔 
  where 資料狀態='A' ");

        if (!string.IsNullOrEmpty(帳號類別))
            sb.Append(@"and 帳號類別=@帳號類別 ");
        if (!string.IsNullOrEmpty(網站類別))
            sb.Append(@"and 網站類別=@網站類別 ");
        if (!string.IsNullOrEmpty(業者guid))
            sb.Append(@"and 業者guid=@業者guid ");
        if (!string.IsNullOrEmpty(KeyWord))
            sb.Append(@"and (姓名 like '%' + @KeyWord + '%')  ");

        sb.Append(@"
select count(*) as total from #tmp

select * from (
           select ROW_NUMBER() over (order by 帳號類別, 網站類別, 姓名) itemNo,* from #tmp
)#tmp where itemNo between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);
        oCmd.Parameters.AddWithValue("@帳號類別", 帳號類別);
        oCmd.Parameters.AddWithValue("@網站類別", 網站類別);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from 會員檔 where 資料狀態='A' and guid=@guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oda.Fill(ds);
        return ds;
    }

    public void UpdatePwd()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 會員檔 set
使用者密碼=@使用者密碼,
修改日期=@修改日期,
修改者=@修改者
where guid=@guid
";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@使用者密碼", 使用者密碼);
        oCmd.Parameters.AddWithValue("@修改日期", DateTime.Now);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable GetCommittee()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select ROW_NUMBER() OVER(ORDER BY 姓名) AS 場次,* from 會員檔 where 帳號類別='01' and 資料狀態='A' ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oda.Fill(ds);
        return ds;
    }

    public void addMember(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"insert into 會員檔 (
guid,
業者guid,
使用者帳號,
使用者密碼,
姓名,
mail,
電話,
單位名稱,
角色,
帳號類別,
網站類別,
建立者,
修改者,
資料狀態 
) values (
@guid,
@業者guid,
@使用者帳號,
@使用者密碼,
@姓名,
@mail,
@電話,
@單位名稱,
@角色,
@帳號類別,
@網站類別,
@建立者,
@修改者,
@資料狀態 
) ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@使用者帳號", 使用者帳號);
        oCmd.Parameters.AddWithValue("@使用者密碼", 使用者密碼);
        oCmd.Parameters.AddWithValue("@姓名", 姓名);
        oCmd.Parameters.AddWithValue("@mail", mail);
        oCmd.Parameters.AddWithValue("@電話", 電話);
        oCmd.Parameters.AddWithValue("@單位名稱", 單位名稱);
        oCmd.Parameters.AddWithValue("@角色", 角色);
        oCmd.Parameters.AddWithValue("@帳號類別", 帳號類別);
        oCmd.Parameters.AddWithValue("@網站類別", 網站類別);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void UpdateMember(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"update 會員檔 set 
業者guid=@業者guid,
使用者帳號=@使用者帳號,
使用者密碼=@使用者密碼,
姓名=@姓名,
mail=@mail,
電話=@電話,
單位名稱=@單位名稱,
角色=@角色,
帳號類別=@帳號類別,
網站類別=@網站類別,
修改者=@修改者 
where guid=@guid and 資料狀態=@資料狀態 
");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@業者guid", 業者guid);
        oCmd.Parameters.AddWithValue("@使用者帳號", 使用者帳號);
        oCmd.Parameters.AddWithValue("@使用者密碼", 使用者密碼);
        oCmd.Parameters.AddWithValue("@姓名", 姓名);
        oCmd.Parameters.AddWithValue("@mail", mail);
        oCmd.Parameters.AddWithValue("@電話", 電話);
        oCmd.Parameters.AddWithValue("@單位名稱", 單位名稱);
        oCmd.Parameters.AddWithValue("@角色", 角色);
        oCmd.Parameters.AddWithValue("@帳號類別", 帳號類別);
        oCmd.Parameters.AddWithValue("@網站類別", 網站類別);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }

    public void DeleteData()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        oCmd.CommandText = @"update 會員檔 set 
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