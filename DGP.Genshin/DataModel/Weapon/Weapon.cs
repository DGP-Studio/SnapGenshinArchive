using Snap.Data.Primitive;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.Weapon
{
    /// <summary>
    /// 武器
    /// </summary>
    public class Weapon : Primitive
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 攻击力
        /// </summary>
        public string? ATK { get; set; }

        /// <summary>
        /// 副属性
        /// </summary>
        public string? SubStat { get; set; }

        /// <summary>
        /// 副属性的值
        /// </summary>
        public string? SubStatValue { get; set; }

        /// <summary>
        /// 被动
        /// </summary>
        public string? Passive { get; set; }

        /// <summary>
        /// 被动名称
        /// </summary>
        public PassiveDescription? PassiveDescription { get; set; }

        /// <summary>
        /// 突破材料
        /// </summary>
        public Material.Weapon? Ascension { get; set; }

        /// <summary>
        /// 精英怪物材料
        /// </summary>
        public Material.Material? Elite { get; set; }

        /// <summary>
        /// 普通怪物材料
        /// </summary>
        public Material.Material? Monster { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 故事
        /// </summary>
        public string? Story { get; set; }

        /// <summary>
        /// 武器详细属性
        /// </summary>
        public List<NamedValue<WeaponStatValues>>? WeaponStat { get; set; }

        /// <inheritdoc/>
        public override string? GetBadge()
        {
            return Type;
        }
    }
}