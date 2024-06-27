using System.Threading.Tasks;

namespace DGP.Genshin.Helper.Extension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ObjectExtesions
    {
        /// <summary>
        /// 异步的委托主线程执行
        /// </summary>
        /// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="func">待执行的委托</param>
        /// <returns>委托的结果</returns>
        [SuppressMessage("", "VSTHRD001")]
        public static async Task<TResult> ExecuteOnUIAsync<TResult>(this object obj, Func<Task<TResult>> func)
        {
            return await App.Current.Dispatcher.InvokeAsync(func).Task.Unwrap();
        }

        /// <summary>
        /// 委托主线程执行
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="action">待执行的委托</param>
        [SuppressMessage("", "VSTHRD001")]
        public static void ExecuteOnUI(this object obj, Action action)
        {
            App.Current.Dispatcher.Invoke(action);
        }
    }
}