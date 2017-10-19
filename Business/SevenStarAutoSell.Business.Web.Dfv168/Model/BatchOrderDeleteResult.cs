using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 批量删除下注单
    /// </summary>
    public class BatchOrderDeleteResult:BaseMessage
    {
        /// <summary>
        /// 删除的订单ID
        /// </summary>
        public string Data { get; set; }
    }
}
