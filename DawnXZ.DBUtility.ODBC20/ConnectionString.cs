//==================================================================== 
//*****                    晨曦小竹常用工具集                    *****
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释   **
//====================================================================
// 文件名称：ConnectionString.cs
// 项目名称：数据库操作实用工具集
// 创建时间：2014年2月26日14时40分
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;

namespace DawnXZ.DBUtility
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public sealed class ODBCConnectionString
    {
        /// <summary>
        /// ODBC 连接字符串
		/// <para>调用时请对此方法特别调用</para>
        /// </summary>
        /// <param name="readonlyflg">是否只读</param>
        /// <param name="OdbcName">ODBC 名称</param>
        /// <param name="OdbcUid">ODBC 用户名</param>
        /// <param name="OdbcPwd">ODBC 密码</param>
        /// <returns>连接字符串</returns>
		public static string ConnectionString(bool readonlyflg, string OdbcName, string OdbcUid, string OdbcPwd)
        {
            string result = null;
            result = string.Format(@"maxbuffersize=10240;pagetimeout=60;Dsn={0};", OdbcName);
            if (readonlyflg)
            {
                result += "ReadOnly=1;";
            }
            if (!string.IsNullOrEmpty(OdbcUid))
            {
                result += string.Format("uid={0};", OdbcUid);
            }
            if (!string.IsNullOrEmpty(OdbcPwd))
            {
                result += string.Format("pwd={0};", OdbcPwd);
            }
            return result;
        }
    }
}
