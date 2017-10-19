using SevenStarAutoSell.Common.Funcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Common
{
    public delegate void ActionLogEventHandler(string message);
    /// <summary>
    /// 注册Controler的事件
    /// </summary>
    internal class ControlerEvents : Singleton<ControlerEvents>
    {
        public ActionLogEventHandler ActionLogEvent = null;

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        public void OnActionLog(string message)
        {
            this.ActionLogEvent?.Invoke(message);
        }
    }
}
