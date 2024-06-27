using System;

namespace Snap.Data.Utility
{
    /// <summary>
    /// 封装了打开浏览器的方法
    /// </summary>
    public static class Browser
    {
        /// <summary>
        /// 打开浏览器
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="failAction">失败时执行的回调</param>
        public static void Open(string url, Action<Exception>? failAction = null)
        {
            try
            {
                ProcessHelper.Start(url);
            }
            catch (Exception ex)
            {
                failAction?.Invoke(ex);
            }
        }

        /// <summary>
        /// 打开浏览器
        /// </summary>
        /// <param name="urlFunc">获取链接回调</param>
        /// <param name="failAction">失败时执行的回调</param>
        public static void Open(Func<string> urlFunc, Action<Exception>? failAction = null)
        {
            try
            {
                ProcessHelper.Start(urlFunc.Invoke());
            }
            catch (Exception ex)
            {
                failAction?.Invoke(ex);
            }
        }
    }
}