using System.Linq;

namespace DGP.Genshin.Helper.Extension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class BytesExtensions
    {
        /// <summary>
        /// 将字节数组格式化输出
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="format">格式化输出字符串</param>
        /// <returns>格式化后的字节</returns>
        public static string Stringify(this byte[] bytes, string format = "X2")
        {
            return string.Concat(bytes.Select(b => b.ToString(format)));
        }
    }
}