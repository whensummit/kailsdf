using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Sell.Model
{
    /// <summary>
    /// 下注过程
    /// </summary>
    public class BetProcess
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BetProcess()
        {
            this.BetList = new ConcurrentDictionary<BetPlatformEnum, BetInformation>();
            this.CollectList = new ConcurrentDictionary<BetPlatformEnum, CollectInformation>();       
            this.InitCollectList();             
        }

        /// <summary>
        /// 初始化扫水列表
        /// </summary>
        private void InitCollectList()
        {
            this.CollectList.TryAdd(BetPlatformEnum.QXDFV168, null);
        }       

        /// <summary>
        /// 下单序号
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; internal set; }

        /// <summary>
        /// 下注金额
        /// </summary>
        public double Money { get; internal set; }

        /// <summary>
        /// 下注号码
        /// </summary>
        public string Number { get; internal set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperateID { get; internal set; }

        /// <summary>
        /// 出货端ID
        /// </summary>
        public string BuyerSessionID { get; internal set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientSessionID { get; internal set; }

        /// <summary>
        /// 扫水结果
        /// </summary>
        public ConcurrentDictionary<BetPlatformEnum,CollectInformation> CollectList { get; set; }

        /// <summary>
        /// 下注结果
        /// </summary>
        public ConcurrentDictionary<BetPlatformEnum,BetInformation> BetList { get; set; }

        /// <summary>
        /// 锁定
        /// </summary>
        public bool Locked { get; set; }

    }

    /// <summary>
    /// 下注信息
    /// </summary>
    public class BetInformation
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 删单ID
        /// </summary>
        public string DeleteOrderID { get; set; }

        /// <summary>
        /// 下注号码
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 下注金额
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 实际下注金额
        /// </summary>
        public double BetMoney { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        public double Odds { get; set; }

        /// <summary>
        /// 扫水端名称
        /// </summary>
        public BetPlatformEnum BetPlatform { get; set; }

        /// <summary>
        /// 下注端ID
        /// </summary>
        public string BetSessionID { get; set; }

        /// <summary>
        /// 下单帐号
        /// </summary>
        public string BetAccount { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public long OperateTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ResultStatus Status { get; set; }

    }

    /// <summary>
    /// 扫水信息
    /// </summary>
    public class CollectInformation
    {
        /// <summary>
        /// 下注号码
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 下注金额
        /// </summary>
        public double MaxBetMoney { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        public double Odds { get; set; }

        /// <summary>
        /// 扫水端名称
        /// </summary>
        public BetPlatformEnum CollectPlatform{ get; set; }

        /// <summary>
        /// 扫水端ID
        /// </summary>
        public string CollectSessionID { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public int OperateTime { get; set; }
    }
}
