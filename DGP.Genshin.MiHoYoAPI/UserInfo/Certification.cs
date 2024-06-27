using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.UserInfo
{
    public class Certification
    {
        [JsonProperty("type")] public int Type { get; set; }
        [JsonProperty("label")] public string? Label { get; set; }
    }

    public class Certification2 : Certification
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("certification_id")] public string? CertificationId { get; set; }
    }
}
