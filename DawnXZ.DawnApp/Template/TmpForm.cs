using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DawnXZ.DawnApp.Template {
	/// <summary>
	/// 窗体类模板控件
	/// </summary>
	public partial class TmpForm : Form {
		/// <summary>
		/// 构造函数
		/// </summary>
		public TmpForm() {
			InitializeComponent();
		}
		/// <summary>
		/// 窗体加载时
		/// </summary>
		/// <param name="sender">传送对象</param>
		/// <param name="e">传送事件</param>
		private void TmpForm_Load(object sender, EventArgs e) {
			this.Text = Core.ConfigHelper.AppName;
		}
	}
}
