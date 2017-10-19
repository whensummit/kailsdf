using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 账号信息
    /// </summary>
    public class AccountInformation
    {
        /// <summary>
        /// 网站域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public double Point { get; set; }

        /// <summary>
        /// 可用金额
        /// </summary>
        public double AvailablePoint { get; set; }

        /// <summary>
        /// 已用金额
        /// </summary>
        public double UsePoint { get; set; }

        /// <summary>
        /// 查询时间
        /// </summary>
        public long UpdatedTime { get; set; }
    }
}
