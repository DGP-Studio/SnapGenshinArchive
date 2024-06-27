using Newtonsoft.Json;
using System.Collections.Generic;

namespace Snap.Net.Afdian
{
    public class SponsorPlan
    {
        [JsonProperty("plan_id")] public string? PlanId { get; set; }
        [JsonProperty("rank")] public int Rank { get; set; }
        [JsonProperty("user_id")] public string? UserId { get; set; }
        [JsonProperty("status")] public int Status { get; set; }
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("pic")] public string? Pic { get; set; }
        [JsonProperty("desc")] public string? Desc { get; set; }
        [JsonProperty("price")] public string? Price { get; set; }
        [JsonProperty("update_time")] public int UpdateTime { get; set; }
        [JsonProperty("pay_month")] public int PayMonth { get; set; }
        [JsonProperty("show_price")] public string? ShowPrice { get; set; }
        [JsonProperty("independent")] public int Independent { get; set; }
        [JsonProperty("permanent")] public int Permanent { get; set; }
        [JsonProperty("can_buy_hide")] public int CanBuyHide { get; set; }
        [JsonProperty("need_address")] public int NeedAddress { get; set; }
        [JsonProperty("product_type")] public int ProductType { get; set; }
        [JsonProperty("sale_limit_count")] public int SaleLimitCount { get; set; }
        [JsonProperty("need_invite_code")] public bool NeedInviteCode { get; set; }
        [JsonProperty("expire_time")] public int ExpireTime { get; set; }
        [JsonProperty("sku_processed")] public List<object>? SkuProcessed { get; set; }
        [JsonProperty("rankType")] public int RankType { get; set; }
    }
}
