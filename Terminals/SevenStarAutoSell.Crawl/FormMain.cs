using NetMQ.Sockets;

using SevenStarAutoSell.Crawl.Common;
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

using SevenStarAutoSell.Models.Collect;
using SevenStarAutoSell.Common.Extensions;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models.Defs;
using SevenStarAutoSell.Models.Session;
using SevenStarAutoSell.Models;

namespace SevenStarAutoSell.Crawl
{
    public partial class FormMain : Form
    {
        private bool _mustClose = false;
        private bool _installed = false;

        //QiXingdfv168Adapter dfv = new QiXingdfv168Adapter();       
        

        #region 窗体事件

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        { 
            Install();
            InitCurrentPlatforms.Init();

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

        #endregion

        /// <summary>
        /// 安装
        /// </summary>
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
                Task.Factory.StartNew(ThreadHeartbeat, PublicData.CancellationToken.Token);
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
                            new ValueTypeParam<ClientTypeEnum>(ClientTypeEnum.Crawl)));
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 绑定消息通知事件
        /// </summary>
        private void BindMqEvents()
        {
            MQThreads.Instance.StartCenterServerEvent += StartCenterServerEvent;
            MQThreads.Instance.CloseCenterServerEvent += CloseCenterServerEvent;
            MQThreads.Instance.ServerParameterPushNewEvent += ServerParameterPushNewEvent;
        
          
            MQThreads.Instance.PushCollectEvent += PushCollectEvent;
            MQThreads.Instance.SendCollectEvent += SendCollectEvent;
        }

        /// <summary>
        /// 客户端直接扫水
        /// </summary>
        /// <param name="collect"></param>
        private void SendCollectEvent(Collect collect)
        {
            this.CollectProcess(collect, true);
        }

        /// <summary>
        /// 绑定消息通知事件
        /// </summary>
        private void UnbindMqEvents()
        {
            MQThreads.Instance.StartCenterServerEvent -= StartCenterServerEvent;
            MQThreads.Instance.CloseCenterServerEvent -= CloseCenterServerEvent;
            MQThreads.Instance.ServerParameterPushNewEvent -= ServerParameterPushNewEvent;

          
            MQThreads.Instance.PushCollectEvent -= PushCollectEvent;
            MQThreads.Instance.SendCollectEvent -= SendCollectEvent;
        }
        
        private void PushCollectEvent(Collect collect)
        {
            this.CollectProcess(collect, false);
        }

        private void CollectProcess(Collect collect,bool isClient)
        {
             
            CollectResult coll = new CollectResult();
            coll.ErrorMessage = "";
            coll.MaxBetMoney = 300;
            coll.Odds =7000;
            coll.ResultState = ResultStatus.Success;

            //var result = adp.SeekWater(content.Number);
            //if (result != null)
            //{
            //    coll.ErrorMessage = "";
            //    coll.MaxBetMoney = result.Amount;
            //    coll.Odds = result.Odds;
            //    coll.ResultState = ResultStatus.Success;
            //}
            //else
            //{
            //    coll.ErrorMessage = "查询赔率失败";
            //    coll.MaxBetMoney = 0;
            //    coll.Odds = 0;
            //    coll.ResultState = ResultStatus.Failure;
            //}
            coll.Id = collect.Id;
            coll.BuyerSessionID = collect.BuyerSessionID;
            coll.ClientSessionID = collect.ClientSessionID;
            coll.Domain = "www.dfv168.com";
            coll.Number = collect.Number;
            var rs= PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(IdKeyData.Create(PublicData.SessionId, "Collect/CollectCompletedToClient", coll));
            //IList<Task> taskList = new List<Task>();

            //foreach (var item in PublicData.CurrentPlatforms)
            //{
            //    var task = Task.Run(() =>
            //    {
            //        if (item.Value != null)
            //        {
            //            //初始化登录
            //            if (item.Value.LoginToken == null)
            //            {
            //                item.Value.Login(new Model.Web.UserLogin() { Domain = "www.dfv168.com", LoginName = "kf001", Password = "qwe123" });
            //                // .Initialize(new LoginModel() { Loginname = "kf001", Password = "qwe123" }, false);                 
            //            }

            //            var query = item.Value.CollectNumber(collect);

            //             PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(IdKeyData.Create(PublicData.SessionId, "Collect/CollectCompletedToClient", query));

            //            if (query != null)
            //            {
            //                query.CollectSessionID = PublicData.SessionId;
            //                MQActionVoidResult result;

            //                if (isClient)
            //                {
            //                    result = PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(IdKeyData.Create(PublicData.SessionId, "Collect/CollectCompletedToClient", query));
            //                }
            //                else
            //                {
            //                    result = PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(IdKeyData.Create(PublicData.SessionId, "Collect/CollectCompleted", query));
            //                }

            //                if (result != null && result.IsOK)
            //                {
            //                    //记录成功
            //                }
            //                else
            //                {
            //                    //记录失败
            //                }
            //            }
            //        }
            //    });
            //    taskList.Add(task);
            //}

            //Task.WaitAll(taskList.ToArray());

            //string xxx = "";

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
                    IdKeyData.Create("", "Session/Connect",new ValueTypeParam<ClientTypeEnum>(ClientTypeEnum.Crawl)));
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
                            new HeartbeatParam() { ClientType = ClientTypeEnum.Crawl }));
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
