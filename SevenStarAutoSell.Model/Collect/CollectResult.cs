using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.Collect
{
    /// <summary>
    /// 扫水返回内容
    /// </summary>
    public class CollectResult : Collect
    {
        /// <summary>
        /// 扫水端ID
        /// </summary>
        public string CollectSessionID { get; set; }

        /// <summary>
        /// 买货公司
        /// </summary>
        public BetPlatformEnum Platform { get; set; }

        /// <summary>
        /// 网站地址
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        public double Odds { get; set; }

        /// <summary>
        /// 最大下注金额
        /// </summary>
        public double MaxBetMoney { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ResultStatus ResultState { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

    }
}
