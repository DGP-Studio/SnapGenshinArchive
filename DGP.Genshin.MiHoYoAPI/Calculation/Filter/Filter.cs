using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Calculation.Filter
{
    public class Filter
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("name")] public string? Name { get; set; }
    }
}
