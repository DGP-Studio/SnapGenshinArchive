using DGP.Genshin.EnkaAPI.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.EnkaAPI;

/// <summary>
/// Enka API 响应
/// </summary>
public class EnkaResponse
{
    /// <summary>
    /// 玩家基础信息
    /// </summary>
    [JsonProperty("playerInfo")]
    public PlayerInfo? PlayerInfo { get; set; } = default!;

    /// <summary>
    /// 展示的角色详细信息列表
    /// </summary>
    [JsonProperty("avatarInfoList")]
    public IList<AvatarInfo>? AvatarInfoList { get; set; } = default!;

    /// <summary>
    /// 刷新剩余秒数
    /// 生存时间值
    /// </summary>
    [JsonProperty("ttl")]
    public int? Ttl { get; set; }

    /// <summary>
    /// 此响应是否有效
    /// </summary>
    public bool IsValid => Ttl.HasValue;

    /// <summary>
    /// 是否包含角色详细数据
    /// </summary>
    public bool HasDetail => AvatarInfoList != null;
}