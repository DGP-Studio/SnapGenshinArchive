using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record.Card
{
    public class CardData
    {
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("type")] public int Type { get; set; }
        [JsonProperty("value")] public string? Value { get; set; }
    }
}
