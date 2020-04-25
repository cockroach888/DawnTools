//==================================================================== 
//**   解决方案或项目总名称
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：Conn.cs
// 项目名称：请更改为实际项目名称
// 创建时间：2014-12-09 23:41:28
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;
using DawnXZ.DawnUtility;
using DawnXZ.DBUtility;

namespace DawnXZ.DawnApp.Core {
	/// <summary>
	/// 数据库连接字符串
	/// </summary>
	internal class ConnHelper {
		/// <summary>
		/// 数据库连接字符串
		/// </summary>
		public static readonly string MongoConn = CryptoHelper.Decrypt(MongoConnectionString.ConnectionString("MongoConnectionString"));
		/// <summary>
		/// 数据库名称
		/// </summary>
		public static readonly string MongoName = CryptoHelper.Decrypt(MongoConnectionString.DatabaseName("MongoDatabaseName"));
	}
}
