﻿using System.Text;

namespace Snap.Data.Utility.Extension
{
    /// <summary>
    /// <see cref="StringBuilder"/> 扩展方法
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// 当条件符合时执行 <see cref="StringBuilder.Append(string?)"/>
        /// </summary>
        /// <param name="sb">字符串建造器</param>
        /// <param name="condition">条件</param>
        /// <param name="value">附加的字符串</param>
        /// <returns>同一个字符串建造器</returns>
        public static StringBuilder AppendIf(this StringBuilder sb, bool condition, string? value)
        {
            return condition ? sb.Append(value) : sb;
        }
    }
}