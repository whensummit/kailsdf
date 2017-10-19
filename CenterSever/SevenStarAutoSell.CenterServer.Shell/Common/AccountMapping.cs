using SevenStarAutoSell.CenterServer.Shell.App_Start;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenStarAutoSell.Models.Session;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models.Defs;

namespace SevenStarAutoSell.CenterServer.Shell.Common
{
    /// <summary>
    /// 与投注账号相关的数据（管理三方映射关系：账号，控盘端，投注端）
    /// <para>1.可登陆服务端列表；</para>
    /// <para>2.已登陆账号服务端注册表；</para>
    /// <para>3.已登陆账号控盘端注册表；</para>
    /// <para>4.已登陆账号的投注结果；</para>
    /// </summary>
    class AccountMapping
    {
        static AccountMapping()
        {
           
        }

        #region 可登陆投注端数量“投注端-可用数量”映射

        // [BetServerSessionId][剩余可用会员数]
        private static readonly ConcurrentDictionary<string, int> AvailableBetServers = new ConcurrentDictionary<string, int>();

        public static void AddAvailableBetServer(string sessionId)
        {
            if (AvailableBetServers.ContainsKey(sessionId) == false)
            {
                AvailableBetServers[sessionId] = Consts.BetServerMaxSysMembeCount;
            }
        }

        /// <summary>
        /// 采用负载均衡分配原则，分配一个投注服务端资源
        /// </summary>
        /// <returns></returns>
        public static string RandomlyAssignedABetServerId()
        {
            lock (AvailableBetServers)
            {
                int tempMaxCount = 0;
                string resultBetServerId = "";
                foreach (string key in AvailableBetServers.Keys)
                {
                    if (AvailableBetServers[key] > tempMaxCount)
                    {
                        tempMaxCount = AvailableBetServers[key];
                        resultBetServerId = key;
                    }
                }

                return resultBetServerId;
            }
        }

        #endregion

        #region 移除投注端和控盘端映射

        /// <summary>
        /// 投注端退出了，注销资源，通知他的控盘端下线
        /// </summary>
        public static void RemoveBetServer(Session session)
        {
            if (AvailableBetServers.ContainsKey(session.ClientId))
            {
                int value;
                AvailableBetServers.TryRemove(session.ClientId, out value);
            }

            // 通知该服务端影响到的一批客户端下线
            var allClients = SessionPool.GetClientsGroup(ClientTypeEnum.Client);
            if (allClients.Count == 0)
            {
                return;
            }
            var clientIds = allClients.Keys.ToArray();
            foreach (var clientId in clientIds)
            {
                if (allClients[clientId].BetServerId == session.ClientId)
                {
                    MQRouterSendQueue.PushBetServerClose(clientId);
                }
            }
        }

        /// <summary>
        /// 客户端注销
        /// </summary>
        /// <param name="session"></param>
        /// <param name="oldUserName"></param>
        public static void RemoveClients(Session session, string oldUserName = null)
        {
            string betServerId = session.BetServerId;
            if (string.IsNullOrEmpty(betServerId) || SessionPool.GetByClientId(betServerId, ClientTypeEnum.Bet) == null)
            {
                return;
            }

            //// 通知投注端清理投注账号的资源
            //List<WebsiteAccountDetail> websiteAccounts =
            //    WebsiteAccountService.Instance.GetAccountListByMemberId(session);
            //if (websiteAccounts != null && websiteAccounts.Count > 0)
            //{
            //    foreach (var account in websiteAccounts)
            //    {
            //        MQRouterSendQueue.QuitOut(betServerId, new WebsiteAccountContract()
            //        {
            //            WebsiteEnum = account.WebsiteEnum,
            //            UserName = account.UserName
            //        });
            //    }
            //}

            // 释放一个控盘端可用额
            if (AvailableBetServers.ContainsKey(betServerId))
            {
                AvailableBetServers[betServerId] -= 1;
            }
        }

        #endregion


    
    }
}
