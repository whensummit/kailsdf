using System.Configuration;

namespace SevenStarAutoSell.Common.Defs
{
    /// <summary>
    /// MQ配置信息
    /// </summary>
    public class MQConfig
    {
        public static string PublishServer
        {
            get
            {
                return ConfigurationManager.AppSettings["MQPublishServer"];
            }
        }

        public static string RouterSendServer
        {
            get
            {
                return ConfigurationManager.AppSettings["MQRouterSendServer"];
            }
        }

        public static string RouterReceiveServer
        {
            get
            {
                return ConfigurationManager.AppSettings["MQRouterReceiveServer"];
            }
        }

        public static string ResponseServer
        {
            get
            {
                return ConfigurationManager.AppSettings["MQResponseServer"];
            }
        }
    }
}
