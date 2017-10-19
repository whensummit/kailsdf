using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Model.Web
{
    /// <summary>
    /// 网站用户信息
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// 主域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

    }

    /// <summary>
    /// 用户身份令牌
    /// </summary>
    public class UserLoginResult
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        public bool Successed { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public long LoginTime { get; set; }
    }
}
