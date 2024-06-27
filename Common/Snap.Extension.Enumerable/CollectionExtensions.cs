using System.Collections.Generic;

namespace Snap.Extenion.Enumerable
{
    /// <summary>
    /// <see cref="ICollection{T}"/>扩展方法
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 检查集合是否为空
        /// </summary>
        /// <typeparam name="T">集合内部的类型</typeparam>
        /// <param name="collection">集合</param>
        /// <returns>集合是否为空</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T>? collection)
        {
            return collection is null || collection?.Count == 0;
        }
    }
}