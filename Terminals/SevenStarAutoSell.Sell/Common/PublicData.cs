using NetMQ.Sockets;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Sell.Common
{
    class PublicData
    {
        static PublicData()
        {
           
        }


        /// <summary>
        /// DealerSend消息池
        /// </summary>
        public static BlockingCollection<KeyData> DealerSendQueue { get; } = new BlockingCollection<KeyData>(new ConcurrentQueue<KeyData>());

        /// <summary>
        /// 当前客户端的SessionId
        /// </summary>
        public static string SessionId { get; set; } = "";

        /// <summary>
        /// 取消线程令牌
        /// </summary>
        public static CancellationTokenSource CancellationToken { get; private set; } = new CancellationTokenSource();

        /// <summary>
        /// 客户端请求全部使用request模式发送
        /// <para>有些业务使用dealer_receive模式接收信息</para>
        /// </summary>
        public static RequestSocket RequestSocket { get; set; }

        public static ServerParameterContract ServerParameter { get; set; } = null;
        
    }
}
