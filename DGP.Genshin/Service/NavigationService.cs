using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Control.Helper;
using DGP.Genshin.Core.Plugins;
using DGP.Genshin.DataModel.WebViewLobby;
using DGP.Genshin.Message;
using DGP.Genshin.Page;
using DGP.Genshin.Service.Abstraction;
using Microsoft.AppCenter.Analytics;
using Microsoft.Toolkit.Uwp.Notifications;
using ModernWpf.Controls;
using ModernWpf.Media.Animation;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Data.Utility.Extension;
using System.Collections.ObjectModel;
using System.Linq;

namespace DGP.Genshin.Service
{
    /// <summary>
    /// 导航服务的默认实现
    /// 注册的类型实际上没有意义，但是为了防止多次创建导航服务
    /// 注册为单例
    /// </summary>
    [Service(typeof(INavigationService), InjectAs.Singleton)]
    internal class NavigationService : INavigationService, IRecipient<NavigateRequestMessage>
    {
        private readonly IMessenger messenger;
        private NavigationView? navigationView;

        /// <summary>
        /// 构造一个新的导航服务
        /// </summary>
        /// <param name="messenger">消息器</param>
        public NavigationService(IMessenger messenger)
        {
            this.messenger = messenger;
            messenger.RegisterAll(this);
        }

        /// <summary>
        /// 释放消息器资源
        /// </summary>
        ~NavigationService()
        {
            messenger.UnregisterAll(this);
        }

        /// <inheritdoc/>
        public Frame? Frame { get; set; }

        /// <inheritdoc/>
        public NavigationView? NavigationView
        {
            get => navigationView;

            set
            {
                // remove old listener
                if (navigationView != null)
                {
                    navigationView.ItemInvoked -= OnItemInvoked;
                }

                navigationView = value;

                // add new listener
                if (navigationView != null)
                {
                    navigationView.ItemInvoked += OnItemInvoked;
                }
            }
        }

        /// <inheritdoc/>
        public NavigationViewItem? Selected { get; set; }

        /// <inheritdoc/>
        public bool HasEverNavigated { get; set; }

        /// <inheritdoc/>
        public bool SyncTabWith(Type pageType)
        {
            if (NavigationView is null)
            {
                return false;
            }

            if (pageType == typeof(SettingPage))
            {
                NavigationView.SelectedItem = NavigationView.SettingsItem;
            }
            else
            {
                NavigationViewItem? target = NavigationView.MenuItems
                    .OfType<NavigationViewItem>()
                    .FirstOrDefault(menuItem => NavHelper.GetNavigateTo(menuItem) == pageType);
                NavigationView.SelectedItem = target;
            }

            Selected = NavigationView.SelectedItem as NavigationViewItem;
            return true;
        }

        /// <inheritdoc/>
        public bool Navigate(Type? pageType, bool isSyncTabRequested = false, object? data = null, NavigationTransitionInfo? info = null)
        {
            Type? currentType = Frame?.Content?.GetType();
            if (pageType is null || (currentType == pageType && currentType != typeof(WebViewHostPage)))
            {
                return false;
            }

            _ = isSyncTabRequested && SyncTabWith(pageType);

            bool result = false;
            try
            {
                result = Frame?.Navigate(App.AutoWired(pageType), data) ?? false;
            }
            catch (Exception ex)
            {
                this.Log(ex);
            }

            this.Log($"Navigate to {pageType}:{(result ? "succeed" : "failed")}");

            // 分析页面统计数据时不应加入启动时导航的首个页面
            if (HasEverNavigated)
            {
                Analytics.TrackEvent("General", ("OpenUI", pageType.ToString()).AsDictionary());
            }

            // fix memory leak issue
            Frame?.RemoveBackEntry();

            // 首次导航失败时使属性持续保存为false
            HasEverNavigated |= result;
            return result;
        }

        /// <inheritdoc/>
        public bool Navigate(NavigateRequestMessage message)
        {
            return Navigate(message.Value, message.IsSyncTabRequested, message.ExtraData);
        }

        /// <inheritdoc/>
        public bool Navigate<T>(bool isSyncTabRequested = false, object? data = null, NavigationTransitionInfo? info = null)
            where T : System.Windows.Controls.Page
        {
            return Navigate(typeof(T), isSyncTabRequested, data, info);
        }

        /// <inheritdoc/>
        public bool AddToNavigation(ImportPageAttribute importPage)
        {
            return AddToNavigation(importPage.PageType, importPage.Label, importPage.Icon);
        }

        /// <inheritdoc/>
        public void AddWebViewEntries(ObservableCollection<WebViewEntry>? entries)
        {
            if (NavigationView is null)
            {
                return;
            }

            if (entries is not null)
            {
                NavigationViewItemHeader? header = NavigationView.MenuItems
                    .OfType<NavigationViewItemHeader>()
                    .SingleOrDefault(header => header.Content.ToString() == "网页");
                if (header is not null)
                {
                    int headerIndex = NavigationView.MenuItems.IndexOf(header);

                    if (entries.Count > 0)
                    {
                        header.Visibility = System.Windows.Visibility.Visible;
                    }

                    foreach (WebViewEntry entry in entries)
                    {
                        if (entry.ShowInNavView)
                        {
                            BitmapIcon icon = new() { ShowAsMonochrome = false };

                            icon.UriSource = Uri.TryCreate(entry.IconUrl, UriKind.Absolute, out Uri? iconUri)
                                ? iconUri
                                : new Uri("pack://application:,,,/SG_Logo.ico");

                            NavigationViewItem item = new()
                            {
                                Icon = icon,
                                Content = entry.Name,
                            };

                            NavHelper.SetNavigateTo(item, typeof(WebViewHostPage));
                            NavHelper.SetExtraData(item, entry);

                            NavigationView.MenuItems.Insert(++headerIndex, item);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Receive(NavigateRequestMessage message)
        {
            if (!Navigate(message))
            {
                new ToastContentBuilder()
                    .AddText("导航到指定的页面失败")
                    .Show();
            }
        }

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            Selected = NavigationView?.SelectedItem as NavigationViewItem;
            Type? targetType = args.IsSettingsInvoked ? typeof(SettingPage) : NavHelper.GetNavigateTo(Selected);
            Navigate(targetType, false, NavHelper.GetExtraData(Selected));
        }

        private bool AddToNavigation(Type pageType, string label, IconElement icon)
        {
            if (NavigationView is null)
            {
                return false;
            }

            NavigationViewItem item = new() { Content = label, Icon = icon };
            NavHelper.SetNavigateTo(item, pageType);
            this.Log($"Add {pageType} to NavigationView");
            return NavigationView.MenuItems.Add(item) != -1;
        }
    }
}