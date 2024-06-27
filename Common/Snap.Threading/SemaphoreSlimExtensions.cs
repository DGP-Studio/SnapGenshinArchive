using System;
using System.Threading;
using System.Threading.Tasks;

namespace Snap.Threading
{
    /// <summary>
    /// 信号量扩展方法
    /// </summary>
    public static class SemaphoreSlimExtensions
    {
        /// <summary>
        /// 异步等待进入信号量
        /// 使用 using 语句 简化 <see cref="SemaphoreSlim"/> 的使用
        /// </summary>
        /// <param name="semaphore">待进入的信号量</param>
        /// <param name="cancelToken">取消令牌</param>
        /// <returns>一个等待进入信号量的任务/returns>
        public static async Task<IDisposable> EnterAsync(this SemaphoreSlim semaphore, CancellationToken cancelToken = default)
        {
            await semaphore.WaitAsync(cancelToken).ConfigureAwait(false);
            return new SemaphoreSlimReleaser(semaphore);
        }

        /// <summary>
        /// 同步等待进入信号量
        /// 使用 using 语句 简化 <see cref="SemaphoreSlim"/> 的使用
        /// </summary>
        /// <param name="semaphore">待进入的信号量</param>
        /// <param name="cancelToken">取消令牌</param>
        /// <returns>阻塞当前线程，直到进入信号量</returns>
        public static IDisposable Enter(this SemaphoreSlim semaphore, CancellationToken cancelToken = default)
        {
            semaphore.Wait(cancelToken);
            return new SemaphoreSlimReleaser(semaphore);
        }

        /// <summary>
        /// 信号量释放器
        /// </summary>
        private class SemaphoreSlimReleaser : IDisposable
        {
            private readonly SemaphoreSlim semaphore;

            private bool isDisposed;

            public SemaphoreSlimReleaser(SemaphoreSlim semaphore)
            {
                this.semaphore = semaphore;
            }

            public void Dispose()
            {
                if (isDisposed)
                {
                    return;
                }

                semaphore.Release();
                isDisposed = true;
            }
        }
    }
}