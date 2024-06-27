using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Gacha
{
    public class UIGFItem : GachaLogItem
    {
        [JsonProperty("uigf_gacha_type")] public string? UIGFGachaType { get; set; }
    }
}
