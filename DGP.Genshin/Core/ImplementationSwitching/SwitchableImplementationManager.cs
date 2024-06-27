using DGP.Genshin.Core.Background.Abstraction;
using DGP.Genshin.Service.Abstraction.Launching;
using Snap.Data.Json;
using System.Collections.Generic;
using System.Linq;

namespace DGP.Genshin.Core.ImplementationSwitching
{
    /// <summary>
    /// 可切换服务管理器
    /// 添加可切换实现后需要同时更改
    /// <see cref="SnapGenshinServiceManager.RegisterSwitchableImplementation(Type, SwitchableImplementationAttribute)"/>
    /// </summary>
    internal class SwitchableImplementationManager
    {
        private const string ImplementationsFile = "implementations.json";

        /// <summary>
        /// 构造一个新的可切换服务管理器
        /// </summary>
        public SwitchableImplementationManager()
        {
            TypeData = Json.FromFileOrNew<ImplmentationTypeData>(PathContext.Locate(ImplementationsFile));
        }

        /// <summary>
        /// 背景图片提供器集合
        /// </summary>
        [SwitchableInterfaceType(typeof(IBackgroundProvider))]
        public List<SwitchableEntry<IBackgroundProvider>> BackgroundProviders { get; internal set; } = new();

        /// <summary>
        /// 当前的背景图片提供器
        /// </summary>
        public SwitchableEntry<IBackgroundProvider>? CurrentBackgroundProvider { get; set; }

        /// <summary>
        /// 启动服务合集
        /// </summary>
        [SwitchableInterfaceType(typeof(ILaunchService))]
        public List<SwitchableEntry<ILaunchService>> LaunchServices { get; internal set; } = new();

        /// <summary>
        /// 当前的启动服务
        /// </summary>
        public SwitchableEntry<ILaunchService>? CurrentLaunchService { get; set; }

        private ImplmentationTypeData TypeData { get; }

        /// <summary>
        /// 在程序集探测完成后将管理器内的实现切换到正确的实现
        /// </summary>
        public void SwitchToCorrectImplementations()
        {
            CurrentBackgroundProvider = BackgroundProviders.FirstOrDefault(i => i.Name == TypeData.BackgroundProviderName)
                ?? BackgroundProviders.First(i => i.Name == SwitchableImplementationAttribute.DefaultName);

            CurrentLaunchService = LaunchServices.FirstOrDefault(i => i.Name == TypeData.LaunchServiceName)
                ?? LaunchServices.First(i => i.Name == SwitchableImplementationAttribute.DefaultName);
        }

        /// <summary>
        /// 退出程序时调用以保存选项
        /// </summary>
        public void UnInitialize()
        {
            TypeData.BackgroundProviderName = CurrentBackgroundProvider!.Name;
            TypeData.LaunchServiceName = CurrentLaunchService!.Name;

            Json.ToFile(PathContext.Locate(ImplementationsFile), TypeData);
        }

        /// <summary>
        /// 可切换服务入口点
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        internal class SwitchableEntry<T>
        {
            /// <summary>
            /// 构造一个新的可切换服务入口
            /// </summary>
            /// <param name="attribute">标记的特性</param>
            /// <param name="factory">工厂</param>
            public SwitchableEntry(SwitchableImplementationAttribute attribute, Lazy<T> factory)
            {
                Name = attribute.Name;
                Description = attribute.Description;
                Factory = factory;
            }

            /// <summary>
            /// 唯一名称
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// 展示描述
            /// </summary>
            public string Description { get; }

            /// <summary>
            /// 工厂
            /// </summary>
            public Lazy<T> Factory { get; }
        }

        private class ImplmentationTypeData
        {
            public string BackgroundProviderName { get; set; } = SwitchableImplementationAttribute.DefaultName;

            public string LaunchServiceName { get; set; } = SwitchableImplementationAttribute.DefaultName;
        }
    }
}