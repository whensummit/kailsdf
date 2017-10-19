using SevenStarAutoSell.CenterServer.Strategy.Model;
using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Strategy.Common
{
    /// <summary>
    /// 出货处理池
    /// </summary>
    public class BetProcessPool
    {
        /// <summary>
        /// 出货处理池
        /// </summary>
        private static ConcurrentDictionary<int, BetProcess> processPool = new ConcurrentDictionary<int, BetProcess>();

        /// <summary>
        /// 添加到出货处理池
        /// </summary>
        /// <param name="bet"></param>
        /// <returns></returns>
        public static bool AddBetInPool(BetProcess bet)
        {
            try
            {
                processPool.TryAdd(bet.Id, bet);
                return true;
            }
            catch (AggregateException exc)
            {
                //LogError(Environment.NewLine + "获取下注信息失败:" + result.ErrorMsg);
            }
            catch (OverflowException exc)
            {
                //LogError(Environment.NewLine + "获取下注信息失败:" + result.ErrorMsg);
            }
            return false;
        }

        /// <summary>
        /// 获取一条下注信息
        /// </summary>
        /// <returns></returns>
        public static BetProcess GetBetProcess()
        {
            if (processPool.Count > 0)
            {
                var bet = processPool.FirstOrDefault(q => !q.Value.Locked);
                processPool[bet.Key].Locked = true;
                return bet.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加需要处理的下注平台项
        /// </summary>
        /// <param name="id">下注处理队列ID</param>
        /// <param name="betPlatform">下注平台</param>
        public static bool AddBetListItemInBetProcess(int id, BetPlatformEnum betPlatform)
        {
            if (processPool.ContainsKey(id))
            {
                if (processPool[id].BetList.ContainsKey(betPlatform))
                {
                    //已经存在这个平台
                    return false;
                }
                else
                {
                    processPool[id].BetList.TryAdd(betPlatform, null);
                    return true;
                }
            }
            else
            {
                //处理队列不存在
                return false;
            }

        }


        /// <summary>
        /// 根据ID获取一条处理进程池中的数据
        /// </summary>
        /// <param name="id">下单ID</param>
        /// <param name="result">扫码结果</param>
        /// <returns></returns>
        public static int SetBetProcessCollect(int id, CollectInformation result)
        {
            if (processPool.ContainsKey(id))
            {
                processPool[id].CollectList[result.CollectPlatform] = result;

                var count = processPool[id].CollectList.Count(q => q.Value.Equals(null));
                return count;
            }
            else
            {
                //不存在返回-1
                return -1;
            }

        }

        /// <summary>
        /// 根据ID获取一条处理进程池中的数据
        /// </summary>
        /// <param name="id">下单ID</param>
        /// <param name="result">扫码结果</param>
        /// <returns></returns>
        public static int SetBetProcessBetContent(int id, BetInformation result)
        {
            if (processPool.ContainsKey(id))
            {
                processPool[id].BetList[result.BetPlatform] = result;

                var count = processPool[id].BetList.Count(q => q.Value.Equals(null) || q.Value.Status == ResultStatus.Deleting);

                return count;
            }
            else
            {
                //不存在返回-1
                return -1;
            }

        }

        /// <summary>
        /// 获取扫水结果
        /// </summary>
        /// <param name="id">下单ID</param>
        /// <returns></returns>
        public static IList<CollectInformation> GetCollectByID(int id)
        {
            if (processPool.ContainsKey(id))
            {
                return processPool[id].CollectList.Values?.ToList();
            }
            else
            {
                //不存在返回-1
                return null;
            }
        }

        /// <summary>
        /// 出货完成
        /// </summary>
        /// <param name="Id">下注信息ID</param>
        /// <returns></returns>
        public static bool Remove(int Id)
        {
            if (processPool.ContainsKey(Id))
            {
                BetProcess bet;
                if (processPool.TryRemove(Id, out bet))
                {                   
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
