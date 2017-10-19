using NetMQ;
using NetMQ.Sockets;
using SevenStarAutoSell.CenterServer.Shell.App_Start;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SevenStarAutoSell.CenterServer.Shell.Common
{
    /// <summary>
    /// MQ线程异常处理方法
    /// <para>中心服务端只用MQ的3种方法：Response, RouterSend，Publish</para>
    /// <para>各个客户端只用MQ的3种方法：Request, DealerReceive，Subscribe</para>
    /// </summary>
    /// <param name="ex"></param>
    internal delegate void MQThreadExceptionHandler(Exception ex);

    /// <summary>
    /// 统一处理MQ的各种队列发送收发方式
    /// </summary>
    internal class MQThreads
    {
        public MQThreadExceptionHandler MQThreadExceptionHandler { get; set; }


        private CancellationTokenSource _cancellationTokenSource;

        public MQThreads(CancellationTokenSource cancellationTokenSource)
        {
            this._cancellationTokenSource = cancellationTokenSource;
        }

        public void ThreadMQPublishServer()
        {
            try
            {
                using (PublisherSocket publishSocket = new PublisherSocket())
                {
                    publishSocket.Bind(MQConfig.PublishServer);

                    try
                    {
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            KeyData item;
                            if (MQPublishQueue.TryTake(out item))
                            {
                                publishSocket.PublisherSend(item);
                            }
                        }
                    }
                    catch (TerminatingException)
                    {
                    }
                    catch (Exception ex)
                    {
                        MQThreadExceptionHandler?.Invoke(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                MQThreadExceptionHandler?.Invoke(ex);
            }
        }

        public void ThreadMQRouterSend()
        {
            try
            {
                using (RouterSocket routerSocket = new RouterSocket())
                {
                    routerSocket.Bind(MQConfig.RouterSendServer);

                    while (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        try
                        {
                            IdKeyData item;
                            if (MQRouterSendQueue.TryTake(out item))
                            {
                                routerSocket.RouterSend(item);
                            }
                        }
                        catch (TerminatingException)
                        {
                        }
                        catch (Exception ex)
                        {
                            MQThreadExceptionHandler?.Invoke(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MQThreadExceptionHandler?.Invoke(ex);
            }
        }

        /// <summary>
        /// Router模式，会自动路由到Controler
        /// </summary>
        public void ThreadMQRouterReceive()
        {
            try
            {
                using (RouterSocket routerSocket = new RouterSocket())
                {
                    routerSocket.Bind(MQConfig.RouterReceiveServer);

                    while (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        try
                        {
                            IdKeyData receiveData = routerSocket.RouterReceive();
                            Task.Factory.StartNew(() =>
                            {
                                RouteConfig.Instance.ExecCmd(receiveData.Id, receiveData.Key, receiveData.Data);
                            });
                        }
                        catch (TerminatingException)
                        {
                        }
                        catch (Exception ex)
                        {
                            MQThreadExceptionHandler?.Invoke(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MQThreadExceptionHandler?.Invoke(ex);
            }
        }

        /// <summary>
        /// Response模式，会自动路由到Controler
        /// </summary>
        public void ThreadMQResponse()
        {
            try
            {
                using (ResponseSocket responseSocket = new ResponseSocket())
                {
                    responseSocket.Bind(MQConfig.ResponseServer);

                    while (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        try
                        {
                            IdKeyData receiveData = responseSocket.ResponseReceive();

                            // request模式也需要异步处理，因为有很多个系统用户，会产生并发
                            Task.Factory.StartNew(() =>
                            {
                                string result = RouteConfig.Instance.ExecCmd(receiveData.Id, receiveData.Key, receiveData.Data);
                                responseSocket.ResonseSend(result);
                            });
                        }
                        catch (TerminatingException)
                        {
                        }
                        catch (Exception ex)
                        {
                            MQThreadExceptionHandler?.Invoke(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MQThreadExceptionHandler?.Invoke(ex);
            }
        }
    }
}
