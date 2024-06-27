using DGP.Genshin.Core.Plugins;
using System;

[assembly: SnapGenshinPlugin]

namespace DGP.Genshin.Sample.Plugin
{
    /// <summary>
    /// 插件实例实现
    /// </summary>
    [ImportPage(typeof(SamplePage), "设计图标集", "\uE734")]
    public class SamplePlugin : IPlugin
    {
        /// <inheritdoc/>
        public string Name
        {
            get => "设计图标集";
        }

        /// <inheritdoc/>
        public string Description
        {
            get => "本插件用于设计人员查看所有的 Segoe Fluent Icons 字符";
        }

        /// <inheritdoc/>
        public string Author
        {
            get => "DGP Studio";
        }

        /// <inheritdoc/>
        public Version Version
        {
            get => new("0.0.0.2");
        }

        /// <inheritdoc/>
        public bool IsEnabled
        {
            get;
        }
    }
}