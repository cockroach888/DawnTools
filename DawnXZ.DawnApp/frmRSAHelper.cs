using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DawnXZ.DawnUtility;

namespace DawnXZ.DawnApp {
	/// <summary>
	/// 
	/// </summary>
	public partial class frmRSAHelper : Form {
		/// <summary>
		/// 
		/// </summary>
		public frmRSAHelper() {
			InitializeComponent();
		}

		private void btnBuildRSAStr_Click(object sender, EventArgs e) {
			var _key = RSAHelper.GetRASKey();
			txtPublicString.Text = _key.PublicKey;
			txtPrivateString.Text = _key.PrivateKey;
		}

		private void btnPublicEncrypt_Click(object sender, EventArgs e) {
			var _val = txtPublicString.Text.Trim();
			if (string.IsNullOrEmpty(_val)) return;
			txtEncryptSecret.Text = _val;
		}

		private void btnPublicDecrypt_Click(object sender, EventArgs e) {
			var _val = txtPublicString.Text.Trim();
			if (string.IsNullOrEmpty(_val)) return;
			txtDecryptSecret.Text = _val;
		}

		private void btnPrivateEncrypt_Click(object sender, EventArgs e) {
			var _val = txtPrivateString.Text.Trim();
			if (string.IsNullOrEmpty(_val)) return;
			txtEncryptSecret.Text = _val;
		}

		private void btnPrivateDecrypt_Click(object sender, EventArgs e) {
			var _val = txtPrivateString.Text.Trim();
			if (string.IsNullOrEmpty(_val)) return;
			txtDecryptSecret.Text = _val;
		}

		private void btnEncrypt_Click(object sender, EventArgs e) {
			var _val = txtEncryptString.Text;
			var _key = txtEncryptSecret.Text;
			if (string.IsNullOrEmpty(_val) || string.IsNullOrEmpty(_key)) {
				MessageBox.Show("需要加密的明文和加密密钥均不能为空，请重试。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else {
				txtEncryptResult.Text = RSAHelper.EncryptString(_val, _key);
			}
		}

		private void btnDecrypt_Click(object sender, EventArgs e) {
			var _val = txtDecryptString.Text;
			var _key = txtDecryptSecret.Text;
			if (string.IsNullOrEmpty(_val) || string.IsNullOrEmpty(_key)) {
				MessageBox.Show("需要解密的密文和解密密钥均不能为空，请重试。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else {
				txtDecryptResult.Text = RSAHelper.DecryptString(_val, _key);
			}
		}
	}
}
