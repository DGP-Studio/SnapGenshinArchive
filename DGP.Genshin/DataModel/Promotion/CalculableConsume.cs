using DGP.Genshin.MiHoYoAPI.Calculation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DGP.Genshin.DataModel.Promotion
{
    /// <summary>
    /// 可计算与消耗
    /// </summary>
    public record CalculableConsume
    {
        /// <summary>
        /// 构造一个新的 可计算与消耗 记录
        /// </summary>
        /// <param name="calculable">可计算</param>
        /// <param name="consumption">消耗</param>
        [JsonConstructor]
        public CalculableConsume(Calculable calculable, List<ConsumeItem>? consumption)
        {
            Calculable = calculable;
            ConsumeItems = consumption is null
                ? new()
                : new(consumption);
        }

        /// <summary>
        /// 构造一个新的 可计算与消耗 记录
        /// </summary>
        /// <param name="calculable">可计算</param>
        /// <param name="consumption">消耗</param>
        /// <param name="editCommand">编辑命令</param>
        /// <param name="removeCommand">移除命令</param>
        public CalculableConsume(Calculable calculable, List<ConsumeItem>? consumption, ICommand editCommand, ICommand removeCommand)
            : this(calculable, consumption)
        {
            EditCommand = editCommand;
            RemoveCommand = removeCommand;
        }

        /// <summary>
        /// 可计算的实例
        /// </summary>
        public Calculable Calculable { get; set; }

        /// <summary>
        /// 消耗物品的列表
        /// </summary>
        public ObservableCollection<ConsumeItem> ConsumeItems { get; set; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        [JsonIgnore]
        public ICommand? EditCommand { get; set; }

        /// <summary>
        /// 移除命令
        /// </summary>
        [JsonIgnore]
        public ICommand? RemoveCommand { get; set; }
    }
}