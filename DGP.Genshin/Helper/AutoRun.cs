using Microsoft.Win32;
using System.Diagnostics;

namespace DGP.Genshin.Helper
{
    /// <summary>
    /// 自启动
    /// </summary>
    public class AutoRun
    {
        private const string AppName = "SnapGenshin";
        private const string RunPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// 是否自动启动
        /// </summary>
        public bool IsAutoRun
        {
            get
            {
                RegistryKey currentUser = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                RegistryKey? run = currentUser.OpenSubKey(RunPath);
                return run?.GetValue(AppName) is not null;
            }

            set
            {
                RegistryKey currentUser = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                RegistryKey? run = currentUser.CreateSubKey(RunPath);
                Must.NotNull(run!);

                if (value)
                {
                    string? appFileName = Process.GetCurrentProcess().MainModule?.FileName;
                    Must.NotNull(appFileName!);
                    run.SetValue(AppName, appFileName);
                }
                else
                {
                    run.DeleteValue(AppName);
                }
            }
        }
    }
}