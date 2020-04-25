using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DawnXZ.DawnApp {
	public partial class frmBuilding : Form {
		/// <summary>
		/// 构造函数
		/// </summary>
		public frmBuilding() {
			InitializeComponent();
		}
		/// <summary>
		/// 窗体加载
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmBuilding_Load(object sender, EventArgs e) {
			this.cboxFormat.SelectedIndex = 0;
		}

		private void btnBuildGUID_Click(object sender, EventArgs e) {
			var _format = this.cboxFormat.SelectedItem.ToString();
			var result = Guid.NewGuid().ToString(_format);
			if (chkToUpper.Checked) result = result.ToUpper();
			this.txtHashCode.Text = result.GetHashCode().ToString();
			this.txtGuidCode.Text = result;
		}
	}
}
