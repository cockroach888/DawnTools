//==================================================================== 
//**   晨曦小竹常用工具集
//====================================================================
//**   Copyright © DawnXZ.com 2015 -- QQ：6808240 -- 请保留此注释
//====================================================================
// 文件名称：DynamicJson.cs
// 项目名称：常用方法实用工具集
// 创建时间：2015-04-26 17:23:03
// 创建人员：宋杰军
// 负 责 人：宋杰军
// 参与人员：宋杰军
// ===================================================================
// 修改日期：
// 修改人员：
// 修改内容：
// ===================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DawnXZ.DawnUtility {
	/// <summary>
	/// C#动态类型对象处理类
	/// <para>动态JSON对象实现</para>
	/// </summary>
	public class DynamicJson : DynamicObject {		
		/// <summary>
		/// 动态JSON对象实现
		/// </summary>
		public DynamicJson() {
			xml = new XElement("root", CreateTypeAttr(JsonType.@object));
			jsonType = JsonType.@object;
		}
		/// <summary>
		/// 动态JSON对象实现
		/// </summary>
		/// <param name="element">XML元素对象</param>
		/// <param name="type">JSON类型</param>
		private DynamicJson(XElement element, JsonType type) {
			Debug.Assert(type == JsonType.array || type == JsonType.@object);
			xml = element;
			jsonType = type;
		}
		
		#region 成员变量

		/// <summary>
		/// XML元素对象
		/// </summary>
		readonly XElement xml;
		/// <summary>
		/// JSON类型
		/// </summary>
		readonly JsonType jsonType;

		#endregion

		#region 成员属性

		/// <summary>
		/// 是否为对象
		/// </summary>
		public bool IsObject {
			get { return jsonType == JsonType.@object; }
		}
		/// <summary>
		/// 是否为数组
		/// </summary>
		public bool IsArray {
			get { return jsonType == JsonType.array; }
		}

		#endregion

		#region 成员方法

		/// <summary>
		/// 检测属性定义是否存在
		/// <para>属性名称</para>
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <returns>检测结果</returns>
		public bool IsDefined(string name) {
			return IsObject && (xml.Element(name) != null);
		}
		/// <summary>
		/// 检测属性定义是否存在
		/// <para>属性索引</para>
		/// </summary>
		/// <param name="index">属性索引</param>
		/// <returns>检测结果</returns>
		public bool IsDefined(int index) {
			return IsArray && (xml.Elements().ElementAtOrDefault(index) != null);
		}
		/// <summary>
		/// 删除指定名称的属性
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <returns>删除结果</returns>
		public bool Delete(string name) {
			var elem = xml.Element(name);
			if (elem != null) {
				elem.Remove();
				return true;
			}
			else return false;
		}
		/// <summary>
		/// 删除指定索引的属性
		/// </summary>
		/// <param name="index">属性索引</param>
		/// <returns>删除结果</returns>
		public bool Delete(int index) {
			var elem = xml.Elements().ElementAtOrDefault(index);
			if (elem != null) {
				elem.Remove();
				return true;
			}
			else return false;
		}
		/// <summary>
		/// 获得所有动态成员的名称
		/// </summary>
		/// <returns>结果集</returns>
		public override IEnumerable<string> GetDynamicMemberNames() {
			return (IsArray)
				? xml.Elements().Select((x, i) => i.ToString())
				: xml.Elements().Select(x => x.Name.LocalName);
		}
		/// <summary>
		/// 序列化为JSON字符串
		/// </summary>
		/// <returns>JSON字符串</returns>
		public override string ToString() {
			// <foo type="null"></foo> is can't serialize. replace to <foo type="null" />
			foreach (var elem in xml.Descendants().Where(x => x.Attribute("type").Value == "null")) {
				elem.RemoveNodes();
			}
			return CreateJsonString(new XStreamingElement("root", CreateTypeAttr(jsonType), xml.Elements()));
		}

		#region 反序列化

		/// <summary>
		/// 将映射到数组或类的公共属性名
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Deserialize<T>() {
			return (T)Deserialize(typeof(T));
		}
		/// <summary>
		/// 反序列化
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private object Deserialize(Type type) {
			return (IsArray) ? DeserializeArray(type) : DeserializeObject(type);
		}
		/// <summary>
		/// 反序列化值
		/// </summary>
		/// <param name="element"></param>
		/// <param name="elementType"></param>
		/// <returns></returns>
		private dynamic DeserializeValue(XElement element, Type elementType) {
			var value = ToValue(element);
			if (value is DynamicJson) {
				value = ((DynamicJson)value).Deserialize(elementType);
			}
			return Convert.ChangeType(value, elementType);
		}
		/// <summary>
		/// 反序列化对象
		/// </summary>
		/// <param name="targetType"></param>
		/// <returns></returns>
		private object DeserializeObject(Type targetType) {
			var result = Activator.CreateInstance(targetType);
			var dict = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.CanWrite)
				.ToDictionary(pi => pi.Name, pi => pi);
			foreach (var item in xml.Elements()) {
				PropertyInfo propertyInfo;
				if (!dict.TryGetValue(item.Name.LocalName, out propertyInfo)) continue;
				var value = DeserializeValue(item, propertyInfo.PropertyType);
				propertyInfo.SetValue(result, value, null);
			}
			return result;
		}
		/// <summary>
		/// 反序列化数据
		/// </summary>
		/// <param name="targetType"></param>
		/// <returns></returns>
		private object DeserializeArray(Type targetType) {
			if (targetType.IsArray) // Foo[]
            {
				var elemType = targetType.GetElementType();
				dynamic array = Array.CreateInstance(elemType, xml.Elements().Count());
				var index = 0;
				foreach (var item in xml.Elements()) {
					array[index++] = DeserializeValue(item, elemType);
				}
				return array;
			}
			else // List<Foo>
            {
				var elemType = targetType.GetGenericArguments()[0];
				dynamic list = Activator.CreateInstance(targetType);
				foreach (var item in xml.Elements()) {
					list.Add(DeserializeValue(item, elemType));
				}
				return list;
			}
		}

		#endregion

		#region Try

		/// <summary>
		/// Delete
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="args"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryInvoke(InvokeBinder binder, object[] args, out object result) {
			result = (IsArray)
				? Delete((int)args[0])
				: Delete((string)args[0]);
			return true;
		}
		/// <summary>
		/// IsDefined, if has args then TryGetMember
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="args"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
			if (args.Length > 0) {
				result = null;
				return false;
			}

			result = IsDefined(binder.Name);
			return true;
		}
		/// <summary>
		/// Deserialize or foreach(IEnumerable)
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryConvert(ConvertBinder binder, out object result) {
			if (binder.Type == typeof(IEnumerable) || binder.Type == typeof(object[])) {
				var ie = (IsArray)
					? xml.Elements().Select(x => ToValue(x))
					: xml.Elements().Select(x => (dynamic)new KeyValuePair<string, object>(x.Name.LocalName, ToValue(x)));
				result = (binder.Type == typeof(object[])) ? ie.ToArray() : ie;
			}
			else {
				result = Deserialize(binder.Type);
			}
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private bool TryGet(XElement element, out object result) {
			if (element == null) {
				result = null;
				return false;
			}
			result = ToValue(element);
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="indexes"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
			return (IsArray)
				? TryGet(xml.Elements().ElementAtOrDefault((int)indexes[0]), out result)
				: TryGet(xml.Element((string)indexes[0]), out result);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result) {
			return (IsArray)
				? TryGet(xml.Elements().ElementAtOrDefault(int.Parse(binder.Name)), out result)
				: TryGet(xml.Element(binder.Name), out result);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private bool TrySet(string name, object value) {
			var type = GetJsonType(value);
			var element = xml.Element(name);
			if (element == null) {
				xml.Add(new XElement(name, CreateTypeAttr(type), CreateJsonNode(value)));
			}
			else {
				element.Attribute("type").Value = type.ToString();
				element.ReplaceNodes(CreateJsonNode(value));
			}
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private bool TrySet(int index, object value) {
			var type = GetJsonType(value);
			var e = xml.Elements().ElementAtOrDefault(index);
			if (e == null) {
				xml.Add(new XElement("item", CreateTypeAttr(type), CreateJsonNode(value)));
			}
			else {
				e.Attribute("type").Value = type.ToString();
				e.ReplaceNodes(CreateJsonNode(value));
			}

			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="indexes"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value) {
			return (IsArray)
				? TrySet((int)indexes[0], value)
				: TrySet((string)indexes[0], value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool TrySetMember(SetMemberBinder binder, object value) {
			return (IsArray)
				? TrySet(int.Parse(binder.Name), value)
				: TrySet(binder.Name, value);
		}

		#endregion

		#endregion

		#region 成员静态方法场

		#region 成员方法

		#region Parse

		/// <summary>
		/// JSON字符串转换为动态JSON对象
		/// </summary>
		/// <param name="json">需要转换的JSON字符串</param>
		/// <returns>转换后的动态JSON对象</returns>
		public static dynamic Parse(string json) {
			return Parse(json, Encoding.Unicode);
		}
		/// <summary>
		/// JSON字符串转换为动态JSON对象
		/// <para>指定字符编码</para>
		/// </summary>
		/// <param name="json">需要转换的JSON字符串</param>
		/// <param name="encoding">字符编码</param>
		/// <returns>转换后的动态JSON对象</returns>
		public static dynamic Parse(string json, Encoding encoding) {
			using (var reader = JsonReaderWriterFactory.CreateJsonReader(encoding.GetBytes(json), XmlDictionaryReaderQuotas.Max)) {
				return ToValue(XElement.Load(reader));
			}
		}
		/// <summary>
		/// JSON流转换为动态JSON对象
		/// </summary>
		/// <param name="stream">需要转换的JSON流</param>
		/// <returns>转换后的动态JSON对象</returns>
		public static dynamic Parse(Stream stream) {
			using (var reader = JsonReaderWriterFactory.CreateJsonReader(stream, XmlDictionaryReaderQuotas.Max)) {
				return ToValue(XElement.Load(reader));
			}
		}
		/// <summary>
		/// JSON流转换为动态JSON对象
		/// <para>指定字符编码</para>
		/// </summary>
		/// <param name="stream">需要转换的JSON流</param>
		/// <param name="encoding">字符编码</param>
		/// <returns>转换后的动态JSON对象</returns>
		public static dynamic Parse(Stream stream, Encoding encoding) {
			using (var reader = JsonReaderWriterFactory.CreateJsonReader(stream, encoding, XmlDictionaryReaderQuotas.Max, _ => { })) {
				return ToValue(XElement.Load(reader));
			}
		}

		#endregion

		/// <summary>
		/// 将对象序列化为JSON字符串
		/// <para>来源于原始的primitive</para>
		/// <para>来源于IEnumerable</para>
		/// <para>来源于Object</para>
		/// </summary>
		/// <param name="obj">需要序列化的对象</param>
		/// <returns>序列化后的JSON字符串</returns>
		public static string Serialize(object obj) {
			return CreateJsonString(new XStreamingElement("root", CreateTypeAttr(GetJsonType(obj)), CreateJsonNode(obj)));
		}

		#endregion

		#region 私有方法

		/// <summary>
		/// 值转换
		/// </summary>
		/// <param name="element">需要对值转换的元素对象</param>
		/// <returns>转换结果</returns>
		private static dynamic ToValue(XElement element) {
			var type = (JsonType)Enum.Parse(typeof(JsonType), element.Attribute("type").Value);
			switch (type) {
				case JsonType.boolean:
					return (bool)element;
				case JsonType.number:
					return (double)element;
				case JsonType.@string:
					return (string)element;
				case JsonType.@object:
				case JsonType.array:
					return new DynamicJson(element, type);
				case JsonType.@null:
				default:
					return null;
			}
		}
		/// <summary>
		/// 获得JSON类型
		/// </summary>
		/// <param name="obj">元素对象</param>
		/// <returns>JSON类型</returns>
		private static JsonType GetJsonType(object obj) {
			if (obj == null) return JsonType.@null;
			switch (Type.GetTypeCode(obj.GetType())) {
				case TypeCode.Boolean:
					return JsonType.boolean;
				case TypeCode.String:
				case TypeCode.Char:
				case TypeCode.DateTime:
					return JsonType.@string;
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
				case TypeCode.SByte:
				case TypeCode.Byte:
					return JsonType.number;
				case TypeCode.Object:
					return (obj is IEnumerable) ? JsonType.array : JsonType.@object;
				case TypeCode.DBNull:
				case TypeCode.Empty:
				default:
					return JsonType.@null;
			}
		}

		#region Create

		/// <summary>
		/// JSON属性创建
		/// </summary>
		/// <param name="type">JSON类型</param>
		/// <returns>JSON属性</returns>
		private static XAttribute CreateTypeAttr(JsonType type) {
			return new XAttribute("type", type.ToString());
		}
		/// <summary>
		/// JSON节点创建
		/// </summary>
		/// <param name="obj">被创建的对象</param>
		/// <returns>JSON节点</returns>
		private static object CreateJsonNode(object obj) {
			var type = GetJsonType(obj);
			switch (type) {
				case JsonType.@string:
				case JsonType.number:
					return obj;
				case JsonType.boolean:
					return obj.ToString().ToLower();
				case JsonType.@object:
					return CreateXObject(obj);
				case JsonType.array:
					return CreateXArray(obj as IEnumerable);
				case JsonType.@null:
				default:
					return null;
			}
		}
		/// <summary>
		/// JOSN数组创建
		/// </summary>
		/// <typeparam name="T">泛型类型</typeparam>
		/// <param name="obj">泛型对象</param>
		/// <returns>JOSN数组</returns>
		private static IEnumerable<XStreamingElement> CreateXArray<T>(T obj) where T : IEnumerable {
			return obj.Cast<object>()
				.Select(o => new XStreamingElement("item", CreateTypeAttr(GetJsonType(o)), CreateJsonNode(o)));
		}
		/// <summary>
		/// JSON对象创建
		/// </summary>
		/// <param name="obj">被创建的对象</param>
		/// <returns>JSON对象</returns>
		private static IEnumerable<XStreamingElement> CreateXObject(object obj) {
			return obj.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Select(pi => new { Name = pi.Name, Value = pi.GetValue(obj, null) })
				.Select(a => new XStreamingElement(a.Name, CreateTypeAttr(GetJsonType(a.Value)), CreateJsonNode(a.Value)));
		}
		/// <summary>
		/// JSON字符串创建
		/// </summary>
		/// <param name="element">元素对象</param>
		/// <returns>JSON字符串</returns>
		private static string CreateJsonString(XStreamingElement element) {
			using (var ms = new MemoryStream())
			using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.Unicode)) {
				element.WriteTo(writer);
				writer.Flush();
				return Encoding.Unicode.GetString(ms.ToArray());
			}
		}

		#endregion

		#endregion

		#endregion

		#region JSON类型枚举

		/// <summary>
		/// JSON类型
		/// </summary>
		private enum JsonType {
			/// <summary>
			/// 字符串
			/// </summary>
			@string,
			/// <summary>
			/// 数字
			/// </summary>
			number,
			/// <summary>
			/// 布尔
			/// </summary>
			boolean,
			/// <summary>
			/// 对象
			/// </summary>
			@object,
			/// <summary>
			/// 数组
			/// </summary>
			array,
			/// <summary>
			/// 空对象
			/// </summary>
			@null
		}

		#endregion

	}
}
