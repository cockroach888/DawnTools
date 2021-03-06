﻿//==================================================================== 
//*****                    晨曦小竹常用工具集                    *****
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释   **
//====================================================================
// 文件名称：PageElapsed.cs
// 项目名称：网页网站实用工具集
// 创建时间：2014年2月21日17时31分
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;
using System.Web;

namespace DawnXZ.WebUtility
{
    /// <summary>
    /// 页面运行时间辅助类
    /// <remarks>
    /// <para>在 Web.config 文件中的 <httpModules></httpModules> 中加入下面代码</para>
    /// <para>&lt;add name="IsPageElapsed" type="DawnXZ.WebUtility.PageElapsed, WebUtility" /&gt;</para>
    /// <para>需要显示的地方加入下面代码</para>
    /// <para>&lt;div datasrc="#PageElapseTime" datafld="ElapseTime"&gt;&lt;/div&gt;</para>
    /// </remarks>
    /// </summary>
    public class PageElapsed : IHttpModule
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime FStartTime;
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="context"></param>
        public void Init(System.Web.HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
            context.EndRequest += new EventHandler(OnEndRequest);
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        { }
        /// <summary>
        /// 在开始时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBeginRequest(object sender, EventArgs e)
        {
            FStartTime = DateTime.Now;
        }
        /// <summary>
        /// 在结束时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndRequest(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - FStartTime;
            HttpContext.Current.Response.Write("<XML ID=PageElapseTime><Time><ElapseTime>页面执行时间：" + ts.TotalMilliseconds + "ms</ElapseTime></Time></XML>");
        }
    }
}
