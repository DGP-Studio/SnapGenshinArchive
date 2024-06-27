using CommunityToolkit.Mvvm.ComponentModel;
using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.Factory.Abstraction;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Net.Afdian;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 赞助者列表视图模型
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    internal class SponsorViewModel : ObservableObject, ISupportCancellation
    {
        private const string UserId = "8f9ed3e87f4911ebacb652540025c377";
        private const string Token = "Th98JamKvc5FHYyErgM4d6spAXGVwbPD";

        private List<Sponsor>? sponsors;

        /// <summary>
        /// 构造一个新的赞助者列表视图模型
        /// </summary>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        public SponsorViewModel(IAsyncRelayCommandFactory asyncRelayCommandFactory)
        {
            OpenUICommand = asyncRelayCommandFactory.Create(OpenUIAsync);
        }

        /// <inheritdoc/>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// 赞助者列表
        /// </summary>
        public List<Sponsor>? Sponsors
        {
            get => sponsors;

            set => SetProperty(ref sponsors, value);
        }

        /// <summary>
        /// 打开界面时触发的命令
        /// </summary>
        public ICommand OpenUICommand { get; }

        private async Task OpenUIAsync()
        {
            try
            {
                int nextPage = 1;
                List<Sponsor> result = new();
                Response<ListWrapper<Sponsor>>? response;
                do
                {
                    response = await new AfdianProvider(UserId, Token).QuerySponsorAsync(nextPage++, CancellationToken);
                    if (response?.Data?.List is List<Sponsor> part)
                    {
                        result.AddRange(part);
                    }
                }
                while (nextPage <= response?.Data?.TotalPage);

                Sponsors = result.Where(s => !SponsorTooEarly(s)).ToList();
            }
            catch (TaskCanceledException)
            {
                this.Log("Open UI canceled");
            }
        }

        private bool SponsorTooEarly(Sponsor s)
        {
            long limitTime = DateTimeOffset.UtcNow.AddDays(-30).ToUnixTimeSeconds();
            return s.LastPayTime < limitTime;
        }
    }
}