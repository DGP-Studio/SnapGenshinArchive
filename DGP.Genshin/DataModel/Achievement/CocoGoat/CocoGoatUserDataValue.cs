using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.Achievement.CocoGoat
{
    /// <summary>
    /// 椰羊用户数据值
    /// </summary>
    public class CocoGoatUserDataValue
    {
        /// <summary>
        /// 成就
        /// </summary>
        [JsonProperty("achievements")]
        public List<CocoGoatAchievement>? Achievements { get; set; }
    }
}