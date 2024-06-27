using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Snap.Threading
{
    /// <summary>
    /// 异步集合循环
    /// </summary>
    public static class ParalelForEachExtensions
    {
        /// <summary>
        /// 异步的等待集合循环，默认处理器个数*4个并发
        /// </summary>
        /// <typeparam name="T">集合的内部类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="asyncAction">异步操作</param>
        /// <returns>任务</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> asyncAction)
        {
            await ParallelForEachAsync(source, asyncAction, Environment.ProcessorCount * 4);
        }

        /// <summary>
        /// 异步的等待集合循环，可配置并发数
        /// </summary>
        /// <typeparam name="T">集合的内部类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="asyncAction">异步操作</param>
        /// <param name="taskCount">并发的任务数量</param>
        /// <returns>任务</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> asyncAction, int taskCount)
        {
            SemaphoreSlim throttler = new(taskCount, taskCount);

            IEnumerable<Task> tasks = source.Select(async item =>
            {
                await throttler.WaitAsync();
                try
                {
                    await asyncAction(item).ConfigureAwait(false);
                }
                finally
                {
                    throttler.Release();
                }
            });
            await Task.WhenAll(tasks);
        }
    }
}