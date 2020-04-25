//==================================================================== 
//*****                    晨曦小竹常用工具集                    *****
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释   **
//====================================================================
// 文件名称：CpuUsage.cs
// 项目名称：物理操作实用工具集
// 创建时间：2014年2月25日10时58分
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;

namespace DawnXZ.PHYUtility
{
    /// <summary>
    /// 定义 CPU 使用情况计数器实现的抽象的基类。
    /// </summary>
    public abstract class CpuUsage
    {
        /// <summary>
        /// 创建并返回一个 CpuUsage 实例可以用于查询在此操作系统上的 CPU 时间。
        /// </summary>
        /// <returns>CpuUsage 类的实例。</returns>
        /// <exception cref="NotSupportedException">不支持这个平台-或者-CPUUsage 对象的初始化失败。</exception>
        public static CpuUsage Create()
        {
            if (m_CpuUsage == null)
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    m_CpuUsage = new CpuUsageNt();
                }
                else if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
                {
                    m_CpuUsage = new CpuUsage9x();
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            return m_CpuUsage;
        }
        /// <summary>
        /// 确定当前的平均 CPU 负载。
        /// </summary>
        /// <returns>一个整数，持有的 CPU 负载百分比。</returns>
        /// <exception cref="NotSupportedException">其中一个系统调用失败。不能获得的 CPU 时间。</exception>
        public abstract int Query();
        /// <summary>
        /// 持有 CPUUsage 类的一个实例。
        /// </summary>
        private static CpuUsage m_CpuUsage = null;
    }
}
