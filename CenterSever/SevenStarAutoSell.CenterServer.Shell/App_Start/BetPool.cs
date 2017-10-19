using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using SevenStarAutoSell.Models.BetPool;

namespace SevenStarAutoSell.CenterServer.Shell.App_Start
{
    /// <summary>
    /// 投注池
    /// </summary>
    public class BetPool
    {
        /// <summary>
        /// 出货池
        /// </summary>
        private static ConcurrentDictionary<int, BetPoolItem> betPool = new ConcurrentDictionary<int, BetPoolItem>();

        /// <summary>
        /// 出货池已变更
        /// </summary>
        public static event BetPoolChangedHandler BetPoolChanged;

        /// <summary>
        /// 下注信息状态变更
        /// </summary>
        public static event BetPoolChangedHandler BetStateChanged;

        /// <summary>
        /// 下注信息优先级变更
        /// </summary>
        public static event BetPoolChangedHandler BetPriorityChanged;

        /// <summary>
        /// 出货完成通知
        /// </summary>
        public static event BetPoolChangedHandler BetCompletedNotification;

        /// <summary>
        /// 将下注信息加入出货池
        /// </summary>
        /// <param name="bet">下注信息</param>
        /// <returns></returns>
        public static bool AddBetInPool(BetPoolItem bet)
        {
            betPool.TryAdd(bet.Id,bet);           
            BetPoolChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// 获取下注信息
        /// </summary>
        /// <returns>返回下注信息</returns>
        public static BetPoolItem GetBetContent()
        {
            BetPoolItem result;
            if ((result = betPool.FirstOrDefault(q =>
                                                    (q.Value.Priority == Priority.High ||
                                                     q.Value.Priority == Priority.Middle ||
                                                     q.Value.Priority == Priority.Lower)
                                                   && q.Value.BetState== BetStatus.Pending).Value) != null)
            {
                result.BetState = BetStatus.Process;
                betPool.AddOrUpdate(result.Id, result, (key, value) => result );
                BetStateChanged?.Invoke();
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 出货完成
        /// </summary>
        /// <param name="Id">下注信息ID</param>
        /// <returns></returns>
        public static bool BetSuccess(int Id)
        {
            if (betPool.ContainsKey(Id))
            {
                BetPoolItem bet;
                if (betPool.TryRemove(Id, out bet))
                {
                    BetCompletedNotification?.Invoke();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
