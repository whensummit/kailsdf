using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models
{
    public class ServerParameterContract
    {
        public DateTime RaceDate
        {
            get; set;
        }

        public string RaceCode
        {
            get; set;
        }

        /// <summary>
        /// 长城域名
        /// </summary>
        public string UrlCC { get; set; }

        /// <summary>
        /// 万利域名
        /// </summary>
        public string UrlWL { get; set; }

        /// <summary>
        /// 新万利域名
        /// </summary>
        public string UrlXWL { get; set; }

        /// <summary>
        /// 668域名
        /// </summary>
        public string Url668 { get; set; }

        /// <summary>
        /// 668安全码
        /// </summary>
        public string Safecode668 { get; set; }

        public bool IsEmpty()
        {
            if (string.IsNullOrEmpty(RaceCode))
                return true;
            else
                return false;
        }
    }
}
