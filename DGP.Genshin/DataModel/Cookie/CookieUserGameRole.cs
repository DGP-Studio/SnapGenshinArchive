using DGP.Genshin.MiHoYoAPI.GameRole;

namespace DGP.Genshin.DataModel.Cookie
{
    /// <summary>
    /// Cookie与对应的某个角色信息
    /// </summary>
    public record CookieUserGameRole
    {
        /// <summary>
        /// 构造一个新的Cookie角色信息记录
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <param name="userGameRole">角色信息</param>
        public CookieUserGameRole(string cookie, UserGameRole userGameRole)
        {
            Cookie = cookie;
            UserGameRole = userGameRole;
        }

        /// <summary>
        /// Cookie
        /// </summary>
        public string Cookie { get; init; }

        /// <summary>
        /// 角色信息
        /// </summary>
        public UserGameRole UserGameRole { get; init; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return UserGameRole.ToString();
        }
    }
}