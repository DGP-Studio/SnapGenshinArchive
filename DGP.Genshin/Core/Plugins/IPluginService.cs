using System.Collections.Generic;
using System.Reflection;

namespace DGP.Genshin.Core.Plugins
{
    /// <summary>
    /// 插件服务抽象
    /// </summary>
    internal interface IPluginService
    {
        /// <summary>
        /// 此处的程序集可能包括了不含插件实现的 污染程序集
        /// </summary>
        IEnumerable<Assembly> PluginAssemblies { get; }

        /// <summary>
        /// 插件
        /// </summary>
        IEnumerable<IPlugin> Plugins { get; }

        /// <summary>
        /// 实例化插件
        /// </summary>
        /// <param name="assembly">插件所在的程序集</param>
        /// <returns>插件主类的实例</returns>
        IPlugin? InstantiatePlugin(Assembly assembly);
    }
}