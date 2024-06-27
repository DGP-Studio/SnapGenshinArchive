using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Calculation.Filter
{
    public class AllAvatarIdFilter : AvatarIdFilter
    {
        [JsonProperty("is_all")] public bool IsAll { get; set; } = true;
    }
}
