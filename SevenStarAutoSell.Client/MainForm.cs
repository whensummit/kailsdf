using SevenStarAutoSell.Client.Common;
using SevenStarAutoSell.Common.Extensions;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Threading.Tasks;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models.Session;
using SevenStarAutoSell.Common.Utils;
using SevenStarAutoSell.Models.Defs;
using SevenStarAutoSell.Client.UserControls;
using NetMQ.Sockets;

namespace SevenStarAutoSell.Client
{
    public partial class MainForm : Form
    {
        private bool _mustClose = false;

        #region 自带事件

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Initial();
#if DEBUG1
#else
            Install();
#endif
            Install();

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!_mustClose)
            {
                if (!MessageBoxEx.Confirm("确认关闭客户端程序吗？", this))
                {
                    e.Cancel = true;
                    return;
                }
            }

            Uninstall();
        }

        #endregion

        #region 界面操作

        UcSubMenuSellPalte ucSellPalte = new UcSubMenuSellPalte();
        UcSubMenuReport ucReport = new UcSubMenuReport();
        UcMainContentQuickInput ucQuickInput = new UcMainContentQuickInput();
        UcMainContentTxtImport ucTxtImport = new UcMainContentTxtImport();
        private void Initial()
        {
            #region 出货盘子菜单

            ucSellPalte.Dock = DockStyle.Fill;
            ucSellPalte.Location = new System.Drawing.Point(0, 0);
            ucSellPalte.Name = "ucSubMenuSellPalte";
            //ucSellPalte.Size = new System.Drawing.Size(909, 59);
            ucSellPalte.TabIndex = 100;
            #endregion

            #region 出货盘子菜单

            ucReport.Dock = DockStyle.Fill;
            ucReport.Location = new System.Drawing.Point(0, 0);
            ucReport.Name = "ucSubMenuReport";
            #endregion

            #region 快打主界面

            ucQuickInput.Dock = System.Windows.Forms.DockStyle.Fill;
            ucQuickInput.Location = new System.Drawing.Point(0, 0);
            ucQuickInput.Name = "ucQuickInput";
            //ucQuickInput.Size = new System.Drawing.Size(909, 544);
            #endregion

            #region txt导入主界面

            ucTxtImport.Dock = System.Windows.Forms.DockStyle.Fill;
            ucTxtImport.Location = new System.Drawing.Point(0, 0);
            ucTxtImport.Name = "ucTxtImport";
            #endregion

            #region 点击事件赋值

            //主菜单点击事件
            btnMainMenuSellPlate.Click += btnMainMenus_Click;
            btnMainMenuControlPlate.Click += btnMainMenus_Click;
            btnMainMenuSellReport.Click += btnMainMenus_Click;
            btnMainMenuHotCode.Click += btnMainMenus_Click;
            btnMainMenuLog.Click += btnMainMenus_Click;
            btnMainMenuSysSet.Click += btnMainMenus_Click;
            //下注菜单点击事件
            linkLabel1.Click += btnMainMenus_Click;
            linkLabel2.Click += btnMainMenus_Click;
            linkLabel3.Click += btnMainMenus_Click;
            linkLabel4.Click += btnMainMenus_Click;
            #endregion
        }

        /// <summary>
        /// 主菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMainMenus_Click(object sender, EventArgs e)
        {
            var btnTemp = sender as Control;
            var mainMenuType = MainMenuType.其他;

            if (btnTemp == null)
                return;

            switch (btnTemp.Name)
            {
                case "btnMainMenuSellPlate":
                    mainMenuType = MainMenuType.出货盘;
                    break;
                case "btnMainMenuControlPlate":
                    mainMenuType = MainMenuType.控制台;
                    break;
                case "btnMainMenuSellReport":
                    mainMenuType = MainMenuType.报表统计;
                    break;
                case "btnMainMenuHotCode":
                    mainMenuType = MainMenuType.热门号码;
                    break;
                case "btnMainMenuLog":
                    mainMenuType = MainMenuType.日志;
                    break;
                case "btnMainMenuSysSet":
                    mainMenuType = MainMenuType.系统设置;
                    break;
                case "linkLabel1":
                    mainMenuType = MainMenuType.快打;
                    break;
                case "linkLabel2":
                    mainMenuType = MainMenuType.快选;
                    break;
                case "linkLabel3":
                    mainMenuType = MainMenuType.自动;
                    break;
                case "linkLabel4":
                    mainMenuType = MainMenuType.txt导入;
                    break;
                default:
                    break;
            }

            ChangeSubMenuList(mainMenuType);
        }

        private void ChangeSubMenuList(MainMenuType type /*= MainMenuType.出货盘*/)
        {
            pnlSubMenu.Controls.Clear();
            pnlMainContent.Controls.Clear();//清空内容

            switch (type)
            {
                case MainMenuType.出货盘:
                    pnlSubMenu.Controls.Add(ucSellPalte);
                    break;
                case MainMenuType.报表统计:
                    pnlSubMenu.Controls.Add(ucReport);
                    break;
                case MainMenuType.热门号码:

                    break;
                case MainMenuType.控制台:

                    break;
                case MainMenuType.日志:

                    break;
                case MainMenuType.系统设置:

                    break;
                case MainMenuType.其他:
                    break;
                case MainMenuType.快打:
                    pnlMainContent.Controls.Add(ucQuickInput);
                    break;
                case MainMenuType.快选:
                    break;
                case MainMenuType.自动:
                    break;
                case MainMenuType.txt导入:
                    pnlMainContent.Controls.Add(ucTxtImport);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 业务操作

        private void Install()
        {
            // MQ
            MQThreads.Instance.Install();
            //this.BindMqEvents();

            //ServerParameterRequest();
            //RequestResidualVolumeBetModel();

            //// 组件初始化
            //PublicData.DiscountsData = new DiscountsData();
            //PublicData.DiscountsData.BindMQEvents();

            //PublicData.TransData = new TransData();
            //PublicData.TransData.BindMqEvents();

            //PublicData.BettingService = new BettingService.BettingService();
            //PublicData.BettingService.Install();

            //todo 暂时取消心跳检测
            //Task.Factory.StartNew(ThreadHeartbeat, PublicData.CancellationToken.Token);

            //Task.Factory.StartNew(ThreadCheckWebsiteAccountOffline, PublicData.CancellationToken.Token);
            //_loadComplated = true;
        }

        private void Uninstall()
        {
            PublicData.CancellationToken.Cancel();
            MQThreads.Instance.Uninstall();
            //UnbindMqEvents();
            //if (_loadComplated)
            //{
            //    PublicData.DiscountsData.UnbindMQEvents();
            //    PublicData.TransData.UnbindMqEvents();
            //    PublicData.BettingService.Uninstall();
            //}

            try
            {
                //PublicData.RequestSocket.RequestSendReceive<MQActionVoidResult>(
                //    IdKeyData.Create(PublicData.SessionId, "Session/Disconnect",
                //        new ValueTypeParam<ClientTypeEnum>(ClientTypeEnum.ClientDesk)));
            }
            catch
            {
                //ignored
            }

            Thread.Sleep(500);
        }

        /// <summary>
        /// 发送心跳线程,保持与中心服务器连接
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
                            new HeartbeatParam() { ClientType = ClientTypeEnum.Client }));
                    if (result.IsOK)
                    {
                        heartbeatErrorCount = 0;
                    }
                    else
                    {
                        heartbeatErrorCount++;
                        LogUtil.Warn(Environment.NewLine + "控盘端心跳失败，错误信息：" + result.ErrorMsg);
                    }
                }
                catch (Exception ex)
                {
                    heartbeatErrorCount++;
                    LogUtil.Warn(Environment.NewLine + "控盘端Request发送心跳异常，错误信息：" + Environment.NewLine + ex.StackTrace);
                }

                Thread.Sleep(TimeSpan.FromSeconds(2));
            }

            if (heartbeatErrorCount >= 5)
            {
                MessageBoxEx.Alert("您已掉线，请关闭重新启动！");
            }
        }

        #endregion

        ///// <summary>
        ///// 连接中心服务器(这段代码要提到登录里面去做)
        ///// </summary>
        ///// <returns></returns>
        //private bool ConnectCenterServer()
        //{
        //    try
        //    {

        //        PublicData.RequestSocket = new RequestSocket();
        //        PublicData.RequestSocket.Connect(MQConfig.ResponseServer);

        //        MQActionResult<string> result = PublicData.RequestSocket.RequestSendReceive<MQActionResult<string>>(
        //            IdKeyData.Create("", "Session/Connect", new ValueTypeParam<ClientTypeEnum>(ClientTypeEnum.Client)));
        //        if (null != result && result.IsOK)
        //        {

        //            PublicData.SessionId = result.Data;
        //            return true;
        //        }
        //        else
        //        {
        //            LogUtil.Warn(Environment.NewLine + "测试代码");
        //            MessageBoxEx.Alert("中心服务器连接失败，信息：" + result.ErrorMsg);
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBoxEx.Alert(ex.Message);
        //        return false;
        //    }
        //}
    }

    internal enum MainMenuType
    {
        出货盘 = 1,
        报表统计 = 2,
        热门号码 = 4,
        控制台 = 8,
        日志 = 16,
        系统设置 = 32,
        快打 = 64,
        快选 = 128,
        自动 = 256,
        txt导入 = 512,
        其他 = 4048,
    }
}
