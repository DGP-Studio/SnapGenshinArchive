namespace DGP.Genshin.DataModel.Material
{
    /// <summary>
    /// 武器材料
    /// </summary>
    public class Weapon : Material
    {
        /// <summary>
        /// 高塔孤王
        /// </summary>
        public const string Decarabian = "https://genshin.honeyhunterworld.com/img/i_504.png";

        /// <summary>
        /// 孤云寒林
        /// </summary>
        public const string Guyun = "https://genshin.honeyhunterworld.com/img/i_514.png";

        /// <summary>
        /// 凛风奔狼
        /// </summary>
        public const string BorealWolf = "https://genshin.honeyhunterworld.com/img/i_524.png";

        /// <summary>
        /// 雾海云间
        /// </summary>
        public const string MistVeiled = "https://genshin.honeyhunterworld.com/img/i_534.png";

        /// <summary>
        /// 狮牙斗士
        /// </summary>
        public const string DandelionGladiator = "https://genshin.honeyhunterworld.com/img/i_544.png";

        /// <summary>
        /// 漆黑陨铁
        /// </summary>
        public const string Aerosiderite = "https://genshin.honeyhunterworld.com/img/i_554.png";

        /// <summary>
        /// 远海夷地
        /// </summary>
        public const string DistantSea = "https://genshin.honeyhunterworld.com/img/i_564.png";

        /// <summary>
        /// 鸣神御灵
        /// </summary>
        public const string Narukami = "https://genshin.honeyhunterworld.com/img/i_574.png";

        /// <summary>
        /// 今昔剧画
        /// </summary>
        public const string Mask = "https://genshin.honeyhunterworld.com/img/i_584.png";

        /// <summary>
        /// 是否为今天的材料
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// 设置材料的可用性
        /// </summary>
        /// <param name="availablity">是否可用</param>
        /// <returns>当前材料</returns>
        public Weapon SetAvailability(bool availablity)
        {
            IsAvailable = availablity;
            return this;
        }
    }
}