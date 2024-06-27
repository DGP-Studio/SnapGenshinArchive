using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.PostModel
{
    /// <summary>
    /// 玩家角色
    /// </summary>
    public class PlayerAvatar
    {
        /// <summary>
        /// 构造一个新的玩家角色
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <param name="level">角色等级</param>
        /// <param name="activedConstellationNum">命座</param>
        /// <param name="weapon">武器</param>
        /// <param name="reliquarySets">圣遗物套装</param>
        public PlayerAvatar(int id, int level, int activedConstellationNum, AvatarWeapon weapon, List<AvatarReliquarySet> reliquarySets)
        {
            Id = id;
            Level = level;
            ActivedConstellationNum = activedConstellationNum;
            Weapon = weapon;
            ReliquarySets = reliquarySets;
        }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// 命座
        /// </summary>
        public int ActivedConstellationNum { get; }

        /// <summary>
        /// 武器
        /// </summary>
        public AvatarWeapon Weapon { get; }

        /// <summary>
        /// 圣遗物套装
        /// </summary>
        public List<AvatarReliquarySet> ReliquarySets { get; }
    }
}