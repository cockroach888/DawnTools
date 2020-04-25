//==================================================================== 
//**   解决方案或项目总名称
//====================================================================
//**   Copyright © DawnXZ.com 2016 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：EMailAttachments.cs
// 项目名称：原子能式的高深学问方法实用工具集
// 创建时间：2016-04-14 11:20:46
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;
using System.Collections;

namespace DawnXZ.NuclearUtility {
	/// <summary>
	/// 电子邮件[E-Mail]附件操作帮助类
	/// </summary>
	public class EMailAttachments {
		/// <summary>
		/// 电子邮件[E-Mail]附件操作帮助类
		/// </summary>
		public EMailAttachments() {
			FAttachments = new ArrayList();
		}

		#region 成员变量

		/// <summary>
		/// 邮件附件存储对象
		/// </summary>
		private IList FAttachments;
		/// <summary>
		/// 邮件附件最大数量
		/// </summary>
        private const int MaxAttachmentNum = 10;

		#endregion

		#region 成员属性

		/// <summary>
		/// 附件索引器
		/// </summary>
		/// <param name="index">索引位</param>
		/// <returns>附件信息</returns>
		public string this[int index] {
			get {
				if (index < 0 || index >= FAttachments.Count) return null;
				return FAttachments[index] as string;
			}
		}
		/// <summary>
		/// 获取附件个数
		/// </summary>
		public int Count {
			get {
				return FAttachments.Count;
			}
		}

		#endregion

		#region 成员方法

		/// <summary>
		/// 添加邮件附件
		/// </summary>
		/// <param name="FilePath">附件的绝对路径</param>
		public void Add(params string[] filePath) {
			if (filePath == null) {
				throw (new ArgumentNullException("非法的附件"));
			}
			else {
				for (int i = 0; i < filePath.Length; i++) {
					Add(filePath[i]);
				}
			}
		}
		/// <summary>
		/// 添加一个附件,当指定的附件不存在时，忽略该附件，不产生异常。
		/// </summary>
		/// <param name="filePath">附件的绝对路径</param>
		public void Add(string filePath) {
			if (System.IO.File.Exists(filePath)) {
				if (FAttachments.Count < MaxAttachmentNum) {
					FAttachments.Add(filePath);
				}
			}
		}
		/// <summary>
		/// 清除所有附件
		/// </summary>
		public void Clear() {
			FAttachments.Clear();
		}

		#endregion

	}
}
