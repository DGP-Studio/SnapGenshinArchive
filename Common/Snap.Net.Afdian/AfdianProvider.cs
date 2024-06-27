using System.Threading;
using System.Threading.Tasks;

namespace Snap.Net.Afdian
{
    /// <summary>
    /// 爱发电 API 请求提供器
    /// https://afdian.net/dashboard/dev
    /// </summary>
    public class AfdianProvider
    {
        private const string ApiBaseUrl = @"https://afdian.net/api/open";
        public string UserId { get; set; }
        public string Token { get; set; }

        private readonly AfdianRequester requester = new();

        public AfdianProvider(string userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public async Task<string?> PingAsync(CancellationToken cancellationToken = default)
        {
            RequestData body = RequestData.Create(UserId, Token);
            return await requester.PostAsync<string>($"{ApiBaseUrl}/ping", body, cancellationToken);
        }

        public async Task<Response<ListWrapper<Order>>?> QueryOrderAsync(int pageNumber, CancellationToken cancellationToken = default)
        {
            RequestData body = RequestData.CreateWithPage(UserId, Token, pageNumber);
            return await requester.PostAsync<Response<ListWrapper<Order>>>($"{ApiBaseUrl}/query-order", body, cancellationToken);
        }

        public async Task<Response<ListWrapper<Sponsor>>?> QuerySponsorAsync(int pageNumber, CancellationToken cancellationToken = default)
        {
            RequestData body = RequestData.CreateWithPage(UserId, Token, pageNumber);
            return await requester.PostAsync<Response<ListWrapper<Sponsor>>>($"{ApiBaseUrl}/query-sponsor", body, cancellationToken);
        }
    }
}
