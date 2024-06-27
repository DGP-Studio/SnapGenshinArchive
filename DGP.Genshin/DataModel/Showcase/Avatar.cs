using Snap.Data.Primitive;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.Showcase;

/// <summary>
/// 角色
/// </summary>
public class Avatar
{
    /// <summary>
    /// 基底角色信息
    /// </summary>
    public Character.Character? BaseCharacter { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 好感度
    /// </summary>
    public int FetterLevel { get; set; } = default!;

    /// <summary>
    /// 双暴评分
    /// </summary>
    public double CritScore { get; set; }

    /// <summary>
    /// 属性表
    /// </summary>
    public IList<NamedValue<string>> Stats { get; set; } = default!;
}
