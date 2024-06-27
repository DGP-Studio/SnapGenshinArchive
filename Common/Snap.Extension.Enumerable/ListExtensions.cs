using Snap.Data.Primitive;
using System.Collections.Generic;
using System.Linq;

namespace Snap.Extenion.Enumerable
{
    /// <summary>
    /// <see cref="List{T}"/>扩展方法
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// 向列表添加物品，检测是否为空
        /// </summary>
        /// <typeparam name="T">列表内部类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="item">新的项</param>
        /// <returns>是否添加</returns>
        public static bool AddIfNotNull<T>(this List<T> list, T? item)
        {
            if (item is not null)
            {
                list.Add(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 克隆列表，对其中存储的实例也进行克隆
        /// </summary>
        /// <typeparam name="T">列表内部类型</typeparam>
        /// <param name="listToClone">列表</param>
        /// <returns>克隆的列表</returns>
        public static List<T> Clone<T>(this List<T> listToClone)
            where T : ICloneable<T>
        {
            return listToClone.Select(item => item.Clone()).ToList();
        }

        /// <summary>
        /// 部分克隆列表，对其中存储的实例也进行部分克隆
        /// </summary>
        /// <typeparam name="T">列表内部类型</typeparam>
        /// <param name="listToClone">列表</param>
        /// <returns>部分克隆的列表</returns>
        public static List<T> ClonePartially<T>(this List<T> listToClone)
            where T : IPartiallyCloneable<T>
        {
            return listToClone.Select(item => item.ClonePartially()).ToList();
        }

        /// <summary>
        /// 检查列表是否存在对应的索引
        /// </summary>
        /// <typeparam name="T">列表内部类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="index">待检查的索引</param>
        /// <returns>对应的索引是否有效</returns>
        public static bool ExistsIndex<T>(this IList<T> list, int index)
        {
            return index > 0 && index < list.Count;
        }
    }
}