using System;
using System.Reflection;

namespace Snap.Reflection
{
    /// <summary>
    /// 程序集扩展
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// 检查程序集是否标记了指定的特性
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="assembly">指定的程序集</param>
        /// <returns>是否标记了指定的特性</returns>
        public static bool HasAttribute<TAttribute>(this Assembly assembly)
            where TAttribute : Attribute
        {
            return assembly.GetCustomAttribute<TAttribute>() is not null;
        }

        /// <summary>
        /// 对程序集中的所有类型进行指定的操作
        /// </summary>
        /// <param name="assembly">指定的程序集</param>
        /// <param name="action">进行的操作</param>
        public static void ForEachType(this Assembly assembly, Action<Type> action)
        {
            foreach (Type type in assembly.GetTypes())
            {
                action.Invoke(type);
            }
        }
    }
}