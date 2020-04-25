//==================================================================== 
//**   晨曦小竹常用工具集
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：EMailMessage.cs
// 项目名称：原子能式的高深学问方法实用工具集
// 创建时间：2016-04-14 11:20:18
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
	/// 电子邮件[E-Mail]正文信息操作帮助类
	/// </summary>
	public class EMailMessage {
		/// <summary>
		/// 构造函数
		/// </summary>
		public EMailMessage() {
			Charset = "GB2312";
			MaxRecipientNum = 30;
			Attachments = new EMailAttachments();
			Priority = EMailPriority.Normal;
			Recipients = new ArrayList();
			BodyFormat = EMailFormat.HTML;
		}

		#region 成员属性

		/// <summary>
		/// 设定语言代码的字符编码格式
		/// <para>默认GB2312</para>
		/// <para>不需要时设置为""</para>
		/// </summary>
		public string Charset { get; set; }
		/// <summary>
		/// 收件人最大数量
		/// <para>默认30个</para>
		/// </summary>
		public int MaxRecipientNum { get; set; }
		/// <summary>
		/// 发件人地址
		/// </summary>
		public string From { get; set; }
		/// <summary>
		/// 发件人姓名
		/// </summary>
		public string FromName { get; set; }
		/// <summary>
		/// 邮件内容正文
		/// </summary>
		public string Body { get; set; }
		/// <summary>
		/// 邮件主题
		/// </summary>
		public string Subject { get; set; }
		/// <summary>
		/// 邮件附件
		/// </summary>
		public EMailAttachments Attachments { get; set; }
		/// <summary>
		/// 邮件优先级
		/// </summary>
		public EMailPriority Priority { get; set; }
		/// <summary>
		/// 收件人列表
		/// </summary>
		public IList Recipients { get; private set; }
		/// <summary>
		/// 邮件格式
		/// </summary>
		public EMailFormat BodyFormat { get; set; }

		#endregion

		#region 成员方法

		/// <summary>
		/// 增加一个收件人地址
		/// </summary>
		/// <param name="recipient">收件人的Email地址</param>
		public void AddRecipients(string recipient) {
			if (Recipients.Count < MaxRecipientNum) {
				Recipients.Add(recipient);
			}
		}
		/// <summary>
		/// 增加多个收件人地址
		/// </summary>
		/// <param name="recipient">收件人的Email地址集合</param>
		public void AddRecipients(params string[] recipient) {
			if (recipient == null) {
				throw (new ArgumentException("收件人不能为空."));
			}
			else {
				for (int i = 0; i < recipient.Length; i++) {
					AddRecipients(recipient[i]);
				}
			}
		}

		#endregion

	}
}
