using Newtonsoft.Json;

namespace Snap.Net.Afdian
{
    public class Response<T>
    {
        /// <summary>
        /// ec 为 200 时，表示请求正常，否则 异常，同时 em 会提示错误信息
        /// 400001  params incomplete
        /// 400002  time was expired
        /// 400003  params was not valid json string
        /// 400004  no valid token found
        /// 400005  sign validation failed
        /// 响应 400005 时，会 data.debug 处返回服务端对参数做拼接的结构
        /// </summary>
        [JsonProperty("ec")] public int ExceptionCode { get; set; }
        [JsonProperty("em")] public string? ExceptionMessage { get; set; }
        [JsonProperty("data")] public T? Data { get; set; }
    }
}
