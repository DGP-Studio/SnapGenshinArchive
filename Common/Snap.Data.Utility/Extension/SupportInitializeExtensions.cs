using System;
using System.ComponentModel;

namespace Snap.Data.Utility.Extension
{
    /// <summary>
    /// <see cref="ISupportInitialize"/> 扩展方法
    /// </summary>
    public static class SupportInitializeExtensions
    {
        /// <summary>
        /// 开始初始化
        /// 并将支持初始化的类型转化为可处理的类型
        /// 以支持更好的语法格式
        /// </summary>
        /// <param name="supportInitialize">支持初始化的对象</param>
        /// <returns>一个可释放的对象，用于在操作结束时自动结束初始化</returns>
        public static IDisposable AsDisposableInit(this ISupportInitialize supportInitialize)
        {
            supportInitialize.BeginInit();
            return new SupportInitializeDisposable(supportInitialize);
        }

        private struct SupportInitializeDisposable : IDisposable
        {
            private readonly ISupportInitialize supportInitialize;

            public SupportInitializeDisposable(ISupportInitialize supportInitialize)
            {
                this.supportInitialize = supportInitialize;
            }

            public void Dispose()
            {
                supportInitialize.EndInit();
            }
        }
    }
}