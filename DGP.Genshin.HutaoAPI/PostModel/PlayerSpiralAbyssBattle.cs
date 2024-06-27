using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.PostModel
{
    /// <summary>
    /// 玩家深渊某间的战斗信息
    /// </summary>
    public class PlayerSpiralAbyssBattle
    {
        /// <summary>
        /// 构造一个新的战斗信息
        /// </summary>
        /// <param name="battleIndex">战斗上下半间 0,1</param>
        /// <param name="avatarIds">角色Id列表</param>
        public PlayerSpiralAbyssBattle(int battleIndex, List<int> avatarIds)
        {
            BattleIndex = battleIndex;
            AvatarIds = avatarIds;
        }

        /// <summary>
        /// 战斗上下半间 0,1
        /// </summary>
        public int BattleIndex { get; }

        /// <summary>
        /// 角色Id列表
        /// </summary>
        public List<int> AvatarIds { get; }
    }
}