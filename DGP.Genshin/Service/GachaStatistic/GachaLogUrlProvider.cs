using DGP.Genshin.Control.GenshinElement.GachaStatistic;
using DGP.Genshin.Service.Abstraction.GachaStatistic;
using ModernWpf.Controls;
using Snap.Data.Primitive;
using System.IO;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.GachaStatistic
{
    /// <summary>
    /// 联机抽卡Url提供器
    /// </summary>
    internal static class GachaLogUrlProvider
    {
        private const string GachaLogBaseUrl = "https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog";
        private const string WebStaticHost = @"https://webstatic.mihoyo.com";
        private const string Hk4eApiHost = @"https://hk4e-api.mihoyo.com";

        private static readonly string LocalPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string LogFilePath = $@"{LocalPath}Low\miHoYo\原神\output_log.txt";

        /// <summary>
        /// 根据模式获取Url
        /// </summary>
        /// <param name="mode">模式</param>
        /// <returns>文件存在返回true,若获取失败返回null</returns>
        public static async Task<Result<bool, string>> GetUrlAsync(GachaLogUrlMode mode)
        {
            switch (mode)
            {
                case GachaLogUrlMode.Proxy:
                    string url = string.Empty;
                    if (File.Exists(LogFilePath))
                    {
                        url = await GetUrlFromProxyAsync();
                    }

                    bool isOk = url != string.Empty;
                    return new(isOk, url);
                case GachaLogUrlMode.ManualInput:
                    return await GetUrlFromManualInputAsync();
                default:
                    throw Must.NeverHappen();
            }
        }

        private static async Task<string> GetUrlFromProxyAsync()
        {
            ContentDialog blockingDialog = new()
            {
                Title = "系统代理 [127.0.0.1:18371] 已开启",
                Content = "请打开游戏 [祈愿] 功能中的 [历史记录] 页面\n若已打开，请尝试翻动祈愿记录页面\n成功获取链接后对话框会即刻关闭",
            };
            using (blockingDialog.BlockInteraction())
            {
                using (ProxyHelper proxyHelper = new())
                {
                    string url = await proxyHelper.GetTargetUrlAsync();

                    url = url.Replace("#/log", string.Empty);
                    string[] splitedUrl = url.Split('?');
                    splitedUrl[0] = GachaLogBaseUrl;
                    string result = string.Join("?", splitedUrl);

                    return result;
                }
            }
        }

        /// <summary>
        /// 获取用户输入的Url
        /// </summary>
        /// <returns>用户输入的Url，若不可用则为 null</returns>
        private static async Task<Result<bool, string>> GetUrlFromManualInputAsync()
        {
            Result<bool, string> input = await new GachaLogUrlDialog().GetInputUrlAsync();
            string result = string.Empty;
            if (input.IsOk)
            {
                string url = input.Value.Trim();

                // compat with iOS url
                if (url.StartsWith(WebStaticHost) || url.StartsWith(Hk4eApiHost))
                {
                    url = url.Replace("#/log", string.Empty);
                    string[] splitedUrl = url.Split('?');
                    splitedUrl[0] = GachaLogBaseUrl;
                    result = string.Join("?", splitedUrl);
                }
            }

            return new(input.IsOk, result);
        }
    }
}