using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Message;
using System.Threading;

namespace DGP.Genshin.DiscordPresence.Plugin;

/// <summary>
/// App 退出事件接收器
/// </summary>
internal class AppExitRecipient : IRecipient<AppExitingMessage>
{
    /// <summary>
    /// 构造一个新的 App 退出事件接收器
    /// </summary>
    public AppExitRecipient()
    {
        CancellationTokenSource = new();
    }

    /// <summary>
    /// 取消令牌源
    /// </summary>
    public CancellationTokenSource CancellationTokenSource { get; set; }

    /// <inheritdoc/>
    public void Receive(AppExitingMessage message)
    {
        CancellationTokenSource.Cancel();
    }
}