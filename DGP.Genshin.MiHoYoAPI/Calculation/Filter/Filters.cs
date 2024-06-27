using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Calculation.Filter
{
    public class Filters
    {
        [JsonProperty("element_attr_filter")] public Filter? ElementAttrFilter { get; set; }
        [JsonProperty("reliquary_level_filter")] public Filter? ReliquaryLevelFilter { get; set; }
        [JsonProperty("reliquary_type_filter")] public Filter? ReliquaryTypeFilter { get; set; }
        [JsonProperty("weapon_level_filter")] public Filter? WeaponLevelFilter { get; set; }
        [JsonProperty("weapon_type_filter")] public Filter? WeaponTypeFilter { get; set; }
    }
}
