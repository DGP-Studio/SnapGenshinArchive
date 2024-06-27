using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Core.Notification;
using DGP.Genshin.DataModel.Cookie;
using DGP.Genshin.DataModel.DailyNote;
using DGP.Genshin.Message;
using DGP.Genshin.MiHoYoAPI.GameRole;
using DGP.Genshin.MiHoYoAPI.Record.DailyNote;
using DGP.Genshin.Service.Abstraction;
using DGP.Genshin.Service.Abstraction.Setting;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.Threading;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGP.Genshin.Service
{
    /// <inheritdoc cref="IDailyNoteService"/>
    [Service(typeof(IDailyNoteService), InjectAs.Singleton)]
    internal class DailyNoteService : IDailyNoteService, IRecipient<TickScheduledMessage>, IRecipient<UserRequestRefreshMessage>
    {
        private readonly ICookieService cookieService;
        private readonly IScheduleService scheduleService;
        private readonly IMessenger messenger;
        private readonly TaskPreventer taskPreventer = new();

        /// <summary>
        /// 构造一个新的实时便笺服务
        /// </summary>
        /// <param name="cookieService">cookie服务</param>
        /// <param name="scheduleService">定时任务</param>
        /// <param name="messenger">消息器</param>
        public DailyNoteService(ICookieService cookieService, IScheduleService scheduleService, IMessenger messenger)
        {
            this.messenger = messenger;
            this.cookieService = cookieService;
            this.scheduleService = scheduleService;
        }

        private Dictionary<CookieUserGameRole, DailyNote?> DailyNotes { get; set; } = new();

        private Dictionary<UserGameRole, bool> ContinueNotifyResin { get; } = new();

        private Dictionary<UserGameRole, bool> ContinueNotifyHomeCoin { get; } = new();

        private Dictionary<UserGameRole, bool> ContinueNotifyTransformer { get; } = new();

        private Dictionary<UserGameRole, bool> ContinueNotifyDailyTask { get; } = new();

        private Dictionary<UserGameRole, bool> ContinueNotifyExpedition { get; } = new();

        /// <inheritdoc/>
        public void Initialize()
        {
            messenger.RegisterAll(this);
            scheduleService.InitializeAsync().Forget();
            UpdateDailyNotesAsync().Forget();
        }

        /// <inheritdoc/>
        public void UnInitialize()
        {
            scheduleService.UnInitialize();
            messenger.UnregisterAll(this);
        }

        /// <inheritdoc/>
        public DailyNote? GetDailyNote(CookieUserGameRole cookieUserGameRole)
        {
            DailyNotes.TryGetValue(cookieUserGameRole, out DailyNote? dailyNote);
            return dailyNote;
        }

        /// <inheritdoc/>
        public void Receive(TickScheduledMessage message)
        {
            this.Log("scheduled tick received");
            UpdateDailyNotesAsync().Forget();
        }

        /// <inheritdoc/>
        public void Receive(UserRequestRefreshMessage message)
        {
            this.Log("user requested a refresh");
            UpdateDailyNotesAsync().Forget();
        }

        private async Task UpdateDailyNotesAsync()
        {
            if (taskPreventer.ShouldExecute)
            {
                Dictionary<CookieUserGameRole, DailyNote?> dailyNotes = new();
                using (await cookieService.CookiesLock.ReadLockAsync())
                {
                    foreach (string cookie in cookieService.Cookies.ToList())
                    {
                        List<UserGameRole> userGameRoles = await new UserGameRoleProvider(cookie).GetUserGameRolesAsync();
                        DailyNoteProvider dailyNoteProvider = new(cookie);
                        foreach (UserGameRole userGameRole in userGameRoles)
                        {
                            DailyNote? dailyNote = await dailyNoteProvider.GetDailyNoteAsync(userGameRole);
                            dailyNotes[new(cookie, userGameRole)] = dailyNote;

                            TrySendDailyNoteNotification(
                                userGameRole,
                                dailyNote,
                                ContinueNotifyResin,
                                EvaluateResin,
                                dailyNote => $"当前原粹树脂：{dailyNote.CurrentResin}");
                            TrySendDailyNoteNotification(
                                userGameRole,
                                dailyNote,
                                ContinueNotifyHomeCoin,
                                EvaluateHomeCoin,
                                dailyNote => $"当前洞天宝钱：{dailyNote.CurrentHomeCoin}");
                            TrySendDailyNoteNotification(
                                userGameRole,
                                dailyNote,
                                ContinueNotifyTransformer,
                                EvaluateTransformer,
                                dailyNote => $"参量质变仪已可使用");
                            TrySendDailyNoteNotification(
                                userGameRole,
                                dailyNote,
                                ContinueNotifyDailyTask,
                                EvaluateDailyTask,
                                dailyNote => dailyNote.ExtraTaskRewardDescription);
                            TrySendDailyNoteNotification(
                                userGameRole,
                                dailyNote,
                                ContinueNotifyExpedition,
                                EvaluateExpedition,
                                dailyNote => "探索派遣已完成");
                        }
                    }
                }

                DailyNotes = dailyNotes;
                messenger.Send(new DailyNotesRefreshedMessage());
                taskPreventer.Release();
            }
        }

        private void TrySendDailyNoteNotification(
            UserGameRole userGameRole,
            DailyNote? dailyNote,
            Dictionary<UserGameRole, bool> notifyFlags,
            Func<DailyNoteNotifyConfiguration, DailyNote, bool> sendConditionEvaluator,
            Func<DailyNote, string> appendTextFunc)
        {
            if (dailyNote is not null)
            {
                DailyNoteNotifyConfiguration notify = Setting2.DailyNoteNotifyConfiguration.GetNonValueType(() => new());

                ToastContentBuilder builder = new ToastContentBuilder()
                    .AddHeader("DAILYNOTE", "实时便笺提醒", "DAILYNOTE")
                    .AddAttributionText(userGameRole.ToString());

                if (notify.KeepNotificationFront)
                {
                    builder.SetToastScenario(ToastScenario.Reminder);
                }

                // prepare notify flag
                if (!notifyFlags.ContainsKey(userGameRole))
                {
                    notifyFlags[userGameRole] = true;
                }

                bool shouldSend = sendConditionEvaluator.Invoke(notify, dailyNote);
                if (shouldSend)
                {
                    builder.AddText(appendTextFunc.Invoke(dailyNote), hintMaxLines: 1);
                }
                else
                {
                    // condition not meet ,reset it.
                    notifyFlags[userGameRole] = true;
                }

                AddToastButtons(builder);

                if (notifyFlags[userGameRole] && shouldSend)
                {
                    builder.SafeShow();

                    // once it send out, flag this role
                    notifyFlags[userGameRole] = false;
                }
            }
        }

        private void AddToastButtons(ToastContentBuilder builder)
        {
            builder
                .AddButton(new ToastButton()
                    .SetContent("开始游戏")
                    .AddArgument("launch", "game")
                    .SetBackgroundActivation())
                .AddButton(new ToastButton()
                    .SetContent("打开启动器")
                    .AddArgument("launch", "launcher")
                    .SetBackgroundActivation())
                .AddButton(new ToastButtonDismiss("我知道了"));
        }

        private bool EvaluateResin(DailyNoteNotifyConfiguration notify, DailyNote note)
        {
            return (notify.NotifyOnResinReach155 && note.CurrentResin >= 155)
                || (notify.NotifyOnResinReach120 && note.CurrentResin >= 120)
                || (notify.NotifyOnResinReach80 && note.CurrentResin >= 80)
                || (notify.NotifyOnResinReach40 && note.CurrentResin >= 40)
                || (notify.NotifyOnResinReach20 && note.CurrentResin >= 20);
        }

        private bool EvaluateHomeCoin(DailyNoteNotifyConfiguration notify, DailyNote note)
        {
            return notify.NotifyOnHomeCoinReach80Percent && (note.CurrentHomeCoin >= (note.MaxHomeCoin * 0.8));
        }

        private bool EvaluateTransformer(DailyNoteNotifyConfiguration notify, DailyNote note)
        {
            if (note.Transformer?.RecoveryTime is null)
            {
                return false;
            }

            return notify.NotifyOnTransformerReady && (note.Transformer.RecoveryTime.Reached);
        }

        private bool EvaluateDailyTask(DailyNoteNotifyConfiguration notify, DailyNote note)
        {
            return notify.NotifyOnDailyTasksIncomplete && (!note.IsExtraTaskRewardReceived);
        }

        private bool EvaluateExpedition(DailyNoteNotifyConfiguration notify, DailyNote note)
        {
            return notify.NotifyOnExpeditionsComplete && (note.Expeditions?.All(exp => exp.Status == "Finished") == true);
        }
    }
}