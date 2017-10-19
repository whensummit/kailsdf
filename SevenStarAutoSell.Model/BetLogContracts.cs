using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models
{
    /// <summary>
    /// 投注日志
    /// </summary>
    public class BetLogItemContract
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        public DateTime RaceDate { get; set; }

        public int RoundNo { get; set; }

        public PlayTypeEnum PlayType { get; set; }

        public string HorseNo { get; set; }

        public double BetMoney { get; set; }

        public double Discount { get; set; }

        public BetEatEnum BetEat { get; set; }

        /// <summary>
        /// 极限赔率
        /// </summary>
        public double LimitMoney { get; set; }


        /// <summary>
        /// 独赢极限（只在长城/万利的“独赢+位置”模式下有值，此时不用LimitMoney）
        /// </summary>
        public double LimitMoneyWin { get; set; }

        /// <summary>
        /// 位置极限（只在长城/万利的“独赢+位置”模式下有值，此时不用LimitMoney）
        /// </summary>
        public double LimitMoneyPlace { get; set; }


        public bool IsError { get; set; }

        public string ErrorInfo { get; set; }

        /// <summary>
        /// 要投注的目标网站
        /// </summary>
        public string WebsiteName { get; set; }

        /// <summary>
        /// 投注账号
        /// </summary>
        public string AccountName { get; set; }
    }
}
