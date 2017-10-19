using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.BetPool
{
    /// <summary>
    /// 出货状态
    /// </summary>
    public enum BetStatus
    {
        /// <summary>
        /// 出货中
        /// </summary>
        Process=0,

        /// <summary>
        /// 等待出货
        /// </summary>
        Pending=1,
    }
}
