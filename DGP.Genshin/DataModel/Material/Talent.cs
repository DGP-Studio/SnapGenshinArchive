namespace DGP.Genshin.DataModel.Material
{
    /// <summary>
    /// 天赋材料
    /// </summary>
    public class Talent : Material
    {
        /// <summary>
        /// 诗文
        /// </summary>
        public const string Ballad = "https://genshin.honeyhunterworld.com/img/i_403.png";

        /// <summary>
        /// 浮世
        /// </summary>
        public const string Transience = @"https://genshin.honeyhunterworld.com/img/i_408.png";

        /// <summary>
        /// 勤劳
        /// </summary>
        public const string Diligence = @"https://genshin.honeyhunterworld.com/img/i_413.png";

        /// <summary>
        /// 风雅
        /// </summary>
        public const string Elegance = @"https://genshin.honeyhunterworld.com/img/i_418.png";

        /// <summary>
        /// 自由
        /// </summary>
        public const string Freedom = @"https://genshin.honeyhunterworld.com/img/i_423.png";

        /// <summary>
        /// 天光
        /// </summary>
        public const string Light = "https://genshin.honeyhunterworld.com/img/i_428.png";

        /// <summary>
        /// 黄金
        /// </summary>
        public const string Gold = "https://genshin.honeyhunterworld.com/img/i_433.png";

        /// <summary>
        /// 繁荣
        /// </summary>
        public const string Prosperity = @"https://genshin.honeyhunterworld.com/img/i_443.png";

        /// <summary>
        /// 抗争
        /// </summary>
        public const string Resistance = @"https://genshin.honeyhunterworld.com/img/i_453.png";

        /// <summary>
        /// 是否为今天的材料
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// 设置材料的可用性
        /// </summary>
        /// <param name="availablity">是否可用</param>
        /// <returns>当前材料</returns>
        public Talent SetAvailability(bool availablity)
        {
            IsAvailable = availablity;
            return this;
        }
    }
}