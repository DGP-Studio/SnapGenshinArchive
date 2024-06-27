using DGP.Genshin.Service.Abstraction.Setting;
using Newtonsoft.Json;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Data.Json;
using System.Collections.Concurrent;
using System.IO;

namespace DGP.Genshin.Service
{
    /// <summary>
    /// 设置服务的默认实现
    /// </summary>
    [Service(typeof(ISettingService), InjectAs.Singleton)]
    internal class SettingService : ISettingService
    {
        private readonly string settingFile = PathContext.Locate("settings.json");

        private ConcurrentDictionary<string, object?> settings = new();

        /// <inheritdoc/>
        public T Get<T>(SettingDefinition<T> definition)
        {
            string key = definition.Name;
            T defaultValue = definition.DefaultValue;
            Func<object, T>? converter = definition.Converter;

            if (!settings.TryGetValue(key, out object? value))
            {
                settings[key] = defaultValue;
                return defaultValue;
            }
            else
            {
                if (value is T tValue)
                {
                    return tValue;
                }
                else
                {
                    if (converter is null)
                    {
                        return (T)value!;
                    }
                    else
                    {
                        return converter.Invoke(value!);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Set<T>(SettingDefinition<T> definition, object? value, bool log = false)
        {
            string key = definition.Name;
            if (log)
            {
                this.Log($"setting {key} to {value} internally without notify");
            }

            settings[key] = value;
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            if (File.Exists(settingFile))
            {
                try
                {
                    settings = Json.ToObjectOrNew<ConcurrentDictionary<string, object?>>(File.ReadAllText(settingFile));

                    // only catch those exception that json file corrupted
                }
                catch (JsonReaderException)
                {
                    settings = new();
                }
            }
        }

        /// <inheritdoc/>
        public void UnInitialize()
        {
            string settingString = Json.Stringify(settings);
            this.Log(settingString);
            File.WriteAllText(settingFile, settingString);
        }
    }
}