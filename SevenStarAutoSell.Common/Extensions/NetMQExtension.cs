using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using SevenStarAutoSell.Common.Utils;
using SevenStarAutoSell.Common.Defs;
using NetMQ.Sockets;

namespace SevenStarAutoSell.Common.Extensions
{
    /// <summary>
    /// MQ扩展方法（统一各端收发数据的格式）
    /// </summary>
    public static class NetMQExtension
    {
        public static void PublisherSend(this PublisherSocket socket, KeyData kd)
        {
            socket.SendMoreFrame(kd.Key).SendFrame(kd.Data);
        }

        public static KeyData SubscriberReceive(this SubscriberSocket socket)
        {
            KeyData kd = new KeyData();

            NetMQMessage msg = socket.ReceiveMultipartMessage();
            kd.Key = msg[0].ConvertToString();
            kd.Data = msg[1].Buffer;

            return kd;
        }


        public static void DealerSend(this DealerSocket socket, KeyData kd)
        {
            byte[] zipData = GZipUtil.Compress(kd.Data);
            socket.SendMoreFrameEmpty().SendMoreFrame(kd.Key).SendFrame(zipData);
        }

        public static KeyData DealerReceive(this DealerSocket socket)
        {
            KeyData kd = new KeyData();

            NetMQMessage msg = socket.ReceiveMultipartMessage();
            kd.Key = msg[1].ConvertToString();
            kd.Data = msg[2].Buffer;

            return kd;
        }

        public static IdKeyData RouterReceive(this RouterSocket socket)
        {
            IdKeyData ikd = new IdKeyData();
            NetMQMessage msg = socket.ReceiveMultipartMessage();

            ikd.Id = msg[0].ConvertToString(); //socket.Options.Identity
            ikd.Key = msg[2].ConvertToString();
            byte[] zipData = msg[3].Buffer;
            ikd.Data = GZipUtil.Decompress(zipData);

            return ikd;
        }

        public static void RouterSend(this RouterSocket socket, IdKeyData ikd)
        {
            socket.SendMoreFrame(ikd.Id).SendMoreFrameEmpty()
                .SendMoreFrame(ikd.Key).SendFrame(ikd.Data);
            ;
        }


        public static T RequestSendReceive<T>(this RequestSocket socket, IdKeyData ikd)
        {
            try
            {
                lock (socket)
                {
                    socket.SendMoreFrame(ikd.Id).SendMoreFrame(ikd.Key).SendFrame(ikd.Data);

                    NetMQMessage msg = null;
#if DEBUG1
                    msg = socket.ReceiveMultipartMessage();  // debug模式打开，不会超时
#else
                    socket.TryReceiveMultipartMessage(TimeSpan.FromSeconds(10), ref msg);
#endif
                    if (msg != null)
                    {
                        var result = JsonUtil.Deserialize<T>(msg[0].Buffer);
                        return result;
                    }
                    else
                    {
                        throw new Exception("中心服务器请求失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                socket = new RequestSocket();
                socket.Connect(MQConfig.ResponseServer);

                LogUtil.Warn("RequestSendReceive 异常：" + ex.StackTrace);
                return default(T);
            }
        }

        public static IdKeyData ResponseReceive(this ResponseSocket socket)
        {
            IdKeyData kd = new IdKeyData();

            NetMQMessage msg = socket.ReceiveMultipartMessage();
            kd.Id = msg[0].ConvertToString();
            kd.Key = msg[1].ConvertToString();
            kd.Data = msg[2].Buffer;

            return kd;
        }

        public static void ResonseSend(this ResponseSocket socket, string jsonData)
        {
            socket.SendFrame(Encoding.UTF8.GetBytes(jsonData));
        }

    }
}
