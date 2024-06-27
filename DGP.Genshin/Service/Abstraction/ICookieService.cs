using DGP.Genshin.DataModel.Cookie;
using Microsoft.VisualStudio.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 全局Cookie管理服务
    /// </summary>
    public interface ICookieService
    {
        /// <summary>
        /// 定义Cookie池
        /// </summary>
        public interface ICookiePool : IList<string>
        {
            /// <summary>
            /// 添加Cookie,
            /// 隐藏了基类成员<see cref="ICollection{T}.Add(T)"/>以便发送事件
            /// 该方法是线程不安全的，请勿直接调用
            /// </summary>
            /// <param name="cookie">cookie</param>
            new void Add(string cookie);

            /// <summary>
            /// 添加或忽略相同的 Cookie
            /// </summary>
            /// <param name="cookie">cookie</param>
            /// <returns>是否成功添加</returns>
            bool AddOrIgnore(string cookie);

            /// <summary>
            /// 移除Cookie,
            /// 隐藏了基类成员<see cref="ICollection{T}.Remove(T)"/>以便发送事件
            /// </summary>
            /// <param name="cookie">cookie</param>
            /// <returns>是否移除成功</returns>
            new bool Remove(string cookie);
        }

        /// <summary>
        /// 备选Cookie池
        /// </summary>
        ICookiePool Cookies { get; set; }

        /// <summary>
        /// 当前使用的Cookie, 由 <see cref="ICookieService"/> 保证不为 <see cref="null"/>
        /// </summary>
        string CurrentCookie { get; }

        /// <summary>
        /// 用于在初始化时判断Cookie是否可用
        /// </summary>
        bool IsCookieAvailable { get; }

        /// <summary>
        /// 使用读写锁保证 <see cref="Cookies"/> 线程安全
        /// </summary>
        AsyncReaderWriterLock CookiesLock { get; init; }

        /// <summary>
        /// 向Cookie池异步添加Cookie,
        /// 忽略已存在的Cookie,
        /// 不更新 <see cref="CurrentCookie"/> 的值
        /// </summary>
        /// <returns>任务</returns>
        Task AddCookieToPoolOrIgnoreAsync();

        /// <summary>
        /// 将 <see cref="CurrentCookie"/> 的值设置为 <see cref="Cookies"/> 中查找的值
        /// </summary>
        /// <param name="cookie">cookie</param>
        void ChangeOrIgnoreCurrentCookie(string? cookie);

        /// <summary>
        /// 保存 <see cref="Cookies"/> 内的所有 Cookie 信息
        /// </summary>
        void SaveCookies();

        /// <summary>
        /// 异步设置新的Cookie,
        /// 更新 <see cref="CurrentCookie"/> 的值
        /// </summary>
        /// <returns>任务</returns>
        Task SetCookieAsync();

        /// <summary>
        /// 保存当前的Cookie
        /// </summary>
        /// <param name="cookie">cookie</param>
        void SaveCookie(string cookie);

        /// <summary>
        /// 初始化Cookie服务
        /// 在初始化完成前 <see cref="IsCookieAvailable"/> 始终为 false
        /// </summary>
        /// <returns>任务</returns>
        Task InitializeAsync();

        /// <summary>
        /// 获取cookie对应的 <see cref="CookieUserGameRole"/> 集合
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <returns>对应的 <see cref="CookieUserGameRole"/> 集合</returns>
        Task<IEnumerable<CookieUserGameRole>> GetCookieUserGameRolesOfAsync(string cookie);
    }
}