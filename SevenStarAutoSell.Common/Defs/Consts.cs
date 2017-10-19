using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Defs
{
    public class Consts
    {
        /// <summary>
        /// 最多比赛场次
        /// </summary>
        public const int MAX_ROUND = 12;

        /// <summary>
        /// 最多比赛马匹
        /// </summary>
        public const int MAX_HORSE = 14;

        /// <summary>
        /// 新万利货量默认值
        /// </summary>
        public const double DiscountMaxAmountXwl = 20000;

        /// <summary>
        /// 668货量默认值
        /// </summary>
        public const double DiscountMaxAmount668 = 20000;
        

        #region 香港赛事-极限默认值（长城极限赔率/万利极限赔率）
        //长城
        public const double CC_WIN_BET = 300;
        public const double CC_WIN_EAT = 300;

        public const double CC_PLACE_BET = 100;
        public const double CC_PLACE_EAT = 100;

        public const double CC_QUINELLA_BET = 700;
        public const double CC_QUINELLA_EAT = 700;

        public const double CC_PLACEQ_BET = 400;
        public const double CC_PLACEQ_EAT = 400;

        //万利
        public const double WL_WIN_BET = 300;
        public const double WL_WIN_EAT = 300;

        public const double WL_PLACE_BET = 100;
        public const double WL_PLACE_EAT = 100;

        public const double WL_QUINELLA_BET = 700;
        public const double WL_QUINELLA_EAT = 700;

        public const double WL_PLACEQ_BET = 400;
        public const double WL_PLACEQ_EAT = 400;

        #endregion
        

        //#region 每个投注服务端可接收最大账户数

        //public const int MAX_ACCOUNT_COUNT_PERSERVER_CC = 30;
        //public const int MAX_ACCOUNT_COUNT_PERSERVER_WL = 30;
        //public const int MAX_ACCOUNT_COUNT_PERSERVER_XWL = 30;
        //public const int MAX_ACCOUNT_COUNT_PERSERVER_668 = 30;

        //#endregion

        /// <summary>
        /// 每个投注服务可处理的最大会员数
        /// </summary>
        public const int BetServerMaxSysMembeCount = 30;
        
        /// <summary>
        /// 同一个号码最多显示N条折扣数据（长城/万利的模式中才会用到）
        /// <para>多的不需要，只需要每种玩法的最优前几条数据即可</para>
        /// </summary>
        public const int MAX_DISCOUNT_ROWCOUNT = 8;

    }
}
