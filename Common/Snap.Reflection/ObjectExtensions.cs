using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Snap.Reflection
{
    /// <summary>
    /// 反射扩展方法
    /// </summary>
    [SuppressMessage("", "SA1600")]
    public static class ObjectExtensions
    {
        public static void ForEachAttribute<TAttribute>(this object obj, Action<TAttribute> action)
            where TAttribute : Attribute
        {
            foreach (TAttribute attribute in obj.GetType().GetCustomAttributes<TAttribute>())
            {
                action(attribute);
            }
        }

        public static void ForEachPropertyInfo(this object obj, Action<PropertyInfo> action)
        {
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                action(propInfo);
            }
        }

        public static void ForEachPropertyInfoWithAttribute<TAttribute>(this object obj, Action<PropertyInfo, TAttribute> action) where TAttribute : Attribute
        {
            obj.ForEachPropertyInfo(propInfo =>
            propInfo.OnHaveAttribute<TAttribute>(attribute =>
            action(propInfo, attribute)));
        }

        /// <summary>
        /// 循环操作每个不包含对应特性的属性
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="action">执行的操作</param>
        public static void ForEachPropertyInfoWithoutAttribute<TAttribute>(this object obj, Action<PropertyInfo> action) where TAttribute : Attribute
        {
            obj.ForEachPropertyInfo(propInfo =>
            propInfo.OnNotHaveAttribute<TAttribute>(() =>
            action(propInfo)));
        }

        public static void ForEachPropertyWithAttribute<TAttribute>(this object obj, Action<object, TAttribute> action) where TAttribute : Attribute
        {
            obj.ForEachPropertyInfo(propInfo =>
            propInfo.OnHaveAttribute<TAttribute>(attribute =>
            action(obj.GetPropertyValueByInfo(propInfo)!, attribute)));
        }

        public static void ForEachPropertyWithAttribute<TProperty, TAttribute>(this object obj, Action<TProperty, TAttribute> action) where TProperty : class where TAttribute : Attribute
        {
            obj.ForEachPropertyInfo(propInfo =>
            propInfo.OnHaveAttribute<TAttribute>(attribute =>
            action(obj.GetPropertyValueByInfo<TProperty>(propInfo)!, attribute)));
        }

        public static PropertyInfo GetPropertyInfoByName(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)!;
        }

        public static T? GetPropertyValueByInfo<T>(this object obj, PropertyInfo propInfo)
            where T : class
        {
            return propInfo.GetValue(obj, null) as T;
        }

        /// <summary>
        /// 使用属性信息获取属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propInfo">属性信息</param>
        /// <returns>属性的值</returns>
        public static object? GetPropertyValueByInfo(this object obj, PropertyInfo propInfo)
        {
            return propInfo.GetValue(obj, null);
        }

        public static T GetPropertyValueByName<T>(this object obj, string propertyName)
        {
            return (T)(obj.GetType().GetProperty(propertyName)!.GetValue(obj)!);
        }

        public static bool Implement<TInterface>(this object obj)
        {
            return obj.GetType().Implement<TInterface>();
        }

        public static void InvokeMethodByName(this object obj, string name)
        {
            if (obj.GetType().GetMethod(name) is MethodInfo method)
            {
                method.Invoke(obj, null);
            }
        }

        public static void InvokeMethodByName(this object obj, string name, params object?[] param)
        {
            if (obj.GetType().GetMethod(name) is MethodInfo method)
            {
                method.Invoke(obj, param);
            }
        }

        public static void SetPrivateFieldValueByName(this object obj, string fieldName, object? value)
        {
            FieldInfo? fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo?.SetValue(obj, value);
        }

        /// <summary>
        /// 按属性名称设置属性的值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">新的值</param>
        public static void SetPropertyValueByName(this object obj, string propertyName, object? value)
        {
            PropertyInfo? property = obj.GetPropertyInfoByName(propertyName);
            if (property?.CanWrite == true)
            {
                property.SetValue(obj, value, null);
            }
        }
    }
}