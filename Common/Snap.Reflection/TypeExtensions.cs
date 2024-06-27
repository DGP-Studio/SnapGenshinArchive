using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Snap.Reflection
{
    /// <summary>
    /// 类型扩展
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 在指定的成员中尝试获取标记的特性
        /// </summary>
        /// <typeparam name="TAttribute">特性的类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="attribute">获取的特性</param>
        /// <returns>是否获取成功</returns>
        public static bool TryGetAttribute<TAttribute>(this Type type, [NotNullWhen(true)] out TAttribute? attribute)
            where TAttribute : Attribute
        {
            attribute = type.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }

        /// <summary>
        /// 检测类型是否实现接口
        /// </summary>
        /// <typeparam name="TInterface">被实现的类型</typeparam>
        /// <param name="type">被检测的类型/param>
        /// <returns>是否实现接口</returns>
        [SuppressMessage("", "SA1615")]
        public static bool Implement<TInterface>(this Type type)
        {
            return type.IsAssignableTo(typeof(TInterface));
        }
    }
}