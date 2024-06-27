using DGP.Genshin.MiHoYoAPI.UserInfo;

namespace DGP.Genshin.DataModel.Cookie
{
    /// <summary>
    /// Cookie与对应的用户信息
    /// </summary>
    public class CookieUserInfo
    {
        /// <summary>
        /// 构造一个新的Cookie用户信息记录
        /// </summary>
        /// <param name="cookie">Cookie</param>
        /// <param name="userInfo">用户信息</param>
        public CookieUserInfo(string cookie, UserInfo userInfo)
        {
            Cookie = cookie;
            UserInfo = userInfo;
        }

        /// <summary>
        /// Cookie
        /// </summary>
        public string Cookie { get; init; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; init; }
    }
}