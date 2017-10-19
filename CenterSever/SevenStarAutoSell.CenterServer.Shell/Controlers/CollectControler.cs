using SevenStarAutoSell.CenterServer.Shell.App_Start;
using SevenStarAutoSell.CenterServer.Shell.Common;
using SevenStarAutoSell.Common.Defs;

using SevenStarAutoSell.Models.Collect;
using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Controlers
{
    /// <summary>
    /// 扫水消息转发
    /// </summary>
    public class CollectControler : IControler
    {
        #region 扫水

        /// <summary>
        /// 转发扫水命令到扫水客户端
        /// </summary>
        /// <param name="sessionId">命令发送端</param>
        /// <param name="betContent">扫水内容</param>
        /// <returns></returns>
        public static MQActionVoidResult PushCollect(string sessionId, Collect content)
        {
            var session = SessionPool.GetClientsGroup(ClientTypeEnum.Crawl);
            if (session != null)
            {
                foreach (var item in session)
                {
                    MQRouterSendQueue.PushCollect(item.Key, content);
                }

                return new MQActionVoidResult() { IsOK = true };
            }
            else
            {
                return new MQActionVoidResult() { IsOK = false, ErrorMsg = "未找到扫水客户端" };
            }
        }

        /// <summary>
        /// 转发扫水命令到扫水客户端
        /// </summary>
        /// <param name="sessionId">命令发送端</param>
        /// <param name="betContent">扫水内容</param>
        /// <returns></returns>
        public static MQActionVoidResult SendCollect(string sessionId, Collect content)
        {
            var session = SessionPool.GetClientsGroup(ClientTypeEnum.Crawl);
            if (session != null)
            {
                foreach (var item in session)
                {
                    MQRouterSendQueue.SendCollect(item.Key, content);
                }

                return new MQActionVoidResult() { IsOK = true };
            }
            else
            {
                return new MQActionVoidResult() { IsOK = false, ErrorMsg = "未找到扫水客户端" };
            }
        }


        /// <summary>
        /// 转发扫水结果到客户端
        /// </summary>
        /// <param name="sessionId">发送端</param>
        /// <param name="content">扫水结果</param>
        /// <returns></returns>
        public static MQActionVoidResult CollectCompletedToClient(string sessionId, CollectResult content)
        {

            var session = SessionPool.GetClientsGroup(ClientTypeEnum.Client);
            if (session != null)
            {
                foreach (var item in session)
                {
                    if(item.Key==content.ClientSessionID)
                    { 
                        MQRouterSendQueue.PushCollectCompleted(item.Key, content);
                        break;
                    }
                }

                return new MQActionVoidResult() { IsOK = true };
            }
            else
            {
                return new MQActionVoidResult() { IsOK = false, ErrorMsg = "未找到出货客户端" };
            }
        }


        /// <summary>
        /// 转发扫水完成消息
        /// </summary>
        /// <param name="sessionId">发送端</param>
        /// <param name="content">扫水结果</param>
        /// <returns></returns>
        public static MQActionVoidResult CollectCompleted(string sessionId, CollectResult content)
        {

            var session = SessionPool.GetClientsGroup(ClientTypeEnum.Strategy);
            if (session != null)
            {
                foreach (var item in session)
                {
                    MQRouterSendQueue.PushCollectCompleted(item.Key, content);
                }

                return new MQActionVoidResult() { IsOK = true };
            }
            else
            {
                return new MQActionVoidResult() { IsOK = false, ErrorMsg = "未找到出货客户端" };
            }
        }


        #endregion

    }
}
