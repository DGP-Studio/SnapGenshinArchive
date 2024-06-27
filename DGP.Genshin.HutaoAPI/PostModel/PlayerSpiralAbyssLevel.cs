using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.PostModel
{
    /// <summary>
    /// 玩家深渊战斗间信息
    /// </summary>
    public class PlayerSpiralAbyssLevel
    {
        /// <summary>
        /// 构造一个新的
        /// </summary>
        /// <param name="floorIndex">层号</param>
        /// <param name="levelIndex">间号</param>
        /// <param name="star">星数</param>
        /// <param name="battles">战斗列表</param>
        public PlayerSpiralAbyssLevel(int floorIndex, int levelIndex, int star, List<PlayerSpiralAbyssBattle> battles)
        {
            FloorIndex = floorIndex;
            LevelIndex = levelIndex;
            Star = star;
            Battles = battles;
        }

        /// <summary>
        /// 层号
        /// </summary>
        public int FloorIndex { get; }

        /// <summary>
        /// 间号
        /// </summary>
        public int LevelIndex { get; }

        /// <summary>
        /// 星数
        /// </summary>
        public int Star { get; }

        /// <summary>
        /// 战斗列表
        /// </summary>
        public List<PlayerSpiralAbyssBattle> Battles { get; }
    }
}