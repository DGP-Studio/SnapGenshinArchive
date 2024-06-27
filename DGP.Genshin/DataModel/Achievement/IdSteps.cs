using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DGP.Genshin.DataModel.Achievement
{
    /// <summary>
    /// Id分步信息
    /// </summary>
    public class IdSteps
    {
        /// <summary>
        /// 构造一个新的 Id分步信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="steps">分步信息</param>
        [JsonConstructor]
        public IdSteps(int id, List<bool>? steps)
        {
            Id = id;
            Steps = steps;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public List<bool>? Steps { get; set; }
    }
}