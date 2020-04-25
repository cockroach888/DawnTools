using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DawnXZ.DawnApp {
	/// <summary>
	/// 闪屏加载界面
	/// </summary>
	public partial class SplashScreen : Form {
		/// <summary>
		/// 构造函数
		/// </summary>
		public SplashScreen() {
			InitializeComponent();
			this.Text = Core.ConfigHelper.AppName;
			this.lblTitle.Text = this.Text;
			this.lblVer.Text = Core.ConfigHelper.AppVersion;
		}

		#region 窗体事件

		/// <summary>
		/// 窗体加载时
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送事件</param>
		private void SplashScreen_Load(object sender, EventArgs e) {
			DawnXZ.FormUtility.FEffects.ShowEff(this.Handle, 500, DawnXZ.FormUtility.FEffects.AW_ACTIVATE | DawnXZ.FormUtility.FEffects.AW_CENTER);
		}
		/// <summary>
		/// 窗体显示时
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送事件</param>
		private void SplashScreen_Shown(object sender, EventArgs e) {

		}
		/// <summary>
		/// 窗体关闭时
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送事件</param>
		private void SplashScreen_FormClosing(object sender, FormClosingEventArgs e) {
			DawnXZ.FormUtility.FEffects.ShowEff(this.Handle, 500, DawnXZ.FormUtility.FEffects.AW_HIDE | DawnXZ.FormUtility.FEffects.AW_BLEND);
			if (!FCloseFlag) e.Cancel = true;
		}
		/// <summary>
		/// 窗体结束时
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送事件</param>
		private void SplashScreen_FormClosed(object sender, FormClosedEventArgs e) {
			this.Dispose();
		}

		#endregion

		#region 成员变量

		/// <summary>
		/// 关闭标识
		/// </summary>
		bool FCloseFlag = false;
		/// <summary>
		/// 闪屏窗体
		/// </summary>
		private static SplashScreen FSSC;

		#endregion

		#region 成员方法

		#region 闪屏窗体状态

		/// <summary>
		/// 闪屏开始
		/// </summary>
		/// <returns></returns>
		public static SplashScreen Begin() {
			if (FSSC == null) {
				FSSC = new SplashScreen();
			}
			Thread td = new Thread(FSSC.ShowSplashScreen);
			td.IsBackground = true;
			td.Start();
			return FSSC;
		}
		/// <summary>
		/// 闪屏结束
		/// </summary>
		public static void End() {
			FSSC.CloseSplashScreen();
		}
		/// <summary>
		/// 显示闪屏窗体
		/// </summary>
		public void ShowSplashScreen() {
			if (InvokeRequired) {
				BeginInvoke(new Action(ShowSplashScreen));
				return;
			}
			this.Show();
			Application.Run(this);
		}
		/// <summary>
		/// 关闭闪屏窗体
		/// </summary>
		public void CloseSplashScreen() {
			if (InvokeRequired) {
				BeginInvoke(new Action(CloseSplashScreen));
				return;
			}
			FCloseFlag = true;
			this.Close();
		}

		#endregion

		#region 闪屏消息容器

		/// <summary>
		/// 显示指定消息内容
		/// </summary>
		/// <param name="strText">消息内容</param>
		public void MsgUpdate(string strText) {
			MsgUpdate(strText, MessageStatus.Success);
		}
		/// <summary>
		/// 显示指定消息内容
		/// </summary>
		/// <param name="strText">消息内容</param>
		/// <param name="msta">消息状态</param>
		public void MsgUpdate(string strText, MessageStatus msta) {
			if (InvokeRequired) {
				BeginInvoke(new Action<string, MessageStatus>(MsgUpdate), new object[] { strText, msta });
				return;
			}
			switch (msta) {
				case MessageStatus.Error:
					lblInfo.ForeColor = Color.Red;
					break;
				case MessageStatus.Warning:
					lblInfo.ForeColor = Color.Yellow;
					break;
				case MessageStatus.Success:
					lblInfo.ForeColor = Color.White;
					break;
			}
			lblInfo.Text = strText;
		}

		#endregion

		#region 闪屏消息状态

		/// <summary>
		/// 消息状态
		/// </summary>
		public enum MessageStatus {
			/// <summary>
			/// 成功
			/// </summary>
			Success,
			/// <summary>
			/// 警告
			/// </summary>
			Warning,
			/// <summary>
			/// 错误
			/// </summary>
			Error,
		}

		#endregion

		#endregion

	}
}
