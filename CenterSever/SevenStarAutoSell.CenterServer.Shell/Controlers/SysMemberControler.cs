using System;
using SevenStarAutoSell.CenterServer.Shell.App_Start;
using SevenStarAutoSell.CenterServer.Shell.Common;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Utils;
using SevenStarAutoSell.Models;
using SevenStarAutoSell.Models.Defs;
using SevenStarAutoSell.Models.Session;
using SevenStarAutoSell.Business.DataService;

namespace SevenStarAutoSell.CenterServer.Shell.Controlers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class SysMemberControler : IControler
    {
        /// <summary>
        /// 登陆, 如果登陆成功，带回SessionId
        /// </summary>
        /// <param name="sessionId">无效</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [NoNeedBettingStart]
        public static MQActionResult<string> Login(string sessionId, SysMemberLoginParam param)
        {
            //if (PublicData.ServerParameter.IsEmpty())
            //{
            //    return MQActionResult<string>.Error(CommonErrors.CenterServerNotStart);
            //}

            var user = SysMemberService.Instance.FindByUserName(param.UserName);
            if (user == null)
            {
                return MQActionResult<string>.Error($"没找到用户'{param.UserName}'");
            }

            if (!CryptoUtil.MD5(param.Password).Equals(user.Pwd))
            {
                return MQActionResult<string>.Error("用户名或密码不正确！");
            }

            // 检查不允许重复登陆
            if (SessionPool.GetDeskClientSessionIdByUserName(param.UserName) != null)
            {
                return MQActionResult<string>.Error($"用户'{param.UserName}'正在别处登陆，请先关闭之前的登陆账户");
            }

            // 为新的Session分配新的BetServer
            //string betServerId = AccountMapping.RandomlyAssignedABetServerId();
            //if (string.IsNullOrEmpty(betServerId))
            //{
            //    return MQActionResult<string>.Error("投注服务资源不足，无法登陆，请联系客服！");
            //}
            //Session betServerSession = SessionPool.GetByClientId(betServerId, ClientTypeEnum.Bet);
            //if (betServerSession == null)
            //{
            //    return MQActionVoidResult.Error("未找到匹配的投注端");
            //}

            // 创建Session
            sessionId = Guid.NewGuid().ToString();
            Session newession = new Session(sessionId, ClientTypeEnum.Client)
            {
                SysmemberId = user.Id,
                SysmemberUsername = user.Account,
                BetServerId ="" //betServerId
            };

            SessionPool.SetNew(sessionId, newession);

            // 为BetServerSession设置ClientDeskId
            //betServerSession.ClientDeskId = sessionId;

            SysMemberService.Instance.UpdateLoginTime(user.Id);
            return MQActionResult<string>.Ok(newession.ClientId);
        }

        // 修改密码
        [NoNeedBettingStart]
        public static MQActionVoidResult ChangePassword(string sessionId, ChangePasswordParam param)
        {
            Session session = SessionPool.GetByClientId(sessionId, ClientTypeEnum.Client);
            if (session == null)
            {
                return MQActionVoidResult.Error(CommonErrors.NotConnectingCenterServer);
            }

            string result = SysMemberService.Instance.ChangePassword(session.SysmemberId, param);

            if (result == "1")
                return MQActionVoidResult.Ok();
            else
                return MQActionVoidResult.Error(result);
        }
    }
}
