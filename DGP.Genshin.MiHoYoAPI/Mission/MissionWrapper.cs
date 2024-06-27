using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Mission
{
    public class MissionWrapper
    {
        [JsonProperty("missions")] public List<Mission>? Missions { get; set; }
        [JsonProperty("more_missions")] public List<Mission>? MoreMissions { get; set; }
    }
}
