using System;

namespace Snap.Data.Utility
{
    /// <summary>
    /// 封装了文件资源管理器的启动操作
    /// </summary>
    public static class FileExplorer
    {
        /// <summary>
        /// 打开文件资源管理器
        /// </summary>
        /// <param name="folder">文件夹</param>
        /// <param name="failAction">失败时执行的操作</param>
        public static void Open(string folder, Action<Exception>? failAction = null)
        {
            try
            {
                ProcessHelper.Start("explorer.exe", folder);
            }
            catch (Exception ex)
            {
                failAction?.Invoke(ex);
            }
        }
    }
}