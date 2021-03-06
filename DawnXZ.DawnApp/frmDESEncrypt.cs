﻿using System;
using System.Windows.Forms;
using DawnXZ.DawnUtility;
using DawnXZ.VerifyUtility;

namespace DawnXZ.DawnApp {
	/// <summary>
	/// 
	/// </summary>
	public partial class frmDESEncrypt : Form {
		/// <summary>
		/// 构造函数
		/// </summary>
		public frmDESEncrypt() {
			InitializeComponent();
		}
		/// <summary>
		/// 窗口加载
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmDESEncrypt_Load(object sender, EventArgs e) {

		}
		/// <summary>
		/// 通用密钥加密数据
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDESOrigin_Click(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(txtDESOrigin.Text)) {
				txtDESValue.Clear();
				txtDESValue.Text = CryptoHelper.Encrypt(txtDESOrigin.Text);
			}
		}
		/// <summary>
		/// 通用密钥解密数据
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDESValue_Click(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(txtDESOrigin.Text)) {
				txtDESValue.Clear();
				try {
					txtDESValue.Text = CryptoHelper.Decrypt(txtDESOrigin.Text);
				}
				catch {
					txtDESValue.Text = "错误的加密原数据！";
				}
			}
		}
		/// <summary>
		/// MD5数据生成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMD5_Click(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(txtMD5Origin.Text)) {
				txtMD5Value.Clear();
				txtMD5Value.Text = CryptoHelper.MD5(txtMD5Origin.Text, true);
			}
		}
		/// <summary>
		/// 转Base64
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnToBase64_Click(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(txtBase64Origin.Text.Trim())) {
				this.txtBase64Value.Clear();
				this.txtBase64Value.Text = TypeHelper.ToBase64Encode(txtBase64Origin.Text.Trim());
			}
		}
		/// <summary>
		/// 反转Base64
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnToUnBase64_Click(object sender, EventArgs e) {
			string strVal = txtBase64Origin.Text.Trim();
			if (!string.IsNullOrEmpty(strVal)) {
				this.txtBase64Value.Clear();
				if (!ValidHelper.IsBase64String(strVal)) {
					this.txtBase64Value.Text = "错误的[Base64]原数据！";
					return;
				}
				this.txtBase64Value.Text = TypeHelper.ToBase64Decode(txtBase64Origin.Text.Trim());
			}
		}
	}
}
