using Newtonsoft.Json;

namespace DGP.Genshin.EnkaAPI.Model;

/// <summary>
/// 档案头像
/// </summary>
public class ProfilePicture
{
    /// <summary>
    /// 使用的角色Id
    /// </summary>
    [JsonProperty("avatarId")]
    public int AvatarId { get; set; }
}
