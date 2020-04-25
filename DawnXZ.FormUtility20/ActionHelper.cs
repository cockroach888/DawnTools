//==================================================================== 
//**   解决方案或项目总名称
//====================================================================
//**   Copyright © DawnXZ.com 2014 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：ActionHelper.cs
// 项目名称：请更改为实际项目名称
// 创建时间：2014-10-20 15:39:03
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;

namespace DawnXZ.FormUtility {
	/// <summary>
	/// Action
	/// </summary>
	public delegate void Action();
	/// <summary>
	/// Action
	/// <para>T - T1</para>
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	/// <typeparam name="T1">T1</typeparam>
	/// <param name="t">T</param>
	/// <param name="t1">T1</param>
	public delegate void Action<T, T1>(T t, T1 t1);
	/// <summary>
	/// Action
	/// <para>T - T1 - T2</para>
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	/// <typeparam name="T1">T1</typeparam>
	/// <typeparam name="T2">T2</typeparam>
	/// <param name="t">T</param>
	/// <param name="t1">T1</param>
	/// <param name="t2">T2</param>
	public delegate void Action<T, T1, T2>(T t, T1 t1, T2 t2);
	/// <summary>
	/// Action
	/// <para>T - T1 - T2 - T3</para>
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	/// <typeparam name="T1">T1</typeparam>
	/// <typeparam name="T2">T2</typeparam>
	/// <typeparam name="T3">T3</typeparam>
	/// <param name="t">T</param>
	/// <param name="t1">T1</param>
	/// <param name="t2">T2</param>
	/// <param name="t3">T3</param>
	public delegate void Action<T, T1, T2, T3>(T t, T1 t1, T2 t2, T3 t3);
	/// <summary>
	/// Func
	/// <para>T</para>
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	/// <returns>T</returns>
	public delegate T Func<T>();
	/// <summary>
	/// Func
	/// <para>T - T1</para>
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	/// <typeparam name="T1">T1</typeparam>
	/// <returns>T1</returns>
	public delegate T1 Func<T, T1>();
	/// <summary>
	/// Func
	/// <para>T - T1 - T2</para>
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	/// <typeparam name="T1">T1</typeparam>
	/// <typeparam name="T2">T2</typeparam>
	/// <returns>T2</returns>
	public delegate T2 Func<T, T1, T2>();
	/// <summary>
	/// Func
	/// <para>T - T1 - T2 - T3</para>
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	/// <typeparam name="T1">T1</typeparam>
	/// <typeparam name="T2">T2</typeparam>
	/// <typeparam name="T3">T3</typeparam>
	/// <returns>T3</returns>
	public delegate T3 Func<T, T1, T2, T3>();
}
