using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region 會員系統
/// <summary>
/// LogInfo 的摘要描述
/// </summary>
public class LogInfo
{
	/// <summary>
	/// id。
	/// </summary>
	public static string id
	{
		get
		{
			return (HttpContext.Current.Session["id"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["id"].ToString())) ? HttpContext.Current.Session["id"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["id"] = value;
		}
	}

	/// <summary>
	/// Guid。
	/// </summary>
	public static string mGuid
	{
		get
		{
			return (HttpContext.Current.Session["mGuid"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["mGuid"].ToString())) ? HttpContext.Current.Session["mGuid"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["mGuid"] = value;
		}
	}

	/// <summary>
	/// 業者guid。
	/// </summary>
	public static string companyGuid
	{
		get
		{
			return (HttpContext.Current.Session["companyGuid"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["companyGuid"].ToString())) ? HttpContext.Current.Session["companyGuid"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["companyGuid"] = value;
		}
	}


	/// <summary>
	/// 帳號。
	/// </summary>
	public static string account
	{
		get
		{
			return (HttpContext.Current.Session["account"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["account"].ToString())) ? HttpContext.Current.Session["account"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["account"] = value;
		}
	}

	/// <summary>
	/// 姓名。
	/// </summary>
	public static string name
	{
		get
		{
			return (HttpContext.Current.Session["name"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["name"].ToString())) ? HttpContext.Current.Session["name"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["name"] = value;
		}
	}

	/// <summary>
	/// 電話。
	/// </summary>
	public static string tel
	{
		get
		{
			return (HttpContext.Current.Session["tel"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["tel"].ToString())) ? HttpContext.Current.Session["tel"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["tel"] = value;
		}
	}

	/// <summary>
	/// 分機。
	/// </summary>
	public static string ext
	{
		get
		{
			return (HttpContext.Current.Session["ext"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["ext"].ToString())) ? HttpContext.Current.Session["ext"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["ext"] = value;
		}
	}

	/// <summary>
	/// 傳真。
	/// </summary>
	public static string fax
	{
		get
		{
			return (HttpContext.Current.Session["fax"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["fax"].ToString())) ? HttpContext.Current.Session["fax"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["fax"] = value;
		}
	}

	/// <summary>
	/// 手機。
	/// </summary>
	public static string phone
	{
		get
		{
			return (HttpContext.Current.Session["phone"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["phone"].ToString())) ? HttpContext.Current.Session["phone"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["phone"] = value;
		}
	}

	/// <summary>
	/// E-Mail。
	/// </summary>
	public static string email
	{
		get
		{
			return (HttpContext.Current.Session["email"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["email"].ToString())) ? HttpContext.Current.Session["email"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["email"] = value;
		}
	}

	/// <summary>
	/// 地址。
	/// </summary>
	public static string addr
	{
		get
		{
			return (HttpContext.Current.Session["addr"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["addr"].ToString())) ? HttpContext.Current.Session["addr"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["addr"] = value;
		}
	}

	/// <summary>
	/// 單位(所)。
	/// </summary>
	public static string orgcd
	{
		get
		{
			return (HttpContext.Current.Session["orgcd"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["orgcd"].ToString())) ? HttpContext.Current.Session["orgcd"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["orgcd"] = value;
		}
	}

	/// <summary>
	/// 單位名稱(所)。
	/// </summary>
	public static string orgname
	{
		get
		{
			return (HttpContext.Current.Session["orgname"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["orgname"].ToString())) ? HttpContext.Current.Session["orgname"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["orgname"] = value;
		}
	}

	/// <summary>
	/// 身份/權限。
	/// </summary>
	public static string competence
	{
		get
		{
			return (HttpContext.Current.Session["competence"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["competence"].ToString())) ? HttpContext.Current.Session["competence"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["competence"] = value;
		}
	}

	/// <summary>
	/// 業者類別。
	/// </summary>
	public static string user
	{
		get
		{
			return (HttpContext.Current.Session["user"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["user"].ToString())) ? HttpContext.Current.Session["user"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["user"] = value;
		}
	}
}
#endregion

#region 工研人事系統
public class ITRI_Common
{
	/// <summary>
	/// 工研院-工號
	/// </summary>
	public static string empno
	{
		get
		{
			return (HttpContext.Current.Session["empno"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["empno"].ToString())) ? HttpContext.Current.Session["empno"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["empno"] = value;
		}
	}

	/// <summary>
	/// 工研院-中文姓名
	/// </summary>
	public static string cname
	{
		get
		{
			return (HttpContext.Current.Session["cname"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["cname"].ToString())) ? HttpContext.Current.Session["cname"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["cname"] = value;
		}
	}

	/// <summary>
	/// 工研院-英文姓名
	/// </summary>
	public static string ename
	{
		get
		{
			return (HttpContext.Current.Session["ename"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["ename"].ToString())) ? HttpContext.Current.Session["ename"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["ename"] = value;
		}
	}

	/// <summary>
	/// 工研院-所別
	/// </summary>
	public static string orgcd
	{
		get
		{
			return (HttpContext.Current.Session["orgcd"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["orgcd"].ToString())) ? HttpContext.Current.Session["orgcd"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["orgcd"] = value;
		}
	}

	/// <summary>
	/// 工研院-部門
	/// </summary>
	public static string deptcd
	{
		get
		{
			return (HttpContext.Current.Session["deptcd"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["deptcd"].ToString())) ? HttpContext.Current.Session["deptcd"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["deptcd"] = value;
		}
	}

	/// <summary>
	/// 工研院-分機
	/// </summary>
	public static string telext
	{
		get
		{
			return (HttpContext.Current.Session["telext"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["telext"].ToString())) ? HttpContext.Current.Session["telext"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["telext"] = value;
		}
	}

	/// <summary>
	/// 工研院-E-Mail
	/// </summary>
	public static string mailadd
	{
		get
		{
			return (HttpContext.Current.Session["mailadd"] != null) ?
				 (!string.IsNullOrEmpty(HttpContext.Current.Session["mailadd"].ToString())) ? HttpContext.Current.Session["mailadd"].ToString() : "" : "";
		}
		set
		{
			HttpContext.Current.Session["mailadd"] = value;
		}
	}
}
#endregion
