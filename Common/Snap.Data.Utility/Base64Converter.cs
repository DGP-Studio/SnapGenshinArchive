using System;
using System.Text;

namespace Snap.Data.Utility
{
    /// <summary>
    /// Base64 字符串转换器
    /// </summary>
    public abstract class Base64Converter
    {
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="encoding">编码类型</param>
        /// <param name="input">输入</param>
        /// <returns>输出</returns>
        public static string Base64Decode(Encoding encoding, string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="encoding">编码类型</param>
        /// <param name="base64">输入</param>
        /// <returns>输出</returns>
        public static string Base64Encode(Encoding encoding, string base64)
        {
            byte[] bytes = encoding.GetBytes(base64);
            return Convert.ToBase64String(bytes);
        }
    }
}