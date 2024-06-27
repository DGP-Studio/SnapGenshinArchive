using DGP.Genshin.DataModel.Promotion;
using DGP.Genshin.Service.Abstraction;
using Snap.Core.DependencyInjection;
using Snap.Data.Json;
using Snap.Extenion.Enumerable;

namespace DGP.Genshin.Service
{
    /// <inheritdoc cref="IMaterialListService"/>
    [Service(typeof(IMaterialListService), InjectAs.Transient)]
    internal class MaterialListService : IMaterialListService
    {
        private const string MaterialListFileName = "MaterialList.json";

        /// <inheritdoc/>
        public MaterialList Load()
        {
            return Json.FromFileOrNew<MaterialList>(PathContext.Locate(MaterialListFileName));
        }

        /// <inheritdoc/>
        public MaterialList Load(ICommand editCommand, ICommand removeCommand)
        {
            MaterialList list = Load();
            list.ForEach(item =>
            {
                item.EditCommand = editCommand;
                item.RemoveCommand = removeCommand;
            });
            return list;
        }

        /// <inheritdoc/>
        public void Save(MaterialList? materialList)
        {
            Json.ToFile(MaterialListFileName, materialList);
        }
    }
}