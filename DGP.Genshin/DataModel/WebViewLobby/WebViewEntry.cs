using DGP.Genshin.Control.WebViewLobby;
using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.WebViewLobby
{
    /// <summary>
    /// 自定义网页入口点
    /// </summary>
    public class WebViewEntry
    {
        /// <summary>
        /// 构造一个新的入口点实例
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="navigateUrl">导航链接</param>
        /// <param name="iconUrl">图标链接</param>
        /// <param name="javaScript">JS脚本</param>
        /// <param name="showInNavView">在导航栏中显示</param>
        [JsonConstructor]
        public WebViewEntry(string name, string navigateUrl, string? iconUrl, string? javaScript, bool showInNavView = true)
        {
            Name = name;
            NavigateUrl = navigateUrl;
            IconUrl = iconUrl;
            JavaScript = javaScript;
            ShowInNavView = showInNavView;
        }

        /// <summary>
        /// 使用对话框构造一个新的入口点实例
        /// </summary>
        /// <param name="dialog">对话框</param>
        public WebViewEntry(WebViewEntryDialog dialog)
        {
            Name = dialog.EntryName;
            NavigateUrl = dialog.NavigateUrl;
            IconUrl = dialog.IconUrl;
            JavaScript = dialog.JavaScript;
            ShowInNavView = dialog.ShowInNavView;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 导航链接
        /// </summary>
        public string NavigateUrl { get; set; }

        /// <summary>
        /// 图标链接
        /// </summary>
        public string? IconUrl { get; set; }

        /// <summary>
        /// JS脚本
        /// </summary>
        public string? JavaScript { get; set; }

        /// <summary>
        /// 在导航栏中显示，默认为 <see langword="true"/>
        /// </summary>
        public bool ShowInNavView { get; set; } = true;

        /// <summary>
        /// 修改命令
        /// </summary>
        [JsonIgnore]
        public ICommand? ModifyCommand { get; set; }

        /// <summary>
        /// 移除命令
        /// </summary>
        [JsonIgnore]
        public ICommand? RemoveCommand { get; set; }

        /// <summary>
        /// 导航命令
        /// </summary>
        [JsonIgnore]
        public ICommand? NavigateCommand { get; set; }
    }
}