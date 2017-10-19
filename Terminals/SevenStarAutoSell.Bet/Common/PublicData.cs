﻿using NetMQ.Sockets;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models;
using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Bet.Common
{
    class PublicData
    {
        static PublicData()
        {
            List<BetPlatformEnum> allWebsite =
                new List<BetPlatformEnum>()
                {
                    BetPlatformEnum.QX1688,
                    BetPlatformEnum.QXDFV168,
                    BetPlatformEnum.QXEG6,
                    BetPlatformEnum.QXS6
                };
            foreach (var item in allWebsite)
            {
              
                DicAccountRoundNo[item] = new ConcurrentDictionary<string, ConcurrentDictionary<int, bool>>();
                DicAccountBetResult[item] = new ConcurrentDictionary<string, ConcurrentDictionary<int, string>>();
            }
        }


        /// <summary>
        /// DealerSend消息池
        /// </summary>
        public static BlockingCollection<KeyData> DealerSendQueue { get; } =
            new BlockingCollection<KeyData>(new ConcurrentQueue<KeyData>());

        /// <summary>
        /// 当前客户端的SessionId
        /// </summary>
        public static string SessionId { get; set; } = "";

        public static CancellationTokenSource CancellationToken { get; private set; } = new CancellationTokenSource();

        /// <summary>
        /// 客户端请求全部使用request模式发送
        /// <para>有些业务使用dealer_receive模式接收信息</para>
        /// </summary>
        public static RequestSocket RequestSocket { get; set; }

        public static ServerParameterContract ServerParameter { get; set; } = null;


     




        //[网站类型][账号][场次][交易对象]
        public static ConcurrentDictionary<BetPlatformEnum, ConcurrentDictionary<string, ConcurrentDictionary<int, string>>> DicAccountBetResult { get; } =
            new ConcurrentDictionary<BetPlatformEnum, ConcurrentDictionary<string, ConcurrentDictionary<int, string>>>();

        //[网站][账号][场次][是否采集]
        public static ConcurrentDictionary<BetPlatformEnum, ConcurrentDictionary<string, ConcurrentDictionary<int, bool>>> DicAccountRoundNo { get; } =
            new ConcurrentDictionary<BetPlatformEnum, ConcurrentDictionary<string, ConcurrentDictionary<int, bool>>>();
    }
}
