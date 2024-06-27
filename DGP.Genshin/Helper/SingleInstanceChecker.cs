using DGP.Genshin.Helper.Extension;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Helper
{
    /// <summary>
    /// 单例程序检查器
    /// </summary>
    internal class SingleInstanceChecker
    {
        private readonly string uniqueEventName;
        private EventWaitHandle? eventWaitHandle;

        /// <summary>
        /// 构造新的检测器实例
        /// </summary>
        /// <param name="uniqueEventName">App的唯一名称标识符</param>
        public SingleInstanceChecker(string uniqueEventName)
        {
            this.uniqueEventName = uniqueEventName;
        }

        /// <summary>
        /// 指示是否由于单例限制而退出
        /// </summary>
        public bool IsExitDueToSingleInstanceRestriction { get; set; }

        /// <summary>
        /// 指示是否在进行验证
        /// </summary>
        public bool IsEnsureingSingleInstance { get; set; }

        /// <summary>
        /// 确保应用程序是否为第一个打开
        /// </summary>
        /// <param name="app">应用程序</param>
        /// <param name="multiInstancePresentAction">发现多实例时执行的回调，由先打开的进程执行</param>
        public void Ensure(Application app, Action multiInstancePresentAction)
        {
            // check if it is already open.
            try
            {
                IsEnsureingSingleInstance = true;
                eventWaitHandle = EventWaitHandle.OpenExisting(uniqueEventName);
                eventWaitHandle.Set();
                IsExitDueToSingleInstanceRestriction = true;

                app.Shutdown();
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, uniqueEventName);
            }
            finally
            {
                IsEnsureingSingleInstance = false;
            }

            new Task(() =>
            {
                // if this instance gets the signal
                while (eventWaitHandle.WaitOne())
                {
                    this.ExecuteOnUI(multiInstancePresentAction);
                }
            }).Start();
        }
    }
}