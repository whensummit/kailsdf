using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 扫水查询结果
    /// </summary>
    public class QueryInformation
    {
        /// <summary>
        /// 号码
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 倍率
        /// </summary>
        public double Odds { get; set; }

        /// <summary>
        /// 未知
        /// </summary>
        public int Change { get; set; }

        /// <summary>
        /// 最大下注金额
        /// </summary>
        public double Amount { get; set; }
    }
}
