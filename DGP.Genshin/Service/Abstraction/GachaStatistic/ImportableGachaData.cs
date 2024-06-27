using DGP.Genshin.MiHoYoAPI.Gacha;

namespace DGP.Genshin.Service.Abstraction.GachaStatistic
{
    /// <summary>
    /// 表示可导入的数据实体
    /// 仅能表示一个uid的数据
    /// </summary>
    public class ImportableGachaData
    {
        /// <summary>
        /// 待导入的数据
        /// </summary>
        public GachaData? Data { get; set; }

        /// <summary>
        /// 待导入数据的Uid
        /// </summary>
        public string? Uid { get; set; }
    }
}