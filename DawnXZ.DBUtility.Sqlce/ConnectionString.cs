//==================================================================== 
//*****                    晨曦小竹常用工具集                    *****
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释   **
//====================================================================
// 文件名称：ConnectionString.cs
// 项目名称：数据库操作实用工具集
// 创建时间：2014年2月26日11时43分
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
    public sealed class SqlceConnectionString
    {
        /// <summary>
        /// Sql Server SDF 连接字符串
        /// </summary>
        /// <param name="sdfPath">SDF 文件路径</param>
        /// <returns>连接字符串</returns>
		public static string ConnectionString(string sdfPath)
        {
            return string.Format(@"Data Source={0}", sdfPath);
        }
        /// <summary>
        /// Sql Server SDF 连接字符串
        /// </summary>
        /// <param name="sdfPath">SDF 文件路径</param>
        /// <param name="sdfPwd">SDF 密码</param>
        /// <returns>连接字符串</returns>
		public static string ConnectionString(string sdfPath, string sdfPwd)
        {
            return string.Format(@"Data Source={0};pwd={1};", sdfPath, sdfPwd);
        }
    }
}
