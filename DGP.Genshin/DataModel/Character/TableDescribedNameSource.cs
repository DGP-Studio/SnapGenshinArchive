using System.Collections.Generic;

namespace DGP.Genshin.DataModel.Character
{
    /// <summary>
    /// 带表格的 <see cref="DescribedNameSource"/>
    /// </summary>
    public class TableDescribedNameSource : DescribedNameSource
    {
        /// <summary>
        /// 表格
        /// </summary>
        public List<NameValues<SkillStatValues>>? Table { get; set; }
    }
}