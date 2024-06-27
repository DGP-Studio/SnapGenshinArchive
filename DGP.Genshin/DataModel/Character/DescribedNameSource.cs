namespace DGP.Genshin.DataModel.Character
{
    /// <summary>
    /// 带有描述信息的 名称与Url
    /// </summary>
    [SuppressMessage("", "SA1516")]
    [SuppressMessage("", "SA1600")]
    public class DescribedNameSource
    {
        public string? Name { get; set; }
        public string? Source { get; set; }
        public string? Description { get; set; }
    }
}