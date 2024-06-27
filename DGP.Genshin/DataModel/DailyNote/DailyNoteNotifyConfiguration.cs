using Snap.Data.Primitive;

namespace DGP.Genshin.DataModel.DailyNote
{
    /// <summary>
    /// 封装实时便笺提醒开关
    /// </summary>
    public class DailyNoteNotifyConfiguration : Observable
    {
        private bool notifyOnResinReach20;
        private bool notifyOnResinReach40;
        private bool notifyOnResinReach80;
        private bool notifyOnResinReach120;
        private bool notifyOnResinReach155;
        private bool notifyOnHomeCoinReach80Percent;
        private bool notifyOnDailyTasksIncomplete;
        private bool notifyOnExpeditionsComplete;
        private bool keepNotificationFront;
        private bool notifyOnTransformerReady;

        /// <summary>
        /// 保持通知在前台不收入通知中心
        /// </summary>
        public bool KeepNotificationFront
        {
            get => keepNotificationFront;

            set => Set(ref keepNotificationFront, value);
        }

        /// <summary>
        /// 当树脂达到20时提醒
        /// </summary>
        public bool NotifyOnResinReach20
        {
            get => notifyOnResinReach20;

            set => Set(ref notifyOnResinReach20, value);
        }

        /// <summary>
        /// 当树脂达到40时提醒
        /// </summary>
        public bool NotifyOnResinReach40
        {
            get => notifyOnResinReach40;

            set => Set(ref notifyOnResinReach40, value);
        }

        /// <summary>
        /// 当树脂达到80时提醒
        /// </summary>
        public bool NotifyOnResinReach80
        {
            get => notifyOnResinReach80;

            set => Set(ref notifyOnResinReach80, value);
        }

        /// <summary>
        /// 当树脂达到120时提醒
        /// </summary>
        public bool NotifyOnResinReach120
        {
            get => notifyOnResinReach120;

            set => Set(ref notifyOnResinReach120, value);
        }

        /// <summary>
        /// 当树脂达到155时提醒
        /// </summary>
        public bool NotifyOnResinReach155
        {
            get => notifyOnResinReach155;

            set => Set(ref notifyOnResinReach155, value);
        }

        /// <summary>
        /// 当洞天宝钱达到80%时提醒
        /// </summary>
        public bool NotifyOnHomeCoinReach80Percent
        {
            get => notifyOnHomeCoinReach80Percent;

            set => Set(ref notifyOnHomeCoinReach80Percent, value);
        }

        /// <summary>
        /// 当参量质变仪可用时提醒
        /// </summary>
        public bool NotifyOnTransformerReady
        {
            get => notifyOnTransformerReady;

            set => Set(ref notifyOnTransformerReady, value);
        }

        /// <summary>
        /// 当每日任务奖励未领取时提醒
        /// </summary>
        public bool NotifyOnDailyTasksIncomplete
        {
            get => notifyOnDailyTasksIncomplete;

            set => Set(ref notifyOnDailyTasksIncomplete, value);
        }

        /// <summary>
        /// 当探索派遣全部完成时提醒
        /// </summary>
        public bool NotifyOnExpeditionsComplete
        {
            get => notifyOnExpeditionsComplete;

            set => Set(ref notifyOnExpeditionsComplete, value);
        }
    }
}