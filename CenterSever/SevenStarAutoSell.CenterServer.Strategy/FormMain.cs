using NetMQ.Sockets;
using SevenStarAutoSell.CenterServer.Strategy.Common;
using SevenStarAutoSell.CenterServer.Strategy.Model;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Extensions;
using SevenStarAutoSell.Models;
using SevenStarAutoSell.Models.Bet;
using SevenStarAutoSell.Models.BetPool;
using SevenStarAutoSell.Models.Collect;
using SevenStarAutoSell.Models.Defs;
using SevenStarAutoSell.Models.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenStarAutoSell.CenterServer.Strategy
{
    public partial class FormMain : Form
    {
        private bool _mustClose = false;

        private bool _installed = false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Install();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_mustClose)
            {
                if (!MessageBoxEx.Confirm("确认关闭投注后台服务吗（建议最后退出）？"))
                {
                    e.Cancel = true;
                    return;
                }
            }

            Uninstall();
        }

        private void Install()
        {
            if (_installed)
            {
                return;
            }

            // 连接中心服务器
            if (string.IsNullOrEmpty(PublicData.SessionId))
            {
                Thread.Sleep(500); // 等一下中心服务器
                if (ConnectCenterServer() == false)
                {
                    this._mustClose = true;
                    this.Close();
                    return;
                }

                // MQ
                MQThreads.Instance.Init();
                BindMqEvents();

                //心跳线程
                Task.Factory.StartNew(ThreadHeartbeat, PublicData.CancellationToken.Token);

                //获取出货信息线程
                Task.Factory.StartNew(ThreadBetInformation, PublicData.CancellationToken.Token);

                //下单扫水线程
                Task.Factory.StartNew(ThreadBetProcess, PublicData.CancellationToken.Token);

            }

            _installed = true;
        }
        
        private void Uninstall()
        {
            PublicData.CancellationToken.Cancel();
            UnbindMqEvents();
            while (PublicData.DealerSendQueue.Count > 0)
            {
                Thread.Sleep(500);
            }

            // 断开服务器
            try
            {
                if (!string.IsNullOrEmpty(PublicData.SessionId))
                {
                    PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(
                        IdKeyData.Create(PublicData.SessionId, "Session/Disconnect",
                            new ValueTypeParam<ClientTypeEnum>(ClientTypeEnum.Sell)));
                }
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// 下单处理线程
        /// </summary>
        private void ThreadBetProcess()
        {
            while (!PublicData.CancellationToken.IsCancellationRequested)
            {
                //扫水
                var task = Task.Run(() =>
                {
                    this.BetCollect();
                });

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 扫水线程
        /// </summary>
        private void BetCollect()
        {
            var betProc = BetProcessPool.GetBetProcess();
            if (null != betProc)
            {
                Collect col = new Collect();
                col.Id = betProc.Id;
                col.BuyerSessionID = PublicData.SessionId;
                col.ClientSessionID = betProc.ClientSessionID;
                col.Number = betProc.Number;               
                MQActionVoidResult result = PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(IdKeyData.Create(PublicData.SessionId, "Collect/PushCollect", col));
                if (null != result && result.IsOK)
                {
                    //记录命令发送成功
                }
                else
                {
                    //记录命令发送失败
                }

            }
        }
       
        /// <summary>
        /// 获取出货信息
        /// </summary>
        private void ThreadBetInformation()
        {
            while (!PublicData.CancellationToken.IsCancellationRequested)
            {
                try
                {
                    MQActionResult<BetPoolItem> result = PublicData.RequestSocket.RequestSendReceive<MQActionResult<BetPoolItem>>(
                        IdKeyData.Create(PublicData.SessionId, "BetPool/GetBetInPool", PublicData.SessionId));
                    if (result.IsOK)
                    {
                        BetPoolItem betResult = result.Data;

                        //添加到处理池
                        Task.Run(()=> {
                            BetProcess bet = new BetProcess();
                            bet.Id = betResult.Id;
                            bet.Locked = false;
                            bet.Money = betResult.Money;
                            bet.Number = betResult.Number;
                            bet.OperateID = betResult.OperateID;
                            bet.Order = betResult.Order;
                            bet.BuyerSessionID = PublicData.SessionId;                  
                            bet.ClientSessionID = betResult.ClientSessionID;
                            BetProcessPool.AddBetInPool(bet);
                        });
                    }
                    else
                    {                     
                        LogError(Environment.NewLine + "获取下注信息失败:" + result.ErrorMsg);
                    }
                }
                catch (Exception ex)
                {
                   
                    LogError(Environment.NewLine + "获取下注信息异常:" + Environment.NewLine + ex.StackTrace);
                }

                Thread.Sleep(200); //200毫秒获取一次 
            }
        }

        private void BindMqEvents()
        {
            MQThreads.Instance.StartCenterServerEvent += StartCenterServerEvent;
            MQThreads.Instance.CloseCenterServerEvent += CloseCenterServerEvent;
            MQThreads.Instance.ServerParameterPushNewEvent += ServerParameterPushNewEvent;        

            MQThreads.Instance.CollectResultEvent += CollectResultEvent;
            MQThreads.Instance.BetContentEvent += BetContentEvent;
            MQThreads.Instance.DeletedBetContentEvent += DeletedBetContentEvent;
        }

        /// <summary>
        /// 退单结果事件处理
        /// </summary>
        /// <param name="content"></param>
        private void DeletedBetContentEvent(DeleteBetContentResult content)
        {
           //处理退单结果

        }

        /// <summary>
        /// 处理下单结果
        /// </summary>
        /// <param name="content"></param>
        private void BetContentEvent(BetContentResult content)
        {
            //收到下注结果
            if (null != content)
            {
                BetInformation bet = new BetInformation();
                bet.BetAccount = content.BetAccount;               
                bet.BetMoney = content.BetMoney;
                bet.BetPlatform = content.BetPlatformEnum;
                bet.BetSessionID = content.BetSessionID;
                bet.DeleteOrderID = content.DeleteOrderID;
                bet.Number = content.Number;
                bet.Odds = content.Odds;
                bet.OperateTime = content.BetTime;
                bet.OrderID = content.OrderID;
                bet.Money = content.Money;               

                //符合赔率
                if (bet.Odds >= 9000)
                {
                    bet.Status = ResultStatus.Success;
                }
                else
                {
                    //退码
                    bet.Status = ResultStatus.Deleting;
                    content.ResultStatus = ResultStatus.Deleting;
                    MQActionVoidResult result = PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(IdKeyData.Create(PublicData.SessionId, "Bet/DeleteBet", content));
                    if (null != result && result.IsOK)
                    {
                        //记录命令发送成功
                    }
                    else
                    {
                        //记录命令发送失败
                    }


                }
                var count = BetProcessPool.SetBetProcessBetContent(content.Id, bet);

                if (count == 0)
                {
                    //下单完成 可能存在已经退水了的
                }
            }
        }

        /// <summary>
        /// 收到扫水结果
        /// </summary>
        /// <param name="content">扫水结果</param>
        private void CollectResultEvent(CollectResult content)
        {
            CollectInformation ci = new CollectInformation();
            ci.CollectPlatform = content.Platform;
            ci.CollectSessionID = content.CollectSessionID;
            ci.MaxBetMoney = content.MaxBetMoney;
            ci.Number = content.Number;
            var count= BetProcessPool.SetBetProcessCollect(content.Id, ci);
            //如果扫水完成
            if (count == 0)
            {
                var colls = BetProcessPool.GetCollectByID(content.Id);
                if (null != colls)
                {
                    //分析扫水结果
                    //->暂缺

                    //开始下注
                    foreach (CollectInformation item in colls)
                    {
                        //设置分配好的下注平台
                        BetProcessPool.AddBetListItemInBetProcess(content.Id, item.CollectPlatform);

                        Task.Run(() => {
                            BetContent bet = new BetContent();
                            bet.Id = content.Id;
                            bet.BetPlatformEnum = item.CollectPlatform;
                            bet.BuyerSessionID = content.BuyerSessionID;
                            bet.ClientSessionID = content.ClientSessionID;
                            bet.InputType = 1; //号码类型
                            bet.Money = 1;// 分配的金额
                            bet.Number = item.Number;
                            MQActionVoidResult result = PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(IdKeyData.Create(PublicData.SessionId, "Bet/AddBet", bet));
                            if (null != result && result.IsOK)
                            {
                                //记录命令发送成功
                            }
                            else
                            {
                                //记录命令发送失败
                            }


                        });
                    }
                }
               
            }
        }

        private void UnbindMqEvents()
        {
            MQThreads.Instance.StartCenterServerEvent -= StartCenterServerEvent;
            MQThreads.Instance.CloseCenterServerEvent -= CloseCenterServerEvent;
            MQThreads.Instance.ServerParameterPushNewEvent -= ServerParameterPushNewEvent;

            MQThreads.Instance.CollectResultEvent -= CollectResultEvent;
            MQThreads.Instance.BetContentEvent -= BetContentEvent;
            MQThreads.Instance.DeletedBetContentEvent -= DeletedBetContentEvent;

        }

        private void StartCenterServerEvent()
        {
            Log("中心服务器已启动投注");
        }

        private void CloseCenterServerEvent()
        {
            Log(Environment.NewLine + "提醒：中心服务器已关闭，此程序1秒后自动关闭！");
            Log(Environment.NewLine + "提醒：中心服务器已关闭，此程序1秒后自动关闭！");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            this._mustClose = true;
            Application.Exit();
        }

        private void ServerParameterPushNewEvent(ServerParameterContract newServerParam)
        {
            Log("接收到新的服务器参数");
            if (PublicData.ServerParameter != null && !PublicData.ServerParameter.IsEmpty())
            {
                if (!PublicData.ServerParameter.UrlCC.Equals(newServerParam.UrlCC, StringComparison.CurrentCultureIgnoreCase)
                    || !PublicData.ServerParameter.UrlCC.Equals(newServerParam.UrlWL, StringComparison.CurrentCultureIgnoreCase)
                    || !PublicData.ServerParameter.UrlCC.Equals(newServerParam.UrlXWL, StringComparison.CurrentCultureIgnoreCase)
                    || !PublicData.ServerParameter.UrlCC.Equals(newServerParam.Url668, StringComparison.CurrentCultureIgnoreCase))
                {
                    // todo URL变化，重新初始化URL已经变化的网站线程（重要不紧急，中心服务器URL更新，不紧急）
                }
            }
            PublicData.ServerParameter = newServerParam;

        }

     
        
        /// <summary>
        /// 连接中心服务器
        /// </summary>
        /// <returns></returns>
        private bool ConnectCenterServer()
        {
            try
            {
                PublicData.RequestSocket = new RequestSocket();
                PublicData.RequestSocket.Connect(MQConfig.ResponseServer);

                MQActionResult<string> result = PublicData.RequestSocket.RequestSendReceive<MQActionResult<string>>(
                    IdKeyData.Create("", "Session/Connect",
                        new ValueTypeParam<ClientTypeEnum>(ClientTypeEnum.Strategy)));
                if (null != result && result.IsOK)
                {
                    PublicData.SessionId = result.Data;

                    Log("连接中心服务器成功。");
                    return true;
                }
                else
                {
                    MessageBoxEx.Alert("中心服务器连接失败，信息：" + result.ErrorMsg);
                    Log("中心服务器连接失败。");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Alert(ex.Message);
                return false;
            }
        }

        private void Log(string message)
        {
            try
            {

            }
            catch
            {
                // ignored
            }
        }

        private void LogError(string message)
        {
            try
            {

            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// 发送心跳线程
        /// </summary>
        private void ThreadHeartbeat()
        {
            int heartbeatErrorCount = 0;
            while (!PublicData.CancellationToken.IsCancellationRequested && heartbeatErrorCount < 5)
            {
                try
                {
                    MQActionVoidResult result = PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(
                        IdKeyData.Create(PublicData.SessionId, "Session/Heartbeat",
                            new HeartbeatParam() { ClientType = ClientTypeEnum.Sell }));
                    if (result.IsOK)
                    {
                        heartbeatErrorCount = 0;
                    }
                    else
                    {
                        heartbeatErrorCount++;
                        LogError(Environment.NewLine + "投注服务端心跳失败，错误信息：" + result.ErrorMsg);
                    }
                }
                catch (Exception ex)
                {
                    heartbeatErrorCount++;
                    LogError(Environment.NewLine + "Request发送心跳异常，错误信息：" + Environment.NewLine + ex.StackTrace);
                }

                Thread.Sleep(TimeSpan.FromSeconds(2));
            }

            if (heartbeatErrorCount >= 5)
            {
                _mustClose = true;
                this.Close();
            }
        }

       
    }
}
