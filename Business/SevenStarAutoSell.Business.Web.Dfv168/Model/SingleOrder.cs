using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 单注下单
    /// </summary>
    public class SingleOrder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleOrder()
        {
            this.Flags = 0;
        }

        /// <summary>
        /// 押注号码
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 押注金额
        /// </summary>
        public double Bet { get; set; }

        /// <summary>
        /// 标识？
        /// </summary>
        public int Flags { get; set; } 

    }
}
