using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Record.Card
{
    public class Card
    {
        [JsonProperty("has_role")] public bool HasRole { get; set; }
        [JsonProperty("game_id")] public int GameId { get; set; }
        [JsonProperty("game_role_id")] public string? GameRoleId { get; set; }
        [JsonProperty("nickname")] public string? NickName { get; set; }
        [JsonProperty("region")] public string? Region { get; set; }
        [JsonProperty("level")] public int Level { get; set; }
        [JsonProperty("background_image")] public string? BackgroundImage { get; set; }
        [JsonProperty("is_public")] public bool IsPublic { get; set; }
        [JsonProperty("data")] public List<CardData>? Data { get; set; }
        [JsonProperty("region_name")] public string? RegionName { get; set; }
        [JsonProperty("url")] public string? Url { get; set; }
        [JsonProperty("data_switches")] public List<DataSwitch>? DataSwitches { get; set; }
        [JsonProperty("h5_data_switches")] public List<DataSwitch>? H5DataSwitches { get; set; }
    }
}
