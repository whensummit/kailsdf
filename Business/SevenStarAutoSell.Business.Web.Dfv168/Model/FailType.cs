using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 失败类型
    /// </summary>
    public enum FailType
    {
        /// <summary>
        /// 无
        /// </summary>
        NoFail = 0,

        /// <summary>
        /// 打开网址失败
        /// </summary>
        VisitFail = 1,

        /// <summary>
        /// 登陆失败
        /// </summary>
        LoginFail = 2
    }
}
