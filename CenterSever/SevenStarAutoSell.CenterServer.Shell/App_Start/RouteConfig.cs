using SevenStarAutoSell.CenterServer.Shell.Controlers;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Funcs;
using SevenStarAutoSell.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SevenStarAutoSell.CenterServer.Shell.App_Start
{
    /// <summary>
    /// 处理MQ的消息路由
    /// </summary>
    public class RouteConfig : Singleton<RouteConfig>
    {
        private Dictionary<string, MethodInfo> _cmdActions = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// 注册所有命令路由（处理来自Request/DealerSend的命令）
        /// </summary>
        public void RegisterRoutes()
        {
            if (_cmdActions.Count > 0) return;

            Type[] types = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("SevenStarAutoSell.Center"))
                   .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IControler))))
                   .ToArray();

            string controlerName = "";
            foreach (var v in types)
            {
                controlerName = v.Name.Substring(0, v.Name.Length - "Controler".Length);

                MethodInfo[] allMethods = v.GetMethods(BindingFlags.Public | BindingFlags.Static);
                foreach (var method in allMethods)
                {
                    _cmdActions.Add($"{controlerName}/{method.Name}", method);
                }
            }
        }

        /// <summary>
        /// 执行命令(返回json字符串)
        /// </summary>
        /// <param name="sessionId">客户端id</param>
        /// <param name="cmdText">Controler/Action</param>
        /// <param name="mqData">MQ数据</param>
        /// <returns></returns>
        public string ExecCmd(string sessionId, string cmdText, byte[] mqData)
        {
            if (_cmdActions.ContainsKey(cmdText))
            {
                // （统一拦截Action的异常信息）
                try
                {
                    MethodInfo method = _cmdActions[cmdText];

                    // todo 要放开检查校验逻辑（不重要不紧急，如果放开需要调整各Action权限检查）
                    ////if (PublicData.IsStopped && 
                    ////    method.GetCustomAttribute(typeof(NoNeedBettingStartAttribute)) == null)
                    ////{
                    ////    return JsonUtil.Serialize(MQActionVoidResult.Error(CommonErrors.CenterServerNotStart));
                    ////}

                    // Action参数反序列号
                    List<object> arguments = new List<object>() { sessionId };
                    var paramsInfo = method.GetParameters();
                    if (paramsInfo.Length > 1)
                    {
                        arguments.Add(JsonUtil.Deserialize(mqData, paramsInfo[1].ParameterType));
                    }

                    // 执行Action
                    object result = method.Invoke(null, arguments.ToArray());
                    return JsonUtil.Serialize(result);
                }
                catch (Exception ex)
                {
                    return JsonUtil.Serialize(MQActionVoidResult.Error(ex.Message));
                }
            }
            else if (cmdText == "IsRunning")
            {
                return JsonUtil.Serialize(MQActionVoidResult.Ok());
            }
            else
            {
                return JsonUtil.Serialize(MQActionVoidResult.Error($"不存在命令：{cmdText}"));
            }
        }
    }
}
