using SevenStarAutoSell.Models.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models.Session
{
    public class Session
    {
        public Session(string clientId, ClientTypeEnum clientType)
        {
            this.ClientId = clientId;
            this.ClientType = clientType;
            Heartbeat = DateTime.Now;
        }

        #region Session核心

        public string ClientId { get; }

        public ClientTypeEnum ClientType { get; }

        /// <summary>
        /// 最后心跳时间
        /// </summary>
        public DateTime Heartbeat { get; set; }

        /// <summary>
        /// 如果是折扣端，才有该类型
        /// </summary>
        public BetPlatformEnum? platform { get; set; }

        #endregion;


        #region 当前已登陆会员信息

        public int SysmemberId { get; set; }

        public string SysmemberUsername { get; set; }

        /// <summary>
        /// 会员已登陆
        /// </summary>
        public bool IsLoggedIn
        {
            get { return SysmemberId > 0 && !string.IsNullOrEmpty(SysmemberUsername); }
        }

        /// <summary>
        /// 服务端Id（控盘端Session才有）
        /// </summary>
        public string BetServerId { get; set; }

        #endregion

        /// <summary>
        /// 控盘端Id（仅投注服务端Session才有）
        /// </summary>
        public string ClientDeskId { get; set; }

    }
}
