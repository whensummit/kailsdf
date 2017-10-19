using NetMQ.Sockets;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Extensions;
using SevenStarAutoSell.Models;

using SevenStarAutoSell.Models.Defs;
using SevenStarAutoSell.Models.Session;
using SevenStarAutoSell.Sell.Common;
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

namespace SevenStarAutoSell.Sell
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


     

      
       
      

        private void BindMqEvents()
        {
            MQThreads.Instance.StartCenterServerEvent += StartCenterServerEvent;
            MQThreads.Instance.CloseCenterServerEvent += CloseCenterServerEvent;
            MQThreads.Instance.ServerParameterPushNewEvent += ServerParameterPushNewEvent;        

        
        }

     
     

   

        private void UnbindMqEvents()
        {
            MQThreads.Instance.StartCenterServerEvent -= StartCenterServerEvent;
            MQThreads.Instance.CloseCenterServerEvent -= CloseCenterServerEvent;
            MQThreads.Instance.ServerParameterPushNewEvent -= ServerParameterPushNewEvent;         

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
                        new ValueTypeParam<ClientTypeEnum>(ClientTypeEnum.Sell)));
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
