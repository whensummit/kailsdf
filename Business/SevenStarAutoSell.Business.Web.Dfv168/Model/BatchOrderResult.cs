using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 批量押注返回数据
    /// </summary>
    public class BatchOrderResult:BaseMessage
    {
        public BatchOrderResultData Data { get; set; }
    }

    /// <summary>
    /// 批量下注数据
    /// </summary>
    public class BatchOrderResultData
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
        
        public JToken Success { get; set; }

        public IList<IList<Array>> Fail { get; set; }


    }
}
