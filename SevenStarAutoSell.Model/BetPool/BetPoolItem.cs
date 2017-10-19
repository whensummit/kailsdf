using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.BetPool
{
    /// <summary>
    ///出货信息
    /// </summary>
    public class BetPoolItem
    {
        /// <summary>
        /// 出货Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 出货号码
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 出货金额
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 出货时间
        /// </summary>
        public int CreateTime { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int InputType { get; set; }

        /// <summary>
        /// 出货公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 出货公司ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 出货端ID
        /// </summary>
        public string BuyerSessionID { get; set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientSessionID { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public Priority Priority { get; set; }

        /// <summary>
        /// 出货状态
        /// </summary>
        public BetStatus BetState { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperateID { get; set; }
    }
}
