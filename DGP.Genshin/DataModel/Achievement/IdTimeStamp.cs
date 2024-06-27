using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.Achievement
{
    /// <summary>
    /// 成就初始化用Id与时间戳
    /// </summary>
    public class IdTimeStamp
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonRequired]
        public long TimeStamp { get; set; }
    }
}