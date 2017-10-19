using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.Session
{
    /// <summary>
    /// 心跳数据参数
    /// </summary>
    public class HeartbeatParam
    {
        public ClientTypeEnum ClientType { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public BetPlatformEnum? platform { get; set; }      
    }
}
