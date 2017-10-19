using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models.Bet;
using SevenStarAutoSell.Models.Collect;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Common
{
    /// <summary>
    /// 统一处理RouterSend信息
    /// 
    /// <para>(例如：控盘端登陆命令->中心服务器->下注端登陆->返回状态给中心服务器->中心服务器返回状态数据给控盘端)</para>
    /// </summary>
    class MQRouterSendQueue
    {
        private static BlockingCollection<IdKeyData> _routerSendQueue { get; } = new BlockingCollection<IdKeyData>(new ConcurrentQueue<IdKeyData>());

        internal static bool TryTake(out IdKeyData result)
        {
            return _routerSendQueue.TryTake(out result, -1);
        }

        /// <summary>
        /// 推送扫水
        /// </summary>
        /// <param name="clientSessionId">扫水端ID</param>
        /// <param name="content">扫水内容</param>
        internal static void PushCollect(string clientSessionId, Collect content)
        {
            _routerSendQueue.Add(IdKeyData.Create(clientSessionId, "PushCollect", content));
        }

        /// <summary>
        /// 发送扫水命令到扫水端
        /// </summary>
        /// <param name="clientSessionId">扫水端ID</param>
        /// <param name="content">扫水内容</param>
        internal static void SendCollect(string clientSessionId, Collect content)
        {
            _routerSendQueue.Add(IdKeyData.Create(clientSessionId, "SendCollect", content));
        }


        /// <summary>
        /// 推送扫水完成
        /// </summary>
        /// <param name="clientSessionId">出货端ID</param>
        /// <param name="content">扫水结果</param>
        internal static void PushCollectCompleted(string clientSessionId, CollectResult content)
        {
            _routerSendQueue.Add(IdKeyData.Create(clientSessionId, "PushCollectCompleted", content));
        }

        /// <summary>
        /// 推送下注
        /// </summary>
        /// <param name="clientSessionId"></param>
        /// <param name="content"></param>
        internal static void PushAddBet(string clientSessionId, BetContent content)
        {
            _routerSendQueue.Add(IdKeyData.Create(clientSessionId, "PushAddBet", content));
        }

        /// <summary>
        /// 推送下注完成
        /// </summary>
        /// <param name="clientSessionId"></param>
        /// <param name="content"></param>
        internal static void PushAddBetCompleted(string clientSessionId, BetContentResult content)
        {
            _routerSendQueue.Add(IdKeyData.Create(clientSessionId, "PushAddBetCompleted", content));
        }

        /// <summary>
        /// 推送退码到下注端
        /// </summary>
        /// <param name="clientSessionId"></param>
        /// <param name="content"></param>
        internal static void PushDeleteBet(string clientSessionId, BetContentResult content)
        {
            _routerSendQueue.Add(IdKeyData.Create(clientSessionId, "PushDeleteBet", content));
        }


        /// <summary>
        /// 推送退码成功到策略端
        /// </summary>
        /// <param name="clientSessionId"></param>
        /// <param name="content"></param>
        internal static void PushDeleteBetResult(string clientSessionId, DeleteBetContentResult content)
        {
            _routerSendQueue.Add(IdKeyData.Create(clientSessionId, "PushDeleteBetResult", content));
        }



        // 投注端关闭了，通知相应客户端
        internal static void PushBetServerClose(string clientSessionId)
        {
            _routerSendQueue.Add(IdKeyData.Create(clientSessionId, "PushBetServerClose"));
        }
    }
}
