using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace DawnXZ.DawnApp {
	static class Program {
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() {
			try {
				//处理未捕获的异常
				Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
				//处理UI线程异常
				Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
				//处理非UI线程异常
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
				//启用视觉样式
				Application.EnableVisualStyles();
				//设置默认的文本渲染兼容方式
				Application.SetCompatibleTextRenderingDefault(false);
				// ********** 方式一 **********
				bool createdNew; //返回是否赋予了使用线程的互斥体初始所属权
				Mutex instance = new Mutex(true, "MutexName", out createdNew); //同步基元变量
				if (createdNew) //赋予了线程初始所属权，也就是首次使用互斥体
                {
					Application.Run(new frmMain()); //这句是系统自动写的
					instance.ReleaseMutex();
				}
				else {
					MessageBox.Show("系统正在运行中，请勿重复运行，谢谢！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					Application.Exit();
					return;
				}
				// ********** 方式二 **********
				//if (AppAlreadyRunning())
				//{
				//    MessageBox.Show("系统正在运行中，请勿重复运行，谢谢！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				//    Application.Exit();
				//    return;
				//}
				//else
				//{
				//    Application.Run(new frmMain()); //这句是系统自动写的
				//}
				// ********** 方式三 **********
				//Process[] pros = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
				//if (pros.Length > 1)
				//{
				//    MessageBox.Show("系统正在运行中，请勿重复运行，谢谢！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				//    Application.Exit();
				//    return;
				//}
				//else
				//{
				//    Application.Run(new frmMain()); //这句是系统自动写的
				//}
			}
			catch (Exception ex) {
				string strMsg = string.Format("未知系统错误！（请联系管理员或相关人员[{0}]）【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message);
				MessageBox.Show(strMsg, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);//调试用
			}
		}
		/// <summary>
		/// 判断应用程序是否已经运行
		/// </summary>
		/// <returns></returns>
		private static bool AppAlreadyRunning() {
			Process curProcess = Process.GetCurrentProcess();
			Process[] allProcess = Process.GetProcesses();
			foreach (Process process in allProcess) {
				if (process.Id != curProcess.Id) {
					if (process.ProcessName == curProcess.ProcessName) return true;
				}
			}
			return false;
		}

		#region 捕捉全局异常代码

		/// <summary>
		/// 处理UI线程异常
		/// </summary>
		/// <param name="sender">object</param>
		/// <param name="e">ThreadExceptionEventArgs</param>
		static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
			Exception error = e.Exception as Exception;
			string strMsg = string.Empty;
			if (error != null) {
				strMsg = string.Format("出现应用程序未处理异常！（请联系管理员或相关人员[{0}]）【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), error.Message);
			}
			else {
				strMsg = string.Format("应用程序线程错误！（请联系管理员或相关人员[{0}]）", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			MessageBox.Show(strMsg, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);//调试用
		}
		/// <summary>
		/// 处理非UI线程异常
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			Exception error = e.ExceptionObject as Exception;
			string strMsg = string.Empty;
			if (error != null) {
				strMsg = string.Format("出现应用程序未处理异常！（请联系管理员或相关人员[{0}]）【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), error.Message);
			}
			else {
				strMsg = string.Format("应用程序线程错误！（请联系管理员或相关人员[{0}]）", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			MessageBox.Show(strMsg, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);//调试用
		}

		#endregion

	}
}
