using System;
using System.Collections.Generic;
using System.Linq;

namespace Snap.Extenion.Enumerable
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/>扩展方法
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// 立即展开迭代器，并对其中的项进行枚举
        /// </summary>
        /// <typeparam name="T">集合内部类型</typeparam>
        /// <param name="source">待处理的集合</param>
        /// <param name="action">执行的操作</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// source.FirstOrDefault(predicate) ?? source.FirstOrDefault();
        /// </summary>
        /// <typeparam name="T">集合内部类型</typeparam>
        /// <param name="source">待处理的集合</param>
        /// <param name="predicate">谓词</param>
        /// <returns>对应的元素</returns>
        public static T? MatchedOrFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.FirstOrDefault(predicate) ?? source.FirstOrDefault();
        }

        /// <summary>
        /// 将集合内的 <see langword="null"/> 引用去除
        /// </summary>
        /// <typeparam name="T">集合内部类型</typeparam>
        /// <param name="source">待处理的集合</param>
        /// <returns>去除 <see langword="null"/> 引用 的集合</returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> source)
        {
            return source.Where(x => x is not null)!;
        }

        /// <summary>
        /// condition ? source.Where(predicate) : source;
        /// </summary>
        /// <typeparam name="T">集合内部类型</typeparam>
        /// <param name="source">待处理的集合</param>
        /// <param name="predicate">谓词</param>
        /// <param name="condition">条件</param>
        /// <returns>处理后的集合</returns>
        public static IEnumerable<T> WhereWhen<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}