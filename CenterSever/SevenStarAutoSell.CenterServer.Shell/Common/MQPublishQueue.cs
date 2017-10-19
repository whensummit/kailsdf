
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Common
{
    /// <summary>
    /// 统一处理MQ广播信息
    /// </summary>
    class MQPublishQueue
    {
        private static BlockingCollection<KeyData> _publishQueue { get; } = new BlockingCollection<KeyData>(new ConcurrentQueue<KeyData>());

        internal static bool TryTake(out KeyData result)
        {
            return _publishQueue.TryTake(out result, -1);
        }

        // 开始投注
        internal static void StartCenterServer()
        {
            _publishQueue.Add(KeyData.Create("StartCenterServer"));
        }

        // 关闭中心服务器
        internal static void CloseCenterServer()
        {
            _publishQueue.Add(KeyData.Create("CloseCenterServer"));
        }

        // 修改服务器参数（投注URL等信息）
        internal static void ServerParameterPushNew(ServerParameterContract newServerParam)
        {
            _publishQueue.Add(KeyData.Create("ServerParameterPushNew", newServerParam));
        }


        // 推送新的开奖结果
        internal static void PushNewLotteryResult(LotteryResultContract lottryResult)
        {
            _publishQueue.Add(KeyData.Create("PushNewLotteryResult", lottryResult));
        }
    }
}
