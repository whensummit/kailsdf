using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Controlers
{
    /// <summary>
    /// 所有处理(Response, DealerReceive)命令的类都继承此接口，程序会自动注册
    /// <para>子类命名要求：子类的命名方式必须为“xxxControler”，而且命令映射的方法必须为static；而且第一个参数必须为sessionId，第二个随意，无第三个。</para>
    /// <para>如果有第二个参数，要求：必须为Class；如果是单一值类型，请试用ValueTypeParam<T>，例如：ValueTypeParam<string></para>
    /// <para>路由中心<see cref="RouteConfig.ExecCmd"/>有统一拦截Action的错误，Controler内部无需再Try{}Catch, 直接抛出异常</para>
    /// </summary>
    interface IControler
    {
    }

    /// <summary>
    /// 某个Action上添加该属性，表示无需启动投注也可访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class NoNeedBettingStartAttribute : Attribute
    {
    }
}
