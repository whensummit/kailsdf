using SevenStarAutoSell.Business.Web._1688.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web._1688.Service
{
    /// <summary>
    /// 1688
    /// </summary>
    public class QiXing1688Adapter
    {
        protected log4net.ILog log = log4net.LogManager.GetLogger(typeof(QiXing1688Adapter));

        /// <summary>
        /// Http客户端
        /// </summary>
        public HttpClient HttpClient { get; set; }

        public QiXing1688Adapter()
        {
            HttpClient = new HttpClient();
        }
    }
}
