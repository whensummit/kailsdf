using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 单注下单数据
    /// </summary>
    public class SingleOrderResult:BaseMessage
    {
        public SingleOrderResultData Data { get; set; }

    }

    /// <summary>
    /// 单注下单返回数据
    /// </summary>
    public class SingleOrderResultData
    {
        /// <summary>
        /// 成功押注金额
        /// </summary>
        public double SuccessBet { get; set; }

        /// <summary>
        /// 成功押注数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// 押注数据
        /// </summary>
        public List<SingleResultContent> Data { get; set; }


    }

    /// <summary>
    /// 返回内容
    /// </summary>
    public class SingleResultContent
    {
        /// <summary>
        /// 下单ID
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime BetDate { get; set; }

        /// <summary>
        /// 下注明细
        /// </summary>
        public List<SingleResultContentData> List { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Stamp { get; set; }

    }

    /// <summary>
    /// 下注明细
    /// </summary>
    public class SingleResultContentData
    {
        /// <summary>
        /// 押注号码
        /// </summary>
        public int No { get; set; }
        
        /// <summary>
        /// 押注金额
        /// </summary>
        public double Bet { get; set; }

        /// <summary>
        /// 押注时的赔率
        /// </summary>
        public double Odds { get; set; }

        /// <summary>
        /// 倍率状态
        /// </summary>
        public int OddsState { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 删单ID
        /// </summary>
        public string DelID { get; set; }
    }
        
}
