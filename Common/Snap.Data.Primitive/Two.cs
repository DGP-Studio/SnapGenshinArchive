namespace Snap.Data.Primitive
{
    /// <summary>
    /// 包装两个相同类型的对象
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <remarks>不便于使用元组时可以使用此类</remarks>
    public class Two<T>
    {
        /// <summary>
        /// 构造一个新的双者实例
        /// </summary>
        /// <param name="first">第一个值</param>
        /// <param name="second">第二个值</param>
        public Two(T first, T second)
        {
            First = first;
            Second = second;
        }

        /// <summary>
        /// 第一个对象
        /// </summary>
        public T First { get; init; }

        /// <summary>
        /// 第二个对象
        /// </summary>
        public T Second { get; init; }
    }
}