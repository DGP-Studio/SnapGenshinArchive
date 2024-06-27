using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Message;
using DGP.Genshin.Service.Abstraction;
using DGP.Genshin.Service.Abstraction.Setting;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Service
{
    /// <inheritdoc cref="IScheduleService"/>
    [Service(typeof(IScheduleService), InjectAs.Singleton)]
    internal class ScheduleService : IScheduleService, IRecipient<AppExitingMessage>
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly IMessenger messenger;

        private DateTime lastScheduledTime = DateTime.UtcNow.AddHours(8);

        /// <summary>
        /// 构造一个新的计划服务
        /// </summary>
        /// <param name="messenger">消息器</param>
        public ScheduleService(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            messenger.RegisterAll(this);
            try
            {
                await Task.Run(
                    async () =>
                    {
                        while (true)
                        {
                            double minutes = Setting2.ResinRefreshMinutes;
                            await Task.Delay(TimeSpan.FromMinutes(minutes), cancellationTokenSource.Token);

                            // await Task.Delay(10000, cancellationTokenSource.Token);
                            this.Log("Tick scheduled");
                            messenger.Send(new TickScheduledMessage());
                            DateTime current = DateTime.UtcNow.AddHours(8);
                            if (current.Date > lastScheduledTime.Date)
                            {
                                this.Log("Date changed");
                                messenger.Send(new DayChangedMessage());
                            }

                            lastScheduledTime = current;
                        }
                    },
                    cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
            }
        }

        /// <inheritdoc/>
        public void UnInitialize()
        {
            cancellationTokenSource.Cancel();
            messenger.UnregisterAll(this);
        }

        /// <inheritdoc/>
        public void Receive(AppExitingMessage message)
        {
            UnInitialize();
        }
    }
}