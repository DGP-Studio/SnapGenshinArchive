using Newtonsoft.Json;
using System.Collections.Generic;

namespace Snap.Net.Afdian
{
    public class Order
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("out_trade_no")] public string? OutTradeNo { get; set; }
        /// <summary>
        /// 下单用户ID
        /// </summary>
        [JsonProperty("user_id")] public string? UserId { get; set; }
        /// <summary>
        /// 方案ID，如自选，则为空
        /// </summary>
        [JsonProperty("plan_id")] public string? PlanId { get; set; }
        /// <summary>
        /// 订单描述
        /// </summary>
        [JsonProperty("title")] public string? Title { get; set; }
        /// <summary>
        /// 赞助月份
        /// </summary>
        [JsonProperty("month")] public int Month { get; set; }
        /// <summary>
        /// 真实付款金额，如有兑换码，则为0.00
        /// </summary>
        [JsonProperty("total_amount")] public string? TotalAmount { get; set; }
        /// <summary>
        /// 显示金额，如有折扣则为折扣前金额
        /// </summary>
        [JsonProperty("show_amount")] public string? ShowAmount { get; set; }
        /// <summary>
        /// 2 为交易成功。目前仅会推送此类型
        /// </summary>
        [JsonProperty("status")] public int Status { get; set; }
        /// <summary>
        /// 订单留言
        /// </summary>
        [JsonProperty("remark")] public string? Remark { get; set; }
        /// <summary>
        /// 兑换码ID
        /// </summary>
        [JsonProperty("redeem_id")] public string? RedeemId { get; set; }
        /// <summary>
        /// 0表示常规方案 1表示售卖方案
        /// </summary>
        [JsonProperty("product_type")] public int ProductType { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        [JsonProperty("discount")] public string? Discount { get; set; }
        /// <summary>
        /// 如果为售卖类型，以数组形式表示具体型号
        /// </summary>
        [JsonProperty("sku_detail")] public List<SkuDetail>? SkuDetail { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        [JsonProperty("address_person")] public string? AddressPerson { get; set; }
        /// <summary>
        /// 收件人电话
        /// </summary>
        [JsonProperty("address_phone")] public string? AddressPhone { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        [JsonProperty("address_address")] public string? AddressAddress { get; set; }
    }
}
