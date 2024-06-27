using System;

namespace Snap.Data.Utility.Extension
{
    /// <summary>
    /// <see cref="DateTime"/>扩展
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 获取相对于此时间的上个月的最后一天
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>相对于此时间的上个月的最后一天</returns>
        public static DateTime LastDayOfLastMonth(this DateTime dateTime)
        {
            DateTime month = new(dateTime.Year, dateTime.Month, 1);
            DateTime last = month.AddDays(-1);
            return last;
        }
    }
}