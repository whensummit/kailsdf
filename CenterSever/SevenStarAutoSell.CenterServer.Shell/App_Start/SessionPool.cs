using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models.Defs;
using SevenStarAutoSell.Models.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.App_Start
{
    /// <summary>
    /// Session池（维护所有端的Session）
    /// <para>按客户端类型对Session分组管理</para>
    /// </summary>
    public class SessionPool
    {
        //策略端
        private static readonly ConcurrentDictionary<string, Session> StrategyClients = new ConcurrentDictionary<string, Session>();

        //出货端
        private static readonly ConcurrentDictionary<string, Session> SellClients = new ConcurrentDictionary<string, Session>();

        //扫水
        private static readonly ConcurrentDictionary<string, Session> CrawlerClients =  new ConcurrentDictionary<string, Session>();
        
        //下注
        private static readonly ConcurrentDictionary<string, Session> BetClients =  new ConcurrentDictionary<string, Session>();

        //用户端
        private static readonly ConcurrentDictionary<string, Session> Clients = new ConcurrentDictionary<string, Session>();


        /// <summary>
        /// 获取一个Session
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientType"></param>
        /// <exception cref="NotFindSessionException"></exception>
        public static Session GetByClientId(string clientId, ClientTypeEnum clientType)
        {
            return GetClientsGroup(clientType).GetByClientId(clientId);
        }

        /// <summary>
        /// 新增或者修改某个Session
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="session"></param>
        /// <exception cref="NotFindSessionException"></exception>
        public static void SetNew(string clientId, Session session)
        {
            GetClientsGroup(session.ClientType).SetNew(clientId, session);
        }

        /// <summary>
        /// 移除一个Session
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientType"></param>
        /// <exception cref="NotFindSessionException"></exception>
        public static Session RemoveByClientId(string clientId, ClientTypeEnum clientType)
        {
            return GetClientsGroup(clientType).RemoveByClientId(clientId);
        }

        /// <summary>
        /// 控盘端，根据登录名找到以前登陆的session
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Session GetDeskClientSessionIdByUserName(string userName)
        {
            return Clients.FirstOrDefault(a => a.Value.SysmemberUsername == userName).Value;
        }

        /// <summary>
        /// 如果存在未断开连接的端，则返回true；否则，返回false;
        /// </summary>
        /// <returns></returns>
        public static bool HasUnConnectedServers()
        {
            return (StrategyClients.Count > 0
                    || SellClients.Count > 0
                    || CrawlerClients.Count > 0
                    || BetClients.Count > 0);
        }

        public static ConcurrentDictionary<string, Session> GetClientsGroup(ClientTypeEnum clientType)
        {
            switch (clientType)
            {
                case ClientTypeEnum.Sell:
                    return SellClients;
                case ClientTypeEnum.Crawl:
                    return CrawlerClients;
                case ClientTypeEnum.Bet:
                    return BetClients;
                case ClientTypeEnum.Client:
                    return Clients;
                case ClientTypeEnum.Strategy:
                    return StrategyClients;
                default:
                    // 异常情况（可能是需求后期变更，未及时调整这里）
                    throw new NotFindSessionException($"未找到{clientType.ToString()}类型的Session");
            }
        }
    }

    /// <summary>
    /// Session字典扩展方法
    /// </summary>
    public static class SessionDictionaryUtil
    {
        /// <exception cref="NotFindSessionException"></exception>
        public static Session GetByClientId(this ConcurrentDictionary<string, Session> _dict, string clientId)
        {
            Session session = null;
            _dict.TryGetValue(clientId, out session);
            return session;
        }

        /// <exception cref="NotFindSessionException"></exception>
        public static void SetNew(this ConcurrentDictionary<string, Session> _dict, string clientId, Session session)
        {
            _dict.AddOrUpdate(clientId, session, (key, value) => session);
        }

        /// <exception cref="NotFindSessionException"></exception>
        public static Session RemoveByClientId(this ConcurrentDictionary<string, Session> _dict, string clientId)
        {
            Session session = null;
            _dict.TryRemove(clientId, out session);
            return session;
        }
    }
}
