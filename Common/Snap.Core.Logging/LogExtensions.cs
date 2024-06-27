using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Snap.Core.Logging
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public static partial class LogExtensions
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">记录日志的调用类</param>
        /// <param name="info">信息</param>
        /// <param name="formatter">格式化器</param>
        /// <param name="callerMemberName">调用名称</param>
        /// <param name="callerLineNumber">调用行号</param>
        /// <param name="callerFilePath">调用文件</param>
        public static void Log<T>(
            this T obj,
            object? info,
            Func<object?, string>? formatter = null,
            [CallerMemberName] string? callerMemberName = null,
            [CallerLineNumber] int? callerLineNumber = null,
            [CallerFilePath] string? callerFilePath = null)
        {
            Logger.Instance.LogInternal(info, formatter, callerMemberName, callerLineNumber, callerFilePath);
        }

        /// <summary>
        /// 将信息写入桌面上的文件
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="info">信息</param>
        /// <param name="fileName">文件名称</param>
        [Conditional("DEBUG")]
        public static void WriteToDesktopFile<T>(this T obj, string? info, string fileName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.WriteAllText(Path.Combine(desktopPath, fileName), info);
        }
    }
}