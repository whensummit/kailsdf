using SevenStarAutoSell.Model.Web;
using SevenStarAutoSell.Models.Bet;
using SevenStarAutoSell.Models.Collect;
using SevenStarAutoSell.Models.Defs;

namespace SevenStarAutoSell.Contract
{
    /// <summary>
    /// 下注接口
    /// </summary>
    public interface IWebBetAdapter
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userLogin">登录信息</param>
        /// <returns></returns>
        UserLoginResult Login(UserLogin userLogin);

        /// <summary>
        /// 扫水
        /// </summary>
        /// <param name="content">扫水内容</param>
        /// <returns></returns>
        CollectResult CollectNumber(Collect content);

        /// <summary>
        /// 下注
        /// </summary>
        /// <param name="content">下注内容</param>
        /// <returns></returns>
        BetContentResult AddBet(BetContent content);

        /// <summary>
        /// 退码
        /// </summary>
        /// <param name="content">退码内容</param>
        /// <returns></returns>
        DeleteBetContentResult DeleteBetOrder(BetContentResult content);

        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns></returns>
        BalanceInformation CheckBalance();

        /// <summary>
        /// 网站状态
        /// </summary>
        WebStatus WebState { get; set; }

        /// <summary>
        /// 所属平台
        /// </summary>
        BetPlatformEnum Platform { get;}

        /// <summary>
        /// 登录票
        /// </summary>
        UserLoginResult LoginToken { get; }

    }
}
