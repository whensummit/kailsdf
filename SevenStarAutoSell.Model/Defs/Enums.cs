using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.Defs
{
    public enum BetEatEnum
    {
        /// <summary>
        /// 赌
        /// </summary>
        Bet = 1,
        /// <summary>
        /// 吃
        /// </summary>
        Eat = 2
    }


    /// <summary>
    /// 玩法
    /// </summary>
    public enum PlayTypeEnum
    {
        /// <summary>
        /// 独赢
        /// </summary>
        Win = 1,
        /// <summary>
        /// 位置
        /// </summary>
        Place = 2,

        /// <summary>
        /// 连赢
        /// </summary>
        Quinella = 3,
        /// <summary>
        /// 位置Q
        /// </summary>
        PlaceQ = 4,

        /// <summary>
        /// 独赢+位置（只有长城和万利有这种投注方式，实际没这种开奖）
        /// </summary>
        Wwp = 5
    }


    /// <summary>
    /// 各客户端类型枚举（除服务中心其它各端）
    /// </summary>
    public enum ClientTypeEnum
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknow = 1,

        /// <summary>
        /// 出货端
        /// </summary>
        Sell = 2,

        /// <summary>
        /// 采集端
        /// </summary>
        Crawl = 3,

        /// <summary>
        /// 投注端
        /// </summary>
        Bet = 4,

        /// <summary>
        /// 客户控盘端
        /// </summary>
        Client = 5,

        /// <summary>
        /// 策略
        /// </summary>
        Strategy = 6
    }


    /// <summary>
    /// 优先挂单方式
    /// </summary>
    public enum EnumPriorityBetMode
    {
        /// <summary>
        /// 不挂单（一次投注）
        /// </summary>
        NoPreOrder = 0,

        // 长城
        OrderCC = 1,

        // 万利
        OrderWL = 2,

        // 等待
        Waiting = 3
    }

    /// <summary>
    /// 交易平台
    /// </summary>
    public enum BetPlatformEnum
    {
        /// <summary>
        /// 金财神
        /// </summary>
        QXDFV168 = 0,

        /// <summary>
        /// 凤凰S6
        /// </summary>
        QXS6 = 1,

        /// <summary>
        /// 1688
        /// </summary>
        QX1688 = 2,

        /// <summary>
        /// 英皇
        /// </summary>
        QXEG6 = 3,
    }

    /// <summary>
    /// 返回状态
    /// </summary>
    public enum ResultStatus
    {
        /// <summary>
        /// 失败
        /// </summary>
        Failure = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 删除中
        /// </summary>
        Deleting = 2,

        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = 3,
    }

    /// <summary>
    /// 网站状态
    /// </summary>
    public enum WebStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown=0,

        /// <summary>
        /// 开盘
        /// </summary>
        Opened=1,

        /// <summary>
        /// 关盘
        /// </summary>
        Closed=2

    }
}
