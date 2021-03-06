﻿//==================================================================== 
//*****                    晨曦小竹常用工具集                    *****
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释   **
//====================================================================
// 文件名称：Toolskit.cs
// 项目名称：窗体常用操作工具集
// 创建时间：2014年3月7日19时06分
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;

namespace DawnXZ.FormUtility
{
    /// <summary>
    /// 常用方法
    /// </summary>
    public static class Toolskit
    {
        /// <summary>
        /// 软件版本号
        /// </summary>
        public static string GetVersion
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            }
        }

        #region 内存回收

        /// <summary>
        /// 重新设定进程工作集大小
        /// </summary>
        /// <param name="process">进程句柄号</param>
        /// <param name="minSize">最小值</param>
        /// <param name="maxSize">最大值</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        #endregion

    }
}
