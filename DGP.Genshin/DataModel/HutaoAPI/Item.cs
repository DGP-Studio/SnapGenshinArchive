using DGP.Genshin.HutaoAPI.PostModel;

namespace DGP.Genshin.DataModel.HutaoAPI
{
    /// <summary>
    /// 胡桃API数据元件：
    /// Id Name Icon Value
    /// </summary>
    /// <typeparam name="TValue">值的类型</typeparam>
    public class Item<TValue>
    {
        /// <summary>
        /// 构造一个新的项
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">名称</param>
        /// <param name="icon">图标</param>
        /// <param name="value">值</param>
        public Item(int id, string? name, string? icon, TValue value)
        {
            Id = id;
            Name = name;
            Icon = icon;
            Value = value;
        }

        /// <summary>
        /// 构造一个新的项
        /// </summary>
        /// <param name="hutaoItem">胡桃物品</param>
        /// <param name="value">值</param>
        public Item(HutaoItem hutaoItem, TValue value)
        {
            Id = hutaoItem.Id;
            Name = hutaoItem.Name;
            Icon = hutaoItem.Url;
            Value = value;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; init; }

        /// <summary>
        /// 值
        /// </summary>
        public TValue Value { get; init; }
    }
}