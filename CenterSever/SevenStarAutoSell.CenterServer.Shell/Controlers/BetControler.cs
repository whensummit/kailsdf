using SevenStarAutoSell.CenterServer.Shell.App_Start;
using SevenStarAutoSell.CenterServer.Shell.Common;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models.Bet;
using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Controlers
{
    /// <summary>
    /// 下注消息
    /// </summary>
    public class BetControler : IControler
    {
        #region 下注

        /// <summary>
        /// 转发下注命令到下注客户端
        /// </summary>
        /// <param name="sessionId">客户端ID</param>
        /// <param name="betContent">下注内容</param>
        /// <returns></returns>
        public static MQActionVoidResult AddBet(string sessionId, BetContent betContent)
        {
            var session = SessionPool.GetClientsGroup(ClientTypeEnum.Bet);
            if (session != null)
            {
                foreach (var item in session)
                {
                    MQRouterSendQueue.PushAddBet(item.Key, betContent);
                }
                return new MQActionVoidResult() { IsOK = true };
            }
            else
            {
                return new MQActionVoidResult() { IsOK = false, ErrorMsg = "未找到下注客户端" };
            }
        }


        /// <summary>
        /// 转发下注命令到出货客户端
        /// </summary>
        /// <param name="sessionId">下注端ID</param>
        /// <param name="betContent">下注结果信息</param>
        /// <returns></returns>
        public static MQActionVoidResult BetCompleted(string sessionId, BetContentResult betContent)
        {
            var session = SessionPool.GetClientsGroup(ClientTypeEnum.Strategy);
            if (session != null)
            {
                foreach (var item in session)
                {
                    MQRouterSendQueue.PushAddBetCompleted(item.Key, betContent);
                }
                return new MQActionVoidResult() { IsOK = true };
            }
            else
            {
                return new MQActionVoidResult() { IsOK = false, ErrorMsg = "未找到下注客户端" };
            }
        }

        #endregion

        #region 退码

        /// <summary>
        /// 转发退码命令到出货客户端
        /// </summary>
        /// <param name="sessionId">下注端ID</param>
        /// <param name="betContent">下注结果信息</param>
        /// <returns></returns>
        public static MQActionVoidResult DeleteBet(string sessionId, BetContentResult betContent)
        {
            var session = SessionPool.GetClientsGroup(ClientTypeEnum.Bet);
            if (session != null)
            {
                foreach (var item in session)
                {
                    if (item.Value.platform.Value == betContent.BetPlatformEnum)
                    {
                        MQRouterSendQueue.PushDeleteBet(item.Key, betContent);
                    }
                }
                return new MQActionVoidResult() { IsOK = true };
            }
            else
            {
                return new MQActionVoidResult() { IsOK = false, ErrorMsg = "未找到下注客户端" };
            }
        }

        /// <summary>
        /// 转发下注命令到出货客户端
        /// </summary>
        /// <param name="sessionId">下注端ID</param>
        /// <param name="betContent">下注结果信息</param>
        /// <returns></returns>
        public static MQActionVoidResult DeleteBetResult(string sessionId, DeleteBetContentResult betContent)
        {
            var session = SessionPool.GetClientsGroup(ClientTypeEnum.Strategy);
            if (session != null)
            {
                foreach (var item in session)
                {
                    MQRouterSendQueue.PushDeleteBetResult(item.Key, betContent);
                }
                return new MQActionVoidResult() { IsOK = true };
            }
            else
            {
                return new MQActionVoidResult() { IsOK = false, ErrorMsg = "未找到下注客户端" };
            }
        }

        #endregion
    }
}
