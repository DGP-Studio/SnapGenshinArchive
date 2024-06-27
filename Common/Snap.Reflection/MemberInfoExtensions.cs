using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Snap.Reflection
{
    /// <summary>
    /// 成员信息扩展
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// 在指定的成员中尝试获取标记的特性
        /// </summary>
        /// <typeparam name="TAttribute">特性的类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="attribute">获取的特性</param>
        /// <returns>是否获取成功</returns>
        public static bool TryGetAttribute<TAttribute>(this MemberInfo member, [NotNullWhen(true)] out TAttribute? attribute)
            where TAttribute : Attribute
        {
            attribute = member.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }

        /// <summary>
        /// 当在指定的成员中标记了指定类型的特性后执行操作
        /// </summary>
        /// <typeparam name="TAttribute">特性的类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="action">进行的操作</param>
        public static void OnHaveAttribute<TAttribute>(this MemberInfo member, Action<TAttribute> action)
            where TAttribute : Attribute
        {
            if (member.TryGetAttribute(out TAttribute? attribute))
            {
                action(attribute);
            }
        }

        /// <summary>
        /// 当在指定的成员中未标记指定类型的特性后执行操作
        /// </summary>
        /// <typeparam name="TAttribute">特性的类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="action">进行的操作</param>
        public static void OnNotHaveAttribute<TAttribute>(this MemberInfo member, Action action)
            where TAttribute : Attribute
        {
            if (!member.TryGetAttribute(out TAttribute? _))
            {
                action();
            }
        }
    }
}