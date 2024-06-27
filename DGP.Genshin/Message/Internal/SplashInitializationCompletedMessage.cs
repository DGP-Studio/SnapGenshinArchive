using DGP.Genshin.ViewModel;

namespace DGP.Genshin.Message.Internal
{
    /// <summary>
    /// 初始化校验完成消息
    /// </summary>
    internal class SplashInitializationCompletedMessage : TypedMessage<SplashViewModel>
    {
        /// <summary>
        /// 构造一个新的消息
        /// </summary>
        /// <param name="viewModel">值</param>
        public SplashInitializationCompletedMessage(SplashViewModel viewModel)
            : base(viewModel)
        {
        }
    }
}