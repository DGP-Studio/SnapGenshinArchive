using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.Achievement.CocoGoat
{
    /// <summary>
    /// 椰羊用户数据格式
    /// </summary>
    public class CocoGoatUserData
    {
        /// <summary>
        /// 用户数据值
        /// </summary>
        [JsonProperty("value")]
        public CocoGoatUserDataValue? Value { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }
    }
}