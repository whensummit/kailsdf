using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Utils
{
    public static class EnumUtil
    {
        public static string ToPlatformString(this BetPlatformEnum betEnum)
        {
            switch (betEnum)
            {
               
                case BetPlatformEnum.QXS6: return "凤凰S6";
                case BetPlatformEnum.QXEG6: return "英皇";
                case BetPlatformEnum.QX1688: return "1688";
                case BetPlatformEnum.QXDFV168: return "金财神";
                default: return "";
            }
        }

        // todo Matt 这个方法有点low，最好可以数据存储改为枚举名称，而不是中文名（暂缓）
        public static BetPlatformEnum ToPlatformEnum(object obj)
        {
            switch (obj.ToString())
            {

                case "1688": return BetPlatformEnum.QX1688;
                case "凤凰S6": return BetPlatformEnum.QXS6;
                case "英皇": return BetPlatformEnum.QXEG6;
                case "金财神": return BetPlatformEnum.QXDFV168;
                default: return 0;
            }
        }

        public static string ToPlayTypeString(this PlayTypeEnum playEnum)
        {
            switch (playEnum)
            {
                case PlayTypeEnum.Win: return "独赢";
                case PlayTypeEnum.Place: return "位置";
                case PlayTypeEnum.Wwp: return "独赢位置";
                case PlayTypeEnum.Quinella: return "连赢";
                case PlayTypeEnum.PlaceQ: return "位置Q";
                default: return "";
            }
        }

        public static string ToBetEatString(this BetEatEnum betEatEnum)
        {
            return betEatEnum == BetEatEnum.Bet ? "赌" :
                   betEatEnum == BetEatEnum.Eat ? "吃" : "";
        }

        public static string ToClientTypeString(this ClientTypeEnum clientType)
        {
            switch (clientType)
            {
                case ClientTypeEnum.Bet: return "下注服务端";
                case ClientTypeEnum.Client: return "万利投注软件";
                case ClientTypeEnum.Crawl: return "折扣采集端";
                case ClientTypeEnum.Unknow: return "赔率采集端";
                case ClientTypeEnum.Sell: return "赛事信息采集端";
                default: return "";
            }
        }
    }


}
