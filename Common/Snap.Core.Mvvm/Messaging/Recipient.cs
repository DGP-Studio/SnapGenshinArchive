using CommunityToolkit.Mvvm.Messaging;

namespace Snap.Core.Mvvm.Messaging
{
    /// <summary>
    /// 消息接收器
    /// </summary>
    /// <typeparam name="TMessage">消息的类型</typeparam>
    public class Recipient<TMessage> : IRecipient<TMessage>
        where TMessage : class
    {
        private readonly IMessenger messenger;
        private readonly SemaphoreSlim messageBlocker = new(1, 1);

        /// <summary>
        /// 构造一个新的消息接收器
        /// </summary>
        /// <param name="messenger">消息器</param>
        public Recipient(IMessenger messenger)
        {
            this.messenger = messenger;
            messenger.Register(this);
            messageBlocker.Wait();
        }

        /// <summary>
        /// 析构器
        /// </summary>
        ~Recipient()
        {
            messenger.Unregister<TMessage>(this);
        }

        /// <inheritdoc/>
        public void Receive(TMessage message)
        {
            // 释放信号量 使等待方进入
            messageBlocker.Release();
        }

        /// <summary>
        /// 等待一个消息
        /// </summary>
        /// <returns>任务</returns>
        public async Task<bool> WaitOneAsync()
        {
            return await messageBlocker.WaitAsync(TimeSpan.FromSeconds(5));
        }
    }
}