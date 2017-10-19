using NetMQ;
using NetMQ.Sockets;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Extensions;
using SevenStarAutoSell.Common.Funcs;
using SevenStarAutoSell.Common.Utils;


using SevenStarAutoSell.Models;
using SevenStarAutoSell.Models.Bet;
using SevenStarAutoSell.Models.Collect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Strategy.Common
{
    /// <summary>
    /// MQ消息统一处理中心(Subscrib/Dealer-Receive)
    /// <para>Request由各个业务代码内处理；本控盘端无Dealer-Send模式消息</para>
    /// <para>其它各个窗体对该类订阅事件即可</para>
    /// </summary>
    public class MQThreads : Singleton<MQThreads>
    {
        public event MqDealerSendEventHandler MqDealerSendEvent;
        public event MqDealerReceiveEventHandler MqDealerReceiveEvent;

        public event StartCenterServerEventHandler StartCenterServerEvent;
        public event CloseCenterServerEventHandler CloseCenterServerEvent;
        public event ServerParameterPushNewEventHandler ServerParameterPushNewEvent;

        /// <summary>
        /// 扫水结果返回事件
        /// </summary>
        public event CollectResultEventHandler CollectResultEvent;

        /// <summary>
        /// 下注结果返回事件
        /// </summary>
        public event BetContentEventHandler BetContentEvent;

        /// <summary>
        /// 退码结果返回事件
        /// </summary>
        public event DeletedBetContentEventHandler DeletedBetContentEvent;     

        private bool _inited = false;
        /// <summary>
        /// 初始化（程序入口主窗体调用）
        /// </summary>
        public void Init()
        {
            if (!_inited)
            {
                Task.Factory.StartNew(ThreadSubscriber, PublicData.CancellationToken.Token);
                Task.Factory.StartNew(ThreadDealerReceive, PublicData.CancellationToken.Token);
                Task.Factory.StartNew(ThreadDealerSend, PublicData.CancellationToken.Token);
                _inited = true;
            }
        }

        // 发送数据量大的情况使用DealerSend，否则使用RequestSocket
        private void ThreadDealerSend()
        {
            using (DealerSocket dealerSocket = new DealerSocket())
            {
                dealerSocket.Options.Identity = Encoding.UTF8.GetBytes(PublicData.SessionId);
                dealerSocket.Connect(MQConfig.RouterReceiveServer);

                while (!PublicData.CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        KeyData item;
                        if (PublicData.DealerSendQueue.TryTake(out item, -1))
                        {
                            dealerSocket.DealerSend(item);
                            MqDealerSendEvent?.Invoke(item);
                        }
                    }
                    catch (TerminatingException ex)
                    {
                        LogUtil.Error($"dealer消息发送异常：{ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error($"dealer消息发送异常：{ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        private void ThreadSubscriber()
        {
            using (SubscriberSocket subscriberSocket = new SubscriberSocket())
            {
                subscriberSocket.Subscribe("StartCenterServer");
                subscriberSocket.Subscribe("CloseCenterServer");
                subscriberSocket.Subscribe("ServerParameterPushNew");
                subscriberSocket.Subscribe("RaceInfoPushNew");

                subscriberSocket.Connect(MQConfig.PublishServer);
                while (!PublicData.CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        KeyData kd = subscriberSocket.SubscriberReceive();
                        HandleKeyData_Subscriber(kd.Key, kd.DataString);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error($"订阅消息执行异常：{ex.Message}");
                    }
                }
            }
        }

        // 处理订阅消息
        private void HandleKeyData_Subscriber(string cmdText, string actionJsonResult)
        {
            if (cmdText == "StartCenterServer")
            {
                StartCenterServerEvent?.Invoke();
            }
            else if (cmdText == "CloseCenterServer")
            {
                CloseCenterServerEvent?.Invoke();
            }
            else if (cmdText == "ServerParameterPushNew")
            {
                ServerParameterPushNewEvent?.Invoke(
                    JsonUtil.Deserialize<ServerParameterContract>(actionJsonResult));
            }
            else if (cmdText == "RaceInfoPushNew")
            {
              
            }
            else
            {
                LogUtil.Error($"未处理的订阅信息：cmd:{cmdText}; json:{actionJsonResult}");
            }
        }


        // Dealer-Receive接收信息
        private void ThreadDealerReceive()
        {
            using (DealerSocket dealerSocket = new DealerSocket())
            {
                dealerSocket.Options.Identity = Encoding.UTF8.GetBytes(PublicData.SessionId);
                dealerSocket.Connect(MQConfig.RouterSendServer);

                while (!PublicData.CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        KeyData kd = dealerSocket.DealerReceive();
                        MqDealerReceiveEvent?.Invoke(kd);
                        Task.Factory.StartNew(() => HandleKeyData_DealerReceive(kd.Key, kd.DataString));
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error($"Dealer-Receive消息执行异常：{ex.Message}");
                    }
                }

            }
        }

        // DealerReceive数据处理
        private void HandleKeyData_DealerReceive(string cmdText, string actionJsonResult)
        {
            //if (PublicData.ServerParameter == null || PublicData.ServerParameter.IsEmpty())
            //{
            //    MessageBoxEx.Alert($"服务器参数获取失败，无法处理{cmdText}方法");
            //    return;
            //}

            
            if (cmdText == "PushCollectCompleted") //扫水结果
            {
                //解析扫水结果
                CollectResult collectResult = JsonUtil.Deserialize<CollectResult>(actionJsonResult);
                CollectResultEvent?.Invoke(collectResult);
            }
            else if (cmdText == "PushAddBetCompleted") //下注结果
            {
                //解析下注结果
                BetContentResult betContentResult = JsonUtil.Deserialize<BetContentResult>(actionJsonResult);
                BetContentEvent?.Invoke(betContentResult);
            }
            else if (cmdText == "PushDeleteBetResult") //退码结果
            {
                //解析下注结果
                DeleteBetContentResult betContentResult = JsonUtil.Deserialize<DeleteBetContentResult>(actionJsonResult);
                DeletedBetContentEvent?.Invoke(betContentResult);
            }
            else
            {
                LogUtil.Error($"未处理的DealerReceive消息：cmd:{cmdText}; json:{actionJsonResult}");
            }
        }
    }

    // MQ发送数据时触发事件
    public delegate void MqDealerSendEventHandler(KeyData item);
    public delegate void MqDealerReceiveEventHandler(KeyData item);

    // 中心服务开启暂停投注
    public delegate void StartCenterServerEventHandler();
    public delegate void CloseCenterServerEventHandler();
    public delegate void ServerParameterPushNewEventHandler(ServerParameterContract newServerParam);

    /// <summary>
    /// 扫水结果 委托
    /// </summary>
    /// <param name="content">返回信息</param>
    public delegate void CollectResultEventHandler(CollectResult content);

    /// <summary>
    /// 下注结果 委托
    /// </summary>
    /// <param name="content"></param>
    public delegate void BetContentEventHandler(BetContentResult content);

    /// <summary>
    /// 退码结果 委托
    /// </summary>
    /// <param name="content"></param>
    public delegate void DeletedBetContentEventHandler(DeleteBetContentResult content);

    /// <summary>
    /// 共用实体委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    public delegate void CommonEventHandler<T>(T content);


}
