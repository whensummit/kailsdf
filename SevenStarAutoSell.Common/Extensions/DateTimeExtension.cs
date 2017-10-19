using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Extensions
{
    /// <summary>
    /// 基于时间扩展
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 转换为毫秒级时间戳
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public static long ToLongTimeSpan(this DateTime datetime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = datetime;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }

        /// <summary>
        /// 转换为秒级时间戳
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public static int ToTimeSpan(this DateTime datetime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(datetime - startTime).TotalSeconds;
        }

        /// <summary>
        /// 转换成时间类
        /// </summary>
        /// <param name="timeSpan">毫秒时间戳</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timeSpan)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));          
            TimeSpan toNow = new TimeSpan(timeSpan);
            return dtStart.Add(toNow);

        }

        /// <summary>
        /// 转换成时间类
        /// </summary>
        /// <param name="timeSpan">毫秒时间戳</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this int timeSpan)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeSpan + "0000000");
            TimeSpan toNow = new TimeSpan();
            return dtStart.Add(toNow);
        }
    }
}
