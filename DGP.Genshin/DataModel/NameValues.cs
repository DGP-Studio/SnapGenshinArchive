namespace DGP.Genshin.DataModel
{
    /// <summary>
    /// 带有名称的值
    /// </summary>
    /// <typeparam name="T">值的类型</typeparam>
    public class NameValues<T>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public T? Values { get; set; }
    }
}