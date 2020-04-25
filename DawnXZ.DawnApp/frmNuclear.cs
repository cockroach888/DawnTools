using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DawnXZ.VerifyUtility;
using DawnXZ.NuclearUtility;
using DawnXZ.DawnUtility;

namespace DawnXZ.DawnApp {
	public partial class frmNuclear : Form {
		/// <summary>
		/// 构造函数
		/// </summary>
		public frmNuclear() {
			InitializeComponent();
		}
		/// <summary>
		/// 窗体加载
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmNuclear_Load(object sender, EventArgs e) {
			lstRecipients.Items.Clear();
		}

		#region 成员变量



		#endregion

		#region 成员按钮事件（电子邮件）

		/// <summary>
		/// 增加收件人信息
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRecipients_Click(object sender, EventArgs e) {
			var _val = txtRecipients.Text.Trim();
			if (ValidHelper.IsEmailValid(_val)) {
				lstRecipients.BeginUpdate();
				lstRecipients.Items.Add(_val);
				lstRecipients.EndUpdate();
				//MessageBox.Show(string.Format("对不起！收件人数量已达到最大设定值【{0}】。", FMailMessage.MaxRecipientNum), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else {
				MessageBox.Show(ValidError.IsEmailValid, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		/// <summary>
		/// 开启电子邮件发射之旅
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMailSend_Click(object sender, EventArgs e) {
			if (lstRecipients.Items.Count <= 0) {
				MessageBox.Show("收件人信息不能为空。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			//SMTP 服务器信息
			var _server = txtSMTPServer.Text.Trim();
			var _port = TypeHelper.TypeToInt32(txtSMTPPort.Text.Trim(), 25);
			var _user = txtSMTPUser.Text.Trim();
			var _pwd = txtSMTPPwd.Text.Trim();
			//服务器信息
			EMailSrvInfo _srvInfo = new EMailSrvInfo();
			_srvInfo.SMTPServer = _server;
			_srvInfo.SMTPPort = _port;
			_srvInfo.UserName = _user;
			_srvInfo.Password = _pwd;
			_srvInfo.DisplayName = txtMailUser.Text.Trim();
			//电子邮件信息
			EMailInfo _mailInfo = new EMailInfo();
			foreach (string item in lstRecipients.Items) {
				_mailInfo.Recipient.Enqueue(item);
			}
			_mailInfo.Subject = txtMailSubject.Text.Trim();
			_mailInfo.IsHTML = chkMailHtml.Checked;
			_mailInfo.Body = txtMailBody.Text.Trim();
			EMailHelper _mailHelper = new EMailHelper(_srvInfo);
			_mailHelper.Send(_mailInfo);
			if (_mailHelper.IsSuccessful) {
				MessageBox.Show("电子邮件发送成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else {
				MessageBox.Show(string.Format("电子邮件发送失败！【原因：{0}】", _mailHelper.LastErrorMessage), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		#endregion

	}
}
