using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.Bet
{
    /// <summary>
    /// 下注帐号余额信息
    /// </summary>
    public class BalanceInformation
    {
        /// <summary>
        /// 平台
        /// </summary>
        public BetPlatformEnum Platform { get; set; }

        /// <summary>
        /// 网站地址
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 最大金额
        /// </summary>
        public double Point { get; set; }

        /// <summary>
        /// 可用金额
        /// </summary>
        public double AvailablePoint { get; set; }

        /// <summary>
        /// 已使用金额
        /// </summary>
        public double UsePoint { get; set; }

        /// <summary>
        /// 查询时间
        /// </summary>
        public long UpdatedTime { get; set; }

    }
}
