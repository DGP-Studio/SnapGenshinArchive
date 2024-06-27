namespace DGP.Genshin.Message.Internal
{
    /// <summary>
    /// 后初始化完成消息
    /// </summary>
    internal class PostInitializationCompletedMessage : TypedMessage<MainWindow>
    {
        /// <summary>
        /// 构造一个新的后初始化完成消息
        /// </summary>
        /// <param name="mainWindow">主窗体</param>
        public PostInitializationCompletedMessage(MainWindow mainWindow)
            : base(mainWindow)
        {
        }
    }
}