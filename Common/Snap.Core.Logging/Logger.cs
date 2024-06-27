using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Snap.Core.Logging
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Logger
    {
        private static volatile Logger? instance;

        [SuppressMessage("", "IDE0044")]
        private static object locker = new();

        private string? lastCallerFilePath;
        private string? lastCallerMemberName;

        private Logger()
        {
        }

        /// <summary>
        /// 获取日志记录器的唯一实例
        /// </summary>
        internal static Logger Instance
        {
            get
            {
                if (instance is null)
                {
                    lock (locker)
                    {
                        instance ??= new();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// 静态的记录信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="formatter">格式化器</param>
        /// <param name="callerMemberName">调用名称</param>
        /// <param name="callerLineNumber">调用行号</param>
        /// <param name="callerFilePath">调用文件</param>
        public static void LogStatic(
            object? info,
            Func<object?, string>? formatter = null,
            [CallerMemberName] string? callerMemberName = null,
            [CallerLineNumber] int? callerLineNumber = null,
            [CallerFilePath] string? callerFilePath = null)
        {
            Instance.LogInternal(info, formatter, callerMemberName, callerLineNumber, callerFilePath);
        }

        /// <summary>
        /// 核心日志方法
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="formatter">格式化器</param>
        /// <param name="callerMemberName">调用名称</param>
        /// <param name="callerLineNumber">调用行号</param>
        /// <param name="callerFilePath">调用文件</param>
        internal void LogInternal(
            object? info,
            Func<object?, string>? formatter = null,
            string? callerMemberName = null,
            int? callerLineNumber = null,
            string? callerFilePath = null)
        {
            // pre format string
            if (formatter != null)
            {
                info = formatter.Invoke(info);
            }

            // trim callerFilePath
            if (callerFilePath is not null)
            {
                int pos = callerFilePath.IndexOf("Snap.Genshin", StringComparison.Ordinal);
                callerFilePath = callerFilePath[pos..];
            }

            if (callerMemberName == lastCallerMemberName && callerFilePath == lastCallerFilePath)
            {
                Debug.WriteLine($"[Line:{callerLineNumber,6}] {info}");
            }
            else
            {
                string log = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | {callerFilePath} | {callerMemberName} |\n[Line:{callerLineNumber,6}] {info}";

                if (lastCallerFilePath != callerFilePath)
                {
                    Debug.Write('\n');
                }

                Debug.WriteLine(log);
            }

            lastCallerMemberName = callerMemberName;
            lastCallerFilePath = callerFilePath;
        }
    }
}