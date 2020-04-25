using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DawnXZ.DawnApp {
	/// <summary>
	/// 登录界面
	/// </summary>
	public partial class frmLogin : Form {
		/// <summary>
		/// 构造函数
		/// </summary>
		public frmLogin() {
			InitializeComponent();
			InitializeThis();
		}

		#region 成员属性

		/// <summary>
		/// 验证码
		/// </summary>
		string FCheckCode { get; set; }

		#endregion

		#region 初始化

		/// <summary>
		/// 初始化
		/// </summary>
		private void InitializeThis() {
			this.FCheckCode = DawnXZ.DawnUtility.CheckCodeHelper.GetChs(4);
			this.Text = Core.ConfigHelper.AppName;
			this.picCoder.Image = DawnXZ.DawnUtility.CheckCodeHelper.CreateToImage(this.FCheckCode, true, false);
		}

		#endregion

		#region 窗体事件

		/// <summary>
		/// 窗体加载时
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送事件</param>
		private void frmLogin_Load(object sender, EventArgs e) {
			DawnXZ.FormUtility.FEffects.ShowEff(this.Handle, 500, DawnXZ.FormUtility.FEffects.AW_ACTIVATE | DawnXZ.FormUtility.FEffects.AW_CENTER);
		}
		/// <summary>
		/// 窗体显示时
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送事件</param>
		private void frmLogin_Shown(object sender, EventArgs e) {

		}
		/// <summary>
		/// 窗体关闭时
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送事件</param>
		private void frmLogin_FormClosing(object sender, FormClosingEventArgs e) {
			if (MessageBox.Show(string.Format("您确定要退出【{0}】吗？", Core.ConfigHelper.AppName), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
				this.FCloseFlag = true;
				e.Cancel = false;
				DawnXZ.FormUtility.FEffects.ShowEff(this.Handle, 500, DawnXZ.FormUtility.FEffects.AW_HIDE | DawnXZ.FormUtility.FEffects.AW_BLEND);
			}
			else {
				e.Cancel = true;
			}
		}
		/// <summary>
		/// 窗体结束时
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送事件</param>
		private void frmLogin_FormClosed(object sender, FormClosedEventArgs e) {
			this.Dispose();
			if (this.FCloseFlag) {
				Application.ExitThread();
				Application.Exit();
			}
		}

		#endregion

		#region 成员变量

		/// <summary>
		/// 关闭标识
		/// </summary>
		bool FCloseFlag = false;
		/// <summary>
		/// 登录窗体
		/// </summary>
		private static frmLogin FFormLogin;

		#endregion
		
		#region 成员方法

		#region 登录窗体状态

		/// <summary>
		/// 登录开始
		/// </summary>
		/// <returns></returns>
		public static frmLogin Begin() {
			if (FFormLogin == null) {
				FFormLogin = new frmLogin();
			}
			Thread td = new Thread(FFormLogin.ShowFormLogin);
			td.IsBackground = true;
			td.Start();
			return FFormLogin;
		}
		/// <summary>
		/// 登录结束
		/// </summary>
		public static void End() {
			FFormLogin.CloseFormLogin();
		}
		/// <summary>
		/// 显示登录窗体
		/// </summary>
		public void ShowFormLogin() {
			if (InvokeRequired) {
				BeginInvoke(new Action(ShowFormLogin));
				return;
			}
			this.Show();
			Application.Run(this);
		}
		/// <summary>
		/// 关闭登录窗体
		/// </summary>
		public void CloseFormLogin() {
			if (InvokeRequired) {
				BeginInvoke(new Action(CloseFormLogin));
				return;
			}
			this.Close();
		}

		#endregion

		/// <summary>
		/// 登录事件
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送方法</param>
		private void btnLogin_Click(object sender, EventArgs e) {
			MessageBox.Show(string.Format("系统生成的验证码：{0}", this.FCheckCode), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
			MessageBox.Show(string.Format("您输入的验证码：{0}", this.txtCoder.Text), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
			MessageBox.Show("登录成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		#endregion

	}
}
