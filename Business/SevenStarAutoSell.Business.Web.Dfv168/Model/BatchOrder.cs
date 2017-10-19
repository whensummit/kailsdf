using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 批量下单
    /// </summary>
    public class BatchOrder
    {
        /// <summary>
        /// 押注号码
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 押注金额
        /// </summary>
        public int Bet { get; set; }
    }

    /// <summary>
    /// 批量下单
    /// </summary>
    public class BatchOrders
    {
        private ItemType itemId;

        /// <summary>
        /// 批量下单构造函数
        /// </summary>
        public BatchOrders()
        {
            this.IsBatch = false;            
        }



        /// <summary>
        /// API的地址
        /// </summary>
        public string ApiAddress { get; set; }

        /// <summary>
        /// 引用页
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 产品ID 如果ItemID为负数，忽略ItemID和IsBatch
        /// </summary>
        public ItemType ItemID
        {
            get
            {
                return itemId;
            }
            set
            {
                if (itemId != value)
                {
                    itemId = value;
                    switch ((int)itemId)
                    {
                        case -1:
                            this.ApiAddress = "http:{0}/api/FrontC0207/OrderAdd";
                            this.Referer = "http:{0}/Front/C/C0207";
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            this.ApiAddress = "http:{0}/api/FrontC0101/OrderAdd";
                            this.Referer = "http:{0}/Front/C/C0101";
                            break;
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                            this.ApiAddress = "http:{0}/api/FrontC0205/OrderAdd";
                            this.Referer = "http:{0}/Front/C/C0205";                        
                            break;


                    }
                }
            }

        }

        /// <summary>
        /// 是否包牌
        /// </summary>
        public bool IsBatch { get; private set; }

        /// <summary>
        /// 押注列表
        /// </summary>
        public IList<BatchOrder> OrderData { get; set; }

    }

    /// <summary>
    /// 产品枚举
    /// </summary>
    public enum ItemType
    {
        二定_千百=5,
        二定_千十 = 6,
        二定_千个 = 7,
        二定_百十 = 8,
        二定_百个 = 9,
        二定_十个 = 10,
        三定_千百十=11,
        三定_千百个=12,
        三定_千十个=13,
        三定_百十个=14,
        四定=-1
    }
}
