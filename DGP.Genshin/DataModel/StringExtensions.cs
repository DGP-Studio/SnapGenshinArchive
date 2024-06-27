using System.IO;

namespace DGP.Genshin.DataModel
{
    /// <summary>
    /// 拓展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 获取url中的文件名部分
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>短文件名</returns>
        public static string? ToShortFileName(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                if (Uri.TryCreate(source, UriKind.Absolute, out Uri? uri))
                {
                    return Path.GetFileName(uri.LocalPath);
                }
            }

            return null;
        }
    }
}