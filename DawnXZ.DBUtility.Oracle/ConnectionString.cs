//==================================================================== 
//*****                    晨曦小竹常用工具集                    *****
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释   **
//====================================================================
// 文件名称：ConnectionString.cs
// 项目名称：数据库操作实用工具集
// 创建时间：2014年2月26日15时31分
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;
using System.Configuration;

namespace DawnXZ.DBUtility {
	/// <summary>
	/// 数据库连接字符串
	/// <para>Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;Integrated Security=no;</para>
	/// </summary>
	public sealed class OracleConnectionString {
		/// <summary>
		/// 数据库连接字符串
		/// <para>默认名称：OracleConnectionString</para>
		/// </summary>
		/// <param name="KeyName">键值名称</param>
		/// <returns>数据库连接字符串</returns>
		public static string ConnectionString(string KeyName) {
			string result = null;
			if (!string.IsNullOrEmpty(KeyName)) {
				result = ConfigurationManager.ConnectionStrings[KeyName].ConnectionString;
			}
			else {
				result = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;
			}
			return result;
		}
	}
}
