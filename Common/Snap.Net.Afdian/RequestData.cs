using Newtonsoft.Json;
using Snap.Data.Json;
using Snap.Data.Utility;
using System;

namespace Snap.Net.Afdian
{
    public class RequestData
    {
        [JsonProperty("user_id")] public string? UserId { get; set; }
        [JsonProperty("params")] public string? Params { get; set; }
        [JsonProperty("ts")] public long Ts { get; set; }
        [JsonProperty("sign")] public string? Sign { get; set; }

        public static RequestData CreateWithPage(string userId, string token, int pageNumber)
        {
            string page = Json.Stringify(new { page = pageNumber }, false);
            return new RequestData
            {
                UserId = userId,
                Ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Params = page,
                Sign = SignMd5(userId, token, page)
            };
        }

        public static RequestData Create(string userId, string token)
        {
            return new RequestData
            {
                UserId = userId,
                Ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Sign = SignMd5(userId, token, string.Empty)
            };
        }

        private static string SignMd5(string userId, string token, string page)
        {
            long ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return Md5Converter.GetComputedMd5($"{token}params{page}ts{ts}user_id{userId}");
        }
    }
}
