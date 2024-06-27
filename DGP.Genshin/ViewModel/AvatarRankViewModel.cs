using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.DataModel.Showcase;
using DGP.Genshin.EnkaAPI;
using DGP.Genshin.Factory.Abstraction;
using DGP.Genshin.HutaoAPI;
using DGP.Genshin.HutaoAPI.PostModel;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Core.Mvvm;
using Snap.Data.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel;

/// <summary>
/// 角色评分视图模型
/// </summary>
[ViewModel(InjectAs.Transient)]
internal class AvatarRankViewModel : ObservableObject2, ISupportCancellation
{
    private readonly MetadataViewModel metadataViewModel;

    private Showcase? showcase;
    private string? currentUid;

    /// <summary>
    /// 构造一个新的角色评分视图模型
    /// </summary>
    /// <param name="metadataViewModel">元数据视图模型</param>
    /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
    public AvatarRankViewModel(MetadataViewModel metadataViewModel, IAsyncRelayCommandFactory asyncRelayCommandFactory)
    {
        this.metadataViewModel = metadataViewModel;
        QueryCommand = asyncRelayCommandFactory.Create<string>(UpdateShowcaseAsync);
    }

    /// <inheritdoc/>
    public CancellationToken CancellationToken { get; set; }

    /// <summary>
    /// 当前Uid
    /// </summary>
    public string? CurrentUid { get => currentUid; set => SetProperty(ref currentUid, value); }

    /// <summary>
    /// 角色展柜
    /// </summary>
    public Showcase? Showcase { get => showcase; set => SetProperty(ref showcase, value); }

    /// <summary>
    /// 查询命令
    /// </summary>
    public ICommand QueryCommand { get; }

    private async Task UpdateShowcaseAsync(string? uid)
    {
        this.Log("triggered");
        if (uid != null)
        {
            CurrentUid = uid;
            EnkaResponse? response = await Json.FromWebsiteAsync<EnkaResponse>($"https://enka.shinshin.moe/u/{uid}/__data.json");
            IEnumerable<HutaoItem>? avatars = await new PlayerRecordClient().GetAvatarMapAsync();
            Showcase = Showcase.Build(response, metadataViewModel, avatars);
        }
        else
        {
            CurrentUid = null;
            Showcase = null;
        }
    }
}