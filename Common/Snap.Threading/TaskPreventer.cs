using System.Threading;

namespace Snap.Threading
{
    /// <summary>
    /// 阻止前台的Command多次重复调用耗时任务
    /// 如此使用
    /// <code>
    /// TaskPreventer taskPreventer = new();
    /// if (taskPreventer.ShouldExecute)
    /// {
    ///     ...
    ///     taskPreventer.Release();
    /// }
    /// </code>
    /// </summary>
    public class TaskPreventer
    {
        private int shouldExecute = 1;

        /// <summary>
        /// 当前任务是否可以继续执行
        /// </summary>
        public bool ShouldExecute
        {
            get => Interlocked.Exchange(ref shouldExecute, 0) == 1;
        }

        /// <summary>
        /// 重置状态，使任务可以再次执行
        /// </summary>
        public void Release()
        {
            Interlocked.Exchange(ref shouldExecute, 1);
        }
    }
}