using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Funcs
{
    /// <summary>
    /// （各端使用的MQ消息事件绑定，统一MQThreads的处理方式，和事件绑定解绑）
    /// </summary>
    public interface IMQThreadsEventsBindable
    {
        void BindMQEvents();
        void UnbindMQEvents();
    }
}
