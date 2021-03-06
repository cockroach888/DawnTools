﻿//==================================================================== 
//*****                    晨曦小竹常用工具集                    *****
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释   **
//====================================================================
// 文件名称：ServiceHelper.cs
// 项目名称：网页网站实用工具集
// 创建时间：2014年2月24日17时54分
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;

namespace DawnXZ.WebUtility
{
    /// <summary>
    /// WebService动态调用操作辅助类
    /// </summary>
    public class ServiceHelper
    {
        /// <summary>
        /// web服务的地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// web服务的方法名
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// web服务的方法参数
        /// </summary>
        public object[] Args { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 是否需要身份验证
        /// </summary>
        public bool NeedCredential { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ServiceHelper()
        {
            Url = string.Empty;
            MethodName = string.Empty;
            Args = null;
            Username = string.Empty;
            Password = string.Empty;
            Domain = string.Empty;
            NeedCredential = false;
        }
        /// <summary>
        /// 调用WebService
        /// </summary>
        public object Response()
        {
            object obj = null;
            Type t = typeof(ServiceProxy);
            AppDomain otherDomain = AppDomain.CreateDomain("WebServieDoamin");
            try
            {
                ServiceProxy proxy = (ServiceProxy)otherDomain.CreateInstanceAndUnwrap(t.Assembly.FullName, t.FullName);
                proxy.Url = Url;
                proxy.MethodName = MethodName;
                proxy.Args = Args;
                proxy.Username = Username;
                proxy.Password = Password;
                proxy.Domain = Domain;
                proxy.NeedCredential = NeedCredential;
                obj = proxy.InvokeWebService();
            }
            finally
            {
                AppDomain.Unload(otherDomain);
            }
            return obj;
        }
    }
}
