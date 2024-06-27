using DGP.Genshin.DataModel.GachaStatistic;
using DGP.Genshin.MiHoYoAPI.Gacha;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DGP.Genshin.Service.GachaStatistic
{
    /// <summary>
    /// 包装了包含Uid与抽卡记录的字典
    /// 所有与抽卡记录相关的服务都基于对此类的操作
    /// 所有更改此集合的操作都需要切换到主线程执行
    /// </summary>
    public class GachaDataCollection : ObservableCollection<UidGachaData>
    {
        /// <summary>
        /// 获取uid对应的祈愿记录
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns>对应的祈愿记录</returns>
        public GachaData? this[string uid]
        {
            get => this.FirstOrDefault(x => x.Uid == uid)?.Data;
        }

        /// <summary>
        /// 向集合添加数据
        /// 触发uid增加事件，便于前台响应
        /// <para/>
        /// 该方法是线程不安全的 需要切换到主线程操作
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="data">祈愿记录</param>
        public void Add(string uid, GachaData data)
        {
            Add(new(uid, data));
        }

        /// <summary>
        /// 检测集合中是否包含该Uid的信息
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns>是否包含数据</returns>
        public bool HasUid(string uid)
        {
            return this.Any(x => x.Uid == uid);
        }

        /// <summary>
        /// 获取最新的时间戳id
        /// </summary>
        /// <param name="type">卡池类型</param>
        /// <param name="uid">uid</param>
        /// <returns>default 0</returns>
        public long GetNewestTimeIdOf(ConfigType type, string? uid)
        {
            string? typeId = type.Key;

            if (uid is null || typeId is null)
            {
                return 0;
            }

            // 有uid有卡池记录就读取最新物品的id,否则返回0
            if (this[uid] is GachaData matchedData)
            {
                if (matchedData.ContainsKey(typeId))
                {
                    if (matchedData[typeId] is List<GachaLogItem> item)
                    {
                        if (item.Any())
                        {
                            return item[0].TimeId;
                        }
                    }
                }
            }

            return 0;
        }
    }
}