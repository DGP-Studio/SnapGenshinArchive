using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.PostModel
{
    /// <summary>
    /// 玩家记录
    /// </summary>
    public class PlayerRecord
    {
        /// <summary>
        /// 构造一个新的玩家记录
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="playerAvatars">玩家角色</param>
        /// <param name="playerSpiralAbyssesLevels">玩家深渊信息</param>
        public PlayerRecord(string uid, List<PlayerAvatar> playerAvatars, List<PlayerSpiralAbyssLevel> playerSpiralAbyssesLevels)
        {
            Uid = uid;
            PlayerAvatars = playerAvatars;
            PlayerSpiralAbyssesLevels = playerSpiralAbyssesLevels;
        }

        /// <summary>
        /// uid
        /// </summary>
        public string Uid { get; }

        /// <summary>
        /// 玩家角色
        /// </summary>
        public List<PlayerAvatar> PlayerAvatars { get; }

        /// <summary>
        /// 玩家深渊信息
        /// </summary>
        public List<PlayerSpiralAbyssLevel> PlayerSpiralAbyssesLevels { get; }

        /// <summary>
        /// 造成最多伤害
        /// </summary>
        public RankInfo? DamageMost { get; set; }

        /// <summary>
        /// 承受最多伤害
        /// </summary>
        public RankInfo? TakeDamageMost { get; set; }
    }

    /// <summary>
    /// 排行信息
    /// </summary>
    public class RankInfo
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int AvatarId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }
}