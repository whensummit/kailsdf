using SevenStarAutoSell.CenterServer.Shell.App_Start;
using SevenStarAutoSell.CenterServer.Shell.Common;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Utils;
using SevenStarAutoSell.Models.Defs;
using SevenStarAutoSell.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Controlers
{
    /// <summary>
    /// 维护各个客户端的session
    /// </summary>
    internal class SessionControler : IControler
    {
        /// <summary>
        /// 某个端建立连接, 并由中心服务器返回分配的SessionId
        /// </summary>
        /// <param name="sessionId">空字符串</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static MQActionResult<string> Connect(string sessionId, ValueTypeParam<ClientTypeEnum> param)
        {
            if (param.Value == ClientTypeEnum.Client)
            {
                return MQActionResult<string>.Error("控盘端只能在登陆成功逻辑中创建Session.");
            }

            sessionId = Guid.NewGuid().ToString();
            Session session = new Session(sessionId, param.Value);
            session.platform = param.Platform;
            
            SessionPool.SetNew(sessionId, session);

            // 投注端建立链接，初始化可投注缓存数据
            if (param.Value == ClientTypeEnum.Bet)
            {
                AccountMapping.AddAvailableBetServer(sessionId);
            }

            return MQActionResult<string>.Ok(sessionId);
        }

        // 断开连接（Session/Disconnect）
        public static MQActionVoidResult Disconnect(string sessionId, ValueTypeParam<ClientTypeEnum> param)
        {
            Session session = SessionPool.RemoveByClientId(sessionId, param.Value);
            if (session != null && param.Value == ClientTypeEnum.Bet)
            {
                AccountMapping.RemoveBetServer(session);
            }
            else if (session != null && param.Value == ClientTypeEnum.Client)
            {
                AccountMapping.RemoveClients(session);
            }
            return MQActionVoidResult.Ok();
        }

        // 各个服务端发送心跳数据
        public static MQActionVoidResult Heartbeat(string sessionId, HeartbeatParam param)
        {
            Session session = SessionPool.GetByClientId(sessionId, param.ClientType);
            if (session != null)
            {
                session.Heartbeat = DateTime.Now;
                if (param.platform.HasValue)
                {
                    session.platform = param.platform;
                }
                return MQActionVoidResult.Ok();
            }
            else
            {
                return MQActionVoidResult.Error($"{param.ClientType.ToClientTypeString()}连接中心服务器超时，请关闭重启");
            }
        }
    }
}
