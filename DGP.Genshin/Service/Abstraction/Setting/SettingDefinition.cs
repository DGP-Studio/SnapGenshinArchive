namespace DGP.Genshin.Service.Abstraction.Setting
{
    /// <summary>
    /// 设置入口定义
    /// </summary>
    /// <typeparam name="T">设置项定义</typeparam>
    public class SettingDefinition<T>
    {
        private static readonly ISettingService SettingService;

        static SettingDefinition()
        {
            SettingService = App.AutoWired<ISettingService>();
        }

        /// <summary>
        /// 构造一个新的设置对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="converter">转换器</param>
        public SettingDefinition(string name, T defaultValue, Func<object, T>? converter = null)
        {
            Name = name;
            DefaultValue = defaultValue;
            Converter = converter;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 默认值
        /// </summary>
        public T DefaultValue { get; }

        /// <summary>
        /// 转换器
        /// </summary>
        public Func<object, T>? Converter { get; }

        /// <summary>
        /// 将设置项隐式转换为值
        /// </summary>
        /// <param name="me">设置项</param>
        public static implicit operator T(SettingDefinition<T> me)
        {
            return me.Get();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns>值</returns>
        public T Get()
        {
            return SettingService.Get(this);
        }

        /// <summary>
        /// 获取非值类型的对象
        /// </summary>
        /// <param name="defaultValueFactory">默认值工厂</param>
        /// <returns>非空的对象</returns>
        [return: NotNull]
        public T GetNonValueType(Func<T> defaultValueFactory)
        {
            T obj = Get();
            obj ??= defaultValueFactory.Invoke();

            Set(obj);
            return obj!;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="log">是否记录到日志</param>
        public void Set(object? value, bool log = false)
        {
            SettingService.Set(this, value, log);
        }

        /// <summary>
        /// 提供单参数重载以便 <see cref="Snap.Core.Mvvm.ObservableObject2"/> 的通知方法调用
        /// </summary>
        /// <param name="value">值</param>
        public void Set(T value)
        {
            Set(value, false);
        }
    }
}