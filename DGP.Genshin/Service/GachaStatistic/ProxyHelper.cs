using System.Net;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;
/// <summary>
/// 代理帮助类
/// </summary>
internal class ProxyHelper : IDisposable
{
    private readonly ProxyServer proxyServer;
    private readonly TaskCompletionSource probingUrl = new();

    private string? targetUrl;

    /// <summary>
    /// 构造一个新的代理帮助类
    /// </summary>
    public ProxyHelper()
    {
        proxyServer = new ProxyServer();
        proxyServer.BeforeRequest += OnEventAsync;

        ExplicitProxyEndPoint endPoint = new(IPAddress.Any, 18371);
        proxyServer.AddEndPoint(endPoint);
        proxyServer.Start();

        // proxyServer.SetAsSystemProxy(endPoint, ProxyProtocolType.AllHttp);
        proxyServer.SetAsSystemHttpProxy(endPoint);
        proxyServer.SetAsSystemHttpsProxy(endPoint);
    }

    /// <summary>
    /// 获取Url
    /// </summary>
    /// <returns>url</returns>
    public async Task<string> GetTargetUrlAsync()
    {
        await probingUrl.Task;
        return targetUrl ?? string.Empty;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        proxyServer.Stop();
        proxyServer.RestoreOriginalProxySettings();
        proxyServer.Dispose();
    }

    private Task OnEventAsync(object sender, SessionEventArgs e)
    {
        Titanium.Web.Proxy.Http.Request request = e.HttpClient.Request;
        if ((request.Host == "webstatic.mihoyo.com" && request.RequestUri.AbsolutePath == "/hk4e/event/e20190909gacha-v2/index.html")
            || request.RequestUriString.Contains("https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog"))
        {
            targetUrl = request.RequestUriString;
            probingUrl.TrySetResult();
        }

        return Task.CompletedTask;
    }
}
