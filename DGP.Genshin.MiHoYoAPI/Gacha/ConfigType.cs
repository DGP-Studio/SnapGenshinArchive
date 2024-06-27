using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Gacha
{
    /// <summary>
    /// 奖池类型信息
    /// </summary>
    public class ConfigType
    {
        /// <summary>
        /// 常驻池
        /// </summary>
        public const string PermanentWish = "200";

        /// <summary>
        /// 新手池
        /// </summary>
        public const string NoviceWish = "100";

        /// <summary>
        /// 角色1池
        /// </summary>
        public const string CharacterEventWish = "301";

        /// <summary>
        /// 角色2池
        /// </summary>
        public const string CharacterEventWish2 = "400";

        /// <summary>
        /// 武器池
        /// </summary>
        public const string WeaponEventWish = "302";

        /// <summary>
        /// 已知的卡池类型
        /// </summary>
        public static readonly Dictionary<string, string> Known = new()
        {
            { NoviceWish, "新手祈愿" },
            { PermanentWish, "常驻祈愿" },
            { CharacterEventWish, "角色活动祈愿" },
            { CharacterEventWish2, "角色活动祈愿-2" },
            { WeaponEventWish, "武器活动祈愿" },
        };

        /// <summary>
        /// 排序顺序
        /// </summary>
        public static readonly Dictionary<string, int> Order = new()
        {
            { CharacterEventWish, 4 },
            { CharacterEventWish2, 3 },
            { WeaponEventWish, 2 },
            { PermanentWish, 1 },
            { NoviceWish, 0 },
        };

        /// <summary>
        /// UIGF 映射
        /// </summary>
        public static readonly Dictionary<string, string> UIGFGachaTypeMap = new()
        {
            { NoviceWish, NoviceWish },
            { PermanentWish, PermanentWish },
            { CharacterEventWish, CharacterEventWish },
            { CharacterEventWish2, CharacterEventWish },
            { WeaponEventWish, WeaponEventWish },
        };

        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        [JsonProperty("key")]
        public string? Key { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}