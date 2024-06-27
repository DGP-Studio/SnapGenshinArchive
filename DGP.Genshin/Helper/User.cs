using DGP.Genshin.Helper.Extension;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;

namespace DGP.Genshin.Helper
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User
    {
        private const string CryptographyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\";
        private const string MachineGuidValue = "MachineGuid";

        private static string? id;

        /// <summary>
        /// 用户设备ID
        /// </summary>
        public static string Id
        {
            get
            {
                id ??= GetUniqueUserID();
                return id;
            }
        }

        /// <summary>
        /// 获取设备的UUID
        /// </summary>
        /// <returns>设备的UUID</returns>
        private static string GetUniqueUserID()
        {
            string userName = Environment.UserName;
            object? machineGuid = Registry.GetValue(CryptographyKey, MachineGuidValue, userName);
            byte[] bytes = Encoding.UTF8.GetBytes($"{userName}{machineGuid}");
            byte[] hash = MD5.Create().ComputeHash(bytes);
            return hash.Stringify();
        }
    }
}