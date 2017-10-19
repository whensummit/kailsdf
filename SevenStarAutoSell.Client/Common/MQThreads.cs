using NetMQ.Sockets;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Extensions;
using SevenStarAutoSell.Common.Funcs;
using SevenStarAutoSell.Common.Utils;
using SevenStarAutoSell.Models.Collect;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Client.Common
{
    /// <summary>
    /// MQ消息统一处理中心(Subscrib/Dealer-Receive)
    /// <para>Request由各个业务代码内处理；本控盘端无Dealer-Send模式消息</para>
    /// <para>其它各个窗体对该类订阅事件即可</para>
    /// </summary>
    public class MQThreads : Singleton<MQThreads>
    {
        public event StartCenterServerEventHandler StartCenterServerEvent;

        //public event ServerParameterPushNewEventHandler ServerParameterPushNewEvent;
        //public event CloseCenterServerEventHandler CloseCenterServerEvent;
        //public event RaceInfoPushNewEventHandler RaceInfoPushNewEvent;
        //public event PushNewHorseInfoEventHandler PushNewHorseInfoEvent;

        //public event WebsiteAccountLoginComplateEventHandler WebsiteAccountLoginComplateEvent;

        //public event AddBetComplateEventHandler AddBetComplateEvent;
        //public event DeletePreOrderComplateEventHandler DeletePreOrderComplateEvent;

        //public event PushNewQQPDiscountsBetAndEatEventHandler PushNewQQPDiscountsBetAndEatEvent;
        //public event PushNewWWPDiscountsBetAndEatEventHandler PushNewWWPDiscountsBetAndEatEvent;
        //public event PushNewAllDiscountsBetOnlyEventHandler PushNewAllDiscountsBetOnlyEvent;

        //public event PushNewBetResultsEventHandler PushNewBetResultsEvent;

        public event PushNewOddsEventHandler PushNewOddsEvent;
        //public event PushNewLotteryResultEventHandler PushNewLotteryResultEvent;

        //public event ClientOnlineCheckEventHandler clientOnlineCheckEventHandler;
        /// <summary>
        /// 获取赔率事件
        /// </summary>
        //public event PushBetServerCloseEventHandler PushBetServerCloseEvent;

        private bool _inited = false;
        public void Install()
        {
            if (!_inited)
            {
                //Task.Factory.StartNew(ThreadSubscriber, PublicData.CancellationToken.Token);
                Task.Factory.StartNew(ThreadDealerReceive, PublicData.CancellationToken.Token);
                _inited = true;
            }
        }
        public void Uninstall()
        {
            // nothing
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
                subscriberSocket.Subscribe("PushNewHorseInfo");

                subscriberSocket.Subscribe("PushNewQQPDiscountsBetAndEat");
                subscriberSocket.Subscribe("PushNewWWPDiscountsBetAndEat");
                subscriberSocket.Subscribe("PushNewAllDiscountsBetOnly");

                subscriberSocket.Subscribe("PushNewOdds");

                subscriberSocket.Connect(MQConfig.PublishServer);
                while (!PublicData.CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        KeyData kd = subscriberSocket.SubscriberReceive();
                        Task.Factory.StartNew(() => HandleKeyData_Subscriber(kd.Key, kd.DataString));
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
            //if (cmdText == "StartCenterServer")
            //{
            //    StartCenterServerEvent?.Invoke();
            //}
            //else if (cmdText == "CloseCenterServer")
            //{
            //    CloseCenterServerEvent?.Invoke();
            //}
            //else if (cmdText == "ServerParameterPushNew")
            //{
            //    ServerParameterPushNewEvent?.Invoke(
            //        JsonUtil.Deserialize<ServerParameterContract>(actionJsonResult));
            //}
            //else if (cmdText == "RaceInfoPushNew")
            //{
            //    RaceInfoPushNewEvent?.Invoke(
            //        JsonUtil.Deserialize<RaceInfoContract>(actionJsonResult));
            //}
            //else if (cmdText == "PushNewHorseInfo")
            //{
            //    PushNewHorseInfoEvent?.Invoke(
            //        JsonUtil.Deserialize<List<HorseInfoContract>>(actionJsonResult));
            //}

            //else if (cmdText == "PushNewQQPDiscountsBetAndEat")
            //{
            //    PushNewQQPDiscountsBetAndEatEvent?.Invoke(
            //        JsonUtil.Deserialize<QuinellaPlaceDiscountContract>(actionJsonResult));
            //}
            //else if (cmdText == "PushNewWWPDiscountsBetAndEat")
            //{
            //    PushNewWWPDiscountsBetAndEatEvent?.Invoke(
            //        JsonUtil.Deserialize<WinPlaceDiscountContract>(actionJsonResult));
            //}
            //else if (cmdText == "PushNewAllDiscountsBetOnly")
            //{
            //    PushNewAllDiscountsBetOnlyEvent?.Invoke(
            //        JsonUtil.Deserialize<DiscountBetOnlyContract>(actionJsonResult));
            //}
            //else if (cmdText == "PushNewOdds")
            //{
            //    PushNewOddsEvent?.Invoke(
            //        JsonUtil.Deserialize<PushOddsContract>(actionJsonResult));
            //}
            //else if (cmdText == "PushNewLotteryResult")
            //{
            //    PushNewLotteryResultEvent?.Invoke(
            //        JsonUtil.Deserialize<LotteryResultContract>(actionJsonResult));
            //}
            //else
            //{
            //    LogUtil.Error($"未处理的订阅信息：cmd:{cmdText}; json:{actionJsonResult}");
            //}
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
            if (cmdText == "WebsiteAccountLoginComplate")
            {
                //WebsiteAccountLoginComplateEvent?.Invoke(
                //    JsonUtil.Deserialize<WebsiteLoginResultContract>(actionJsonResult));
            }
            //else if (cmdText == "AddBetComplate")
            //{
            //    AddBetComplateEvent?.Invoke(
            //        JsonUtil.Deserialize<AddBetComplateResult>(actionJsonResult));
            //}
            //else if (cmdText == "DeletePreOrderComplate")
            //{
            //    DeletePreOrderComplateEvent?.Invoke(
            //        JsonUtil.Deserialize<DeletePreOrderComplateContract>(actionJsonResult));
            //}
            //else if (cmdText == "PushNewBetResults")
            //{
            //    PushNewBetResultsEvent?.Invoke(
            //        JsonUtil.Deserialize<AccountBetResultsContract>(actionJsonResult));
            //}
            //else if (cmdText == "PushWebsiteAccountBalance")
            //{
            //    PushWebsiteAccountBalanceEvent?.Invoke(
            //        JsonUtil.Deserialize<WebsiteAccountBlanceContract>(actionJsonResult));
            //}
            else if (cmdText == "PushCollectCompleted")
            {
                PushNewOddsEvent?.Invoke(JsonUtil.Deserialize<CollectResult>(actionJsonResult));
            }
            else
            {
                LogUtil.Error($"未处理的DealerReceive消息：cmd:{cmdText}; json:{actionJsonResult}");
            }
        }
    }

    // 中心服务开启暂停投注
    public delegate void StartCenterServerEventHandler();
    public delegate void CloseCenterServerEventHandler();

    //// 赛事信息改变
    //public delegate void ServerParameterPushNewEventHandler(ServerParameterContract newServerParam);
    //public delegate void RaceInfoPushNewEventHandler(RaceInfoContract newRaceInfo);
    //public delegate void PushNewHorseInfoEventHandler(List<HorseInfoContract> newHorseInfo);

    //// 投注账号登陆
    //public delegate void WebsiteAccountLoginComplateEventHandler(WebsiteLoginResultContract websiteLoginResult);

    //// 投注
    //public delegate void AddBetComplateEventHandler(AddBetComplateResult result);
    //public delegate void DeletePreOrderComplateEventHandler(DeletePreOrderComplateContract result);

    //// 折扣
    //public delegate void PushNewQQPDiscountsBetAndEatEventHandler(QuinellaPlaceDiscountContract newDiscounts);
    //public delegate void PushNewWWPDiscountsBetAndEatEventHandler(WinPlaceDiscountContract newDiscounts);
    //public delegate void PushNewAllDiscountsBetOnlyEventHandler(DiscountBetOnlyContract newDiscounts);

    //// 下注结果
    //public delegate void PushNewBetResultsEventHandler(AccountBetResultsContract results);

    //获取赔率
    public delegate void PushNewOddsEventHandler(CollectResult item);
    //public delegate void PushNewLotteryResultEventHandler(LotteryResultContract lottryResult);

    // 用户客户端心跳
    //public delegate void ClientOnlineCheckEventHandler(WebsiteAccountBlanceContract webAccounBlance);

    /// <summary>
    /// 所属投注服务端关闭了
    /// </summary>
    public delegate void PushBetServerCloseEventHandler();

}
