using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.Bet
{
    public class BetContent
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
        /// 号码类型
        /// </summary>
        public int InputType { get; set; }

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

    }
}
