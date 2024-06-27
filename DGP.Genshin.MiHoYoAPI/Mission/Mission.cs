using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Mission
{

    public class Mission
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("desc")] public string? Desc { get; set; }
        [JsonProperty("threshold")] public int Threshold { get; set; }
        [JsonProperty("limit")] public int Limit { get; set; }
        [JsonProperty("exp")] public int Exp { get; set; }
        [JsonProperty("point")] public int Ponit { get; set; }
        [JsonProperty("active_time")] public int ActiveTime { get; set; }
        [JsonProperty("end_time")] public int EndTime { get; set; }
        [JsonProperty("is_auto_send_award")] public bool IsAutoSendAward { get; set; }
        [JsonProperty("continuous_cycle_times")] public int ContinuousCycleTime { get; set; }
        [JsonProperty("next_points")] public int NextPoints { get; set; }
        [JsonProperty("mission_key")] public string? MissionKey { get; set; }
    }
}
