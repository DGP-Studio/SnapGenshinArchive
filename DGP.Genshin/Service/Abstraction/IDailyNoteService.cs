using DGP.Genshin.DataModel.Cookie;
using DGP.Genshin.MiHoYoAPI.Record.DailyNote;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 实时便笺服务
    /// </summary>
    public interface IDailyNoteService
    {
        /// <summary>
        /// 获取实时便笺
        /// </summary>
        /// <param name="cookieUserGameRole">用户cookie与角色信息</param>
        /// <returns>查询到的实时便笺</returns>
        DailyNote? GetDailyNote(CookieUserGameRole cookieUserGameRole);

        /// <summary>
        /// 初始化服务
        /// </summary>
        void Initialize();

        /// <summary>
        /// 终止服务
        /// </summary>
        void UnInitialize();
    }
}