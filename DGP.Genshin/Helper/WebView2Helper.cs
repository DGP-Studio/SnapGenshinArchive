using Microsoft.Web.WebView2.Core;

namespace DGP.Genshin.Helper
{
    /// <summary>
    /// 检测 WebView2运行时 是否存在
    /// 不再使用注册表检查方式
    /// </summary>
    internal class WebView2Helper
    {
        private static bool hasEverDetected = false;
        private static bool isSupported = false;

        /// <summary>
        /// 检测 WebView2 是否存在
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                if (hasEverDetected)
                {
                    return isSupported;
                }
                else
                {
                    hasEverDetected = true;
                    isSupported = true;
                    try
                    {
                        string version = CoreWebView2Environment.GetAvailableBrowserVersionString();
                    }
                    catch (WebView2RuntimeNotFoundException)
                    {
                        isSupported = false;
                    }

                    return isSupported;
                }
            }
        }
    }
}