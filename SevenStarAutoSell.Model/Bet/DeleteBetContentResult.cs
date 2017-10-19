using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.Bet
{
    /// <summary>
    /// 退码返回内容
    /// </summary>
    public class DeleteBetContentResult
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 下注号码
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 下注金额
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 下单帐号
        /// </summary>
        public string BetAccount { get; set; }

        /// <summary>
        /// 下注金额
        /// </summary>
        public double BetMoney { get; set; }


        /// <summary>
        /// 下注金额
        /// </summary>
        public double Odds { get; set; }

        /// <summary>
        /// 下注平台
        /// </summary>
        public BetPlatformEnum BetPlatformEnum { get; set; }

        /// <summary>
        /// 网站地址
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 出货端ID
        /// </summary>
        public string BuyerSessionID { get; set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientSessionID { get; set; }

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
        public string BetSessionID { get; set; }
    }
}
