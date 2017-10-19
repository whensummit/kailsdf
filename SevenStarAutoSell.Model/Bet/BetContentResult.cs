using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.Bet
{
    /// <summary>
    /// 下注结果
    /// </summary>
    public class BetContentResult:BetContent
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 删单ID
        /// </summary>
        public string DeleteOrderID { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        public double Odds { get; set; }

        /// <summary>
        /// 实际下单金额
        /// </summary>
        public double BetMoney { get; set; }

        /// <summary>
        /// 下单帐号
        /// </summary>
        public string BetAccount { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ResultStatus ResultStatus { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public long BetTime { get; set; }

        /// <summary>
        /// 下注客户端ID
        /// </summary>
        public string  BetSessionID{ get; set; }
    }
}
