namespace DGP.Genshin.DataModel
{
    /// <summary>
    /// 键源对
    /// </summary>
    public class KeySource
    {
        /// <summary>
        /// 键
        /// </summary>
        [Obsolete("健已经没有存在的必要了")]
        public string? Key { get; set; }

        /// <summary>
        /// 源
        /// </summary>
        public string? Source { get; set; }
    }
}