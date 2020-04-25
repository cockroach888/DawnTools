//==================================================================== 
//**   晨曦小竹常用工具集
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：EMailOperation.cs
// 项目名称：原子能式的高深学问方法实用工具集
// 创建时间：2016-04-14 18:14:59
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

namespace DawnXZ.NuclearUtility {
	/// <summary>
	/// 电子邮件[E-Mail]邮箱操作实现类
	/// </summary>
	public class EMailOperation {
		/// <summary>
		/// 电子邮件[E-Mail]邮箱操作实现类
		/// </summary>
		public EMailOperation() { ;}

		#region 成员变量

		/// <summary>
		/// 字节流读取对象
		/// </summary>
		private StreamReader FReader;
		/// <summary>
		/// 字节流写入对象
		/// </summary>
		private StreamWriter FWriter;
		/// <summary>
		/// TcpClient对象，用于连接服务器
		/// </summary>
		private TcpClient FTcpClient;
		/// <summary>
		/// 网络流对象
		/// </summary>
		private NetworkStream FNetworkStream;

		#endregion

		#region 成员服务器交互事件

		/// <summary>
		/// 向服务器发送命令信息
		/// </summary>
		/// <param name="cmdInfo">要发送的信息</param>
		/// <returns>信息发送结果</returns>
		private bool SendDataToServer(string cmdInfo) {
			try {
				FWriter.WriteLine(cmdInfo);
				FWriter.Flush();
				return true;
			}
			catch {
				return false;
			}
		}
		/// <summary>
		/// 从网络流中读取服务器回送的信息
		/// </summary>
		private string ReadDataFromServer() {
			string strMsg = null;
			try {
				strMsg = FReader.ReadLine();
				if (strMsg[0] == '-') strMsg = null;
			}
			catch (Exception ex) {
				strMsg = ex.Message;
			}
			return strMsg;
		}

		#endregion

		#region 成员方法

		#region 获取邮件信息

		/// <summary>
		/// 获取邮件信息
		/// </summary>
		/// <param name="userName">邮箱账号</param>
		/// <param name="userPwd">邮箱密码</param>
		/// <returns>邮件信息列表</returns>
		public ArrayList ReceiveMail(string userName, string userPwd) {
			ArrayList _mails = new ArrayList();
			int _index = userName.IndexOf('@');
			string pop3Server = string.Format("pop3.{0}", userName.Substring(_index + 1));
			FTcpClient = new TcpClient(pop3Server, 110);
			FNetworkStream = FTcpClient.GetStream();
			FReader = new StreamReader(FNetworkStream);
			FWriter = new StreamWriter(FNetworkStream);
			string _readResult = ReadDataFromServer();
			if (string.IsNullOrEmpty(_readResult)) return _mails;
			bool _sendResult = SendDataToServer(string.Format("USER {0}", userName)); //用户名
			if (!_sendResult) return _mails;
			_readResult = ReadDataFromServer();
			if (string.IsNullOrEmpty(_readResult)) return _mails;
			_sendResult = SendDataToServer(string.Format("PASS {0}", userPwd)); //密码
			if (!_sendResult) return _mails;
			_readResult = ReadDataFromServer();
			if (string.IsNullOrEmpty(_readResult)) return _mails;
			_sendResult = SendDataToServer("LIST"); //列表
			if (!_sendResult) return _mails;
			_readResult = ReadDataFromServer();
			if (string.IsNullOrEmpty(_readResult)) return _mails;
			string[] _readSplit = _readResult.Split(' '); //数据分割
			int _count = int.Parse(_readSplit[1]);
			if (_count <= 0) return _mails;
			for (int i = 0; i < _count; i++) {
				_readResult = ReadDataFromServer();
				if (string.IsNullOrEmpty(_readResult)) break;
				_readSplit = _readResult.Split(' '); //数据分割
				_mails.Add(string.Format("第{0}封邮件，{1}字节", _readSplit[0], _readSplit[1]));
			}
			return _mails;
		}

		#endregion

		#region 读取邮件内容

		/// <summary>
		/// 读取邮件内容
		/// </summary>
		/// <param name="mailMessage">邮件序列索引号</param>
		/// <returns>操作结果信息</returns>
		public string ReadEmail(int mailIndex) {
			string result = "Error";
			bool _exResult = SendDataToServer(string.Format("RETR {0}", mailIndex));
			if (_exResult) result = FReader.ReadToEnd();
			return result;
		}

		#endregion

		#region 删除邮件

		/// <summary>
		/// 删除邮件
		/// </summary>
		/// <param name="mailIndex">邮件序列索引号</param>
		/// <returns>操作结果信息</returns>
		public string DeleteEmail(int mailIndex) {
			string result = "Error";
			bool _exResult = SendDataToServer(string.Format("DELE {0}", mailIndex));
			if (_exResult) result = "成功删除";
			return result;
		}

		#endregion

		#region 关闭服务器连接

		/// <summary>
		/// 关闭服务器连接
		/// </summary>
		public void CloseConnection() {
			SendDataToServer("QUIT");
			FReader.Close();
			FWriter.Close();
			FNetworkStream.Close();
			FTcpClient.Close();
		}

		#endregion

		#endregion

	}
}
