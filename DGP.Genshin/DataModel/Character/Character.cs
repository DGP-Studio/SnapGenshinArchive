using DGP.Genshin.DataModel.Material;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.Character
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Character : Primitive
    {
        /// <summary>
        /// 武器图标Url
        /// </summary>
        public string? Weapon { get; set; }

        /// <summary>
        /// 元素图标Url
        /// </summary>
        public string? Element { get; set; }

        /// <summary>
        /// 名片Url
        /// </summary>
        public string? Profile { get; set; }

        /// <summary>
        /// 卡池立绘卡Url
        /// </summary>
        public string? GachaCard { get; set; }

        /// <summary>
        /// 卡池立绘Url
        /// </summary>
        public string? GachaSplash { get; set; }

        /// <summary>
        /// 城市Url
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// 天赋材料
        /// </summary>
        public Talent? Talent { get; set; }

        /// <summary>
        /// Boss突破材料
        /// </summary>
        public Material.Material? Boss { get; set; }

        /// <summary>
        /// 元素晶石
        /// </summary>
        public Material.Material? GemStone { get; set; }

        /// <summary>
        /// 本地材料
        /// </summary>
        public Material.Material? Local { get; set; }

        /// <summary>
        /// 普通怪物材料
        /// </summary>
        public Material.Material? Monster { get; set; }

        /// <summary>
        /// 周本材料
        /// </summary>
        public Material.Material? Weekly { get; set; }

        /// <summary>
        /// 角色基础数值
        /// </summary>
        public List<NameValues<CharStatValues>>? CharStat { get; set; }

        /// <summary>
        /// 命座
        /// </summary>
        public Constellation? Constellation { get; set; }

        /// <summary>
        /// 普攻
        /// </summary>
        public TableDescribedNameSource? NormalAttack { get; set; }

        /// <summary>
        /// E
        /// </summary>
        public TableDescribedNameSource? TalentE { get; set; }

        /// <summary>
        /// Q
        /// </summary>
        public TableDescribedNameSource? TalentQ { get; set; }

        /// <summary>
        /// 被动天赋
        /// </summary>
        public List<DescribedNameSource>? PassiveTalents { get; set; }

        /// <summary>
        /// 称号
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 命座名称
        /// </summary>
        public string? AstrolabeName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <inheritdoc/>
        public override string? GetBadge()
        {
            return Element;
        }
    }
}