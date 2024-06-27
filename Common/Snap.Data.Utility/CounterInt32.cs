using System;
using System.Collections.Generic;

namespace Snap.Data.Utility
{
    /// <summary>
    /// 表示一个对 <see cref="TItem"/> 类型的计数器
    /// </summary>
    /// <typeparam name="TItem">待计数的类型</typeparam>
    public class CounterInt32<TItem> : Dictionary<TItem, int>
        where TItem : notnull, IEquatable<TItem>
    {
        /// <summary>
        /// 增加计数器
        /// </summary>
        /// <param name="item">物品</param>
        public void Increase(TItem? item)
        {
            if (item != null)
            {
                if (!ContainsKey(item))
                {
                    this[item] = 0;
                }

                this[item] += 1;
            }
        }
    }
}