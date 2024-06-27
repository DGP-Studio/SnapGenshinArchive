using System;
using System.Collections.Generic;

namespace Snap.Extenion.Enumerable
{
    /// <summary>
    /// <see cref="List{T}"/> 随机扩展
    /// </summary>
    public static class ListRandomExtensions
    {
        private static readonly Lazy<Random> LazyRandom = new(() => new());

        /// <summary>
        /// 随机获取列表中的一个项
        /// </summary>
        /// <typeparam name="T">列表中的类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>随机项</returns>
        public static T? GetRandom<T>(this List<T> list)
        {
            return list.Count > 0
                ? list[LazyRandom.Value.Next(0, list.Count)]
                : default;
        }

        /// <summary>
        /// 在列表中获取随机的不重复项目
        /// 使用默认的比较器比较与上个项目的不同
        /// </summary>
        /// <typeparam name="T">列表中的类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="lastItem">上一次的项</param>
        /// <returns>随机项</returns>
        public static T? GetRandomNoRepeat<T>(this List<T> list, T? lastItem)
        {
            if (list.Count >= 2)
            {
                T? random;

                do
                {
                    random = list.GetRandom();
                }
                while (EqualityComparer<T>.Default.Equals(lastItem, random));
                return random;
            }
            else
            {
                return list.GetRandom();
            }
        }

        /// <summary>
        /// 在列表中获取随机的不重复项目
        /// 使用评估器比较与上个项目的不同
        /// </summary>
        /// <typeparam name="T">列表中的类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="duplicationEvaluator">重复项评估器</param>
        /// <returns>随机项</returns>
        public static T? GetRandomNoRepeat<T>(this List<T> list, Func<T?, bool> duplicationEvaluator)
        {
            if (list.Count >= 2)
            {
                T? random;

                do
                {
                    random = list.GetRandom();
                }
                while (!duplicationEvaluator.Invoke(random));
                return random;
            }
            else
            {
                return list.GetRandom();
            }
        }
    }
}