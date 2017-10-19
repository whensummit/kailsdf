using System;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace SevenStarAutoSell.Common.Utils
{
    public static class LogUtil
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LogUtil));

        public static void Debug(string message)
        {
            try
            {
                ConsoleWrite(message);
                Log.Debug(message);
            }
            catch
            {
                // ignored
            }
        }

        public static void Info(string message)
        {
            try
            {
                ConsoleWrite(message);
                Log.Info(message);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 警告信息（一般只发送给项目负责人或者测试负责人。 不是立马要处理的问题）
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            try
            {
                ConsoleWrite(message);
                Log.Warn(message);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 未知的错误信息（一般发送给项目负责人，IT，测试，技术总监。 是立马要处理的问题）
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            try
            {
                ConsoleWrite(message);
                Log.Error(message);
            }
            catch
            {
                // ignored
            }
        }

        public static void Error(Exception ex)
        {
            try
            {
                LogUtil.Error($"引发异常的方法：{ex.TargetSite}; \n异常消息：{ex.Message}; \n异常堆栈信息：{ex.StackTrace}");
            }
            catch
            {
                // ignored
            }
        }

        public static void ConsoleWrite(string message)
        {
            System.Diagnostics.Trace.WriteLine($"{System.DateTime.Now} \t{message}");
        }
    }
}
