using Microsoft.Win32;
using System.Diagnostics;

namespace DGP.Genshin.Helper.UrlProtocol
{
    /// <summary>
    /// Url 协议
    /// </summary>
    public static class UrlProtocol
    {
        private const string ClassesPath = @"SOFTWARE\Classes";
        private const string ProtocolName = "snapgenshin";
        private const string LastLaunchParameterKey = "LastLaunchParameter";

        /// <summary>
        /// Url协议启动参数
        /// </summary>
        public static string Argument
        {
            get
            {
                return (string)RegistryKey
                    .OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64)
                    .CreateSubKey(ClassesPath)
                    .CreateSubKey(ProtocolName)
                    .GetValue(LastLaunchParameterKey)!;
            }

            set
            {
                RegistryKey
                    .OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64)
                    .CreateSubKey(ClassesPath)
                    .CreateSubKey(ProtocolName)
                    .SetValue(LastLaunchParameterKey, value);
            }
        }

        /// <summary>
        /// 注册协议
        /// </summary>
        public static void Register()
        {
            RegistryKey currentUser = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            RegistryKey? classesKey = currentUser.CreateSubKey(ClassesPath);
            Must.NotNull(classesKey!);

            RegistryKey schemeKey = classesKey.CreateSubKey(ProtocolName);

            // set protocol description
            schemeKey.SetValue(string.Empty, "URL:snapgenshin");
            schemeKey.SetValue("URL Protocol", string.Empty);

            string? appFileName = Process.GetCurrentProcess().MainModule?.FileName;
            Must.NotNull(appFileName!);

            // set protocol open command
            schemeKey
                .CreateSubKey("Shell")
                .CreateSubKey("Open")
                .CreateSubKey("Command")
                .SetValue(string.Empty, $@"""{appFileName}"" ""%1""");
        }
    }
}