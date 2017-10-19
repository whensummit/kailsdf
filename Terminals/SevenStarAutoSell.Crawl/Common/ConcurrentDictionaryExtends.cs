using SevenStarAutoSell.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Crawl.Common
{
    public static class ConcurrentDictionaryExtends
    {
        private const int TryMaxCount = 5;
      

        public static void MustRemove(this ConcurrentDictionary<string, ConcurrentDictionary<int, string>> dict, string key)
        {
            if (!dict.ContainsKey(key)) return;
            ConcurrentDictionary<int, string> betResult = null;
            int tryCount = 1;
            bool removeSucess = false;
            lock (dict)
            {
                while (tryCount < TryMaxCount && !removeSucess)
                {
                    removeSucess = dict.TryRemove(key, out betResult);
                    tryCount++;
                }
            }
            if (!removeSucess)
            {
                LogUtil.Error($"betresultdict移除{key}5次失败！");
            }
        }


        public static void MustRemove(this ConcurrentDictionary<string, ConcurrentDictionary<int, bool>> dict, string key)
        {
            if (!dict.ContainsKey(key)) return;
            ConcurrentDictionary<int, bool> roundNo = null;
            int tryCount = 1;
            bool removeSucess = false;
            lock (dict)
            {
                while (tryCount < TryMaxCount && !removeSucess)
                {
                    removeSucess = dict.TryRemove(key, out roundNo);
                    tryCount++;
                }
            }
            if (!removeSucess)
            {
                LogUtil.Error($"roundNo移除{key}5次失败！");
            }
        }

        public static ConcurrentDictionary<int, bool> MustGetValue(this ConcurrentDictionary<string, ConcurrentDictionary<int, bool>> dict, string key)
        {
            if (!dict.ContainsKey(key)) return null;
            ConcurrentDictionary<int, bool> roundNo = null;
            int tryCount = 1;
            bool removeSucess = false;
            lock (dict)
            {
                while (tryCount < TryMaxCount && !removeSucess)
                {
                    removeSucess = dict.TryGetValue(key, out roundNo);
                    tryCount++;
                }
            }
            if (!removeSucess)
            {
                LogUtil.Error($"roundNo获取{key}5次失败！");
            }
            return roundNo;
        }

    }
}
