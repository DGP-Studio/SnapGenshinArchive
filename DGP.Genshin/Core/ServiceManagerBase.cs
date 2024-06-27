using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Snap.Core.DependencyInjection;
using Snap.Reflection;

namespace DGP.Genshin.Core
{
    /// <summary>
    /// 服务管理器
    /// 依赖注入的核心管理类
    /// </summary>
    internal class ServiceManagerBase
    {
        /// <summary>
        /// 实例化一个新的服务管理器
        /// </summary>
        public ServiceManagerBase()
        {
            Services = ConfigureServices();
        }

        /// <summary>
        /// 获取 <see cref="IServiceProvider"/> 的实例
        /// 存放类
        /// </summary>
        public IServiceProvider? Services { get; protected set; }

        /// <summary>
        /// 向容器注册类型
        /// </summary>
        /// <param name="services">容器</param>
        /// <param name="type">待检测的类型</param>
        /// <exception cref="SnapGenshinInternalException">未知的注册类型</exception>
        protected virtual void RegisterService(ServiceCollection services, Type type)
        {
            if (type.TryGetAttribute(out InjectableAttribute? attr))
            {
                if (attr is InterfaceInjectableAttribute interfaceInjectable)
                {
                    _ = interfaceInjectable.InjectAs switch
                    {
                        InjectAs.Singleton => services.AddSingleton(interfaceInjectable.InterfaceType, type),
                        InjectAs.Transient => services.AddTransient(interfaceInjectable.InterfaceType, type),
                        _ => throw Must.NeverHappen(),
                    };
                }
                else
                {
                    _ = attr.InjectAs switch
                    {
                        InjectAs.Singleton => services.AddSingleton(type),
                        InjectAs.Transient => services.AddTransient(type),
                        _ => throw Must.NeverHappen(),
                    };
                }
            }
        }

        /// <summary>
        /// 向容器注册服务, 调用<see cref="RegisterService(ServiceCollection, Type)"/>
        /// </summary>
        /// <param name="services">容器</param>
        /// <param name="entryType">入口类型，该类型所在的程序集均会被扫描</param>
        protected virtual void RegisterServices(ServiceCollection services, Type entryType)
        {
            entryType.Assembly.ForEachType(type => RegisterService(services, type));
        }

        /// <summary>
        /// 探测服务
        /// 并向容器添加默认的 <see cref="IMessenger"/> 与 服务
        /// </summary>
        /// <param name="services">待加入的目标服务集合</param>
        protected virtual void OnProbingServices(ServiceCollection services)
        {
            // register default services
            RegisterServices(services, typeof(App));
        }

        /// <summary>
        /// 配置服务，
        /// 探测服务，
        /// <see cref="OnProbingServices(ServiceCollection)"/>会被调用以探测整个程序集，
        /// 一旦服务配置完成，就无法继续注册
        /// </summary>
        /// <returns>服务提供器</returns>
        protected IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new();
            OnProbingServices(services);
            return services.BuildServiceProvider();
        }
    }
}