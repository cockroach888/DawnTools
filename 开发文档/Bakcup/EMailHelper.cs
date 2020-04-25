//==================================================================== 
//**   晨曦小竹常用工具集
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：EMailHelper.cs
// 项目名称：原子能式的高深学问方法实用工具集
// 创建时间：2016-04-13 23:59:35
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
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace DawnXZ.NuclearUtility {
	/// <summary>
	/// 邮件格式枚举定义类
	/// </summary>
	public enum EMailFormat {
		/// <summary>
		/// 文本格式
		/// </summary>
		Text,
		/// <summary>
		/// HTML格式
		/// </summary>
		HTML
	}
	/// <summary>
	/// 邮件优先级枚举定义类
	/// </summary>
	public enum EMailPriority {
		/// <summary>
		/// 最低
		/// </summary>
		Low = 1,
		/// <summary>
		/// 正常
		/// </summary>
		Normal = 3,
		/// <summary>
		/// 最高
		/// </summary>
		High = 5
	}
	/// <summary>
	/// 电子邮件[E-Mail]服务交互操作帮助类
	/// </summary>
	public class EMailHelper {
		/// <summary>
		/// 电子邮件[E-Mail]收发操作帮助类
		/// </summary>
		public EMailHelper() {
			FLogs = new StringBuilder();
			FErrMsg = new StringBuilder();
			FErrorCode = new Hashtable();
			FRightCode = new Hashtable();
			SMTPCodeCreate();
		}
		/// <summary>
		/// 析构函数
		/// </summary>
		~EMailHelper() {
			if (null != FNetworkStream) FNetworkStream.Close();
			if (null != FTcpClient) FTcpClient.Close();
		}

		#region 成员变量

		/// <summary>
		/// 回车换行符
		/// </summary>
		private const string CRLF = @"\r\n";
		/// <summary>
		/// TcpClient对象，用于连接服务器
		/// </summary> 
		private TcpClient FTcpClient;
		/// <summary>
		/// NetworkStream对象
		/// </summary> 
		private NetworkStream FNetworkStream;
		/// <summary>
		/// 服务器交互记录
		/// </summary>
		private StringBuilder FLogs;
		/// <summary>
		/// 错误消息反馈信息
		/// </summary>
		private StringBuilder FErrMsg;
		/// <summary>
		/// SMTP错误代码哈希表对象
		/// </summary>
		private Hashtable FErrorCode;
		/// <summary>
		/// SMTP正确代码哈希表对象
		/// </summary>
		private Hashtable FRightCode;

		#endregion

		#region 成员属性

		/// <summary>
		/// 错误消息反馈信息
		/// </summary>
		public string ErrorMessage {
			get {
				return FErrMsg.ToString();
			}
			set {
				FErrMsg.Length = 0; //重置
				FErrMsg.Append(value);
			}
		}

		#endregion

		#region SMTP回应代码表

		/// <summary>
		/// SMTP回应代码哈希表
		/// </summary>
		private void SMTPCodeCreate() {
			//SMTP错误代码表
			FErrorCode.Add("421", "服务未就绪，关闭传输信道");
			FErrorCode.Add("432", "需要一个密码转换");
			FErrorCode.Add("450", "要求的邮件操作未完成，邮箱不可用（例如，邮箱忙）");
			FErrorCode.Add("451", "放弃要求的操作；处理过程中出错");
			FErrorCode.Add("452", "系统存储不足，要求的操作未执行");
			FErrorCode.Add("454", "临时认证失败");
			FErrorCode.Add("500", "邮箱地址错误");
			FErrorCode.Add("501", "参数格式错误");
			FErrorCode.Add("502", "命令不可实现");
			FErrorCode.Add("503", "服务器需要SMTP验证");
			FErrorCode.Add("504", "命令参数不可实现");
			FErrorCode.Add("530", "需要认证");
			FErrorCode.Add("534", "认证机制过于简单");
			FErrorCode.Add("538", "当前请求的认证机制需要加密");
			FErrorCode.Add("550", "要求的邮件操作未完成，邮箱不可用（例如，邮箱未找到，或不可访问）");
			FErrorCode.Add("551", "用户非本地，请尝试<forward-path>");
			FErrorCode.Add("552", "过量的存储分配，要求的操作未执行");
			FErrorCode.Add("553", "邮箱名不可用，要求的操作未执行（例如邮箱格式错误）");
			FErrorCode.Add("554", "传输失败");
			//SMTP正确代码表
			FRightCode.Add("220", "服务就绪");
			FRightCode.Add("221", "服务关闭传输信道");
			FRightCode.Add("235", "验证成功");
			FRightCode.Add("250", "要求的邮件操作完成");
			FRightCode.Add("251", "非本地用户，将转发向<forward-path>");
			FRightCode.Add("334", "服务器响应验证Base64字符串");
			FRightCode.Add("354", "开始邮件输入，以<CRLF>.<CRLF>结束");
		}

		#endregion

		#region 成员常规方法

		/// <summary>
		/// 将字符串编码为Base64字符串
		/// </summary>
		/// <param name="strContent">要编码的字符串</param>
		private string Base64Encode(string strContent) {
			byte[] barray = Encoding.Default.GetBytes(strContent);
			return Convert.ToBase64String(barray);
		}
		/// <summary>
		/// 将Base64字符串解码为普通字符串
		/// </summary>
		/// <param name="strContent">要解码的字符串</param>
		private string Base64Decode(string strContent) {
			byte[] barray = Convert.FromBase64String(strContent);
			return Encoding.Default.GetString(barray);
		}
		/// <summary>
		/// 得到上传附件的文件流
		/// </summary>
		/// <param name="fileFullPath">附件的绝对路径</param>
		private string GetStream(string fileFullPath) {
			if (!File.Exists(fileFullPath)) return null;
			byte[] _buffer;
			using (FileStream _fr = new FileStream(fileFullPath, FileMode.Open)) {
				_buffer = new byte[_fr.Length];
				_fr.Read(_buffer, 0, _buffer.Length);
				_fr.Close();
			}
			return (Convert.ToBase64String(_buffer));
		}

		#endregion

		#region 成员邮箱命令事件

		/// <summary>
		/// 发送SMTP命令
		/// </summary>
		/// <param name="cmdInfo">SMTP命令</param>
		/// <returns>命令执行结果</returns>
		private bool SendCommand(string cmdInfo) {
			if (string.IsNullOrEmpty(cmdInfo)) return true;
			byte[] _writeBuffer;
			FLogs.Append(cmdInfo);
			_writeBuffer = Encoding.Default.GetBytes(cmdInfo);
			try {
				FNetworkStream.Write(_writeBuffer, 0, _writeBuffer.Length);
			}
			catch {
				FErrMsg.Length = 0; //重置
				FErrMsg.Append("网络连接错误");
				return false;
			}
			return true;
		}
		/// <summary>
		/// 接收SMTP服务器回应
		/// </summary>
		private string RecvResponse() {
			int StreamSize;
			string Returnvalue = String.Empty;
			byte[] ReadBuffer = new byte[1024];
			try {
				StreamSize = FNetworkStream.Read(ReadBuffer, 0, ReadBuffer.Length);
			}
			catch {
				FErrMsg.Length = 0; //重置
				FErrMsg.Append("网络连接错误");
				return "false";
			}
			if (StreamSize == 0) return Returnvalue;
			Returnvalue = Encoding.Default.GetString(ReadBuffer).Substring(0, StreamSize);
			FLogs.Append(Returnvalue);
			FLogs.Append(CRLF);
			return Returnvalue;
		}

		#endregion

		#region 成员邮箱交互事件

		/// <summary>
		/// 与服务器交互，发送一条命令并接收回应。
		/// </summary>
		/// <param name="cmdInfo">一个要发送的命令</param>
		/// <param name="errInfo">如果错误，要反馈的信息</param>
		/// <returns>与服务器交互结果</returns>
		private bool DialogEx(string cmdInfo, string errInfo) {
			if (string.IsNullOrEmpty(cmdInfo)) return true;
			bool _cmdResult = SendCommand(cmdInfo);
			if (!_cmdResult) return false;
			string _recvResult = RecvResponse();
			if (_recvResult == "false") return false;
			//检查返回的代码，根据[RFC 821]返回代码为3位数字代码如220
			string _code = _recvResult.Substring(0, 3);
			if (FRightCode[_code] != null) return true;
			if (FErrorCode[_code] != null) {
				FErrMsg.Append(_code);
				FErrMsg.Append(FErrorCode[_code].ToString());
				FErrMsg.Append(CRLF);
			}
			else {
				FErrMsg.Append(_recvResult);
			}
			FErrMsg.Append(errInfo);
			return false;
		}
		/// <summary>
		/// 与服务器交互，发送一组命令并接收回应。
		/// </summary>
		/// <param name="cmdInfos">一组要发送的命令</param>
		/// <param name="errInfo">如果错误，要反馈的信息</param>
		/// <returns>与服务器交互结果</returns>
		private bool DialogEx(string[] cmdInfos, string errInfo) {
			bool _exResult = true;
			for (int i = 0, iLen = cmdInfos.Length; i < iLen; i++) {
				bool _result = DialogEx(cmdInfos[i], null);
				if (!_result) {
					FErrMsg.Append(CRLF);
					FErrMsg.Append(errInfo);
					_exResult = false;
					break;
				}
			}
			return _exResult;
		}
		/// <summary>
		/// 连接SMTP服务器
		/// </summary>
		/// <param name="smtpServer">SMTP服务器地址</param>
		/// <param name="smtpPort">SMTP服务器端口号</param>
		/// <returns>SMTP服务器连接结果</returns>
		private bool Connect(string smtpServer, int smtpPort) {
			try {
				FTcpClient = new TcpClient(smtpServer, smtpPort);
			}
			catch (Exception ex) {
				FErrMsg.Length = 0; //重置
				FErrMsg.Append(ex.ToString());
				return false;
			}
			FNetworkStream = FTcpClient.GetStream();
			string _recvResult = RecvResponse();
			string _code = _recvResult.Substring(0, 3);
			if (null == FRightCode[_code]) {
				FErrMsg.Length = 0; //重置
				FErrMsg.Append("网络连接失败");
				return false;
			}
			return true;
		}
		/// <summary>
		/// 获取邮件优先级
		/// </summary>
		/// <param name="mailPriority">邮件优先级</param>
		/// <returns>邮件优先级</returns>
		private string GetPriorityString(EMailPriority mailPriority) {
			string result = "Normal";
			switch (mailPriority) {
				case EMailPriority.Low:
					result = "Low";
					break;
				case EMailPriority.High:
					result = "High";
					break;
				case EMailPriority.Normal:
				default:
					result = "Normal";
					break;
			}
			return result;
		}

		#endregion

		#region 成员邮件发送事件

		/// <summary>
		/// 发送电子邮件
		/// </summary>
		/// <param name="smtpServer">发信SMTP服务器</param>
		/// <param name="smtpPort">端口，默认为25</param>
		/// <param name="smtpCheck">是否进行SMTP验证</param>
		/// <param name="userName">发信人邮箱地址</param>
		/// <param name="password">发信人邮箱密码</param>
		/// <param name="mailMessage">邮件内容</param>
		/// <returns>电子邮件发送结果</returns>
		private bool SendEmail(string smtpServer, int smtpPort, bool smtpCheck, string userName, string password, EMailMessage mailMessage) {
			bool _connResult = Connect(smtpServer, smtpPort);
			if (!_connResult) return false;
			string _priority = GetPriorityString(mailMessage.Priority);
			bool _htmlFlag = (mailMessage.BodyFormat == EMailFormat.HTML);
			string[] _sendBuffer;
			StringBuilder _sendString = new StringBuilder();
			//进行SMTP验证
			if (smtpCheck) {
				_sendBuffer = new String[4];
				_sendBuffer[0] = string.Format("EHLO {0}{1}", smtpServer, CRLF);
				_sendBuffer[1] = string.Format("AUTH LOGIN{0}", CRLF);
				_sendBuffer[2] = string.Format("{0}{1}", Base64Encode(userName), CRLF);
				_sendBuffer[3] = string.Format("{0}{1}", Base64Encode(password), CRLF);
				bool _tmpResult = DialogEx(_sendBuffer, "SMTP服务器验证失败，请核对用户名和密码。");
				if (!_tmpResult) return false;
			}
			else {
				_sendString.Length = 0; //重置
				_sendString.Append("HELO ");
				_sendString.Append(smtpServer);
				_sendString.Append(CRLF);
				bool _tmpResult = DialogEx(_sendString.ToString(), null);
				if (!_tmpResult) return false;
			}
			//发件人地址
			_sendString.Length = 0; //重置
			_sendString.Append("MAIL FROM:<");
			_sendString.Append(userName);
			_sendString.Append(">");
			_sendString.Append(CRLF);
			bool _exResult = DialogEx(_sendString.ToString(), "发件人地址错误，或不能为空");
			if (!_exResult) return false;
			//收件人地址
			_sendBuffer = new string[mailMessage.Recipients.Count];
			for (int i = 0; i < mailMessage.Recipients.Count; i++) {
				_sendBuffer[i] = string.Format("RCPT TO:<{0}>{1}", mailMessage.Recipients[i] as string, CRLF);
			}
			_exResult = DialogEx(_sendBuffer, "收件人地址有误");
			if (!_exResult) return false;
			_sendString.Length = 0; //重置
			_sendString.Append("DATA");
			_sendString.Append(CRLF);
			_exResult = DialogEx(_sendString.ToString(), null);
			if (!_exResult) return false;
			//发件人姓名
			_sendString.Length = 0; //重置
			_sendString.Append("From:");
			_sendString.Append(mailMessage.FromName);
			_sendString.Append("<");
			_sendString.Append(mailMessage.From);
			_sendString.Append(">");
			_sendString.Append(CRLF);
			if (mailMessage.Recipients.Count == 0) {
				return false;
			}
			else {
				_sendString.Append("To:=?");
				_sendString.Append(mailMessage.Charset.ToUpper());
				_sendString.Append("?B?");
				_sendString.Append(Base64Encode(mailMessage.Recipients[0] as string));
				_sendString.Append("?=");
				_sendString.Append("<");
				_sendString.Append(mailMessage.Recipients[0] as string);
				_sendString.Append(">");
				_sendString.Append(CRLF);
			}
			string _tmpString = null;
			if (string.IsNullOrEmpty(mailMessage.Subject)) {
				_tmpString = "Subject:";
			}
			else if (string.IsNullOrEmpty(mailMessage.Charset)) {
				_tmpString = string.Format("Subject:{0}", mailMessage.Subject);
			}
			else {
				_tmpString = string.Format("Subject:=?{0}?B?{1}?=", mailMessage.Charset.ToUpper(), Base64Encode(mailMessage.Subject));
			}
			_sendString.Append(_tmpString);
			_sendString.Append(CRLF);
			_sendString.Append("X-Priority:");
			_sendString.Append(_priority);
			_sendString.Append(CRLF);
			_sendString.Append("X-MSMail-Priority:");
			_sendString.Append(_priority);
			_sendString.Append(CRLF);
			_sendString.Append("Importance:");
			_sendString.Append(_priority);
			_sendString.Append(CRLF);
			_sendString.Append("X-Mailer: DawnXZ.NuclearUtility.EMailOperation Pubclass [cn]");
			_sendString.Append(CRLF);
			_sendString.Append("MIME-Version: 1.0");
			_sendString.Append(CRLF);
			if (mailMessage.Attachments.Count > 0) {
				_sendString.Append("Content-Type: multipart/mixed;");
				_sendString.Append(CRLF);
				_sendString.Append(" boundary=\"=====");
				if (_htmlFlag) {
					_sendString.Append("001_Dragon520636771063_");
				}
				else {
					_sendString.Append("001_Dragon303406132050_");
				}
				_sendString.Append("=====\"");
				_sendString.Append(CRLF);
				_sendString.Append(CRLF);
			}
			if (_htmlFlag) {
				if (mailMessage.Attachments.Count > 0) {
					_sendString.Append("Content-Type: multipart/alternative;"); //内容格式和分隔符
					_sendString.Append(CRLF);
					_sendString.Append(" boundary=\"=====003_Dragon520636771063_=====\"");
					_sendString.Append(CRLF);
					_sendString.Append(CRLF);
					_sendString.Append("This is a multi-part message in MIME format.");
					_sendString.Append(CRLF);
					_sendString.Append(CRLF);
				}
				else {
					_sendString.Append("This is a multi-part message in MIME format.");
					_sendString.Append(CRLF);
					_sendString.Append(CRLF);
					_sendString.Append("--=====001_Dragon520636771063_=====");
					_sendString.Append(CRLF);
					_sendString.Append("Content-Type: multipart/alternative;"); //内容格式和分隔符
					_sendString.Append(CRLF);
					_sendString.Append(" boundary=\"=====003_Dragon520636771063_=====\"");
					_sendString.Append(CRLF);
					_sendString.Append(CRLF);
				}
				_sendString.Append("--=====003_Dragon520636771063_=====");
				_sendString.Append(CRLF);
				_sendString.Append("Content-Type: text/plain;");
				_sendString.Append(CRLF);
				if (string.IsNullOrEmpty(mailMessage.Charset)) {
					_sendString.Append(" charset=\"iso-8859-1\"");
				}
				else {
					_sendString.Append(" charset=\"");
					_sendString.Append(mailMessage.Charset.ToLower());
					_sendString.Append("\"");
				}
				_sendString.Append(CRLF);
				_sendString.Append("Content-Transfer-Encoding: base64");
				_sendString.Append(CRLF);
				_sendString.Append(CRLF);
				_sendString.Append(Base64Encode("邮件内容为HTML格式，请选择HTML方式查看"));
				_sendString.Append(CRLF);
				_sendString.Append(CRLF);
				_sendString.Append("--=====003_Dragon520636771063_=====");
				_sendString.Append(CRLF);
				_sendString.Append("Content-Type: text/html;");
				_sendString.Append(CRLF);
				if (string.IsNullOrEmpty(mailMessage.Charset)) {
					_sendString.Append(" charset=\"iso-8859-1\"");
				}
				else {
					_sendString.Append(" charset=\"");
					_sendString.Append(mailMessage.Charset.ToLower());
					_sendString.Append(CRLF);
				}
				_sendString.Append("Content-Transfer-Encoding: base64");
				_sendString.Append(CRLF);
				_sendString.Append(CRLF);
				_sendString.Append(Base64Encode(mailMessage.Body));
				_sendString.Append(CRLF);
				_sendString.Append(CRLF);
				_sendString.Append("--=====003_Dragon520636771063_=====--");
				_sendString.Append(CRLF);
			}
			else {
				if (mailMessage.Attachments.Count > 0) {
					_sendString.Append("--=====001_Dragon303406132050_=====");
					_sendString.Append(CRLF);
				}
				_sendString.Append("Content-Type: text/plain;");
				_sendString.Append(CRLF);
				if (string.IsNullOrEmpty(mailMessage.Charset)) {
					_sendString.Append(" charset=\"iso-8859-1\"");
				}
				else {
					_sendString.Append(" charset=\"");
					_sendString.Append(mailMessage.Charset.ToLower());
					_sendString.Append("\"");
				}
				_sendString.Append(CRLF);
				_sendString.Append("Content-Transfer-Encoding: base64");
				_sendString.Append(CRLF);
				_sendString.Append(CRLF);
				_sendString.Append(Base64Encode(mailMessage.Body));
				_sendString.Append(CRLF);
			}
			if (mailMessage.Attachments.Count > 0) {
				for (int i = 0; i < mailMessage.Attachments.Count; i++) {
					string _filePath = mailMessage.Attachments[i] as string;
					_sendString.Append("--=====");
					if (_htmlFlag) {
						_sendString.Append("001_Dragon520636771063_");
					}
					else {
						_sendString.Append("001_Dragon303406132050_");
					}
					_sendString.Append("=====");
					_sendString.Append(CRLF);
					_sendString.Append("Content-Type: text/plain;");
					_sendString.Append(CRLF);
					_sendString.Append(" name=\"=?");
					_sendString.Append(mailMessage.Charset.ToUpper());
					_sendString.Append("?B?");
					_sendString.Append(Base64Encode(_filePath.Substring(_filePath.LastIndexOf("\\") + 1)));
					_sendString.Append("?=\"");
					_sendString.Append(CRLF);
					_sendString.Append("Content-Transfer-Encoding: base64");
					_sendString.Append(CRLF);
					_sendString.Append("Content-Disposition: attachment;");
					_sendString.Append(CRLF);
					_sendString.Append(" filename=\"=?");
					_sendString.Append(mailMessage.Charset.ToUpper());
					_sendString.Append("?B?");
					_sendString.Append(Base64Encode(_filePath.Substring(_filePath.LastIndexOf("\\") + 1)));
					_sendString.Append("?=\"");
					_sendString.Append(CRLF);
					_sendString.Append(CRLF);
					_sendString.Append(GetStream(_filePath));
					_sendString.Append(CRLF);
					_sendString.Append(CRLF);
				}
				_sendString.Append("--=====");
				if (_htmlFlag) {
					_sendString.Append("001_Dragon520636771063_");
				}
				else {
					_sendString.Append("001_Dragon303406132050_");
				}
				_sendString.Append("=====--");
				_sendString.Append(CRLF);
				_sendString.Append(CRLF);
			}
			_sendString.Append(CRLF);
			_sendString.Append(".");
			_sendString.Append(CRLF);
			_exResult = DialogEx(_sendString.ToString(), "错误信件信息");
			if (!_exResult) return false;
			_sendString.Length = 0; //重置
			_sendString.Append("QUIT");
			_sendString.Append(CRLF);
			_exResult = DialogEx(_sendString.ToString(), "断开连接时错误");
			if (!_exResult) return false;
			FNetworkStream.Close();
			FTcpClient.Close();
			return true;
		}

		#endregion

		#region 成员方法

		/// <summary>
		/// 发送电子邮件
		/// <para>SMTP服务器不需要身份验证</para>
		/// </summary>
		/// <param name="smtpServer">发信SMTP服务器</param>
		/// <param name="smtpPort">端口，默认为25</param>
		/// <param name="mailMessage">邮件内容</param>
		public bool SendEmail(string smtpServer, int smtpPort, EMailMessage mailMessage) {
			return SendEmail(smtpServer, smtpPort, false, "", "", mailMessage);
		}
		/// <summary>
		/// 发送电子邮件
		/// <para>SMTP服务器需要身份验证</para>
		/// </summary>
		/// <param name="smtpServer">发信SMTP服务器</param>
		/// <param name="smtpPort">端口，默认为25</param>
		/// <param name="userName">发信人邮箱地址</param>
		/// <param name="password">发信人邮箱密码</param>
		/// <param name="mailMessage">邮件内容</param>
		public bool SendEmail(string smtpServer, int smtpPort, string userName, string password, EMailMessage mailMessage) {
			return SendEmail(smtpServer, smtpPort, true, userName, password, mailMessage);
		}

		#endregion

	}
}
