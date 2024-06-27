using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Control.Cookie;
using DGP.Genshin.DataModel.Cookie;
using DGP.Genshin.Message;
using DGP.Genshin.MiHoYoAPI.GameRole;
using DGP.Genshin.MiHoYoAPI.UserInfo;
using DGP.Genshin.Service.Abstraction;
using Microsoft.VisualStudio.Threading;
using ModernWpf.Controls;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Data.Json;
using Snap.Data.Primitive;
using Snap.Data.Utility;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGP.Genshin.Service
{
    /// <summary>
    /// TODO 添加 current cookie 启动时校验
    /// </summary>
    [Service(typeof(ICookieService), InjectAs.Singleton)]
    internal class CookieService : ICookieService
    {
        private const string CookieFile = "cookie.dat";
        private const string CookieListFile = "cookielist.dat";

        private static string? currentCookie;

        private readonly IMessenger messenger;

        /// <summary>
        /// prevent multiple times initializaion
        /// </summary>
        private readonly WorkWatcher initialization = new(false);

        /// <summary>
        /// 构造一个新的Cookie服务
        /// </summary>
        /// <param name="joinableTaskContext">可加入的任务上下文</param>
        /// <param name="messenger">消息器</param>
        public CookieService(JoinableTaskContext joinableTaskContext, IMessenger messenger)
        {
            this.messenger = messenger;

            CookiesLock = new(joinableTaskContext);

            LoadCookies();
            LoadCookie();
        }

        /// <inheritdoc/>
        public ICookieService.ICookiePool Cookies { get; set; }

        /// <inheritdoc/>
        public AsyncReaderWriterLock CookiesLock { get; init; }

        /// <inheritdoc/>
        public string CurrentCookie
        {
            get => Must.NotNull(currentCookie!);

            private set
            {
                if (currentCookie == value)
                {
                    return;
                }

                currentCookie = value;

                try
                {
                    SaveCookie(value);
                    Cookies.AddOrIgnore(currentCookie);
                    messenger.Send(new CookieChangedMessage(currentCookie));
                }
                catch (UnauthorizedAccessException)
                {
                    Verify.FailOperation("Snap Genshin 无法访问当前目录，请将应用程序移动到别处。");
                }
            }
        }

        /// <inheritdoc/>
        public bool IsCookieAvailable
        {
            get => initialization.IsCompleted && (!string.IsNullOrEmpty(currentCookie));
        }

        /// <inheritdoc/>
        public async Task SetCookieAsync()
        {
            (bool isOk, string cookie) = await new CookieDialog().GetInputCookieAsync();
            if (isOk)
            {
                // prevent user input unexpected invalid cookie
                if (!string.IsNullOrEmpty(cookie) && await ValidateCookieAsync(cookie))
                {
                    CurrentCookie = cookie;
                }

                File.WriteAllText(PathContext.Locate(CookieFile), CurrentCookie);
            }
        }

        /// <inheritdoc/>
        public void ChangeOrIgnoreCurrentCookie(string? cookie)
        {
            if (cookie is not null)
            {
                CurrentCookie = cookie;
            }
        }

        /// <inheritdoc/>
        public async Task AddCookieToPoolOrIgnoreAsync()
        {
            (bool isOk, string newCookie) = await new CookieDialog(false).GetInputCookieAsync();

            if (isOk)
            {
                if (await ValidateCookieAsync(newCookie))
                {
                    using (await CookiesLock.WriteLockAsync())
                    {
                        Cookies.AddOrIgnore(newCookie);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void SaveCookie(string cookie)
        {
            try
            {
                File.WriteAllText(PathContext.Locate(CookieFile), cookie);
            }
            catch (IOException)
            {
                Verify.FailOperation($"{CookieFile} 文件被占用，保存cookie失败");
            }
        }

        /// <inheritdoc/>
        public void SaveCookies()
        {
            IEnumerable<string> encodedCookies = Cookies.Select(c => Base64Converter.Base64Encode(Encoding.UTF8, c));
            Json.ToFile(PathContext.Locate(CookieListFile), encodedCookies);
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            if (initialization.IsWorking || initialization.IsCompleted)
            {
                return;
            }

            using (initialization.Watch())
            {
                using (await CookiesLock.WriteLockAsync())
                {
                    // Enumerate the shallow copied list to remove item in foreach loop
                    // Prevent InvalidOperationException
                    foreach (string cookie in Cookies.ToList())
                    {
                        if (await new UserInfoProvider(cookie).GetUserInfoAsync() is null)
                        {
                            // 删除用户无法手动选中的cookie(失效的cookie)
                            Cookies.Remove(cookie);
                        }
                    }

                    if (Cookies.Count <= 0)
                    {
                        currentCookie = null;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CookieUserGameRole>> GetCookieUserGameRolesOfAsync(string cookie)
        {
            List<UserGameRole> userGameRoles = await new UserGameRoleProvider(cookie).GetUserGameRolesAsync();
            return userGameRoles.Select(u => new CookieUserGameRole(cookie, u));
        }

        /// <summary>
        /// 加载 cookie.dat 文件
        /// </summary>
        private void LoadCookie()
        {
            // load cookie
            string? cookieFile = PathContext.Locate(CookieFile);
            if (File.Exists(cookieFile))
            {
                CurrentCookie = File.ReadAllText(cookieFile);
            }
            else
            {
                this.Log("无可用的Cookie");
                PathContext.CreateFileOrIgnore(CookieFile);
            }
        }

        /// <summary>
        /// 加载 cookielist.dat 文件
        /// </summary>
        [MemberNotNull(nameof(Cookies))]
        private void LoadCookies()
        {
            IEnumerable<string> cookies = Json
                    .FromFileOrNew<List<string>>(CookieListFile)
                    .Select(b => Base64Converter.Base64Decode(Encoding.UTF8, b));
            Cookies = new CookiePool(this, messenger, cookies);

            Must.NotNull(Cookies!);
        }

        /// <summary>
        /// 验证Cookie是否任处于登录态
        /// 若Cookie已失效则会弹出对话框提示用户
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <returns>是否有效</returns>
        private async Task<bool> ValidateCookieAsync(string cookie, bool showDialog = true)
        {
            if (await new UserInfoProvider(cookie).GetUserInfoAsync() is null)
            {
                if (showDialog)
                {
                    await new ContentDialog
                    {
                        Title = "该Cookie无效",
                        Content = "无法获取到你的账户信息，\n可能是Cookie已经失效，请重新登录获取",
                        PrimaryButtonText = "确认",
                        DefaultButton = ContentDialogButton.Primary,
                    }.ShowAsync();
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Cookie池的默认实现，提供Cookie操作事件支持
        /// Cookie池是非线程安全的对象，需要外围操作确保线程安全
        /// </summary>
        internal class CookiePool : List<string>, ICookieService.ICookiePool
        {
            private readonly List<string> acountIds = new();
            private readonly ICookieService cookieService;
            private readonly IMessenger messenger;

            /// <summary>
            /// 构造新的 Cookie 池的默认实例
            /// </summary>
            /// <param name="cookieService">cookie服务</param>
            /// <param name="messenger">消息器</param>
            /// <param name="collection">字符串集合</param>
            public CookiePool(ICookieService cookieService, IMessenger messenger, IEnumerable<string> collection)
                : base(collection)
            {
                this.cookieService = cookieService;
                this.messenger = messenger;
                acountIds.AddRange(collection.Select(item => GetCookiePairs(item)["account_id"]));
            }

            /// <inheritdoc/>
            public new void Add(string cookie)
            {
                if (!string.IsNullOrEmpty(cookie))
                {
                    base.Add(cookie);
                    messenger.Send(new CookieAddedMessage(cookie));
                    cookieService.SaveCookies();
                }
            }

            /// <inheritdoc/>
            public bool AddOrIgnore(string cookie)
            {
                if (GetCookiePairs(cookie).TryGetValue("account_id", out string? id))
                {
                    if (!acountIds.Contains(id))
                    {
                        acountIds.Add(id);
                        Add(cookie);
                        return true;
                    }
                }

                return false;
            }

            /// <inheritdoc/>
            public new bool Remove(string cookie)
            {
                string id = GetCookiePairs(cookie)["account_id"];
                acountIds.Remove(id);
                bool result = base.Remove(cookie);
                messenger.Send(new CookieRemovedMessage(cookie));
                cookieService.SaveCookies();
                return result;
            }

            private IDictionary<string, string> GetCookiePairs(string cookie)
            {
                Dictionary<string, string> cookieDictionary = new();

                string[] values = cookie.TrimEnd(';').Split(';');
                foreach (string[] parts in values.Select(c => c.Split(new[] { '=' }, 2)))
                {
                    string cookieName = parts[0].Trim();
                    string cookieValue = parts.Length == 1 ? string.Empty : parts[1].Trim();

                    cookieDictionary[cookieName] = cookieValue;
                }

                return cookieDictionary;
            }
        }
    }
}