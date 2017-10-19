using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 基础消息
    /// </summary>
    public class BaseMessage
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        public string Redirect { get; set; }
    }
}
