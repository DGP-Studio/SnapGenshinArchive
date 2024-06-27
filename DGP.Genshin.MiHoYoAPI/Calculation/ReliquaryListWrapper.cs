using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    public class ReliquaryListWrapper
    {
        [JsonProperty("reliquary_list")] public List<Reliquary>? ReliquaryList { get; set; }
    }
}
