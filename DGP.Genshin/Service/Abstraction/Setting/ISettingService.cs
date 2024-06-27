namespace DGP.Genshin.Service.Abstraction.Setting
{
    /// <summary>
    /// 设置服务
    /// 否则会影响已有的设置值
    /// </summary>
    public interface ISettingService
    {
        /// <summary>
        /// 使用定义获取设置值
        /// </summary>
        /// <typeparam name="T">设置项的类型</typeparam>
        /// <param name="definition">设置定义</param>
        /// <returns>设置项的值</returns>
        T Get<T>(SettingDefinition<T> definition);

        /// <summary>
        /// 初始化设置服务，加载设置数据
        /// </summary>
        void Initialize();

        /// <summary>
        /// 使用定义设置设置值
        /// </summary>
        /// <typeparam name="T">设置项的类型</typeparam>
        /// <param name="definition">定义</param>
        /// <param name="value">值</param>
        /// <param name="log">输出到日志</param>
        void Set<T>(SettingDefinition<T> definition, object? value, bool log = false);

        /// <summary>
        /// 卸载设置数据
        /// </summary>
        void UnInitialize();
    }
}