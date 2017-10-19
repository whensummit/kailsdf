using System.Collections.Concurrent;
using NetMQ.Sockets;
using System.Threading;
using SevenStarAutoSell.Models;
using SevenStarAutoSell.Models.Defs;

namespace SevenStarAutoSell.Client.Common
{
    internal class PublicData
    {
        /// <summary>
        /// 当前客户端的SessionId
        /// </summary>
        public static string SessionId { get; set; } = "";

        public static bool CenterServerIsClosing { get; set; } = false;

        /// <summary>
        /// 客户端请求全部使用request模式发送
        /// <para>有些业务使用dealer_receive模式接收信息</para>
        /// </summary>
        public static RequestSocket RequestSocket { get; set; }

        public static CancellationTokenSource CancellationToken { get; private set; } 
            = new CancellationTokenSource();

        /// <summary>
        /// 中心服务器参数
        /// </summary>
        public static ServerParameterContract ServerParameter { get; set; } = null;
        
        /// <summary>
        /// 当前登陆的系统会员
        /// </summary>
        public static string SysmemberAccount { get; set; }

        /// <summary>
        /// 剩余未成交金额优先投注策略
        /// </summary>
        public static EnumPriorityBetMode ResidualVolumeBetModel { get; set; } = EnumPriorityBetMode.NoPreOrder;

        /// <summary>
        /// 开奖的场次
        /// </summary>
        public static ConcurrentDictionary<int, bool> OpenedResultRoundNos { get; set; } =
            new ConcurrentDictionary<int, bool>();

    }
    
}
