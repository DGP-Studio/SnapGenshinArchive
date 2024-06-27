using Newtonsoft.Json;
using System.Collections.Generic;

namespace Snap.Net.Afdian
{
    public class Sponsor
    {
        [JsonProperty("sponsor_plans")] public List<SponsorPlan>? SponsorPlans { get; set; }
        [JsonProperty("current_plan")] public SponsorPlan? CurrentPlan { get; set; }
        [JsonProperty("all_sum_amount")] public string? AllSumAmount { get; set; }
        [JsonProperty("create_time")] public long CreateTime { get; set; }
        [JsonProperty("last_pay_time")] public long LastPayTime { get; set; }
        [JsonProperty("user")] public User? User { get; set; }
    }
}
