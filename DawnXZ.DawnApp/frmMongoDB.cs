using DawnXZ.DBUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DawnXZ.DawnApp {
	/// <summary>
	/// 
	/// </summary>
	public partial class frmMongoDB : Template.TmpForm {
		/// <summary>
		/// 
		/// </summary>
		public frmMongoDB() {
			InitializeComponent();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLoadMongo_Click(object sender, EventArgs e) {
			try {
				var data = MongoDBHelper<Entity.CasePartTemplateInfo>.Select<Entity.CasePartTemplateInfo>(Core.ConnHelper.MongoConn, Core.ConnHelper.MongoName, "CasePartTemplate");
				this.dataGridView1.DataSource = data;
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
