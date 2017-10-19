using SevenStarAutoSell.CenterServer.Shell.App_Start;
using SevenStarAutoSell.CenterServer.Shell.Common;
using SevenStarAutoSell.CenterServer.Shell.Controlers;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Extensions;
using SevenStarAutoSell.Common.Utils;
using SevenStarAutoSell.Models.Defs;
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

namespace SevenStarAutoSell.CenterServer.Shell
{
    public partial class FormMain : Form
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private MQThreads _mqThreads = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // 加载配置
            ControlerEvents.Instance.ActionLogEvent = this.Log;
            this.StartMQThreads();
            //to do 取消心跳
            //Task.Factory.StartNew(CheckHeartbeat, _cancellationTokenSource.Token);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!MessageBoxEx.Confirm("确认关闭中心服务器吗（如果关闭，将提示用户关闭投注程序）？"))
            {
                e.Cancel = true;
                return;
            }

            // 广播中心服务器关闭命令
            Log(Environment.NewLine + "关闭中心服务器，等待其它端自动关闭！");
            MQPublishQueue.CloseCenterServer();

            // 等待各个服务端都退出
            int tryCount = 20;
            while (tryCount > 0)
            {
                if (SessionPool.HasUnConnectedServers())
                {
                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }
                tryCount--;
            }

            // 检查是否有未退出的服务端（除开客户使用的端）
            if (SessionPool.HasUnConnectedServers())
            {
                MessageBoxEx.Confirm("还存在未关闭的其它控制端软件，请手工退出所有后台采集程序？");
            }

            _cancellationTokenSource.Cancel();
            Thread.Sleep(500);
        }


        private void StartMQThreads()
        {
            if (_mqThreads != null) return;

            _mqThreads = new MQThreads(_cancellationTokenSource);
            Task.Factory.StartNew(_mqThreads.ThreadMQPublishServer, _cancellationTokenSource.Token);
            Task.Factory.StartNew(_mqThreads.ThreadMQRouterSend, _cancellationTokenSource.Token);
            Task.Factory.StartNew(_mqThreads.ThreadMQRouterReceive, _cancellationTokenSource.Token);
            Task.Factory.StartNew(_mqThreads.ThreadMQResponse, _cancellationTokenSource.Token);
        }

        #region 检查心跳

        private void CheckHeartbeat()
        {
            Color activeColor = Color.ForestGreen;
            Color disableColor = Color.DarkGray;
            DateTime checkTime;

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    this.Invoke((MethodInvoker)(delegate
                    {
                        checkTime = DateTime.Now.AddSeconds(-15);
                        CheckHeartbeat_CollectRaceInfo(activeColor, disableColor, checkTime);
                        //CheckHeartbeat_CollectOdds(activeColor, disableColor, checkTime);
                        CheckHeartbeat_CollectDiscount(activeColor, disableColor, checkTime);

                        CheckHeartbeat_BetServer(activeColor, disableColor);
                        CheckHeartbeat_ClientDesk(activeColor, disableColor);
                    }));
                }
                catch (Exception e)
                {
                    LogUtil.Error(e);
                }

                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }

        private void CheckHeartbeat_CollectRaceInfo(Color activeColor, Color disableColor, DateTime checkTime)
        {
            var clientsCollectRaceInfo = SessionPool.GetClientsGroup(ClientTypeEnum.Sell);
            if (clientsCollectRaceInfo.Count > 0 && clientsCollectRaceInfo.First().Value.Heartbeat >= checkTime)
            {
               
            }
            else
            {
                
            }
        }
        private void CheckHeartbeat_CollectOdds(Color activeColor, Color disableColor, DateTime checkTime)
        {
            var clientsCollectOdds = SessionPool.GetClientsGroup(ClientTypeEnum.Unknow); //
            if (clientsCollectOdds.Count > 0 && clientsCollectOdds.First().Value.Heartbeat >= checkTime)
            {
               
            }
            else
            {
               
            }
        }

        private void CheckHeartbeat_BetServer(Color activeColor, Color disableColor)
        {
            var clientsBet = SessionPool.GetClientsGroup(ClientTypeEnum.Bet);
            int okCount = 0;
            if (clientsBet.Count > 0)
            {
                DateTime checkTime = DateTime.Now.AddSeconds(-20);
                var sessionIds = clientsBet.Keys.ToArray();
                foreach (var item in sessionIds)
                {
                    if (clientsBet[item].Heartbeat >= checkTime)
                    {
                        okCount++;
                    }
                    else
                    {
                        // 移除服务端资源
                        SessionControler.Disconnect(item,
                            new ValueTypeParam<ClientTypeEnum>(ClientTypeEnum.Bet));
                    }
                }
            }

            if (okCount > 0)
            {
               
            }
            else
            {
               
            }
        }

        private void CheckHeartbeat_CollectDiscount(Color activeColor, Color disableColor, DateTime checkTime)
        {
            var clientsCollectDiscount = SessionPool.GetClientsGroup(ClientTypeEnum.Crawl);
            Dictionary<BetPlatformEnum, bool> isOkWebs = new Dictionary<BetPlatformEnum, bool>();
            isOkWebs[BetPlatformEnum.QX1688] = false;
            isOkWebs[BetPlatformEnum.QXDFV168] = false;
            isOkWebs[BetPlatformEnum.QXEG6] = false;
            isOkWebs[BetPlatformEnum.QXS6] = false;
            if (clientsCollectDiscount.Count > 0)
            {
                foreach (var item in clientsCollectDiscount)
                {
                    if (item.Value.Heartbeat >= checkTime && item.Value.platform.HasValue)
                    {
                        isOkWebs[item.Value.platform.Value] = true;
                    }
                }
            }

            if (isOkWebs[BetPlatformEnum.QX1688])
            {
              
            }
            else
            {
               
            }

            if (isOkWebs[BetPlatformEnum.QXDFV168])
            {
               
            }
            else
            {
                
            }

            if (isOkWebs[BetPlatformEnum.QXEG6])
            {
               
            }
            else
            {
               
            }

            if (isOkWebs[BetPlatformEnum.QXS6])
            {
                
            }
            else
            {               
            }
        }

        /// <summary>
        /// 检测客户端是否在线，否则移除sesion
        /// </summary>
        /// <param name="activeColor"></param>
        /// <param name="disableColor"></param>
        private void CheckHeartbeat_ClientDesk(Color activeColor, Color disableColor)
        {
            var clients = SessionPool.GetClientsGroup(ClientTypeEnum.Client);
            var sessionIds = clients.Keys.ToArray();
            int okCount = 0;
            if (sessionIds.Length > 0)
            {
                DateTime checkTime = DateTime.Now.AddSeconds(-20);
                foreach (var item in sessionIds)
                {
                    if (clients[item].Heartbeat >= checkTime)
                    {
                        okCount++;
                    }
                    else
                    {
                        // 移除控盘端资源
                        SessionControler.Disconnect(item,
                            new ValueTypeParam<ClientTypeEnum>(ClientTypeEnum.Client));
                    }
                }
            }

            if (okCount > 0)
            {
             
            }
            else
            {
               
            }
        }

        #endregion

        private void Log(string message)
        {
            try
            {
                //this.txtLog.Invoke((MethodInvoker)(delegate
                //{
                //    if (this.txtLog.TextLength > 20000)
                //    {
                //        this.txtLog.Clear();
                //    }

                //    this.txtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + message + System.Environment.NewLine);
                //    this.txtLog.ScrollToCaret();
                //}));
            }
            catch
            {
                // ignored
            }
        }

        private void btnPushMessage_Click(object sender, EventArgs e)
        {
            var  session = SessionPool.GetClientsGroup(ClientTypeEnum.Bet);
            if (session != null)
            {
                var ses = session.First();
                MQRouterSendQueue.PushBetServerClose(ses.Key);
            }
        }
    }
}
