using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Control.WebViewLobby;
using DGP.Genshin.DataModel.WebViewLobby;
using DGP.Genshin.Factory.Abstraction;
using DGP.Genshin.Message;
using DGP.Genshin.Page;
using Snap.Core.DependencyInjection;
using Snap.Core.Mvvm;
using Snap.Data.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 自定义网页管理视图模型
    /// </summary>
    [ViewModel(InjectAs.Singleton)]
    internal class WebViewLobbyViewModel : ObservableObject2
    {
        private const string EntriesFileName = "WebviewEntries.json";
        private const string CommonScriptLinkUrl = "https://www.snapgenshin.com/documents/features/customize-webpage.html";

        private readonly IMessenger messenger;

        private ObservableCollection<WebViewEntry>? entries;

        /// <summary>
        /// 构造一个新的自定义网页管理视图模型
        /// </summary>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        /// <param name="messenger">消息器</param>
        public WebViewLobbyViewModel(IAsyncRelayCommandFactory asyncRelayCommandFactory, IMessenger messenger)
        {
            this.messenger = messenger;

            AddEntryCommand = asyncRelayCommandFactory.Create(AddEntryAsync);
            ModifyCommand = asyncRelayCommandFactory.Create<WebViewEntry>(ModifyEntryAsync);
            RemoveEntryCommand = new RelayCommand<WebViewEntry>(RemoveEntry);
            NavigateCommand = new RelayCommand<WebViewEntry>(Navigate);
            CommonScriptCommand = new RelayCommand(() => Process.Start(new ProcessStartInfo() { FileName = CommonScriptLinkUrl, UseShellExecute = true }));

            LoadEntries();
        }

        /// <summary>
        /// 自定义网页集合
        /// </summary>
        public ObservableCollection<WebViewEntry>? Entries
        {
            get => entries;

            set => SetProperty(ref entries, value);
        }

        /// <summary>
        /// 添加页面命令
        /// </summary>
        public ICommand AddEntryCommand { get; }

        /// <summary>
        /// 打开常用脚本网页
        /// </summary>
        public ICommand CommonScriptCommand { get; }

        /// <summary>
        /// 修改页面命令
        /// </summary>
        public ICommand ModifyCommand { get; }

        /// <summary>
        /// 移除页面命令
        /// </summary>
        public ICommand RemoveEntryCommand { get; }

        /// <summary>
        /// 打开页面命令
        /// </summary>
        public ICommand NavigateCommand { get; }

        private async Task AddEntryAsync()
        {
            WebViewEntry? entry = await new WebViewEntryDialog().GetWebViewEntryAsync();
            if (entry is not null)
            {
                entry.ModifyCommand = ModifyCommand;
                entry.RemoveCommand = RemoveEntryCommand;
                entry.NavigateCommand = NavigateCommand;
                Entries?.Add(entry);
                SaveEntries();
            }
        }

        private async Task ModifyEntryAsync(WebViewEntry? entry)
        {
            if (entry is not null)
            {
                int index = Entries!.IndexOf(entry);
                WebViewEntry? modified = await new WebViewEntryDialog(entry).GetWebViewEntryAsync();
                if (modified is not null)
                {
                    modified.ModifyCommand = ModifyCommand;
                    modified.RemoveCommand = RemoveEntryCommand;
                    modified.NavigateCommand = NavigateCommand;
                    Entries.RemoveAt(index);
                    Entries.Insert(index, modified);
                    SaveEntries();
                }
            }
        }

        private void RemoveEntry(WebViewEntry? entry)
        {
            if (entry is not null)
            {
                Entries!.Remove(entry);
                SaveEntries();
            }
        }

        private void Navigate(WebViewEntry? entry)
        {
            messenger.Send(new NavigateRequestMessage(typeof(WebViewHostPage), false, entry));
        }

        private void LoadEntries()
        {
            if (PathContext.FileExists(EntriesFileName))
            {
                List<WebViewEntry>? list = Json.FromFile<List<WebViewEntry>>(PathContext.Locate(EntriesFileName));
                if (list is not null)
                {
                    list.ForEach(entry =>
                    {
                        entry.ModifyCommand = ModifyCommand;
                        entry.RemoveCommand = RemoveEntryCommand;
                        entry.NavigateCommand = NavigateCommand;
                    });
                    Entries = new(list);
                    return;
                }
            }

            Entries = new();
        }

        private void SaveEntries()
        {
            Json.ToFile(PathContext.Locate(EntriesFileName), Entries);
        }
    }
}