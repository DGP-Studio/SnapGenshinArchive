using DGP.Genshin.MiHoYoAPI;
using Microsoft.VisualStudio.Threading;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Helper
{
    /// <summary>
    /// 网络状态
    /// 仅针对米哈游服务器检测
    /// </summary>
    public static class Network
    {
        private static readonly ManualResetEvent NetworkConnected = new(true);

        /// <summary>
        /// 无限等待，直到网络连接成功
        /// </summary>
        /// <returns>任务</returns>
        public static async Task WaitConnectionAsync()
        {
            NetworkChange.NetworkAddressChanged += HandleNetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged += HandleNetworkAvailabilityChanged;

            // first init
            await TrySetNetworkConnectedAsync();

            // continious work
            TryWaitNetworkConnectionAsync().Forget();

            // wait for connection
            await Task.Run(() => NetworkConnected.WaitOne());

            NetworkChange.NetworkAddressChanged -= HandleNetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged -= HandleNetworkAvailabilityChanged;
        }

        /// <summary>
        /// 反复尝试
        /// </summary>
        /// <returns>任务</returns>
        private static async Task TryWaitNetworkConnectionAsync()
        {
            while (await TrySetNetworkConnectedAsync())
            {
                await Task.Delay(5000);
            }
        }

        /// <summary>
        /// 尝试设置网络状态
        /// </summary>
        /// <returns>是否继续尝试</returns>
        private static async Task<bool> TrySetNetworkConnectedAsync()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (await new Status().CheckStatusAsync())
                {
                    NetworkConnected.Set();
                    return false;
                }
                else
                {
                    NetworkConnected.Reset();
                    return true;
                }
            }

            NetworkConnected.Reset();
            return true;
        }

        private static void HandleNetworkAddressChanged(object? s, EventArgs e)
        {
            Task.Run(async () =>
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    if (await new Status().CheckStatusAsync())
                    {
                        NetworkConnected.Set();
                    }
                }
            }).Forget();
        }

        private static void HandleNetworkAvailabilityChanged(object? s, NetworkAvailabilityEventArgs e)
        {
            Task.Run(async () =>
            {
                if (e.IsAvailable)
                {
                    if (await new Status().CheckStatusAsync())
                    {
                        NetworkConnected.Set();
                    }
                }
            }).Forget();
        }
    }
}