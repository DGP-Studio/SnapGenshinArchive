using DGP.Genshin.DataModel.Achievement;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DGP.Genshin.Service.Abstraction.Achievement
{
    /// <summary>
    /// 成就服务
    /// </summary>
    public interface IAchievementService
    {
        /// <summary>
        /// 从本地读取已完成的项目数据
        /// </summary>
        /// <returns>已完成的项目列表</returns>
        List<IdTime> GetCompletedItems();

        /// <summary>
        /// 从本地读取已完成的
        /// </summary>
        /// <returns>分步状态</returns>
        List<IdSteps> GetCompletedSteps();

        /// <summary>
        /// 保存完成的项目与完成的步骤
        /// </summary>
        /// <param name="achievements">成就列表</param>
        void SaveItems(ObservableCollection<DataModel.Achievement.Achievement> achievements);

        /// <summary>
        /// 尝试以指定的源与指定的路径获取导入数据
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="argument">参数</param>
        /// <returns>可导入的成就数据</returns>
        IEnumerable<IdTime>? TryGetImportData(ImportAchievementSource source, string argument);

        /// <summary>
        /// 从字符串中获取导入的数据
        /// </summary>
        /// <param name="dataString">数据字符串</param>
        /// <returns>可导入的成就数据</returns>
        IEnumerable<IdTime>? TryGetImportData(string dataString);
    }
}