using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Core;
using DGP.Genshin.Core.LifeCycle;
using DGP.Genshin.Core.Plugins;
using Microsoft.VisualStudio.Threading;
using System;

[assembly:SnapGenshinPlugin]

namespace DGP.Genshin.DiscordPresence.Plugin;

/// <summary>
/// 插件实例实现
/// </summary>
public class DiscordPlugin : IPlugin, IAppStartUp
{
    /// <inheritdoc/>
    public string Name
    {
        get => "Discord 状态同步";
    }

    /// <inheritdoc/>
    public string Description
    {
        get => "本插件用于将原神的游玩状态同步到Discord";
    }

    /// <inheritdoc/>
    public string Author
    {
        get => "DGP Studio";
    }

    /// <inheritdoc/>
    public Version Version
    {
        get => new("1.0.0.0");
    }

    /// <inheritdoc/>
    public bool IsEnabled
    {
        get => true;
    }

    /// <inheritdoc/>
    public void Happen(IContainer container)
    {
        AppExitRecipient appExitRecipient = new();
        container.Find<IMessenger>().Register(appExitRecipient);
        new DiscordPresenceRunner().RunAsync(appExitRecipient.CancellationTokenSource.Token).Forget();
    }
}