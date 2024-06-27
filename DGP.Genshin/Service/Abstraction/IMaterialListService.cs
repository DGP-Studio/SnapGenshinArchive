using DGP.Genshin.DataModel.Promotion;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 材料清单服务
    /// </summary>
    public interface IMaterialListService
    {
        /// <summary>
        /// 加载并获取清单数据
        /// </summary>
        /// <returns>清单</returns>
        MaterialList Load();

        /// <summary>
        /// 使用指定的命令加载并获取清单数据
        /// </summary>
        /// <param name="editCommand">编辑命令</param>
        /// <param name="removeCommand">删除命令</param>
        /// <returns>清单</returns>
        MaterialList Load(ICommand editCommand, ICommand removeCommand);

        /// <summary>
        /// 保存清单
        /// </summary>
        /// <param name="materialList">待保存的清单</param>
        void Save(MaterialList? materialList);
    }
}