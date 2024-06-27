using System.Collections.Generic;

namespace Snap.Data.Primitive
{
    /// <summary>
    /// 索引列表
    /// </summary>
    /// <typeparam name="TIndex">索引类型</typeparam>
    /// <typeparam name="TListElement">列表的元素类型</typeparam>
    public class Indexed<TIndex, TListElement>
    {
        /// <summary>
        /// 构造一个新的索引列表
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="list">列表</param>
        public Indexed(TIndex index, IList<TListElement> list)
        {
            Index = index;
            List = list;
        }

        /// <summary>
        /// 索引
        /// </summary>
        public TIndex Index { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        public IList<TListElement> List { get; set; }
    }
}