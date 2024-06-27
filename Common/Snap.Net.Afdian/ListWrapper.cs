using Newtonsoft.Json;
using System.Collections.Generic;

namespace Snap.Net.Afdian
{
    public class ListWrapper<T>
    {
        [JsonProperty("list")] public List<T>? List { get; set; }
        [JsonProperty("total_count")] public int TotalCount { get; set; }
        [JsonProperty("total_page")] public int TotalPage { get; set; }
    }
}
