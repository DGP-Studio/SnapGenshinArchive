namespace DGP.Genshin.DataModel.Character
{
    /// <summary>
    /// 命座信息
    /// </summary>
    [SuppressMessage("", "SA1516")]
    [SuppressMessage("", "SA1600")]
    public class Constellation
    {
        public DescribedNameSource? Constellation1 { get; set; }
        public DescribedNameSource? Constellation2 { get; set; }
        public DescribedNameSource? Constellation3 { get; set; }
        public DescribedNameSource? Constellation4 { get; set; }
        public DescribedNameSource? Constellation5 { get; set; }
        public DescribedNameSource? Constellation6 { get; set; }
    }
}