using SevenStarAutoSell.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenStarAutoSell.Model.Web;
using SevenStarAutoSell.Models.Bet;
using SevenStarAutoSell.Models.Collect;
using SevenStarAutoSell.Models.Defs;
using SevenStarAutoSell.Business.Web.Dfv168.Service;

namespace SevenStarAutoSell.Business.Web.Dfv168
{
    /// <summary>
    /// 平台实现
    /// </summary>
    public class WebAdapter : IWebBetAdapter
    {
        QiXingAdapter adp = new QiXingAdapter();

        private UserLoginResult loginToken;

        private WebStatus webStatus;

        /// <summary>
        /// 登录票
        /// </summary>
        public UserLoginResult LoginToken
        {
            get
            {
                return loginToken;
            }
        }

        public BetPlatformEnum Platform
        {
            get
            {
                return BetPlatformEnum.QXDFV168;
            }
        }

        /// <summary>
        /// 网站状态
        /// </summary>
        public WebStatus WebState
        {
            get
            {
                return webStatus;
            }

            set
            {
                webStatus = value;
            }
        }

        /// <summary>
        /// 下注
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public BetContentResult AddBet(BetContent content)
        {
            BetContentResult betResult = new BetContentResult();
            betResult.BetAccount = adp.LoginModel.LoginName;
           
            betResult.BetPlatformEnum = content.BetPlatformEnum;
            betResult.BuyerSessionID = content.BuyerSessionID;
            betResult.ClientSessionID = content.ClientSessionID;
            betResult.Domain = content.Domain;
            betResult.Id = content.Id;
            betResult.InputType = content.InputType;
            betResult.Money = content.Money;
            betResult.Number = content.Number;

            var result = adp.SendOrder(content.Number, content.Money);
            if (result != null && result.Code == 0)
            {
                betResult.BetMoney = result.Data.SuccessBet;
                betResult.BetTime = result.Data.Data.First().Stamp;
                betResult.DeleteOrderID = result.Data.Data.FirstOrDefault()?.List.FirstOrDefault()?.DelID;            
                betResult.Odds = result.Data.Data.FirstOrDefault().List.FirstOrDefault().Odds;
                betResult.OrderID = result.Data.Data.FirstOrDefault().OrderID;
                betResult.ResultStatus = ResultStatus.Success;
            }
            else
            {
                betResult.ResultStatus = ResultStatus.Failure;
            }
            return betResult;
        }

        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns></returns>
        public BalanceInformation CheckBalance()
        {
            var result = adp.GetAccountInformation();
            if (result != null)
            {
                var bi = new BalanceInformation();
                bi.Account = result.Account;
                bi.AvailablePoint = result.AvailablePoint;
                bi.Domain = result.Domain;
                bi.Platform = BetPlatformEnum.QXDFV168;
                bi.Point = result.Point;
                bi.UpdatedTime = result.UpdatedTime;
                bi.UsePoint = result.UsePoint;

                return bi;

            }
            else
            {
                //获取账户余额失败
                return null;
            }
        }

        /// <summary>
        /// 扫水
        /// </summary>
        /// <param name="content">查询内容</param>
        /// <returns></returns>
        public CollectResult CollectNumber(Collect content)
        {
            CollectResult coll = new CollectResult();

            var result = adp.SeekWater(content.Number);
            if (result != null)
            { 
                coll.ErrorMessage = "";
                coll.MaxBetMoney = result.Amount;
                coll.Odds = result.Odds;
                coll.ResultState = ResultStatus.Success;              
            }
            else
            {
                coll.ErrorMessage = "查询赔率失败";
                coll.MaxBetMoney =0;               
                coll.Odds = 0;
                coll.ResultState = ResultStatus.Failure;
            }
            coll.Id = content.Id;
            coll.BuyerSessionID = content.BuyerSessionID;
            coll.ClientSessionID = content.ClientSessionID;
            coll.Domain = adp.LoginModel.Domain;
            coll.Number = content.Number;

            return coll;
        }

        /// <summary>
        /// 退码
        /// </summary>
        /// <param name="content">退码内容</param>
        /// <returns></returns>
        public DeleteBetContentResult DeleteBetOrder(BetContentResult content)
        {
            DeleteBetContentResult del = new DeleteBetContentResult();
            del.BetAccount = content.BetAccount;
            del.BetMoney = content.BetMoney;
            del.BetPlatformEnum = content.BetPlatformEnum;
            del.BetSessionID = content.BetSessionID;
            del.BetTime = content.BetTime;
            del.BuyerSessionID = content.BuyerSessionID;
            del.ClientSessionID = content.ClientSessionID;
            del.Domain = content.Domain;
            del.Id = content.Id;
            del.Money = content.Money;
            del.Number = content.Number;
            del.Odds = content.Odds;

            var result = adp.CancelOrder(content.DeleteOrderID);

            if (result != null && result.Code == 0)
            {               
                
                del.ResultStatus = ResultStatus.Deleted;
            }
            else
            {
                del.ResultStatus = ResultStatus.Failure;
            }

            return del;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userLogin">用户登录信息</param>
        /// <returns></returns>
        public UserLoginResult Login(UserLogin userLogin)
        {
            loginToken = adp.Login(userLogin);
            return loginToken;
        }
    }
}
