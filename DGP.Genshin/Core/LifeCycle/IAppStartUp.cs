namespace DGP.Genshin.Core.LifeCycle
{
    /// <summary>
    /// 表示接收生命周期启动事件通知
    /// 实现类必须提示实现 <see cref="IPlugin"/> 主类
    /// </summary>
    public interface IAppStartUp
    {
        /// <summary>
        /// 主窗体尚未出现，但应用程序已经准备完成时触发
        /// </summary>
        /// <param name="container">容器</param>
        void Happen(IContainer container);
    }
}