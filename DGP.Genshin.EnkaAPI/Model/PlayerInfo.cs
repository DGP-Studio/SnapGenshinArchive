using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.EnkaAPI.Model;

/// <summary>
/// 玩家信息
/// </summary>
public class PlayerInfo
{
    /// <summary>
    /// 昵称
    /// Player Nickname
    /// </summary>
    [JsonProperty("nickname")]
    public string Nickname { get; set; } = default!;

    /// <summary>
    /// 等级
    /// </summary>
    [JsonProperty("level")]
    public int Level { get; set; }

    /// <summary>
    /// 签名
    /// Profile Signature
    /// </summary>
    [JsonProperty("signature")]
    public string Signature { get; set; } = default!;

    /// <summary>
    /// 世界等级
    /// Player World Level
    /// </summary>
    [JsonProperty("worldLevel")]
    public int WorldLevel { get; set; }

    /// <summary>
    /// 名片的Id
    /// Profile Namecard ID
    /// </summary>
    [JsonProperty("nameCardId")]
    public int NameCardId { get; set; }

    /// <summary>
    /// 完成的成就个数
    /// Number of Completed Achievements
    /// </summary>
    [JsonProperty("finishAchievementNum")]
    public int FinishAchievementNum { get; set; }

    /// <summary>
    /// 深渊层数
    /// Abyss Floor
    /// </summary>
    [JsonProperty("towerFloorIndex")]
    public int TowerFloorIndex { get; set; }

    /// <summary>
    /// 深渊间数
    /// Abyss Floor's Level
    /// </summary>
    [JsonProperty("towerLevelIndex")]
    public int TowerLevelIndex { get; set; }

    /// <summary>
    /// 展示的角色信息
    /// List of Character IDs and Levels
    /// </summary>
    [JsonProperty("showAvatarInfoList")]
    public IList<ShowAvatarInfo> ShowAvatarInfoList { get; set; } = default!;

    /// <summary>
    /// 展示的名片信息
    /// List of Namecard IDs
    /// </summary>
    [JsonProperty("showNameCardIdList")]
    public IList<int> ShowNameCardIdList { get; set; } = default!;

    /// <summary>
    /// 头像信息
    /// Character ID of Profile Picture
    /// </summary>
    [JsonProperty("profilePicture")]
    public ProfilePicture ProfilePicture { get; set; } = default!;
}
