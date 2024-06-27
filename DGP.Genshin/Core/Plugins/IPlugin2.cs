namespace DGP.Genshin.Core.Plugins
{
    /// <summary>
    /// 插件功能拓展接口
    /// 支持插件页面的额外详细链接导航
    /// 支持插件页面的右键上下文菜单
    /// </summary>
    public interface IPlugin2
    {
        /// <summary>
        /// 插件的详情链接
        /// </summary>
        public string? DetailLink { get; }

        /// <summary>
        /// 设置是否在插件页面上显示设置按钮
        /// </summary>
        public bool IsSettingSupported { get; }

        /// <summary>
        /// 按下设置按钮后执行的命令
        /// <para/>
        /// 在构造函数中
        /// <code>
        /// SettingCommand = new RelayCommand(NavigateToSettingPage);
        /// </code>
        /// <para/>
        /// 实际导航函数
        /// <code>
        /// private void NavigateToSettingPage()
        /// {
        ///     App.Messenger.Send(new NavigateRequestMessage(typeof(YourSettingPage)));
        /// }
        /// </code>
        /// </summary>
        public ICommand SettingCommand { get; }
    }
}