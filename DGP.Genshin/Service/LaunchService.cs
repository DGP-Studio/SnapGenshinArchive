using DGP.Genshin.Core.ImplementationSwitching;
using DGP.Genshin.Core.Notification;
using DGP.Genshin.DataModel.Launching;
using DGP.Genshin.FPSUnlocking;
using DGP.Genshin.Service.Abstraction.Launching;
using DGP.Genshin.Service.Abstraction.Setting;
using IniParser;
using IniParser.Exceptions;
using IniParser.Model;
using Microsoft.AppCenter.Crashes;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Snap.Core.Logging;
using Snap.Data.Json;
using Snap.Data.Primitive;
using Snap.Data.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DGP.Genshin.Service
{
    /// <summary>
    /// 启动服务的默认实现
    /// </summary>
    [SwitchableImplementation(typeof(ILaunchService))]
    internal class LaunchService : ILaunchService
    {
        private const string AccountsFileName = "accounts.json";
        private const string LauncherSection = "launcher";
        private const string GameName = "game_start_name";
        private const string GeneralSection = "General";
        private const string Channel = "channel";
        private const string CPS = "cps";
        private const string SubChannel = "sub_channel";
        private const string GameInstallPath = "game_install_path";
        private const string ConfigFileName = "config.ini";
        private const string LauncherExecutable = "launcher.exe";

        private IniData? launcherConfig;
        private IniData? gameConfig;
        private Unlocker? unlocker;

        /// <summary>
        /// 构造一个新的默认启动服务
        /// </summary>
        public LaunchService()
        {
            PathContext.CreateFileOrIgnore(AccountsFileName);
            string? launcherPath = Setting2.LauncherPath;
            TryLoadIniData(launcherPath);
        }

        /// <inheritdoc/>
        public IniData LauncherConfig
        {
            get => Must.NotNull(launcherConfig!);
        }

        /// <inheritdoc/>
        public IniData GameConfig
        {
            get => Must.NotNull(gameConfig!);
        }

        /// <inheritdoc/>
        public WorkWatcher GameWatcher { get; } = new();

        /// <inheritdoc/>
        [MemberNotNullWhen(true, nameof(gameConfig))]
        [MemberNotNullWhen(true, nameof(launcherConfig))]
        public bool TryLoadIniData(string? launcherPath)
        {
            if (launcherPath != null)
            {
                try
                {
                    string configPath = Path.Combine(Path.GetDirectoryName(launcherPath)!, ConfigFileName);
                    launcherConfig = GetIniData(configPath);

                    string unescapedGameFolder = GetUnescapedGameFolderFromLauncherConfig();
                    gameConfig = GetIniData(Path.Combine(unescapedGameFolder, ConfigFileName));
                }
                catch (ParsingException)
                {
                    return false;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task LaunchAsync(LaunchOption option, Action<Exception> failAction)
        {
            string? launcherPath = Setting2.LauncherPath.Get();
            if (launcherPath is not null)
            {
                string unescapedGameFolder = GetUnescapedGameFolderFromLauncherConfig();
                string gamePath = Path.Combine(unescapedGameFolder, LauncherConfig[LauncherSection][GameName]);

                try
                {
                    if (GameWatcher.IsWorking)
                    {
                        Verify.FailOperation("游戏已经启动");
                    }

                    // https://docs.unity.cn/cn/current/Manual/PlayerCommandLineArguments.html
                    string commandLine = new CommandLineBuilder()
                        .AppendIf("-popupwindow", option.IsBorderless)
                        .Append("-screen-fullscreen", option.IsFullScreen ? 1 : 0)
                        .Append("-screen-width", option.ScreenWidth)
                        .Append("-screen-height", option.ScreenHeight)
                        .Build();

                    Process? game = new()
                    {
                        StartInfo = new()
                        {
                            Arguments = commandLine,
                            FileName = gamePath,
                            UseShellExecute = true,
                            Verb = "runas",
                            WorkingDirectory = Path.GetDirectoryName(gamePath),
                        },
                    };

                    using (GameWatcher.Watch())
                    {
                        if (option.UnlockFPS)
                        {
                            unlocker = new(game, option.TargetFPS);
                            UnlockResult result = await unlocker.StartProcessAndUnlockAsync();
                            this.Log(result);
                        }
                        else
                        {
                            if (game.Start())
                            {
                                await game.WaitForExitAsync();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failAction.Invoke(ex);
                }
            }
        }

        /// <inheritdoc/>
        public void SetTargetFPSDynamically(int targetFPS)
        {
            if (unlocker is not null)
            {
                unlocker.TargetFPS = targetFPS;
            }
        }

        /// <inheritdoc/>
        public void OpenOfficialLauncher(Action<Exception>? failAction)
        {
            string? launcherPath = Setting2.LauncherPath.Get();
            try
            {
                ProcessStartInfo info = new()
                {
                    FileName = launcherPath,
                    Verb = "runas",
                    UseShellExecute = true,
                };
                Process? p = Process.Start(info);
            }
            catch (Exception ex)
            {
                failAction?.Invoke(ex);
            }
        }

        /// <inheritdoc/>
        public string? SelectLaunchDirectoryIfIncorrect(string? launcherPath)
        {
            if (!File.Exists(launcherPath) || Path.GetFileName(launcherPath) != LauncherExecutable)
            {
                OpenFileDialog openFileDialog = new()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Filter = "启动器|launcher.exe",
                    Title = "选择启动器文件",
                    CheckPathExists = true,
                    FileName = LauncherExecutable,
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string fileName = openFileDialog.FileName;
                    if (Path.GetFileName(fileName) == LauncherExecutable)
                    {
                        launcherPath = openFileDialog.FileName;
                        Setting2.LauncherPath.Set(launcherPath);
                    }
                }
            }

            return launcherPath;
        }

        /// <inheritdoc/>
        public void SaveLaunchScheme(LaunchScheme? scheme)
        {
            if (scheme is not null)
            {
                string unescapedGameFolder = GetUnescapedGameFolderFromLauncherConfig();
                string configFilePath = Path.Combine(unescapedGameFolder, ConfigFileName);

                bool shouldSave = SwitchSchemeForFileOperations(scheme, unescapedGameFolder);

                if (shouldSave)
                {
                    GameConfig[GeneralSection][Channel] = scheme.Channel;
                    GameConfig[GeneralSection][CPS] = scheme.CPS;
                    GameConfig[GeneralSection][SubChannel] = scheme.SubChannel;

                    try
                    {
                        // new UTF8Encoding(false) compat with https://github.com/DawnFz/GenShin-LauncherDIY
                        new FileIniDataParser().WriteFile(configFilePath, GameConfig, new UTF8Encoding(false));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        new ToastContentBuilder()
                            .AddText("保存服务器配置失败")
                            .AddText("无法写入游戏所在目录的配置文件")
                            .SafeShow();
                    }
                }
            }
        }

        /// <inheritdoc/>
        public ObservableCollection<GenshinAccount> LoadAllAccount()
        {
            // fix load file failure while launched by updater in admin
            return Json.FromFileOrNew<ObservableCollection<GenshinAccount>>(PathContext.Locate(AccountsFileName));
        }

        /// <inheritdoc/>
        public void SaveAllAccounts(IEnumerable<GenshinAccount> accounts)
        {
            // trim account with same id
            Json.ToFile(PathContext.Locate(AccountsFileName), accounts.DistinctBy(account => account.MihoyoSDK));
        }

        /// <inheritdoc/>
        public GenshinAccount? GetFromRegistry()
        {
            return GenshinRegistry.Get();
        }

        /// <inheritdoc/>
        public bool SetToRegistry(GenshinAccount? account)
        {
            return GenshinRegistry.Set(account);
        }

        /// <summary>
        /// 读取 ini 文件
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>ini数据</returns>
        private IniData GetIniData(string file)
        {
            FileIniDataParser parser = new();
            parser.Parser.Configuration.AssigmentSpacer = string.Empty;
            return parser.ReadFile(file);
        }

        private string GetUnescapedGameFolderFromLauncherConfig()
        {
            string gameInstallPath = LauncherConfig[LauncherSection][GameInstallPath];
            string? hex4Result = Regex.Replace(gameInstallPath, @"\\x([0-9a-f]{4})", @"\u$1");

            // 不包含中文
            if (!hex4Result.Contains(@"\u"))
            {
                // fix path with \
                hex4Result = hex4Result.Replace(@"\", @"\\");
            }

            return Regex.Unescape(hex4Result);
        }

        private bool SwitchSchemeForFileOperations(LaunchScheme scheme, string unescapedGameFolder)
        {
            string bilibiliSdkFile = Path.Combine(unescapedGameFolder, "YuanShen_Data/Plugins/PCGameSDK.dll");
            string bilibiliSdkBakFile = Path.Combine(unescapedGameFolder, "YuanShen_Data/Plugins/PCGameSDK.dll.bak");

            bool shouldSave = true;
            switch (scheme.GetSchemeType())
            {
                // this type is not support by default
                case SchemeType.Mihoyo:
                    {
                        new ToastContentBuilder()
                            .AddText("尚未切换到国际服服务实现")
                            .AddText("请安装相关插件后在设置中切换")
                            .SafeShow();
                        shouldSave = false;
                        break;
                    }

                case SchemeType.Bilibili:
                    {
                        if (File.Exists(bilibiliSdkBakFile))
                        {
                            try
                            {
                                File.Move(bilibiliSdkBakFile, bilibiliSdkFile);
                            }
                            catch
                            {
                                new ToastContentBuilder()
                                    .AddText("备份 Bilibili SDK 失败")
                                    .AddText("转换可能不会生效")
                                    .SafeShow();
                                shouldSave = false;
                            }
                        }
                        else
                        {
                            new ToastContentBuilder()
                                .AddText("未在游戏目录发现 Bilibili SDK 备份")
                                .AddText("转换可能不会生效")
                                .SafeShow();
                        }

                        break;
                    }

                case SchemeType.Officical:
                    {
                        if (File.Exists(bilibiliSdkFile))
                        {
                            try
                            {
                                File.Move(bilibiliSdkFile, bilibiliSdkBakFile);
                            }
                            catch
                            {
                                new ToastContentBuilder()
                                    .AddText("备份 Bilibili SDK 失败")
                                    .AddText("转换可能不会生效")
                                    .SafeShow();
                                shouldSave = false;
                            }
                        }
                        else
                        {
                            new ToastContentBuilder()
                                    .AddText("未在游戏目录发现 Bilibili SDK 备份")
                                    .AddText("转换可能不会生效")
                                    .SafeShow();
                        }

                        break;
                    }
            }

            return shouldSave;
        }

        /// <summary>
        /// 定义了对注册表的操作
        /// </summary>
        private class GenshinRegistry
        {
            private const string GenshinKey = @"HKEY_CURRENT_USER\Software\miHoYo\原神";
            private const string SdkKey = "MIHOYOSDK_ADL_PROD_CN_h3123967166";

            public static bool Set(GenshinAccount? account)
            {
                if (account?.MihoyoSDK is not null)
                {
                    Registry.SetValue(GenshinKey, SdkKey, Encoding.UTF8.GetBytes(account.MihoyoSDK));
                    return true;
                }

                return false;
            }

            /// <summary>
            /// 在注册表中获取账号信息
            /// </summary>
            /// <returns>当前注册表中的信息</returns>
            public static GenshinAccount? Get()
            {
                object? sdk = Registry.GetValue(GenshinKey, SdkKey, Array.Empty<byte>());

                if (sdk is byte[] bytes)
                {
                    string sdkString = Encoding.UTF8.GetString(bytes);
                    return new GenshinAccount { MihoyoSDK = sdkString };
                }

                return null;
            }
        }
    }
}