using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.Achievement
{
    /// <summary>
    /// 成就初始化用Id与时间
    /// </summary>
    public class IdTime
    {
        /// <summary>
        /// 构造一个新的 成就初始化用Id与时间
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="time">时间</param>
        [JsonConstructor]
        public IdTime(int id, DateTime time)
        {
            Id = id;
            Time = time;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Time { get; set; }
    }
}