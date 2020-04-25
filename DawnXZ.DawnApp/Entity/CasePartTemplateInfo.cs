//==================================================================== 
//**   解决方案或项目总名称
//====================================================================
//**   Copyright © DawnXZ.com 2015 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：CasePartTemplateInfo.cs
// 项目名称：请更改为实际项目名称
// 创建时间：2015-05-05 23:08:28
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;
using MongoDB.Bson;

namespace DawnXZ.DawnApp.Entity {
	/// <summary>
	/// 案件部分的模板
	/// </summary>
	[Serializable]
	public class CasePartTemplateInfo {
		/// <summary>
		/// 案件部分的模板
		/// </summary>
		public CasePartTemplateInfo() {
			// -->
		}

		#region 成员属性

		/// <summary>
		/// 系统编号
		/// </summary>
		public ObjectId _id { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }
		/// <summary>
		/// 末次更新时间
		/// </summary>
		public DateTime LastUpdateTime { get; set; }
		/// <summary>
		/// 部分的编号
		/// </summary>
		public string PartId { get; set; }
		/// <summary>
		/// 父级编号
		/// </summary>
		public string ParentId { get; set; }
		/// <summary>
		/// AJLX编号
		/// </summary>
		public int AJLX { get; set; }
		/// <summary>
		/// 部分的XML数据
		/// </summary>
		public string PartXML { get; set; }
		/// <summary>
		/// 部分的名称
		/// </summary>
		public string PartName { get; set; }

		#endregion

	}
}
