//==================================================================== 
//**   晨曦小竹常用工具集
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：EMailClient.cs
// 项目名称：原子能式的高深学问方法实用工具集
// 创建时间：2016-04-14 18:16:01
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;

namespace DawnXZ.NuclearUtility {
	/// <summary>
	/// 电子邮件[E-Mail]发送客户端实现类
	/// <para>调用示例：</para>
	/// <para>EMailAttachments mailAttas=new EMailAttachments();</para>
	/// <para>mailAttas.Add(@"D:\邮件附件.7z");</para>
	/// <para>---</para>
	/// <para>EMailMessage mailMsg = new EMailMessage();</para>
	/// <para>mailMsg.Attachments=mailAttas;</para>
	/// <para>mailMsg.Body="这里是邮件正文信息";</para>
	/// <para>mailMsg.AddRecipients("收件人邮箱@163.com");</para>
	/// <para>mailMsg.From="发件人邮箱@163.com";</para>
	/// <para>mailMsg.FromName="发件人名称";</para>
	/// <para>mailMsg.Subject="邮件主题";</para>
	/// <para>---</para>
	/// <para>EMailClient mailClient = new EMailClient();</para>
	/// <para>mailClient.SMTPServer = "smtp.163.com";</para>
	/// <para>mailClient.SMTPPort = 25;</para>
	/// <para>mailClient.SMTPUserName = "发件人邮箱@163.com";</para>
	/// <para>mailClient.SMTPPassword = "发件人邮箱密码";</para>
	/// <para>mailClient.Send(mailMsg);</para>
	/// </summary>
	public class EMailClient {
		/// <summary>
		/// 电子邮件[E-Mail]发送客户端实现类
		/// <para>使用时必须设置：</para>
		/// <para>1、邮件服务器地址</para>
		/// <para>2、邮件服务器端口号</para>
		/// <para>3、发件人用户名</para>
		/// <para>4、发件人密码</para>
		/// </summary>
		public EMailClient() {
			FEMailHelper = new EMailHelper();
			ErrorMessage = string.Empty;
		}
		/// <summary>
		/// 电子邮件[E-Mail]发送客户端实现类
		/// <para>使用时必须设置：</para>
		/// <para>1、邮件服务器地址</para>
		/// <para>2、邮件服务器端口号</para>
		/// <para>3、发件人用户名</para>
		/// <para>4、发件人密码</para>
		/// </summary>
		/// <param name="smtpServer">邮件服务器地址</param>
		/// <param name="userName">发件人用户名</param>
		/// <param name="password">发件人密码</param>
		public EMailClient(string smtpServer, string userName, string password)
			: this(smtpServer, 25, userName, password) { ;}
		/// <summary>
		/// 电子邮件[E-Mail]发送客户端实现类
		/// <para>使用时必须设置：</para>
		/// <para>1、邮件服务器地址</para>
		/// <para>2、邮件服务器端口号</para>
		/// <para>3、发件人用户名</para>
		/// <para>4、发件人密码</para>
		/// </summary>
		/// <param name="smtpServer">邮件服务器地址</param>
		/// <param name="smtpPort">邮件服务器端口号</param>
		/// <param name="userName">发件人用户名</param>
		/// <param name="password">发件人密码</param>
		public EMailClient(string smtpServer, int smtpPort, string userName, string password) {			
			FEMailHelper = new EMailHelper();
			ErrorMessage = string.Empty;
			SMTPServer = smtpServer;
			SMTPPort = smtpPort;
			SMTPUserName = userName;
			SMTPPassword = password;
		}

		#region 成员变量

		/// <summary>
		/// 电子邮件[E-Mail]服务交互操作对象
		/// </summary>
		private EMailHelper FEMailHelper;

		#endregion

		#region 成员属性

		/// <summary>
		/// 错误消息反馈信息
		/// </summary>
		public string ErrorMessage { get; private set; }
		/// <summary>
		/// 邮件服务器地址
		/// </summary>
		public string SMTPServer { get; set; }
		/// <summary>
		/// 邮件服务器端口号
		/// </summary>
		public int SMTPPort { get; set; }
		/// <summary>
		/// 发件人用户名
		/// </summary>
		public string SMTPUserName { get; set; }
		/// <summary>
		/// 发件人密码
		/// </summary>
		public string SMTPPassword { get; set; }

		#endregion

		#region 成员方法

		/// <summary>
		/// 电子邮件[E-Mail]发送事件
		/// </summary>
		/// <param name="mailMessage">电子邮件[E-Mail]正文信息</param>
		/// <returns>邮件发送结果</returns>
		public bool Send(EMailMessage mailMessage) {
			if (string.IsNullOrEmpty(SMTPServer) || SMTPPort <= 0 || string.IsNullOrEmpty(SMTPUserName) || string.IsNullOrEmpty(SMTPPassword)) {
				ErrorMessage = "邮件服务器地址、端口号、用户名、密码有空值或填写不正确。";
				return false;
			}
			bool result = FEMailHelper.SendEmail(SMTPServer, SMTPPort, SMTPUserName, SMTPPassword, mailMessage);
			ErrorMessage = result ? string.Empty : FEMailHelper.ErrorMessage;
			return result;
		}

		#endregion

	}
}
