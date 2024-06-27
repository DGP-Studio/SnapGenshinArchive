using Newtonsoft.Json;

namespace DGP.Genshin.HutaoAPI.PostModel
{
    /// <summary>
    /// 胡桃数据库物品
    /// </summary>
    public class HutaoItem
    {
        /// <summary>
        /// 构造一个新的
        /// </summary>
        /// <param name="id">物品Id</param>
        /// <param name="name">名称</param>
        /// <param name="url">链接</param>
        [JsonConstructor]
        public HutaoItem(int id, string? name, string? url)
        {
            Id = id;
            Name = name;
            Url = url;
        }

        /// <summary>
        /// 物品Id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// 链接
        /// </summary>
        public string? Url { get; }
    }
}