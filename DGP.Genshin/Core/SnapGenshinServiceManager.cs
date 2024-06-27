using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Core.Background.Abstraction;
using DGP.Genshin.Core.ImplementationSwitching;
using DGP.Genshin.Core.Plugins;
using DGP.Genshin.Service.Abstraction.Launching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Threading;
using Snap.Extenion.Enumerable;
using Snap.Reflection;
using System.Collections.Generic;

namespace DGP.Genshin.Core
{
    /// <summary>
    /// 实现了注入插件的服务管理类
    /// </summary>
    internal class SnapGenshinServiceManager : ServiceManagerBase
    {
        /// <summary>
        /// 重载探测程序集方法
        /// 注入插件
        /// </summary>
        /// <param name="services">服务集合</param>
        protected override void OnProbingServices(ServiceCollection services)
        {
            // default messager
            services.AddSingleton<IMessenger>(App.Messenger);

            // JoinableTaskContext
            JoinableTaskContext context = new();
            services.AddSingleton(context);
            services.AddSingleton(new JoinableTaskFactory(context));

            base.OnProbingServices(services);

            // insert plugins services here
            RegisterPluginsServices(services, App.Current.PluginService.Plugins);
        }

        /// <inheritdoc/>
        protected override void RegisterService(ServiceCollection services, Type type)
        {
            base.RegisterService(services, type);

            // filter out switchable implementation
            if (type.TryGetAttribute(out SwitchableImplementationAttribute? attr))
            {
                RegisterSwitchableImplementation(type, attr);
            }
            else
            {
                base.RegisterService(services, type);
            }
        }

        /// <summary>
        /// 向容器注册插件内的服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="plugins">插件集合</param>
        private void RegisterPluginsServices(ServiceCollection services, IEnumerable<IPlugin> plugins)
        {
            plugins.ForEach(plugin => RegisterServices(services, plugin.GetType()));
        }

        private void RegisterSwitchableImplementation(Type type, SwitchableImplementationAttribute attr)
        {
            SwitchableImplementationManager manager = App.Current.SwitchableImplementationManager;

            // no switch expression here
            if (attr.TargetType == typeof(IBackgroundProvider))
            {
                manager.BackgroundProviders
                    .Add(new(attr, new(() => (IBackgroundProvider)Activator.CreateInstance(type)!)));
            }
            else if (attr.TargetType == typeof(ILaunchService))
            {
                manager.LaunchServices
                    .Add(new(attr, new(() => (ILaunchService)Activator.CreateInstance(type)!)));
            }
        }
    }
}