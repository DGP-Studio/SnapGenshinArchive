using DGP.Genshin.DataModel.DailyNote;
using DGP.Genshin.Service.Abstraction.Updating;
using ModernWpf;
using Snap.Data.Json;

namespace DGP.Genshin.Service.Abstraction.Setting
{
    /// <summary>
    /// 设置
    /// </summary>
    public static class Setting2
    {
        /// <summary>
        /// App 主题色
        /// </summary>
        public static readonly SettingDefinition<ApplicationTheme?> AppTheme = new("AppTheme", null, ApplicationThemeConverter);

        /// <summary>
        /// App 版本
        /// </summary>
        public static readonly SettingDefinition<Version?> AppVersion = new("AppVersion", null, VersionConverter);

        /// <summary>
        /// 最后一次签到的时间
        /// </summary>
        public static readonly SettingDefinition<DateTime?> LastAutoSignInTime = new("LastAutoSignInTime", DateTime.Today.AddDays(-1), NullableDataTimeConverter);

        /// <summary>
        /// 是否启用自动签到
        /// </summary>
        public static readonly SettingDefinition<bool> AutoDailySignInOnLaunch = new("AutoDailySignInOnLaunch", false);

        /// <summary>
        /// 静默签到
        /// </summary>
        public static readonly SettingDefinition<bool> SignInSilently = new("SignInSilently", false);

        /// <summary>
        /// 启动器路径
        /// </summary>
        public static readonly SettingDefinition<string?> LauncherPath = new("LauncherPath", null);

        /// <summary>
        /// 启动游戏是否全屏
        /// </summary>
        public static readonly SettingDefinition<bool> IsFullScreen = new("IsFullScreenLaunch", true);

        /// <summary>
        /// 启动游戏是否无边框
        /// </summary>
        public static readonly SettingDefinition<bool> IsBorderless = new("IsBorderlessLaunch", true);

        /// <summary>
        /// 启动游戏是否解锁FPS
        /// </summary>
        public static readonly SettingDefinition<bool> UnlockFPS = new("FPSUnlockingEnabled", false);

        /// <summary>
        /// 解锁帧率的目标FPS
        /// </summary>
        public static readonly SettingDefinition<double> TargetFPS = new("FPSUnlockingTarget", 60D);

        /// <summary>
        /// 启动游戏的窗口宽度
        /// </summary>
        public static readonly SettingDefinition<long> ScreenWidth = new("LaunchScreenWidth", 1920);

        /// <summary>
        /// 启动游戏的窗口高度
        /// </summary>
        public static readonly SettingDefinition<long> ScreenHeight = new("LaunchScreenHeight", 1080);

        /// <summary>
        /// 是否跳过完整性检查
        /// </summary>
        public static readonly SettingDefinition<bool> SkipCacheCheck = new("SkipCacheCheck", false);

        /// <summary>
        /// 当前选择的更新通道
        /// </summary>
        public static readonly SettingDefinition<UpdateAPI> UpdateAPI = new("UpdateChannel", Updating.UpdateAPI.PatchAPI, UpdateAPIConverter);

        /// <summary>
        /// 祈愿记录历史页签是否展示无物品的卡池
        /// </summary>
        public static readonly SettingDefinition<bool> IsBannerWithNoItemVisible = new("IsBannerWithNoItemVisible", true);

        /// <summary>
        /// 实时便笺刷新时间（单位分钟）
        /// </summary>
        public static readonly SettingDefinition<double> ResinRefreshMinutes = new("ResinRefreshMinutes", 8D);

        /// <summary>
        /// 实时便笺通知开关
        /// </summary>
        public static readonly SettingDefinition<DailyNoteNotifyConfiguration?> DailyNoteNotifyConfiguration = new("DailyNoteNotifyConfiguration", null, ComplexConverter<DailyNoteNotifyConfiguration>);

        /// <summary>
        /// 是否启用任务栏图标
        /// </summary>
        public static readonly SettingDefinition<bool> IsTaskBarIconEnabled = new("IsTaskBarIconEnabled", true);

        /// <summary>
        /// 是否提示最小化到任务栏
        /// </summary>
        public static readonly SettingDefinition<bool> IsTaskBarIconHintDisplay = new("IsTaskBarIconHintDisplay", true);

        /// <summary>
        /// 是否在初始化后关闭主窗体
        /// </summary>
        public static readonly SettingDefinition<bool> CloseMainWindowAfterInitializaion = new("CloseMainWindowAfterInitializaion", false);

        /// <summary>
        /// 主窗体宽度
        /// </summary>
        public static readonly SettingDefinition<double> MainWindowWidth = new("MainWindowWidth", 1108D);

        /// <summary>
        /// 主窗体高度
        /// </summary>
        public static readonly SettingDefinition<double> MainWindowHeight = new("MainWindowHeight", 750D);

        /// <summary>
        /// 导航栏是否展开
        /// </summary>
        public static readonly SettingDefinition<bool> IsNavigationViewPaneOpen = new("IsNavigationViewPaneOpen", true);

        /// <summary>
        /// 背景透明度
        /// </summary>
        public static readonly SettingDefinition<double> BackgroundOpacity = new("BackgroundOpacity", 0.4);

        /// <summary>
        /// 是否启用背景自动透明度
        /// </summary>
        public static readonly SettingDefinition<bool> IsBackgroundOpacityAdaptive = new("IsBackgroundOpacityAdaptive", false);

        /// <summary>
        /// 是否启用背景模糊
        /// </summary>
        public static readonly SettingDefinition<bool> IsBackgroundBlurEnabled = new("IsBackgroundBlurEnabled", true);

        /// <summary>
        /// 非值类型转换器
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="value">待转换的值</param>
        /// <returns>转换完成的对象</returns>
        public static T? ComplexConverter<T>(object? value)
            where T : class
        {
            return value is null ? null : Json.ToObject<T>(value.ToString()!);
        }

        private static ApplicationTheme? ApplicationThemeConverter(object? obj)
        {
            return obj is null ? null : (ApplicationTheme)Enum.ToObject(typeof(ApplicationTheme), obj);
        }

        private static UpdateAPI UpdateAPIConverter(object obj)
        {
            return (UpdateAPI)Enum.ToObject(typeof(ApplicationTheme), obj);
        }

        private static Version? VersionConverter(object? obj)
        {
            return obj is string str ? Version.Parse(str) : null;
        }

        private static DateTime? NullableDataTimeConverter(object? str)
        {
            return str is not null ? DateTimeOffset.Parse((string)str).UtcDateTime : null;
        }
    }
}