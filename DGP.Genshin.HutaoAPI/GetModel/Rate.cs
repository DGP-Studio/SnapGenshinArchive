namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 比率
    /// </summary>
    /// <typeparam name="T"><see cref="Id"/> 的类型</typeparam>
    public record Rate<T>
    {
        /// <summary>
        /// 表示唯一标识符的实例
        /// </summary>
        public T? Id { get; set; }

        /// <summary>
        /// 比率
        /// </summary>
        public double Value { get; set; }
    }
}