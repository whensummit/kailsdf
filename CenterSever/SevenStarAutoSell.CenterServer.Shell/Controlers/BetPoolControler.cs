using SevenStarAutoSell.CenterServer.Shell.App_Start;
using SevenStarAutoSell.CenterServer.Shell.Common;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models.BetPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Controlers
{
    /// <summary>
    /// 出货池管理
    /// </summary>
    public class BetPoolControler : IControler
    {
        /// <summary>
        /// 添加下注信息到出货池
        /// </summary>
        /// <param name="sessionId">客户端ID</param>
        /// <param name="bet">下注</param>
        /// <returns></returns>
        public static MQActionResult<string> AddBetInPool(string sessionId, BetPoolItem bet)
        {
            MQActionResult<string> result = new MQActionResult<string>();

            if (BetPool.AddBetInPool(bet))
            {
                result.Data = "添加成功";
                result.ErrorMsg = "";
                result.IsOK = true;
                return result;
            }
            else
            {
                result.Data = "添加失败,请重试！";
                result.ErrorMsg = "添加失败,请重试！";
                result.IsOK = false;
                return result;
            }            
        }

        /// <summary>
        /// 获取下注信息
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static MQActionResult<BetPoolItem> GetBetInPool(string sessionId)
        {
            MQActionResult<BetPoolItem> result = new MQActionResult<BetPoolItem>();

            var bet = BetPool.GetBetContent();
            if (bet != null)
            {
                result.Data = bet;
                result.ErrorMsg = "";
                result.IsOK = true;
                return result;
            }
            else
            {
                result.Data = null;
                result.ErrorMsg = "未找到下注信息";
                result.IsOK = false;
                return result;
            }           
        }

        /// <summary>
        /// 下注完成
        /// </summary>
        /// <param name="sessionId">客户端ID</param>
        /// <param name="betCompleted">信息</param>
        /// <returns></returns>
        public static MQActionVoidResult BetCompleted(string sessionId, BetPoolItem betCompleted)
        {
            if (BetPool.BetSuccess(betCompleted.Id))
            {
                return new MQActionVoidResult() { IsOK = true };
            }
            else
            {
                return new MQActionVoidResult() { IsOK = false, ErrorMsg="操作失败,请重试" };
            }

        }
        
    }
}
