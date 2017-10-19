using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.Collect
{
    /// <summary>
    /// 扫水
    /// </summary>
    public class Collect
    {
        /// <summary>
        /// 下注ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 扫水号码
        /// </summary>
        public string Number { get; set; }

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
