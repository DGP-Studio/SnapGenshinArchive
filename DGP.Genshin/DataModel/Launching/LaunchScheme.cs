﻿namespace DGP.Genshin.DataModel.Launching
{
    /// <summary>
    /// 启动方案
    /// </summary>
    public record LaunchScheme
    {
        /// <summary>
        /// 构造一个新的启动方案
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="channel">通道</param>
        /// <param name="cps">通道描述字符串</param>
        /// <param name="subChannel">子通道</param>
        public LaunchScheme(string name, string channel, string cps, string subChannel)
        {
            Name = name;
            Channel = channel;
            CPS = cps;
            SubChannel = subChannel;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 通道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 通道描述字符串
        /// </summary>
        public string CPS { get; set; }

        /// <summary>
        /// 子通道
        /// </summary>
        public string SubChannel { get; set; }

        /// <summary>
        /// 获取方案类型
        /// </summary>
        /// <returns>方案类型</returns>
        public SchemeType GetSchemeType()
        {
            return (Channel, SubChannel) switch
            {
                ("1", "1") => SchemeType.Officical,
                ("14", "0") => SchemeType.Bilibili,
                ("1", "0") => SchemeType.Mihoyo,
                _ => throw Must.NeverHappen(),
            };
        }
    }
}