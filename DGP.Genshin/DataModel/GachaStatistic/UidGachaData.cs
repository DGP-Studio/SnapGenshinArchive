using DGP.Genshin.MiHoYoAPI.Gacha;

namespace DGP.Genshin.DataModel.GachaStatistic
{
    /// <summary>
    /// Uid与祈愿记录
    /// </summary>
    public class UidGachaData
    {
        /// <summary>
        /// 构造一个新的Uid祈愿记录实例
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="data">祈愿记录数据</param>
        public UidGachaData(string uid, GachaData data)
        {
            Uid = uid;
            Data = data;
        }

        /// <summary>
        /// Uid
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 祈愿记录数据
        /// </summary>
        public GachaData Data { get; set; }
    }
}