using System;
using System.Net.NetworkInformation;
using System.Text;

namespace Snap.Net.Networking
{
    /// <summary>
    /// Ping
    /// </summary>
    [Obsolete("检查网络连接不再使用此类")]
    public static class Pinger
    {
        /// <summary>
        /// 检测连接
        /// </summary>
        /// <param name="host">主机</param>
        /// <returns>到主机的连接是否可用</returns>
        public static bool Test(string host)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("ping test data");
            PingOptions options = new() { DontFragment = true };
            try
            {
                // enlarge timeout to fix blocking issue.
                PingReply reply = new Ping().Send(host, 10000, buffer, options);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}