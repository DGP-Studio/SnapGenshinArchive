using Snap.Core.Logging;
using Snap.Extenion.Enumerable;
using Snap.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DGP.Genshin.Core.Plugins
{
    /// <summary>
    /// 此服务会保证插件文件夹的存在
    /// </summary>
    internal class PluginService : IPluginService
    {
        private const string PluginFolder = "Plugins";
        private readonly IEnumerable<Assembly> pluginAssemblies;
        private readonly IEnumerable<IPlugin> plugins;

        /// <summary>
        /// 构造一个新的插件服务
        /// </summary>
        public PluginService()
        {
            pluginAssemblies = LoadAllPluginDlls();
            plugins = PluginAssemblies.Select(p => InstantiatePlugin(p)).NotNull();
        }

        /// <inheritdoc/>
        public IEnumerable<Assembly> PluginAssemblies
        {
            get => pluginAssemblies;
        }

        /// <inheritdoc/>
        public IEnumerable<IPlugin> Plugins
        {
            get => plugins;
        }

        /// <inheritdoc/>
        public IPlugin? InstantiatePlugin(Assembly assembly)
        {
            Type? type = assembly.GetTypes().FirstOrDefault(type => type.Implement<IPlugin>());
            return type is null ? null : Activator.CreateInstance(type) as IPlugin;
        }

        private IEnumerable<Assembly> LoadAllPluginDlls()
        {
            // fix autorun fail issue
            string pluginPath = PathContext.Locate(PluginFolder);
            Directory.CreateDirectory(pluginPath);
            IEnumerable<string> pluginsPaths = Directory.EnumerateFiles(pluginPath, "*.dll", SearchOption.AllDirectories);
            List<Assembly> plugins = new();

            foreach (string pluginLocation in pluginsPaths)
            {
                this.Log($"Loading plugin from: {pluginLocation}");
                PluginLoadContext loadContext = new(pluginLocation);
                try
                {
                    AssemblyName pluginAssemblyName = new(Path.GetFileNameWithoutExtension(pluginLocation));
                    Assembly assembly = loadContext.LoadFromAssemblyName(pluginAssemblyName);
                    if (assembly.HasAttribute<SnapGenshinPluginAttribute>())
                    {
                        plugins.Add(assembly);
                        this.Log($"plugin : {assembly.FullName} added to plugin collection");
                    }
                }
                catch (Exception e)
                {
                    if (loadContext.IsCollectible)
                    {
                        loadContext.Unload();
                    }

                    this.Log(e);
                    this.Log($"Failed to load plugin from: {pluginLocation}");
                }
            }

            return plugins;
        }
    }
}