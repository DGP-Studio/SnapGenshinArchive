namespace DGP.Genshin.Core.Background.Xunkong
{
    internal record XunkongResponse<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T? Data { get; set; }
    }
}