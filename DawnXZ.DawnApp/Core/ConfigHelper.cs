//==================================================================== 
//**   解决方案或项目总名称
//====================================================================
//**   Copyright © DawnXZ.com 2016 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：ConfigHelper.cs
// 项目名称：请更改为实际项目名称
// 创建时间：2016-01-18 21:42:48
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
using DawnXZ.DawnUtility;

namespace DawnXZ.DawnApp.Core {
	/// <summary>
	/// 配置参数帮助类
	/// </summary>
	public static class ConfigHelper {
		/// <summary>
		/// 应用名称
		/// </summary>
		public static string AppName {
			get {
				var result = ConfigurationManager.AppSettings["AppName"];
				var tmp = RSAHelper.GetRASKey();
				return result;
			}
		}
		/// <summary>
		/// 版本号
		/// </summary>
		public static string AppVersion {
			get {
				var result = ConfigurationManager.AppSettings["AppVersion"];

				return result;
			}
		}
	}
}
