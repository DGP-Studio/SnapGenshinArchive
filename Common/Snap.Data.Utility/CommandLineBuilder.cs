﻿using System.Collections.Generic;
using System.Text;

namespace Snap.Data.Utility
{
    /// <summary>
    /// 命令行建造器
    /// </summary>
    public class CommandLineBuilder
    {
        private const char WhiteSpace = ' ';
        private readonly Dictionary<string, string?> options = new();

        /// <summary>
        /// 当符合条件时添加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="condition">条件</param>
        /// <param name="value">值</param>
        /// <returns>命令行建造器</returns>
        public CommandLineBuilder AppendIf(string name, bool condition, object? value = null)
        {
            return condition ? Append(name, value) : this;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">值</param>
        /// <returns>命令行建造器</returns>
        public CommandLineBuilder Append(string name, object? value = null)
        {
            options.Add(name, value?.ToString());
            return this;
        }

        /// <inheritdoc cref="ToString"/>
        public string Build()
        {
            return ToString();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder s = new();
            foreach ((string key, string? value) in options)
            {
                s.Append(WhiteSpace);
                s.Append(key);
                if (!string.IsNullOrEmpty(value))
                {
                    s.Append(WhiteSpace);
                    s.Append(value);
                }
            }

            return s.ToString();
        }
    }
}