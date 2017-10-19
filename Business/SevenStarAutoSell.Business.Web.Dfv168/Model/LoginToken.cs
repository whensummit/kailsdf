using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Business.Web.Dfv168.Model
{
    /// <summary>
    /// 登陆票
    /// </summary>
    public class LoginToken:BaseMessage
    { 
        /// <summary>
        /// 身份票
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 路径名称
        /// </summary>
        public string PathName { get; set; }

        /// <summary>
        /// GID
        /// </summary>
        public string GID { get; set; }

        /// <summary>
        /// 提交身份票
        /// </summary>
        public string SubmitData { get; set; }
    }
}
