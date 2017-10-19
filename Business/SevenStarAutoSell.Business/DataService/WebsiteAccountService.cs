using SevenStarAutoSell.Common.Funcs;
using SevenStarAutoSell.Models.Session;
using System;

namespace SevenStarAutoSell.Business.DataService
{
    class WebsiteAccountService : Singleton<WebsiteAccountService>
    {
        
        /// <summary>
        /// 删除配置账号
        /// </summary>
        /// <param name="session"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public string DeleteAccount(Session session, int accountId)
        {          
            return "";
        }

        /// <summary>
        /// 根据登陆ID和网站获取其配置数据
        /// </summary>
        /// <param name="memberId">登陆ID</param>
        /// <param name="websiteName">网站名</param>
        /// <returns></returns>
        private string GetAccountData(int memberId, string websiteName)
        {
            return "";
        }

        /// <summary>
        /// 获取登陆用户的配置账号数
        /// </summary>
        /// <param name="memberId">登陆用户ID</param>
        /// <returns></returns>
        private int GetAccountCount(int memberId)
        {
            return -1;
        }

        /// <summary>
        /// object—string
        /// </summary>
        /// <param name="dbData"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string ConvertToString(object dbData, string defaultValue)
        {
            if (dbData == DBNull.Value)
                return defaultValue;
            return dbData.ToString();
        }

    }
}
