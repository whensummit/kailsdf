using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Common.Defs
{
    /// <summary>
    /// MQ请求返回值消息类型
    /// <para>T 如果是object类型，则表示void</para>
    /// </summary>
    /// <typeparam name="T">Class</typeparam>
    public class MQActionResult<T>
    {
        public bool IsOK { get; set; }

        public string ErrorMsg { get; set; }

        /// <summary>
        /// 对象类型返回值
        /// </summary>
        public T Data { get; set; }

        public MQActionResult()
        {
            this.ErrorMsg = "";
            this.Data = default(T);
        }

        public static MQActionResult<T> Ok(T data)
        {
            return new MQActionResult<T>()
            {
                IsOK = true,
                Data = data
            };
        }

        public static MQActionResult<T> Error(string message)
        {
            return new MQActionResult<T>()
            {
                IsOK = false,
                ErrorMsg = message
            };
        }
    }


    /// <summary>
    /// Action空返回值
    /// </summary>
    public class MQActionVoidResult : MQActionResult<string>
    {
        public static MQActionVoidResult Ok()
        {
            return new MQActionVoidResult()
            {
                IsOK = true
            };
        }

        public static new MQActionVoidResult Error(string message)
        {
            return new MQActionVoidResult()
            {
                IsOK = false,
                ErrorMsg = message
            };
        }
    }
}
