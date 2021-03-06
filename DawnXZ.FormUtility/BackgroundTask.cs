﻿//==================================================================== 
//*****                   晨曦小竹常用工具集                   *****
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释   **
//====================================================================
// 文件名称：BackgroundTask.cs
// 项目名称：窗体常用操作工具集
// 创建时间：2014-03-20 11:08:34
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;
using System.ComponentModel;

namespace DawnXZ.FormUtility
{
    /// <summary>
    /// 后台执行事件
    /// <para>使用方法：</para>
    /// <para>private void loadData()</para>
    /// <para>{</para>
    /// <para>　　BackgroundTask.BackgroundWork(getData, null);</para>
    /// <para>}</para>
    /// <para>private void getData(object obj)</para>
    /// <para>{</para>
    /// <para>　　//这里执行耗时操作 的代码 譬如，加载远程数据之类，还有绑定数据到UI</para>
    /// <para>　　//比如this.gridview.datasource = 之类的操作</para>
    /// <para>}</para>
    /// <para></para>
    /// <para></para>
    /// <para></para>
    /// <para></para>
    /// </summary>
    public static class BackgroundTask
    {
        /// <summary>
        /// 加载提示窗口
        /// </summary>
        private static WaitDialogForm LoadingDlgForm = null;
        /// <summary>
        /// 后台执行事件
        /// </summary>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        public static void BackgroundWork(Action<object> action, object obj)
        {
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.RunWorkerCompleted += (s, e) =>
               {
                   LoadingDlgForm.Close();
                   LoadingDlgForm.Dispose();
               };
                bw.DoWork += (s, e) =>
                 {
                     try
                     {
                         Action<object> a = action;
                         a.Invoke(obj);
                     }
                     catch { }
                 };
                bw.RunWorkerAsync();
                LoadingDlgForm = new WaitDialogForm("正在加载中......");
            }
        }
    }
}
