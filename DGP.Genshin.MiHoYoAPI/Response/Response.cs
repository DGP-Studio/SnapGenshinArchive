using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Response
{
    /// <summary>
    /// 提供 <see cref="Response{T}"/> 的非泛型基类
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 0 is OK
        /// </summary>
        [JsonProperty("retcode")] public int ReturnCode { get; set; }
        [JsonProperty("message")] public string? Message { get; set; }

        public override string ToString()
        {
            return $"状态：{ReturnCode} | 信息：{Message}";
        }

        public static bool IsOk(Response? response)
        {
            return response?.ReturnCode == 0;
        }
        public static Response CreateFail(string message)
        {
            return new Response()
            {
                ReturnCode = (int)KnownReturnCode.InternalFailure,
                Message = message
            };
        }
    }

    /// <summary>
    /// Mihoyo 标准API响应
    /// </summary>
    /// <typeparam name="TData">数据类型</typeparam>
    public class Response<TData> : Response
    {
        [JsonProperty("data")] public TData? Data { get; set; }
        public static new Response<TData> CreateFail(string message)
        {
            return new Response<TData>()
            {
                ReturnCode = (int)KnownReturnCode.InternalFailure,
                Message = message
            };
        }
    }
}