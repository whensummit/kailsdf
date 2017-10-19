using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenStarAutoSell.Common.Extensions;
using System.Text.RegularExpressions;
using SevenStarAutoSell.Client.Common;
using NetMQ.Sockets;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Models.Collect;

namespace SevenStarAutoSell.Client.UserControls
{
    public partial class UcMainContentQuickInput : UserControl
    {
        List<OddsEntity> oddsList = new List<OddsEntity>();

        public UcMainContentQuickInput()
        {
            InitializeComponent();
            MQThreads.Instance.PushNewOddsEvent += BetNoOddsDataBind;//绑定赔率获取回调事件

            //OddsData.Columns.AddRange();
        }

        private void btnDirectSell_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Confirm("确认要直接出货吗？", this))
            {
                for (int i = 0; i < 5; i++)
                {
                    BetNoOddsDataBind(null);
                }  
                
                //出货
            }

            dgvOddsList.DataSource = oddsList;
        }

        //private void txtBetNo_KeyUp(object sender, KeyEventArgs e)
        //{
        //    txtBetNo_TextChanged(null, null);
        //}

        private void txtBetNo_TextChanged(object sender, EventArgs e)
        {
            var betNo = txtBetNo.Text;
            if (ValidBetNo(betNo))
            {
                SendScanWaterToCenterServer(betNo);
            }
        }


        Regex _regex = new Regex(@"^[1-9_xX]{4}$");
        private bool ValidBetNo(string betNo)
        {
            if (betNo == null || betNo.Length != 4)//目前只考虑4位号码
            {
                return false;
            }
            if (betNo.Replace("X", "").Replace("x", "").Length < 2)
            {
                return false;
            }

            Match m = _regex.Match(betNo);
            return m.Success;
        }

        /// <summary>
        /// 开始执行扫水操作
        /// </summary>
        /// <param name="betNo"></param>
        private void SendScanWaterToCenterServer(string betNo)
        {
            //扫水
            //PublicData.RequestSocket = new RequestSocket();
            //PublicData.RequestSocket.Connect(MQConfig.ResponseServer);

            var data = new Collect() { Number = betNo, ClientSessionID = PublicData.SessionId, };
            var result = PublicData.RequestSocket.RequestSendReceive<MQActionResult<string>>(
                    IdKeyData.Create(PublicData.SessionId, "Collect/SendCollect", data));

            if (result == null)
            {
                MessageBoxEx.Alert("连接服务器失败，请检查网络或联系管理员！");
                return;
            }
            else if (result.IsOK)
            {
                //命令发送成功
            }
        }

        private void BetNoOddsDataBind(CollectResult item)
        {
            if (item != null)
            {
                oddsList.Add(new OddsEntity(item.Id, item.Platform.ToString(), (decimal)item.Odds, (decimal)item.MaxBetMoney));
            }
            else
            {
                oddsList.Add(new OddsEntity(1, "公司", 965, 9500));
            }

        }

        private void dgvOddsList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

    class OddsEntity
    {
        public OddsEntity(int id, string companyName, decimal odds, decimal canBetMoney)
        {
            Id = id;
            CompanyName = companyName;
            Odds = odds;
            CanBetMoney = canBetMoney;
        }
        public int Id { set; get; }
        public string CompanyName { set; get; }
        public decimal Odds { set; get; }
        public decimal CanBetMoney { set; get; }
    }
}
