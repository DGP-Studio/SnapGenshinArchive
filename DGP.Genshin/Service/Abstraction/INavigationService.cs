using DGP.Genshin.Core.Plugins;
using DGP.Genshin.DataModel.WebViewLobby;
using DGP.Genshin.Message;
using ModernWpf.Controls;
using ModernWpf.Media.Animation;
using System.Collections.ObjectModel;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 导航服务
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// 指示是否曾导航过,用于启动时导航判断
        /// </summary>
        bool HasEverNavigated { get; set; }

        /// <summary>
        /// 管理的 <see cref="ModernWpf.Controls.Frame"/>
        /// </summary>
        Frame? Frame { get; set; }

        /// <summary>
        /// 管理的 <see cref="ModernWpf.Controls.NavigationView"/>
        /// </summary>
        NavigationView? NavigationView { get; set; }

        /// <summary>
        /// 选中的 <see cref="NavigationViewItem"/>
        /// </summary>
        NavigationViewItem? Selected { get; set; }

        /// <summary>
        /// 将页面添加到导航视图
        /// </summary>
        /// <param name="importPage">导入的页面</param>
        /// <returns>是否导入成功</returns>
        bool AddToNavigation(ImportPageAttribute importPage);

        /// <summary>
        /// 导航到指定类型的页面
        /// </summary>
        /// <param name="pageType">指定的页面类型</param>
        /// <param name="isSyncTabRequested">是否同步标签，当在代码中调用时应设为 true</param>
        /// <param name="data">要传递的数据</param>
        /// <param name="info">导航动画变换信息</param>
        /// <returns>是否导航成功</returns>
        bool Navigate(Type? pageType, bool isSyncTabRequested = false, object? data = null, NavigationTransitionInfo? info = null);

        /// <summary>
        /// 导航到指定类型的页面
        /// </summary>
        /// <typeparam name="T">指定的页面类型</typeparam>
        /// <param name="isSyncTabRequested">是否同步标签，当在代码中调用时应设为 true</param>
        /// <param name="data">要传递的数据</param>
        /// <param name="info">导航动画变换信息</param>
        /// <returns>是否导航成功</returns>
        bool Navigate<T>(bool isSyncTabRequested = false, object? data = null, NavigationTransitionInfo? info = null)
            where T : System.Windows.Controls.Page;

        /// <summary>
        /// 同步导航标签
        /// </summary>
        /// <param name="pageType">同步的页面类型</param>
        /// <returns>是否同步成功</returns>
        bool SyncTabWith(Type pageType);

        /// <summary>
        /// 添加自定义网页入口
        /// </summary>
        /// <param name="entries">入口集合</param>
        void AddWebViewEntries(ObservableCollection<WebViewEntry>? entries);

        /// <summary>
        /// 导航到指定的页面
        /// </summary>
        /// <param name="message">导航请求消息</param>
        /// <returns>是否导航成功</returns>
        bool Navigate(NavigateRequestMessage message);
    }
}