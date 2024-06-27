namespace DGP.Genshin.DataModel.Material
{
    /// <summary>
    /// 材料
    /// </summary>
    public class Material : Primitive
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// 不实现此方法
        /// </summary>
        /// <returns><see langword="null"/></returns>
        public override string? GetBadge()
        {
            return null;
        }
    }
}