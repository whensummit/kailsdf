
using SevenStarAutoSell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Common
{
    class PublicData
    {


        /// 当前赛事参数
        /// </summary>
        public static ServerParameterContract ServerParameter { get; } = new ServerParameterContract();
    }
}
