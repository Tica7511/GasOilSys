﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// GasTubeCheckPlanAndResult_DB 的摘要描述
/// </summary>
public class GasTubeCheckPlanAndResult_DB
{
	string KeyWord = string.Empty;
	public string _KeyWord { set { KeyWord = value; } }
	#region private
	string id = string.Empty;
	string guid = string.Empty;
	string 業者guid = string.Empty;
	string 年度 = string.Empty;
	string 用戶管線定期檢查 = string.Empty;
	string 委外檢查 = string.Empty;
	string 用戶名稱 = string.Empty;
	string 檢查期限是否符合 = string.Empty;
	string 檢查結果是否符合 = string.Empty;
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
	public string _用戶管線定期檢查 { set { 用戶管線定期檢查 = value; } }
	public string _委外檢查 { set { 委外檢查 = value; } }
	public string _用戶名稱 { set { 用戶名稱 = value; } }
	public string _檢查期限是否符合 { set { 檢查期限是否符合 = value; } }
	public string _檢查結果是否符合 { set { 檢查結果是否符合 = value; } }
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

		sb.Append(@"select ROW_NUMBER() OVER(ORDER BY id ASC) AS itemNo,* from 天然氣_用戶管線定期檢查計畫及檢查結果 where 資料狀態='A' and 業者guid=@業者guid ");

		oCmd.CommandText = sb.ToString();
		oCmd.CommandType = CommandType.Text;
		SqlDataAdapter oda = new SqlDataAdapter(oCmd);
		DataTable ds = new DataTable();

		oCmd.Parameters.AddWithValue("@業者guid", 業者guid);

		oda.Fill(ds);
		return ds;
	}
}