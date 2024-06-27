using Snap.Data.Primitive;
using Snap.Extenion.Enumerable;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.Showcase;

/// <summary>
/// 角色选项转换器
/// </summary>
public static class AvatarStatConverter
{
    private static readonly Dictionary<string, string> ActualNameOf = new()
    {
        ["2000"] = "生命值",
        ["2001"] = "攻击力",
        ["2002"] = "防御力",
        ["20"] = "暴击率",
        ["22"] = "暴击伤害",
        ["23"] = "元素充能效率",
        ["26"] = "治疗加成",
        ["27"] = "受治疗加成",
        ["28"] = "元素精通",
        ["29"] = "物理抗性提升",
        ["30"] = "物理伤害加成",

        ["40"] = "火元素伤害加成",
        ["41"] = "雷元素伤害加成",
        ["42"] = "水元素伤害加成",
        ["43"] = "草元素伤害加成",
        ["44"] = "风元素伤害加成",
        ["45"] = "岩元素伤害加成",
        ["46"] = "冰元素伤害加成",

        ["50"] = "火元素抗性提升",
        ["51"] = "雷元素抗性提升",
        ["52"] = "水元素抗性提升",
        ["53"] = "草元素抗性提升",
        ["54"] = "风元素抗性提升",
        ["55"] = "岩元素抗性提升",
        ["56"] = "冰元素抗性提升",

        ["81"] = "护盾强效",
    };

    private static readonly List<int> IgnorableProperties = new()
    {
        21, 25, 26, 27, 28, 29,
        30, 31, 32,
        40, 41, 42, 43, 44, 45, 46, // 增伤
        50, 51, 52, 53, 54, 55, 56, // 抗性
        80, 81,
    };

    /// <summary>
    /// 转换
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static IList<NamedValue<string>> Convert(IDictionary<string, double> map)
    {
        List<NamedValue<string>> results = new()
        {
            new(ActualNameOf["2000"], Math.Round(map["2000"]).ToString()),
            new(ActualNameOf["2001"], Math.Round(map["2001"]).ToString()),
            new(ActualNameOf["2002"], Math.Round(map["2002"]).ToString()),
        };

        foreach ((string name, double value) in map)
        {
            int index = int.Parse(name);
            bool ignorable = IgnorableProperties.Contains(index);
            if (ignorable && value == 0D)
            {
                continue;
            }

            // not ignored
            // TODO lazy...
            NamedValue<string>? result = index switch
            {
                >=1 and <=9 => null, // 直接忽略基础数值
                >= 20 and <= 23 => new(ActualNameOf[name], $"{value:P1}"),
                24 => null,
                26 or 27 or 29 or 30 => new(ActualNameOf[name], $"{value:P1}"),
                28 => new(ActualNameOf[name], $"{Math.Round(value)}"),
                >= 40 and <= 46 => new(ActualNameOf[name], $"{value:P1}"),
                >= 50 and <= 56 => new(ActualNameOf[name], $"{value:P1}"),
                >= 60 and <= 67 => null,
                >= 70 and <= 76 => null,
                80 => null,
                81 => new(ActualNameOf[name], $"{value:P1}"),
                >= 1000 and <= 2003 => null,
                >= 3000 => null,
                _ => new(name, value.ToString()),
            };

            results.AddIfNotNull(result);
        }

        return results;
    }

    public static double CirtScore(IDictionary<string, double> map)
    {
        double critRate = map["20"];
        double critDMG = map["22"];
        return 100 * ((2 * critRate) + critDMG);
    }
}