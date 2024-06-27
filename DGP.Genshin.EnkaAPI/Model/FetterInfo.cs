using Newtonsoft.Json;

namespace DGP.Genshin.EnkaAPI.Model;

/// <summary>
/// 好感度信息
/// </summary>
public class FetterInfo
{
    /// <summary>
    /// 好感度等级
    /// </summary>
    [JsonProperty("expLevel")]
    public int ExpLevel { get; set; }
}
