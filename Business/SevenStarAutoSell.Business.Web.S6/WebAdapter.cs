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

namespace SevenStarAutoSell.Business.Web.S6
{
    public class WebAdapter : IWebBetAdapter
    {

        private UserLoginResult loginToken;

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
                return BetPlatformEnum.QXS6;
            }
        }

        public WebStatus WebState
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public BetContentResult AddBet(BetContent content)
        {
            throw new NotImplementedException();
        }

        public BalanceInformation CheckBalance()
        {
            throw new NotImplementedException();
        }

        public CollectResult CollectNumber(Collect content)
        {
            throw new NotImplementedException();
        }

        public DeleteBetContentResult DeleteBetOrder(BetContentResult content)
        {
            throw new NotImplementedException();
        }

        public UserLoginResult Login(UserLogin userLogin)
        {
            throw new NotImplementedException();
        }
    }
}
