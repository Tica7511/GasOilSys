using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// TrackChangesFileTable 的摘要描述
/// </summary>
public class TrackChangesFileTable
{
    string KeyWord = string.Empty;
    public string _KeyWord { set { KeyWord = value; } }
    #region private
    string id = string.Empty;
    string guid = string.Empty;
    string 父層guid = string.Empty;
    string 年度 = string.Empty;
    string 狀態 = string.Empty;
    string 檔案類型 = string.Empty;
    string 檔案名稱 = string.Empty;
    byte[] 內容;
    string 文字 = string.Empty;
    string 路徑 = string.Empty;
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
    public string _年度 { set { 年度 = value; } }
    public string _狀態 { set { 狀態 = value; } }
    public string _檔案類型 { set { 檔案類型 = value; } }
    public string _檔案名稱 { set { 檔案名稱 = value; } }
    public byte[] _內容 { set { 內容 = value; } }
    public string _文字 { set { 文字 = value; } }
    public string _路徑 { set { 路徑 = value; } }
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

        sb.Append(@"select a.*, CONVERT(nvarchar(100),a.建立日期, 20) as 上傳日期 ,b.姓名,
  用印文件名稱=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='01' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A'),
  用途=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='02' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A'),
  件數=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='03' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A'),
  印信名稱=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='04' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A'),
  主管=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='05' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A'),
  部門主管=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='06' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A'),
  申請人=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='07' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A'),
  年份=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='08' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A'),
  月份=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='09' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A'),
  日期=(select 文字 from 追蹤修訂附件檔 where 檔案名稱='10' and guid=a.業者guid and 父層guid=a.guid and 資料狀態='A') 
  from 附件檔 a 
  left join 會員檔 b on a.建立者=b.guid 
  where a.guid=@guid and a.資料狀態='A' and a.排序<>'1'
  order by a.建立日期 asc ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();
;
        oCmd.Parameters.AddWithValue("@guid", guid);

        oda.Fill(ds);
        return ds;
    }

    public void UpdateFile_Trans(SqlConnection oConn, SqlTransaction oTran)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
insert into 追蹤修訂附件檔 (
guid,
父層guid,
年度,
狀態,
檔案類型,
檔案名稱,
內容, 
文字, 
路徑, 
建立者,
修改者,
資料狀態
) values (
@guid,
@父層guid,
@年度,
@狀態,
@檔案類型,
@檔案名稱,
@內容,
@文字, 
@路徑, 
@建立者,
@修改者,
@資料狀態 
) 
 ");
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = sb.ToString();

        oCmd.Parameters.AddWithValue("@guid", guid);
        oCmd.Parameters.AddWithValue("@父層guid", 父層guid);
        oCmd.Parameters.AddWithValue("@年度", "114");
        oCmd.Parameters.AddWithValue("@狀態", 狀態);
        oCmd.Parameters.AddWithValue("@檔案類型", 檔案類型);
        oCmd.Parameters.AddWithValue("@檔案名稱", 檔案名稱);
        oCmd.Parameters.AddWithValue("@內容", 內容);
        oCmd.Parameters.AddWithValue("@文字", 文字);
        oCmd.Parameters.AddWithValue("@路徑", 路徑);
        oCmd.Parameters.AddWithValue("@建立者", 建立者);
        oCmd.Parameters.AddWithValue("@修改者", 修改者);
        oCmd.Parameters.AddWithValue("@資料狀態", "A");

        oCmd.Transaction = oTran;
        oCmd.ExecuteNonQuery();
    }
}