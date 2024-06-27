using DGP.Genshin.MiHoYoAPI.Record;
using DGP.Genshin.MiHoYoAPI.Record.Avatar;
using DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.Reccording
{
    /// <summary>
    /// 包装一次查询的数据
    /// </summary>
    public class Record
    {
        /// <summary>
        /// 构造新的<see cref="Record"/>实例
        /// </summary>
        public Record()
        {
        }

        /// <summary>
        /// 构造新的<see cref="Record"/>实例
        /// </summary>
        /// <param name="message">错误消息</param>
        public Record(string? message)
        {
            Message = message;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误消息，应该先验证 <see cref="Success"/> 的值
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Uid
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 角色基础信息
        /// </summary>
        public PlayerInfo? PlayerInfo { get; set; }

        /// <summary>
        /// 当期深渊
        /// </summary>
        public SpiralAbyss? SpiralAbyss { get; set; }

        /// <summary>
        /// 上期深渊
        /// </summary>
        public SpiralAbyss? LastSpiralAbyss { get; set; }

        /// <summary>
        /// 详细角色信息
        /// </summary>
        public List<DetailedAvatar>? DetailedAvatars { get; set; }
    }
}