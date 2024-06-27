using Newtonsoft.Json;

namespace DGP.Genshin.EnkaAPI.Model;

/// <summary>
/// 角色列表信息
/// </summary>
public class ShowAvatarInfo
{
    /// <summary>
    /// 角色Id
    /// Character ID
    /// </summary>
    [JsonProperty("avatarId")]
    public int AvatarId { get; set; }

    /// <summary>
    /// 角色等级
    /// Character Level
    /// </summary>
    [JsonProperty("level")]
    public int Level { get; set; }

    /// <summary>
    /// 可能的皮肤Id
    /// </summary>
    [JsonProperty("costumeId")]
    public int? CostumeId { get; set; }
}
