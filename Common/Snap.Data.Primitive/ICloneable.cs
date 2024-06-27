namespace Snap.Data.Primitive
{
    /// <summary>
    /// 指示该对象可克隆到指定类型
    /// </summary>
    /// <typeparam name="T">指定的类型，通常是自身</typeparam>
    public interface ICloneable<T>
    {
        /// <summary>
        /// 返回一个克隆的新对象
        /// </summary>
        /// <returns>一个克隆的新对象</returns>
        T Clone();
    }
}